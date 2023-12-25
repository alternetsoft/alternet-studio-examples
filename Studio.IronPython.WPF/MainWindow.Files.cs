#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.IronPython.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Syntax.Parsers.Python;

namespace AlternetStudio.IronPython.Wpf.Demo
{
    public partial class MainWindow
    {
        private IDictionary<TabItem, ScriptCodeEdit> editors = new Dictionary<TabItem, ScriptCodeEdit>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();

        private string startupDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ContextMenu editorsMenu = new ContextMenu();
        private MenuItem miCloseEditor;
        private MenuItem miCloseAllEditors;

        private ScriptCodeEdit ActiveSyntaxEdit
        {
            get
            {
                if (editorsTabControl.Items.Count == 0)
                    return null;

                return GetEditor(editorsTabControl.SelectedItem as TabItem);
            }
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

        protected virtual void LoadStartupFile()
        {
            openFileDialog.Filter = "Python files (*.py) |*.py| All project files (*.pyproj)|*.pyproj| Any files (*.*) | *.*";
            saveFileDialog.Filter = "Python files (*.py) |*.py| All project files (*.pyproj)|*.pyproj| Any files (*.*) | *.*";

            if (new DirectoryInfo(startupDirectory).Exists)
            {
                openFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);
                saveFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);

                var projectFile = Path.GetFullPath(Path.Combine(startupDirectory, @"Debugger.IronPython\Project.pyproj"));
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

        protected virtual void SetMenuIcon(MenuItem item, string uri)
        {
            item.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(uri, UriKind.Relative)),
            };
        }

        protected virtual IScriptEdit NewFile(string fileName)
        {
            TabItem page = new TabItem();
            page.Header = new TextBlock { Text = Path.GetFileName(fileName), ToolTip = fileName };

            editorsTabControl.Items.Add(page);
            editorsTabControl.SelectedItem = page;

            var edit = CreateDebugEdit();
            edit.AllowedActions &= ~(AllowedActions.SetNextStatement | AllowedActions.FindAllImplementations);
            editors.Add(page, edit);
            edit.DefaultMenu.Opened += ContextMenu_Opened;
            InitMenuIcons(edit.DefaultMenu);

            edit.AllowedActions = edit.AllowedActions & ~AllowedActions.SetNextStatement;

            if (File.Exists(fileName))
                edit.LoadFile(fileName);

            var parser = new IronPythonParser();
            parser.CodeEnvironment = scriptRun.CodeEnvironment;
            edit.Lexer = parser;

            if (Debugger.IsStarted)
                edit.ReadOnly = true;

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
            if (fileInfo.Exists)
            {
                edit.LoadFile(fileName);
                edit.FileName = fileName;
            }

            edit.HighlightReferences = true;

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

        private void InitializeEditors()
        {
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

        private void EditorsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!projectIsClosing)
            {
                UpdateCodeNavigation();
                navigationHistory.ClearCurrentHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            }
        }

        private ScriptCodeEdit GetEditor(TabItem key)
        {
            ScriptCodeEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
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

            if (groupPerProject && Project.HasProject)
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
            AddFile(files, fileName);
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

                    AddFile(result, edit.FileName);
                }
            }

            if (Project.HasProject && Project.IsModified)
            {
                if (ContainsFile(files, Project.ProjectFileName))
                    AddFile(result, Project.ProjectFileName);
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
            saveFileDialog.FileName = FindUniqueName("Class", ".ts");

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

        private string FindUniqueName(string baseName, string extesion)
        {
            int count = 1;
            string result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            while (!IsNameUnique(result))
            {
                count++;
                result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            }

            return result + extesion;
        }

        private bool IsNameUnique(string baseName)
        {
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                if (tabPage.Header.ToString().StartsWith(baseName, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
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
                OpenFile(openFileDialog.FileName);
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
                if ((Project != null) && Project.HasProject && FileBelongsToProject(edit.FileName))
                {
                    SaveBreakpoints(GetBreakpointFile(Project));
                    SaveBookmarks(GetBookmarkFile(Project));
                }
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
            if ((Project != null) && Project.HasProject)
            {
                SaveBreakpoints(GetBreakpointFile(Project));
                SaveBookmarks(GetBookmarkFile(Project));
            }
        }

        private string GetBreakpointFile(IronPythonProject project)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.ProjectFileName), project.ProjectName + ".Breakpoints.xml"));
        }

        private string GetBookmarkFile(IronPythonProject project)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.ProjectFileName), project.ProjectName + ".Bookmarks.xml"));
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
                }
            }

            if (Project.HasProject && Project.IsModified)
            {
                Project.Save();
                scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            }

            return true;
        }

        private string[] GetSourceFiles(IList<string> files)
        {
            IList<string> sourceFiles = new List<string>();
            foreach (var file in files)
            {
                sourceFiles.Add(file);
            }

            return sourceFiles.ToArray();
        }

        private IScriptEdit FindFile(string fileName)
        {
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

        private void CodeEdit_StatusChanged(object sender, EventArgs e)
        {
            if (sender != ActiveSyntaxEdit)
                return;

            var args = e as NotifyEventArgs;

            bool update = args != null && ((args.State & NotifyState.Edit) != 0 || (args.State & NotifyState.Modified) != 0 || (args.State & NotifyState.TextParsed) != 0);

            UpdateCodeNavigation(update);

            if (args != null)
            {
                if ((args.State & NotifyState.PositionChanged) != 0)
                    navigationHistory.ClearCurrentHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);

                if ((args.State & NotifyState.OverWriteChanged) != 0 || (args.State & NotifyState.PositionChanged) != 0)
                    UpdateStatusBar();
            }
        }

        private void DebugEdit_FindAllImplementations(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
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
            if (fileName == string.Empty)
                return null;
            IScriptEdit edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabItem))
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

            UpdateControls();
            return edit;
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

        private void CloseEditor_Click(object sender, RoutedEventArgs e)
        {
            RemovePage();
        }

        private void CloseAllEditors_Click(object sender, RoutedEventArgs e)
        {
            RemoveAllPages();
        }

        private IScriptEdit GetEditor(int index)
        {
            if (editorsTabControl.Items.Count == 0)
                return null;

            return GetEditor((TabItem)editorsTabControl.Items[index]);
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

            if (editors.ContainsKey(tab))
                editors.Remove(tab);
            editorsTabControl.Items.Remove(tab);
            return true;
        }

        private void RemovePage()
        {
            RemovePage(editorsTabControl.SelectedIndex);
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

        private bool SaveFileAs(IScriptEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            if (!string.IsNullOrEmpty(edit.FileName))
            {
                saveFileDialog.FileName = edit.FileName;
                string extenstion = Path.GetExtension(edit.FileName).ToLower();
                switch (extenstion)
                {
                    case ".jb":
                        saveFileDialog.FilterIndex = 2;
                        break;
                }
            }

            if (saveFileDialog.ShowDialog(Window.GetWindow(this)).Value != true)
                return false;

            string fileName = saveFileDialog.FileName;
            edit.SaveFile(fileName);
            edit.FileName = fileName;
            UpdatePage((TabItem)editorsTabControl.SelectedItem, fileName);
            return true;
        }

        private void CloseFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemovePage();
        }

        private bool SaveModifiedFiles(IList<string> files)
        {
            foreach (string file in files)
            {
                if (IsProjectFile(file))
                {
                    if (string.Compare(Project.ProjectFileName, file, true) == 0)
                        Project.Save();

                    continue;
                }

                IScriptEdit edit = FindFile(file);
                if (edit != null)
                {
                    if (edit.Modified)
                        edit.SaveFile(edit.FileName);
                }
            }

            return true;
        }
    }
}