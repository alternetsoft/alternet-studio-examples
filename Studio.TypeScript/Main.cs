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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.Common;
using Alternet.Editor.TextSource;
using Alternet.Editor.TypeScript;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm : Form
    {
        #region Private Members

        private const int SizeGap = 6;

        private bool projectIsClosing;
        private string dir = Application.StartupPath + @"\";
        #endregion

        public MainForm(string[] args)
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
            ScaleControls();
        }

        protected override void OnClosed(EventArgs e)
        {
            FinalizeCodeSearch();

            base.OnClosed(e);

            if (debugger != null)
                debugger.ClearTemporaryGeneratedModules();
        }

        private static Image LoadImage(string imageName)
        {
            Func<string, Image> getImage = name => Image.FromStream(
                typeof(MainForm).Assembly.GetManifestResourceStream(
                    string.Format("AlternetStudio.Demo.TypeScript.Resources.{0}.png", name)));

            return new DisplayScaledImage(
                    () => getImage(imageName),
                    () => getImage(imageName + "_HighDpi")).Image;
        }

        private void InitImages()
        {
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

            codeExplorerTreeView.ImageList = DisplayScaling.CloneAndAutoScaleImageList(codeExplorerTreeView.ImageList);
            projectExplorerTreeView.ImageList = DisplayScaling.CloneAndAutoScaleImageList(projectExplorerTreeView.ImageList);
            imageList = DisplayScaling.CloneAndAutoScaleImageList(imageList1);
            codeNavigationBarPanel.Height = methodsComboBox.Height + 2;
        }

        private void UpdateControls()
        {
            UpdateEditorButtons();
            UpdateStatusBar();
            UpdateDebugButtons();
        }

        #region Scripter and Debugger

        private ScriptLanguage GetScriptLanguage(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension == ".ts")
                return ScriptLanguage.TypeScript;
            if (extension == ".js")
                return ScriptLanguage.JavaScript;
            throw new Exception();
        }

        private void Dialog_WatchAdded(object sender, WatchAddedEventArgs e)
        {
            ActivateWatchesTab();
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

        private void UpdateSyntax()
        {
            foreach (TabPage page in editors.Keys)
            {
                IScriptEdit edit = GetEditor(page);
                if (edit != null)
                    edit.UpdateSyntax();
            }
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

        private bool CanBeMoved(TreeNode node, bool direction)
        {
            if (node.Level != 1)
                return false;

            TreeNode next = direction ? node.PrevNode : node.NextNode;
            return next != null && IsFileNode(next);
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

        private void Panel1_Resize(object sender, EventArgs e)
        {
            int newWidth = (codeNavigationBarPanel.ClientSize.Width - SizeGap) / 2;
            classesComboBox.Width = newWidth;
            methodsComboBox.Width = newWidth;
        }

        private void OnDrawItem(ComboBox combo, DrawItemEventArgs e)
        {
            CodeUtils.DrawItem(combo, e, imageList1);
        }

        #endregion
    }
}
