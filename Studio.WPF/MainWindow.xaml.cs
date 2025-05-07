#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.FormDesigner.Wpf;
using Microsoft.Win32;

namespace AlternetStudio.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        private const int OutputTabIndex = 1;
        private const int CallStackTabIndex = 2;
        private const int VariablesInScopeTabIndex = 3;
        private const int WatchesTabIndex = 4;
        private const int ErrorsTabIndex = 5;
        private const int ThreadsTabIndex = 6;
        private const int FindResultTabIndex = 7;

        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

#endregion

        static MainWindow()
        {
            Utilities.DoApplicationEvents += (s, e) =>
            {
                DoEvents();
            };
        }

        public MainWindow()
        {
            InitializeScripter();
            InitializeEditors();
            InitializeComponent();
            InitializeCodeNavigationBar();
            InitEditorsContextMenu();
            InitImages();
            InitializeFormDesigner();
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);
            BookMarkManager.SharedBookMarks.BookMarkAdded += SharedBookMarks_BookMarkAdded;
            BookMarkManager.SharedBookMarks.BookMarkRemoved += SharedBookMarks_BookMarkRemoved;
        }

        /// <summary>
        /// Processes all Windows messages currently in the message queue.
        /// </summary>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        protected override void OnClosed(EventArgs e)
        {
            FinalizeCodeSearch();
            base.OnClosed(e);
        }

        private static ImageSource LoadImageSource(string imageName)
        {
            return ScaledImageLoader.GetImage(typeof(MainWindow), "AlternetStudio.Wpf.Resources.", imageName);
        }

        private static bool CheckUserCodeLanguageNonAmbiguous(string xamlFileName, string userCodeFileName)
        {
            string existingUserCodeFile;
            if (!FormFilesUtility.CheckUserCodeLanguageNonAmbiguous(
                xamlFileName,
                userCodeFileName,
                out existingUserCodeFile))
            {
                MessageBox.Show(
                    string.Format(
                        "There is an existing code-behind file of a different language: {0}. Please remove the file or select a different file name.",
                        existingUserCodeFile));
                return false;
            }

            return true;
        }

        private Image LoadImage(string imageName)
        {
            var source = LoadImageSource(imageName);
            return new Image
            {
                Source = source,
                Style = (Style)FindResource("ToolbarImageStyle"),
            };
        }

        #region Designer

        private void AddReference(IList<string> references, string reference)
        {
            if (!references.Contains(reference))
                references.Add(reference);
        }

        private void Designer_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            if (designer != ActiveFormDesigner)
                return;

            var source = (EditorFormDesignerDataSource)designer.Source;

            OpenFile(source.UserCodeFileName);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);

            Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(() =>
                    {
                        var editor = (Control)ActiveSyntaxEdit;
                        if (editor != null && editor.Focusable)
                        {
                            editor.Focus();
                            Keyboard.Focus(editor);
                        }
                    }));
        }

        private FormDesignerControl CreateDesignerControl(string fileName)
        {
            FormDesignerControl designer;

            try
            {
                designer = new CustomizedFormDesignerControl
                {
                    ReferencedAssemblies = GetDesignerReferencedAssemblies(fileName),
                    AutoAddComponentAssemblyReferences = false,
                    Source = GetDesignerSource(fileName),
                };

                designer.NavigateToUserMethodRequested += Designer_NavigateToUserMethodRequested;
                designer.DesignedContentChanged += Designer_DesignedContentChanged;
                designer.ShowPropertiesRequested += Designer_ShowPropertiesRequested;
                designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).XamlTextSource).Edits.Any();
                designer.DesignContext.Services.Component.ComponentRegisteredAndAddedToContainer += Component_ComponentRegisteredAndAddedToContainer;
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return designer;
        }

        private void Component_ComponentRegisteredAndAddedToContainer(object sender, DesignItemEventArgs e)
        {
            var designer = e.Item.Context.Services.GetService(typeof(FormDesignerControl)) as FormDesignerControl;

            if ((designer == null) || designer.IsBeingLoaded)
                return;

            var references = ComponentAssemblyReferenceAdder.TryAddComponentAssemblyReferences(
                e.Item.ComponentType,
                scriptRun.ScriptSource.References);

            scriptRun.ScriptSource.References.Clear();
            foreach (var reference in references)
                scriptRun.ScriptSource.References.Add(reference);

            designer.ReferencedAssemblies = ComponentAssemblyReferenceAdder.TryAddComponentAssemblyReferences(
                e.Item.ComponentType,
                designer.ReferencedAssemblies);
            var source = (EditorFormDesignerDataSource)designer.Source;
            var project = GetProject(source.UserCodeFileName);

            if (project != null && project.HasProject)
            {
                if (AddReferencesToProject(
                    project,
                    ComponentAssemblyReferenceAdder.TryAddComponentAssemblyReferences(e.Item.ComponentType, project.References.Select(x => x.FullName))))
                {
                    UpdateProjectExplorer();
                }
            }
        }

        private void OnDesignerSelectionChanged(FormDesignerControl designer, DesignItemCollectionEventArgs e)
        {
            if (ActiveFormDesigner != designer)
                return;

            UpdateDesignerControls();
        }

#endregion

#region Files and Projects

        private void UpdatePageHeader(TabItem page, string text, string toolTip)
        {
            if (page.Header is TextBlock)
            {
                if (((TextBlock)page.Header).Text != text)
                    page.Header = new TextBlock { Text = text, ToolTip = toolTip };
            }
        }

        #endregion

        #region Toolbar, Statusbar and event handlers

        private void InitImages()
        {
            newMenuItem.Icon = LoadImage("NewFile");

            newStripSplitImage.Source = LoadImageSource("NewFile");

            openMenuItem.Icon = LoadImage("OpenFile");
            openToolButton.Content = LoadImage("OpenFile");
            saveMenuItem.Icon = LoadImage("Save");
            saveToolButton.Content = LoadImage("Save");

            saveMenuItemAll.Icon = LoadImage("SaveAll");
            saveMenuItemAs.Icon = LoadImage("SaveAs");
            exitMenuItem.Icon = LoadImage("Exit");

            gotoToolButton.Content = LoadImage("GoToDefinition");
            historyBackwardToolButton.Content = LoadImage("Backwards");
            historyForwardToolButton.Content = LoadImage("Forwards");

            printMenuItem.Icon = LoadImage("Print");
            printToolButton.Content = LoadImage("Print");
            printPreviewMenuItem.Icon = LoadImage("PrintPreview");
            printPreviewToolButton.Content = LoadImage("PrintPreview");

            findMenuItem.Icon = LoadImage("FindInFile");
            findToolButton.Content = LoadImage("FindInFile");
            replaceMenuItem.Icon = LoadImage("ReplaceInFiles");
            replaceToolButton.Content = LoadImage("ReplaceInFiles");
            gotoDefinitionMenuItem.Icon = LoadImage("GoToDefinition");

            undoMenuItem.Icon = LoadImage("Undo");
            undoToolButton.Content = LoadImage("Undo");
            redoMenuItem.Icon = LoadImage("Redo");
            redoToolButton.Content = LoadImage("Redo");
            cutMenuItem.Icon = LoadImage("Cut");
            cutToolButton.Content = LoadImage("Cut");
            copyMenuItem.Icon = LoadImage("Copy");
            copyToolButton.Content = LoadImage("Copy");
            pasteMenuItem.Icon = LoadImage("Paste");
            pasteToolButton.Content = LoadImage("Paste");
            selectAllMenuItem.Icon = LoadImage("SelectAll");

            toggleBookmarkToolButton.Content = LoadImage("Bookmark");
            prevBookmarkToolButton.Content = LoadImage("PreviousBookmark");
            nextBookmarkToolButton.Content = LoadImage("NextBookmark");
            clearAllBookmarksToolButton.Content = LoadImage("ClearBookmark");
        }

        private void NewStripSplitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }

        private string TryGetXamlFileNameToOpenDesigner(string codeOrXamlFileName)
        {
            if (string.IsNullOrEmpty(codeOrXamlFileName))
                return null;

            string formId;
            if (FormFilesUtility.IsXamlFile(codeOrXamlFileName, out formId))
                return codeOrXamlFileName;

            var possibleXamlFile = GetXamlFileName(codeOrXamlFileName);
            var userCodeLanguage = FormFilesUtility.TryFindUserCodeFile(possibleXamlFile);
            if (userCodeLanguage == null)
                return null;

            if (!FormFilesUtility.CheckIfFormFilesExist(possibleXamlFile, userCodeLanguage))
                return null;

            return possibleXamlFile;
        }

        private void UpdateControls()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        private void UpdateStatusBar()
        {
            UpdateEditorStatus();
        }

        private string GetXamlFileName(string codeFileName)
        {
            if (codeFileName == string.Empty)
                return string.Empty;
            string str = Path.GetFileNameWithoutExtension(codeFileName);
            str = str.EndsWith("xaml") ? str : str + ".xaml";
            return Path.Combine(Path.GetDirectoryName(codeFileName), str);
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCodeSearch();
            InitializeExplorerTrees();
            InitializeToolbar();
            LocateStartupDirectory();
            LoadStartupFile();
            UpdateControls();
            InitializeNavigationHistory();
            InitializeDebugger();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (Keyboard.IsKeyDown(Key.F12) && findImplementationsMenuItem.IsEnabled)
                    {
                        e.Handled = true;
                        FindImplementationsMenuItem_Click(this, new RoutedEventArgs());
                    }
                }

                if (Keyboard.IsKeyDown(Key.F10) && debugMenu.RunToCursorMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    RunToCursorMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StartWithoutDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StartWithoutDebugMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.S) && saveMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    SaveMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.P) && printMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    PrintMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.O) && openMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    OpenMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Z) && undoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    UndoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Y) && redoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    RedoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.X) && cutMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    CutMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.C) && copyMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    CopyMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.V) && pasteMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    PasteMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.A) && selectAllMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    SelectAllMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F) && findMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    FindMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.H) && replaceMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    ReplaceMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.G) && gotoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    GotoMenuItem_Click(this, new RoutedEventArgs());
                }
            }
            else
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (Keyboard.IsKeyDown(Key.F12) && findReferencesMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    FindReferencesMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F9) && debugMenu.EvaluateMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    EvaluateMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StopDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StopDebugMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F7) && viewDesignerMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    ViewDesignerMenuItem_Click(this, new RoutedEventArgs());
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.F12) && gotoDefinitionMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    GotoDefinitionMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.Delete) && deleteMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    DeleteMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F5) && debugMenu.StartDebugMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StartDebugMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F6) && debugMenu.CompileMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    CompileMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F11) && debugMenu.StepIntoMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    StepIntoMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F10) && debugMenu.StepOverMenuIem.IsEnabled)
                {
                    e.Handled = true;
                    StepOverMenuItem_Click(this, new RoutedEventArgs());
                }

                if (Keyboard.IsKeyDown(Key.F7) && viewCodeMenuItem.IsEnabled)
                {
                    e.Handled = true;
                    ViewCodeMenuItem_Click(this, new RoutedEventArgs());
                }
            }
        }

        #endregion

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