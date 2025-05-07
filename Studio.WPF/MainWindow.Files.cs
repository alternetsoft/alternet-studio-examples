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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter.Integration.Wpf;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private IDictionary<TabItem, IScriptEdit> editors = new Dictionary<TabItem, IScriptEdit>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();

        private string startupDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ContextMenu editorsMenu = new ContextMenu();
        private MenuItem miCloseEditor;
        private MenuItem miCloseAllEditors;

        private IScriptEdit ActiveSyntaxEdit
        {
            get
            {
                if (editorsTabControl.Items.Count == 0)
                    return null;

                return GetEditor(editorsTabControl.SelectedItem as TabItem);
            }
        }

        protected virtual void LoadStartupFile()
        {
            openFileDialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|CS script files (*.csx) |*.csx| All project files (*.csproj; *.vbproj; *.sln)|*.csproj; *.vbproj; *.sln| Any files (*.*) | *.*";
            saveFileDialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|CS script files (*.csx) |*.csx| All files (*.*)|*.*";

            if (new DirectoryInfo(startupDirectory).Exists)
            {
                openFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);
                saveFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);

                var projectFile = Path.GetFullPath(Path.Combine(startupDirectory, @"Debugger\cs\HelloWorld.Wpf\HelloWorld.Wpf.csproj"));
                //var projectFile = "C:\\develop\\git\\AlternetStudio\\Demo\\Resources\\Debugger\\CS\\MauiTest.Net9\\MauiApp2.csproj";
                if (File.Exists(projectFile))
                    OpenProject(projectFile);
            }
        }

        protected virtual void InitMenuIcons(ContextMenu menu)
        {
            IDictionary<string, string> menuIcons = new Dictionary<string, string>();
            menuIcons.Add(StringConsts.MenuUndoCaption, "Undo");
            menuIcons.Add(StringConsts.MenuRedoCaption, "Redo");
            menuIcons.Add(StringConsts.MenuCutCaption, "Cut");
            menuIcons.Add(StringConsts.MenuCopyCaption, "Copy");
            menuIcons.Add(StringConsts.MenuPasteCaption, "Paste");
            menuIcons.Add(StringConsts.MenuSelectAllCaption, "SelectAll");
            menuIcons.Add(StringConsts.SearchCaption, "FindInFile");
            menuIcons.Add(StringConsts.ReplaceCaption.Replace("&", string.Empty), "ReplaceInFiles");
            menuIcons.Add(StringConsts.GotoDefinition, "GoToDefinition");

            foreach (var item in menu.Items)
            {
                if (item is MenuItem)
                {
                    MenuItem menuItem = item as MenuItem;
                    if (menuIcons.ContainsKey(menuItem.Header.ToString()))
                    {
                        string imageName = menuIcons[menuItem.Header.ToString()];
                        menuItem.Icon = LoadImage(imageName);
                    }
                }
            }
        }

        protected bool IsXamlCodeBehindFile(string fileName, out string formId)
        {
            return FormFilesUtility.IsXamlCodeBehindFile(fileName, out formId);
        }

        protected virtual IScriptEdit NewFile(string fileName)
        {
            TabItem page = new TabItem();
            page.Header = new TextBlock { Text = Path.GetFileName(fileName), ToolTip = fileName };

            editorsTabControl.Items.Add(page);
            editorsTabControl.SelectedItem = page;

            var edit = CreateDebugEdit();
            editors.Add(page, edit);

            edit.DefaultMenu.Opened += ContextMenu_Opened;
            InitMenuIcons(edit.DefaultMenu);

            string formId;
            var isXamlCodeBehindFile = IsXamlCodeBehindFile(fileName, out formId);
            if (isXamlCodeBehindFile)
            {
                var source = GetDesignerSource(formId);
                FormDesignerEditorHelpers.SetEditorSource(edit, fileName, source);

                if (!HasProject())
                {
                    var generatedCodeFileName = XamlGeneratedCodeFileService.GetGeneratedCodeFile(null, source);
                    CodeEditExtensions.RegisterCode(edit, generatedCodeFileName, File.ReadAllText(generatedCodeFileName));
                }
            }
            else
            {
                if (File.Exists(fileName))
                {
                    edit.LoadFile(fileName);
                    edit.FileName = fileName;
                }
            }

            var projectName = GetProjectName(fileName);
            edit.SetFileNameAndProject(fileName, projectName);

            edit.TextChanged += (o, e) =>
            {
                if (editorsTabControl.SelectedItem == page)
                    editedCodeFiles.Add(fileName);
                UpdatePage(edit.Parent as TabItem, edit.FileName, edit.Modified);
                UpdateCodeNavigation();
            };

            edit.BeforeNavigateToDeclaration += (o, e) =>
            {
                var edt = o as IScriptEdit;
                if (edt != null)
                    navigationHistory.SaveCurrentLocationToHistory(edt.Position, edt.FileName, edt.GetLine(edt.Position.Y));
                navigationHistory.UpdateHistory(backwardMenu.ContextMenu.Items, 0, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            };

            edit.AfterNavigateToDeclaration += (o, e) =>
            {
                var edt = o as IScriptEdit;
                if (edt != null)
                    navigationHistory.SaveCurrentLocationToHistory(edt.Position, edt.FileName, edt.GetLine(edt.Position.Y));
                navigationHistory.UpdateHistory(backwardMenu.ContextMenu.Items, 0, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            };

            edit.StatusChanged += new EventHandler(CodeEdit_StatusChanged);
            page.Content = edit;

            FileInfo fileInfo = new FileInfo(fileName);

            RegisterDesignerImportsInEditor(fileName, edit);

            edit.HighlightReferences = true;
            edit.OpenSharedEditorFunc = OpenSharedEditorFunc;

            editorsTabControl.SelectedItem = page;
            UpdateSearch();
            UpdateBookmarks();
            UpdateCodeNavigation();
            edit.UpdateBreakpoints();
            return edit;
        }

        protected IScriptEdit OpenFile(string fileName)
        {
            return OpenFile(fileName, false);
        }

        protected virtual void InitEditorsContextMenu()
        {
            miCloseEditor = new MenuItem();
            miCloseEditor.Name = "cmiCloseEditor";
            miCloseEditor.Header = StringConsts.CloseTab;
            miCloseEditor.Click += CloseEditor_Click;

            miCloseAllEditors = new MenuItem();
            miCloseAllEditors.Name = "cmiCloseAllEditors";
            miCloseAllEditors.Header = StringConsts.CloseAllTabs;
            miCloseAllEditors.Click += CloseAllEditors_Click;

            editorsMenu.Items.Add(miCloseEditor);
            editorsMenu.Items.Add(miCloseAllEditors);
            editorsTabControl.ContextMenu = editorsMenu;
        }

        private void InitializeEditors()
        {
            CodeEditExtensions.InitSyntax();
        }

        private void LocateStartupDirectory()
        {
            if (!new DirectoryInfo(startupDirectory + "Resources").Exists)
                startupDirectory = Path.GetFullPath(startupDirectory + @"..\..\..\..\");
            startupDirectory = startupDirectory + @"Resources\";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!ConfirmSaveBeforeClosing())
            {
                e.Cancel = true;
                return;
            }

            codeNavigationBar.Dispose();
            codeNavigationBar = null;
        }

        private void SetCaretToMethod(string userCodeFileName, string methodName)
        {
            string toFind;
            var cs = Path.GetExtension(userCodeFileName) == ".cs";
            if (cs)
                toFind = "void " + methodName;
            else
                toFind = "Sub " + methodName;

            var editor = ActiveSyntaxEdit;

            System.Drawing.Point oldPosition = editor.Position;

            editor.Position = new System.Drawing.Point();
            if (editor.Find(toFind))
            {
                editor.MoveToLine(editor.Position.Y + (cs ? 2 : 1));
                editor.MoveLineEnd();
            }
            else
                editor.Position = oldPosition;
        }

        private void EditorsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!projectIsClosing)
            {
                UpdateCodeNavigation();
                UpdateDesignerControls();
                navigationHistory.ClearCurrentHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            }
        }

        private IScriptEdit GetEditor(TabItem key)
        {
            IScriptEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private bool ContainsFile(IList<string> files, string fileName)
        {
            if (files == null || string.IsNullOrEmpty(fileName))
                return true;
            foreach (var file in files)
            {
                if (string.Compare(file, fileName, true) == 0)
                    return true;
            }

            return false;
        }

        private bool ConfirmSaveBeforeClosing(string fileName)
        {
            IList<string> files = new List<string>();
            files.Add(fileName);
            return ConfirmSaveBeforeClosing(files, false);
        }

        private IList<string> GetDisplayFiles(IList<string> files, bool groupPerProject)
        {
            IList<string> result = new List<string>();
            IList<string> newFiles = new List<string>();

            foreach (var file in files)
                newFiles.Add(file);

            if (groupPerProject)
            {
                if (!solution.IsEmpty)
                {
                    foreach (var proj in solution.Projects)
                        GetDisplayFilesForProject(result, proj, newFiles);
                }
                else
                if (Project.HasProject)
                {
                    GetDisplayFilesForProject(result, Project, newFiles);
                }
            }

            foreach (var file in newFiles)
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

                DlgConfirmSave dlg = new DlgConfirmSave(productName, displayFiles);

                if (dlg.ShowDialog().Value)
                {
                    SaveModifiedFiles(files);
                }

                confirm = dlg.Confirm;
            }

            return confirm;
        }

        private void AddFile(IList<string> files, string fileName)
        {
            if (ContainsFile(files, fileName))
                return;
            files.Add(fileName);
        }

        private void AddFiles(IList<string> files, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            AddFile(files, fileName);

            string xamlFileName = GetXamlFileName(fileName);
            if (!string.IsNullOrEmpty(xamlFileName) && File.Exists(xamlFileName))
                AddFile(files, xamlFileName);
        }

        private IList<string> GetModifiedFiles(IList<string> files)
        {
            IList<string> result = new List<string>();

            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                IScriptEdit edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    if (!ContainsFile(files, edit.FileName))
                        continue;

                    AddFiles(result, edit.FileName);
                }
            }

            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                IFormDesignerControl designer;
                if (formDesigners.TryGetValue(tabPage, out designer))
                {
                    if (designer.Source.IsModified)
                    {
                        string fileName = ((IFormDesignerDataSource)designer.Source).UserCodeFileName;
                        if (!ContainsFile(files, fileName))
                            continue;
                        AddFiles(result, fileName);
                    }
                }
            }

            if (!solution.IsEmpty)
            {
                if (solution.IsModified)
                {
                    if (ContainsFile(files, solution.SolutionFileName))
                        AddFiles(result, solution.SolutionFileName);
                }

                foreach (var proj in solution.Projects)
                {
                    if (proj.IsModified)
                    {
                        if (ContainsFile(files, proj.ProjectFileName))
                            AddFiles(result, proj.ProjectFileName);
                    }
                }
            }
            else
            if (Project.HasProject && Project.IsModified)
            {
                if (ContainsFile(files, Project.ProjectFileName))
                    AddFiles(result, Project.ProjectFileName);
            }

            return result;
        }

        private bool ConfirmSaveBeforeClosing()
        {
            var list = GetModifiedFiles(null);
            return ConfirmSaveBeforeClosing(list, true);
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = FindUniqueName("Class", ".cs");

            if (saveFileDialog.ShowDialog(Window.GetWindow(this)).Value != true)
                return;

            using (var stringWriter = new StringWriter())
            {
                using (var streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    var sb = stringWriter.GetStringBuilder();

                    streamWriter.Write(sb);
                }
            }

            OpenFile(saveFileDialog.FileName, true);
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

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog(Window.GetWindow(this)).Value == true)
            {
                var fileName = openFileDialog.FileName;
                string xamlName = GetXamlFileName(fileName);
                if (FormFilesUtility.CheckIfFormFilesExist(xamlName))
                    OpenDesigner(xamlName);
                else
                    OpenFile(fileName);
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IScriptEdit edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.FileName != string.Empty)
                {
                    edit.SaveFile(edit.FileName);
                    UpdatePage(edit.Parent as TabItem, edit.FileName, edit.Modified);
                }
                else
                    SaveFileAs(edit);
                if (FileBelongsToSolution(edit.FileName))
                {
                    SaveBreakpoints(GetBreakpointFile(solution));
                    SaveBookmarks(GetBookmarkFile(solution));
                }
                else
                if ((Project != null) && Project.HasProject && FileBelongsToProject(Project, edit.FileName))
                {
                    SaveBreakpoints(GetBreakpointFile(Project));
                    SaveBookmarks(GetBookmarkFile(Project));
                }
            }
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    SaveDesignerFiles(designer);
            }
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IScriptEdit edit = ActiveSyntaxEdit;
            if (edit != null)
                SaveFileAs(edit);
        }

        private void SaveAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveAllModifiedFiles();
            if ((solution != null) && !solution.IsEmpty)
            {
                SaveBreakpoints(GetBreakpointFile(solution));
                SaveBookmarks(GetBookmarkFile(solution));
            }
            else
            if ((Project != null) && Project.HasProject)
            {
                SaveBreakpoints(GetBreakpointFile(Project));
                SaveBookmarks(GetBookmarkFile(Project));
            }
        }

        private string GetBreakpointFile(DotNetProject project)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.ProjectFileName), project.ProjectName + ".Breakpoints.xml"));
        }

        private string GetBreakpointFile(DotNetSolution solution)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solution.SolutionFileName), solution.SolutionName + ".Breakpoints.xml"));
        }

        private string GetBookmarkFile(DotNetProject project)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.ProjectFileName), project.ProjectName + ".Bookmarks.xml"));
        }

        private string GetBookmarkFile(DotNetSolution solution)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solution.SolutionFileName), solution.SolutionName + ".Bookmarks.xml"));
        }

        private void LoadBreakpoints(string fileName)
        {
            if (File.Exists(fileName))
            {
                Debugger.Breakpoints.LoadFile(fileName);
            }
        }

        private void LoadBookmarks(string fileName)
        {
            if (File.Exists(fileName))
            {
                BookMarkManager.SharedBookMarks.LoadFile(fileName);
            }
        }

        private void SaveBreakpoints(string fileName)
        {
            Debugger.Breakpoints.SaveFile(fileName);
        }

        private void SaveBookmarks(string fileName)
        {
            BookMarkManager.SharedBookMarks.SaveFile(fileName);
        }

        private bool SaveAllModifiedFiles()
        {
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                IScriptEdit edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    edit.SaveFile(edit.FileName);
                    UpdatePage(tabPage, edit.FileName, edit.Modified);
                }
            }

            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                IFormDesignerControl designer;
                if (formDesigners.TryGetValue(tabPage, out designer))
                {
                    SaveDesignerFiles(designer);
                }
            }

            if (SaveAllProjects())
                UpdateProjectExplorer();

            return true;
        }

        private bool IsResourceFileName(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".resx", true) == 0 || string.Compare(ext, ".xaml", true) == 0;
        }

        private bool IsSourceFile(string fileName, string extension)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, extension) == 0;
        }

        private bool FileBelongsToProjectFramework(DotNetProject project, string fileName)
        {
            bool IsPlatform(string path, out string platform)
            {
                platform = string.Empty;
                string name = string.Empty;
                DirectoryInfo info = new DirectoryInfo(path);
                while (info != null)
                {
                    name = info.Name;
                    if (string.Compare(name, "platforms", true) == 0)
                        return true;
                    else
                        platform = name;
                    info = info.Parent;
                }

                return false;
            }

            var framework = project.HasProject ? project.TargetFramework : null;

            if (framework == null)
                return true;

            if (Path.IsPathRooted(fileName))
            {
                var path = Path.GetDirectoryName(fileName);
                if (IsPlatform(path, out string platform))
                    return framework.DisplayName.Equals(platform, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private string[] GetSourceFiles(DotNetProject project, IList<string> files, string extension, bool includeAutoGenerated)
        {
            IList<string> sourceFiles = new List<string>();
            foreach (var file in files)
            {
                if (!IsResourceFileName(file) && IsSourceFile(file, extension) && FileBelongsToProjectFramework(project, file))
                    sourceFiles.Add(file);

                string formId;
                if (includeAutoGenerated && IsXamlCodeBehindFile(file, out formId))
                {
                    sourceFiles.Add(
                        XamlGeneratedCodeFileService.GetGeneratedCodeFile(project, GetDesignerSource(formId)));
                }
            }

            return sourceFiles.ToArray();
        }

        private IScriptEdit FindFile(string fileName)
        {
            if (!PathUtilities.IsPathFullyQualified(fileName))
                return null;

            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                var scriptEdit = GetEditor(tabPage);
                if (scriptEdit != null)
                {
                    var path = scriptEdit.FileName;
                    path = new Uri(path).LocalPath;
                    if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                        return scriptEdit;
                }
            }

            return null;
        }

        private TextEditor OpenSharedEditorFunc(string fileName)
        {
            bool fullPath = PathUtilities.IsPathFullyQualified(fileName);
            if (fullPath)
            {
                var edit = FindFile(fileName);
                if (edit != null)
                    return edit as TextEditor;
            }

            if (Project != null && Project.HasProject)
            {
                if (!fullPath)
                    fileName = Path.Combine(Path.GetDirectoryName(Project.ProjectFileName), fileName);

                if (!FileBelongsToProject(Project, fileName))
                    Project.AddFile(fileName, BuildAction.Compile);
            }

            return OpenFile(fileName) as TextEditor;
        }

        private DebugCodeEdit CreateDebugEdit()
        {
            var edit = new DebugCodeEdit();
            InitCodeEdit(edit);
            InitDebugEdit(edit);
            return edit;
        }

        private void InitCodeEdit(IDebugEdit edit)
        {
            edit.GotoDefinition += DebugEdit_GoToDefinition;
            edit.FindAllReferences += DebugEdit_FindAllReferences;
            edit.FindAllImplementations += DebugEdit_FindAllImplementations;
        }

        private void RegisterDesignerImportsInEditor(string fileName, DebugCodeEdit edit)
        {
            var namespaces = GetImportedNamespaces(fileName);
            var rootNamespace = GetRootNamespce(fileName);
            if (namespaces != null)
                edit.RegisterNamespaces(namespaces, rootNamespace);
        }

        private void CodeEdit_StatusChanged(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit == null || sender != edit)
                return;

            var args = e as NotifyEventArgs;

            bool update = args != null && ((args.State & NotifyState.Edit) != 0 || (args.State & NotifyState.Modified) != 0 || (args.State & NotifyState.TextParsed) != 0);
            bool reparse = args != null && (args.State & NotifyState.TextParsed) != 0;

            UpdateCodeNavigation(update);

            if (reparse)
                UpdateErrors(edit);

            if (args != null)
            {
                if ((args.State & NotifyState.PositionChanged) != 0)
                {
                    navigationHistory.ClearCurrentHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
                }

                if ((args.State & NotifyState.OverWriteChanged) != 0 || (args.State & NotifyState.PositionChanged) != 0)
                    UpdateStatusBar();
            }
        }

        private void DebugEdit_FindAllImplementations(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                FindAllImplementations(edit);
        }

        private void DebugEdit_FindAllReferences(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                FindAllReferences(edit);
        }

        private void DebugEdit_GoToDefinition(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                GoToDefinition(edit);
        }

        private IScriptEdit OpenFile(string fileName, bool forceReopen)
        {
            try
            {
                IScriptEdit edit = FindFile(fileName);
                if (edit != null && edit.Parent is TabItem)
                {
                    if (forceReopen)
                    {
                        if (edit.Modified && !ConfirmSaveBeforeClosing(edit.FileName))
                            return edit;
                        CloseFile(edit.FileName);
                    }
                    else
                    {
                        editorsTabControl.SelectedItem = (TabItem)edit.Parent;
                        return edit;
                    }
                }

                edit = NewFile(fileName);

                if (!HasProject())
                {
                    CodeEditExtensions.RegisterDefaultAssemblies(edit);
                    AddDesignFileForParsing(fileName);
                }

                UpdateControls();
                return edit;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        private void SetActiveEdit(IScriptEdit edit)
        {
            if (edit != null && edit.Parent is TabItem)
                editorsTabControl.SelectedItem = (TabItem)edit.Parent;
        }

        private void CloseFile(string fileName)
        {
            IScriptEdit edit = FindFile(fileName);
            if (edit != null)
            {
                edit.FileName = string.Empty;
                TabItem page = edit.Parent as TabItem;
                if (page != null)
                {
                    editorsTabControl.Items.Remove(page);
                    if (editors.ContainsKey(page))
                        editors.Remove(page);
                }
            }
        }

        private void CloseFile(IScriptEdit edit)
        {
            var fileName = edit.FileName;

            CodeEditExtensions.UnregisterCode(Path.GetExtension(fileName), new string[] { fileName });
            if (!HasProject())
                RemoveDesignFileForParsing(fileName);

            string formId;
            if (IsXamlCodeBehindFile(fileName, out formId))
            {
                if (FindFile(formId) == null && FindDesigner(formId) == null)
                {
                    if (sourcesByFormId.ContainsKey(formId))
                        sourcesByFormId.Remove(formId);
                }
            }

            if (!FileBelongsToProject(fileName))
                edit.FileName = string.Empty;
        }

        private void UpdatePage(TabItem page, string fileName, bool isModified = false)
        {
            if (page == null)
                return;
            string pageText = Path.GetFileName(fileName);

            pageText = isModified ? pageText + "*" : pageText;
            UpdatePageHeader(page, pageText, fileName);
        }

        private IScriptEdit GetEditor(int index)
        {
            if (editorsTabControl.Items.Count == 0)
                return null;

            return GetEditor((TabItem)editorsTabControl.Items[index]);
        }

        private IFormDesignerControl GetFormDesigner(int index)
        {
            if (editorsTabControl.Items.Count == 0)
                return null;

            IFormDesignerControl designer;
            if (!formDesigners.TryGetValue((TabItem)editorsTabControl.Items[index], out designer))
                return null;

            return designer;
        }

        private bool RemovePage(int index)
        {
            var tab = index >= 0 & index < editorsTabControl.Items.Count ? (TabItem)editorsTabControl.Items[index] : null;
            if (tab == null)
                return false;

            var edit = GetEditor(index);

            if (edit != null)
            {
                if (edit.Modified && !ConfirmSaveBeforeClosing(edit.FileName))
                    return false;
                CloseFile(edit);
            }
            else
            {
                var designer = GetFormDesigner(index);
                if (designer != null)
                {
                    if (designer.Source.IsModified && !ConfirmSaveBeforeClosing(((IFormDesignerDataSource)designer.Source).XamlFileName))
                        return false;
                    CloseDesigner(designer);
                }
            }

            if (formDesigners.ContainsKey(tab))
                formDesigners.Remove(tab);
            if (editors.ContainsKey(tab))
                editors.Remove(tab);
            editorsTabControl.Items.Remove(tab);
            return true;
        }

        private void RemovePage()
        {
            RemovePage(editorsTabControl.SelectedIndex);
            UpdateControls();
            UpdateCodeNavigation();
        }

        private void RemoveAllPages()
        {
            bool result = true;
            try
            {
                while (editorsTabControl.Items.Count > 0)
                {
                    result = RemovePage(editorsTabControl.Items.Count - 1);
                    if (!result)
                        break;
                }
            }
            finally
            {
                if (result)
                    UpdateCodeNavigation();
            }
        }

        private void CloseEditor_Click(object sender, RoutedEventArgs e)
        {
            RemovePage();
        }

        private void CloseAllEditors_Click(object sender, RoutedEventArgs e)
        {
            RemoveAllPages();
        }

        private bool SaveFileAs(IScriptEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            string oldFileName = edit.FileName;
            if (!string.IsNullOrEmpty(edit.FileName))
            {
                saveFileDialog.FileName = edit.FileName;
                string extension = Path.GetExtension(edit.FileName).ToLower();
                switch (extension)
                {
                    case ".vb":
                        saveFileDialog.FilterIndex = 2;
                        break;
                    case ".csx":
                        saveFileDialog.FilterIndex = 3;
                        break;
                }
            }

            if (saveFileDialog.ShowDialog(Window.GetWindow(this)).Value != true)
                return false;

            string fileName = saveFileDialog.FileName;

            if (oldFileName != fileName)
            {
                EditorFormDesignerDataSource ds = GetDesignerSource(oldFileName, false);
                if (ds != null)
                    UpdateFormFiles(edit, ds, fileName);
                else
                {
                    edit.SaveFile(fileName);
                    edit.FileName = fileName;
                    UpdatePage((TabItem)editorsTabControl.SelectedItem, fileName);
                }
            }

            return true;
        }

        private void CloseFileMenuItem_Click(object sender, EventArgs e)
        {
            RemovePage();
        }

        private bool SaveModifiedFiles(IList<string> files)
        {
            IList<IScriptEdit> editors = new List<IScriptEdit>();
            IList<Tuple<IFormDesignerControl, TabItem>> designerTabs = new List<Tuple<IFormDesignerControl, TabItem>>();

            foreach (string file in files)
            {
                if (IsSolutionFile(file))
                {
                    if (solution != null && !solution.IsEmpty)
                        solution.Save();

                    continue;
                }

                if (IsProjectFile(file))
                {
                    var project = FindProject(file);
                    if (project != null)
                        project.Save();

                    continue;
                }

                IScriptEdit edit = FindFile(file);
                if (edit != null)
                {
                    if (edit.Modified)
                        editors.Add(edit);
                }
                else
                {
                    var designerTab = FindDesigner(file);
                    if (designerTab != null)
                    {
                        var designer = designerTab.Item1;
                        if (designer.Source.IsModified)
                            designerTabs.Add(designerTab);
                    }
                }
            }

            foreach (var edit in editors)
            {
                edit.SaveFile(edit.FileName);
                UpdatePage(edit.Parent as TabItem, edit.FileName, edit.Modified);
            }

            foreach (var designerTab in designerTabs)
            {
                var designer = designerTab.Item1;
                designer.Save();
                UpdateDesignPage(designerTab.Item2, ((IFormDesignerDataSource)designer.Source).XamlFileName, designer.Source.IsModified);
            }

            return true;
        }
    }
}