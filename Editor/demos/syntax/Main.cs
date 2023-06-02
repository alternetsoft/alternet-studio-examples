#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.Projects;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.Roslyn;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Alternet.CodeEditorSyntax.Demo
{
    public partial class MainForm : Form
    {
#region Private Fields

        private const string SBlank = "Blank File";
        private const string SOpenExplorer = "{0} Code Explorer";
        private const int PropertiesImage = 2;
        private const int FolderCloseImage = 7;
        private const int FolderOpenImage = 8;
        private const int FindResultTabIndex = 2;

        private const string CSharpFileExtension = ".cs";
        private const string DesignerFileNameSuffix = ".Designer";
        private const string ResourceFileExtension = ".resx";
        private const string VisualBasicFileExtension = ".vb";

        private int errImageIndex = 43;
        private int warningImageIndex = 45;
        private int updateCount = 0;

        private int csizeGap = 6;

        private bool projectIsClosing;
        private bool findInProject = false;
        private bool projectTreeDoubleClick;

        private int projectFilterIndex = 0;
        private DotNetProject project = new DotNetProject();
        private DotNetProjectExplorer explorer = new DotNetProjectExplorer();
        private IDictionary<TabPage, ISyntaxEdit> editors = new Dictionary<TabPage, ISyntaxEdit>();
        private string dir = Application.StartupPath + @"\";
        private ProjectCreationData currentProjectData = new ProjectCreationData { ProjectType = "WindowsFormsApp" };
        private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
        private bool updateControlsInProgress;

#endregion

        public MainForm()
        {
            InitializeComponent();
            updateTimer.Interval = 200;
            updateTimer.Tick += UpdateTimer_Tick;
            InitImages();

            ScaleControls();
            cmMain.Opening += Menu_Opening;
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            updateTimer.Stop();
        }

        private static Image LoadImage(string imageName)
        {
            Func<string, Image> getImage = name => Image.FromStream(
                typeof(MainForm).Assembly.GetManifestResourceStream(
                    string.Format("CodeEditorSyntax.Resources.{0}.png", name)));

            return new DisplayScaledImage(
                    () => getImage(imageName),
                    () => getImage(imageName + "_HighDpi")).Image;
        }

        private void InitImages()
        {
            newMenuItem.Image = LoadImage("NewFile");
            newStripSplitButton.Image = LoadImage("NewFile");
            openMenuItem.Image = LoadImage("OpenFile");
            openContextMenuItem.Image = LoadImage("OpenFile");
            openToolButton.Image = LoadImage("OpenFile");
            saveMenuItem.Image = LoadImage("Save");
            saveToolButton.Image = LoadImage("Save");
            saveAllMenuItem.Image = LoadImage("SaveAll");
            saveAsMenuItem.Image = LoadImage("SaveAs");
            exitMenuItem.Image = LoadImage("Exit");

            gotoDefinitionContextMenuItem.Image = LoadImage("GoToDefinition");
            gotoToolButton.Image = LoadImage("GoToDefinition");

            printMenuItem.Image = LoadImage("Print");
            printToolButton.Image = LoadImage("Print");
            printPreviewMenuItem.Image = LoadImage("PrintPreview");
            printPreviewToolButton.Image = LoadImage("PrintPreview");

            findMenuItem.Image = LoadImage("FindInFile");
            findContextMenuItem.Image = LoadImage("FindInFile");
            findToolButton.Image = LoadImage("FindInFile");
            replaceMenuItem.Image = LoadImage("ReplaceInFiles");
            replaceContextMenuItem.Image = LoadImage("ReplaceInFiles");
            replaceToolButton.Image = LoadImage("ReplaceInFiles");

            undoMenuItem.Image = LoadImage("Undo");
            undoToolButton.Image = LoadImage("Undo");
            redoMenuItem.Image = LoadImage("Redo");
            redoToolButton.Image = LoadImage("Redo");
            cutMenuItem.Image = LoadImage("Cut");
            cutContextMenuItem.Image = LoadImage("Cut");
            cutToolButton.Image = LoadImage("Cut");
            copyMenuItem.Image = LoadImage("Copy");
            copyContextMenuItem.Image = LoadImage("Copy");
            copyToolButton.Image = LoadImage("Copy");
            pasteMenuItem.Image = LoadImage("Paste");
            pasteContextMenuItem.Image = LoadImage("Paste");
            pasteToolButton.Image = LoadImage("Paste");
            selectAllMenuItem.Image = LoadImage("SelectAll");
        }

        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            gotoDefinitionContextMenuItem.Enabled = CanGotoDefinition();
        }

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            lvErrors.SmallImageList = DisplayScaling.CloneAndAutoScaleImageList(lvErrors.SmallImageList);
            tvSyntax.ImageList = DisplayScaling.CloneAndAutoScaleImageList(tvSyntax.ImageList);
            imageList1 = DisplayScaling.CloneAndAutoScaleImageList(imageList1);
        }

#region Files and Projects

        private bool FileBelongsToProject(string fileName)
        {
            if (project.HasProject)
            {
                if (project.Files.Contains(fileName) || project.Resources.Contains(fileName))
                    return true;
            }

            return false;
        }

        private ISyntaxEdit FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabPage tabPage in tcEditors.TabPages)
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

        private ISyntaxEdit NewFile(string fileName, int index)
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

            TabPage page = new TabPage(ExtractFileName(fileName));
            tcEditors.TabPages.Add(page);

            ISyntaxEdit edit = new SyntaxEdit();
            edit.UseDefaultMenu = false;

            editors.Add(page, edit);

            edit.Source = source;

            if (Path.GetExtension(fileName).ToLower().Equals(".vb") && (lexer is RoslynParser))
            {
                RoslynParser parser = (RoslynParser)lexer;
                parser.Repository.RegisterAssemblies(GetReferencedAssemblies(fileName));
            }

            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            page.Controls.Add(edit as Control);

            InitEditor(edit, lexer);
            edit.SearchDialog.Font = Font;
            edit.GotoLineDialog.Font = Font;

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileName;
                edit.LoadFile(fileName);
            }

            edit.TextChanged += (o, e) =>
            {
                UpdatePage(edit.Parent as TabPage, edit.Source.FileName, edit.Modified);
                UpdateCodeWindows();
            };

            edit.Spelling.CheckSpelling = true;
            edit.Spelling.SpellColor = Color.Navy;
            edit.LineSeparator.Options |= SeparatorOptions.SeparateContent;
            edit.Outlining.AllowOutlining = true;
            edit.HighlightReferences = true;

            tcEditors.SelectedTab = page;
            UpdateSearch();
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

            return new string[] { "mscorlib", "System", "System.Windows.Forms", "System.Drawing", "Microsoft.VisualBasic" };
        }

        private ISyntaxEdit OpenFile(string fileName, int index)
        {
            return OpenFile(fileName, index, false);
        }

        private ISyntaxEdit OpenFile(string fileName, int index, bool forceReopen)
        {
            ISyntaxEdit edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabPage))
            {
                if (forceReopen)
                {
                    if (edit.Modified && !ConfirmSaveBeforeClosing(edit.Source.FileName))
                        return edit;
                    CloseFile(edit.Source.FileName);
                }
                else
                {
                    tcEditors.SelectedTab = (TabPage)edit.Parent;
                    return edit;
                }
            }

            // loading file from disk
            edit = NewFile(fileName, index);
            UpdateButtons();
            return edit;
        }

        private void SetActiveEdit(ISyntaxEdit edit)
        {
            if ((edit != null) && (edit.Parent is TabPage))
                tcEditors.SelectedTab = (TabPage)edit.Parent;
        }

        private void CloseFile(string fileName)
        {
            ISyntaxEdit edit = FindFile(fileName);
            if (edit != null)
            {
                TabPage page = edit.Parent as TabPage;
                if (page != null)
                {
                    tcEditors.TabPages.Remove(page);
                    editors.Remove(page);
                }

                ((IDisposable)edit).Dispose();
            }
        }

        private void CloseFile(ISyntaxEdit edit)
        {
            var fileName = edit.Source.FileName;

            CodeParsing.UnregisterCode(Path.GetExtension(fileName), new string[] { fileName });

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

        private void RemovePage()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                if (edit.Modified && !ConfirmSaveBeforeClosing(edit.Source.FileName))
                    return;
                CloseFile(edit);
            }

            tcEditors.TabPages.Remove(tcEditors.SelectedTab);
            UpdateButtons();
            UpdateCodeWindows();
        }

        private bool SaveFileAs(ISyntaxEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            string oldExt = string.Empty;
            if (edit.Source.FileName != null)
            {
                saveFileDialog.FileName = edit.Source.FileName;
                oldExt = ExtractFileExt(edit.Source.FileName);
                int idx = FindLangByExt(oldExt);
                if (idx >= 0)
                    saveFileDialog.FilterIndex = idx + 1;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                edit.Source.SaveFile(fileName);
                edit.Source.FileName = fileName;
                UpdatePage(tcEditors.SelectedTab, fileName, edit.Modified);

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
            else
                return false;
            return true;
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
                UpdateButtons();
            }
            finally
            {
                projectIsClosing = false;
            }

            UpdateProjectExplorer();
            return true;
        }

#endregion

#region Toolbar, Statusbar and event handlers

        private void UpdateEvents(bool clear, string item)
        {
            if (clear)
                lbEvents.Items.Clear();
            if (item != string.Empty)
                lbEvents.Items.Add(item);
        }

        private void FillErrors(ISyntaxParser parser)
        {
            lvErrors.Items.Clear();
            if (parser != null)
            {
                IList<ISyntaxError> list = new List<ISyntaxError>();
                parser.GetSyntaxErrors(list);
                ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
                if (syntaxEdit != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string[] error = { list[i].Position.Y.ToString(), list[i].Position.X.ToString(), list[i].Description };
                        ListViewItem item = new ListViewItem(error);
                        switch (list[i].ErrorType)
                        {
                            case SyntaxErrorType.Error:
                                item.ImageIndex = errImageIndex;
                                break;
                            case SyntaxErrorType.Warning:
                                item.ImageIndex = warningImageIndex;
                                break;
                        }

                        item.Tag = list[i];
                        lvErrors.Items.Add(item);
                    }
                }
            }
        }

        private void UpdateEditorButtons()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();

            bool enabled = edit != null;
            bool canCut = enabled && edit.Selection.CanCut();
            bool canCopy = enabled && edit.Selection.CanCopy();
            bool canUndo = enabled && edit.Source.CanUndo();
            bool canRedo = enabled && edit.Source.CanRedo();
            bool canPaste = enabled && edit.Selection.CanPaste();

            closeProjectMenuItem.Enabled = project.HasProject;
            saveProjectMenuItem.Enabled = project.HasProject;
            saveMenuItem.Enabled = enabled;
            saveAsMenuItem.Enabled = enabled && !FileBelongsToProject(edit.Source.FileName);
            findMenuItem.Enabled = enabled;
            findContextMenuItem.Enabled = enabled;
            replaceMenuItem.Enabled = enabled;
            replaceContextMenuItem.Enabled = enabled;
            gotoMenunItem.Enabled = enabled;
            gotoContextMenuItem.Enabled = enabled;
            selectAllMenuItem.Enabled = enabled;
            printMenuItem.Enabled = enabled;
            printPreviewMenuItem.Enabled = enabled;
            closeFileMenuItem.Enabled = enabled;

            cutMenuItem.Enabled = canCut;
            cutContextMenuItem.Enabled = canCut;
            copyMenuItem.Enabled = canCopy;
            copyContextMenuItem.Enabled = canCopy;
            pasteMenuItem.Enabled = canPaste;
            pasteContextMenuItem.Enabled = canPaste;

            undoMenuItem.Enabled = canUndo;
            redoMenuItem.Enabled = canRedo;

            saveToolButton.Enabled = enabled;
            findToolButton.Enabled = enabled;
            replaceToolButton.Enabled = enabled;
            gotoToolButton.Enabled = enabled;
            printToolButton.Enabled = enabled;
            printPreviewToolButton.Enabled = enabled;

            cutToolButton.Enabled = canCut;
            copyToolButton.Enabled = canCopy;
            pasteToolButton.Enabled = canPaste;

            undoToolButton.Enabled = canUndo;
            redoToolButton.Enabled = canRedo;
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

        private void UpdateControls()
        {
            if (updateControlsInProgress)
                return;

            updateControlsInProgress = true;
            try
            {
                // updating toolbar, status bar and menu
                UpdateStatusBar();
                pnCombo.Visible = tcEditors.SelectedTab != null;
                if (tcEditors.SelectedTab != null)
                    pnCombo.Parent = tcEditors.SelectedTab;
            }
            finally
            {
                updateControlsInProgress = false;
            }
        }

        private void InitToolbar()
        {
            newStripSplitButton.Tag = newMenuItem;
            openToolButton.Tag = openMenuItem;
            saveToolButton.Tag = saveMenuItem;
            cutToolButton.Tag = cutMenuItem;
            copyToolButton.Tag = copyMenuItem;
            pasteToolButton.Tag = pasteMenuItem;
            undoToolButton.Tag = undoMenuItem;
            redoToolButton.Tag = redoMenuItem;
            findToolButton.Tag = findMenuItem;
            replaceToolButton.Tag = replaceMenuItem;
            gotoToolButton.Tag = gotoMenunItem;
            printPreviewToolButton.Tag = printPreviewMenuItem;
            printToolButton.Tag = printMenuItem;
        }

        private void UpdateButtons()
        {
            UpdateEditorButtons();
            UpdateControls();
        }

        private void UpdateStatusBar()
        {
            // updating status bar
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
            {
                sslPosition.Text = string.Format("Line: {0}, Char: {1}", syntaxEdit.Source.Position.Y, syntaxEdit.Source.Position.X);
                if (syntaxEdit.Source.ReadOnly)
                    sslModified.Text = "Readonly";
                else
                    if (syntaxEdit.Source.Modified)
                    sslModified.Text = "Modified";
                else
                    sslModified.Text = string.Empty;
                if (syntaxEdit.Source.Overwrite)
                    sslOverwrite.Text = "Overwrite";
                else
                    sslOverwrite.Text = " ";
            }
            else
            {
                sslPosition.Text = string.Empty;
                sslModified.Text = string.Empty;
                sslOverwrite.Text = " ";
            }
        }

        private void UpdateProjectExplorer()
        {
            if (project.HasProject)
                explorer.UpdateExplorer(project);
            else
                explorer.UpdateExplorer(null);
        }

        private void DelayedUpdateCodeWindows()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
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
                cbClasses.Items.Clear();
                cbMethods.Items.Clear();
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

        private int FolderIcon(string folderName, bool expand)
        {
            if (folderName.Equals("Properties"))
                return PropertiesImage;
            else
                return expand ? FolderOpenImage : FolderCloseImage;
        }

        private bool IsFolderNode(TreeNode node)
        {
            return (node.Tag != null) && (node.Tag is string) && ((string)node.Tag).Equals("FoderNode");
        }

        private bool IsReferenceNode(TreeNode node)
        {
            TreeNode parent = node.Parent;
            while (parent != null)
            {
                if (string.Compare(parent.Text, "references", true) == 0)
                    return true;
                parent = parent.Parent;
            }

            return false;
        }

        private bool IsFileNode(TreeNode node)
        {
            string fileName = GetFileNameFromNode(node);

            return !string.IsNullOrEmpty(fileName) && !IsReferenceNode(node);
        }

        private bool IsResourceNode(string nodeName)
        {
            return nodeName.EndsWith(".resx");
        }

        private string GetFileNameFromNode(TreeNode node)
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

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            DelayedUpdateCodeWindows();
            updateTimer.Stop();
        }

        private void NewClick(object sender, System.EventArgs e)
        {
            // new file
            int index = -1;
            int idx = FindLangByDesc((sender as ToolStripItem).Text, ref index);
            saveFileDialog.FilterIndex = (index >= 0) ? index + 1 : 1;
            saveFileDialog.FileName = "file1";

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

            OpenFile(saveFileDialog.FileName, idx, true);
        }

        private void NewProject_MenuItemClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentProjectData.ProjectLocation))
                currentProjectData.ProjectLocation = DefaultProjectSubPath;
            using (var dlg = new DlgNewProject(currentProjectData, new string[] { "C#", "Visual Basic" }, ProjectTypes.SupportedProjectTypes))
            {
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
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
        }

        private void Open_MenuItemClick(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                OpenFile(openFileDialog.FileName);
        }

        private void Save_MenuItemClick(object sender, EventArgs e)
        {
            // saving editor content to file on disk
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                if (edit.Source.FileName != string.Empty)
                {
                    edit.Source.SaveFile(edit.Source.FileName);
                    UpdatePage(edit.Parent as TabPage, edit.Source.FileName, edit.Modified);
                }
                else
                    SaveFileAs(edit);
            }
        }

        private void SaveAs_MenuItemClick(object sender, EventArgs e)
        {
            // saving editor content to file on disk prompting for file name
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
                SaveFileAs(edit);
        }

        private void SaveAll_MenuItemClick(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
        }

        private bool SaveAllModifiedFiles()
        {
            foreach (TabPage tabPage in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
                {
                    edit.SaveFile(edit.Source.FileName);
                    UpdatePage(tabPage, edit.Source.FileName, edit.Modified);
                }
            }

            project.Save();
            UpdateProjectExplorer();

            return true;
        }

        private void OpenProject_MenuItemClick(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = this.projectFilterIndex;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (project.HasProject)
                    CloseProject();
                OpenProject(openFileDialog.FileName);
            }
        }

        private void SaveProject_MenuItemClick(object sender, EventArgs e)
        {
            project.Save();
        }

        private void References_MenuItemOpening(object sender, CancelEventArgs e)
        {
            addFileContextMenuItem.Enabled = project.HasProject;
            addReferenceContextMenuItem.Enabled = project.HasProject;
            removeFileContextMenuItem.Enabled = false;
            removeReferenceContextMenuItem.Enabled = false;
            if (project.HasProject && tvSyntax.SelectedNode != null)
            {
                removeReferenceContextMenuItem.Enabled = IsReferenceNode(tvSyntax.SelectedNode);
                removeFileContextMenuItem.Enabled = IsFileNode(tvSyntax.SelectedNode);
            }
        }

        private void AddReference_MenuItemClick(object sender, EventArgs e)
        {
            if (!project.HasProject)
                return;

            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = Path.GetDirectoryName(project.ProjectFileName);

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var reference = dialog.FileName;
            if (project.AddReference(reference))
            {
                CodeParsing.RegisterAssemblies(project.ProjectExtension, new string[] { reference }, keepExisting: true);
                UpdateProjectExplorer();
                UpdateCodeWindows();
            }
        }

        private void RemoveReference_MenuItemClick(object sender, EventArgs e)
        {
            if (!project.HasProject)
                return;

            if (IsReferenceNode(tvSyntax.SelectedNode))
            {
                var reference = tvSyntax.SelectedNode.Tag as DotNetProject.AssemblyReference;
                if (project.RemoveReference(reference.FullName))
                {
                    var references = project.References.Select(x => x.FullName).ToArray();
                    CodeParsing.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(references).ToArray());
                    UpdateProjectExplorer();
                    UpdateCodeWindows();
                }
            }
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

        private void AddFile_MenuItemClick(object sender, EventArgs e)
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

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                if (IsSupportedLanguage(fileName))
                {
                    string designerFileName;
                    string resourceFileName;
                    TryDetectFormSourceFiles(fileName, out designerFileName, out resourceFileName);
                    addedFiles.Add(fileName);

                    if (!string.IsNullOrEmpty(designerFileName) && File.Exists(designerFileName))
                    {
                        addedFiles.Add(designerFileName);
                    }

                    if (!string.IsNullOrEmpty(resourceFileName) && File.Exists(resourceFileName))
                        addedFiles.Add(fileName);
                }
                else
                {
                    addedFiles.Add(fileName);
                }

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

        private bool IsSupportedLanguage(string fileName)
        {
            Check.NonNullOrEmpty(fileName, "fileName");

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (extension)
            {
                case CSharpFileExtension:
                    return true;

                case VisualBasicFileExtension:
                    return true;

                default:
                    return false;
            }
        }

        private void TryDetectFormSourceFiles(
            string userCodeFileName,
            out string designerFileName,
            out string resourceFileName)
        {
            var basePath = Path.GetDirectoryName(userCodeFileName);
            var baseFileName = Path.GetFileNameWithoutExtension(userCodeFileName);
            var codeExtension = Path.GetExtension(userCodeFileName);

            designerFileName = Path.Combine(basePath, baseFileName + DesignerFileNameSuffix + codeExtension);
            resourceFileName = Path.Combine(basePath, baseFileName + ResourceFileExtension);
        }

        private void RemoveFile_MenuItemClick(object sender, EventArgs e)
        {
            if (!project.HasProject)
                return;

            TreeNode node = GetNodeToRemove(tvSyntax.SelectedNode);

            IList<string> removedFiles = new List<string>();

            var fileName = GetFileNameFromNode(node);

            if (!string.IsNullOrEmpty(fileName))
            {
                removedFiles.Add(fileName);
            }

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                fileName = GetFileNameFromNode(node.Nodes[i]);

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

        private TreeNode GetNodeToRemove(TreeNode node)
        {
            if (node.Tag is FormNodeData)
            {
                if ((node.Parent != null) && (node.Parent.Tag != null) && (node.Parent.Tag is FormNodeData))
                {
                    FormNodeData tag = node.Tag as FormNodeData;
                    FormNodeData parentTag = node.Parent.Tag as FormNodeData;
                    if ((tag.FormId == parentTag.FormId) && (tag.FileName == parentTag.FileName))
                        return node.Parent;
                }
            }

            return node;
        }

        private void CloseProject_MenuItemClick(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void Undo_MenuItemClick(object sender, EventArgs e)
        {
            // undoing the last change
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Source.CanUndo())
                syntaxEdit.Source.Undo();
        }

        private void Redo_MenuItemClick(object sender, EventArgs e)
        {
            // redoing the last change
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Source.CanRedo())
                syntaxEdit.Source.Redo();
        }

        private void Find_MenuItemClick(object sender, EventArgs e)
        {
            // executing search dialog
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplaySearchDialog();
        }

        private void Replace_MenuItemClick(object sender, EventArgs e)
        {
            // executing replace dialog
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplayReplaceDialog();
        }

        private void Goto_MenuItemClick(object sender, EventArgs e)
        {
            // executing goto line dialog
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.DisplayGotoLineDialog();
        }

        private void GotoDefinition_MenuItemClick(object sender, EventArgs e)
        {
            GotoDefinition();
        }

        private async void GotoDefinition()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                SymbolLocation location = await parser.FindDeclarationAsync(edit.Position);
                if (location != null)
                {
                    OpenFile(location.FileName);
                    edit = GetActiveSyntaxEdit();
                    edit.Position = new Point(location.Column, location.Line);
                    edit.MakeVisible(new Point(location.Column, location.Line), true);
                }
            }
        }

        private void FindReferences_MenuItemClick(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
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

        private void FindImplementations_MenuItemClick(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
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

        private bool CanGotoDefinition()
        {
            bool result = false;
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            var parser = edit?.Lexer as ISyntaxParser;
            if (parser != null)
            {
                SymbolLocation location = parser.FindDeclaration(edit.Position);
                return location != null;
            }

            return result;
        }

        private void Cut_MenuItemClick(object sender, EventArgs e)
        {
            // cutting selection to clipboard
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanCut())
                syntaxEdit.Selection.Cut();
        }

        private void Copy_MenuItemClick(object sender, EventArgs e)
        {
            // copying selection to clipboard
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanCopy())
                syntaxEdit.Selection.Copy();
        }

        private void Paste_MenuItemClick(object sender, EventArgs e)
        {
            // pasting selection to clipboard
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null && syntaxEdit.Selection.CanPaste())
                syntaxEdit.Selection.Paste();
        }

        private void SelectAll_MenuItemClick(object sender, EventArgs e)
        {
            // selecting editor's content
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Selection.SelectAll();
        }

        private void PrintPreview_MenuItemClick(object sender, EventArgs e)
        {
            // executing print preview dialog
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Printing.ExecutePrintPreviewDialog();
        }

        private void Print_MenuItemClick(object sender, EventArgs e)
        {
            // printing the editor
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Printing.Print();
        }

        private void About_MenuItemClick(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmSaveBeforeClosing())
                e.Cancel = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SearchManager.SharedSearch.InitSearch += new InitSearchEvent(DoInitSearch);
            SearchManager.SharedSearch.GetSearch += new GetSearchEvent(DoGetSearch);
            SearchManager.SharedSearch.TextFound += new TextFoundEvent(DoTextFound);

            sslPosition.Width = 120;

            InitToolbar();

            // assigning explorer tree
            explorer.ExplorerTree = tvSyntax;

            // locating schemes
            if (!new DirectoryInfo(dir + "Resources").Exists)
                dir = dir + @"\..\..\..\..\..\..\";
            dir = dir + @"Resources\";

            // loading schemes
            DirectoryInfo info = new DirectoryInfo(dir + @"Editor\Schemes");
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
            info = new DirectoryInfo(dir + @"Editor\Text");
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
            foreach (LanguageInfo linfo in LangInfos.LangItems)
            {
                if (linfo.FileType == "-")
                {
                    if (found)
                    {
                        mnuFiles.Items.Add("-", null);
                        newMenuItem.DropDownItems.Add("-", null);
                        samplesMenuItem.DropDownItems.Add("-", null);
                    }
                }
                else
                if ((linfo.FileName != string.Empty) && (linfo.SchemeName != string.Empty))
                {
                    found = true;
                    mnuFiles.Items.Add(linfo.Description, null, new EventHandler(NewClick));
                    newMenuItem.DropDownItems.Add(linfo.Description, null, new EventHandler(NewClick));
                    samplesMenuItem.DropDownItems.Add(linfo.Description, null, new EventHandler(SampleClick));
                }
            }

            newMenuItem.DropDownItems.Add("-", null);
            newMenuItem.DropDownItems.Add(SBlank, null, new EventHandler(NewClick));
            mnuFiles.Items.Add("-", null);
            mnuFiles.Items.Add(SBlank, null, new EventHandler(NewClick));

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
            samplesMenuItem.Visible = samplesMenuItem.DropDownItems.Count > 0;

            // opening sample file
            NewSampleFile("C#");

            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.Source.FormatText();
                edit.CodeCompletionBox.SelectionChanged += new EventHandler(CodeCompletionBox_SelectionChanged);
            }

            UpdateButtons();
            UpdateSearch();

            sslModified.AutoSize = true;
            sslOverwrite.AutoSize = true;
            sslPosition.AutoSize = true;

            ucFindResults.FindResultClick += FindResults_FindResultClick;
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
                    ISyntaxEdit edit = OpenFile(range.FileName);
                    if (edit != null)
                    {
                        var position = new Point(range.StartPoint.X, range.StartPoint.Y);
                        edit.MakeVisible(position, true);
                        edit.Position = position;
                        edit.Focus();
                    }
                }
            }
        }

        private void Exit_MenuItemClick(object sender, EventArgs e)
        {
            // closing the application
            this.Close();
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

        private IList<string> GetModifiedFiles(IList<string> files)
        {
            IList<string> result = new List<string>();

            foreach (TabPage tabPage in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(tabPage);
                if (edit != null && edit.Modified)
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

        private void StandardToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag is ToolStripItem)
            {
                ToolStripItem mi = (ToolStripItem)e.ClickedItem.Tag;
                mi.PerformClick();
            }
        }

        private void EditorsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!projectIsClosing)
            {
                UpdateCodeWindows();
                UpdateEvents(true, string.Empty);
            }
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            int newWidth = (pnCombo.ClientSize.Width - csizeGap) / 2;
            cbClasses.Width = newWidth;
            cbMethods.Width = newWidth;
        }

        private void OnDrawItem(ComboBox combo, DrawItemEventArgs e)
        {
            CodeUtils.DrawItem(combo, e, imageList1);
        }

        private void MethodsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateCount > 0)
                return;
            ISyntaxEdit edit = GetActiveSyntaxEdit();

            RoslynParser parser = null;
            if ((edit != null) && (edit.Lexer is RoslynParser))
                parser = (RoslynParser)edit.Lexer;

            if (edit != null && sender is ComboBox)
            {
                Point position;
                if (CodeParsing.SelectItem((ComboBox)sender, parser, out position))
                {
                    edit.Position = position;
                    edit.Focus();
                }
            }
        }

        private void MethodsComboBox_Format(object sender, ListControlConvertEventArgs e)
        {
            var text = CodeUtils.FormatText(e.ListItem);
            if (!string.IsNullOrEmpty(text))
                e.Value = text;
        }

        private void MethodsComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            OnDrawItem((ComboBox)sender, e);
        }

        private void SyntaxTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (projectTreeDoubleClick && e.Action == TreeViewAction.Collapse)
            {
                projectTreeDoubleClick = false;
                e.Cancel = true;
            }
            else
                if (IsFolderNode(e.Node))
                {
                    e.Node.ImageIndex = FolderIcon(e.Node.Name, false);
                    e.Node.SelectedImageIndex = e.Node.ImageIndex;
                }
        }

        private void SyntaxTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (projectTreeDoubleClick && e.Action == TreeViewAction.Expand)
            {
                projectTreeDoubleClick = false;
                e.Cancel = true;
            }
            else
                if (IsFolderNode(e.Node))
                {
                    e.Node.ImageIndex = FolderIcon(e.Node.Name, true);
                    e.Node.SelectedImageIndex = e.Node.ImageIndex;
                }
        }

        private void SyntaxTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
                projectTreeDoubleClick = true;
            else
                projectTreeDoubleClick = false;
        }

        private void CloseFile_MenuItemClick(object sender, EventArgs e)
        {
            RemovePage();
        }

        private void SyntaxTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var node = e.Node;
                if (node != null)
                {
                    var tag = node.Tag;

                    string codeFileName = null;

                    var formNodeData = tag as FormNodeData;
                    if (formNodeData != null)
                    {
                        codeFileName = formNodeData.FileName;
                    }
                    else
                        codeFileName = tag as string;

                    if (!IsReferenceNode(node) && !string.IsNullOrEmpty(codeFileName) && new FileInfo(codeFileName).Exists)
                        OpenFile(codeFileName);
                }
            }
        }

        private void CodeCompletionBox_SelectionChanged(object sender, EventArgs e)
        {
            string selectedItemName = ((ListBox)sender).SelectedItem != null ? ((ListBox)sender).SelectedItem.ToString() : string.Empty;
        }

        private void PageSetup_MenuItemClick(object sender, EventArgs e)
        {
            // executing page setup dialog
            ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
            if (syntaxEdit != null)
                syntaxEdit.Printing.ExecutePageSetupDialog();
        }

        private void CodeExplorer_MenuItemClick(object sender, EventArgs e)
        {
            OpenExplorer();
        }

        private void Editors_MenuItemOpening(object sender, CancelEventArgs e)
        {
            removePageContextMenuItem.Enabled = tcEditors.SelectedTab != null;
        }

        private void RemovePage_MenuItemClick(object sender, EventArgs e)
        {
            RemovePage();
        }

        private void MainContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateEvents(false, "Context menu is opened");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void ErrorsLisiewTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (lvErrors.SelectedItems.Count > 0)
            {
                object obj = lvErrors.SelectedItems[0].Tag;
                if ((obj != null) && (obj is ISyntaxError))
                {
                    ISyntaxEdit syntaxEdit = GetActiveSyntaxEdit();
                    if (syntaxEdit != null)
                    {
                        syntaxEdit.MoveTo(((ISyntaxError)obj).Position);
                        syntaxEdit.Selection.SetSelection(SelectionType.Stream, ((ISyntaxError)obj).Range.StartPoint, ((ISyntaxError)obj).Range.EndPoint);
                        syntaxEdit.Focus();
                    }
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

        private void SampleClick(object sender, System.EventArgs e)
        {
            // new sample file
            NewSampleFile((sender as ToolStripItem).Text);
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            // updating status bar and toolbar buttons when changing selection in the editor
            UpdateControls();
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
                s = string.Format("Source state changed: Modified={0}", ((ISyntaxEdit)sender).Source.Modified);
                UpdateEvents(false, s);
            }

            if ((e.State & NotifyState.OverWriteChanged) != 0)
            {
                s = string.Format("Text was formatted");
                UpdateEvents(false, s);
            }

            if ((e.State & NotifyState.TextParsed) != 0)
                DoReparse();

            UpdateCodeWindows();
        }

        private void DoReparse()
        {
            // text was fully parsed, updating explorer tree
            UpdateCodeWindows();
            UpdateEvents(false, "Text Reparsed");
        }

#endregion

#region Private Methods

        private ISyntaxEdit GetEditor(TabPage key)
        {
            ISyntaxEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private void UpdateSearch()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            var search = edit as ISearch;
            if (search != null)
            {
                search.SearchGlobal = true;
            }
        }

        private void DoTextFound(object sender, TextFoundEventArgs e)
        {
            if (e.Search != null)
                return;

            var search = OpenFile(e.FileName) as ISearch;
            if (search != null)
            {
                e.Search = search;
                search.OnTextFound(e.Text, e.Options, e.Expression, e.Match, e.Position, e.Len, false, e.MultiLine);
                UpdateSearch();
            }
        }

        private void DoGetSearch(object sender, GetSearchEventArgs e)
        {
            if (findInProject && project.HasProject)
            {
                for (int i = 0; i < project.Files.Count; i++)
                {
                    if (string.Compare(project.Files[i], e.FileName, true) == 0)
                    {
                        ISyntaxEdit edit = FindFile(e.FileName);
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
                foreach (TabPage page in editors.Keys)
                {
                    ISyntaxEdit edit = GetEditor(page);
                    if (edit != null && edit.Source != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                    {
                        tcEditors.SelectedTab = page;
                        UpdateSearch();
                        e.Search = edit;
                        break;
                    }
                }
            }
        }

        private void DoInitSearch(object sender, InitSearchEventArgs e)
        {
            e.Search = GetActiveSyntaxEdit();
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
                foreach (ISyntaxEdit edit in editors.Values)
                {
                    edit.SearchGlobal = true;
                    if (edit.Source != null)
                        e.SearchList.Add(edit.Source.FileName);
                }
            }
        }

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
                if (!string.IsNullOrEmpty(fileExt))
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
                    result = new Syntax.Parser();
                    break;
            }

            if (result is ISyntaxParser)
            {
                ISyntaxParser sp = (ISyntaxParser)result;
                sp.Options |= SyntaxOptions.CodeCompletion;
            }

            return result;
        }

        private void InitEditor(ISyntaxEdit edit, ILexer lexer)
        {
            edit.ContextMenuStrip = cmMain;
            edit.Source.Lexer = lexer;
            edit.CodeCompletionBox.Images = imageList1;
            edit.SourceStateChanged += new NotifyEvent(SourceStateChanged);
            edit.Selection.SelectionChanged += new EventHandler(SelectionChanged);
        }

        private void NewSampleFile(string lang)
        {
            // creating new sample file
            int idx = FindLangByDesc(lang);
            if ((idx >= 0) && (LangInfos.LangItems[idx].FileName != string.Empty))
                OpenFile(LangInfos.LangItems[idx].FileName, idx);
        }

        private ISyntaxEdit OpenFile(string fileName)
        {
            return OpenFile(fileName, -1);
        }

        private ISyntaxEdit GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedTab);
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

                ISyntaxEdit edit = FindFile(file);
                if (edit != null)
                {
                    if (edit.Modified)
                        edit.SaveFile(edit.Source.FileName);
                }
            }

            return true;
        }

        private void ActivateFindResultsTab()
        {
            tcBottom.SelectedIndex = FindResultTabIndex;
        }

        private void OpenExplorer()
        {
            // opening explorer
            if (gbExplorer.Visible)
                codeExplorerMenuItem.Text = string.Format(SOpenExplorer, "Open");
            else
                codeExplorerMenuItem.Text = string.Format(SOpenExplorer, "Close");
            gbExplorer.Visible = !gbExplorer.Visible;
        }

#endregion
    }
}
