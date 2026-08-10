#region Copyright (c) 2016-2026 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;

namespace AlternetStudio.Demo
{
    public partial class MainForm : Form
    {
        static MainForm()
        {
            SyntaxEdit.InstanceCreated += (s, e) =>
            {
                if (s is not SyntaxEdit editor)
                    return;

                bool searchInSeparateWindow = false;

                if (searchInSeparateWindow)
                    editor.SearchDialogAppearance = SearchDialogAppearance.SeparateWindow;
            };

            Utilities.DoApplicationEvents += (s, e) =>
            {
                Application.DoEvents();
            };
        }

        public MainForm()
        {
            InitializeScripter();
            InitializeEditors();
            InitializeComponent();

            positionStatusLabel.Visible = false;
            modifiedStatusLabel.Visible = false;
            overwriteStatusLabel.Visible = false;

            ActivateDarkTheme();

            var asm = this.GetType().Assembly;
            var prefix = "AlternetStudio.Demo.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            InitImages();
            InitializeCodeNavigationBar();
            InitEditorsContextMenu();
            InitializeFormDesigner();
            BookMarkManager.SharedBookMarks.Activate += new EventHandler<ActivateEventArgs>(DoActivate);
            BookMarkManager.SharedBookMarks.BookMarkAdded += SharedBookMarks_BookMarkAdded;
            BookMarkManager.SharedBookMarks.BookMarkRemoved += SharedBookMarks_BookMarkRemoved;
            ScaleControls();

            if (Consts.IsDebugDefinedAndAttached)
            {
                KeyPreview = true;
            }

            ControlUtilities.BindFormInitializer(this, () =>
            {
                using var tempItem = ControlUtilities.ShowTempStatus(statusStrip, "Loading...");
                InitializeCodeSearch();
                InitializeExplorerTrees();
                InitializeToolbar();
                LocateStartupDirectory();
                LoadStartupFile();
                UpdateControls();
                InitializeNavigationHistory();
                InitializeDebugControls();
                InitializeFileProperties();
                ActiveSyntaxEdit?.Focus();
            });
            this.Activated += MainForm_Activated;
        }

        public static void ShowLicenseDialog()
        {
            var dialog = new Alternet.Common.License.LicenseDialog();
            dialog.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            FinalizeCodeSearch();
            AutoSaveToolbox();
            AutoSaveRecentFiles();
            base.OnFormClosing(e);
        }

        private static ImageList LoadImageList(string resource)
        {
            string resName;
            resName = string.Format("AlternetStudio.Demo.Resources.{0}.png", resource);
            return ImageListHelper.LoadImageListFromStrip(typeof(MainForm).Assembly, resName);
        }

        private static Image LoadImage(string imageName)
        {
            Func<string, Image> getImage = name => Image.FromStream(
                typeof(MainForm).Assembly.GetManifestResourceStream(
                    string.Format("AlternetStudio.Demo.Resources.{0}.png", name)));

            var result = new DisplayScaledImage(
                    () => getImage(imageName),
                    () => getImage(imageName + "_HighDpi")).Image;

            var isDarkMode = SyntaxEdit.IsDarkModeEnabled();

            if (isDarkMode && result is Bitmap bitmap)
            {
                var darkImage = ImageRecolorUtils.DefaultRecolorIcon(bitmap);
                return darkImage;
            }

            return result;
        }

        private static void SetTabPageColor(TabPage tp, Color color)
        {
            tp.UseVisualStyleBackColor = false;
            tp.BackColor = color;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)(() => ProcessModifiedProjects()));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void SetTabPageColor(TabPage tp)
        {
            SetTabPageColor(tp, BackColor);
        }

        private void ActivateDarkTheme()
        {
            if (IsDark)
            {
                Color bc = BackColor;

                SetTabPageColor(breakpointsTabPage, bc);
                SetTabPageColor(outputTabPage, bc);
                SetTabPageColor(findResultsTabPage, bc);
                SetTabPageColor(callStackTabPage, bc);
                SetTabPageColor(localsTabPage, bc);
                SetTabPageColor(watchesTabPage, bc);
                SetTabPageColor(errorsTabPage, bc);
                SetTabPageColor(threadsTabPage, bc);
                SetTabPageColor(projectExplorerTabPage, bc);
                SetTabPageColor(propertiesTabPage, bc);
                SetTabPageColor(toolboxTabPage, bc);
                SetTabPageColor(projectExplorerTabPage, bc);
                SetTabPageColor(codeExplorerTabPage, bc);
                SetTabPageColor(outlineTabPage, bc);

                callStackControl.lvCallStack.GridLines = false;
                errorsControl.lvErrorList.GridLines = false;
                threadsControl.lvThreads.GridLines = false;
                breakpointsControl.lvBreakpoints.GridLines = false;
                callStackControl.lvCallStack.GridLines = false;
                errorsControl.lvErrorList.GridLines = false;
                findResultsControl.lvFindResults.GridLines = false;
            }
            else
            {
            }

            findResultsControl.lvFindResults.BorderStyle = BorderStyle.None;
            threadsControl.lvThreads.BorderStyle = BorderStyle.None;
            breakpointsControl.lvBreakpoints.BorderStyle = BorderStyle.None;
            callStackControl.lvCallStack.BorderStyle = BorderStyle.None;
            errorsControl.lvErrorList.BorderStyle = BorderStyle.None;
            localsControl.localsTreeView.BorderStyle = BorderStyle.None;
            watchesControl.watchesTreeView.BorderStyle = BorderStyle.None;

            projectExplorerTreeView.BorderStyle = BorderStyle.None;
            codeExplorerTreeView.BorderStyle = BorderStyle.None;
        }

        private void InitImageLists()
        {
            const string ProjectExplorerSmallDarkImages = "ProjectExplorerImages.16.Dark";
            const string ProjectExplorerSmallLightImages = "ProjectExplorerImages.16";
            const string ProjectExplorerLargeDarkImages = "ProjectExplorerImages.32.Dark";
            const string ProjectExplorerLargeLightImages = "ProjectExplorerImages.32";

            const string CodeExplorerSmallDarkImages = "CodeExplorerImages.16.Dark";
            const string CodeExplorerSmallLightImages = "CodeExplorerImages.16";
            const string CodeExplorerLargeDarkImages = "CodeExplorerImages.32.Dark";
            const string CodeExplorerLargeLightImages = "CodeExplorerImages.32";

            bool largeImages = DisplayScaling.NeedImageScaling;

            if (IsDark)
            {
                if (largeImages)
                {
                    projectExplorerTreeView.ImageList = LoadImageList(ProjectExplorerLargeDarkImages);
                    codeExplorerTreeView.ImageList = LoadImageList(CodeExplorerLargeDarkImages);
                }
                else
                {
                    projectExplorerTreeView.ImageList = LoadImageList(ProjectExplorerSmallDarkImages);
                    codeExplorerTreeView.ImageList = LoadImageList(CodeExplorerSmallDarkImages);
                }
            }
            else
            {
                if (largeImages)
                {
                    projectExplorerTreeView.ImageList = LoadImageList(ProjectExplorerLargeLightImages);
                    codeExplorerTreeView.ImageList = LoadImageList(CodeExplorerLargeLightImages);
                }
                else
                {
                    projectExplorerTreeView.ImageList = LoadImageList(ProjectExplorerSmallLightImages);
                    codeExplorerTreeView.ImageList = LoadImageList(CodeExplorerSmallLightImages);
                }
            }
        }

        private void InitImages()
        {
            InitImageLists();

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