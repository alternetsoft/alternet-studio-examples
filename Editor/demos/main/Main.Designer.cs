namespace Alternet.CodeEditor.Demo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]

    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Gutter");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Margin");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Line Numbers");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Line Styles");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Visual Themes");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Miscellaneous");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Pages & Rulers");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Appearance", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27,
            treeNode28,
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Outlining");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Text Source");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Navigation");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Selection");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("WordWrap & Scrolling");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Spelling & HyperText");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Scroll Bar Annotations");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Behavior", new System.Windows.Forms.TreeNode[] {
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35,
            treeNode36,
            treeNode37});
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Common Dialogs");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Printing & Exporting");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Dialogs", new System.Windows.Forms.TreeNode[] {
            treeNode39,
            treeNode40});
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Properties");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("About");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Company Info", new System.Windows.Forms.TreeNode[] {
            treeNode43});
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btPrintSetup = new System.Windows.Forms.Button();
            this.btPrintOptions = new System.Windows.Forms.Button();
            this.chbPersistentBlocks = new System.Windows.Forms.CheckBox();
            this.btPrint = new System.Windows.Forms.Button();
            this.HighlightSelectedWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.chbOverwriteBlocks = new System.Windows.Forms.CheckBox();
            this.btPrintPreview = new System.Windows.Forms.Button();
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.chbSelectLineOnDblClick = new System.Windows.Forms.CheckBox();
            this.chbHideSelection = new System.Windows.Forms.CheckBox();
            this.chbUseColors = new System.Windows.Forms.CheckBox();
            this.chbSelectBeyondEol = new System.Windows.Forms.CheckBox();
            this.chbDisableDragging = new System.Windows.Forms.CheckBox();
            this.chbDisableSelection = new System.Windows.Forms.CheckBox();
            this.gbNavigateOptions = new System.Windows.Forms.GroupBox();
            this.chbMoveOnRightButton = new System.Windows.Forms.CheckBox();
            this.chbDownAtLineEnd = new System.Windows.Forms.CheckBox();
            this.chbUpAtLineBegin = new System.Windows.Forms.CheckBox();
            this.chbBeyondEof = new System.Windows.Forms.CheckBox();
            this.chbBeyondEol = new System.Windows.Forms.CheckBox();
            this.pnNavigate = new System.Windows.Forms.Panel();
            this.tpNavigate = new System.Windows.Forms.TabPage();
            this.pnSelection = new System.Windows.Forms.Panel();
            this.chbAllowOutlining = new System.Windows.Forms.CheckBox();
            this.gbOutlining = new System.Windows.Forms.GroupBox();
            this.chbDrawButtons = new System.Windows.Forms.CheckBox();
            this.chbDrawLines = new System.Windows.Forms.CheckBox();
            this.chbDrawOnGutter = new System.Windows.Forms.CheckBox();
            this.chbShowHints = new System.Windows.Forms.CheckBox();
            this.pnOutlining = new System.Windows.Forms.Panel();
            this.tpOutlining = new System.Windows.Forms.TabPage();
            this.chbShowHyperTextHints = new System.Windows.Forms.CheckBox();
            this.chbCheckSpelling = new System.Windows.Forms.CheckBox();
            this.tpSelection = new System.Windows.Forms.TabPage();
            this.btHtml = new System.Windows.Forms.Button();
            this.gbPrint = new System.Windows.Forms.GroupBox();
            this.btRtf = new System.Windows.Forms.Button();
            this.pnPrinting = new System.Windows.Forms.Panel();
            this.tpPrinting = new System.Windows.Forms.TabPage();
            this.pnDialogs = new System.Windows.Forms.Panel();
            this.gbDialogs = new System.Windows.Forms.GroupBox();
            this.btGoto = new System.Windows.Forms.Button();
            this.btReplace = new System.Windows.Forms.Button();
            this.FindNextButton = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.tpDialogs = new System.Windows.Forms.TabPage();
            this.chbQuickInfoTips = new System.Windows.Forms.CheckBox();
            this.chbTransparent = new System.Windows.Forms.CheckBox();
            this.chbHighlightReferences = new System.Windows.Forms.CheckBox();
            this.cbTempHighlightBraces = new System.Windows.Forms.CheckBox();
            this.chbSeparateLines = new System.Windows.Forms.CheckBox();
            this.chbWhiteSpaceVisible = new System.Windows.Forms.CheckBox();
            this.chbUseRoundRect = new System.Windows.Forms.CheckBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chbDrawColumnsIndent = new System.Windows.Forms.CheckBox();
            this.chbLineModificator = new System.Windows.Forms.CheckBox();
            this.laLineNumbersStart = new System.Windows.Forms.Label();
            this.gbOther = new System.Windows.Forms.GroupBox();
            this.nudLineNumbersStart = new System.Windows.Forms.NumericUpDown();
            this.cbLineNumbersAlign = new System.Windows.Forms.ComboBox();
            this.pnLineNumbers = new System.Windows.Forms.Panel();
            this.gbLineNumbers = new System.Windows.Forms.GroupBox();
            this.laLineNumbersAlign = new System.Windows.Forms.Label();
            this.chbLinesOnGutter = new System.Windows.Forms.CheckBox();
            this.chbLineNumbers = new System.Windows.Forms.CheckBox();
            this.chbLinesBeyondEof = new System.Windows.Forms.CheckBox();
            this.chbHighlightCurrentLine = new System.Windows.Forms.CheckBox();
            this.pnLineStyles = new System.Windows.Forms.Panel();
            this.btStepOver = new System.Windows.Forms.Button();
            this.btSetBreakpoint = new System.Windows.Forms.Button();
            this.tpLineNumbers = new System.Windows.Forms.TabPage();
            this.tpLineStyles = new System.Windows.Forms.TabPage();
            this.gbBraces = new System.Windows.Forms.GroupBox();
            this.chbHighlightBraces = new System.Windows.Forms.CheckBox();
            this.pnOther = new System.Windows.Forms.Panel();
            this.tpOther = new System.Windows.Forms.TabPage();
            this.pnProperties = new System.Windows.Forms.Panel();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.tbCompanyInfo = new System.Windows.Forms.TextBox();
            this.chbHighlightUrls = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pnCompanyInfo = new System.Windows.Forms.Panel();
            this.laMailTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.laAdress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btShowBookmarks = new System.Windows.Forms.Button();
            this.tpCompanyInfo = new System.Windows.Forms.TabPage();
            this.chbDrawLineBookmarks = new System.Windows.Forms.CheckBox();
            this.tpMargin = new System.Windows.Forms.TabPage();
            this.pnMargin = new System.Windows.Forms.Panel();
            this.gbMargin = new System.Windows.Forms.GroupBox();
            this.chbColumnsVisible = new System.Windows.Forms.CheckBox();
            this.chbShowMarginHints = new System.Windows.Forms.CheckBox();
            this.chbAllowDragMargin = new System.Windows.Forms.CheckBox();
            this.nudMarginPos = new System.Windows.Forms.NumericUpDown();
            this.laMarginPos = new System.Windows.Forms.Label();
            this.chbShowMargin = new System.Windows.Forms.CheckBox();
            this.chbPaintBookMarks = new System.Windows.Forms.CheckBox();
            this.syntaxEdit = new Alternet.Editor.SyntaxEdit(this.components);
            this.textSource1 = new Alternet.Editor.TextSource.TextSource(this.components);
            this.gbGutter = new System.Windows.Forms.GroupBox();
            this.laGutterWidth = new System.Windows.Forms.Label();
            this.nudGutterWidth = new System.Windows.Forms.NumericUpDown();
            this.chbShowGutter = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnMain = new System.Windows.Forms.Panel();
            this.pnEditContainer = new System.Windows.Forms.Panel();
            this.syntaxSplitEdit = new Alternet.Editor.SyntaxEdit(this.components);
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnPropertyGrid = new System.Windows.Forms.Panel();
            this.pnManage = new System.Windows.Forms.Panel();
            this.tcContainer = new System.Windows.Forms.TabControl();
            this.tpGutter = new System.Windows.Forms.TabPage();
            this.pnGutter = new System.Windows.Forms.Panel();
            this.tpWordWrap = new System.Windows.Forms.TabPage();
            this.pnWordWrap = new System.Windows.Forms.Panel();
            this.gbWordWrap = new System.Windows.Forms.GroupBox();
            this.chbFlatScrollBars = new System.Windows.Forms.CheckBox();
            this.chbSystemScrollBars = new System.Windows.Forms.CheckBox();
            this.chbScrollButtons = new System.Windows.Forms.CheckBox();
            this.chbAllowSplit = new System.Windows.Forms.CheckBox();
            this.chbWrapAtMargin = new System.Windows.Forms.CheckBox();
            this.chbWordWrap = new System.Windows.Forms.CheckBox();
            this.chbShowScrollHint = new System.Windows.Forms.CheckBox();
            this.chbSmoothScroll = new System.Windows.Forms.CheckBox();
            this.tpTextSource = new System.Windows.Forms.TabPage();
            this.pnTextSource = new System.Windows.Forms.Panel();
            this.laSource = new System.Windows.Forms.Label();
            this.tpPageLayout = new System.Windows.Forms.TabPage();
            this.tpTextMate = new System.Windows.Forms.TabPage();
            this.pnPageLayout = new System.Windows.Forms.Panel();
            this.pnTextMate = new System.Windows.Forms.Panel();
            this.gbRulers = new System.Windows.Forms.GroupBox();
            this.chbVertRuler = new System.Windows.Forms.CheckBox();
            this.chbHorzRuler = new System.Windows.Forms.CheckBox();
            this.cbRulerUnits = new System.Windows.Forms.ComboBox();
            this.laRulerUnits = new System.Windows.Forms.Label();
            this.chbRulerDisplayDragLines = new System.Windows.Forms.CheckBox();
            this.chbRulerDiscrete = new System.Windows.Forms.CheckBox();
            this.chbRulerAllowDrag = new System.Windows.Forms.CheckBox();
            this.gbPages = new System.Windows.Forms.GroupBox();
            this.cbPageSize = new System.Windows.Forms.ComboBox();
            this.laPageSize = new System.Windows.Forms.Label();
            this.cbPageLayout = new System.Windows.Forms.ComboBox();
            this.laPageLayout = new System.Windows.Forms.Label();
            this.tpSpellAndUrl = new System.Windows.Forms.TabPage();
            this.pnSpellAndUrl = new System.Windows.Forms.Panel();
            this.gbSpellAndUrl = new System.Windows.Forms.GroupBox();
            this.tpScrollbarAnnotations = new System.Windows.Forms.TabPage();
            this.pnScrollbarAnnotations = new System.Windows.Forms.Panel();
            this.separatorTextBox = new System.Windows.Forms.TextBox();
            this.changeErrorsAppearanceCheckBox = new System.Windows.Forms.CheckBox();
            this.customAnnotationsCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.scrollBarsVisualStyleComboBox = new System.Windows.Forms.ComboBox();
            this.scrollBarsVisualStyleLabel = new System.Windows.Forms.Label();
            this.customTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.cursorPositionTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.syntaxErrorsTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.searchResultsTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.bookmarksTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.changedLinesTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.scrollBarAnnotationsEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tpVisualThemes = new System.Windows.Forms.TabPage();
            this.pnVisualThemes = new System.Windows.Forms.Panel();
            this.visualThemeComboBox = new System.Windows.Forms.ComboBox();
            this.visualThemeLabel = new System.Windows.Forms.Label();
            this.LanguagesCombobox = new System.Windows.Forms.ComboBox();
            this.LanguagesLabel = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.gbSelection.SuspendLayout();
            this.gbNavigateOptions.SuspendLayout();
            this.pnNavigate.SuspendLayout();
            this.tpNavigate.SuspendLayout();
            this.pnSelection.SuspendLayout();
            this.gbOutlining.SuspendLayout();
            this.pnOutlining.SuspendLayout();
            this.tpOutlining.SuspendLayout();
            this.tpSelection.SuspendLayout();
            this.gbPrint.SuspendLayout();
            this.pnPrinting.SuspendLayout();
            this.tpPrinting.SuspendLayout();
            this.pnDialogs.SuspendLayout();
            this.gbDialogs.SuspendLayout();
            this.tpDialogs.SuspendLayout();
            this.gbOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersStart)).BeginInit();
            this.pnLineNumbers.SuspendLayout();
            this.gbLineNumbers.SuspendLayout();
            this.pnLineStyles.SuspendLayout();
            this.tpLineNumbers.SuspendLayout();
            this.tpLineStyles.SuspendLayout();
            this.gbBraces.SuspendLayout();
            this.pnOther.SuspendLayout();
            this.tpOther.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.pnCompanyInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tpCompanyInfo.SuspendLayout();
            this.tpMargin.SuspendLayout();
            this.pnMargin.SuspendLayout();
            this.gbMargin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarginPos)).BeginInit();
            this.gbGutter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGutterWidth)).BeginInit();
            this.pnMain.SuspendLayout();
            this.pnEditContainer.SuspendLayout();
            this.pnManage.SuspendLayout();
            this.tcContainer.SuspendLayout();
            this.tpGutter.SuspendLayout();
            this.pnGutter.SuspendLayout();
            this.tpWordWrap.SuspendLayout();
            this.pnWordWrap.SuspendLayout();
            this.gbWordWrap.SuspendLayout();
            this.tpTextSource.SuspendLayout();
            this.pnTextSource.SuspendLayout();
            this.tpPageLayout.SuspendLayout();
            this.tpTextMate.SuspendLayout();
            this.pnPageLayout.SuspendLayout();
            this.pnTextMate.SuspendLayout();
            this.gbRulers.SuspendLayout();
            this.gbPages.SuspendLayout();
            this.tpSpellAndUrl.SuspendLayout();
            this.pnSpellAndUrl.SuspendLayout();
            this.gbSpellAndUrl.SuspendLayout();
            this.tpScrollbarAnnotations.SuspendLayout();
            this.pnScrollbarAnnotations.SuspendLayout();
            this.tpVisualThemes.SuspendLayout();
            this.pnVisualThemes.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            // 
            // btPrintSetup
            // 
            this.btPrintSetup.BackColor = System.Drawing.SystemColors.Control;
            this.btPrintSetup.Location = new System.Drawing.Point(267, 69);
            this.btPrintSetup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPrintSetup.Name = "btPrintSetup";
            this.btPrintSetup.Size = new System.Drawing.Size(107, 28);
            this.btPrintSetup.TabIndex = 11;
            this.btPrintSetup.Text = "Page Setup";
            this.btPrintSetup.UseVisualStyleBackColor = false;
            this.btPrintSetup.Click += new System.EventHandler(this.PageSetupButton_Click);
            this.btPrintSetup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintSetupButton_MouseMove);
            // 
            // btPrintOptions
            // 
            this.btPrintOptions.BackColor = System.Drawing.SystemColors.Control;
            this.btPrintOptions.Location = new System.Drawing.Point(267, 30);
            this.btPrintOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPrintOptions.Name = "btPrintOptions";
            this.btPrintOptions.Size = new System.Drawing.Size(107, 28);
            this.btPrintOptions.TabIndex = 8;
            this.btPrintOptions.Text = "Print Options";
            this.btPrintOptions.UseVisualStyleBackColor = false;
            this.btPrintOptions.Click += new System.EventHandler(this.PrintOptionsButton_Click);
            this.btPrintOptions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintOptionsButton_MouseMove);
            // 
            // chbPersistentBlocks
            // 
            this.chbPersistentBlocks.Location = new System.Drawing.Point(395, 49);
            this.chbPersistentBlocks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbPersistentBlocks.Name = "chbPersistentBlocks";
            this.chbPersistentBlocks.Size = new System.Drawing.Size(149, 26);
            this.chbPersistentBlocks.TabIndex = 12;
            this.chbPersistentBlocks.Text = "Persistent Blocks";
            this.chbPersistentBlocks.CheckedChanged += new System.EventHandler(this.PersistenlocksCheckBoxTextBox_CheckedChanged);
            this.chbPersistentBlocks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PersistenlocksCheckBoxTextBox_MouseMove);
            // 
            // btPrint
            // 
            this.btPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btPrint.Location = new System.Drawing.Point(139, 69);
            this.btPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(107, 28);
            this.btPrint.TabIndex = 10;
            this.btPrint.Text = "Print";
            this.btPrint.UseVisualStyleBackColor = false;
            this.btPrint.Click += new System.EventHandler(this.PrinuttonTextBox_Click);
            this.btPrint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrinuttonTextBox_MouseMove);
            // 
            // HighlightSelectedWordsCheckBox
            // 
            this.HighlightSelectedWordsCheckBox.Location = new System.Drawing.Point(395, 20);
            this.HighlightSelectedWordsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.HighlightSelectedWordsCheckBox.Name = "HighlightSelectedWordsCheckBox";
            this.HighlightSelectedWordsCheckBox.Size = new System.Drawing.Size(203, 26);
            this.HighlightSelectedWordsCheckBox.TabIndex = 11;
            this.HighlightSelectedWordsCheckBox.Text = "Highlight Selected Words";
            this.HighlightSelectedWordsCheckBox.CheckedChanged += new System.EventHandler(this.HighlightSelectedWordsCheckBox_CheckedChanged);
            this.HighlightSelectedWordsCheckBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightSelectedWordsCheckBox_MouseMove);
            // 
            // chbOverwriteBlocks
            // 
            this.chbOverwriteBlocks.Location = new System.Drawing.Point(395, 79);
            this.chbOverwriteBlocks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbOverwriteBlocks.Name = "chbOverwriteBlocks";
            this.chbOverwriteBlocks.Size = new System.Drawing.Size(149, 26);
            this.chbOverwriteBlocks.TabIndex = 13;
            this.chbOverwriteBlocks.Text = "Overwrite Blocks";
            this.chbOverwriteBlocks.CheckedChanged += new System.EventHandler(this.OverwriteBlocksCheckBox_CheckedChanged);
            this.chbOverwriteBlocks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OverwriteBlocksCheckBox_MouseMove);
            // 
            // btPrintPreview
            // 
            this.btPrintPreview.BackColor = System.Drawing.SystemColors.Control;
            this.btPrintPreview.Location = new System.Drawing.Point(11, 69);
            this.btPrintPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btPrintPreview.Name = "btPrintPreview";
            this.btPrintPreview.Size = new System.Drawing.Size(107, 28);
            this.btPrintPreview.TabIndex = 9;
            this.btPrintPreview.Text = "Print Preview";
            this.btPrintPreview.UseVisualStyleBackColor = false;
            this.btPrintPreview.Click += new System.EventHandler(this.PrintPreviewButton_Click);
            this.btPrintPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintPreviewButton_MouseMove);
            // 
            // gbSelection
            // 
            this.gbSelection.Controls.Add(this.chbOverwriteBlocks);
            this.gbSelection.Controls.Add(this.chbPersistentBlocks);
            this.gbSelection.Controls.Add(this.HighlightSelectedWordsCheckBox);
            this.gbSelection.Controls.Add(this.chbSelectLineOnDblClick);
            this.gbSelection.Controls.Add(this.chbHideSelection);
            this.gbSelection.Controls.Add(this.chbUseColors);
            this.gbSelection.Controls.Add(this.chbSelectBeyondEol);
            this.gbSelection.Controls.Add(this.chbDisableDragging);
            this.gbSelection.Controls.Add(this.chbDisableSelection);
            this.gbSelection.Location = new System.Drawing.Point(11, 10);
            this.gbSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSelection.Size = new System.Drawing.Size(661, 118);
            this.gbSelection.TabIndex = 2;
            this.gbSelection.TabStop = false;
            this.gbSelection.Text = "Selection Options";
            // 
            // chbSelectLineOnDblClick
            // 
            this.chbSelectLineOnDblClick.Location = new System.Drawing.Point(192, 79);
            this.chbSelectLineOnDblClick.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbSelectLineOnDblClick.Name = "chbSelectLineOnDblClick";
            this.chbSelectLineOnDblClick.Size = new System.Drawing.Size(187, 26);
            this.chbSelectLineOnDblClick.TabIndex = 10;
            this.chbSelectLineOnDblClick.Text = "Select Line on DblClick";
            this.chbSelectLineOnDblClick.CheckedChanged += new System.EventHandler(this.SelectLineOnDblClickCheckBox_CheckedChanged);
            this.chbSelectLineOnDblClick.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectLineOnDblClickCheckBox_MouseMove);
            // 
            // chbHideSelection
            // 
            this.chbHideSelection.Location = new System.Drawing.Point(192, 49);
            this.chbHideSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHideSelection.Name = "chbHideSelection";
            this.chbHideSelection.Size = new System.Drawing.Size(139, 26);
            this.chbHideSelection.TabIndex = 9;
            this.chbHideSelection.Text = "Hide Selection";
            this.chbHideSelection.CheckedChanged += new System.EventHandler(this.HideSelectionCheckBox_CheckedChanged);
            this.chbHideSelection.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HideSelectionCheckBox_MouseMove);
            // 
            // chbUseColors
            // 
            this.chbUseColors.Location = new System.Drawing.Point(192, 20);
            this.chbUseColors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbUseColors.Name = "chbUseColors";
            this.chbUseColors.Size = new System.Drawing.Size(139, 26);
            this.chbUseColors.TabIndex = 8;
            this.chbUseColors.Text = "Use Colors";
            this.chbUseColors.CheckedChanged += new System.EventHandler(this.UseColorsCheckBox_CheckedChanged);
            this.chbUseColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UseColorsCheckBox_MouseMove);
            // 
            // chbSelectBeyondEol
            // 
            this.chbSelectBeyondEol.Location = new System.Drawing.Point(11, 79);
            this.chbSelectBeyondEol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbSelectBeyondEol.Name = "chbSelectBeyondEol";
            this.chbSelectBeyondEol.Size = new System.Drawing.Size(160, 26);
            this.chbSelectBeyondEol.TabIndex = 7;
            this.chbSelectBeyondEol.Text = "Select Beyond Eol";
            this.chbSelectBeyondEol.CheckedChanged += new System.EventHandler(this.SeleceyondEolCheckBoxTextBox_CheckedChanged);
            this.chbSelectBeyondEol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SeleceyondEolCheckBoxTextBox_MouseMove);
            // 
            // chbDisableDragging
            // 
            this.chbDisableDragging.Location = new System.Drawing.Point(11, 49);
            this.chbDisableDragging.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDisableDragging.Name = "chbDisableDragging";
            this.chbDisableDragging.Size = new System.Drawing.Size(149, 26);
            this.chbDisableDragging.TabIndex = 6;
            this.chbDisableDragging.Text = "Disable Dragging";
            this.chbDisableDragging.CheckedChanged += new System.EventHandler(this.DisableDraggingCheckBox_CheckedChanged);
            this.chbDisableDragging.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisableDraggingCheckBox_MouseMove);
            // 
            // chbDisableSelection
            // 
            this.chbDisableSelection.Location = new System.Drawing.Point(11, 20);
            this.chbDisableSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDisableSelection.Name = "chbDisableSelection";
            this.chbDisableSelection.Size = new System.Drawing.Size(149, 26);
            this.chbDisableSelection.TabIndex = 5;
            this.chbDisableSelection.Text = "Disable Selection";
            this.chbDisableSelection.CheckedChanged += new System.EventHandler(this.DisableSelectionCheckBox_CheckedChanged);
            this.chbDisableSelection.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisableSelectionCheckBox_MouseMove);
            // 
            // gbNavigateOptions
            // 
            this.gbNavigateOptions.Controls.Add(this.chbMoveOnRightButton);
            this.gbNavigateOptions.Controls.Add(this.chbDownAtLineEnd);
            this.gbNavigateOptions.Controls.Add(this.chbUpAtLineBegin);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEof);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEol);
            this.gbNavigateOptions.Location = new System.Drawing.Point(11, 10);
            this.gbNavigateOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbNavigateOptions.Name = "gbNavigateOptions";
            this.gbNavigateOptions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbNavigateOptions.Size = new System.Drawing.Size(661, 118);
            this.gbNavigateOptions.TabIndex = 0;
            this.gbNavigateOptions.TabStop = false;
            this.gbNavigateOptions.Text = "Navigate Options";
            // 
            // chbMoveOnRightButton
            // 
            this.chbMoveOnRightButton.Location = new System.Drawing.Point(192, 79);
            this.chbMoveOnRightButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbMoveOnRightButton.Name = "chbMoveOnRightButton";
            this.chbMoveOnRightButton.Size = new System.Drawing.Size(181, 26);
            this.chbMoveOnRightButton.TabIndex = 4;
            this.chbMoveOnRightButton.Text = "Move on Right Button";
            this.chbMoveOnRightButton.CheckedChanged += new System.EventHandler(this.MoveOnRighuttonCheckBoxTextBox_CheckedChanged);
            this.chbMoveOnRightButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveOnRighuttonCheckBoxTextBox_MouseMove);
            // 
            // chbDownAtLineEnd
            // 
            this.chbDownAtLineEnd.Location = new System.Drawing.Point(192, 49);
            this.chbDownAtLineEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDownAtLineEnd.Name = "chbDownAtLineEnd";
            this.chbDownAtLineEnd.Size = new System.Drawing.Size(149, 26);
            this.chbDownAtLineEnd.TabIndex = 3;
            this.chbDownAtLineEnd.Text = "Down at Line End";
            this.chbDownAtLineEnd.CheckedChanged += new System.EventHandler(this.DownAtLineEndCheckBox_CheckedChanged);
            this.chbDownAtLineEnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DownAtLineEndCheckBox_MouseMove);
            // 
            // chbUpAtLineBegin
            // 
            this.chbUpAtLineBegin.Location = new System.Drawing.Point(192, 20);
            this.chbUpAtLineBegin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbUpAtLineBegin.Name = "chbUpAtLineBegin";
            this.chbUpAtLineBegin.Size = new System.Drawing.Size(149, 26);
            this.chbUpAtLineBegin.TabIndex = 2;
            this.chbUpAtLineBegin.Text = "Up at Line Begin";
            this.chbUpAtLineBegin.CheckedChanged += new System.EventHandler(this.UpAtLineBeginCheckBox_CheckedChanged);
            this.chbUpAtLineBegin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpAtLineBeginCheckBox_MouseMove);
            // 
            // chbBeyondEof
            // 
            this.chbBeyondEof.Location = new System.Drawing.Point(11, 49);
            this.chbBeyondEof.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbBeyondEof.Name = "chbBeyondEof";
            this.chbBeyondEof.Size = new System.Drawing.Size(139, 26);
            this.chbBeyondEof.TabIndex = 1;
            this.chbBeyondEof.Text = "Beyond Eof";
            this.chbBeyondEof.CheckedChanged += new System.EventHandler(this.BeyondEofCheckBox_CheckedChanged);
            this.chbBeyondEof.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BeyondEofCheckBox_MouseMove);
            // 
            // chbBeyondEol
            // 
            this.chbBeyondEol.Location = new System.Drawing.Point(11, 20);
            this.chbBeyondEol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbBeyondEol.Name = "chbBeyondEol";
            this.chbBeyondEol.Size = new System.Drawing.Size(139, 26);
            this.chbBeyondEol.TabIndex = 0;
            this.chbBeyondEol.Text = "Beyond Eol";
            this.chbBeyondEol.CheckedChanged += new System.EventHandler(this.BeyondEolCheckBox_CheckedChanged);
            this.chbBeyondEol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BeyondEolCheckBox_MouseMove);
            // 
            // pnNavigate
            // 
            this.pnNavigate.BackColor = System.Drawing.SystemColors.Control;
            this.pnNavigate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnNavigate.Controls.Add(this.gbNavigateOptions);
            this.pnNavigate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnNavigate.Location = new System.Drawing.Point(0, 0);
            this.pnNavigate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnNavigate.Name = "pnNavigate";
            this.pnNavigate.Size = new System.Drawing.Size(1081, 137);
            this.pnNavigate.TabIndex = 1;
            // 
            // tpNavigate
            // 
            this.tpNavigate.Controls.Add(this.pnNavigate);
            this.tpNavigate.Location = new System.Drawing.Point(4, 25);
            this.tpNavigate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpNavigate.Name = "tpNavigate";
            this.tpNavigate.Size = new System.Drawing.Size(1081, 148);
            this.tpNavigate.TabIndex = 7;
            this.tpNavigate.Text = "Navigate";
            this.tpNavigate.Visible = false;
            // 
            // pnSelection
            // 
            this.pnSelection.BackColor = System.Drawing.SystemColors.Control;
            this.pnSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnSelection.Controls.Add(this.gbSelection);
            this.pnSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSelection.Location = new System.Drawing.Point(0, 0);
            this.pnSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnSelection.Name = "pnSelection";
            this.pnSelection.Size = new System.Drawing.Size(1081, 137);
            this.pnSelection.TabIndex = 0;
            // 
            // chbAllowOutlining
            // 
            this.chbAllowOutlining.BackColor = System.Drawing.SystemColors.Control;
            this.chbAllowOutlining.Location = new System.Drawing.Point(11, 20);
            this.chbAllowOutlining.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbAllowOutlining.Name = "chbAllowOutlining";
            this.chbAllowOutlining.Size = new System.Drawing.Size(139, 26);
            this.chbAllowOutlining.TabIndex = 0;
            this.chbAllowOutlining.Text = "Allow Outlining";
            this.chbAllowOutlining.UseVisualStyleBackColor = false;
            this.chbAllowOutlining.CheckedChanged += new System.EventHandler(this.AllowOutliningCheckBox_CheckedChanged);
            this.chbAllowOutlining.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowOutliningCheckBox_MouseMove);
            // 
            // gbOutlining
            // 
            this.gbOutlining.BackColor = System.Drawing.SystemColors.Control;
            this.gbOutlining.Controls.Add(this.chbAllowOutlining);
            this.gbOutlining.Controls.Add(this.chbDrawButtons);
            this.gbOutlining.Controls.Add(this.chbDrawLines);
            this.gbOutlining.Controls.Add(this.chbDrawOnGutter);
            this.gbOutlining.Controls.Add(this.chbShowHints);
            this.gbOutlining.Location = new System.Drawing.Point(11, 10);
            this.gbOutlining.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbOutlining.Name = "gbOutlining";
            this.gbOutlining.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbOutlining.Size = new System.Drawing.Size(661, 118);
            this.gbOutlining.TabIndex = 5;
            this.gbOutlining.TabStop = false;
            this.gbOutlining.Text = "Outlining";
            // 
            // chbDrawButtons
            // 
            this.chbDrawButtons.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawButtons.Location = new System.Drawing.Point(192, 20);
            this.chbDrawButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDrawButtons.Name = "chbDrawButtons";
            this.chbDrawButtons.Size = new System.Drawing.Size(139, 26);
            this.chbDrawButtons.TabIndex = 3;
            this.chbDrawButtons.Text = "Draw Buttons";
            this.chbDrawButtons.UseVisualStyleBackColor = false;
            this.chbDrawButtons.CheckedChanged += new System.EventHandler(this.DrawButtonsCheckBox_CheckedChanged);
            this.chbDrawButtons.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawButtonsCheckBox_MouseMove);
            // 
            // chbDrawLines
            // 
            this.chbDrawLines.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawLines.Location = new System.Drawing.Point(11, 79);
            this.chbDrawLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDrawLines.Name = "chbDrawLines";
            this.chbDrawLines.Size = new System.Drawing.Size(139, 26);
            this.chbDrawLines.TabIndex = 2;
            this.chbDrawLines.Text = "Draw Lines";
            this.chbDrawLines.UseVisualStyleBackColor = false;
            this.chbDrawLines.CheckedChanged += new System.EventHandler(this.DrawLinesCheckBox_CheckedChanged);
            this.chbDrawLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawLinesCheckBox_MouseMove);
            // 
            // chbDrawOnGutter
            // 
            this.chbDrawOnGutter.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawOnGutter.Location = new System.Drawing.Point(11, 49);
            this.chbDrawOnGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDrawOnGutter.Name = "chbDrawOnGutter";
            this.chbDrawOnGutter.Size = new System.Drawing.Size(139, 26);
            this.chbDrawOnGutter.TabIndex = 1;
            this.chbDrawOnGutter.Text = "Draw on Gutter";
            this.chbDrawOnGutter.UseVisualStyleBackColor = false;
            this.chbDrawOnGutter.CheckedChanged += new System.EventHandler(this.DrawOnGutterCheckBox_CheckedChanged);
            this.chbDrawOnGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawOnGutterCheckBox_MouseMove);
            // 
            // chbShowHints
            // 
            this.chbShowHints.BackColor = System.Drawing.SystemColors.Control;
            this.chbShowHints.Location = new System.Drawing.Point(192, 49);
            this.chbShowHints.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowHints.Name = "chbShowHints";
            this.chbShowHints.Size = new System.Drawing.Size(139, 26);
            this.chbShowHints.TabIndex = 4;
            this.chbShowHints.Text = "Display Hints";
            this.chbShowHints.UseVisualStyleBackColor = false;
            this.chbShowHints.CheckedChanged += new System.EventHandler(this.ShowHintsCheckBox_CheckedChanged);
            this.chbShowHints.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowHintsCheckBox_MouseMove);
            // 
            // pnOutlining
            // 
            this.pnOutlining.BackColor = System.Drawing.SystemColors.Control;
            this.pnOutlining.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnOutlining.Controls.Add(this.gbOutlining);
            this.pnOutlining.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnOutlining.Location = new System.Drawing.Point(0, 0);
            this.pnOutlining.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnOutlining.Name = "pnOutlining";
            this.pnOutlining.Size = new System.Drawing.Size(1081, 137);
            this.pnOutlining.TabIndex = 3;
            // 
            // tpOutlining
            // 
            this.tpOutlining.Controls.Add(this.pnOutlining);
            this.tpOutlining.Location = new System.Drawing.Point(4, 25);
            this.tpOutlining.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpOutlining.Name = "tpOutlining";
            this.tpOutlining.Size = new System.Drawing.Size(1081, 148);
            this.tpOutlining.TabIndex = 4;
            this.tpOutlining.Text = "Outlining";
            this.tpOutlining.Visible = false;
            // 
            // chbShowHyperTextHints
            // 
            this.chbShowHyperTextHints.Location = new System.Drawing.Point(29, 78);
            this.chbShowHyperTextHints.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowHyperTextHints.Name = "chbShowHyperTextHints";
            this.chbShowHyperTextHints.Size = new System.Drawing.Size(139, 26);
            this.chbShowHyperTextHints.TabIndex = 3;
            this.chbShowHyperTextHints.Text = "Display Hints";
            this.chbShowHyperTextHints.CheckedChanged += new System.EventHandler(this.ShowHyperTextHintsCheckBox_CheckedChanged);
            this.chbShowHyperTextHints.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowHyperTextHintsCheckBox_MouseMove);
            // 
            // chbCheckSpelling
            // 
            this.chbCheckSpelling.Location = new System.Drawing.Point(11, 20);
            this.chbCheckSpelling.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbCheckSpelling.Name = "chbCheckSpelling";
            this.chbCheckSpelling.Size = new System.Drawing.Size(139, 26);
            this.chbCheckSpelling.TabIndex = 0;
            this.chbCheckSpelling.Text = "Check Spelling";
            this.chbCheckSpelling.CheckedChanged += new System.EventHandler(this.CheckSpellingCheckBox_CheckedChanged);
            this.chbCheckSpelling.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CheckSpellingCheckBox_MouseMove);
            // 
            // tpSelection
            // 
            this.tpSelection.Controls.Add(this.pnSelection);
            this.tpSelection.Location = new System.Drawing.Point(4, 25);
            this.tpSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpSelection.Name = "tpSelection";
            this.tpSelection.Size = new System.Drawing.Size(1081, 148);
            this.tpSelection.TabIndex = 11;
            this.tpSelection.Text = "Selection";
            // 
            // btHtml
            // 
            this.btHtml.BackColor = System.Drawing.SystemColors.Control;
            this.btHtml.Location = new System.Drawing.Point(139, 30);
            this.btHtml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btHtml.Name = "btHtml";
            this.btHtml.Size = new System.Drawing.Size(107, 28);
            this.btHtml.TabIndex = 7;
            this.btHtml.Text = "HTML";
            this.btHtml.UseVisualStyleBackColor = false;
            this.btHtml.Click += new System.EventHandler(this.HtmuttonLisoxTextBox_Click);
            this.btHtml.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HtmuttonLisoxTextBox_MouseMove);
            // 
            // gbPrint
            // 
            this.gbPrint.Controls.Add(this.btPrintOptions);
            this.gbPrint.Controls.Add(this.btPrintSetup);
            this.gbPrint.Controls.Add(this.btPrint);
            this.gbPrint.Controls.Add(this.btPrintPreview);
            this.gbPrint.Controls.Add(this.btHtml);
            this.gbPrint.Controls.Add(this.btRtf);
            this.gbPrint.Location = new System.Drawing.Point(11, 10);
            this.gbPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPrint.Name = "gbPrint";
            this.gbPrint.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPrint.Size = new System.Drawing.Size(661, 118);
            this.gbPrint.TabIndex = 6;
            this.gbPrint.TabStop = false;
            this.gbPrint.Text = "Printing && Exporting";
            // 
            // btRtf
            // 
            this.btRtf.BackColor = System.Drawing.SystemColors.Control;
            this.btRtf.Location = new System.Drawing.Point(11, 30);
            this.btRtf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btRtf.Name = "btRtf";
            this.btRtf.Size = new System.Drawing.Size(107, 28);
            this.btRtf.TabIndex = 6;
            this.btRtf.Text = "RTF";
            this.btRtf.UseVisualStyleBackColor = false;
            this.btRtf.Click += new System.EventHandler(this.RtfButton_Click);
            this.btRtf.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RtfButton_MouseMove);
            // 
            // pnPrinting
            // 
            this.pnPrinting.BackColor = System.Drawing.SystemColors.Control;
            this.pnPrinting.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnPrinting.Controls.Add(this.gbPrint);
            this.pnPrinting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPrinting.Location = new System.Drawing.Point(0, 0);
            this.pnPrinting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnPrinting.Name = "pnPrinting";
            this.pnPrinting.Size = new System.Drawing.Size(1081, 137);
            this.pnPrinting.TabIndex = 1;
            // 
            // tpPrinting
            // 
            this.tpPrinting.Controls.Add(this.pnPrinting);
            this.tpPrinting.Location = new System.Drawing.Point(4, 25);
            this.tpPrinting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpPrinting.Name = "tpPrinting";
            this.tpPrinting.Size = new System.Drawing.Size(1081, 148);
            this.tpPrinting.TabIndex = 3;
            this.tpPrinting.Text = "Printing";
            this.tpPrinting.Visible = false;
            // 
            // pnDialogs
            // 
            this.pnDialogs.BackColor = System.Drawing.SystemColors.Control;
            this.pnDialogs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnDialogs.Controls.Add(this.gbDialogs);
            this.pnDialogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDialogs.Location = new System.Drawing.Point(0, 0);
            this.pnDialogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnDialogs.Name = "pnDialogs";
            this.pnDialogs.Size = new System.Drawing.Size(1081, 137);
            this.pnDialogs.TabIndex = 1;
            // 
            // gbDialogs
            // 
            this.gbDialogs.Controls.Add(this.btGoto);
            this.gbDialogs.Controls.Add(this.btReplace);
            this.gbDialogs.Controls.Add(this.FindNextButton);
            this.gbDialogs.Controls.Add(this.btSave);
            this.gbDialogs.Controls.Add(this.btLoad);
            this.gbDialogs.Location = new System.Drawing.Point(11, 10);
            this.gbDialogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbDialogs.Name = "gbDialogs";
            this.gbDialogs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbDialogs.Size = new System.Drawing.Size(661, 118);
            this.gbDialogs.TabIndex = 6;
            this.gbDialogs.TabStop = false;
            this.gbDialogs.Text = "Dialogs";
            // 
            // btGoto
            // 
            this.btGoto.BackColor = System.Drawing.SystemColors.Control;
            this.btGoto.Location = new System.Drawing.Point(267, 30);
            this.btGoto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btGoto.Name = "btGoto";
            this.btGoto.Size = new System.Drawing.Size(107, 28);
            this.btGoto.TabIndex = 10;
            this.btGoto.Text = "Go to Line";
            this.btGoto.UseVisualStyleBackColor = false;
            this.btGoto.Click += new System.EventHandler(this.GotoButton_Click);
            this.btGoto.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GotoButton_MouseMove);
            // 
            // btReplace
            // 
            this.btReplace.BackColor = System.Drawing.SystemColors.Control;
            this.btReplace.Location = new System.Drawing.Point(139, 69);
            this.btReplace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(107, 28);
            this.btReplace.TabIndex = 9;
            this.btReplace.Text = "Replace";
            this.btReplace.UseVisualStyleBackColor = false;
            this.btReplace.Click += new System.EventHandler(this.ReplaceButton_Click);
            this.btReplace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ReplaceButton_MouseMove);
            // 
            // FindNextButton
            // 
            this.FindNextButton.BackColor = System.Drawing.SystemColors.Control;
            this.FindNextButton.Location = new System.Drawing.Point(139, 30);
            this.FindNextButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FindNextButton.Name = "FindNextButton";
            this.FindNextButton.Size = new System.Drawing.Size(107, 28);
            this.FindNextButton.TabIndex = 8;
            this.FindNextButton.Text = "Find Next";
            this.FindNextButton.UseVisualStyleBackColor = false;
            this.FindNextButton.Click += new System.EventHandler(this.FindButton_Click);
            this.FindNextButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FindButton_MouseMove);
            // 
            // btSave
            // 
            this.btSave.BackColor = System.Drawing.SystemColors.Control;
            this.btSave.Location = new System.Drawing.Point(11, 69);
            this.btSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(107, 28);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.SaveButton_Click);
            this.btSave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SaveButton_MouseMove);
            // 
            // btLoad
            // 
            this.btLoad.BackColor = System.Drawing.SystemColors.Control;
            this.btLoad.Location = new System.Drawing.Point(11, 30);
            this.btLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(107, 28);
            this.btLoad.TabIndex = 6;
            this.btLoad.Text = "Load";
            this.btLoad.UseVisualStyleBackColor = false;
            this.btLoad.Click += new System.EventHandler(this.LoadButton_Click);
            this.btLoad.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadButton_MouseMove);
            // 
            // tpDialogs
            // 
            this.tpDialogs.Controls.Add(this.pnDialogs);
            this.tpDialogs.Location = new System.Drawing.Point(4, 25);
            this.tpDialogs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpDialogs.Name = "tpDialogs";
            this.tpDialogs.Size = new System.Drawing.Size(1081, 148);
            this.tpDialogs.TabIndex = 2;
            this.tpDialogs.Text = "Dialogs";
            this.tpDialogs.Visible = false;
            // 
            // chbQuickInfoTips
            // 
            this.chbQuickInfoTips.Location = new System.Drawing.Point(160, 20);
            this.chbQuickInfoTips.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbQuickInfoTips.Name = "chbQuickInfoTips";
            this.chbQuickInfoTips.Size = new System.Drawing.Size(128, 26);
            this.chbQuickInfoTips.TabIndex = 4;
            this.chbQuickInfoTips.Text = "Quick Info Tips";
            this.chbQuickInfoTips.CheckedChanged += new System.EventHandler(this.QuickInfoTipsCheckBox_CheckedChanged);
            this.chbQuickInfoTips.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QuickInfoTipsCheckBox_MouseMove);
            // 
            // chbTransparent
            // 
            this.chbTransparent.Location = new System.Drawing.Point(11, 79);
            this.chbTransparent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbTransparent.Name = "chbTransparent";
            this.chbTransparent.Size = new System.Drawing.Size(149, 26);
            this.chbTransparent.TabIndex = 3;
            this.chbTransparent.Text = "Use Background";
            this.chbTransparent.CheckedChanged += new System.EventHandler(this.TransparentCheckBox_CheckedChanged);
            this.chbTransparent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransparentCheckBox_MouseMove);
            // 
            // chbHighlightReferences
            // 
            this.chbHighlightReferences.Location = new System.Drawing.Point(160, 79);
            this.chbHighlightReferences.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHighlightReferences.Name = "chbHighlightReferences";
            this.chbHighlightReferences.Size = new System.Drawing.Size(172, 26);
            this.chbHighlightReferences.TabIndex = 6;
            this.chbHighlightReferences.Text = "Highlight References";
            this.chbHighlightReferences.CheckedChanged += new System.EventHandler(this.HighlightReferencesCheckBox_CheckedChanged);
            this.chbHighlightReferences.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightReferencesCheckBox_MouseMove);
            // 
            // cbTempHighlightBraces
            // 
            this.cbTempHighlightBraces.Location = new System.Drawing.Point(11, 79);
            this.cbTempHighlightBraces.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTempHighlightBraces.Name = "cbTempHighlightBraces";
            this.cbTempHighlightBraces.Size = new System.Drawing.Size(149, 26);
            this.cbTempHighlightBraces.TabIndex = 3;
            this.cbTempHighlightBraces.Text = "Temp Highlight";
            this.cbTempHighlightBraces.CheckedChanged += new System.EventHandler(this.TempHighlighracesComboBoxTextBox_CheckedChanged);
            this.cbTempHighlightBraces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TempHighlighracesCheckBoxTextBox_MouseMove);
            // 
            // chbSeparateLines
            // 
            this.chbSeparateLines.Location = new System.Drawing.Point(11, 20);
            this.chbSeparateLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbSeparateLines.Name = "chbSeparateLines";
            this.chbSeparateLines.Size = new System.Drawing.Size(139, 26);
            this.chbSeparateLines.TabIndex = 1;
            this.chbSeparateLines.Text = "Separate Lines";
            this.chbSeparateLines.CheckedChanged += new System.EventHandler(this.SeparateLinesCheckBox_CheckedChanged);
            this.chbSeparateLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SeparateLinesCheckBox_MouseMove);
            // 
            // chbWhiteSpaceVisible
            // 
            this.chbWhiteSpaceVisible.Location = new System.Drawing.Point(11, 49);
            this.chbWhiteSpaceVisible.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWhiteSpaceVisible.Name = "chbWhiteSpaceVisible";
            this.chbWhiteSpaceVisible.Size = new System.Drawing.Size(160, 26);
            this.chbWhiteSpaceVisible.TabIndex = 2;
            this.chbWhiteSpaceVisible.Text = "Whitespace Visible";
            this.chbWhiteSpaceVisible.CheckedChanged += new System.EventHandler(this.WhiteSpaceVisibleCheckBox_CheckedChanged);
            this.chbWhiteSpaceVisible.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WhiteSpaceVisibleCheckBox_MouseMove);
            // 
            // chbUseRoundRect
            // 
            this.chbUseRoundRect.Location = new System.Drawing.Point(11, 49);
            this.chbUseRoundRect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbUseRoundRect.Name = "chbUseRoundRect";
            this.chbUseRoundRect.Size = new System.Drawing.Size(216, 26);
            this.chbUseRoundRect.TabIndex = 2;
            this.chbUseRoundRect.Text = "Draw Frame around Braces";
            this.chbUseRoundRect.CheckedChanged += new System.EventHandler(this.UseRoundRectCheckBox_CheckedChanged);
            this.chbUseRoundRect.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UseRoundRectCheckBox_MouseMove);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView1.Name = "treeView1";
            treeNode23.Name = "";
            treeNode23.Text = "Gutter";
            treeNode24.Name = "";
            treeNode24.Text = "Margin";
            treeNode25.Name = "";
            treeNode25.Text = "Line Numbers";
            treeNode26.Name = "";
            treeNode26.Text = "Line Styles";
            treeNode27.Name = "";
            treeNode27.Text = "Visual Themes";
            treeNode28.Name = "";
            treeNode28.Text = "Miscellaneous";
            treeNode29.Name = "";
            treeNode29.Text = "Pages & Rulers";
            treeNode30.Name = "";
            treeNode30.Text = "Appearance";
            treeNode31.Name = "";
            treeNode31.Text = "Outlining";
            treeNode32.Name = "";
            treeNode32.Text = "Text Source";
            treeNode33.Name = "";
            treeNode33.Text = "Navigation";
            treeNode34.Name = "";
            treeNode34.Text = "Selection";
            treeNode35.Name = "";
            treeNode35.Text = "WordWrap & Scrolling";
            treeNode36.Name = "";
            treeNode36.Text = "Spelling & HyperText";
            treeNode37.Name = "";
            treeNode37.Text = "Scroll Bar Annotations";
            treeNode38.Name = "";
            treeNode38.Text = "Behavior";
            treeNode39.Name = "";
            treeNode39.Text = "Common Dialogs";
            treeNode40.Name = "";
            treeNode40.Text = "Printing & Exporting";
            treeNode41.Name = "";
            treeNode41.Text = "Dialogs";
            treeNode42.Name = "";
            treeNode42.Text = "Properties";
            treeNode43.Name = "";
            treeNode43.Text = "About";
            treeNode44.Name = "";
            treeNode44.Text = "Company Info";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode38,
            treeNode41,
            treeNode42,
            treeNode44});
            this.treeView1.Size = new System.Drawing.Size(212, 788);
            this.treeView1.TabIndex = 11;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|C # files (*.cs)|*.cs|All files (*.*)|*.*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text files (*.txt)|*.txt|C # files (*.cs)|*.cs|All files (*.*)|*.*";
            // 
            // chbDrawColumnsIndent
            // 
            this.chbDrawColumnsIndent.Location = new System.Drawing.Point(160, 49);
            this.chbDrawColumnsIndent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDrawColumnsIndent.Name = "chbDrawColumnsIndent";
            this.chbDrawColumnsIndent.Size = new System.Drawing.Size(173, 26);
            this.chbDrawColumnsIndent.TabIndex = 5;
            this.chbDrawColumnsIndent.Text = "Draw Columns Indent";
            this.chbDrawColumnsIndent.CheckedChanged += new System.EventHandler(this.DrawColumnsIndentCheckBox_CheckedChanged);
            this.chbDrawColumnsIndent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawColumnsIndentCheckBox_MouseMove);
            // 
            // chbLineModificator
            // 
            this.chbLineModificator.Checked = true;
            this.chbLineModificator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbLineModificator.Location = new System.Drawing.Point(192, 20);
            this.chbLineModificator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbLineModificator.Name = "chbLineModificator";
            this.chbLineModificator.Size = new System.Drawing.Size(149, 26);
            this.chbLineModificator.TabIndex = 3;
            this.chbLineModificator.Text = "Line Modificators";
            this.chbLineModificator.CheckedChanged += new System.EventHandler(this.LineModificatorCheckBox_CheckedChanged);
            this.chbLineModificator.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineModificatorCheckBox_MouseMove);
            // 
            // laLineNumbersStart
            // 
            this.laLineNumbersStart.AutoSize = true;
            this.laLineNumbersStart.Location = new System.Drawing.Point(395, 23);
            this.laLineNumbersStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laLineNumbersStart.Name = "laLineNumbersStart";
            this.laLineNumbersStart.Size = new System.Drawing.Size(115, 16);
            this.laLineNumbersStart.TabIndex = 5;
            this.laLineNumbersStart.Text = "Line numbers start";
            // 
            // gbOther
            // 
            this.gbOther.Controls.Add(this.chbDrawColumnsIndent);
            this.gbOther.Controls.Add(this.chbQuickInfoTips);
            this.gbOther.Controls.Add(this.chbTransparent);
            this.gbOther.Controls.Add(this.chbHighlightReferences);
            this.gbOther.Controls.Add(this.chbSeparateLines);
            this.gbOther.Controls.Add(this.chbWhiteSpaceVisible);
            this.gbOther.Location = new System.Drawing.Point(11, 10);
            this.gbOther.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbOther.Name = "gbOther";
            this.gbOther.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbOther.Size = new System.Drawing.Size(341, 118);
            this.gbOther.TabIndex = 4;
            this.gbOther.TabStop = false;
            this.gbOther.Text = "Miscellaneous";
            // 
            // nudLineNumbersStart
            // 
            this.nudLineNumbersStart.Location = new System.Drawing.Point(533, 20);
            this.nudLineNumbersStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudLineNumbersStart.Name = "nudLineNumbersStart";
            this.nudLineNumbersStart.Size = new System.Drawing.Size(85, 22);
            this.nudLineNumbersStart.TabIndex = 7;
            this.nudLineNumbersStart.ValueChanged += new System.EventHandler(this.LineNumbersStartNumeric_ValueChanged);
            // 
            // cbLineNumbersAlign
            // 
            this.cbLineNumbersAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineNumbersAlign.ItemHeight = 16;
            this.cbLineNumbersAlign.Location = new System.Drawing.Point(533, 49);
            this.cbLineNumbersAlign.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLineNumbersAlign.Name = "cbLineNumbersAlign";
            this.cbLineNumbersAlign.Size = new System.Drawing.Size(84, 24);
            this.cbLineNumbersAlign.TabIndex = 8;
            this.cbLineNumbersAlign.SelectedIndexChanged += new System.EventHandler(this.LineNumbersAlignComboBox_SelectedIndexChanged);
            this.cbLineNumbersAlign.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersAlignCheckBox_MouseMove);
            // 
            // pnLineNumbers
            // 
            this.pnLineNumbers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnLineNumbers.Controls.Add(this.gbLineNumbers);
            this.pnLineNumbers.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnLineNumbers.Location = new System.Drawing.Point(0, 0);
            this.pnLineNumbers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnLineNumbers.Name = "pnLineNumbers";
            this.pnLineNumbers.Size = new System.Drawing.Size(1081, 137);
            this.pnLineNumbers.TabIndex = 0;
            // 
            // gbLineNumbers
            // 
            this.gbLineNumbers.Controls.Add(this.chbLineModificator);
            this.gbLineNumbers.Controls.Add(this.laLineNumbersStart);
            this.gbLineNumbers.Controls.Add(this.nudLineNumbersStart);
            this.gbLineNumbers.Controls.Add(this.cbLineNumbersAlign);
            this.gbLineNumbers.Controls.Add(this.laLineNumbersAlign);
            this.gbLineNumbers.Controls.Add(this.chbLinesOnGutter);
            this.gbLineNumbers.Controls.Add(this.chbLineNumbers);
            this.gbLineNumbers.Controls.Add(this.chbLinesBeyondEof);
            this.gbLineNumbers.Controls.Add(this.chbHighlightCurrentLine);
            this.gbLineNumbers.Location = new System.Drawing.Point(11, 10);
            this.gbLineNumbers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbLineNumbers.Name = "gbLineNumbers";
            this.gbLineNumbers.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbLineNumbers.Size = new System.Drawing.Size(661, 118);
            this.gbLineNumbers.TabIndex = 3;
            this.gbLineNumbers.TabStop = false;
            this.gbLineNumbers.Text = "Line Numbers";
            // 
            // laLineNumbersAlign
            // 
            this.laLineNumbersAlign.AutoSize = true;
            this.laLineNumbersAlign.Location = new System.Drawing.Point(395, 53);
            this.laLineNumbersAlign.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laLineNumbersAlign.Name = "laLineNumbersAlign";
            this.laLineNumbersAlign.Size = new System.Drawing.Size(119, 16);
            this.laLineNumbersAlign.TabIndex = 6;
            this.laLineNumbersAlign.Text = "Line numbers align";
            // 
            // chbLinesOnGutter
            // 
            this.chbLinesOnGutter.Location = new System.Drawing.Point(11, 49);
            this.chbLinesOnGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbLinesOnGutter.Name = "chbLinesOnGutter";
            this.chbLinesOnGutter.Size = new System.Drawing.Size(139, 26);
            this.chbLinesOnGutter.TabIndex = 1;
            this.chbLinesOnGutter.Text = "Lines on Gutter";
            this.chbLinesOnGutter.CheckedChanged += new System.EventHandler(this.LinesOnGutterCheckBox_CheckedChanged);
            this.chbLinesOnGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LinesOnGutterCheckBox_MouseMove);
            // 
            // chbLineNumbers
            // 
            this.chbLineNumbers.Location = new System.Drawing.Point(11, 20);
            this.chbLineNumbers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new System.Drawing.Size(139, 26);
            this.chbLineNumbers.TabIndex = 0;
            this.chbLineNumbers.Text = "Line Numbers";
            this.chbLineNumbers.CheckedChanged += new System.EventHandler(this.LineNumbersCheckBox_CheckedChanged);
            this.chbLineNumbers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineNumbersCheckBox_MouseMove);
            // 
            // chbLinesBeyondEof
            // 
            this.chbLinesBeyondEof.Location = new System.Drawing.Point(11, 79);
            this.chbLinesBeyondEof.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbLinesBeyondEof.Name = "chbLinesBeyondEof";
            this.chbLinesBeyondEof.Size = new System.Drawing.Size(149, 26);
            this.chbLinesBeyondEof.TabIndex = 2;
            this.chbLinesBeyondEof.Text = "Lines Beyond Eof";
            this.chbLinesBeyondEof.CheckedChanged += new System.EventHandler(this.LinesBeyondEofCheckBox_CheckedChanged);
            this.chbLinesBeyondEof.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LinesBeyondEofCheckBox_MouseMove);
            // 
            // chbHighlightCurrentLine
            // 
            this.chbHighlightCurrentLine.Location = new System.Drawing.Point(192, 49);
            this.chbHighlightCurrentLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHighlightCurrentLine.Name = "chbHighlightCurrentLine";
            this.chbHighlightCurrentLine.Size = new System.Drawing.Size(181, 26);
            this.chbHighlightCurrentLine.TabIndex = 4;
            this.chbHighlightCurrentLine.Text = "Highlight Current Line";
            this.chbHighlightCurrentLine.CheckedChanged += new System.EventHandler(this.HighlightCurrentLineCheckBox_CheckedChanged);
            this.chbHighlightCurrentLine.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightCurrentLineCheckBox_MouseMove);
            // 
            // pnLineStyles
            // 
            this.pnLineStyles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnLineStyles.Controls.Add(this.btStepOver);
            this.pnLineStyles.Controls.Add(this.btSetBreakpoint);
            this.pnLineStyles.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnLineStyles.Location = new System.Drawing.Point(0, 0);
            this.pnLineStyles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnLineStyles.Name = "pnLineStyles";
            this.pnLineStyles.Size = new System.Drawing.Size(1081, 137);
            this.pnLineStyles.TabIndex = 0;
            // 
            // btStepOver
            // 
            this.btStepOver.Location = new System.Drawing.Point(11, 30);
            this.btStepOver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btStepOver.Name = "btStepOver";
            this.btStepOver.Size = new System.Drawing.Size(100, 28);
            this.btStepOver.TabIndex = 1;
            this.btStepOver.Text = "Step Over";
            this.btStepOver.Click += new System.EventHandler(this.StepOver_ButtonClick);
            this.btStepOver.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StepOverButton_MouseMove);
            // 
            // btSetBreakpoint
            // 
            this.btSetBreakpoint.Location = new System.Drawing.Point(123, 30);
            this.btSetBreakpoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSetBreakpoint.Name = "btSetBreakpoint";
            this.btSetBreakpoint.Size = new System.Drawing.Size(141, 28);
            this.btSetBreakpoint.TabIndex = 2;
            this.btSetBreakpoint.Text = "Toggle Breakpoint";
            this.btSetBreakpoint.Click += new System.EventHandler(this.SereakpointTextBox_ButtonClick);
            this.btSetBreakpoint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SereakpointButtonTextBox_MouseMove);
            // 
            // tpLineNumbers
            // 
            this.tpLineNumbers.Controls.Add(this.pnLineNumbers);
            this.tpLineNumbers.Location = new System.Drawing.Point(4, 25);
            this.tpLineNumbers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpLineNumbers.Name = "tpLineNumbers";
            this.tpLineNumbers.Size = new System.Drawing.Size(1081, 148);
            this.tpLineNumbers.TabIndex = 10;
            this.tpLineNumbers.Text = "Line Numbers";
            this.tpLineNumbers.Visible = false;
            // 
            // tpLineStyles
            // 
            this.tpLineStyles.Controls.Add(this.pnLineStyles);
            this.tpLineStyles.Location = new System.Drawing.Point(4, 25);
            this.tpLineStyles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpLineStyles.Name = "tpLineStyles";
            this.tpLineStyles.Size = new System.Drawing.Size(1081, 148);
            this.tpLineStyles.TabIndex = 11;
            this.tpLineStyles.Text = "Line Styles";
            this.tpLineStyles.Visible = false;
            // 
            // gbBraces
            // 
            this.gbBraces.Controls.Add(this.cbTempHighlightBraces);
            this.gbBraces.Controls.Add(this.chbUseRoundRect);
            this.gbBraces.Controls.Add(this.chbHighlightBraces);
            this.gbBraces.Location = new System.Drawing.Point(363, 10);
            this.gbBraces.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbBraces.Name = "gbBraces";
            this.gbBraces.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbBraces.Size = new System.Drawing.Size(309, 118);
            this.gbBraces.TabIndex = 5;
            this.gbBraces.TabStop = false;
            this.gbBraces.Text = "Braces";
            // 
            // chbHighlightBraces
            // 
            this.chbHighlightBraces.Location = new System.Drawing.Point(11, 20);
            this.chbHighlightBraces.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHighlightBraces.Name = "chbHighlightBraces";
            this.chbHighlightBraces.Size = new System.Drawing.Size(149, 26);
            this.chbHighlightBraces.TabIndex = 1;
            this.chbHighlightBraces.Text = "Highlight Braces";
            this.chbHighlightBraces.CheckedChanged += new System.EventHandler(this.HighlighracesCheckBoxTextBox_CheckedChanged);
            this.chbHighlightBraces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlighracesCheckBoxTextBox_MouseMove);
            // 
            // pnOther
            // 
            this.pnOther.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnOther.Controls.Add(this.gbBraces);
            this.pnOther.Controls.Add(this.gbOther);
            this.pnOther.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnOther.Location = new System.Drawing.Point(0, 0);
            this.pnOther.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnOther.Name = "pnOther";
            this.pnOther.Size = new System.Drawing.Size(1081, 137);
            this.pnOther.TabIndex = 0;
            // 
            // tpOther
            // 
            this.tpOther.Controls.Add(this.pnOther);
            this.tpOther.Location = new System.Drawing.Point(4, 25);
            this.tpOther.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpOther.Name = "tpOther";
            this.tpOther.Size = new System.Drawing.Size(1081, 148);
            this.tpOther.TabIndex = 12;
            this.tpOther.Text = "Miscellaneous";
            // 
            // pnProperties
            // 
            this.pnProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnProperties.Location = new System.Drawing.Point(0, 0);
            this.pnProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnProperties.Name = "pnProperties";
            this.pnProperties.Size = new System.Drawing.Size(1081, 0);
            this.pnProperties.TabIndex = 0;
            // 
            // tpProperties
            // 
            this.tpProperties.Controls.Add(this.pnProperties);
            this.tpProperties.Location = new System.Drawing.Point(4, 25);
            this.tpProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Size = new System.Drawing.Size(1081, 148);
            this.tpProperties.TabIndex = 14;
            this.tpProperties.Text = "Properties";
            // 
            // tbCompanyInfo
            // 
            this.tbCompanyInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tbCompanyInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCompanyInfo.Location = new System.Drawing.Point(33, 127);
            this.tbCompanyInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbCompanyInfo.Multiline = true;
            this.tbCompanyInfo.Name = "tbCompanyInfo";
            this.tbCompanyInfo.Size = new System.Drawing.Size(533, 101);
            this.tbCompanyInfo.TabIndex = 9;
            this.tbCompanyInfo.Text = resources.GetString("tbCompanyInfo.Text");
            // 
            // chbHighlightUrls
            // 
            this.chbHighlightUrls.Location = new System.Drawing.Point(11, 49);
            this.chbHighlightUrls.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHighlightUrls.Name = "chbHighlightUrls";
            this.chbHighlightUrls.Size = new System.Drawing.Size(139, 26);
            this.chbHighlightUrls.TabIndex = 2;
            this.chbHighlightUrls.Text = "Highlight Urls";
            this.chbHighlightUrls.CheckedChanged += new System.EventHandler(this.HighlightUrlsCheckBox_CheckedChanged);
            this.chbHighlightUrls.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightUrlsCheckBox_MouseMove);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 272);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "AlterNET Code Editor Library";
            this.label6.Visible = false;
            // 
            // pnCompanyInfo
            // 
            this.pnCompanyInfo.BackColor = System.Drawing.SystemColors.Control;
            this.pnCompanyInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnCompanyInfo.Controls.Add(this.tbCompanyInfo);
            this.pnCompanyInfo.Controls.Add(this.label6);
            this.pnCompanyInfo.Controls.Add(this.laMailTo);
            this.pnCompanyInfo.Controls.Add(this.label4);
            this.pnCompanyInfo.Controls.Add(this.laAdress);
            this.pnCompanyInfo.Controls.Add(this.label2);
            this.pnCompanyInfo.Controls.Add(this.pictureBox1);
            this.pnCompanyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnCompanyInfo.Location = new System.Drawing.Point(0, 0);
            this.pnCompanyInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnCompanyInfo.Name = "pnCompanyInfo";
            this.pnCompanyInfo.Size = new System.Drawing.Size(1081, 148);
            this.pnCompanyInfo.TabIndex = 1;
            // 
            // laMailTo
            // 
            this.laMailTo.AutoSize = true;
            this.laMailTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.laMailTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMailTo.ForeColor = System.Drawing.Color.Blue;
            this.laMailTo.Location = new System.Drawing.Point(324, 78);
            this.laMailTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laMailTo.Name = "laMailTo";
            this.laMailTo.Size = new System.Drawing.Size(210, 17);
            this.laMailTo.TabIndex = 5;
            this.laMailTo.Text = "mailto:contact@alternetsoft.com";
            this.laMailTo.Click += new System.EventHandler(this.MailToLabel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 78);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "e-mail:";
            // 
            // laAdress
            // 
            this.laAdress.AutoSize = true;
            this.laAdress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.laAdress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laAdress.ForeColor = System.Drawing.Color.Blue;
            this.laAdress.Location = new System.Drawing.Point(324, 44);
            this.laAdress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laAdress.Name = "laAdress";
            this.laAdress.Size = new System.Drawing.Size(176, 17);
            this.laAdress.TabIndex = 3;
            this.laAdress.Text = "http://www.alternetsoft.com";
            this.laAdress.Click += new System.EventHandler(this.AdressLebel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "WWW:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(33, 42);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 45);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btShowBookmarks
            // 
            this.btShowBookmarks.BackColor = System.Drawing.SystemColors.Control;
            this.btShowBookmarks.Location = new System.Drawing.Point(245, 59);
            this.btShowBookmarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btShowBookmarks.Name = "btShowBookmarks";
            this.btShowBookmarks.Size = new System.Drawing.Size(139, 28);
            this.btShowBookmarks.TabIndex = 10;
            this.btShowBookmarks.Text = "Set Bookmarks";
            this.btShowBookmarks.UseVisualStyleBackColor = false;
            this.btShowBookmarks.Click += new System.EventHandler(this.ShowBookmarksButton_Click);
            this.btShowBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowBookmarksButton_MouseMove);
            // 
            // tpCompanyInfo
            // 
            this.tpCompanyInfo.Controls.Add(this.pnCompanyInfo);
            this.tpCompanyInfo.Location = new System.Drawing.Point(4, 25);
            this.tpCompanyInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpCompanyInfo.Name = "tpCompanyInfo";
            this.tpCompanyInfo.Size = new System.Drawing.Size(1081, 148);
            this.tpCompanyInfo.TabIndex = 8;
            this.tpCompanyInfo.Text = "Company Info";
            this.tpCompanyInfo.Visible = false;
            // 
            // chbDrawLineBookmarks
            // 
            this.chbDrawLineBookmarks.Location = new System.Drawing.Point(11, 79);
            this.chbDrawLineBookmarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbDrawLineBookmarks.Name = "chbDrawLineBookmarks";
            this.chbDrawLineBookmarks.Size = new System.Drawing.Size(181, 26);
            this.chbDrawLineBookmarks.TabIndex = 2;
            this.chbDrawLineBookmarks.Text = "Draw Line Bookmarks";
            this.chbDrawLineBookmarks.CheckedChanged += new System.EventHandler(this.DrawLineBookmarksCheckBox_CheckedChanged);
            this.chbDrawLineBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawLineBookmarksCheckBox_MouseMove);
            // 
            // tpMargin
            // 
            this.tpMargin.Controls.Add(this.pnMargin);
            this.tpMargin.Location = new System.Drawing.Point(4, 25);
            this.tpMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpMargin.Name = "tpMargin";
            this.tpMargin.Size = new System.Drawing.Size(1081, 148);
            this.tpMargin.TabIndex = 9;
            this.tpMargin.Text = "Margin";
            this.tpMargin.Visible = false;
            // 
            // pnMargin
            // 
            this.pnMargin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnMargin.Controls.Add(this.gbMargin);
            this.pnMargin.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnMargin.Location = new System.Drawing.Point(0, 0);
            this.pnMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnMargin.Name = "pnMargin";
            this.pnMargin.Size = new System.Drawing.Size(1081, 137);
            this.pnMargin.TabIndex = 0;
            // 
            // gbMargin
            // 
            this.gbMargin.Controls.Add(this.chbColumnsVisible);
            this.gbMargin.Controls.Add(this.chbShowMarginHints);
            this.gbMargin.Controls.Add(this.chbAllowDragMargin);
            this.gbMargin.Controls.Add(this.nudMarginPos);
            this.gbMargin.Controls.Add(this.laMarginPos);
            this.gbMargin.Controls.Add(this.chbShowMargin);
            this.gbMargin.Location = new System.Drawing.Point(11, 10);
            this.gbMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbMargin.Name = "gbMargin";
            this.gbMargin.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbMargin.Size = new System.Drawing.Size(661, 118);
            this.gbMargin.TabIndex = 0;
            this.gbMargin.TabStop = false;
            this.gbMargin.Text = "Margin";
            // 
            // chbColumnsVisible
            // 
            this.chbColumnsVisible.Location = new System.Drawing.Point(224, 79);
            this.chbColumnsVisible.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbColumnsVisible.Name = "chbColumnsVisible";
            this.chbColumnsVisible.Size = new System.Drawing.Size(160, 26);
            this.chbColumnsVisible.TabIndex = 7;
            this.chbColumnsVisible.Text = "Columns Visible";
            this.chbColumnsVisible.CheckedChanged += new System.EventHandler(this.ColumnsVisibleCheckBox_CheckedChanged);
            this.chbColumnsVisible.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColumnsVisibleCheckBox_MouseMove);
            // 
            // chbShowMarginHints
            // 
            this.chbShowMarginHints.Location = new System.Drawing.Point(224, 49);
            this.chbShowMarginHints.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowMarginHints.Name = "chbShowMarginHints";
            this.chbShowMarginHints.Size = new System.Drawing.Size(160, 26);
            this.chbShowMarginHints.TabIndex = 6;
            this.chbShowMarginHints.Text = "Display Drag Hints";
            this.chbShowMarginHints.CheckedChanged += new System.EventHandler(this.ShowMarginHintsCheckBox_CheckedChanged);
            this.chbShowMarginHints.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowMarginHintsCheckBox_MouseMove);
            // 
            // chbAllowDragMargin
            // 
            this.chbAllowDragMargin.Location = new System.Drawing.Point(224, 20);
            this.chbAllowDragMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbAllowDragMargin.Name = "chbAllowDragMargin";
            this.chbAllowDragMargin.Size = new System.Drawing.Size(160, 26);
            this.chbAllowDragMargin.TabIndex = 5;
            this.chbAllowDragMargin.Text = "Allow Drag Margin";
            this.chbAllowDragMargin.CheckedChanged += new System.EventHandler(this.AllowDragMarginCheckBox_CheckedChanged);
            this.chbAllowDragMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowDragMarginCheckBox_MouseMove);
            // 
            // nudMarginPos
            // 
            this.nudMarginPos.Location = new System.Drawing.Point(117, 49);
            this.nudMarginPos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudMarginPos.Name = "nudMarginPos";
            this.nudMarginPos.Size = new System.Drawing.Size(85, 22);
            this.nudMarginPos.TabIndex = 4;
            this.nudMarginPos.ValueChanged += new System.EventHandler(this.MarginPosNumeric_ValueChanged);
            // 
            // laMarginPos
            // 
            this.laMarginPos.AutoSize = true;
            this.laMarginPos.Location = new System.Drawing.Point(11, 53);
            this.laMarginPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laMarginPos.Name = "laMarginPos";
            this.laMarginPos.Size = new System.Drawing.Size(98, 16);
            this.laMarginPos.TabIndex = 3;
            this.laMarginPos.Text = "Margin position";
            // 
            // chbShowMargin
            // 
            this.chbShowMargin.Location = new System.Drawing.Point(11, 20);
            this.chbShowMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowMargin.Name = "chbShowMargin";
            this.chbShowMargin.Size = new System.Drawing.Size(128, 26);
            this.chbShowMargin.TabIndex = 0;
            this.chbShowMargin.Text = "Display Margin";
            this.chbShowMargin.CheckedChanged += new System.EventHandler(this.ShowMarginCheckBox_CheckedChanged);
            this.chbShowMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowMarginCheckBox_MouseMove);
            // 
            // chbPaintBookMarks
            // 
            this.chbPaintBookMarks.Location = new System.Drawing.Point(11, 49);
            this.chbPaintBookMarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbPaintBookMarks.Name = "chbPaintBookMarks";
            this.chbPaintBookMarks.Size = new System.Drawing.Size(149, 26);
            this.chbPaintBookMarks.TabIndex = 1;
            this.chbPaintBookMarks.Text = "Paint Bookmarks";
            this.chbPaintBookMarks.CheckedChanged += new System.EventHandler(this.PainookMarksCheckBoxTextBox_CheckedChanged);
            this.chbPaintBookMarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PainookMarksCheckBoxTextBox_MouseMove);
            // 
            // syntaxEdit
            // 
            this.syntaxEdit.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("syntaxEdit.BackgroundImage")));
            this.syntaxEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.syntaxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit.EditMargin.ColumnPositions = new int[] {
        16,
        48};
            this.syntaxEdit.Location = new System.Drawing.Point(0, 0);
            this.syntaxEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.syntaxEdit.Name = "syntaxEdit";
            this.syntaxEdit.Outlining.AllowOutlining = true;
            this.syntaxEdit.Scrolling.Options = ((Alternet.Editor.ScrollingOptions)(((((((Alternet.Editor.ScrollingOptions.SmoothScroll | Alternet.Editor.ScrollingOptions.UseScrollDelta) 
            | Alternet.Editor.ScrollingOptions.AllowSplitHorz) 
            | Alternet.Editor.ScrollingOptions.AllowSplitVert) 
            | Alternet.Editor.ScrollingOptions.HorzButtons) 
            | Alternet.Editor.ScrollingOptions.VertButtons) 
            | Alternet.Editor.ScrollingOptions.VerticalScrollBarAnnotations)));
            this.syntaxEdit.Scrolling.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.syntaxEdit.Size = new System.Drawing.Size(788, 488);
            this.syntaxEdit.Source = this.textSource1;
            this.syntaxEdit.TabIndex = 38;
            this.syntaxEdit.WordSpell += new Alternet.Editor.TextSource.WordSpellEvent(this.SyntaxEdit_WordSpell);
            this.syntaxEdit.ScrollButtonClick += new System.EventHandler(this.SyntaxEdit_ScrollButtonClick);
            // 
            // gbGutter
            // 
            this.gbGutter.Controls.Add(this.btShowBookmarks);
            this.gbGutter.Controls.Add(this.chbPaintBookMarks);
            this.gbGutter.Controls.Add(this.chbDrawLineBookmarks);
            this.gbGutter.Controls.Add(this.laGutterWidth);
            this.gbGutter.Controls.Add(this.nudGutterWidth);
            this.gbGutter.Controls.Add(this.chbShowGutter);
            this.gbGutter.Location = new System.Drawing.Point(11, 10);
            this.gbGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbGutter.Name = "gbGutter";
            this.gbGutter.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbGutter.Size = new System.Drawing.Size(661, 118);
            this.gbGutter.TabIndex = 0;
            this.gbGutter.TabStop = false;
            this.gbGutter.Text = "Gutter";
            // 
            // laGutterWidth
            // 
            this.laGutterWidth.AutoSize = true;
            this.laGutterWidth.Location = new System.Drawing.Point(192, 23);
            this.laGutterWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laGutterWidth.Name = "laGutterWidth";
            this.laGutterWidth.Size = new System.Drawing.Size(79, 16);
            this.laGutterWidth.TabIndex = 6;
            this.laGutterWidth.Text = "Gutter Width";
            // 
            // nudGutterWidth
            // 
            this.nudGutterWidth.Location = new System.Drawing.Point(299, 20);
            this.nudGutterWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudGutterWidth.Name = "nudGutterWidth";
            this.nudGutterWidth.Size = new System.Drawing.Size(85, 22);
            this.nudGutterWidth.TabIndex = 8;
            this.nudGutterWidth.ValueChanged += new System.EventHandler(this.GutterWidthNumeric_ValueChanged);
            // 
            // chbShowGutter
            // 
            this.chbShowGutter.Location = new System.Drawing.Point(11, 20);
            this.chbShowGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowGutter.Name = "chbShowGutter";
            this.chbShowGutter.Size = new System.Drawing.Size(139, 26);
            this.chbShowGutter.TabIndex = 0;
            this.chbShowGutter.Text = "Display Gutter";
            this.chbShowGutter.CheckedChanged += new System.EventHandler(this.ShowGutterCheckBox_CheckedChanged);
            this.chbShowGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowGutterCheckBox_MouseMove);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 488);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(788, 6);
            this.splitter1.TabIndex = 37;
            this.splitter1.TabStop = false;
            // 
            // pnMain
            // 
            this.pnMain.Controls.Add(this.pnEditContainer);
            this.pnMain.Controls.Add(this.splitter2);
            this.pnMain.Controls.Add(this.pnPropertyGrid);
            this.pnMain.Controls.Add(this.pnManage);
            this.pnMain.Controls.Add(this.treeView1);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(1303, 788);
            this.pnMain.TabIndex = 2;
            // 
            // pnEditContainer
            // 
            this.pnEditContainer.Controls.Add(this.syntaxEdit);
            this.pnEditContainer.Controls.Add(this.splitter1);
            this.pnEditContainer.Controls.Add(this.syntaxSplitEdit);
            this.pnEditContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEditContainer.Location = new System.Drawing.Point(212, 177);
            this.pnEditContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnEditContainer.Name = "pnEditContainer";
            this.pnEditContainer.Size = new System.Drawing.Size(788, 611);
            this.pnEditContainer.TabIndex = 39;
            // 
            // syntaxSplitEdit
            // 
            this.syntaxSplitEdit.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxSplitEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("syntaxSplitEdit.BackgroundImage")));
            this.syntaxSplitEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.syntaxSplitEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.syntaxSplitEdit.EditMargin.ColumnPositions = new int[] {
        16,
        48};
            this.syntaxSplitEdit.Location = new System.Drawing.Point(0, 494);
            this.syntaxSplitEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.syntaxSplitEdit.Name = "syntaxSplitEdit";
            this.syntaxSplitEdit.Outlining.AllowOutlining = true;
            this.syntaxSplitEdit.Scrolling.Options = ((Alternet.Editor.ScrollingOptions)((((((Alternet.Editor.ScrollingOptions.SmoothScroll | Alternet.Editor.ScrollingOptions.UseScrollDelta) 
            | Alternet.Editor.ScrollingOptions.AllowSplitHorz) 
            | Alternet.Editor.ScrollingOptions.AllowSplitVert) 
            | Alternet.Editor.ScrollingOptions.HorzButtons) 
            | Alternet.Editor.ScrollingOptions.VertButtons)));
            this.syntaxSplitEdit.Size = new System.Drawing.Size(788, 117);
            this.syntaxSplitEdit.Source = this.textSource1;
            this.syntaxSplitEdit.TabIndex = 36;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(1000, 177);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(4, 611);
            this.splitter2.TabIndex = 38;
            this.splitter2.TabStop = false;
            // 
            // pnPropertyGrid
            // 
            this.pnPropertyGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnPropertyGrid.Location = new System.Drawing.Point(1004, 177);
            this.pnPropertyGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnPropertyGrid.Name = "pnPropertyGrid";
            this.pnPropertyGrid.Size = new System.Drawing.Size(299, 611);
            this.pnPropertyGrid.TabIndex = 36;
            this.pnPropertyGrid.Visible = false;
            // 
            // pnManage
            // 
            this.pnManage.Controls.Add(this.tcContainer);
            this.pnManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnManage.Location = new System.Drawing.Point(212, 0);
            this.pnManage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnManage.Name = "pnManage";
            this.pnManage.Size = new System.Drawing.Size(1091, 177);
            this.pnManage.TabIndex = 0;
            // 
            // tcContainer
            // 
            this.tcContainer.Controls.Add(this.tpGutter);
            this.tcContainer.Controls.Add(this.tpCompanyInfo);
            this.tcContainer.Controls.Add(this.tpMargin);
            this.tcContainer.Controls.Add(this.tpWordWrap);
            this.tcContainer.Controls.Add(this.tpTextSource);
            this.tcContainer.Controls.Add(this.tpPageLayout);
            this.tcContainer.Controls.Add(this.tpTextMate);
            this.tcContainer.Controls.Add(this.tpSpellAndUrl);
            this.tcContainer.Controls.Add(this.tpProperties);
            this.tcContainer.Controls.Add(this.tpOutlining);
            this.tcContainer.Controls.Add(this.tpPrinting);
            this.tcContainer.Controls.Add(this.tpSelection);
            this.tcContainer.Controls.Add(this.tpNavigate);
            this.tcContainer.Controls.Add(this.tpLineNumbers);
            this.tcContainer.Controls.Add(this.tpLineStyles);
            this.tcContainer.Controls.Add(this.tpOther);
            this.tcContainer.Controls.Add(this.tpDialogs);
            this.tcContainer.Controls.Add(this.tpScrollbarAnnotations);
            this.tcContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcContainer.Location = new System.Drawing.Point(0, 0);
            this.tcContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tcContainer.Name = "tcContainer";
            this.tcContainer.SelectedIndex = 0;
            this.tcContainer.Size = new System.Drawing.Size(1091, 177);
            this.tcContainer.TabIndex = 2;
            this.tcContainer.Visible = false;
            // 
            // tpGutter
            // 
            this.tpGutter.Controls.Add(this.pnGutter);
            this.tpGutter.Location = new System.Drawing.Point(4, 25);
            this.tpGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpGutter.Name = "tpGutter";
            this.tpGutter.Size = new System.Drawing.Size(1083, 148);
            this.tpGutter.TabIndex = 0;
            this.tpGutter.Text = "Gutter";
            // 
            // pnGutter
            // 
            this.pnGutter.BackColor = System.Drawing.SystemColors.Control;
            this.pnGutter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnGutter.Controls.Add(this.gbGutter);
            this.pnGutter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnGutter.Location = new System.Drawing.Point(0, 0);
            this.pnGutter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnGutter.Name = "pnGutter";
            this.pnGutter.Size = new System.Drawing.Size(1083, 137);
            this.pnGutter.TabIndex = 1;
            // 
            // tpWordWrap
            // 
            this.tpWordWrap.Controls.Add(this.pnWordWrap);
            this.tpWordWrap.Location = new System.Drawing.Point(4, 25);
            this.tpWordWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpWordWrap.Name = "tpWordWrap";
            this.tpWordWrap.Size = new System.Drawing.Size(1081, 148);
            this.tpWordWrap.TabIndex = 1;
            this.tpWordWrap.Text = "WordWrap";
            this.tpWordWrap.Visible = false;
            // 
            // pnWordWrap
            // 
            this.pnWordWrap.BackColor = System.Drawing.SystemColors.Control;
            this.pnWordWrap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnWordWrap.Controls.Add(this.gbWordWrap);
            this.pnWordWrap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnWordWrap.Location = new System.Drawing.Point(0, 0);
            this.pnWordWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnWordWrap.Name = "pnWordWrap";
            this.pnWordWrap.Size = new System.Drawing.Size(1081, 137);
            this.pnWordWrap.TabIndex = 1;
            // 
            // gbWordWrap
            // 
            this.gbWordWrap.Controls.Add(this.chbFlatScrollBars);
            this.gbWordWrap.Controls.Add(this.chbSystemScrollBars);
            this.gbWordWrap.Controls.Add(this.chbScrollButtons);
            this.gbWordWrap.Controls.Add(this.chbAllowSplit);
            this.gbWordWrap.Controls.Add(this.chbWrapAtMargin);
            this.gbWordWrap.Controls.Add(this.chbWordWrap);
            this.gbWordWrap.Controls.Add(this.chbShowScrollHint);
            this.gbWordWrap.Controls.Add(this.chbSmoothScroll);
            this.gbWordWrap.Location = new System.Drawing.Point(11, 10);
            this.gbWordWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbWordWrap.Name = "gbWordWrap";
            this.gbWordWrap.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbWordWrap.Size = new System.Drawing.Size(661, 118);
            this.gbWordWrap.TabIndex = 13;
            this.gbWordWrap.TabStop = false;
            this.gbWordWrap.Text = "Word Wrap && Scrolling";
            // 
            // chbFlatScrollBars
            // 
            this.chbFlatScrollBars.Location = new System.Drawing.Point(384, 49);
            this.chbFlatScrollBars.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbFlatScrollBars.Name = "chbFlatScrollBars";
            this.chbFlatScrollBars.Size = new System.Drawing.Size(139, 26);
            this.chbFlatScrollBars.TabIndex = 8;
            this.chbFlatScrollBars.Text = "Flat Scroll";
            this.chbFlatScrollBars.Visible = false;
            this.chbFlatScrollBars.CheckedChanged += new System.EventHandler(this.FlatScrolarsCheckBoxLisoxTextBox_CheckedChanged);
            // 
            // chbSystemScrollBars
            // 
            this.chbSystemScrollBars.Location = new System.Drawing.Point(384, 20);
            this.chbSystemScrollBars.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbSystemScrollBars.Name = "chbSystemScrollBars";
            this.chbSystemScrollBars.Size = new System.Drawing.Size(139, 26);
            this.chbSystemScrollBars.TabIndex = 7;
            this.chbSystemScrollBars.Text = "System Scroll";
            this.chbSystemScrollBars.Visible = false;
            this.chbSystemScrollBars.CheckedChanged += new System.EventHandler(this.SystemScrolarsCheckBoxLisoxTextBox_CheckedChanged);
            // 
            // chbScrollButtons
            // 
            this.chbScrollButtons.Location = new System.Drawing.Point(192, 79);
            this.chbScrollButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbScrollButtons.Name = "chbScrollButtons";
            this.chbScrollButtons.Size = new System.Drawing.Size(139, 26);
            this.chbScrollButtons.TabIndex = 6;
            this.chbScrollButtons.Text = "Scroll Buttons";
            this.chbScrollButtons.CheckedChanged += new System.EventHandler(this.ScroluttonsCheckBoxLisoxTextBox_CheckedChanged);
            this.chbScrollButtons.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScroluttonsCheckBoxLisoxTextBox_MouseMove);
            // 
            // chbAllowSplit
            // 
            this.chbAllowSplit.Location = new System.Drawing.Point(192, 49);
            this.chbAllowSplit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbAllowSplit.Name = "chbAllowSplit";
            this.chbAllowSplit.Size = new System.Drawing.Size(139, 26);
            this.chbAllowSplit.TabIndex = 5;
            this.chbAllowSplit.Text = "Allow Split";
            this.chbAllowSplit.CheckedChanged += new System.EventHandler(this.AllowSplitCheckBox_CheckedChanged);
            this.chbAllowSplit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowSplitCheckBox_MouseMove);
            // 
            // chbWrapAtMargin
            // 
            this.chbWrapAtMargin.Location = new System.Drawing.Point(11, 49);
            this.chbWrapAtMargin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWrapAtMargin.Name = "chbWrapAtMargin";
            this.chbWrapAtMargin.Size = new System.Drawing.Size(139, 26);
            this.chbWrapAtMargin.TabIndex = 1;
            this.chbWrapAtMargin.Text = "Wrap at Margin";
            this.chbWrapAtMargin.CheckedChanged += new System.EventHandler(this.WrapAtMarginCheckBox_CheckedChanged);
            this.chbWrapAtMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WrapAtMarginCheckBox_MouseMove);
            // 
            // chbWordWrap
            // 
            this.chbWordWrap.Location = new System.Drawing.Point(11, 20);
            this.chbWordWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new System.Drawing.Size(139, 26);
            this.chbWordWrap.TabIndex = 0;
            this.chbWordWrap.Text = "Word Wrap";
            this.chbWordWrap.CheckedChanged += new System.EventHandler(this.WordWrapCheckBox_CheckedChanged);
            this.chbWordWrap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WordWrapCheckBox_MouseMove);
            // 
            // chbShowScrollHint
            // 
            this.chbShowScrollHint.Location = new System.Drawing.Point(11, 79);
            this.chbShowScrollHint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowScrollHint.Name = "chbShowScrollHint";
            this.chbShowScrollHint.Size = new System.Drawing.Size(149, 26);
            this.chbShowScrollHint.TabIndex = 3;
            this.chbShowScrollHint.Text = "Display Scroll Hint";
            this.chbShowScrollHint.CheckedChanged += new System.EventHandler(this.ShowScrollHintCheckBox_CheckedChanged);
            this.chbShowScrollHint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowScrollHintCheckBox_MouseMove);
            // 
            // chbSmoothScroll
            // 
            this.chbSmoothScroll.Location = new System.Drawing.Point(192, 20);
            this.chbSmoothScroll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbSmoothScroll.Name = "chbSmoothScroll";
            this.chbSmoothScroll.Size = new System.Drawing.Size(139, 26);
            this.chbSmoothScroll.TabIndex = 4;
            this.chbSmoothScroll.Text = "Smooth Scroll";
            this.chbSmoothScroll.CheckedChanged += new System.EventHandler(this.SmoothScrollCheckBox_CheckedChanged);
            this.chbSmoothScroll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SmoothScrollCheckBox_MouseMove);
            // 
            // tpTextSource
            // 
            this.tpTextSource.Controls.Add(this.pnTextSource);
            this.tpTextSource.Location = new System.Drawing.Point(4, 25);
            this.tpTextSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpTextSource.Name = "tpTextSource";
            this.tpTextSource.Size = new System.Drawing.Size(1081, 148);
            this.tpTextSource.TabIndex = 6;
            this.tpTextSource.Text = "Text Source";
            this.tpTextSource.Visible = false;
            // 
            // pnTextSource
            // 
            this.pnTextSource.BackColor = System.Drawing.SystemColors.Control;
            this.pnTextSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnTextSource.Controls.Add(this.laSource);
            this.pnTextSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTextSource.Location = new System.Drawing.Point(0, 0);
            this.pnTextSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnTextSource.Name = "pnTextSource";
            this.pnTextSource.Size = new System.Drawing.Size(1081, 89);
            this.pnTextSource.TabIndex = 4;
            // 
            // laSource
            // 
            this.laSource.AutoSize = true;
            this.laSource.Location = new System.Drawing.Point(213, 30);
            this.laSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laSource.Name = "laSource";
            this.laSource.Size = new System.Drawing.Size(254, 16);
            this.laSource.TabIndex = 0;
            this.laSource.Text = "Several Editor can work with the same text";
            // 
            // tpPageLayout
            // 
            this.tpPageLayout.Controls.Add(this.pnPageLayout);
            this.tpPageLayout.Location = new System.Drawing.Point(4, 25);
            this.tpPageLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpPageLayout.Name = "tpPageLayout";
            this.tpPageLayout.Size = new System.Drawing.Size(1083, 148);
            this.tpPageLayout.TabIndex = 13;
            this.tpPageLayout.Text = "Page Layout";
            // 
            // tpTextMate
            // 
            this.tpTextMate.Controls.Add(this.pnTextMate);
            this.tpTextMate.Location = new System.Drawing.Point(4, 25);
            this.tpTextMate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpTextMate.Name = "tpTextMate";
            this.tpTextMate.Size = new System.Drawing.Size(1083, 148);
            this.tpTextMate.TabIndex = 14;
            this.tpTextMate.Text = "Text Mate";
            // 
            // pnPageLayout
            // 
            this.pnPageLayout.BackColor = System.Drawing.SystemColors.Control;
            this.pnPageLayout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnPageLayout.Controls.Add(this.gbRulers);
            this.pnPageLayout.Controls.Add(this.gbPages);
            this.pnPageLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPageLayout.Location = new System.Drawing.Point(0, 0);
            this.pnPageLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnPageLayout.Name = "pnPageLayout";
            this.pnPageLayout.Size = new System.Drawing.Size(1083, 137);
            this.pnPageLayout.TabIndex = 2;
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(4);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(914, 48);
            this.pnDescription.TabIndex = 2;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(914, 48);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "TextMate parser support lexical analysis for the vast subset of programming languages." +
    "It includes all language Visual Studio Code grammars, and provides some advanced text editing features like indentation-based outlining and structure guidelines and code snippets and basic code completion.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnTextMate
            // 
            this.pnTextMate.BackColor = System.Drawing.SystemColors.Control;
            this.pnTextMate.Controls.Add(this.pnDescription);
            this.pnTextMate.Controls.Add(this.LanguagesCombobox);
            this.pnTextMate.Controls.Add(this.LanguagesLabel);
            this.pnTextMate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnTextMate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTextMate.Location = new System.Drawing.Point(0, 0);
            this.pnTextMate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnTextMate.Name = "pnTextMate";
            this.pnTextMate.Size = new System.Drawing.Size(1083, 137);
            this.pnTextMate.TabIndex = 2;
            // 
            // gbRulers
            // 
            this.gbRulers.Controls.Add(this.chbVertRuler);
            this.gbRulers.Controls.Add(this.chbHorzRuler);
            this.gbRulers.Controls.Add(this.cbRulerUnits);
            this.gbRulers.Controls.Add(this.laRulerUnits);
            this.gbRulers.Controls.Add(this.chbRulerDisplayDragLines);
            this.gbRulers.Controls.Add(this.chbRulerDiscrete);
            this.gbRulers.Controls.Add(this.chbRulerAllowDrag);
            this.gbRulers.Location = new System.Drawing.Point(277, 10);
            this.gbRulers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRulers.Name = "gbRulers";
            this.gbRulers.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRulers.Size = new System.Drawing.Size(395, 118);
            this.gbRulers.TabIndex = 14;
            this.gbRulers.TabStop = false;
            this.gbRulers.Text = "Rulers";
            // 
            // chbVertRuler
            // 
            this.chbVertRuler.Location = new System.Drawing.Point(21, 49);
            this.chbVertRuler.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbVertRuler.Name = "chbVertRuler";
            this.chbVertRuler.Size = new System.Drawing.Size(139, 26);
            this.chbVertRuler.TabIndex = 2;
            this.chbVertRuler.Text = "Vert Ruler";
            this.chbVertRuler.CheckedChanged += new System.EventHandler(this.VertRulerCheckBox_CheckedChanged);
            this.chbVertRuler.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VertRulerCheckBox_MouseMove);
            // 
            // chbHorzRuler
            // 
            this.chbHorzRuler.Location = new System.Drawing.Point(21, 20);
            this.chbHorzRuler.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHorzRuler.Name = "chbHorzRuler";
            this.chbHorzRuler.Size = new System.Drawing.Size(139, 26);
            this.chbHorzRuler.TabIndex = 1;
            this.chbHorzRuler.Text = "Horz Ruler";
            this.chbHorzRuler.CheckedChanged += new System.EventHandler(this.HorzRulerCheckBox_CheckedChanged);
            this.chbHorzRuler.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HorzRulerCheckBox_MouseMove);
            // 
            // cbRulerUnits
            // 
            this.cbRulerUnits.Location = new System.Drawing.Point(96, 79);
            this.cbRulerUnits.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbRulerUnits.Name = "cbRulerUnits";
            this.cbRulerUnits.Size = new System.Drawing.Size(127, 24);
            this.cbRulerUnits.TabIndex = 4;
            this.cbRulerUnits.SelectedIndexChanged += new System.EventHandler(this.RulerUnitsComboBox_SelectedIndexChanged);
            this.cbRulerUnits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerUnitsComboBox_MouseMove);
            // 
            // laRulerUnits
            // 
            this.laRulerUnits.AutoSize = true;
            this.laRulerUnits.Location = new System.Drawing.Point(11, 82);
            this.laRulerUnits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laRulerUnits.Name = "laRulerUnits";
            this.laRulerUnits.Size = new System.Drawing.Size(72, 16);
            this.laRulerUnits.TabIndex = 3;
            this.laRulerUnits.Text = "Ruler Units";
            // 
            // chbRulerDisplayDragLines
            // 
            this.chbRulerDisplayDragLines.Location = new System.Drawing.Point(235, 79);
            this.chbRulerDisplayDragLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbRulerDisplayDragLines.Name = "chbRulerDisplayDragLines";
            this.chbRulerDisplayDragLines.Size = new System.Drawing.Size(157, 26);
            this.chbRulerDisplayDragLines.TabIndex = 7;
            this.chbRulerDisplayDragLines.Text = "Display Drag Lines";
            this.chbRulerDisplayDragLines.CheckedChanged += new System.EventHandler(this.RulerDisplayDragLinesCheckBox_CheckedChanged);
            this.chbRulerDisplayDragLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerDisplayDragLinesCheckBox_MouseMove);
            // 
            // chbRulerDiscrete
            // 
            this.chbRulerDiscrete.Location = new System.Drawing.Point(235, 49);
            this.chbRulerDiscrete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbRulerDiscrete.Name = "chbRulerDiscrete";
            this.chbRulerDiscrete.Size = new System.Drawing.Size(139, 26);
            this.chbRulerDiscrete.TabIndex = 6;
            this.chbRulerDiscrete.Text = "Discrete";
            this.chbRulerDiscrete.CheckedChanged += new System.EventHandler(this.RulerDiscreteCheckBox_CheckedChanged);
            this.chbRulerDiscrete.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerDiscreteCheckBox_MouseMove);
            // 
            // chbRulerAllowDrag
            // 
            this.chbRulerAllowDrag.Location = new System.Drawing.Point(235, 20);
            this.chbRulerAllowDrag.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbRulerAllowDrag.Name = "chbRulerAllowDrag";
            this.chbRulerAllowDrag.Size = new System.Drawing.Size(139, 26);
            this.chbRulerAllowDrag.TabIndex = 5;
            this.chbRulerAllowDrag.Text = "Allow Drag";
            this.chbRulerAllowDrag.CheckedChanged += new System.EventHandler(this.RulerAllowDragCheckBox_CheckedChanged);
            this.chbRulerAllowDrag.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerAllowDragCheckBox_MouseMove);
            // 
            // gbPages
            // 
            this.gbPages.Controls.Add(this.cbPageSize);
            this.gbPages.Controls.Add(this.laPageSize);
            this.gbPages.Controls.Add(this.cbPageLayout);
            this.gbPages.Controls.Add(this.laPageLayout);
            this.gbPages.Location = new System.Drawing.Point(11, 10);
            this.gbPages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPages.Name = "gbPages";
            this.gbPages.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPages.Size = new System.Drawing.Size(245, 118);
            this.gbPages.TabIndex = 13;
            this.gbPages.TabStop = false;
            this.gbPages.Text = "Pages";
            // 
            // cbPageSize
            // 
            this.cbPageSize.Location = new System.Drawing.Point(107, 59);
            this.cbPageSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPageSize.Name = "cbPageSize";
            this.cbPageSize.Size = new System.Drawing.Size(127, 24);
            this.cbPageSize.TabIndex = 4;
            this.cbPageSize.SelectedIndexChanged += new System.EventHandler(this.PageSizeComboBox_SelectedIndexChanged);
            this.cbPageSize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageSizeComboBox_MouseMove);
            // 
            // laPageSize
            // 
            this.laPageSize.AutoSize = true;
            this.laPageSize.Location = new System.Drawing.Point(11, 63);
            this.laPageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laPageSize.Name = "laPageSize";
            this.laPageSize.Size = new System.Drawing.Size(69, 16);
            this.laPageSize.TabIndex = 3;
            this.laPageSize.Text = "Page Size";
            // 
            // cbPageLayout
            // 
            this.cbPageLayout.Location = new System.Drawing.Point(107, 30);
            this.cbPageLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPageLayout.Name = "cbPageLayout";
            this.cbPageLayout.Size = new System.Drawing.Size(127, 24);
            this.cbPageLayout.TabIndex = 2;
            this.cbPageLayout.SelectedIndexChanged += new System.EventHandler(this.PageLayoutComboBox_SelectedIndexChanged);
            this.cbPageLayout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageLayoutComboBox_MouseMove);
            // 
            // laPageLayout
            // 
            this.laPageLayout.AutoSize = true;
            this.laPageLayout.Location = new System.Drawing.Point(11, 33);
            this.laPageLayout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laPageLayout.Name = "laPageLayout";
            this.laPageLayout.Size = new System.Drawing.Size(83, 16);
            this.laPageLayout.TabIndex = 1;
            this.laPageLayout.Text = "Page Layout";
            // 
            // tpSpellAndUrl
            // 
            this.tpSpellAndUrl.Controls.Add(this.pnSpellAndUrl);
            this.tpSpellAndUrl.Location = new System.Drawing.Point(4, 25);
            this.tpSpellAndUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpSpellAndUrl.Name = "tpSpellAndUrl";
            this.tpSpellAndUrl.Size = new System.Drawing.Size(1081, 148);
            this.tpSpellAndUrl.TabIndex = 5;
            this.tpSpellAndUrl.Text = "Spell&&Url";
            this.tpSpellAndUrl.Visible = false;
            // 
            // pnSpellAndUrl
            // 
            this.pnSpellAndUrl.BackColor = System.Drawing.SystemColors.Control;
            this.pnSpellAndUrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnSpellAndUrl.Controls.Add(this.gbSpellAndUrl);
            this.pnSpellAndUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSpellAndUrl.Location = new System.Drawing.Point(0, 0);
            this.pnSpellAndUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnSpellAndUrl.Name = "pnSpellAndUrl";
            this.pnSpellAndUrl.Size = new System.Drawing.Size(1081, 137);
            this.pnSpellAndUrl.TabIndex = 3;
            // 
            // gbSpellAndUrl
            // 
            this.gbSpellAndUrl.Controls.Add(this.chbShowHyperTextHints);
            this.gbSpellAndUrl.Controls.Add(this.chbCheckSpelling);
            this.gbSpellAndUrl.Controls.Add(this.chbHighlightUrls);
            this.gbSpellAndUrl.Location = new System.Drawing.Point(11, 10);
            this.gbSpellAndUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSpellAndUrl.Name = "gbSpellAndUrl";
            this.gbSpellAndUrl.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbSpellAndUrl.Size = new System.Drawing.Size(661, 118);
            this.gbSpellAndUrl.TabIndex = 0;
            this.gbSpellAndUrl.TabStop = false;
            this.gbSpellAndUrl.Text = "Spelling && HyperText";
            // 
            // tpScrollbarAnnotations
            // 
            this.tpScrollbarAnnotations.BackColor = System.Drawing.SystemColors.Control;
            this.tpScrollbarAnnotations.Controls.Add(this.pnScrollbarAnnotations);
            this.tpScrollbarAnnotations.Location = new System.Drawing.Point(4, 25);
            this.tpScrollbarAnnotations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpScrollbarAnnotations.Name = "tpScrollbarAnnotations";
            this.tpScrollbarAnnotations.Size = new System.Drawing.Size(1081, 148);
            this.tpScrollbarAnnotations.TabIndex = 15;
            this.tpScrollbarAnnotations.Text = "Scroll bar Annotations";
            // 
            // pnScrollbarAnnotations
            // 
            this.pnScrollbarAnnotations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnScrollbarAnnotations.Controls.Add(this.separatorTextBox);
            this.pnScrollbarAnnotations.Controls.Add(this.changeErrorsAppearanceCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.customAnnotationsCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.saveButton);
            this.pnScrollbarAnnotations.Controls.Add(this.scrollBarsVisualStyleComboBox);
            this.pnScrollbarAnnotations.Controls.Add(this.scrollBarsVisualStyleLabel);
            this.pnScrollbarAnnotations.Controls.Add(this.customTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.cursorPositionTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.syntaxErrorsTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.searchResultsTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.bookmarksTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.changedLinesTypeCheckBox);
            this.pnScrollbarAnnotations.Controls.Add(this.scrollBarAnnotationsEnabledCheckBox);
            this.pnScrollbarAnnotations.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnScrollbarAnnotations.Location = new System.Drawing.Point(0, 0);
            this.pnScrollbarAnnotations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnScrollbarAnnotations.Name = "pnScrollbarAnnotations";
            this.pnScrollbarAnnotations.Size = new System.Drawing.Size(1081, 137);
            this.pnScrollbarAnnotations.TabIndex = 0;
            // 
            // separatorTextBox
            // 
            this.separatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.separatorTextBox.Enabled = false;
            this.separatorTextBox.Location = new System.Drawing.Point(329, 14);
            this.separatorTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.separatorTextBox.Multiline = true;
            this.separatorTextBox.Name = "separatorTextBox";
            this.separatorTextBox.Size = new System.Drawing.Size(1, 123);
            this.separatorTextBox.TabIndex = 17;
            // 
            // changeErrorsAppearanceCheckBox
            // 
            this.changeErrorsAppearanceCheckBox.AutoSize = true;
            this.changeErrorsAppearanceCheckBox.Location = new System.Drawing.Point(781, 47);
            this.changeErrorsAppearanceCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.changeErrorsAppearanceCheckBox.Name = "changeErrorsAppearanceCheckBox";
            this.changeErrorsAppearanceCheckBox.Size = new System.Drawing.Size(233, 20);
            this.changeErrorsAppearanceCheckBox.TabIndex = 16;
            this.changeErrorsAppearanceCheckBox.Text = "Change Errors Appearance Demo";
            this.changeErrorsAppearanceCheckBox.UseVisualStyleBackColor = true;
            this.changeErrorsAppearanceCheckBox.CheckedChanged += new System.EventHandler(this.ChangeErrorsAppearanceCheckBox_CheckedChanged);
            // 
            // customAnnotationsCheckBox
            // 
            this.customAnnotationsCheckBox.AutoSize = true;
            this.customAnnotationsCheckBox.Location = new System.Drawing.Point(781, 16);
            this.customAnnotationsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customAnnotationsCheckBox.Name = "customAnnotationsCheckBox";
            this.customAnnotationsCheckBox.Size = new System.Drawing.Size(187, 20);
            this.customAnnotationsCheckBox.TabIndex = 15;
            this.customAnnotationsCheckBox.Text = "Custom Annotations Demo";
            this.customAnnotationsCheckBox.UseVisualStyleBackColor = true;
            this.customAnnotationsCheckBox.CheckedChanged += new System.EventHandler(this.CustomAnnotationsCheckBox_CheckedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(781, 78);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(167, 33);
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Save Text Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_ButtonClick);
            // 
            // scrollBarsVisualStyleComboBox
            // 
            this.scrollBarsVisualStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scrollBarsVisualStyleComboBox.FormattingEnabled = true;
            this.scrollBarsVisualStyleComboBox.Location = new System.Drawing.Point(364, 42);
            this.scrollBarsVisualStyleComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scrollBarsVisualStyleComboBox.Name = "scrollBarsVisualStyleComboBox";
            this.scrollBarsVisualStyleComboBox.Size = new System.Drawing.Size(261, 24);
            this.scrollBarsVisualStyleComboBox.TabIndex = 13;
            this.scrollBarsVisualStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.ScrolarsVisualStyleComboBoxLisoxTextBox_SelectedIndexChanged);
            // 
            // scrollBarsVisualStyleLabel
            // 
            this.scrollBarsVisualStyleLabel.AutoSize = true;
            this.scrollBarsVisualStyleLabel.Location = new System.Drawing.Point(360, 16);
            this.scrollBarsVisualStyleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.scrollBarsVisualStyleLabel.Name = "scrollBarsVisualStyleLabel";
            this.scrollBarsVisualStyleLabel.Size = new System.Drawing.Size(148, 16);
            this.scrollBarsVisualStyleLabel.TabIndex = 12;
            this.scrollBarsVisualStyleLabel.Text = "Scroll Bars Visual Style:";
            // 
            // customTypeCheckBox
            // 
            this.customTypeCheckBox.AutoSize = true;
            this.customTypeCheckBox.Checked = true;
            this.customTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customTypeCheckBox.Location = new System.Drawing.Point(191, 108);
            this.customTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customTypeCheckBox.Name = "customTypeCheckBox";
            this.customTypeCheckBox.Size = new System.Drawing.Size(74, 20);
            this.customTypeCheckBox.TabIndex = 11;
            this.customTypeCheckBox.Text = "Custom";
            this.customTypeCheckBox.UseVisualStyleBackColor = true;
            this.customTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // cursorPositionTypeCheckBox
            // 
            this.cursorPositionTypeCheckBox.AutoSize = true;
            this.cursorPositionTypeCheckBox.Checked = true;
            this.cursorPositionTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cursorPositionTypeCheckBox.Location = new System.Drawing.Point(191, 78);
            this.cursorPositionTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cursorPositionTypeCheckBox.Name = "cursorPositionTypeCheckBox";
            this.cursorPositionTypeCheckBox.Size = new System.Drawing.Size(119, 20);
            this.cursorPositionTypeCheckBox.TabIndex = 10;
            this.cursorPositionTypeCheckBox.Text = "Cursor Position";
            this.cursorPositionTypeCheckBox.UseVisualStyleBackColor = true;
            this.cursorPositionTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // syntaxErrorsTypeCheckBox
            // 
            this.syntaxErrorsTypeCheckBox.AutoSize = true;
            this.syntaxErrorsTypeCheckBox.Checked = true;
            this.syntaxErrorsTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.syntaxErrorsTypeCheckBox.Location = new System.Drawing.Point(191, 47);
            this.syntaxErrorsTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.syntaxErrorsTypeCheckBox.Name = "syntaxErrorsTypeCheckBox";
            this.syntaxErrorsTypeCheckBox.Size = new System.Drawing.Size(108, 20);
            this.syntaxErrorsTypeCheckBox.TabIndex = 9;
            this.syntaxErrorsTypeCheckBox.Text = "Syntax Errors";
            this.syntaxErrorsTypeCheckBox.UseVisualStyleBackColor = true;
            this.syntaxErrorsTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // searchResultsTypeCheckBox
            // 
            this.searchResultsTypeCheckBox.AutoSize = true;
            this.searchResultsTypeCheckBox.Checked = true;
            this.searchResultsTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.searchResultsTypeCheckBox.Location = new System.Drawing.Point(53, 108);
            this.searchResultsTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchResultsTypeCheckBox.Name = "searchResultsTypeCheckBox";
            this.searchResultsTypeCheckBox.Size = new System.Drawing.Size(120, 20);
            this.searchResultsTypeCheckBox.TabIndex = 8;
            this.searchResultsTypeCheckBox.Text = "Search Results";
            this.searchResultsTypeCheckBox.UseVisualStyleBackColor = true;
            this.searchResultsTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // bookmarksTypeCheckBox
            // 
            this.bookmarksTypeCheckBox.AutoSize = true;
            this.bookmarksTypeCheckBox.Checked = true;
            this.bookmarksTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bookmarksTypeCheckBox.Location = new System.Drawing.Point(53, 78);
            this.bookmarksTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bookmarksTypeCheckBox.Name = "bookmarksTypeCheckBox";
            this.bookmarksTypeCheckBox.Size = new System.Drawing.Size(98, 20);
            this.bookmarksTypeCheckBox.TabIndex = 6;
            this.bookmarksTypeCheckBox.Text = "Bookmarks";
            this.bookmarksTypeCheckBox.UseVisualStyleBackColor = true;
            this.bookmarksTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // changedLinesTypeCheckBox
            // 
            this.changedLinesTypeCheckBox.AutoSize = true;
            this.changedLinesTypeCheckBox.Checked = true;
            this.changedLinesTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.changedLinesTypeCheckBox.Location = new System.Drawing.Point(53, 47);
            this.changedLinesTypeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.changedLinesTypeCheckBox.Name = "changedLinesTypeCheckBox";
            this.changedLinesTypeCheckBox.Size = new System.Drawing.Size(119, 20);
            this.changedLinesTypeCheckBox.TabIndex = 5;
            this.changedLinesTypeCheckBox.Text = "Changed Lines";
            this.changedLinesTypeCheckBox.UseVisualStyleBackColor = true;
            this.changedLinesTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // scrollBarAnnotationsEnabledCheckBox
            // 
            this.scrollBarAnnotationsEnabledCheckBox.AutoSize = true;
            this.scrollBarAnnotationsEnabledCheckBox.Checked = true;
            this.scrollBarAnnotationsEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scrollBarAnnotationsEnabledCheckBox.Location = new System.Drawing.Point(17, 16);
            this.scrollBarAnnotationsEnabledCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scrollBarAnnotationsEnabledCheckBox.Name = "scrollBarAnnotationsEnabledCheckBox";
            this.scrollBarAnnotationsEnabledCheckBox.Size = new System.Drawing.Size(214, 20);
            this.scrollBarAnnotationsEnabledCheckBox.TabIndex = 4;
            this.scrollBarAnnotationsEnabledCheckBox.Text = "Scroll Bar Annotations Enabled";
            this.scrollBarAnnotationsEnabledCheckBox.UseVisualStyleBackColor = true;
            this.scrollBarAnnotationsEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ScrolarAnnotationsEnabledCheckBoxLisoxTextBox_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.Filter = "Rtf files (*.rtf)|*.rtf|Html files (*.html; *.htm)|*.html;*.htm|Xml files (*.xml)" +
    "|*.xml|All files (*.*)|*.*";
            // 
            // tpVisualThemes
            // 
            this.tpVisualThemes.Controls.Add(this.pnVisualThemes);
            this.tpVisualThemes.Location = new System.Drawing.Point(4, 22);
            this.tpVisualThemes.Name = "tpVisualThemes";
            this.tpVisualThemes.Padding = new System.Windows.Forms.Padding(3);
            this.tpVisualThemes.Size = new System.Drawing.Size(809, 118);
            this.tpVisualThemes.TabIndex = 16;
            this.tpVisualThemes.Text = "Visual Themes";
            this.tpVisualThemes.UseVisualStyleBackColor = true;
            // 
            // pnVisualThemes
            // 
            this.pnVisualThemes.BackColor = System.Drawing.SystemColors.Control;
            this.pnVisualThemes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnVisualThemes.Controls.Add(this.visualThemeComboBox);
            this.pnVisualThemes.Controls.Add(this.visualThemeLabel);
            this.pnVisualThemes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnVisualThemes.Location = new System.Drawing.Point(3, 3);
            this.pnVisualThemes.Name = "pnVisualThemes";
            this.pnVisualThemes.Size = new System.Drawing.Size(803, 112);
            this.pnVisualThemes.TabIndex = 0;
            // 
            // visualThemeComboBox
            // 
            this.visualThemeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.visualThemeComboBox.FormattingEnabled = true;
            this.visualThemeComboBox.Location = new System.Drawing.Point(193, 16);
            this.visualThemeComboBox.Name = "visualThemeComboBox";
            this.visualThemeComboBox.Size = new System.Drawing.Size(179, 24);
            this.visualThemeComboBox.TabIndex = 1;
            this.visualThemeComboBox.SelectedIndexChanged += new System.EventHandler(this.VisualThemeComboBox_SelectedIndexChanged);
            // 
            // visualThemeLabel
            // 
            this.visualThemeLabel.AutoSize = true;
            this.visualThemeLabel.Location = new System.Drawing.Point(13, 20);
            this.visualThemeLabel.Name = "visualThemeLabel";
            this.visualThemeLabel.Size = new System.Drawing.Size(93, 16);
            this.visualThemeLabel.TabIndex = 0;
            this.visualThemeLabel.Text = "Visual Theme:";
            // 
            // LanguagesCombobox
            // 
            this.LanguagesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguagesCombobox.FormattingEnabled = true;
            this.LanguagesCombobox.Location = new System.Drawing.Point(95, 70);
            this.LanguagesCombobox.Name = "LanguagesCombobox";
            this.LanguagesCombobox.Size = new System.Drawing.Size(169, 24);
            this.LanguagesCombobox.TabIndex = 1;
            this.LanguagesCombobox.SelectedIndexChanged += new System.EventHandler(this.LanguagesCombobox_SelectedIndexChanged);
            // 
            // LanguagesLabel
            // 
            this.LanguagesLabel.AutoSize = true;
            this.LanguagesLabel.Location = new System.Drawing.Point(12, 74);
            this.LanguagesLabel.Name = "LanguagesLabel";
            this.LanguagesLabel.Size = new System.Drawing.Size(78, 16);
            this.LanguagesLabel.TabIndex = 0;
            this.LanguagesLabel.Text = "Languages:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 788);
            this.Controls.Add(this.pnMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbSelection.ResumeLayout(false);
            this.gbNavigateOptions.ResumeLayout(false);
            this.pnNavigate.ResumeLayout(false);
            this.tpNavigate.ResumeLayout(false);
            this.pnSelection.ResumeLayout(false);
            this.gbOutlining.ResumeLayout(false);
            this.pnOutlining.ResumeLayout(false);
            this.tpOutlining.ResumeLayout(false);
            this.tpSelection.ResumeLayout(false);
            this.gbPrint.ResumeLayout(false);
            this.pnPrinting.ResumeLayout(false);
            this.tpPrinting.ResumeLayout(false);
            this.pnDialogs.ResumeLayout(false);
            this.gbDialogs.ResumeLayout(false);
            this.tpDialogs.ResumeLayout(false);
            this.gbOther.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudLineNumbersStart)).EndInit();
            this.pnLineNumbers.ResumeLayout(false);
            this.gbLineNumbers.ResumeLayout(false);
            this.gbLineNumbers.PerformLayout();
            this.pnLineStyles.ResumeLayout(false);
            this.tpLineNumbers.ResumeLayout(false);
            this.tpLineStyles.ResumeLayout(false);
            this.gbBraces.ResumeLayout(false);
            this.pnOther.ResumeLayout(false);
            this.tpOther.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.pnCompanyInfo.ResumeLayout(false);
            this.pnCompanyInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tpCompanyInfo.ResumeLayout(false);
            this.tpMargin.ResumeLayout(false);
            this.pnMargin.ResumeLayout(false);
            this.gbMargin.ResumeLayout(false);
            this.gbMargin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMarginPos)).EndInit();
            this.gbGutter.ResumeLayout(false);
            this.gbGutter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGutterWidth)).EndInit();
            this.pnMain.ResumeLayout(false);
            this.pnEditContainer.ResumeLayout(false);
            this.pnManage.ResumeLayout(false);
            this.tcContainer.ResumeLayout(false);
            this.tpGutter.ResumeLayout(false);
            this.pnGutter.ResumeLayout(false);
            this.tpWordWrap.ResumeLayout(false);
            this.pnWordWrap.ResumeLayout(false);
            this.gbWordWrap.ResumeLayout(false);
            this.tpTextSource.ResumeLayout(false);
            this.pnTextSource.ResumeLayout(false);
            this.pnTextSource.PerformLayout();
            this.tpPageLayout.ResumeLayout(false);
            this.tpTextMate.ResumeLayout(false);
            this.pnPageLayout.ResumeLayout(false);
            this.pnTextMate.ResumeLayout(false);
            this.gbRulers.ResumeLayout(false);
            this.gbRulers.PerformLayout();
            this.gbPages.ResumeLayout(false);
            this.gbPages.PerformLayout();
            this.tpSpellAndUrl.ResumeLayout(false);
            this.pnSpellAndUrl.ResumeLayout(false);
            this.gbSpellAndUrl.ResumeLayout(false);
            this.tpScrollbarAnnotations.ResumeLayout(false);
            this.pnScrollbarAnnotations.ResumeLayout(false);
            this.pnScrollbarAnnotations.PerformLayout();
            this.tpVisualThemes.ResumeLayout(false);
            this.pnVisualThemes.ResumeLayout(false);
            this.pnVisualThemes.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btPrintSetup;
        private System.Windows.Forms.Button btPrintOptions;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.CheckBox chbPersistentBlocks;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.CheckBox HighlightSelectedWordsCheckBox;
        private System.Windows.Forms.CheckBox chbOverwriteBlocks;
        private System.Windows.Forms.Button btPrintPreview;
        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.CheckBox chbSelectLineOnDblClick;
        private System.Windows.Forms.CheckBox chbHideSelection;
        private System.Windows.Forms.CheckBox chbUseColors;
        private System.Windows.Forms.CheckBox chbSelectBeyondEol;
        private System.Windows.Forms.CheckBox chbDisableDragging;
        private System.Windows.Forms.CheckBox chbDisableSelection;
        private System.Windows.Forms.GroupBox gbNavigateOptions;
        private System.Windows.Forms.CheckBox chbMoveOnRightButton;
        private System.Windows.Forms.CheckBox chbDownAtLineEnd;
        private System.Windows.Forms.CheckBox chbUpAtLineBegin;
        private System.Windows.Forms.CheckBox chbBeyondEof;
        private System.Windows.Forms.CheckBox chbBeyondEol;
        private System.Windows.Forms.Panel pnNavigate;
        private System.Windows.Forms.TabPage tpNavigate;
        private System.Windows.Forms.Panel pnSelection;
        private System.Windows.Forms.CheckBox chbAllowOutlining;
        private System.Windows.Forms.GroupBox gbOutlining;
        private System.Windows.Forms.CheckBox chbDrawButtons;
        private System.Windows.Forms.CheckBox chbDrawLines;
        private System.Windows.Forms.CheckBox chbDrawOnGutter;
        private System.Windows.Forms.CheckBox chbShowHints;
        private System.Windows.Forms.Panel pnOutlining;
        private System.Windows.Forms.TabPage tpOutlining;
        private System.Windows.Forms.CheckBox chbShowHyperTextHints;
        private System.Windows.Forms.CheckBox chbCheckSpelling;
        private System.Windows.Forms.TabPage tpSelection;
        private System.Windows.Forms.Button btHtml;
        private System.Windows.Forms.GroupBox gbPrint;
        private System.Windows.Forms.Button btRtf;
        private System.Windows.Forms.Panel pnPrinting;
        private System.Windows.Forms.TabPage tpPrinting;
        private System.Windows.Forms.Panel pnDialogs;
        private System.Windows.Forms.GroupBox gbDialogs;
        private System.Windows.Forms.Button btGoto;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button FindNextButton;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.TabPage tpDialogs;
        private System.Windows.Forms.CheckBox chbQuickInfoTips;
        private System.Windows.Forms.CheckBox chbTransparent;
        private System.Windows.Forms.CheckBox chbHighlightReferences;
        private System.Windows.Forms.CheckBox cbTempHighlightBraces;
        private System.Windows.Forms.CheckBox chbSeparateLines;
        private System.Windows.Forms.CheckBox chbWhiteSpaceVisible;
        private System.Windows.Forms.CheckBox chbUseRoundRect;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chbDrawColumnsIndent;
        private System.Windows.Forms.CheckBox chbLineModificator;
        private System.Windows.Forms.Label laLineNumbersStart;
        private System.Windows.Forms.GroupBox gbOther;
        private System.Windows.Forms.NumericUpDown nudLineNumbersStart;
        private System.Windows.Forms.ComboBox cbLineNumbersAlign;
        private System.Windows.Forms.Panel pnLineNumbers;
        private System.Windows.Forms.Panel pnLineStyles;
        private System.Windows.Forms.GroupBox gbLineNumbers;
        private System.Windows.Forms.Label laLineNumbersAlign;
        private System.Windows.Forms.CheckBox chbLinesOnGutter;
        private System.Windows.Forms.CheckBox chbLineNumbers;
        private System.Windows.Forms.CheckBox chbLinesBeyondEof;
        private System.Windows.Forms.CheckBox chbHighlightCurrentLine;
        private System.Windows.Forms.TabPage tpLineNumbers;
        private System.Windows.Forms.TabPage tpLineStyles;
        private System.Windows.Forms.GroupBox gbBraces;
        private System.Windows.Forms.CheckBox chbHighlightBraces;
        private System.Windows.Forms.Panel pnOther;
        private System.Windows.Forms.TabPage tpOther;
        private System.Windows.Forms.Panel pnProperties;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.TextBox tbCompanyInfo;
        private System.Windows.Forms.CheckBox chbHighlightUrls;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnCompanyInfo;
        private System.Windows.Forms.Label laMailTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label laAdress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btShowBookmarks;
        private System.Windows.Forms.TabPage tpCompanyInfo;
        private System.Windows.Forms.CheckBox chbDrawLineBookmarks;
        private System.Windows.Forms.TabPage tpMargin;
        private System.Windows.Forms.Panel pnMargin;
        private System.Windows.Forms.GroupBox gbMargin;
        private System.Windows.Forms.CheckBox chbColumnsVisible;
        private System.Windows.Forms.CheckBox chbShowMarginHints;
        private System.Windows.Forms.CheckBox chbAllowDragMargin;
        private System.Windows.Forms.NumericUpDown nudMarginPos;
        private System.Windows.Forms.Label laMarginPos;
        private System.Windows.Forms.CheckBox chbShowMargin;
        private System.Windows.Forms.CheckBox chbPaintBookMarks;
        private Alternet.Editor.SyntaxEdit syntaxEdit;
        private Alternet.Editor.TextSource.TextSource textSource1;
        private System.Windows.Forms.GroupBox gbGutter;
        private System.Windows.Forms.Label laGutterWidth;
        private System.Windows.Forms.NumericUpDown nudGutterWidth;
        private System.Windows.Forms.CheckBox chbShowGutter;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnEditContainer;
        private Alternet.Editor.SyntaxEdit syntaxSplitEdit;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel pnPropertyGrid;
        private System.Windows.Forms.Panel pnManage;
        private System.Windows.Forms.TabControl tcContainer;
        private System.Windows.Forms.TabPage tpGutter;
        private System.Windows.Forms.Panel pnGutter;
        private System.Windows.Forms.TabPage tpWordWrap;
        private System.Windows.Forms.Panel pnWordWrap;
        private System.Windows.Forms.GroupBox gbWordWrap;
        private System.Windows.Forms.CheckBox chbFlatScrollBars;
        private System.Windows.Forms.CheckBox chbSystemScrollBars;
        private System.Windows.Forms.CheckBox chbScrollButtons;
        private System.Windows.Forms.CheckBox chbAllowSplit;
        private System.Windows.Forms.CheckBox chbWrapAtMargin;
        private System.Windows.Forms.CheckBox chbWordWrap;
        private System.Windows.Forms.CheckBox chbShowScrollHint;
        private System.Windows.Forms.CheckBox chbSmoothScroll;
        private System.Windows.Forms.TabPage tpTextSource;
        private System.Windows.Forms.Panel pnTextSource;
        private System.Windows.Forms.Label laSource;
        private System.Windows.Forms.TabPage tpPageLayout;
        private System.Windows.Forms.TabPage tpTextMate;
        private System.Windows.Forms.Panel pnPageLayout;
        private System.Windows.Forms.Panel pnTextMate;
        private System.Windows.Forms.GroupBox gbRulers;
        private System.Windows.Forms.CheckBox chbVertRuler;
        private System.Windows.Forms.CheckBox chbHorzRuler;
        private System.Windows.Forms.ComboBox cbRulerUnits;
        private System.Windows.Forms.Label laRulerUnits;
        private System.Windows.Forms.CheckBox chbRulerDisplayDragLines;
        private System.Windows.Forms.CheckBox chbRulerDiscrete;
        private System.Windows.Forms.CheckBox chbRulerAllowDrag;
        private System.Windows.Forms.GroupBox gbPages;
        private System.Windows.Forms.ComboBox cbPageSize;
        private System.Windows.Forms.Label laPageSize;
        private System.Windows.Forms.ComboBox cbPageLayout;
        private System.Windows.Forms.Label laPageLayout;
        private System.Windows.Forms.TabPage tpSpellAndUrl;
        private System.Windows.Forms.Panel pnSpellAndUrl;
        private System.Windows.Forms.GroupBox gbSpellAndUrl;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tpScrollbarAnnotations;
        private System.Windows.Forms.Panel pnScrollbarAnnotations;
        private System.Windows.Forms.CheckBox bookmarksTypeCheckBox;
        private System.Windows.Forms.CheckBox changedLinesTypeCheckBox;
        private System.Windows.Forms.CheckBox scrollBarAnnotationsEnabledCheckBox;
        private System.Windows.Forms.CheckBox customTypeCheckBox;
        private System.Windows.Forms.CheckBox cursorPositionTypeCheckBox;
        private System.Windows.Forms.CheckBox syntaxErrorsTypeCheckBox;
        private System.Windows.Forms.CheckBox searchResultsTypeCheckBox;
        private System.Windows.Forms.ComboBox scrollBarsVisualStyleComboBox;
        private System.Windows.Forms.Label scrollBarsVisualStyleLabel;
        private System.Windows.Forms.CheckBox changeErrorsAppearanceCheckBox;
        private System.Windows.Forms.CheckBox customAnnotationsCheckBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox separatorTextBox;
        private System.Windows.Forms.TabPage tpVisualThemes;
        private System.Windows.Forms.Panel pnVisualThemes;
        private System.Windows.Forms.ComboBox visualThemeComboBox;
        private System.Windows.Forms.Label visualThemeLabel;
        private System.Windows.Forms.Button btSetBreakpoint;
        private System.Windows.Forms.Button btStepOver;
        private System.Windows.Forms.ComboBox LanguagesCombobox;
        private System.Windows.Forms.Label LanguagesLabel;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}