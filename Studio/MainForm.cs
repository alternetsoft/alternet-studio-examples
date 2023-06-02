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
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;

namespace AlternetStudio.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
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

        protected override void OnClosed(System.EventArgs e)
        {
            FinalizeCodeSearch();
            base.OnClosed(e);
        }

        private static Image LoadImage(string imageName)
        {
            Func<string, Image> getImage = name => Image.FromStream(
                typeof(MainForm).Assembly.GetManifestResourceStream(
                    string.Format("AlternetStudio.Demo.Resources.{0}.png", name)));

            return new DisplayScaledImage(
                    () => getImage(imageName),
                    () => getImage(imageName + "_HighDpi")).Image;
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
            InitializeFileProperties();
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

            referencesContextMenu.ShowCheckMargin = true;

            codeExplorerTreeView.ImageList = DisplayScaling.CloneAndAutoScaleImageList(codeExplorerTreeView.ImageList);
            projectExplorerTreeView.ImageList = DisplayScaling.CloneAndAutoScaleImageList(projectExplorerTreeView.ImageList);
            imageList1 = DisplayScaling.CloneAndAutoScaleImageList(imageList1);
            codeNavigationBarPanel.Height = methodsComboBox.Height + 2;
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

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            using (var aboutBox = new AboutDialog())
                aboutBox.ShowDialog();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}