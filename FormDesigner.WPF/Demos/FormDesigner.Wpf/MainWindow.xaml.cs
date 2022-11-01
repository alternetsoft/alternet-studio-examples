#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;
using Microsoft.Win32;

namespace FormDesigner.Wpf
{
    public partial class MainWindow : Window
    {
        private const string XamlExtension = ".xaml";

        private Dictionary<string, TabItem> codeOrXamlTabPages = new Dictionary<string, TabItem>();
        private Dictionary<string, TabItem> designerTabPages = new Dictionary<string, TabItem>();

        private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();

        private HashSet<string> editedXamlFiles = new HashSet<string>();

        private bool updating;

        private ScriptRun scriptRun;

        public MainWindow()
        {
            InitializeComponent();

            filesControl.Initialize(GetTestFilesDirectoryPath(), OnFileOpenRequested);

            var foundXamlFiles = filesControl.RefreshFiles();
            if (foundXamlFiles.Any())
                OpenAllFormFiles(foundXamlFiles.First());
            UpdateEditorButtons();
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

        private void OnFileOpenRequested(string formId, string fileName, FormOpenMode openMode)
        {
            switch (openMode)
            {
                case FormOpenMode.Design:
                    OpenDesigner(fileName);
                    break;

                case FormOpenMode.Xaml:
                    OpenXaml(fileName);
                    break;

                case FormOpenMode.Code:
                    OpenCode(fileName);
                    break;

                default:
                    throw Check.GetEnumValueNotSupportedException(openMode);
            }
        }

        private TabItem AddDocumentTab(string filePath, object content)
        {
            var tabItem = new TabItem
            {
                Content = content,
                Header = Path.GetFileName(filePath),
            };

            documentsTabControl.Items.Add(tabItem);
            documentsTabControl.SelectedItem = tabItem;

            return tabItem;
        }

        private void DocumentsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var designer = ActiveDesigner;

            propertyGrid.FormDesigner = designer;
            outline.FormDesigner = designer;
            toolbox.FormDesigner = designer;
            UpdateOptions();
            UpdateEditorButtons();
            UpdateDesignerButtons();

            if (designer != null)
                ReloadDesignerIfNeeded(ActiveDesigner);
        }

        private EditorFormDesignerDataSource GetDesignerSource(string xamlFileName)
        {
            EditorFormDesignerDataSource ds;
            if (!sourcesByFormId.TryGetValue(xamlFileName, out ds))
            {
                var language = FormFilesUtility.FindUserCodeFile(xamlFileName);

                ds = new EditorFormDesignerDataSource(
                    xamlFileName,
                    language,
                    fileName =>
                    {
                        var source = new FormDesignerTextSource();
                        source.LoadFile(fileName);
                        return source;
                    });

                sourcesByFormId.Add(xamlFileName, ds);
            }

            return ds;
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

        private void InitializeScripter()
        {
            if (scriptRun == null)
            {
                scriptRun = new ScriptRun();
                scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            }
        }

        private EditorFormDesignerDataSource GetActiveDesignerSource()
        {
            var designer = ActiveDesigner;
            if (designer != null)
                return (EditorFormDesignerDataSource)designer.Source;

            var editor = ActiveEditor;
            if (editor != null)
            {
                var fileName = editor.FileName;
                var trimmed = Path.ChangeExtension(fileName, string.Empty).TrimEnd('.');
                var extension = Path.GetExtension(fileName).TrimStart('.');

                if (extension.Equals(XamlExtension, StringComparison.OrdinalIgnoreCase))
                    return GetDesignerSource(fileName);
            }

            return null;
        }

        private bool SetScriptSource(EditorFormDesignerDataSource source, DesignerReferencedAssemblies referencedAssemblies)
        {
            string userCodeFileName = source.UserCodeFileName;

            var activeEditor = ActiveEditor;

            if (userCodeFileName != null && File.Exists(userCodeFileName))
            {
                scriptRun.ScriptSource.FromScriptFile(userCodeFileName);
                scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.Wpf);

                var formSettings = FormSettingsService.LoadSettings(source);
                if (formSettings.AssemblyReferences.Any())
                {
                    var references = formSettings.AssemblyReferences.Select(x => x.AssemblyPath).ToArray();
                    scriptRun.ScriptSource.References.AddRange(references);

                    foreach (var reference in references)
                    {
                        if (File.Exists(reference))
                            File.Copy(reference, Path.Combine(Path.GetDirectoryName(scriptRun.ScriptHost.ExecutableModulePath), Path.GetFileName(reference)), true);
                    }
                }

                scriptRun.ScriptSource.WpfResources.AddRange(GetAllImageFilesInFormFolder(userCodeFileName));
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

        private bool SaveAllModifiedFiles()
        {
            foreach (TabItem item in documentsTabControl.Items)
            {
                var content = item.Content;

                var edit = content as IScriptEdit;
                if (edit != null && edit.Modified)
                    edit.SaveFile(edit.FileName);

                var designer = content as IFormDesignerControl;
                if (designer != null)
                    SaveDesignerFiles(designer);
            }

            return true;
        }

        private void SaveDesignerFiles(IFormDesignerControl designer)
        {
            var source = (EditorFormDesignerDataSource)designer.Source;
            source.Save();

            FormSettingsService.SaveSettings(BuildFormSettings(designer), source);
        }

        private FormSettings BuildFormSettings(IFormDesignerControl designer)
        {
            var references = designer.ReferencedAssemblies.AssemblyNames.Select(
                x => new FormSettings.AssemblyReference(x)).ToArray();

            return new FormSettings(references);
        }

        private void RunScript()
        {
            try
            {
                var source = GetActiveDesignerSource();
                if (source == null)
                    return;

                var designer = ActiveDesigner;
                if (designer == null)
                    return;

                var referencedAssemblies = designer != null ? designer.ReferencedAssemblies : GetReferencedAssemblies(source);

                if (SaveAllModifiedFiles())
                {
                    InitializeScripter();

                    if (SetScriptSource(source, referencedAssemblies))
                        scriptRun.RunProcess();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error running form: " + e);
            }
        }

        private string[] GetAllImageFilesInFormFolder(string formFilePath)
        {
            var path = Path.GetDirectoryName(formFilePath);
            if (string.IsNullOrEmpty(path))
                path = Environment.CurrentDirectory;

            var extensions = new[] { "png", "gif", "jpg", "jpeg" };

            return extensions.SelectMany(x => Directory.GetFiles(path, "*." + x)).ToArray();
        }

        private void Close_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var selectedItem = documentsTabControl.SelectedItem as TabItem;
            if (selectedItem == null)
                return;

            var edit = selectedItem.Content as IScriptEdit;
            if (edit == null)
            {
                var xamlFileName = ((IFormDesignerDataSource)ActiveDesigner.Source).XamlFileName;
                if (!codeOrXamlTabPages.Any(x => x.Key.StartsWith(xamlFileName, StringComparison.OrdinalIgnoreCase)))
                    this.sourcesByFormId.Remove(xamlFileName);
            }

            documentsTabControl.Items.Remove(selectedItem);

            RemoveAllByValue(codeOrXamlTabPages, selectedItem);
            RemoveAllByValue(designerTabPages, selectedItem);
            UpdateEditorButtons();
        }

        private void UndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null && edit.CanUndo)
                edit.Undo();
            else
            {
                var designer = ActiveDesigner;
                if ((designer != null) && designer.DesignerCommands.CanUndo)
                    designer.DesignerCommands.Undo();
            }
        }

        private void RedoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null && edit.CanRedo)
                edit.Redo();
            else
            {
                var designer = ActiveDesigner;
                if ((designer != null) && designer.DesignerCommands.CanRedo)
                    designer.DesignerCommands.Redo();
            }
        }

        private void UpdateEditorButtons()
        {
            IScriptEdit edit = ActiveEditor;

            bool enabled = edit != null;

            miUndo.IsEnabled = false;
            miRedo.IsEnabled = false;

            if (edit == null && ActiveDesigner != null)
                UpdateDesignerButtons();
        }

        private void UpdateDesignerButtons()
        {
            var designer = ActiveDesigner;

            if (designer == null)
                return;

            bool enabled = true;

            miUndo.IsEnabled = enabled && designer.DesignerCommands.CanUndo;
            miRedo.IsEnabled = enabled && designer.DesignerCommands.CanRedo;
        }

        private void Cut_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null)
            {
                if (edit.CanCut)
                    edit.Cut();
            }
            else
            {
                var designer = ActiveDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCut)
                    designer.DesignerCommands.Cut();
            }
        }

        private void Copy_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null)
            {
                if (edit.CanCopy)
                    edit.Copy();
            }
            else
            {
                var designer = ActiveDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCopy)
                    designer.DesignerCommands.Copy();
            }
        }

        private void Paste_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            IScriptEdit edit = ActiveEditor;
            if (edit != null)
            {
                if (edit.CanPaste)
                    edit.Paste();
            }
            else
            {
                var designer = ActiveDesigner;
                if ((designer != null) && designer.DesignerCommands.CanPaste)
                    designer.DesignerCommands.Paste();
            }
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

            if (ActiveEditor != null)
            {
                e.CanExecute = ActiveEditor.CanUndo;
            }

            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanUndo;
        }

        private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (ActiveEditor != null)
            {
                e.CanExecute = ActiveEditor.CanRedo;
            }

            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanRedo;
        }

        private void CanCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (ActiveEditor != null)
            {
                e.CanExecute = ActiveEditor.CanCut;
            }

            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanCut;
        }

        private void CanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (ActiveEditor != null)
            {
                e.CanExecute = ActiveEditor.CanCopy;
            }

            if (ActiveDesigner == null)
                return;

            e.CanExecute = ActiveDesigner.DesignerCommands.CanCopy;
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            if (ActiveEditor != null)
            {
                e.CanExecute = ActiveEditor.CanPaste;
            }

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

            var language = FormFilesUtility.TryFindUserCodeFile(xamlFileName);

            if (language == null)
            {
                MessageBox.Show("User code file not found.");
                return;
            }

            OpenAllFormFiles(xamlFileName);
        }

        private void OpenAllFormFiles(string xamlFileName)
        {
            var source = GetDesignerSource(xamlFileName);

            OpenCode(source.UserCodeFileName);
            OpenXaml(source.XamlFileName);
            OpenDesigner(source.XamlFileName);
        }

        private void OpenCode(string codeFileName)
        {
            if (codeOrXamlTabPages.ContainsKey(codeFileName))
            {
                documentsTabControl.SelectedItem = codeOrXamlTabPages[codeFileName];
                return;
            }

            var editor = new ScriptCodeEdit();
            editor.InitSyntax();

            editor.AllowDrop = true;
            editor.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(Editor_DragOver), true);
            editor.AddHandler(RichTextBox.DropEvent, new DragEventHandler(Editor_Drop), true);
            editor.StatusChanged += new EventHandler(EditorStatusChanged);

            var source = GetDesignerSource(codeFileName.Replace(".cs", string.Empty).Replace(".vb", string.Empty));

            FormDesignerEditorHelpers.SetEditorSource(editor, codeFileName, source);
            editor.RegisterAssemblies(GetReferencedAssemblies());

            codeOrXamlTabPages[codeFileName] = AddDocumentTab(codeFileName, editor);
            UpdateEditorButtons();
        }

        private void EditorStatusChanged(object sender, EventArgs e)
        {
            UpdateEditorButtons();
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

        private void OpenDesigner(string xamlFileName)
        {
            if (designerTabPages.ContainsKey(xamlFileName))
            {
                documentsTabControl.SelectedItem = designerTabPages[xamlFileName];
                return;
            }

            var designer = new CustomizedFormDesignerControl();
            designer.DesignPanel.PropertyChanged += DesignPanel_PropertyChanged;
            designer.ShowPropertiesRequested += Designer_ShowPropertiesRequested;
            var source = GetDesignerSource(xamlFileName);
            designer.ReferencedAssemblies = GetReferencedAssemblies(source);

            try
            {
                designer.Source = source;
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error");
                return;
            }

            designer.NavigateToUserMethodRequested += Designer_NavigateToUserMethodRequested;
            designer.ControlManipulationOptions = ControlManipulationOptions.AllowRotate | ControlManipulationOptions.AllowSkew;
            designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).XamlTextSource).Edits.Any();

            designerTabPages[xamlFileName] = AddDocumentTab(xamlFileName, designer);
            UpdateEditorButtons();
            UpdateDesignerButtons();

            designer.DesignContext.Services.Selection.SelectionChanged += (o, e) => OnDesignerSelectionChanged(designer, e);
            designer.DesignContext.Services.GetRequiredService<UndoService>().UndoStackChanged += (o, e) => UpdateDesignerButtons();
        }

        private void Designer_ShowPropertiesRequested(object sender, EventArgs e)
        {
            var designer = ActiveDesigner;
            if (propertyGrid.FormDesigner == designer)
            {
                RightTabControl.SelectedItem = PropertyTab;
            }
        }

        private void OnDesignerSelectionChanged(FormDesignerControl designer, DesignItemCollectionEventArgs e)
        {
            if (ActiveDesigner != designer)
                return;

            UpdateDesignerButtons();
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

            var source = (EditorFormDesignerDataSource)designer.Source;

            OpenCode(source.UserCodeFileName);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);

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

        private void SetCaretToMethod(string userCodeFileName, string methodName)
        {
            string toFind;
            var cs = Path.GetExtension(userCodeFileName) == ".cs";
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
                var xamlFileName = ((IFormDesignerDataSource)designer.Source).XamlFileName;
                if (this.editedXamlFiles.Contains(xamlFileName))
                {
                    designer.Reload();
                    editedXamlFiles.Remove(xamlFileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error");
            }
        }

        private void New_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "C# Files|*.xaml.cs|VB.NET Files|*.xaml.vb|All Files|*.*",
                FileName = "Form1.xaml.cs",
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            var result = saveFileDialog.ShowDialog();

            if (result == null || !result.Value)
                return;

            var userCodeFileName = saveFileDialog.FileName;
            var xamlFileName = Path.ChangeExtension(userCodeFileName, string.Empty).TrimEnd('.');
            if (!xamlFileName.EndsWith(XamlExtension, StringComparison.OrdinalIgnoreCase))
                MessageBox.Show("Form file name should have a .xaml.cs or .xaml.vb extension.");

            var source = new FormDesignerDataSource(
                xamlFileName,
                FormFilesUtility.DetectLanguageFromFileName(userCodeFileName));

            FormFilesUtility.CreateFormFiles(source, new FormFilesUtility.CreateFormFilesOptions { GenerateMainMethod = true });

            OpenDesigner(source.XamlFileName);
            OpenXaml(source.XamlFileName);
            OpenCode(source.UserCodeFileName);

            filesControl.RefreshFiles();
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

        private void OpenXaml(string xamlFileName)
        {
            if (codeOrXamlTabPages.ContainsKey(xamlFileName))
            {
                documentsTabControl.SelectedItem = codeOrXamlTabPages[xamlFileName];
                return;
            }

            var editor = new ScriptCodeEdit();
            (editor as ScriptCodeEdit).LineModificatorsVisible = true;

            editor.AllowDrop = true;
            editor.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(Editor_DragOver), true);
            editor.AddHandler(RichTextBox.DropEvent, new DragEventHandler(Editor_Drop), true);

            var source = GetDesignerSource(xamlFileName);

            FormDesignerEditorHelpers.SetEditorSource(editor as ScriptCodeEdit, xamlFileName, source);

            editor.TextChanged += (o, e) =>
            {
                if (documentsTabControl.SelectedContent == editor)
                    editedXamlFiles.Add(xamlFileName);
            };

            codeOrXamlTabPages[xamlFileName] = AddDocumentTab(xamlFileName, editor);
        }

        private string[] GetReferencedAssemblies()
        {
            return new string[] { "mscorlib", "System", "PresentationCore", "PresentationFramework", "System.Drawing", "WindowsBase", "Microsoft.VisualBasic" };
        }

        private DesignerReferencedAssemblies GetReferencedAssemblies(EditorFormDesignerDataSource source)
        {
            var defaultReferences = Path.GetExtension(source.UserCodeFileName).ToLower().Equals(".vb") ?
                DesignerReferencedAssemblies.DefaultForVisualBasic :
                DesignerReferencedAssemblies.DefaultForCSharp;

            var formSettings = FormSettingsService.LoadSettings(source);
            if (formSettings.AssemblyReferences.Any())
                return defaultReferences.WithAssemblyNames(formSettings.AssemblyReferences.Select(x => x.AssemblyPath).ToArray());
            else
                return defaultReferences;
        }

        private void Save_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (ActiveDesigner == null)
                return;

            var source = (EditorFormDesignerDataSource)ActiveDesigner.Source;

            source.XamlTextSource.SaveFile(source.XamlFileName);
            source.UserCodeTextSource.SaveFile(source.UserCodeFileName);
        }

        internal class CustomizedFormDesignerControl : FormDesignerControl
        {
            public IEnumerable<string> GetReferences(DesignerReferencedAssemblies referencedAssemblies)
            {
                var resolver = new AssemblyReferenceResolver(
                    referencedAssemblies.SearchPaths,
                    referencedAssemblies.FrameworkPath,
                    referencedAssemblies.BaseDirectory);

                var result = referencedAssemblies.AssemblyNames.Select(
                    x => resolver.ResolveReference(x, null, true)).Where(
                        x => !string.IsNullOrEmpty(x));

                var unique = RemoveDuplicateReferences(result).ToArray();

                return unique.ToArray();
            }

            protected override XamlLoadSettings GetDefaultXamlLoadSettings()
            {
                var xamlLoadSettings = base.GetDefaultXamlLoadSettings();
                xamlLoadSettings.DesignerAssemblies.Add(GetType().Assembly);
                if ((Source != null) && (Source is EditorFormDesignerDataSource))
                {
                    var references = GetReferences(ReferencedAssemblies).ToList();
                    foreach (string reference in references)
                    {
                        try
                        {
                            Assembly asm = Assembly.LoadFrom(reference);
                            if (asm != null)
                                xamlLoadSettings.DesignerAssemblies.Add(asm);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                return xamlLoadSettings;
            }

            private IEnumerable<string> RemoveDuplicateReferences(IEnumerable<string> references)
            {
                var existingFileNames = new HashSet<string>();
                foreach (var reference in references)
                {
                    var fileName = Path.GetFileName(reference);
                    if (existingFileNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                        continue;

                    existingFileNames.Add(fileName);
                    yield return reference;
                }
            }
        }
    }
}