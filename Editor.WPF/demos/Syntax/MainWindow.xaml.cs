#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.Projects;
using Alternet.Common.Projects.DotNet;
using Alternet.Common.Wpf;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.Roslyn;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Win32;

namespace SyntaxEditor_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly RoutedCommand GotoDefinitionCommand = new RoutedUICommand(
            "GotoDefinition",
            "GotoDefinitionCommand",
            typeof(MainWindow),
            new InputGestureCollection(new InputGesture[]
                {
                    new KeyGesture(Key.F12),
                }));

        public static readonly RoutedCommand FindReferencesCommand = new RoutedUICommand(
            "Find References",
            "FindReferencesCommand",
            typeof(MainWindow),
            new InputGestureCollection(new InputGesture[]
                {
                    new KeyGesture(Key.F12, ModifierKeys.Shift, "Shift + F12"),
                }));

        public static readonly RoutedCommand FindImplementationsCommand = new RoutedUICommand(
            "Go To Implementation",
            "FindImplementationsCommand",
            typeof(MainWindow),
            new InputGestureCollection(new InputGesture[]
                {
                    new KeyGesture(Key.F12, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl + Shift + F12"),
                }));

        private const string SBlank = "Blank File";
        private const string SOpenExplorer = "{0} Code Explorer";
        private const int PropertiesImage = 2;
        private const int FolderCloseImage = 7;
        private const int FolderOpenImage = 8;
        private const int FindResultTabIndex = 2;

        private readonly System.Collections.Generic.Dictionary<string, ImageSource> typeIcons = new Dictionary<string, ImageSource>();
        private int updateCount = 0;
        private int projectFilterIndex = 0;
        private DotNetProject project = new DotNetProject();
        private DotNetProjectExplorer explorer = new DotNetProjectExplorer();
        private IDictionary<TabItem, TextEditor> editors = new Dictionary<TabItem, TextEditor>();
        private AboutBox companyInfo;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ProjectCreationData currentProjectData = new ProjectCreationData { ProjectType = "WPFApp" };
        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };
        private bool projectIsClosing;
        private bool findInProject = false;
        private DispatcherTimer updateTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            InitImages();

            updateTimer.Interval = TimeSpan.FromMilliseconds(200);
            updateTimer.Tick += UpdateTimer_Tick;
            ucFindResults.FindResultClick += FindResults_FindResultClick;
        }

        private string TemplateSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(dir, @"Studio\Projects"));
            }
        }

        private string DefaultProjectSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(dir, @"..\Projects"));
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (ConfirmSaveBeforeClosing())
                updateTimer.Stop();
            else
                e.Cancel = true;
        }

        #region Files and Projects

        private static ImageSource LoadImageSource(string imageName)
        {
            return ScaledImageLoader.GetImage(typeof(MainWindow), "CodeEditorSyntax.Wpf.Resources.", imageName);
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

        private void InitImages()
        {
            newMenuItem.Icon = LoadImage("NewFile");
            newStripSplitImage.Source = LoadImageSource("NewFile");
            openMenuItem.Icon = LoadImage("OpenFile");
            openToolButton.Content = LoadImage("OpenFile");
            saveMenuItem.Icon = LoadImage("Save");
            saveToolButton.Content = LoadImage("Save");
            saveAllMenuItem.Icon = LoadImage("SaveAll");
            saveAsMenuItem.Icon = LoadImage("SaveAs");
            exitMenuItem.Icon = LoadImage("Exit");

            printMenuItem.Icon = LoadImage("Print");
            printToolButton.Content = LoadImage("Print");
            printPreviewMenuItem.Icon = LoadImage("PrintPreview");
            printPreviewToolButton.Content = LoadImage("PrintPreview");

            findMenuItem.Icon = LoadImage("FindInFile");
            findToolButton.Content = LoadImage("FindInFile");
            replaceMenuItem.Icon = LoadImage("ReplaceInFiles");
            replaceToolButton.Content = LoadImage("ReplaceInFiles");

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

            var popMenu = (ContextMenu)this.Resources["mainContextMenu"];
            if (popMenu != null)
            {
                for (int i = 0; i < popMenu.Items.Count; i++)
                {
                    if (popMenu.Items[i] is MenuItem)
                    {
                        var item = popMenu.Items[i] as MenuItem;
                        switch (item.Name)
                        {
                            case "findContextMenuItem":
                                item.Icon = LoadImage("FindInFile");
                                break;

                            case "replaceContextMenuItem":
                                item.Icon = LoadImage("ReplaceInFiles");
                                break;

                            case "gotoDefinitionContextMenuItem":
                                item.Icon = LoadImage("GoToDefinition");
                                break;

                            case "openContextMenuItem":
                                item.Icon = LoadImage("OpenFile");
                                break;

                            case "cutContextMenuItem":
                                item.Icon = LoadImage("Cut");
                                break;

                            case "copyContextMenuItem":
                                item.Icon = LoadImage("Copy");
                                break;

                            case "pasteContextMenuItem":
                                item.Icon = LoadImage("Paste");
                                break;
                        }
                    }
                }
            }
        }

        private void NewToolButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }

        private bool SaveModifiedFiles(IList<string> files)
        {
            foreach (string file in files)
            {
                if (IsProjectFile(file))
                {
                    if (string.Compare(project.ProjectFileName, file, true) == 0)
                        project.Save();

                    continue;
                }

                TextEditor edit = FindFile(file);
                if (edit != null)
                {
                    if (edit.Source.Modified)
                        edit.Source.SaveFile(edit.Source.FileName);
                }
            }

            return true;
        }

        private TextEditor GetEditor(TabItem key)
        {
            TextEditor result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private TextEditor NewFile(string fileName, int index)
        {
            // creating new editor window and assigning lexer if possible.
            if (index == -1)
                index = FindLangByExt(ExtractFileExt(fileName));
            bool isSpecialScheme;
            ILexer lexer = GetLexer(index, out isSpecialScheme);
            TextSource source = new TextSource();
            source.HighlightReferences = true;
            if ((!isSpecialScheme) && (index > -1))
            {
                if (LangInfos.LangItems[index].SchemeName != string.Empty)
                    lexer.Scheme.LoadFile(LangInfos.LangItems[index].SchemeName);
            }

            TabItem page = new TabItem();
            page.GotFocus += DoGotFocus;
            page.Header = new TextBlock { Text = Path.GetFileName(fileName), ToolTip = fileName };

            tcEditors.Items.Add(page);
            tcEditors.SelectedItem = page;

            TextEditor edit = new TextEditor();
            edit.TabIndex = page.TabIndex + 1;
            editors.Add(page, edit);

            edit.Source = source;

            if (lexer is RoslynParser)
            {
                RoslynParser parser = (RoslynParser)lexer;
                parser.Repository.RegisterAssemblies(GetReferencedAssemblies(fileName));
                var namespaces = GetImportedNamespaces(fileName);
                if (namespaces != null)
                    parser.Repository.RegisterNamespaces(namespaces);
            }

            edit.TextChanged += (o, e) =>
            {
                UpdatePage(edit.Parent as TabItem, edit.Source.FileName, edit.Source.Modified);
                UpdateCodeWindows();
            };

            edit.VerticalAlignment = VerticalAlignment.Stretch;
            edit.HorizontalAlignment = HorizontalAlignment.Stretch;
            page.Content = edit;
            InitEditor(edit, lexer);

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileName;
                edit.Source.LoadFile(fileName);
                if (edit.Lexer is ISyntaxParser)
                    ((ISyntaxParser)edit.Lexer).FileName = fileName;
            }

            edit.AllowOutlining = true;
            edit.Spelling.CheckSpelling = true;
            edit.Spelling.SpellColor = System.Drawing.Color.Navy;
            edit.AllowOutlining = true;
            tcEditors.SelectedItem = page;
            UpdateCodeWindows();
            return edit;
        }

        private string[] GetReferencedAssemblies(string fileName)
        {
            if (project.HasProject)
            {
                IList<string> references = new List<string>();
                foreach (var reference in project.References)
                    references.Add(reference.FullName);
                references.Add("mscorlib");

                foreach (var r in project.AutoReferences)
                    references.Add(r.Name);

                if (Path.GetExtension(fileName).ToLower().Equals(".vb"))
                    references.Add("Microsoft.VisualBasic");
                return references.ToArray();
            }

            return new string[] { "mscorlib", "System", "PresentationCore", "PresentationFramework", "System.Drawing", "WindowsBase", "Microsoft.VisualBasic" };
        }

        private string[] GetImportedNamespaces(string fileName)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(".vb"))
                return null;

            return new string[] { "Microsoft.VisualBasic", "Microsoft.VisualBasic.VBMath", "System", "System.Collections", "System.Collections.Generic", "System.Data", "System.Drawing", "System.Diagnostics", "System.Windows.Media", "System.Windows.Controls", "System.Linq", "System.Xml.Linq", "System.Threading.Tasks" };
        }

        private void NewSampleFile(string lang)
        {
            // creating new sample file
            int idx = FindLangByDesc(lang);
            if ((idx >= 0) && (LangInfos.LangItems[idx].FileName != string.Empty))
                OpenFile(LangInfos.LangItems[idx].FileName, idx);
        }

        private TextEditor OpenFile(string fileName, int index)
        {
            return OpenFile(fileName, index, false);
        }

        private TextEditor OpenFile(string fileName, int index, bool forceReopen)
        {
            TextEditor edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabItem))
            {
                if (forceReopen)
                {
                    if (edit.Source.Modified && !ConfirmSaveBeforeClosing(edit.Source.FileName))
                        return edit;
                    CloseFile(edit.Source.FileName);
                }
                else
                {
                    ((TabItem)edit.Parent).IsSelected = true;
                    return edit;
                }
            }

            // loading file from disk
            edit = NewFile(fileName, index);
            UpdateControls(false);
            return edit;
        }

        private void SetActiveEdit(TextEditor edit)
        {
            if (edit != null && edit.Parent is TabItem)
                ((TabItem)edit.Parent).IsSelected = true;
        }

        private TextEditor OpenFile(string fileName)
        {
            return OpenFile(fileName, -1);
        }

        private bool FileBelongsToProject(string fileName)
        {
            if (project.HasProject)
            {
                if (project.Files.Contains(fileName) || project.Resources.Contains(fileName))
                    return true;
            }

            return false;
        }

        private TextEditor FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabItem tabPage in tcEditors.Items)
            {
                var edit = GetEditor(tabPage);
                if (edit != null)
                {
                    var path = edit.Source.FileName;
                    path = new Uri(path).LocalPath;
                    if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                        return edit;
                }
            }

            return null;
        }

        private void CloseFile(string fileName)
        {
            TextEditor edit = FindFile(fileName);
            if (edit != null)
            {
                TabItem page = edit.Parent as TabItem;
                if (page != null)
                {
                    tcEditors.Items.Remove(page);
                    if (editors.ContainsKey(page))
                        editors.Remove(page);
                }
            }
        }

        private void CloseFile(TextEditor edit)
        {
            var fileName = edit.Source.FileName;

            CodeParsing.UnregisterCode(Path.GetExtension(fileName), new string[] { fileName });
        }

        private void UpdatePageHeader(TabItem page, string text, string toolTip)
        {
            if (page.Header is TextBlock)
            {
                if (((TextBlock)page.Header).Text != text)
                    page.Header = new TextBlock { Text = text, ToolTip = toolTip };
            }
        }

        private void UpdatePage(TabItem page, string fileName, bool isModified = false)
        {
            // updating page control when editor is saved to another file
            if (page == null)
                return;
            string pageText = Path.GetFileName(fileName);

            pageText = isModified ? pageText + "*" : pageText;
            UpdatePageHeader(page, pageText, fileName);
        }

        private void RemovePage(object sender, EventArgs e)
        {
            // removing page from tab control when editor window is closed
            if (tcEditors.SelectedItem != null)
            {
                if (editors.ContainsKey((TabItem)tcEditors.SelectedItem))
                    editors.Remove((TabItem)tcEditors.SelectedItem);
            }

            int index = tcEditors.SelectedIndex;
            tcEditors.Items.Remove(tcEditors.SelectedItem);
            tcEditors.SelectedIndex = Math.Max(index - 1, 0);
        }

        private void RemovePage()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                if (edit.Source.Modified && !ConfirmSaveBeforeClosing(edit.Source.FileName))
                    return;
                BookMarkManager.Unregister(edit.Source);
                CloseFile(edit);
            }

            tcEditors.Items.Remove((TabItem)tcEditors.SelectedItem);
            UpdateControls(false);
            UpdateCodeWindows();
        }

        private void OpenProject(string fileName)
        {
            project.Load(fileName);
            string firstFile = string.Empty;

            if (project.Files.Count > 0)
            {
                firstFile = project.Files[0];
                OpenFile(firstFile);

                var extension = Path.GetExtension(firstFile);

                CodeParsing.RegisterCode(extension, GetSourceFiles(project.Files));
                var references = project.References.Select(x => x.FullName).ToArray();
                CodeParsing.RegisterAssemblies(extension, project.TryResolveAbsolutePaths(references).ToArray());
            }

            UpdateProjectExplorer();
            UpdateCodeWindows();
        }

        private bool IsProjectFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".csproj", true) == 0 || string.Compare(ext, ".vbproj", true) == 0;
        }

        private bool IsResourceFileName(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".resx", true) == 0 || string.Compare(ext, ".xaml", true) == 0;
        }

        private string[] GetSourceFiles(IList<string> files)
        {
            IList<string> sourceFiles = new List<string>();
            foreach (var file in files)
            {
                if (!IsResourceFileName(file))
                    sourceFiles.Add(file);
            }

            return sourceFiles.ToArray();
        }

        private bool CloseProject()
        {
            if (!ConfirmSaveProjectBeforeClosing())
                return false;

            projectIsClosing = true;

            try
            {
                foreach (string fileName in project.Files)
                {
                    CloseFile(fileName);
                }

                foreach (string fileName in project.Resources)
                {
                    CloseFile(fileName);
                }

                if (project.Files.Count > 0)
                {
                    var extension = Path.GetExtension(project.Files[0]);

                    CodeParsing.UnregisterAssemblies(extension);
                    CodeParsing.UnregisterCode(extension, project.Files.ToArray());
                }

                project.Reset();

                UpdateCodeWindows();
            }
            finally
            {
                projectIsClosing = false;
            }

            UpdateProjectExplorer();
            return true;
        }

        private bool ContainsFile(IList<string> files, string fileName)
        {
            if (files == null)
                return true;
            foreach (var file in files)
            {
                if (string.Compare(file, fileName, true) == 0)
                    return true;
            }

            return false;
        }

        private void AddFile(IList<string> files, string fileName)
        {
            if (ContainsFile(files, fileName))
                return;
            files.Add(fileName);
        }

        private void AddFiles(IList<string> files, string fileName)
        {
            AddFile(files, fileName);

            string xamlFileName = GetXamlFileName(fileName);
            if (!string.IsNullOrEmpty(xamlFileName) && File.Exists(xamlFileName))
                AddFile(files, xamlFileName);
        }

        private IList<string> GetModifiedFiles(IList<string> files)
        {
            IList<string> result = new List<string>();

            foreach (TabItem tabPage in tcEditors.Items)
            {
                TextEditor edit = GetEditor(tabPage);
                if (edit != null && edit.Source.Modified)
                {
                    if (!ContainsFile(files, edit.Source.FileName))
                        continue;

                    AddFile(result, edit.Source.FileName);
                }
            }

            if (project.HasProject && project.IsModified)
            {
                if (ContainsFile(files, project.ProjectFileName))
                    AddFile(result, project.ProjectFileName);
            }

            return result;
        }

        private bool ConfirmSaveBeforeClosing()
        {
            IList<string> list = GetModifiedFiles(null);
            return ConfirmSaveBeforeClosing(list, true);
        }

        private bool ConfirmSaveProjectBeforeClosing()
        {
            IList<string> list = GetModifiedFiles(project.AllFiles(true));
            return ConfirmSaveBeforeClosing(list, true);
        }

        private bool ConfirmSaveBeforeClosing(string fileName)
        {
            IList<string> files = new List<string>();
            files.Add(fileName);
            return ConfirmSaveBeforeClosing(files, false);
        }

        private void GetDisplayFilesForProject(IList<string> displayFiles, IList<string> files)
        {
            bool projectAdded = false;
            int i = 0;
            while (i < files.Count)
            {
                string file = files[i];
                if (!IsProjectFile(file) && FileBelongsToProject(file))
                {
                    if (!projectAdded)
                    {
                        bool projectModified = files.IndexOf(project.ProjectFileName) >= 0;
                        displayFiles.Add(Path.GetFileName(project.ProjectFileName) + (projectModified ? "*" : string.Empty));
                        projectAdded = true;
                    }

                    displayFiles.Add(string.Format("   {0}*", Path.GetFileName(file)));
                    files.RemoveAt(i);
                }
                else
                    i++;
            }

            if (projectAdded)
            {
                int idx = files.IndexOf(project.ProjectFileName);
                if (idx >= 0)
                    files.RemoveAt(idx);
            }
        }

        private IList<string> GetDisplayFiles(IList<string> files, bool groupPerProject)
        {
            IList<string> result = new List<string>();
            IList<string> newFiles = new List<string>();

            foreach (var file in files)
                newFiles.Add(file);

            if (groupPerProject && project.HasProject)
            {
                GetDisplayFilesForProject(result, newFiles);
            }

            foreach (string file in newFiles)
            {
                result.Add(Path.GetFileName(file) + "*");
            }

            return result;
        }

        private bool ConfirmSaveBeforeClosing(IList<string> files, bool groupPerProject)
        {
            bool confirm = true;

            if (files.Count > 0)
            {
                string productName = string.Empty;
                IList<string> displayFiles = GetDisplayFiles(files, groupPerProject);
                var list = Application.Current.MainWindow.GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
                if (list != null)
                {
                    if (list.Length > 0)
                    {
                        productName = (list[0] as AssemblyProductAttribute).Product;
                    }
                }

                DlgConfirmSave dlg = new DlgConfirmSave(productName, files);

                if (dlg.ShowDialog().Value)
                {
                    SaveModifiedFiles(files);
                }

                confirm = dlg.Confirm;
            }

            return confirm;
        }

        #endregion Files and Projects

        #region Toolbar, Statusbar and event handlers

        private void NewClick(object sender, RoutedEventArgs e)
        {
            // new file
            int index = -1;
            int idx = FindLangByDesc((sender as MenuItem).Header.ToString(), ref index);
            saveFileDialog.FilterIndex = (index >= 0) ? index + 1 : 1;
            saveFileDialog.FileName = "file1";

            if (!saveFileDialog.ShowDialog().Value)
                return;

            using (var stringWriter = new StringWriter())
            {
                using (var streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    var sb = stringWriter.GetStringBuilder();

                    streamWriter.Write(sb);
                }
            }

            OpenFile(saveFileDialog.FileName, idx, true);
        }

        private void SampleClick(object sender, RoutedEventArgs e)
        {
            // new sample file
            NewSampleFile((sender as MenuItem).Header.ToString());
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing about box dialog
            if (companyInfo == null)
                companyInfo = new AboutBox();
            companyInfo.ShowDialog();
        }

        private string GetXamlFileName(string codeFileName)
        {
            if (codeFileName == string.Empty)
                return string.Empty;
            string str = Path.GetFileNameWithoutExtension(codeFileName);
            str = str.EndsWith("xaml") ? str : str + ".xaml";
            return Path.Combine(Path.GetDirectoryName(codeFileName), str);
        }

        private void NewProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentProjectData.ProjectLocation))
                currentProjectData.ProjectLocation = DefaultProjectSubPath;
            DlgNewProject dlg = new DlgNewProject(currentProjectData, new string[] { "C#", "Visual Basic" }, ProjectTypes.SupportedProjectTypes);
            if (dlg.ShowDialog().Value)
            {
                currentProjectData = dlg.ProjectData;
                if (CloseProject())
                {
                    string projectFileName = project.Create(dlg.ProjectData, TemplateSubPath);
                    if (File.Exists(projectFileName))
                        OpenProject(projectFileName);
                }
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // loading file
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                OpenFile(openFileDialog.FileName);
            }
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemovePage();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
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
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // saving editor content to file on disk
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                if (edit.Source.FileName != string.Empty)
                {
                    edit.Source.SaveFile(edit.Source.FileName);
                    UpdatePage(edit.Parent as TabItem, edit.Source.FileName, edit.Source.Modified);
                }
                else
                    SaveAsMenuItem_Click(sender, e);
            }
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // saving editor content to file on disk prompting for file name
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                saveFileDialog.FilterIndex = LangInfos.LangItems.Length;
                string oldExt = string.Empty;
                if (edit.Source.FileName != null)
                {
                    saveFileDialog.FileName = edit.Source.FileName;
                    oldExt = ExtractFileExt(edit.Source.FileName);
                    int idx = FindLangByExt(oldExt);
                    if (idx >= 0)
                        saveFileDialog.FilterIndex = idx + 1;
                }

                if (saveFileDialog.ShowDialog(Window.GetWindow(this)).Value)
                {
                    string fileName = saveFileDialog.FileName;
                    edit.Source.SaveFile(fileName);
                    edit.Source.FileName = fileName;
                    UpdatePage((TabItem)tcEditors.SelectedItem, fileName);

                    string ext = ExtractFileExt(edit.Source.FileName);
                    if (string.Compare(ext, oldExt, true) != 0)
                    {
                        int index = FindLangByExt(ext);
                        if (index >= 0)
                        {
                            Lexer lexer = new Lexer();
                            if (LangInfos.LangItems[index].SchemeName != string.Empty)
                                lexer.Scheme.LoadFile(LangInfos.LangItems[index].SchemeName);
                            edit.Source.Lexer = lexer;
                        }
                    }
                }
            }
        }

        private void SaveAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveAllModifiedFiles();
        }

        private bool SaveAllModifiedFiles()
        {
            foreach (TabItem tabPage in tcEditors.Items)
            {
                TextEditor edit = GetEditor(tabPage);
                if (edit != null && edit.Source.Modified)
                {
                    edit.Source.SaveFile(edit.Source.FileName);
                    UpdatePage(tabPage, edit.Source.FileName, edit.Source.Modified);
                }
            }

            project.Save();
            UpdateProjectExplorer();

            return true;
        }

        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.FilterIndex = this.projectFilterIndex;
            if (openFileDialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                if (project.HasProject)
                    CloseProject();
                OpenProject(openFileDialog.FileName);
            }
        }

        private void SaveProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            project.Save();
        }

        private void CloseProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseProject();
        }

        private void PrintPreviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing print preview dialog
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Printing.ExecutePrintPreviewDialog(this);
        }

        private void PrintMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // printing the editor
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Printing.Print();
        }

        private void PageSetupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing page setup dialog
        }

        private void CodeExplorerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenExplorer();
        }

        private void FindResults_FindResultClick(object sender, FindResultClickEventArgs e)
        {
            NavigateToRange(e.FileRange);
        }

        private void NavigateToRange(IFileRange range)
        {
            if (range != null)
            {
                if (!string.IsNullOrEmpty(range.FileName))
                {
                    TextEditor edit = OpenFile(range.FileName);
                    if (edit != null)
                    {
                        var position = new System.Drawing.Point(range.StartPoint.X, range.StartPoint.Y);
                        edit.MakeVisible(position, true);
                        edit.Position = position;
                        edit.Focus();
                    }
                }
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // closing the application
            this.Close();
        }

        private void UndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // undoing the last change
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Source.CanUndo())
                syntaxEdit.Source.Undo();
        }

        private void RedoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // redoing the last change
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Source.CanRedo())
                syntaxEdit.Source.Redo();
        }

        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // cutting selection to clipboard
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanCut())
                syntaxEdit.Selection.Cut();
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // copying selection to clipboard
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanCopy())
                syntaxEdit.Selection.Copy();
        }

        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // pasting selection to clipboard
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanPaste())
                syntaxEdit.Selection.Paste();
        }

        private void SelectAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // selecting editor's content
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Selection.SelectAll();
        }

        private void FindMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing search dialog
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplaySearchDialog();
        }

        private void ReplaceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing replace dialog
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplayReplaceDialog();
        }

        private void GotoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // executing goto line dialog
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplayGotoLineDialog();
        }

        private void GotoDefinitionMenuItem_Click(object sender, EventArgs e)
        {
            GotoDefinition();
        }

        private void FindReferencesMenuItem_Click(object sender, EventArgs e)
        {
            FindReferences();
        }

        private void FindImplementationsMenuItem_Click(object sender, EventArgs e)
        {
            FindImplementations();
        }

        private async void GotoDefinition()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                SymbolLocation location = await parser.FindDeclarationAsync(edit.Position);
                if (location != null)
                {
                    OpenFile(location.FileName);
                    edit = GetActiveSyntaxEdit();
                    edit.Position = new System.Drawing.Point(location.Column, location.Line);
                    edit.MakeVisible(new System.Drawing.Point(location.Column, location.Line), true);
                }
            }
        }

        private void FindReferences()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                IRangeList references = new RangeList();
                parser.FindReferences(edit.Position, references, true);
                if (references.Count > 0)
                {
                    ucFindResults.AddFindResults(references);
                    if (references.Count > 1)
                        ActivateFindResultsTab();
                    else
                        NavigateToRange(references[0] as IFileRange);
                }
            }
        }

        private void FindImplementations()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                IRangeList references = new RangeList();
                parser.FindImplementations(edit.Position, references, true);
                if (references.Count > 0)
                {
                    ucFindResults.AddFindResults(references);
                    if (references.Count > 1)
                        ActivateFindResultsTab();
                    else
                        NavigateToRange(references[0] as IFileRange);
                }
                else
                    GotoDefinition();
            }
        }

        private void UpdateSearch()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            var search = edit as ISearch;
            if (search != null)
            {
                search.SearchGlobal = true;
            }
        }

        private void SharedSearch_InitSearch(object sender, InitSearchEventArgs e)
        {
            e.Search = GetActiveSyntaxEdit() as ISearch;
            findInProject = (e.Options & SearchOptions.CurrentProject) != 0;
            if (findInProject && project.HasProject)
            {
                for (int i = 0; i < project.Files.Count; i++)
                {
                    if (File.Exists(project.Files[i]))
                        e.SearchList.Add(project.Files[i]);
                }
            }
            else
            {
                foreach (TextEditor edit in editors.Values)
                {
                    var search = edit as ISearch;
                    if (search != null)
                        search.SearchGlobal = true;
                    if (edit.Source != null)
                        e.SearchList.Add(edit.Source.FileName);
                }
            }
        }

        private void SharedSearch_GetSearch(object sender, GetSearchEventArgs e)
        {
            if (findInProject && project.HasProject)
            {
                for (int i = 0; i < project.Files.Count; i++)
                {
                    if (string.Compare(project.Files[i], e.FileName, true) == 0)
                    {
                        TextEditor edit = FindFile(e.FileName);
                        if (edit != null)
                        {
                            SetActiveEdit(edit);
                            UpdateSearch();
                            e.Search = (ISearch)edit;
                        }

                        break;
                    }
                }
            }
            else
            {
                foreach (TabItem page in editors.Keys)
                {
                    TextEditor edit = GetEditor(page);
                    if (edit != null && edit.Source != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                    {
                        tcEditors.SelectedItem = page;
                        UpdateSearch();
                        e.Search = (ISearch)edit;
                        break;
                    }
                }
            }
        }

        private void SharedSearch_TextFound(object sender, TextFoundEventArgs e)
        {
            TryOpenFile(e.FileName);
            if (e.Search == null)
            {
                TextEditor edit = GetActiveSyntaxEdit();
                if (edit != null)
                {
                    edit.OnTextFound(e.Text, e.Options, e.Expression, e.Match, e.Position, e.Len, false, e.MultiLine);
                    e.Search = edit;
                    UpdateSearch();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitSearch();

            sslPosition.Width = 120;

            // assigning explorer tree
            explorer.ExplorerTree = tvSyntax;

            // locating schemes
            if (!new DirectoryInfo(Path.Combine(dir, "Resources")).Exists)
                dir = Path.GetFullPath(Path.Combine(dir, @"..\..\..\..\..\..\"));
            dir = Path.Combine(dir, @"Resources");

            // loading schemes
            DirectoryInfo info = new DirectoryInfo(Path.Combine(dir, @"Editor\Schemes"));
            FileInfo[] files;
            if (info.Exists)
            {
                files = info.GetFiles();
                for (int k = 0; k < files.Length; k++)
                {
                    int idx = FindLangByName(RemoveFileExt(files[k].Name));
                    if (idx >= 0)
                        LangInfos.LangItems[idx].SchemeName = files[k].FullName;
                }
            }

            // loading example files
            info = new DirectoryInfo(Path.Combine(dir, @"Editor\Text"));
            if (info.Exists)
            {
                files = info.GetFiles();
                for (int j = 0; j < files.Length; j++)
                {
                    int idx = FindLangByName(RemoveFileExt(files[j].Name));
                    if (idx >= 0)
                        LangInfos.LangItems[idx].FileName = files[j].FullName;
                }
            }

            // populating sample menu
            bool found = false;
            var mnuFiles = (ContextMenu)this.Resources["mnuFiles"];
            MenuItem item = null;
            MenuItem citem = null;
            MenuItem sampleItem = null;
            foreach (LanguageInfo linfo in LangInfos.LangItems)
            {
                if (linfo.FileType == "-")
                {
                    if (found)
                    {
                        if (mnuFiles != null)
                            mnuFiles.Items.Add(new Separator());
                        newMenuItem.Items.Add(new Separator());
                        samplesMenuItem.Items.Add(new Separator());
                    }
                }
                else
                    if ((linfo.FileName != string.Empty) && (linfo.SchemeName != string.Empty))
                {
                    found = true;
                    item = new MenuItem();
                    item.Header = linfo.Description;
                    item.Click += new RoutedEventHandler(NewClick);
                    citem = new MenuItem();
                    citem.Header = linfo.Description;
                    citem.Click += new RoutedEventHandler(NewClick);
                    sampleItem = new MenuItem();
                    sampleItem.Header = linfo.Description;
                    sampleItem.Click += new RoutedEventHandler(SampleClick);

                    newMenuItem.Items.Add(item);
                    if (mnuFiles != null)
                        mnuFiles.Items.Add(citem);
                    samplesMenuItem.Items.Add(sampleItem);
                }
            }

            item = new MenuItem();
            item.Header = SBlank;
            item.Click += new RoutedEventHandler(NewClick);
            citem = new MenuItem();
            citem.Header = SBlank;
            citem.Click += new RoutedEventHandler(NewClick);
            newMenuItem.Items.Add(new Separator());
            newMenuItem.Items.Add(item);
            if (mnuFiles != null)
            {
                mnuFiles.Items.Add(new Separator());
                mnuFiles.Items.Add(citem);
            }

            newToolButton.ContextMenu = mnuFiles;
            string filter = string.Empty;
            int count = 0;
            for (int i = 0; i < LangInfos.LangItems.Length; i++)
            {
                if (!string.IsNullOrEmpty(LangInfos.LangItems[i].FileExt))
                {
                    filter += string.Format("{0} files ({1})|{1}" + ((i == LangInfos.LangItems.Length - 1) ? string.Empty : "|"), LangInfos.LangItems[i].FileType, LangInfos.LangItems[i].FileExt);
                    count++;
                }
            }

            string projectFilter = filter + "| All project files (*.csproj; *.vbproj)| *.csproj; *.vbproj";
            projectFilterIndex = count + 1;

            openFileDialog.Filter = projectFilter;
            saveFileDialog.Filter = filter;

            if (new DirectoryInfo(dir).Exists)
            {
                openFileDialog.InitialDirectory = Path.GetFullPath(dir);
                saveFileDialog.InitialDirectory = Path.GetFullPath(dir);
            }

            // assigning tags for toolbar buttons
            UpdateToolBar();
            samplesMenuItem.Visibility = (samplesMenuItem.Items.Count > 0) ? Visibility.Visible : Visibility.Hidden;

            // opening sample file
            NewSampleFile("C#");

            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.CodeCompletionBox.SelectionChanged += new EventHandler(CodeCompletionBox_SelectionChanged);
            }

            tvSyntax.ContextMenu = tvSyntax.Resources["SolutionContext"] as System.Windows.Controls.ContextMenu;
        }

        private void EditorsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!projectIsClosing)
            {
                UpdateCodeWindows();
                UpdateEvents(true, string.Empty);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var popMenu = (ContextMenu)this.Resources["mainContextMenu"];
            if (popMenu != null)
            {
                bool enabled = CanGotoDefinition();
                for (int i = 0; i < popMenu.Items.Count; i++)
                {
                    if (popMenu.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)popMenu.Items[i];
                        switch (item.Name)
                        {
                            case "gotoDefinitionContextMenuItem":
                                item.IsEnabled = enabled;
                                break;
                        }
                    }
                }
            }

            UpdateEvents(false, "Context menu is opened");
        }

        private void DoGotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender is TabItem) && editors.ContainsKey((TabItem)sender))
            {
                ((TabItem)sender).GotFocus -= DoGotFocus;

                TextEditor edit = GetEditor((TabItem)sender);
                if (edit != null && !edit.IsFocused)
                    edit.Focus();
            }
        }

        private void MethodsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateCount > 0)
                return;
            TextEditor edit = GetActiveSyntaxEdit();

            RoslynParser parser = null;
            if ((edit != null) && (edit.Lexer is RoslynParser))
                parser = (RoslynParser)edit.Lexer;

            if (edit != null && sender is ComboBox)
            {
                System.Drawing.Point position;
                if (CodeParsing.SelectItem((ComboBox)sender, parser, out position))
                {
                    edit.Position = position;
                    edit.Focus();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ErrorsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvErrors.SelectedItems.Count > 0)
            {
                object obj = lvErrors.SelectedItem;
                if ((obj != null) && (obj is ListViewItemData))
                {
                    object errObj = ((ListViewItemData)obj).Tag;
                    if ((errObj != null) && (errObj is ISyntaxError))
                    {
                        TextEditor syntaxEdit = GetActiveSyntaxEdit();
                        if (syntaxEdit != null)
                        {
                            syntaxEdit.MoveTo(((ISyntaxError)errObj).Position);
                            syntaxEdit.Selection.SetSelection(SelectionType.Stream, ((ISyntaxError)errObj).Range.StartPoint, ((ISyntaxError)errObj).Range.EndPoint);
                            syntaxEdit.Focus();
                        }
                    }
                }
            }
        }

        private void OnItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                e.Handled = true;
                if ((tvSyntax.SelectedItem != null) && (tvSyntax.SelectedItem is TreeViewItem))
                {
                    var tag = ((TreeViewItem)tvSyntax.SelectedItem).Tag;

                    string codeFileName = null;

                    var formNodeData = tag as FormNodeData;
                    if (formNodeData != null)
                    {
                        codeFileName = formNodeData.FileName;
                    }
                    else
                        codeFileName = tag as string;

                    if (!IsReferenceNode((TreeViewItem)tvSyntax.SelectedItem) && !string.IsNullOrEmpty(codeFileName) && new FileInfo(codeFileName).Exists)
                        OpenFile(codeFileName);
                }
            }
        }

        private void CompletionRepository_MemberLookup(object sender, MemberLookupEventArgs args)
        {
            if (args.Member is Microsoft.CodeAnalysis.SyntaxNode)
            {
                Microsoft.CodeAnalysis.SyntaxNode node = args.Member as Microsoft.CodeAnalysis.SyntaxNode;
                if (node.IsKind(SyntaxKind.VariableDeclaration))
                {
                    args.Result = typeof(int);
                }
            }
        }

        private void CodeCompletionBox_SelectionChanged(object sender, EventArgs e)
        {
            string selectedItemName = (((ListBox)sender).SelectedItem != null) ? ((ListBox)sender).SelectedItem.ToString() : string.Empty;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            DelayedUpdateCodeWindows();
            updateTimer.Stop();
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            if (!project.HasProject)
                return;

            switch (project.DefaultExtension)
            {
                case "cs":
                    openFileDialog.FilterIndex = 1;
                    break;
                case "vb":
                    openFileDialog.FilterIndex = 2;
                    break;
            }

            if (openFileDialog.ShowDialog().Value)
            {
                var userCodeFileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                string xamlFileName = GetXamlFileName(userCodeFileName);
                if (!string.IsNullOrEmpty(xamlFileName) && File.Exists(xamlFileName))
                    addedFiles.Add(xamlFileName);
                addedFiles.Add(userCodeFileName);

                if (addedFiles.Count > 0)
                {
                    foreach (var file in addedFiles)
                    {
                        project.AddFile(file);
                        OpenFile(file);
                    }

                    CodeParsing.RegisterCode(project.ProjectExtension, GetSourceFiles(addedFiles));
                    UpdateProjectExplorer();
                    UpdateCodeWindows();
                }
            }
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (!project.HasProject)
                return;

            TreeViewItem node = GetNodeToRemove((TreeViewItem)tvSyntax.SelectedItem);
            IList<string> removedFiles = new List<string>();

            var fileName = GetFileNameFromNode(node);

            if (!string.IsNullOrEmpty(fileName))
            {
                removedFiles.Add(fileName);
            }

            for (int i = 0; i < node.Items.Count; i++)
            {
                fileName = GetFileNameFromNode((TreeViewItem)node.Items[i]);

                if (!string.IsNullOrEmpty(fileName))
                {
                    removedFiles.Add(fileName);
                }
            }

            if (removedFiles.Count > 0)
            {
                if (!ConfirmSaveBeforeClosing(GetModifiedFiles(removedFiles), true))
                    return;
                foreach (string file in removedFiles)
                {
                    project.RemoveFile(file);
                    CloseFile(file);
                }

                CodeParsing.UnregisterCode(project.ProjectExtension, GetSourceFiles(removedFiles));

                UpdateProjectExplorer();
                UpdateCodeWindows();
            }
        }

        private TreeViewItem GetNodeToRemove(TreeViewItem node)
        {
            if (node.Tag is FormNodeData)
            {
                TreeViewItem parent = GetParentItem(node);
                if ((parent != null) && (parent.Tag != null) && (parent.Tag is FormNodeData))
                {
                    FormNodeData tag = node.Tag as FormNodeData;
                    FormNodeData parentTag = parent.Tag as FormNodeData;
                    if ((tag.FormId == parentTag.FormId) && (tag.FileName == parentTag.FileName))
                        return parent;
                }
            }

            return node;
        }

        private void AddReference_Click(object sender, RoutedEventArgs e)
        {
            if (!project.HasProject)
                return;

            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = Path.GetDirectoryName(project.ProjectFileName);

            if (!dialog.ShowDialog().Value)
                return;

            var reference = dialog.FileName;
            if (project.AddReference(reference))
            {
                CodeParsing.RegisterAssemblies(project.ProjectExtension, new string[] { reference }, keepExisting: true);
                UpdateProjectExplorer();
                UpdateCodeWindows();
            }
        }

        private void RemoveReference_Click(object sender, RoutedEventArgs e)
        {
            if (!project.HasProject)
                return;

            TreeViewItem item = (TreeViewItem)tvSyntax.SelectedItem;
            if (IsReferenceNode(item))
            {
                var reference = item.Tag as DotNetProject.AssemblyReference;
                if (project.RemoveReference(reference.FullName))
                {
                    var references = project.References.Select(x => x.FullName).ToArray();
                    CodeParsing.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(references).ToArray());
                    UpdateProjectExplorer();
                    UpdateCodeWindows();
                }
            }
        }

        private void SyntaxTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TreeView)
            {
                ContextMenu popMenu = ((TreeView)sender).ContextMenu;
                bool enabledRef = project.HasProject && (tvSyntax.SelectedItem != null) && IsReferenceNode((TreeViewItem)tvSyntax.SelectedItem);
                bool enabledFile = project.HasProject && (tvSyntax.SelectedItem != null) && IsFileNode((TreeViewItem)tvSyntax.SelectedItem);

                if (popMenu != null)
                {
                    for (int i = popMenu.Items.Count - 1; i >= 0; i--)
                    {
                        if (popMenu.Items[i] is MenuItem)
                        {
                            MenuItem item = (MenuItem)popMenu.Items[i];
                            switch (item.Name)
                            {
                                case "cmiAddFile":
                                    item.IsEnabled = project.HasProject;
                                    break;
                                case "cmiAddReference":
                                    item.IsEnabled = project.HasProject;
                                    break;
                                case "cmiRemoveFile":
                                    item.IsEnabled = enabledFile;
                                    break;
                                case "cmiRemoveReference":
                                    item.IsEnabled = enabledRef;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private bool IsReferenceNode(TreeViewItem node)
        {
            TreeViewItem parent = GetParentItem(node);
            while (parent != null)
            {
                if (string.Compare(parent.Header.ToString(), "references", true) == 0)
                    return true;
                parent = GetParentItem(parent);
            }

            return false;
        }

        private TreeViewItem GetParentItem(TreeViewItem item)
        {
            for (var i = VisualTreeHelper.GetParent(item); i != null; i = VisualTreeHelper.GetParent(i))
            {
                if (i is TreeViewItem)
                    return (TreeViewItem)i;
            }

            return null;
        }

        private bool IsFileNode(TreeViewItem node)
        {
            string fileName = GetFileNameFromNode(node);

            return !string.IsNullOrEmpty(fileName) && !IsReferenceNode(node);
        }

        private string GetFileNameFromNode(TreeViewItem node)
        {
            var tag = node.Tag;

            string fileName = null;

            var formNodeData = tag as FormNodeData;
            if (formNodeData != null)
            {
                fileName = formNodeData.FileName;
            }
            else
                fileName = tag as string;

            return fileName;
        }

        #endregion Toolbar, Statusbar and event handlers

        #region Private Members

        private string RemoveFileExt(string fileName)
        {
            int p = fileName.LastIndexOf(".");
            return (p >= 0) ? fileName.Substring(0, p) : fileName;
        }

        private string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Name;
            }
            else
                return string.Empty;
        }

        private string ExtractFileExt(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Extension;
            }
            else
                return string.Empty;
        }

        private int FindLangByName(string name)
        {
            for (int i = 0; i < LangInfos.LangItems.Length; i++)
            {
                if (string.Compare(LangInfos.LangItems[i].FileType, name, true) == 0)
                    return i;
            }

            return -1;
        }

        private int FindLangByDesc(string desc, ref int index)
        {
            index = -1;
            for (int i = 0; i < LangInfos.LangItems.Length; i++)
            {
                if (!string.IsNullOrEmpty(LangInfos.LangItems[i].FileExt))
                {
                    index++;
                    if (string.Compare(LangInfos.LangItems[i].Description, desc, true) == 0)
                        return i;
                }
            }

            return -1;
        }

        private int FindLangByDesc(string desc)
        {
            for (int i = 0; i < LangInfos.LangItems.Length; i++)
            {
                if (string.Compare(LangInfos.LangItems[i].Description, desc, true) == 0)
                    return i;
            }

            return -1;
        }

        private int FindLangByExt(string ext)
        {
            int idx = -1;
            for (int i = 0; i < LangInfos.LangItems.Length; i++)
            {
                string fileExt = LangInfos.LangItems[i].FileExt;
                if (fileExt != string.Empty)
                {
                    idx++;
                    foreach (string s in fileExt.Split(new char[] { ';' }))
                    {
                        if (s != string.Empty && s.Substring(1).ToLower() == ext.ToLower())
                            return idx;
                    }
                }
            }

            return -1;
        }

        private string GetFileName(TabItem page)
        {
            TextEditor edit = GetEditor(page);
            if (edit != null && edit.Source != null)
                return edit.Source.FileName;
            return string.Empty;
        }

        private ILexer GetLexer(int index, out bool isSpecialScheme)
        {
            isSpecialScheme = true;
            LanguageInfo info = (index >= 0) && (index < LangInfos.LangItems.Length) ? LangInfos.LangItems[index] : new LanguageInfo(string.Empty, string.Empty, string.Empty);
            ILexer result = null;
            switch (info.FileType)
            {
                case "c":
                    result = new CParser();
                    break;

                case "c#":
                    result = new Alternet.Syntax.Parsers.Roslyn.CsParser();
                    break;

                case "java":
                    result = new JsParser();
                    break;

                case "vb_net":
                    result = new Alternet.Syntax.Parsers.Roslyn.VbParser();
                    FileInfo fileInfo = new FileInfo(dir + @"\VB.xml");
                    if (fileInfo.Exists)
                        ((Alternet.Syntax.Parsers.Roslyn.VbParser)result).CodeSnippets.LoadFile(fileInfo.FullName);
                    break;

                case "xml":
                    result = new XmlParser();
                    break;

                case "html":
                    result = new HtmlScriptParser();
                    break;

                case "java_script":
                    result = new JavaScriptParser();
                    break;

                case "vbs_script":
                    {
                        result = new VbScriptParser();
                        ((VbScriptParser)result).CompletionRepository.MemberLookup += new MemberLookupEvent(CompletionRepository_MemberLookup);
                        break;
                    }

                case "t4":
                    result = new T4Parser();
                    break;

                case "aspx":
                    result = new ASPNETParser();
                    break;

                case "CSS":
                    result = new CssParser();
                    break;

                default:
                    isSpecialScheme = false;
                    result = new Parser();
                    break;
            }

            if (result is ISyntaxParser)
            {
                ISyntaxParser sp = (ISyntaxParser)result;
                sp.Options |= SyntaxOptions.CodeCompletion;
            }

            return result;
        }

        private void TryOpenFile(string fileName)
        {
            bool found = false;
            foreach (TabItem page in editors.Keys)
            {
                string locName = GetFileName(page);
                if (string.Compare(fileName, locName, true) == 0)
                {
                    tcEditors.SelectedItem = page;
                    TextEditor edit = GetEditor(page);
                    if (edit != null)
                    {
                        if (!edit.IsFocused)
                            edit.Focus();
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
                OpenFile(fileName);
        }

        private void InitSearch()
        {
            SearchManager search = SearchManager.SharedSearch;

            search.InitSearch += new InitSearchEvent(SharedSearch_InitSearch);
            search.GetSearch += new GetSearchEvent(SharedSearch_GetSearch);
            search.TextFound += new TextFoundEvent(SharedSearch_TextFound);
        }

        private void InitEditor(TextEditor edit, ILexer lexer)
        {
            var popMenu = (ContextMenu)this.Resources["mainContextMenu"];
            edit.ContextMenu = popMenu;
            edit.Source.Lexer = lexer;
            edit.SourceStateChanged += new NotifyEvent(SourceStateChanged);
            edit.Selection.SelectionChanged += new EventHandler(SelectionChanged);
            edit.Source.BookMarks.Shared = true;
            edit.SearchGlobal = true;
            BookMarkManager.Register(edit.Source);
        }

        private TextEditor GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedItem as TabItem);
        }

        private void ActivateFindResultsTab()
        {
            tcBottom.SelectedIndex = FindResultTabIndex;
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            // updating status bar and toolbar buttons when changing selection in the editor
            UpdateControls(true);
            UpdateCodeWindows();
        }

        private void SourceStateChanged(object sender, NotifyEventArgs e)
        {
            string s = string.Empty;
            if ((e.State & NotifyState.Modified) != 0)
            {
                s = "Source was modified";
                UpdateEvents(false, s);
            }

            if ((e.State & NotifyState.ModifiedChanged) != 0)
            {
                s = string.Format("Source state changed: Modified={0}", ((TextEditor)sender).Source.Modified);
                UpdateEvents(false, s);
            }

            if ((e.State & NotifyState.OverWriteChanged) != 0)
            {
                s = string.Format("Text was formatted");
                UpdateEvents(false, s);
            }

            if ((e.State & NotifyState.TextParsed) != 0)
                DoReparse(null, new EventArgs());

            UpdateCodeWindows();
        }

        private void DoReparse(object sender, EventArgs e)
        {
            // text was fully parsed, updating explorer tree
            UpdateCodeWindows();
            UpdateEvents(false, "Text Reparsed");
        }

        private void OpenExplorer()
        {
            // opening explorer
            if (grExplorer.Visibility == Visibility.Visible)
                codeExplorerMenuItem.Header = string.Format(SOpenExplorer, "Open");
            else
                codeExplorerMenuItem.Header = string.Format(SOpenExplorer, "Close");
            grExplorer.Visibility = (grExplorer.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void DelayedUpdateCodeWindows()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                updateCount++;
                try
                {
                    CodeParsing.FillClasses(cbClasses, edit.Lexer as RoslynParser, edit.Position);
                    CodeParsing.FillMethods(cbMethods, edit.Lexer as RoslynParser, edit.Position, cbClasses);
                    FillErrors(edit.Lexer as ISyntaxParser);
                }
                finally
                {
                    updateCount--;
                }
            }
            else
            {
                cbClasses.ItemsSource = null;
                cbMethods.ItemsSource = null;
                FillErrors(null);
            }

            UpdateButtons();
        }

        private void UpdateCodeWindows()
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                updateTimer.Stop();
                updateTimer.Start();
            }
            else
                DelayedUpdateCodeWindows();
        }

        private void UpdateEvents(bool clear, string item)
        {
            if (clear)
                lbEvents.Items.Clear();
            if (item != string.Empty)
                lbEvents.Items.Add(item);
        }

        private void FillErrors(ISyntaxParser parser)
        {
            if (parser != null)
            {
                ObservableCollection<ListViewItemData> source = new ObservableCollection<ListViewItemData>();
                IList<ISyntaxError> list = new List<ISyntaxError>();
                parser.GetSyntaxErrors(list);
                TextEditor syntaxEdit = GetActiveSyntaxEdit();
                if (syntaxEdit != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ListViewItemData data = new ListViewItemData();
                        data.Description = list[i].Description;
                        data.Col = list[i].Position.X.ToString();
                        data.Line = list[i].Position.Y.ToString();
                        switch (list[i].ErrorType)
                        {
                            case SyntaxErrorType.Error:
                                data.Image = new BitmapImage(new Uri(string.Format("Images/{0}", "StatusCriticalError_12x_16x.png"), UriKind.Relative));
                                break;

                            case SyntaxErrorType.Warning:
                                data.Image = new BitmapImage(new Uri(string.Format("Images/{0}", "StatusWarning_12x_16x.png"), UriKind.Relative));
                                break;
                        }

                        data.Tag = list[i];
                        source.Add(data);
                    }
                }

                lvErrors.ItemsSource = source;
            }
            else
                lvErrors.ItemsSource = null;
        }

        private SourceText GetDocumentText(TextEditor edit)
        {
            if (edit.Lexer is RoslynParser)
            {
                Document document = ((RoslynParser)edit.Lexer).Repository.Document;
                SourceText text;
                return document != null && document.TryGetText(out text) ? text : null;
            }

            return null;
        }

        private void UpdateProjectExplorer()
        {
            if (project.HasProject)
                explorer.UpdateExplorer(project);
            else
                explorer.UpdateExplorer(null);
        }

        private void EnableContextMenuItem(MenuItem item, TextEditor edit, bool enabled, bool selection)
        {
            switch (item.Name)
            {
                case "findContextMenuItem":
                case "replaceContextMenuItem":
                case "gotoContextMenuItem":
                    item.IsEnabled = enabled && edit.Selection.CanCut();
                    break;

                case "cutContextMenuItem":
                    if (selection)
                        item.IsEnabled = enabled && edit.Selection.CanCut();
                    break;

                case "copyContextMenuItem":
                    if (selection)
                        item.IsEnabled = enabled && edit.Selection.CanCut();
                    break;

                case "pasteContextMenuItem":
                    if (selection)
                        item.IsEnabled = enabled && edit.Selection.CanCut();
                    break;
            }
        }

        private void UpdateMenu(bool selection)
        {
            // updating menu items
            TextEditor edit = GetActiveSyntaxEdit();
            bool enabled = edit != null;
            saveProjectMenuItem.IsEnabled = project.HasProject;
            closeProjectMenuItem.IsEnabled = project.HasProject;
            saveMenuItem.IsEnabled = enabled;
            saveAsMenuItem.IsEnabled = enabled;
            findMenuItem.IsEnabled = enabled;
            replaceMenuItem.IsEnabled = enabled;
            gotoMenuItem.IsEnabled = enabled;
            selectAllMenuItem.IsEnabled = enabled;
            undoMenuItem.IsEnabled = enabled && edit.Source.CanUndo();
            redoMenuItem.IsEnabled = enabled && edit.Source.CanRedo();
            if (selection)
            {
                cutMenuItem.IsEnabled = enabled && edit.Selection.CanCut();
                copyMenuItem.IsEnabled = enabled && edit.Selection.CanCopy();
                pasteMenuItem.IsEnabled = enabled && edit.Selection.CanPaste();
            }

            var popMenu = (ContextMenu)this.Resources["mainContextMenu"];
            if (popMenu != null)
            {
                for (int i = 0; i < popMenu.Items.Count; i++)
                {
                    if (popMenu.Items[i] is MenuItem)
                        EnableContextMenuItem((MenuItem)popMenu.Items[i], edit, enabled, selection);
                }
            }
        }

        private void UpdateToolBar()
        {
            this.openToolButton.Tag = openMenuItem;
            this.saveToolButton.Tag = saveMenuItem;
            this.cutToolButton.Tag = cutMenuItem;
            this.copyToolButton.Tag = copyMenuItem;
            this.pasteToolButton.Tag = pasteMenuItem;
            this.undoToolButton.Tag = undoMenuItem;
            this.redoToolButton.Tag = redoMenuItem;
            this.findToolButton.Tag = findMenuItem;
            this.replaceToolButton.Tag = replaceMenuItem;
            this.printPreviewToolButton.Tag = printPreviewMenuItem;
            this.printToolButton.Tag = printMenuItem;
        }

        private void UpdateStatusBar()
        {
            // updating status bar
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
            {
                sslPosition.Content = string.Format("Line: {0}, Char: {1}", syntaxEdit.Source.Position.Y, syntaxEdit.Source.Position.X);
                if (syntaxEdit.Source.ReadOnly)
                    sslModified.Content = "Readonly";
                else
                    if (syntaxEdit.Source.Modified)
                    sslModified.Content = "Modified";
                else
                    sslModified.Content = string.Empty;
                if (syntaxEdit.Source.Overwrite)
                    sslOverwrite.Content = "Overwrite";
                else
                    sslOverwrite.Content = " ";
            }
            else
            {
                sslPosition.Content = string.Empty;
                sslModified.Content = string.Empty;
                sslOverwrite.Content = " ";
            }
        }

        private void UpdateControls(bool selection)
        {
            UpdateToolBarButtons(selection);
            UpdateStatusBar();
            pnCombo.Visibility = (tcEditors.SelectedItem != null) ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateButtons()
        {
            UpdateToolBarButtons(true);
            UpdateControls(true);
        }

        private void UpdateToolBarButtons(bool selection)
        {
            // updating toolbar buttons and menu
            TextEditor syntaxEdit = GetActiveSyntaxEdit();
            bool enabled = syntaxEdit != null;
            bool canCut = enabled && syntaxEdit.Selection.CanCut();
            bool canCopy = enabled && syntaxEdit.Selection.CanCopy();
            bool canUndo = enabled && syntaxEdit.Source.CanUndo();
            bool canRedo = enabled && syntaxEdit.Source.CanRedo();
            bool canPaste = enabled && syntaxEdit.Selection.CanPaste();

            saveToolButton.IsEnabled = enabled;
            undoToolButton.IsEnabled = canUndo;
            redoToolButton.IsEnabled = canRedo;
            findToolButton.IsEnabled = enabled;
            replaceToolButton.IsEnabled = enabled;
            if (selection)
            {
                cutToolButton.IsEnabled = canCut;
                copyToolButton.IsEnabled = canCopy;
                pasteToolButton.IsEnabled = canPaste;
            }

            saveProjectMenuItem.IsEnabled = project.HasProject;
            closeProjectMenuItem.IsEnabled = project.HasProject;
            saveMenuItem.IsEnabled = enabled;
            saveAsMenuItem.IsEnabled = enabled && !FileBelongsToProject(syntaxEdit.Source.FileName);
            findMenuItem.IsEnabled = enabled;
            replaceMenuItem.IsEnabled = enabled;
            gotoMenuItem.IsEnabled = enabled;
            selectAllMenuItem.IsEnabled = enabled;
            undoMenuItem.IsEnabled = canUndo;
            redoMenuItem.IsEnabled = canRedo;
            if (selection)
            {
                cutMenuItem.IsEnabled = canCut;
                copyMenuItem.IsEnabled = canCopy;
                pasteMenuItem.IsEnabled = canPaste;
            }

            var popMenu = (ContextMenu)this.Resources["mainContextMenu"];
            if (popMenu != null)
            {
                for (int i = 0; i < popMenu.Items.Count; i++)
                {
                    if (popMenu.Items[i] is MenuItem)
                        EnableContextMenuItem((MenuItem)popMenu.Items[i], syntaxEdit, enabled, selection);
                }
            }
        }

        private bool CanGotoDefinition()
        {
            bool result = false;
            TextEditor edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                SymbolLocation location = parser.FindDeclaration(edit.Position);
                return location != null;
            }

            return result;
        }

        private System.Drawing.Point GetPositionFromPos(SourceText text, int pos)
        {
            LinePosition linePosition = text.Lines.GetLinePosition(pos);
            return new System.Drawing.Point(linePosition.Character, linePosition.Line);
        }

        private System.Drawing.Point GetPositionFromPos(TextEditor edit, int pos)
        {
            SourceText text = GetDocumentText(edit);
            if (text == null)
                return System.Drawing.Point.Empty;
            LinePosition linePosition = text.Lines.GetLinePosition(pos);
            return new System.Drawing.Point(linePosition.Character, linePosition.Line);
        }

        #endregion Private Members

        private class ListViewItemData
        {
            private string description;
            private string col;
            private string line;
            private object tag;

            public string Description
            {
                get
                {
                    return description;
                }

                set
                {
                    description = value;
                }
            }

            public string Col
            {
                get
                {
                    return col;
                }

                set
                {
                    col = value;
                }
            }

            public string Line
            {
                get
                {
                    return line;
                }

                set
                {
                    line = value;
                }
            }

            public object Tag
            {
                get
                {
                    return tag;
                }

                set
                {
                    tag = value;
                }
            }

            public BitmapSource Image { get; set; }
        }
    }
}
