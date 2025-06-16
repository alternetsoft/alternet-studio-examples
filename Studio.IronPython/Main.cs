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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor.Common;
using Alternet.Editor.IronPython;
using Alternet.Editor.TextSource;
using Alternet.Scripter.Debugger.UI;
using Alternet.Syntax;

namespace AlternetStudio.Demo
{
    public partial class MainForm : Form
    {
        #region Private Members

        private const int OutputTabIndex = 1;
        private const int CallStackTabIndex = 2;
        private const int VariablesInScopeTabIndex = 3;
        private const int WatchesTabIndex = 4;
        private const int ErrorsTabIndex = 5;
        private const int ThreadsTabIndex = 6;
        private const int PropertiesImage = 2;
        private const int FolderCloseImage = 7;
        private const int FolderOpenImage = 8;
        private const int FindResultTabIndex = 7;

        private const int SizeGap = 6;

        private int updateCount;
        private System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
        private ImageList codeExplorerImageList = new ImageList();
        private ImageList projectExplorerImageList = new ImageList();

        #endregion

        public MainForm(string[] args)
        {
            InitializeScripter();
            InitializeEditors();
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "AlternetStudio.Demo.IronPython.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            InitImages();
            InitializeCodeNavigationBar();
            InitEditorsContextMenu();
            InitializeFormDesigner();
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);
            BookMarkManager.SharedBookMarks.BookMarkAdded += SharedBookMarks_BookMarkAdded;
            BookMarkManager.SharedBookMarks.BookMarkRemoved += SharedBookMarks_BookMarkRemoved;
            ScaleControls();
        }

        protected override void OnClosed(EventArgs e)
        {
            FinalizeCodeSearch();

            base.OnClosed(e);
        }

        private static Image LoadImage(string imageName)
        {
            Func<string, Image> getImage = name => Image.FromStream(
                typeof(MainForm).Assembly.GetManifestResourceStream(
                    string.Format("AlternetStudio.Demo.IronPython.Resources.{0}.png", name)));

            return new DisplayScaledImage(
                    () => getImage(imageName),
                    () => getImage(imageName + "_HighDpi")).Image;
        }

        private ImageList LoadImageList(string resource)
        {
            return ImageListHelper.LoadImageListFromStrip(typeof(MainForm).Assembly, string.Format("AlternetStudio.Demo.IronPython.Resources.{0}.png", resource));
        }

        private void InitImages()
        {
            codeExplorerImageList = LoadImageList("CodeExplorerImages");
            projectExplorerImageList = LoadImageList("ProjectExplorerImages");
            codeExplorerTreeView.ImageList = codeExplorerImageList;
            projectExplorerTreeView.ImageList = projectExplorerImageList;

            newMenuItem.Image = LoadImage("NewFile");
            newStripSplitButton.Image = LoadImage("NewFile");
            openMenuItem.Image = LoadImage("OpenFile");
            openToolButton.Image = LoadImage("OpenFile");
            saveMenuItem.Image = LoadImage("Save");
            saveToolButton.Image = LoadImage("Save");
            saveAllMenuItem.Image = LoadImage("SaveAll");
            saveAsMenuItem.Image = LoadImage("SaveAs");
            exitMenuItem.Image = LoadImage("Exit");

            gotoDefinitionMenuItem.Image = LoadImage("GoToDefinition");
            gotoToolButton.Image = LoadImage("GoToDefinition");
            historyBackwardToolSplitButton.Image = LoadImage("Backwards");
            historyForwardToolButton.Image = LoadImage("Forwards");

            printMenuItem.Image = LoadImage("Print");
            printToolButton.Image = LoadImage("Print");
            printPreviewMenuItem.Image = LoadImage("PrintPreview");
            printPreviewToolButton.Image = LoadImage("PrintPreview");

            findMenuItem.Image = LoadImage("FindInFile");
            findToolButton.Image = LoadImage("FindInFile");
            replaceMenuItem.Image = LoadImage("ReplaceInFiles");
            replaceToolButton.Image = LoadImage("ReplaceInFiles");

            undoMenuItem.Image = LoadImage("Undo");
            undoToolButton.Image = LoadImage("Undo");
            redoMenuItem.Image = LoadImage("Redo");
            redoToolButton.Image = LoadImage("Redo");
            cutMenuItem.Image = LoadImage("Cut");
            cutToolButton.Image = LoadImage("Cut");
            copyMenuItem.Image = LoadImage("Copy");
            copyToolButton.Image = LoadImage("Copy");
            pasteMenuItem.Image = LoadImage("Paste");
            pasteToolButton.Image = LoadImage("Paste");
            selectAllMenuItem.Image = LoadImage("SelectAll");

            toggleBookmarkToolButton.Image = LoadImage("Bookmark");
            prevBookmarkToolButton.Image = LoadImage("PreviousBookmark");
            nextBookmarkToolButton.Image = LoadImage("NextBookmark");
            clearAllBookmarksToolButton.Image = LoadImage("ClearBookmark");
        }

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            codeExplorerTreeView.ImageList = DisplayImageScaling.CloneAndAutoScaleImageList(codeExplorerTreeView.ImageList);
            projectExplorerTreeView.ImageList = DisplayImageScaling.CloneAndAutoScaleImageList(projectExplorerTreeView.ImageList);
            codeExplorerImageList = DisplayImageScaling.CloneAndAutoScaleImageList(codeExplorerImageList);
            projectExplorerImageList = DisplayImageScaling.CloneAndAutoScaleImageList(projectExplorerImageList);
            codeNavigationBarPanel.Height = methodsComboBox.Height + 2;
        }

        #region Scripter and Debugger

        private void Dialog_WatchAdded(object sender, WatchAddedEventArgs e)
        {
            ActivateWatchesTab();
        }

        private void Callstack_StackFramesRetrieved(object sender, EventArgs e)
        {
            if (switchToTopUserStackFrameNeeded)
            {
                switchToTopUserStackFrameNeeded = false;
                callStackControl.TrySwitchToTopUserStackFrame();
            }
        }

        #endregion

        private IList<string> GetImportedNamespaces(string fileName)
        {
            var project = GetProject(fileName);

            if (project != null && project.HasProject)
                return project.ImportedNamespaces;

            return null;
        }

        #region Files and Projects

        private void SetActiveEdit(ScriptCodeEdit edit)
        {
            if ((edit != null) && (edit.Parent is TabPage))
                editorsTabControl.SelectedTab = (TabPage)edit.Parent;
        }

        private void CloseFile(ScriptCodeEdit edit)
        {
            var fileName = edit.FileName;

            if (!FileBelongsToProject(fileName))
                edit.FileName = string.Empty;

            ((IDisposable)edit).Dispose();
        }

        private bool SaveFileAs(ScriptCodeEdit edit)
        {
            saveFileDialog.FilterIndex = 1;
            string oldFileName = edit.FileName;
            if (!string.IsNullOrEmpty(edit.FileName))
            {
                saveFileDialog.FileName = edit.FileName;
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return false;

            string fileName = saveFileDialog.FileName;

            if (oldFileName != fileName)
            {
                edit.SaveFile(fileName);
                edit.FileName = fileName;
                UpdatePage(editorsTabControl.SelectedTab, fileName, edit.Modified);
            }

            return true;
        }
        #endregion

        #region Toolbar, Statusbar and event handlers

        private void UpdateButtons()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        private void UpdateStatusBar()
        {
            UpdateEditorStatus();
        }

        private void UpdateControls()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        private void DelayedUpdateCodeWindows()
        {
            ScriptCodeEdit edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                updateCount++;
                try
                {
                    ISyntaxParser parser = edit.Lexer as ISyntaxParser;
                    var tree = parser?.SyntaxTree;
                    CodeUtils.FillClasses(classesComboBox, parser, edit.Position);
                    CodeUtils.FillMethods(methodsComboBox, parser, edit.Position, classesComboBox);
                    codeExplorer.UpdateExplorer(tree);
                }
                finally
                {
                    updateCount--;
                }
            }
            else
            {
                classesComboBox.Items.Clear();
                methodsComboBox.Items.Clear();
                codeExplorer.UpdateExplorer(null);
            }

            UpdateButtons();
        }

        private void UpdateCodeWindows()
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                updateTimer.Stop();
                updateTimer.Start();
            }
            else
                DelayedUpdateCodeWindows();
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

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetDisplayFilesForProject(IList<string> displayFiles, IronPythonProject project, IList<string> files)
        {
            bool projectAdded = false;
            int i = 0;
            while (i < files.Count)
            {
                string file = files[i];
                if (!IsProjectFile(file) && FileBelongsToProject(project, file))
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

        #endregion
    }
}
