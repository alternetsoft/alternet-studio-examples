using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.Wpf.SystemDialogs;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.FormDesigner.Wpf.Outline;
using Alternet.FormDesigner.Wpf.Roslyn;
using Alternet.Scripter;
using Microsoft.Win32;

namespace FormDesigner.InMemory.Wpf
{
    public partial class MainWindow : Window
    {
        private const string XamlExtension = ".xaml";

        private Dictionary<string, TabItem> codeOrXamlTabPages = new Dictionary<string, TabItem>();
        private Dictionary<string, TabItem> designerTabPages = new Dictionary<string, TabItem>();
        private Dictionary<string, MemoryFormDesignerDataSource> sourcesByNames = new Dictionary<string, MemoryFormDesignerDataSource>();
        private HashSet<string> editedXamlSourceNames = new HashSet<string>();
        private bool updating;
        private ScriptRun scriptRun;

        public MainWindow()
        {
            InitializeComponent();

            var dir = GetTestFilesDirectoryPath();
            OpenAllFormFiles(
                "MainWindowCS",
                File.ReadAllText(Path.Combine(dir, @"CS\MainWindowCS.xaml")),
                File.ReadAllText(Path.Combine(dir, @"CS\MainWindowCS.xaml.cs")),
                "HelloWorld.Wpf.MainWindowCS",
                LanguageServices.GetSupportedLanguage(SupportedLanguages.CSharp));
        }

        public IFormDesignerControl ActiveDesigner
        {
            get
            {
                var item = documentsTabControl.SelectedItem as TabItem;
                if (item == null)
                    return null;

                return item.Content as IFormDesignerControl;
            }
        }

        public IScriptEdit ActiveEditor
        {
            get
            {
                var item = documentsTabControl.SelectedItem as TabItem;
                if (item == null)
                    return null;

                return item.Content as IScriptEdit;
            }
        }

#pragma warning disable SA1314 // Type parameter names should begin with T
        public static void RemoveAllByValue<K, V>(Dictionary<K, V> dictionary, V value)
#pragma warning restore SA1314 // Type parameter names should begin with T
        {
            foreach (var key in dictionary.Where(
                kvp => EqualityComparer<V>.Default.Equals(kvp.Value, value)).Select(x => x.Key).ToArray())
                dictionary.Remove(key);
        }

        private string GetTestFilesDirectoryPath()
        {
            const string Subdirectory = @"Resources\Designer\Wpf";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, Subdirectory);

            if (!Directory.Exists(directory))
                directory = Path.Combine(Path.GetFullPath(baseDirectory.TrimEnd('\\') + @"\..\..\..\..\..\..\"), Subdirectory);

            return directory;
        }

        private TabItem AddDocumentTab(string tabName, object content, TabInfo tabInfo)
        {
            var tabItem = new TabItem
            {
                Content = content,
                Header = tabName,
                Tag = tabInfo,
            };

            documentsTabControl.Items.Add(tabItem);
            documentsTabControl.SelectedItem = tabItem;

            return tabItem;
        }

        private void DocumentsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var designer = ActiveDesigner;

            propertyGrid.FormDesigner = designer;
            toolbox.FormDesigner = designer;
            outline.FormDesigner = designer;

            UpdateOptions();

            if (designer != null)
                ReloadDesignerIfNeeded(ActiveDesigner);
        }

        private MemoryFormDesignerDataSource GetDesignerSource(string name)
        {
            return sourcesByNames[name];
        }

        private MemoryFormDesignerDataSource CreateDesignerSource(
            string name,
            string xamlText,
            string userCodeText,
            string designedClassName,
            Language language)
        {
            var ds = new MemoryFormDesignerDataSource(
                name,
                xamlText,
                userCodeText,
                designedClassName,
                language,
                text =>
                {
                    var source = new FormDesignerTextSource();
                    using (var stream = GetTextStream(text))
                        source.LoadStream(stream);
                    return source;
                });

            sourcesByNames.Add(name, ds);

            return ds;
        }

        private Stream GetTextStream(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text), writable: false);
        }

        private void Run_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            RunScript();
        }

        private void ResetToolbox_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            toolbox.Reset();
        }

        private void RasterPlacement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            updating = true;
            try
            {
                var activeDesigner = ActiveDesigner as FormDesignerControl;
                if (activeDesigner != null)
                {
                    activeDesigner.DesignPanel.UseRasterPlacement = chbUseRasterPlacement.IsChecked;
                }
            }
            finally
            {
                updating = false;
            }
        }

        private void SnaplinePlacement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            updating = true;
            try
            {
                var activeDesigner = ActiveDesigner as FormDesignerControl;
                if (activeDesigner != null)
                {
                    activeDesigner.DesignPanel.UseSnaplinePlacement = chbUseSnaplinePlacement.IsChecked;
                }
            }
            finally
            {
                updating = false;
            }
        }

        private void LoadToolboxFromFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return;

            using (var fs = new FileStream(dialog.FileName, FileMode.Open))
            {
                try
                {
                    toolbox.Load(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SaveToolboxToFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return;

            using (var fs = new FileStream(dialog.FileName, FileMode.Create))
                toolbox.Save(fs);
        }

        private void AddControlsFromAssembly_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";

            var result = dialog.ShowDialog();
            if (result == null || !result.Value)
                return;

            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load assembly: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                toolbox.AddItemsFromAssembly("Additional Controls", assembly);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add toolbox items: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private RunnableScriptFiles SaveActiveSourceFilesForRunning(string tempDirectoryPath)
        {
            var selectedItem = documentsTabControl.SelectedItem as TabItem;
            if (selectedItem == null)
                return null;

            var tabInfo = (TabInfo)selectedItem.Tag;
            var sourceName = tabInfo.SourceName;

            var source = sourcesByNames[sourceName];
            return SaveDesignerSourceToDirectory(source, tempDirectoryPath);
        }

        private RunnableScriptFiles SaveDesignerSourceToDirectory(MemoryFormDesignerDataSource source, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var codeFilePath = Path.Combine(directoryPath, source.Name + ".xaml" + GetExtensionFromLanguage(source.UserCodeLanguage));
            var xamlFilePath = Path.Combine(directoryPath, source.Name + ".xaml");

            using (var stream = new FileStream(codeFilePath, FileMode.Create))
                source.UserCodeTextSource.SaveStream(stream);
            using (var stream = new FileStream(xamlFilePath, FileMode.Create))
                source.XamlTextSource.SaveStream(stream);

            return new RunnableScriptFiles(codeFilePath, xamlFilePath);
        }

        private void RunScript()
        {
            try
            {
                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tempDirectoryPath);
                var runnableScriptFiles = SaveActiveSourceFilesForRunning(tempDirectoryPath);
                if (runnableScriptFiles != null)
                {
                    RunScriptWithScripter(runnableScriptFiles, tempDirectoryPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error running form: " + e);
            }
        }

        private void RunScriptWithScripter(RunnableScriptFiles runnableScriptFiles, string tempDirectoryPath)
        {
            FormDesignerDataSource tempSource;
            var language = FormFilesUtility.FindUserCodeFile(runnableScriptFiles.XamlFilePath, out tempSource);

            var designer = ActiveDesigner;
            if (designer == null)
                return;

            InitializeScripter();
            if (SetScriptSource(tempSource, designer.ReferencedAssemblies, tempDirectoryPath))
                scriptRun.RunProcess();
        }

        private bool SetScriptSource(FormDesignerDataSource source, DesignerReferencedAssemblies referencedAssemblies, string tempDirectoryPath)
        {
            string userCodeFileName = source.UserCodeFileName;

            var activeEditor = ActiveEditor;
            if (userCodeFileName != null && File.Exists(userCodeFileName))
            {
                scriptRun.ScriptSource.FromScriptFile(userCodeFileName);
                scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.Wpf);
                scriptRun.ScriptSource.WpfResources.AddRange(SaveInMemoryImageToFile(tempDirectoryPath));

                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N") + ".exe";

                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(
                            scriptRun.ScriptHost.CompilerErrors.Where(
                                x => x.Kind == ScriptCompilationDiagnosticKind.Error).First().ToString());
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void InitializeScripter()
        {
            if (scriptRun == null)
            {
                scriptRun = new ScriptRun();
                scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            }
        }

        private string[] SaveInMemoryImageToFile(string tempDirectoryPath)
        {
            var image = MemoryResourceResolutionService.GetOrCreateImage();
            var imagePath = Path.Combine(tempDirectoryPath, "1.png");
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
                return new[] { imagePath };
            }
        }

        private void Close_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var selectedItem = documentsTabControl.SelectedItem as TabItem;
            if (selectedItem == null)
                return;

            var tabInfo = (TabInfo)selectedItem.Tag;
            var edit = selectedItem.Content as IScriptEdit;
            if (edit != null)
                DisconnectEditFromDesignerSource(edit, tabInfo);

            RemoveAllByValue(codeOrXamlTabPages, selectedItem);
            RemoveAllByValue(designerTabPages, selectedItem);

            var name = tabInfo.SourceName;

            documentsTabControl.Items.Remove(selectedItem);
        }

        private void DisconnectEditFromDesignerSource(IScriptEdit edit, TabInfo tabInfo)
        {
            var source = GetDesignerSource(tabInfo.SourceName);

            if (tabInfo.Type == TabInfo.ContentType.Xaml)
                ((FormDesignerTextSource)source.XamlTextSource).ActiveEdit = null;
            else
                ((FormDesignerTextSource)source.UserCodeTextSource).ActiveEdit = null;
        }

        private void Undo_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanUndo)
                ActiveDesigner.DesignerCommands.Undo();
        }

        private void Redo_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;
            if (ActiveDesigner.DesignerCommands.CanRedo)
                ActiveDesigner.DesignerCommands.Redo();
        }

        private void Cut_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanCut)
                ActiveDesigner.DesignerCommands.Cut();
        }

        private void Copy_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanCopy)
                ActiveDesigner.DesignerCommands.Copy();
        }

        private void Paste_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanPaste)
                ActiveDesigner.DesignerCommands.Paste();
        }

        private void Delete_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanDelete)
                ActiveDesigner.DesignerCommands.Delete();
        }

        private void SelectAll_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanSelectAll)
                ActiveDesigner.DesignerCommands.SelectAll();
        }

        private void BringToFront_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanBringToFont)
                ActiveDesigner.DesignerCommands.BringToFront();
        }

        private void SendToBack_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanSendToBack)
                ActiveDesigner.DesignerCommands.SendToBack();
        }

        private void LockControls_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;
        }

        private void AlignLeft_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignLeft)
                ActiveDesigner.DesignerCommands.AlignLeft();
        }

        private void AlignCenter_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignCenter)
                ActiveDesigner.DesignerCommands.AlignCenter();
        }

        private void AlignRight_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignRight)
                ActiveDesigner.DesignerCommands.AlignRight();
        }

        private void AlignTop_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignTop)
                ActiveDesigner.DesignerCommands.AlignTop();
        }

        private void AlignMiddle_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignMiddle)
                ActiveDesigner.DesignerCommands.AlignMiddle();
        }

        private void AlignBottom_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanAlignBottom)
                ActiveDesigner.DesignerCommands.AlignBottom();
        }

        private void StretchToSameWidth_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanStretchToSameWidth)
                ActiveDesigner.DesignerCommands.StretchToSameWidth();
        }

        private void StretchToSameHeight_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            if (ActiveDesigner.DesignerCommands.CanStretchToSameHeight)
                ActiveDesigner.DesignerCommands.StretchToSameHeight();
        }

        private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanUndo;
        }

        private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanRedo;
        }

        private void CanCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanCut;
        }

        private void CanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanCopy;
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanPaste;
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanDelete;
        }

        private void CanSelectAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanSelectAll;
        }

        private void CanAlignLeft(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignLeft;
        }

        private void CanAlignCenter(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignCenter;
        }

        private void CanAlignRight(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignRight;
        }

        private void CanAlignTop(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignTop;
        }

        private void CanAlignMiddle(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignMiddle;
        }

        private void CanAlignBottom(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanAlignBottom;
        }

        private void CanBringToFont(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanBringToFont;
        }

        private void CanSendToBack(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanSendToBack;
        }

        private void CanStretchToSameHeight(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanStretchToSameHeight;
        }

        private void CanStretchToSameWidth(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanStretchToSameWidth;
        }

        private void CanLockControls(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (ActiveDesigner == null)
                return;
        }

        private void Open_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XAML Files|*.xaml|All Files|*.*",
                Multiselect = false,
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            var result = dialog.ShowDialog(this);

            if (result == null || !result.Value)
                return;

            var xamlFileName = dialog.FileName;

            FormDesignerDataSource tempSource;
            var language = FormFilesUtility.TryFindUserCodeFile(xamlFileName, out tempSource);

            if (language == null)
            {
                MessageBox.Show("User code file not found.");
                return;
            }

            var formName = Path.GetFileNameWithoutExtension(xamlFileName);
            OpenAllFormFiles(formName, File.ReadAllText(xamlFileName), File.ReadAllText(tempSource.UserCodeFileName), tempSource.DesignedClassFullName, language);
        }

        private void OpenAllFormFiles(string name, string xamlText, string userCodeText, string designedClassName, Language language)
        {
            if (sourcesByNames.ContainsKey(name))
                return;

            var source = CreateDesignerSource(name, xamlText, userCodeText, designedClassName, language);

            OpenCode(name, language);
            OpenXaml(name);
            OpenDesigner(name);
        }

        private void OpenCode(string sourceName, Language language)
        {
            var tabName = "Code: " + sourceName;
            if (codeOrXamlTabPages.ContainsKey(tabName))
            {
                documentsTabControl.SelectedItem = codeOrXamlTabPages[tabName];
                return;
            }

            var editor = new ScriptCodeEdit();
            editor.AllowDrop = true;
            editor.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(Editor_DragOver), true);
            editor.AddHandler(RichTextBox.DropEvent, new DragEventHandler(Editor_Drop), true);

            var source = GetDesignerSource(sourceName);
            editor.InitSyntax();

            MemoryFormDesignerEditorHelpers.SetEditorSource(editor, ContentType.UserCode, source);

            editor.FileName = sourceName + GetExtensionFromLanguage(language);

            editor.RegisterAssemblies(GetReferencedAssemblies());

            codeOrXamlTabPages[tabName] = AddDocumentTab(tabName, editor, new TabInfo(TabInfo.ContentType.Code, sourceName));
        }

        private string GetExtensionFromLanguage(Language language)
        {
            return language.Extension;
        }

        private void Editor_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = false;
        }

        private void Editor_Drop(object sender, DragEventArgs e)
        {
            var tool = e.Data.GetData(typeof(CreateComponentTool)) as CreateComponentTool;
            if (tool != null)
                MessageBox.Show("Toolbox item dropped: " + tool.ComponentType.Name);
        }

        private void OpenDesigner(string sourceName)
        {
            var tabName = "Designer: " + sourceName;
            if (designerTabPages.ContainsKey(tabName))
            {
                documentsTabControl.SelectedItem = designerTabPages[tabName];
                return;
            }

            var designer = new MemoryFormDesignerControl();
            designer.DesignPanel.PropertyChanged += DesignPanel_PropertyChanged;
            designer.Source = GetDesignerSource(sourceName);
            designer.NavigateToUserMethodRequested += Designer_NavigateToUserMethodRequested;

            designerTabPages[tabName] = AddDocumentTab(tabName, designer, new TabInfo(TabInfo.ContentType.Designer, sourceName));
        }

        private void UpdateOptions()
        {
            var activeDesigner = ActiveDesigner as FormDesignerControl;
            if (activeDesigner != null)
            {
                chbUseRasterPlacement.IsChecked = activeDesigner.DesignPanel.UseRasterPlacement;
                chbUseSnaplinePlacement.IsChecked = activeDesigner.DesignPanel.UseSnaplinePlacement;
            }
        }

        private void DesignPanel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (updating)
                return;

            if (sender is DesignPanel)
            {
                DesignPanel panel = sender as DesignPanel;
                switch (e.PropertyName)
                {
                    case "UseRasterPlacement":
                        chbUseRasterPlacement.IsChecked = panel.UseRasterPlacement;
                        break;
                    case "UseSnaplinePlacement":
                        chbUseSnaplinePlacement.IsChecked = panel.UseSnaplinePlacement;
                        break;
                }
            }
        }

        private void Designer_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            if (designer != ActiveDesigner)
                return;

            var source = (MemoryFormDesignerDataSource)designer.Source;

            OpenCode(source.Name, source.UserCodeLanguage);
            SetCaretToMethod(source.UserCodeLanguage, e.MethodName);

            Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(() =>
                    {
                        var editor = (Control)ActiveEditor;
                        if (editor != null && editor.Focusable)
                        {
                            editor.Focus();
                            Keyboard.Focus(editor);
                        }
                    }));
        }

        private void SetCaretToMethod(Language language, string methodName)
        {
            string toFind;
            var cs = language.Name == SupportedLanguages.CSharp;
            if (cs)
                toFind = "void " + methodName;
            else
                toFind = "Sub " + methodName;

            var editor = ActiveEditor;

            System.Drawing.Point oldPosition = editor.Position;

            editor.Position = new System.Drawing.Point();
            if (editor.Find(toFind))
            {
                editor.MoveToLine(ActiveEditor.Position.Y + (cs ? 2 : 1));
                editor.MoveLineEnd();
            }
            else
                editor.Position = oldPosition;
        }

        private void ReloadDesignerIfNeeded(IFormDesignerControl designer)
        {
            try
            {
                var name = ((MemoryFormDesignerDataSource)designer.Source).Name;
                if (this.editedXamlSourceNames.Contains(name))
                {
                    designer.Reload();
                    editedXamlSourceNames.Remove(name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error");
            }
        }

        private void TestDesignerCreationWithDefaultSource()
        {
            var designer = new MemoryFormDesignerControl();
            AddDocumentTab(null, designer, null);
        }

        private void OpenDesigner_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XAML Files|*.xaml|All Files|*.*",
                Multiselect = false,
            };

            var result = dialog.ShowDialog(this);

            if (result == null || !result.Value)
                return;

            OpenDesigner(dialog.FileName);
        }

        private void OpenXaml(string sourceName)
        {
            var tabName = "XAML: " + sourceName;
            if (codeOrXamlTabPages.ContainsKey(tabName))
            {
                documentsTabControl.SelectedItem = codeOrXamlTabPages[tabName];
                return;
            }

            var editor = new ScriptCodeEdit();
            (editor as ScriptCodeEdit).LineModificatorsVisible = true;

            editor.AllowDrop = true;
            editor.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(Editor_DragOver), true);
            editor.AddHandler(RichTextBox.DropEvent, new DragEventHandler(Editor_Drop), true);

            var source = GetDesignerSource(sourceName);

            MemoryFormDesignerEditorHelpers.SetEditorSource(editor as ScriptCodeEdit, ContentType.Xaml, source);
            editor.InitSyntax();
            editor.FileName = "test.xaml";

            editor.TextChanged += (o, e) =>
            {
                if (documentsTabControl.SelectedContent == editor)
                    editedXamlSourceNames.Add(sourceName);
            };

            codeOrXamlTabPages[tabName] = AddDocumentTab(tabName, editor, new TabInfo(TabInfo.ContentType.Xaml, sourceName));
        }

        private string[] GetReferencedAssemblies()
        {
            return new string[] { "mscorlib", "System", "PresentationCore", "PresentationFramework", "System.Drawing", "WindowsBase", "Microsoft.VisualBasic" };
        }

        private void Save_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            var dialog = new VistaFolderBrowserDialog();
            var result = dialog.ShowDialog(this);
            if (result == null || !result.Value)
                return;

            SaveDesignerSourceToDirectory((MemoryFormDesignerDataSource)ActiveDesigner.Source, dialog.SelectedPath);
        }

        internal class RunnableScriptFiles
        {
            public RunnableScriptFiles(string codeFilePath, string xamlFilePath)
            {
                CodeFilePath = codeFilePath;
                XamlFilePath = xamlFilePath;
            }

            public string CodeFilePath { get; private set; }

            public string XamlFilePath { get; private set; }
        }

        internal class TabInfo
        {
            public TabInfo(ContentType type, string sourceName)
            {
                Type = type;
                SourceName = sourceName;
            }

            public enum ContentType
            {
                Xaml,
                Code,
                Designer,
            }

            public ContentType Type { get; set; }

            public string SourceName { get; set; }
        }
    }
}