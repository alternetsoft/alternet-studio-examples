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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Editor.Python;
using Alternet.Editor.TextSource;
using Alternet.FormDesigner.Integration;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter.Integration;
using Alternet.Syntax.Parsers.Python;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IDictionary<TabPage, ScriptCodeEdit> editors = new Dictionary<TabPage, ScriptCodeEdit>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();

        private string startupDirectory = Application.StartupPath + @"\";
        private ContextMenuStrip editorsMenu = new ContextMenuStrip();
        private ToolStripMenuItem miCloseEditor;
        private ToolStripMenuItem miCloseAllEditors;

        private ScriptCodeEdit ActiveSyntaxEdit
        {
            get
            {
                if (editorsTabControl.TabCount == 0)
                    return null;

                return GetEditor(editorsTabControl.SelectedTab);
            }
        }

        protected virtual void LoadStartupFile()
        {
            if (new DirectoryInfo(startupDirectory).Exists)
            {
                openFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);
                saveFileDialog.InitialDirectory = Path.GetFullPath(startupDirectory);

                var projectFile = Path.GetFullPath(Path.Combine(startupDirectory, @"Debugger.Python\Project.pyproj"));
                if (File.Exists(projectFile))
                    OpenProject(projectFile);
            }
        }

        protected virtual void InitEditorsContextMenu()
        {
            miCloseEditor = new ToolStripMenuItem(StringConsts.CloseTab, null, new EventHandler(DoCloseEditor));
            miCloseEditor.Name = "cmiCloseEditor";

            miCloseAllEditors = new ToolStripMenuItem(StringConsts.CloseAllTabs, null, new EventHandler(DoCloseAllEditors));
            miCloseAllEditors.Name = "cmiCloseAllEditors";

            editorsMenu.Items.Add(miCloseEditor);
            editorsMenu.Items.Add(miCloseAllEditors);
            editorsTabControl.ContextMenuStrip = editorsMenu;
        }

        protected void DoCloseEditor(object sender, System.EventArgs e)
        {
            RemovePage();
        }

        protected void DoCloseAllEditors(object sender, System.EventArgs e)
        {
            RemoveAllPages();
        }

        protected virtual void InitMenuIcons(ContextMenuStrip menu)
        {
            IDictionary<string, string> menuIcons = new Dictionary<string, string>();

            menuIcons.Add(StringConsts.MenuUndoCaption, "Undo");
            menuIcons.Add(StringConsts.MenuRedoCaption, "Redo");
            menuIcons.Add(StringConsts.MenuCutCaption, "Cut");
            menuIcons.Add(StringConsts.MenuCopyCaption, "Copy");
            menuIcons.Add(StringConsts.MenuPasteCaption, "Paste");
            menuIcons.Add(StringConsts.MenuSelectAllCaption, "SelectAll");
            menuIcons.Add(StringConsts.GotoDefinition, "GoToDefinition");

            foreach (ToolStripItem item in menu.Items)
            {
                if (menuIcons.ContainsKey(item.Text))
                {
                    string imageName = menuIcons[item.Text];
                    item.Image = LoadImage(imageName);
                }
            }
        }

        protected virtual ScriptCodeEdit NewFile(string fileName)
        {
            var page = new TabPage(Path.GetFileName(fileName));
            page.ToolTipText = fileName;

            editorsTabControl.TabPages.Add(page);
            var edit = CreateDebugEdit();
            edit.AllowedActions &= ~(AllowedActions.SetNextStatement | AllowedActions.FindAllImplementations);
            editors.Add(page, edit);

            edit.DefaultMenu.Opened += EditorContextMenu_Opened;
            InitMenuIcons(edit.DefaultMenu);

            string formId;
            if (IsFormFile(fileName, out formId) && File.Exists(formId))
            {
                var source = GetDesignerSource(formId);
                FormDesignerEditorHelpers.SetEditorSource(edit, fileName, source);
            }
            else
            if (File.Exists(fileName))
                edit.LoadFile(fileName);

            var parser = new PythonNETParser();
            parser.CodeEnvironment = scriptRun.CodeEnvironment;

            edit.Lexer = parser;
            if (Debugger.IsStarted)
                edit.ReadOnly = true;

            edit.TextChanged += (o, e) =>
            {
                if (editorsTabControl.SelectedTab == page)
                    editedCodeFiles.Add(fileName);
                UpdatePage(edit.Parent as TabPage, edit.FileName, edit.Modified);
                UpdateCodeNavigation();
            };

            edit.BeforeNavigateToDeclaration += (o, e) =>
            {
                var edt = o as IScriptEdit;
                if (edt != null)
                    navigationHistory.SaveCurrentLocationToHistory(edt.Position, edt.FileName, edt.GetLine(edt.Position.Y));
                navigationHistory.UpdateHistory(historyBackwardContextMenu.Items, 0, historyBackwardToolSplitButton, historyForwardToolButton);
            };

            edit.AfterNavigateToDeclaration += (o, e) =>
            {
                var edt = o as IScriptEdit;
                if (edt != null)
                    navigationHistory.SaveCurrentLocationToHistory(edt.Position, edt.FileName, edt.GetLine(edt.Position.Y));
                navigationHistory.UpdateHistory(historyBackwardContextMenu.Items, 0, historyBackwardToolSplitButton, historyForwardToolButton);
            };

            edit.StatusChanged += new EventHandler(CodeEdit_StatusChanged);

            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            edit.HighlightReferences = true;
            edit.Gutter.Options |= GutterOptions.PaintLinesOnGutter;

            page.Controls.Add(edit as Control);

            editorsTabControl.SelectedTab = page;
            UpdateSearch();
            UpdateBookmarks();
            UpdateCodeNavigation();
            edit.UpdateBreakpoints();
            return edit;
        }

        protected ScriptCodeEdit OpenFile(string fileName)
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

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
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
            toFind = "def " + methodName;

            var edit = ActiveSyntaxEdit;

            var oldPosition = edit.Position;

            edit.Position = new Point();
            if (edit.Find(toFind))
            {
                edit.MoveToLine(edit.Position.Y + 1);
                edit.MoveLineEnd();
            }
            else
                edit.Position = oldPosition;
        }

        private void EditorsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (projectIsClosing)
                e.Cancel = true;
        }

        private void EditorsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!projectIsClosing)
            {
                UpdateCodeNavigation();
                UpdateDesignerControls();
                navigationHistory.ClearCurrentHistory(historyBackwardContextMenu.Items, historyBackwardToolSplitButton, historyForwardToolButton);
            }
        }

        private ScriptCodeEdit GetEditor(TabPage key)
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

            if (groupPerProject)
            {
                if (Project.HasProject)
                {
                    GetDisplayFilesForProject(result, Project, newFiles);
                }
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
                IList<string> displayFiles = GetDisplayFiles(files, groupPerProject);
                DlgConfirmSave dlg = new DlgConfirmSave(Application.ProductName, displayFiles);

                switch (dlg.ShowDialog())
                {
                    case DialogResult.Yes:
                        SaveModifiedFiles(files);
                        confirm = true;
                        break;
                    case DialogResult.No:
                        confirm = true;
                        break;
                    default:
                        confirm = false;
                        break;
                }
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

            string designerFileName;
            string resourceFileName;
            FormFilesUtility.TryDetectFormSourceFiles(fileName, out designerFileName, out resourceFileName);

            if (!string.IsNullOrEmpty(designerFileName) && File.Exists(designerFileName))
                AddFile(files, designerFileName);

            if (!string.IsNullOrEmpty(resourceFileName) && File.Exists(resourceFileName))
                AddFile(files, resourceFileName);
        }

        private IList<string> GetModifiedFiles(IList<string> files)
        {
            IList<string> result = new List<string>();

            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                ScriptCodeEdit edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    if (!ContainsFile(files, edit.FileName))
                        continue;

                    AddFiles(result, edit.FileName);
                }
            }

            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                IFormDesignerControl designer;
                if (formDesigners.TryGetValue(tabPage, out designer))
                {
                    if (designer.Source.IsModified)
                    {
                        var fileName = designer.Source.UserCodeFileName;
                        if (!ContainsFile(files, fileName))
                            continue;
                        AddFiles(result, fileName);
                    }
                }
            }

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

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = FindUniqueName("Class", "py");

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
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

        private bool IsNameUnique(string baseName)
        {
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                if (tabPage.Text.ToString().StartsWith(baseName, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private string FindUniqueName(string baseName, string extesion)
        {
            int count = 1;

            string result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + "." + Path.GetExtension(baseName);
            while (!IsNameUnique(result))
            {
                count++;
                result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + "." + Path.GetExtension(baseName);
            }

            return result + extesion;
        }

        private TreeNode GetNodeToRemove(TreeNode node)
        {
            if (node.Tag is FormNodeData)
            {
                if ((node.Parent != null) && (node.Parent.Tag != null) && (node.Parent.Tag is FormNodeData))
                {
                    var tag = node.Tag as FormNodeData;
                    var parentTag = node.Parent.Tag as FormNodeData;
                    if ((tag.FormId == parentTag.FormId) && (tag.FileName == parentTag.FileName))
                        return node.Parent;
                }
            }

            return node;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;

                if (FormFilesUtility.CheckIfFormFilesExist(fileName))
                    OpenDesigner(fileName);
                else
                    OpenFile(fileName);
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.FileName != string.Empty)
                {
                    edit.SaveFile(edit.FileName);
                    UpdatePage(edit.Parent as TabPage, edit.FileName, edit.Modified);
                }
                else
                    SaveFileAs(edit);
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
                    SaveDesignerFiles(editorsTabControl.SelectedTab, designer);
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                SaveFileAs(edit);
        }

        private void SaveAllMenuItem_Click(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            if ((Project != null) && Project.HasProject)
            {
                SaveBreakpoints(GetBreakpointFile(Project));
                SaveBookmarks(GetBookmarkFile(Project));
            }
        }

        private string GetBreakpointFile(PythonProject project)
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.ProjectFileName), project.ProjectName + ".Breakpoints.xml"));
        }

        private string GetBookmarkFile(PythonProject project)
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
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                var edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    edit.SaveFile(edit.FileName);
                    UpdatePage(tabPage, edit.FileName, edit.Modified);
                }
            }

            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                IFormDesignerControl designer;
                if (formDesigners.TryGetValue(tabPage, out designer))
                {
                    if (designer.Source.IsModified)
                        SaveDesignerFiles(tabPage, designer);
                }
            }

            if (SaveAllProjects())
                UpdateProjectExplorer();

            return true;
        }

        private ScriptCodeEdit FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                var scriptEdit = GetEditor(tabPage);
                if (scriptEdit != null)
                {
                    var path = scriptEdit.FileName;
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = new Uri(path).LocalPath;
                        if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                            return scriptEdit;
                    }
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
                    navigationHistory.ClearCurrentHistory(historyBackwardContextMenu.Items, historyBackwardToolSplitButton, historyForwardToolButton);

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

        private ScriptCodeEdit OpenFile(string fileName, bool forceReopen)
        {
            ScriptCodeEdit edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabPage))
            {
                if (forceReopen)
                {
                    if (edit.Modified && !ConfirmSaveBeforeClosing(edit.FileName))
                        return edit;
                    CloseFile(edit.FileName);
                }
                else
                {
                    editorsTabControl.SelectedTab = (TabPage)edit.Parent;
                    return edit;
                }
            }

            edit = NewFile(fileName);

            UpdateControls();
            return edit;
        }

        private void SetActiveEdit(IScriptEdit edit)
        {
            if ((edit != null) && (edit.Parent is TabPage))
                editorsTabControl.SelectedTab = (TabPage)edit.Parent;
        }

        private void CloseFile(string fileName)
        {
            var edit = FindFile(fileName);
            if (edit != null)
            {
                var page = edit.Parent as TabPage;
                if (page != null)
                {
                    editorsTabControl.TabPages.Remove(page);
                    if (editors.ContainsKey(page))
                        editors.Remove(page);
                }

                edit.FileName = string.Empty;
                ((IDisposable)edit).Dispose();
            }
        }

        private void CloseFile(IScriptEdit edit)
        {
            var fileName = edit.FileName;

            if (!FileBelongsToProject(fileName))
                edit.FileName = string.Empty;

            ((IDisposable)edit).Dispose();
        }

        private void UpdatePage(TabPage page, string fileName, bool isModified = false)
        {
            if (page == null)
                return;
            string pageText = Path.GetFileName(fileName);

            pageText = isModified ? pageText + "*" : pageText;
            if (page.Text != pageText)
                page.Text = pageText;

            if (page.ToolTipText != fileName)
                page.ToolTipText = fileName;
        }

        private bool RemovePage(int index)
        {
            var tab = index >= 0 & index < editorsTabControl.TabCount ? editorsTabControl.TabPages[index] : null;
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
                    if (designer.Source.IsModified && !ConfirmSaveBeforeClosing(designer.Source.DesignerFileName))
                        return false;
                    CloseDesigner(designer);
                }
            }

            if (formDesigners.ContainsKey(tab))
                formDesigners.Remove(tab);

            if (editors.ContainsKey(tab))
                editors.Remove(tab);
            editorsTabControl.TabPages.Remove(tab);
            return true;
        }

        private void RemovePage()
        {
            RemovePage(editorsTabControl.SelectedIndex);
            UpdateButtons();
            UpdateCodeNavigation();
        }

        private void RemoveAllPages()
        {
            bool result = true;
            try
            {
                while (editorsTabControl.TabCount > 0)
                {
                    result = RemovePage(editorsTabControl.TabCount - 1);
                    if (!result)
                        break;
                }
            }
            finally
            {
                if (result)
                {
                    UpdateCodeNavigation();
                }
            }
        }

        private IFormDesignerControl GetFormDesigner(int index)
        {
            if (editorsTabControl.TabCount == 0)
                return null;

            IFormDesignerControl designer;
            if (!formDesigners.TryGetValue(editorsTabControl.TabPages[index], out designer))
                return null;

            return designer;
        }

        private IScriptEdit GetEditor(int index)
        {
            if (editorsTabControl.TabCount == 0)
                return null;

            return GetEditor(editorsTabControl.TabPages[index]);
        }

        private bool SaveFileAs(IScriptEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            var oldFileName = edit.FileName;
            if (!string.IsNullOrEmpty(edit.FileName))
            {
                saveFileDialog.FileName = edit.FileName;
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            var fileName = saveFileDialog.FileName;

            if (oldFileName != fileName)
            {
                var ds = GetDesignerSource(oldFileName, false);
                if (ds != null)
                    UpdateFormFiles(edit, ds, fileName);
                else
                {
                    edit.SaveFile(fileName);
                    edit.FileName = fileName;
                    UpdatePage(editorsTabControl.SelectedTab, fileName, edit.Modified);
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
            IList<ScriptCodeEdit> editors = new List<ScriptCodeEdit>();
            IList<Tuple<IFormDesignerControl, TabPage>> designerTabs = new List<Tuple<IFormDesignerControl, TabPage>>();

            foreach (string file in files)
            {
                if (IsProjectFile(file))
                {
                    var project = FindProject(file);

                    // todo
                    continue;
                }

                ScriptCodeEdit edit = FindFile(file);
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
                UpdatePage(edit.Parent as TabPage, edit.FileName, edit.Modified);
            }

            foreach (var designerTab in designerTabs)
            {
                var designer = designerTab.Item1;
                designer.Save();
                UpdateDesignPage(designerTab.Item2, designer.Source.UserCodeFileName, designer.Source.IsModified);
            }

            return true;
        }
    }
}