#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;
#if USEFORMDESIGNER
using Alternet.FormDesigner.Integration;
using Alternet.FormDesigner.WinForms;
#endif
using Alternet.Scripter.Integration;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IDictionary<TabPage, IScriptEdit> editors = new Dictionary<TabPage, IScriptEdit>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();

        private string startupDirectory = Application.StartupPath + @"\";
        private ContextMenuStrip editorsMenu = new ContextMenuStrip();
        private ToolStripMenuItem miCloseEditor;
        private ToolStripMenuItem miCloseAllEditors;

        protected IScriptEdit ActiveSyntaxEdit
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

                var projectDirectory = Path.Combine(startupDirectory, @"Debugger\cs\HelloWorld");
#if NET6_0_OR_GREATER
                projectDirectory += "-dotnetcore";
#endif

                var projectFile = Path.GetFullPath(Path.Combine(projectDirectory, "HelloWorld.csproj"));
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

        protected virtual IScriptEdit NewFile(string fileName)
        {
            var page = new TabPage(Path.GetFileName(fileName));
            page.ToolTipText = fileName;

            editorsTabControl.TabPages.Add(page);
            var edit = CreateDebugEdit();
            editors.Add(page, edit);

            edit.DefaultMenu.Opened += EditorContextMenu_Opened;
            InitMenuIcons(edit.DefaultMenu);

#if USEFORMDESIGNER
            string formId;
            if (IsFormFile(fileName, out formId) && File.Exists(formId))
            {
                var source = GetDesignerSource(formId);
                FormDesignerEditorHelpers.SetEditorSource(edit, fileName, source);
            }
            else
#endif
            {
                if (File.Exists(fileName))
                    edit.LoadFile(fileName);
            }

            var projectName = GetProjectName(fileName);
            edit.SetFileNameAndProject(fileName, projectName);

            edit.TextChanged += (o, e) =>
            {
                if (editorsTabControl.SelectedTab == page)
                    editedCodeFiles.Add(fileName);
                UpdatePage(edit.Parent as TabPage, edit.FileName, edit.Modified);
                UpdateCodeNavigation();
            };

            edit.StatusChanged += new EventHandler(CodeEdit_StatusChanged);

#if USEFORMDESIGNER
            RegisterDesignerImportsInEditor(fileName, edit);
#endif

            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            edit.HighlightReferences = true;

            page.Controls.Add(edit as Control);

            editorsTabControl.SelectedTab = page;
            UpdateSearch();
            UpdateCodeNavigation();
            return edit;
        }

        protected IScriptEdit OpenFile(string fileName)
        {
            return OpenFile(fileName, false);
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
            var cs = Path.GetExtension(userCodeFileName) == ".cs";
            if (cs)
                toFind = "void " + methodName;
            else
                toFind = "Sub " + methodName;

            var edit = ActiveSyntaxEdit;

            var oldPosition = edit.Position;

            edit.Position = new Point();
            if (edit.Find(toFind))
            {
                edit.MoveToLine(edit.Position.Y + (cs ? 2 : 1));
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
#if USEFORMDESIGNER
                UpdateDesignerControls();
#endif
                navigationHistory.ClearCurrentHistory(historyBackwardContextMenu.Items, historyBackwardToolSplitButton, historyForwardToolButton);
            }
        }

        private IScriptEdit GetEditor(TabPage key)
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
            var confirm = true;

            if (files.Count > 0)
            {
                var displayFiles = GetDisplayFiles(files, groupPerProject);
                var dlg = new DlgConfirmSave(Application.ProductName, displayFiles);

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
            if (string.IsNullOrEmpty(fileName))
                return;

            AddFile(files, fileName);

#if USEFORMDESIGNER
            string designerFileName;
            string resourceFileName;
            FormFilesUtility.TryDetectFormSourceFiles(fileName, out designerFileName, out resourceFileName);

            if (!string.IsNullOrEmpty(designerFileName) && File.Exists(designerFileName))
                AddFile(files, designerFileName);

            if (!string.IsNullOrEmpty(resourceFileName) && File.Exists(resourceFileName))
                AddFile(files, resourceFileName);
#endif
        }

        private IList<string> GetModifiedFiles(IList<string> files)
        {
            IList<string> result = new List<string>();

            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                var edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    if (!ContainsFile(files, edit.FileName))
                        continue;

                    AddFiles(result, edit.FileName);
                }
            }

#if USEFORMDESIGNER
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
#endif

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

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            string location = projectCreationData.ProjectLocation;

            if (string.IsNullOrEmpty(location))
                location = Project.HasProject ? Path.GetDirectoryName(Project.ProjectFileName) : DefaultProjectSubPath;

            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null)
                project = Project;

            var extension = !solution.IsEmpty ? solution.DefaultProject.DefaultExtension : Project.HasProject ? Project.DefaultExtension : "cs";

            switch (extension)
            {
                case "vb":
                    saveFileDialog.FilterIndex = 2;
                    break;
                default:
                    saveFileDialog.FilterIndex = 1;
                    break;
            }

            bool addToProject = project != null && project.HasProject;

            saveFileDialog.FileName = Path.GetFileName(FindUniqueName(location, "Class", extension));
            saveFileDialog.InitialDirectory = location;

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

            if (!addToProject)
            {
                OpenFile(saveFileDialog.FileName);
            }
            else
            {
                project.AddFile(saveFileDialog.FileName);
                OpenFile(saveFileDialog.FileName);
            }
        }

        private string FindUniqueName(string location, string baseName, string extension)
        {
            int count = 1;
            string result = Path.Combine(location, baseName + count.ToString() + "." + extension);
            while (File.Exists(result))
            {
                count++;
                result = Path.Combine(location, baseName + count.ToString() + "." + extension);
            }

            return result;
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

#if USEFORMDESIGNER
                if (FormFilesUtility.CheckIfFormFilesExist(fileName))
                    OpenDesigner(fileName);
                else
#endif
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
            }
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    SaveDesignerFiles(editorsTabControl.SelectedTab, designer);
            }
#endif
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

#if USEFORMDESIGNER
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                IFormDesignerControl designer;
                if (formDesigners.TryGetValue(tabPage, out designer))
                {
                    if (designer.Source.IsModified)
                        SaveDesignerFiles(tabPage, designer);
                }
            }
#endif

            if (SaveAllProjects())
                UpdateProjectExplorer();

            return true;
        }

        private bool IsResourceFileName(string fileName)
        {
            var ext = Path.GetExtension(fileName);
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

        private IScriptEdit FindFile(string fileName)
        {
            if (!PathUtilities.IsPathFullyQualified(fileName))
                return null;

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

#if USEFORMDESIGNER
        private void RegisterDesignerImportsInEditor(string fileName, DebugCodeEdit edit)
        {
            edit.RegisterAssemblies(GetDesignerReferencedAssemblies(fileName).AssemblyNames.ToArray());
            var namespaces = GetDesignerImportedNamespaces(fileName);
            if (namespaces != null)
                edit.RegisterNamespaces(namespaces.Namespaces);
        }
#endif

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

        private IScriptEdit OpenFile(string fileName, bool forceReopen) // forceReopen for case when we try to re-open file modified outside demo
        {
            var edit = FindFile(fileName);
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
            if (!HasProject())
            {
                CodeEditExtensions.RegisterDefaultAssemblies(edit);
#if USEFORMDESIGNER
                AddDesignFileForParsing(fileName);
#endif
            }
#if USEFORMDESIGNER
            else
                AddDesignerReferencesToEditor(edit, fileName);
#endif

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
#if USEFORMDESIGNER
            if (!HasProject())
                RemoveDesignFileForParsing(fileName);
#endif

            if (!FileBelongsToProject(fileName))
                edit.FileName = string.Empty;

            ((IDisposable)edit).Dispose();
        }

        private void UpdatePage(TabPage page, string fileName, bool isModified = false)
        {
            if (page == null)
                return;
            var pageText = Path.GetFileName(fileName);

            pageText = isModified ? pageText + "*" : pageText;
            if (page.Text != pageText)
                page.Text = pageText;

            if (page.ToolTipText != fileName)
                page.ToolTipText = fileName;
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
                    UpdateCodeNavigation();
            }
        }

#if USEFORMDESIGNER
        private IFormDesignerControl GetFormDesigner(int index)
        {
                if (editorsTabControl.TabCount == 0)
                    return null;

                IFormDesignerControl designer;
                if (!formDesigners.TryGetValue(editorsTabControl.TabPages[index], out designer))
                    return null;

                return designer;
        }
#endif

        private IScriptEdit GetEditor(int index)
        {
            if (editorsTabControl.TabCount == 0)
                return null;

            return GetEditor(editorsTabControl.TabPages[index]);
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
#if USEFORMDESIGNER
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
#endif
            if (editors.ContainsKey(tab))
                editors.Remove(tab);
            editorsTabControl.TabPages.Remove(tab);
            return true;
        }

        private void RemovePage()
        {
            RemovePage(editorsTabControl.SelectedIndex);
            UpdateCodeNavigation();
        }

        private bool SaveFileAs(IScriptEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            var oldFileName = edit.FileName;
            if (!string.IsNullOrEmpty(edit.FileName))
            {
                saveFileDialog.FileName = edit.FileName;
                var extenstion = Path.GetExtension(edit.FileName).ToLower();
                switch (extenstion)
                {
                    case ".vb":
                        saveFileDialog.FilterIndex = 2;
                        break;
                    case ".csx":
                        saveFileDialog.FilterIndex = 3;
                        break;
                }
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            var fileName = saveFileDialog.FileName;

            if (oldFileName != fileName)
            {
#if USEFORMDESIGNER
                var ds = GetDesignerSource(oldFileName, false);
                if (ds != null)
                    UpdateFormFiles(edit, ds, fileName);
                else
#endif
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
            IList<IScriptEdit> editors = new List<IScriptEdit>();
#if USEFORMDESIGNER
            IList<Tuple<IFormDesignerControl, TabPage>> designerTabs = new List<Tuple<IFormDesignerControl, TabPage>>();
#endif

            foreach (var file in files)
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

                var edit = FindFile(file);
                if (edit != null)
                {
                    if (edit.Modified)
                        editors.Add(edit);
                }
#if USEFORMDESIGNER
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
#endif
            }

            foreach (var edit in editors)
            {
                edit.SaveFile(edit.FileName);
                UpdatePage(edit.Parent as TabPage, edit.FileName, edit.Modified);
            }

#if USEFORMDESIGNER
            foreach (var designerTab in designerTabs)
            {
                var designer = designerTab.Item1;
                designer.Save();
                UpdateDesignPage(designerTab.Item2, designer.Source.UserCodeFileName, designer.Source.IsModified);
            }

#endif
            return true;
        }
    }
}
