#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Lexer;
using Customize.Serialization;

namespace Customize.Dialogs
{
    #region DlgSyntaxSettings

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Some constants must be public")]
    public class DlgSyntaxSettings : System.Windows.Forms.Form, IEditorSettingsDialog
    {
        public System.Windows.Forms.Panel pnButtons;
        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.Button btOK;
        public System.Windows.Forms.Panel pnMain;
        public System.Windows.Forms.Panel pnTree;
        public System.Windows.Forms.TreeView tvProperties;
        public System.Windows.Forms.Panel pnManage;
        public System.Windows.Forms.ColorDialog colorDialog1;
        public System.Windows.Forms.TabControl tcMain;
        public System.Windows.Forms.TabPage tpGeneral;
        public System.Windows.Forms.TabPage tpFontsAndColors;
        public System.Windows.Forms.TabPage tpAdditional;
        public System.Windows.Forms.Panel pnGeneral;
        public System.Windows.Forms.GroupBox gbGutterMargin;
        public System.Windows.Forms.CheckBox chbShowMargin;
        public System.Windows.Forms.Label laGutterWidth;
        public System.Windows.Forms.CheckBox chbShowGutter;
        public System.Windows.Forms.Label laMarginPosition;
        public System.Windows.Forms.GroupBox gbDocument;
        public System.Windows.Forms.CheckBox chbDragAndDrop;
        public System.Windows.Forms.CheckBox chbHorzScrollBar;
        public System.Windows.Forms.CheckBox chbVertScrollBar;
        public System.Windows.Forms.CheckBox chbWordWrap;
        public System.Windows.Forms.CheckBox chbHighlightUrls;
        public System.Windows.Forms.Panel pnFontsColors;
        public System.Windows.Forms.Label laDisplayItems;
        public System.Windows.Forms.Panel pnSampleText;
        public System.Windows.Forms.Label laSampleText;
        public System.Windows.Forms.Label laSample;
        public System.Windows.Forms.Label laSize;
        public System.Windows.Forms.Label laFont;
        public System.Windows.Forms.ComboBox cbFontName;
        public System.Windows.Forms.Label laDescription;
        public System.Windows.Forms.TextBox tbDescription;
        public System.Windows.Forms.Label laBackColor;
        public System.Windows.Forms.Label laForeColor;
        public System.Windows.Forms.ListBox lbStyles;
        public System.Windows.Forms.GroupBox gbFontAttributes;
        public System.Windows.Forms.CheckBox chbUnderline;
        public System.Windows.Forms.CheckBox chbItalic;
        public System.Windows.Forms.CheckBox chbBold;
        public System.Windows.Forms.Panel pnAdditional;
        public System.Windows.Forms.GroupBox gbTabOptions;
        public System.Windows.Forms.TextBox tbTabStops;
        public System.Windows.Forms.RadioButton rbKeepTabs;
        public System.Windows.Forms.RadioButton rbInsertSpaces;
        public System.Windows.Forms.Label laTabSizes;
        public System.Windows.Forms.GroupBox gbOutlineOptions;
        public System.Windows.Forms.CheckBox chbAllowOutlining;
        public System.Windows.Forms.CheckBox chbShowHints;
        public System.Windows.Forms.GroupBox gbNavigateOptions;
        public System.Windows.Forms.CheckBox chbMoveOnRightButton;
        public System.Windows.Forms.CheckBox chbBeyondEof;
        public System.Windows.Forms.CheckBox chbBeyondEol;
        public Alternet.Editor.Common.ColorBox cbForeColor;
        public Alternet.Editor.Common.ColorBox cbBackColor;
        public System.Windows.Forms.GroupBox gbLineNumbers;
        public System.Windows.Forms.CheckBox chbLineNumbers;
        public System.Windows.Forms.CheckBox chbLineNumbersOnGutter;
        public System.Windows.Forms.TextBox tbGutterWidth;
        public System.Windows.Forms.TextBox tbMarginPosition;
        public System.Windows.Forms.TextBox tbFontSize;
        public System.Windows.Forms.TabPage tpKeyboard;
        public System.Windows.Forms.Panel pnKeyboard;
        public System.Windows.Forms.CheckBox chbLineModificator;
        public System.Windows.Forms.CheckBox chbWhiteSpace;

        private const int OpenFolderImage = 0;
        private const int CloseFolderImage = 1;
        private const int SelectedImage = 2;
        private const int UnSelectedImage = 3;

        private System.Windows.Forms.CheckBox chbForced;
        private System.Windows.Forms.Label laKeyboardMappingScheme;
        private System.Windows.Forms.Button btSaveSchemeAs;
        private System.Windows.Forms.Button btDeleteScheme;
        private System.Windows.Forms.Label laShowCommands;
        private System.Windows.Forms.TextBox tbShowCommands;
        private System.Windows.Forms.ListBox lbEventHandlers;
        private System.Windows.Forms.Label laShortcuts;
        private System.Windows.Forms.ComboBox cbShortcuts;
        private System.Windows.Forms.Button UpdateShortcutButton;
        private System.Windows.Forms.ComboBox cbKeyboardSchemes;
        private System.Windows.Forms.GroupBox gbVisualThemes;
        private System.Windows.Forms.ComboBox cbVisualThemes;
        private System.Windows.Forms.Button btAddVisualTheme;
        private System.Windows.Forms.Button btDeleteVisualTheme;
        private System.Windows.Forms.CheckBox chbLineSeparator;
        private System.ComponentModel.IContainer components;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Fonts and Colors");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Additional");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Keyboard");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Options", 1, 0, new System.Windows.Forms.TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgSyntaxSettings));
            this.pnButtons = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.pnMain = new System.Windows.Forms.Panel();
            this.pnManage = new System.Windows.Forms.Panel();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.pnGeneral = new System.Windows.Forms.Panel();
            this.gbLineNumbers = new System.Windows.Forms.GroupBox();
            this.chbLineNumbers = new System.Windows.Forms.CheckBox();
            this.chbLineNumbersOnGutter = new System.Windows.Forms.CheckBox();
            this.gbGutterMargin = new System.Windows.Forms.GroupBox();
            this.tbMarginPosition = new System.Windows.Forms.TextBox();
            this.tbGutterWidth = new System.Windows.Forms.TextBox();
            this.chbShowMargin = new System.Windows.Forms.CheckBox();
            this.laGutterWidth = new System.Windows.Forms.Label();
            this.chbShowGutter = new System.Windows.Forms.CheckBox();
            this.laMarginPosition = new System.Windows.Forms.Label();
            this.gbDocument = new System.Windows.Forms.GroupBox();
            this.chbLineSeparator = new System.Windows.Forms.CheckBox();
            this.chbLineModificator = new System.Windows.Forms.CheckBox();
            this.chbWhiteSpace = new System.Windows.Forms.CheckBox();
            this.chbForced = new System.Windows.Forms.CheckBox();
            this.chbDragAndDrop = new System.Windows.Forms.CheckBox();
            this.chbHorzScrollBar = new System.Windows.Forms.CheckBox();
            this.chbVertScrollBar = new System.Windows.Forms.CheckBox();
            this.chbWordWrap = new System.Windows.Forms.CheckBox();
            this.chbHighlightUrls = new System.Windows.Forms.CheckBox();
            this.tpAdditional = new System.Windows.Forms.TabPage();
            this.pnAdditional = new System.Windows.Forms.Panel();
            this.gbTabOptions = new System.Windows.Forms.GroupBox();
            this.tbTabStops = new System.Windows.Forms.TextBox();
            this.rbKeepTabs = new System.Windows.Forms.RadioButton();
            this.rbInsertSpaces = new System.Windows.Forms.RadioButton();
            this.laTabSizes = new System.Windows.Forms.Label();
            this.gbOutlineOptions = new System.Windows.Forms.GroupBox();
            this.chbAllowOutlining = new System.Windows.Forms.CheckBox();
            this.chbShowHints = new System.Windows.Forms.CheckBox();
            this.gbNavigateOptions = new System.Windows.Forms.GroupBox();
            this.chbMoveOnRightButton = new System.Windows.Forms.CheckBox();
            this.chbBeyondEof = new System.Windows.Forms.CheckBox();
            this.chbBeyondEol = new System.Windows.Forms.CheckBox();
            this.tpFontsAndColors = new System.Windows.Forms.TabPage();
            this.pnFontsColors = new System.Windows.Forms.Panel();
            this.cbForeColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbBackColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.gbVisualThemes = new System.Windows.Forms.GroupBox();
            this.btDeleteVisualTheme = new System.Windows.Forms.Button();
            this.btAddVisualTheme = new System.Windows.Forms.Button();
            this.cbVisualThemes = new System.Windows.Forms.ComboBox();
            this.tbFontSize = new System.Windows.Forms.TextBox();
            this.laDisplayItems = new System.Windows.Forms.Label();
            this.pnSampleText = new System.Windows.Forms.Panel();
            this.laSampleText = new System.Windows.Forms.Label();
            this.laSample = new System.Windows.Forms.Label();
            this.laSize = new System.Windows.Forms.Label();
            this.laFont = new System.Windows.Forms.Label();
            this.cbFontName = new System.Windows.Forms.ComboBox();
            this.laDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.laBackColor = new System.Windows.Forms.Label();
            this.laForeColor = new System.Windows.Forms.Label();
            this.lbStyles = new System.Windows.Forms.ListBox();
            this.gbFontAttributes = new System.Windows.Forms.GroupBox();
            this.chbUnderline = new System.Windows.Forms.CheckBox();
            this.chbItalic = new System.Windows.Forms.CheckBox();
            this.chbBold = new System.Windows.Forms.CheckBox();
            this.tpKeyboard = new System.Windows.Forms.TabPage();
            this.pnKeyboard = new System.Windows.Forms.Panel();
            this.UpdateShortcutButton = new System.Windows.Forms.Button();
            this.cbShortcuts = new System.Windows.Forms.ComboBox();
            this.laShortcuts = new System.Windows.Forms.Label();
            this.lbEventHandlers = new System.Windows.Forms.ListBox();
            this.tbShowCommands = new System.Windows.Forms.TextBox();
            this.laShowCommands = new System.Windows.Forms.Label();
            this.btDeleteScheme = new System.Windows.Forms.Button();
            this.btSaveSchemeAs = new System.Windows.Forms.Button();
            this.cbKeyboardSchemes = new System.Windows.Forms.ComboBox();
            this.laKeyboardMappingScheme = new System.Windows.Forms.Label();
            this.pnTree = new System.Windows.Forms.Panel();
            this.tvProperties = new System.Windows.Forms.TreeView();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pnButtons.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.pnManage.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.pnGeneral.SuspendLayout();
            this.gbLineNumbers.SuspendLayout();
            this.gbGutterMargin.SuspendLayout();
            this.gbDocument.SuspendLayout();
            this.tpAdditional.SuspendLayout();
            this.pnAdditional.SuspendLayout();
            this.gbTabOptions.SuspendLayout();
            this.gbOutlineOptions.SuspendLayout();
            this.gbNavigateOptions.SuspendLayout();
            this.tpFontsAndColors.SuspendLayout();
            this.pnFontsColors.SuspendLayout();
            this.gbVisualThemes.SuspendLayout();
            this.pnSampleText.SuspendLayout();
            this.gbFontAttributes.SuspendLayout();
            this.tpKeyboard.SuspendLayout();
            this.pnKeyboard.SuspendLayout();
            this.pnTree.SuspendLayout();
            this.SuspendLayout();

            // pnButtons
            this.pnButtons.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            this.pnButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnButtons.Controls.Add(this.btCancel);
            this.pnButtons.Controls.Add(this.btOK);
            this.pnButtons.Location = new System.Drawing.Point(0, 360);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new System.Drawing.Size(556, 40);
            this.pnButtons.TabIndex = 6;

            // btCancel
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(463, 5);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";

            // btOK
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(383, 5);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.Click += new System.EventHandler(this.OKButton_Click);

            // pnMain
            this.pnMain.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            this.pnMain.Controls.Add(this.pnManage);
            this.pnMain.Controls.Add(this.pnTree);
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(552, 360);
            this.pnMain.TabIndex = 7;

            // pnManage
            this.pnManage.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            this.pnManage.Controls.Add(this.tcMain);
            this.pnManage.Location = new System.Drawing.Point(136, 0);
            this.pnManage.Name = "pnManage";
            this.pnManage.Size = new System.Drawing.Size(416, 360);
            this.pnManage.TabIndex = 1;

            // tcMain
            this.tcMain.Controls.Add(this.tpGeneral);
            this.tcMain.Controls.Add(this.tpAdditional);
            this.tcMain.Controls.Add(this.tpFontsAndColors);
            this.tcMain.Controls.Add(this.tpKeyboard);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(416, 360);
            this.tcMain.TabIndex = 0;
            this.tcMain.Visible = false;

            // tpGeneral
            this.tpGeneral.Controls.Add(this.pnGeneral);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new System.Drawing.Size(408, 334);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";

            // pnGeneral
            this.pnGeneral.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            this.pnGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.pnGeneral.Controls.Add(this.gbLineNumbers);
            this.pnGeneral.Controls.Add(this.gbGutterMargin);
            this.pnGeneral.Controls.Add(this.gbDocument);
            this.pnGeneral.Location = new System.Drawing.Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new System.Drawing.Size(424, 368);
            this.pnGeneral.TabIndex = 1;
            this.pnGeneral.Visible = false;

            // gbLineNumbers
            this.gbLineNumbers.Controls.Add(this.chbLineNumbers);
            this.gbLineNumbers.Controls.Add(this.chbLineNumbersOnGutter);
            this.gbLineNumbers.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbLineNumbers.Location = new System.Drawing.Point(8, 200);
            this.gbLineNumbers.Name = "gbLineNumbers";
            this.gbLineNumbers.Size = new System.Drawing.Size(384, 72);
            this.gbLineNumbers.TabIndex = 39;
            this.gbLineNumbers.TabStop = false;
            this.gbLineNumbers.Text = "Line Numbers";

            // chbLineNumbers
            this.chbLineNumbers.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbLineNumbers.Location = new System.Drawing.Point(8, 16);
            this.chbLineNumbers.Name = "chbLineNumbers";
            this.chbLineNumbers.Size = new System.Drawing.Size(200, 25);
            this.chbLineNumbers.TabIndex = 0;
            this.chbLineNumbers.Text = "Show Line Numbers";

            // chbLineNumbersOnGutter
            this.chbLineNumbersOnGutter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbLineNumbersOnGutter.Location = new System.Drawing.Point(8, 41);
            this.chbLineNumbersOnGutter.Name = "chbLineNumbersOnGutter";
            this.chbLineNumbersOnGutter.Size = new System.Drawing.Size(200, 25);
            this.chbLineNumbersOnGutter.TabIndex = 1;
            this.chbLineNumbersOnGutter.Text = "Display on Gutter";

            // gbGutterMargin
            this.gbGutterMargin.Controls.Add(this.tbMarginPosition);
            this.gbGutterMargin.Controls.Add(this.tbGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowMargin);
            this.gbGutterMargin.Controls.Add(this.laGutterWidth);
            this.gbGutterMargin.Controls.Add(this.chbShowGutter);
            this.gbGutterMargin.Controls.Add(this.laMarginPosition);
            this.gbGutterMargin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbGutterMargin.Location = new System.Drawing.Point(8, 116);
            this.gbGutterMargin.Name = "gbGutterMargin";
            this.gbGutterMargin.Size = new System.Drawing.Size(384, 72);
            this.gbGutterMargin.TabIndex = 38;
            this.gbGutterMargin.TabStop = false;
            this.gbGutterMargin.Text = "Gutter&&Margin";

            // tbMarginPosition
            this.tbMarginPosition.Location = new System.Drawing.Point(324, 40);
            this.tbMarginPosition.Name = "tbMarginPosition";
            this.tbMarginPosition.Size = new System.Drawing.Size(48, 20);
            this.tbMarginPosition.TabIndex = 8;
            this.tbMarginPosition.Text = "0";
            this.tbMarginPosition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MarginPositionTextBox_KeyPress);

            // tbGutterWidth
            this.tbGutterWidth.Location = new System.Drawing.Point(324, 16);
            this.tbGutterWidth.Name = "tbGutterWidth";
            this.tbGutterWidth.Size = new System.Drawing.Size(48, 20);
            this.tbGutterWidth.TabIndex = 7;
            this.tbGutterWidth.Text = "0";
            this.tbGutterWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GutterWidthTextBox_KeyPress);

            // chbShowMargin
            this.chbShowMargin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbShowMargin.Location = new System.Drawing.Point(8, 41);
            this.chbShowMargin.Name = "chbShowMargin";
            this.chbShowMargin.Size = new System.Drawing.Size(136, 25);
            this.chbShowMargin.TabIndex = 1;
            this.chbShowMargin.Text = "Show Margin";

            // laGutterWidth
            this.laGutterWidth.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laGutterWidth.Location = new System.Drawing.Point(160, 16);
            this.laGutterWidth.Name = "laGutterWidth";
            this.laGutterWidth.Size = new System.Drawing.Size(152, 25);
            this.laGutterWidth.TabIndex = 3;
            this.laGutterWidth.Text = "Gutter width:";

            // chbShowGutter
            this.chbShowGutter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbShowGutter.Location = new System.Drawing.Point(8, 16);
            this.chbShowGutter.Name = "chbShowGutter";
            this.chbShowGutter.Size = new System.Drawing.Size(136, 25);
            this.chbShowGutter.TabIndex = 0;
            this.chbShowGutter.Text = "Show Gutter";

            // laMarginPosition
            this.laMarginPosition.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laMarginPosition.Location = new System.Drawing.Point(160, 41);
            this.laMarginPosition.Name = "laMarginPosition";
            this.laMarginPosition.Size = new System.Drawing.Size(152, 25);
            this.laMarginPosition.TabIndex = 4;
            this.laMarginPosition.Text = "Margin position:";

            // gbDocument
            this.gbDocument.Controls.Add(this.chbLineSeparator);
            this.gbDocument.Controls.Add(this.chbLineModificator);
            this.gbDocument.Controls.Add(this.chbWhiteSpace);
            this.gbDocument.Controls.Add(this.chbForced);
            this.gbDocument.Controls.Add(this.chbDragAndDrop);
            this.gbDocument.Controls.Add(this.chbHorzScrollBar);
            this.gbDocument.Controls.Add(this.chbVertScrollBar);
            this.gbDocument.Controls.Add(this.chbWordWrap);
            this.gbDocument.Controls.Add(this.chbHighlightUrls);
            this.gbDocument.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbDocument.Location = new System.Drawing.Point(8, 8);
            this.gbDocument.Name = "gbDocument";
            this.gbDocument.Size = new System.Drawing.Size(384, 98);
            this.gbDocument.TabIndex = 11;
            this.gbDocument.TabStop = false;
            this.gbDocument.Text = "Document";

            // chbLineSeparator
            this.chbLineSeparator.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbLineSeparator.Location = new System.Drawing.Point(250, 66);
            this.chbLineSeparator.Name = "chbLineSeparator";
            this.chbLineSeparator.Size = new System.Drawing.Size(122, 25);
            this.chbLineSeparator.TabIndex = 8;
            this.chbLineSeparator.Text = "Line separator";

            // chbLineModificator
            this.chbLineModificator.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbLineModificator.Location = new System.Drawing.Point(250, 41);
            this.chbLineModificator.Name = "chbLineModificator";
            this.chbLineModificator.Size = new System.Drawing.Size(122, 25);
            this.chbLineModificator.TabIndex = 7;
            this.chbLineModificator.Text = "Line modificator";

            // chbWhiteSpace
            this.chbWhiteSpace.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbWhiteSpace.Location = new System.Drawing.Point(250, 16);
            this.chbWhiteSpace.Name = "chbWhiteSpace";
            this.chbWhiteSpace.Size = new System.Drawing.Size(122, 25);
            this.chbWhiteSpace.TabIndex = 6;
            this.chbWhiteSpace.Text = "White space";

            // chbForced
            this.chbForced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbForced.Location = new System.Drawing.Point(130, 66);
            this.chbForced.Name = "chbForced";
            this.chbForced.Size = new System.Drawing.Size(122, 25);
            this.chbForced.TabIndex = 5;
            this.chbForced.Text = "Forced scroll bars";

            // chbDragAndDrop
            this.chbDragAndDrop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbDragAndDrop.Location = new System.Drawing.Point(8, 66);
            this.chbDragAndDrop.Name = "chbDragAndDrop";
            this.chbDragAndDrop.Size = new System.Drawing.Size(144, 25);
            this.chbDragAndDrop.TabIndex = 2;
            this.chbDragAndDrop.Text = "&Drag and drop text";

            // chbHorzScrollBar
            this.chbHorzScrollBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbHorzScrollBar.Location = new System.Drawing.Point(130, 41);
            this.chbHorzScrollBar.Name = "chbHorzScrollBar";
            this.chbHorzScrollBar.Size = new System.Drawing.Size(122, 25);
            this.chbHorzScrollBar.TabIndex = 4;
            this.chbHorzScrollBar.Text = "&Horizontal scroll bar";

            // chbVertScrollBar
            this.chbVertScrollBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbVertScrollBar.Location = new System.Drawing.Point(130, 16);
            this.chbVertScrollBar.Name = "chbVertScrollBar";
            this.chbVertScrollBar.Size = new System.Drawing.Size(122, 25);
            this.chbVertScrollBar.TabIndex = 3;
            this.chbVertScrollBar.Text = "&Vertical scroll bar";

            // chbWordWrap
            this.chbWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbWordWrap.Location = new System.Drawing.Point(8, 16);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new System.Drawing.Size(124, 25);
            this.chbWordWrap.TabIndex = 0;
            this.chbWordWrap.Text = "Word Wrap";

            // chbHighlightUrls
            this.chbHighlightUrls.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbHighlightUrls.Location = new System.Drawing.Point(8, 41);
            this.chbHighlightUrls.Name = "chbHighlightUrls";
            this.chbHighlightUrls.Size = new System.Drawing.Size(128, 25);
            this.chbHighlightUrls.TabIndex = 1;
            this.chbHighlightUrls.Text = "Highlight Urls";

            // tpAdditional
            this.tpAdditional.Controls.Add(this.pnAdditional);
            this.tpAdditional.Location = new System.Drawing.Point(4, 22);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Size = new System.Drawing.Size(408, 334);
            this.tpAdditional.TabIndex = 2;
            this.tpAdditional.Text = "Additional";

            // pnAdditional
            this.pnAdditional.Controls.Add(this.gbTabOptions);
            this.pnAdditional.Controls.Add(this.gbOutlineOptions);
            this.pnAdditional.Controls.Add(this.gbNavigateOptions);
            this.pnAdditional.Location = new System.Drawing.Point(0, 0);
            this.pnAdditional.Name = "pnAdditional";
            this.pnAdditional.Size = new System.Drawing.Size(400, 296);
            this.pnAdditional.TabIndex = 8;
            this.pnAdditional.Visible = false;

            // gbTabOptions
            this.gbTabOptions.Controls.Add(this.tbTabStops);
            this.gbTabOptions.Controls.Add(this.rbKeepTabs);
            this.gbTabOptions.Controls.Add(this.rbInsertSpaces);
            this.gbTabOptions.Controls.Add(this.laTabSizes);
            this.gbTabOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbTabOptions.Location = new System.Drawing.Point(8, 200);
            this.gbTabOptions.Name = "gbTabOptions";
            this.gbTabOptions.Size = new System.Drawing.Size(384, 72);
            this.gbTabOptions.TabIndex = 40;
            this.gbTabOptions.TabStop = false;
            this.gbTabOptions.Text = "Tab Options";

            // tbTabStops
            this.tbTabStops.Location = new System.Drawing.Point(8, 40);
            this.tbTabStops.Name = "tbTabStops";
            this.tbTabStops.Size = new System.Drawing.Size(120, 20);
            this.tbTabStops.TabIndex = 5;

            // rbKeepTabs
            this.rbKeepTabs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbKeepTabs.Location = new System.Drawing.Point(160, 41);
            this.rbKeepTabs.Name = "rbKeepTabs";
            this.rbKeepTabs.Size = new System.Drawing.Size(184, 25);
            this.rbKeepTabs.TabIndex = 4;
            this.rbKeepTabs.Text = "&Keep tabs";

            // rbInsertSpaces
            this.rbInsertSpaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbInsertSpaces.Location = new System.Drawing.Point(160, 16);
            this.rbInsertSpaces.Name = "rbInsertSpaces";
            this.rbInsertSpaces.Size = new System.Drawing.Size(184, 25);
            this.rbInsertSpaces.TabIndex = 3;
            this.rbInsertSpaces.Text = "Insert s&paces";

            // laTabSizes
            this.laTabSizes.AutoSize = true;
            this.laTabSizes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laTabSizes.Location = new System.Drawing.Point(8, 21);
            this.laTabSizes.Name = "laTabSizes";
            this.laTabSizes.Size = new System.Drawing.Size(57, 13);
            this.laTabSizes.TabIndex = 0;
            this.laTabSizes.Text = "Tab Sizes:";

            // gbOutlineOptions
            this.gbOutlineOptions.Controls.Add(this.chbAllowOutlining);
            this.gbOutlineOptions.Controls.Add(this.chbShowHints);
            this.gbOutlineOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbOutlineOptions.Location = new System.Drawing.Point(8, 116);
            this.gbOutlineOptions.Name = "gbOutlineOptions";
            this.gbOutlineOptions.Size = new System.Drawing.Size(384, 72);
            this.gbOutlineOptions.TabIndex = 39;
            this.gbOutlineOptions.TabStop = false;
            this.gbOutlineOptions.Text = "Outline Options";

            // chbAllowOutlining
            this.chbAllowOutlining.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbAllowOutlining.Location = new System.Drawing.Point(8, 16);
            this.chbAllowOutlining.Name = "chbAllowOutlining";
            this.chbAllowOutlining.Size = new System.Drawing.Size(336, 25);
            this.chbAllowOutlining.TabIndex = 0;
            this.chbAllowOutlining.Text = "Allow outlining";

            // chbShowHints
            this.chbShowHints.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbShowHints.Location = new System.Drawing.Point(8, 41);
            this.chbShowHints.Name = "chbShowHints";
            this.chbShowHints.Size = new System.Drawing.Size(336, 25);
            this.chbShowHints.TabIndex = 1;
            this.chbShowHints.Text = "Show Hints";

            // gbNavigateOptions
            this.gbNavigateOptions.Controls.Add(this.chbMoveOnRightButton);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEof);
            this.gbNavigateOptions.Controls.Add(this.chbBeyondEol);
            this.gbNavigateOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbNavigateOptions.Location = new System.Drawing.Point(8, 8);
            this.gbNavigateOptions.Name = "gbNavigateOptions";
            this.gbNavigateOptions.Size = new System.Drawing.Size(384, 98);
            this.gbNavigateOptions.TabIndex = 38;
            this.gbNavigateOptions.TabStop = false;
            this.gbNavigateOptions.Text = "Navigate Options";

            // chbMoveOnRightButton
            this.chbMoveOnRightButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbMoveOnRightButton.Location = new System.Drawing.Point(8, 66);
            this.chbMoveOnRightButton.Name = "chbMoveOnRightButton";
            this.chbMoveOnRightButton.Size = new System.Drawing.Size(336, 25);
            this.chbMoveOnRightButton.TabIndex = 14;
            this.chbMoveOnRightButton.Text = "Move on Right Button";

            // chbBeyondEof
            this.chbBeyondEof.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbBeyondEof.Location = new System.Drawing.Point(8, 41);
            this.chbBeyondEof.Name = "chbBeyondEof";
            this.chbBeyondEof.Size = new System.Drawing.Size(336, 25);
            this.chbBeyondEof.TabIndex = 13;
            this.chbBeyondEof.Text = "Beyond Eof";

            // chbBeyondEol
            this.chbBeyondEol.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbBeyondEol.Location = new System.Drawing.Point(8, 16);
            this.chbBeyondEol.Name = "chbBeyondEol";
            this.chbBeyondEol.Size = new System.Drawing.Size(336, 25);
            this.chbBeyondEol.TabIndex = 12;
            this.chbBeyondEol.Text = "Beyond Eol";

            // tpFontsAndColors
            this.tpFontsAndColors.Controls.Add(this.pnFontsColors);
            this.tpFontsAndColors.Location = new System.Drawing.Point(4, 22);
            this.tpFontsAndColors.Name = "tpFontsAndColors";
            this.tpFontsAndColors.Size = new System.Drawing.Size(408, 334);
            this.tpFontsAndColors.TabIndex = 1;
            this.tpFontsAndColors.Text = "Fonts&&Colors";

            // pnFontsColors
            this.pnFontsColors.Controls.Add(this.cbForeColor);
            this.pnFontsColors.Controls.Add(this.cbBackColor);
            this.pnFontsColors.Controls.Add(this.gbVisualThemes);
            this.pnFontsColors.Controls.Add(this.tbFontSize);
            this.pnFontsColors.Controls.Add(this.laDisplayItems);
            this.pnFontsColors.Controls.Add(this.pnSampleText);
            this.pnFontsColors.Controls.Add(this.laSample);
            this.pnFontsColors.Controls.Add(this.laSize);
            this.pnFontsColors.Controls.Add(this.laFont);
            this.pnFontsColors.Controls.Add(this.cbFontName);
            this.pnFontsColors.Controls.Add(this.laDescription);
            this.pnFontsColors.Controls.Add(this.tbDescription);
            this.pnFontsColors.Controls.Add(this.laBackColor);
            this.pnFontsColors.Controls.Add(this.laForeColor);
            this.pnFontsColors.Controls.Add(this.lbStyles);
            this.pnFontsColors.Controls.Add(this.gbFontAttributes);
            this.pnFontsColors.Location = new System.Drawing.Point(0, 0);
            this.pnFontsColors.Name = "pnFontsColors";
            this.pnFontsColors.Size = new System.Drawing.Size(400, 328);
            this.pnFontsColors.TabIndex = 7;
            this.pnFontsColors.Visible = false;

            // cbForeColor
            this.cbForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbForeColor.Location = new System.Drawing.Point(152, 216);
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbForeColor.Size = new System.Drawing.Size(120, 21);
            this.cbForeColor.TabIndex = 17;
            this.cbForeColor.SelectedIndexChanged += new System.EventHandler(this.ForeColorComboBox_SelectedIndexChanged);

            // cbBackColor
            this.cbBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbBackColor.Location = new System.Drawing.Point(152, 264);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbBackColor.Size = new System.Drawing.Size(120, 21);
            this.cbBackColor.TabIndex = 18;
            this.cbBackColor.SelectedIndexChanged += new System.EventHandler(this.BackColorComboBox_SelectedIndexChanged);

            // gbVisualThemes
            this.gbVisualThemes.Controls.Add(this.btDeleteVisualTheme);
            this.gbVisualThemes.Controls.Add(this.btAddVisualTheme);
            this.gbVisualThemes.Controls.Add(this.cbVisualThemes);
            this.gbVisualThemes.Location = new System.Drawing.Point(8, 8);
            this.gbVisualThemes.Name = "gbVisualThemes";
            this.gbVisualThemes.Size = new System.Drawing.Size(392, 85);
            this.gbVisualThemes.TabIndex = 20;
            this.gbVisualThemes.TabStop = false;
            this.gbVisualThemes.Text = "Visual Themes";

            // btDeleteVisualTheme
            this.btDeleteVisualTheme.Location = new System.Drawing.Point(237, 56);
            this.btDeleteVisualTheme.Name = "btDeleteVisualTheme";
            this.btDeleteVisualTheme.Size = new System.Drawing.Size(147, 23);
            this.btDeleteVisualTheme.TabIndex = 2;
            this.btDeleteVisualTheme.Text = "&Delete Visual Theme";
            this.btDeleteVisualTheme.Click += new System.EventHandler(this.DeleteVisualThemeButton_Click);

            // btAddVisualTheme
            this.btAddVisualTheme.Location = new System.Drawing.Point(237, 17);
            this.btAddVisualTheme.Name = "btAddVisualTheme";
            this.btAddVisualTheme.Size = new System.Drawing.Size(147, 23);
            this.btAddVisualTheme.TabIndex = 1;
            this.btAddVisualTheme.Text = "&Add Visual Theme";
            this.btAddVisualTheme.Click += new System.EventHandler(this.AddVisualThemeButton_Click);

            // cbVisualThemes
            this.cbVisualThemes.Location = new System.Drawing.Point(10, 19);
            this.cbVisualThemes.Name = "cbVisualThemes";
            this.cbVisualThemes.Size = new System.Drawing.Size(215, 21);
            this.cbVisualThemes.TabIndex = 0;
            this.cbVisualThemes.SelectedIndexChanged += new System.EventHandler(this.VisualThemesComboBox_SelectedIndexChanged);
            this.cbVisualThemes.Leave += new System.EventHandler(this.VisualThemesComboBox_Leave);

            // tbFontSize
            this.tbFontSize.Location = new System.Drawing.Point(152, 112);
            this.tbFontSize.Name = "tbFontSize";
            this.tbFontSize.Size = new System.Drawing.Size(32, 20);
            this.tbFontSize.TabIndex = 19;
            this.tbFontSize.Text = "1";
            this.tbFontSize.Leave += new System.EventHandler(this.FontSizeTextBox_Leave);

            // laDisplayItems
            this.laDisplayItems.AutoSize = true;
            this.laDisplayItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laDisplayItems.Location = new System.Drawing.Point(8, 144);
            this.laDisplayItems.Name = "laDisplayItems";
            this.laDisplayItems.Size = new System.Drawing.Size(71, 13);
            this.laDisplayItems.TabIndex = 4;
            this.laDisplayItems.Text = "&Display items:";

            // pnSampleText
            this.pnSampleText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnSampleText.Controls.Add(this.laSampleText);
            this.pnSampleText.Location = new System.Drawing.Point(8, 305);
            this.pnSampleText.Name = "pnSampleText";
            this.pnSampleText.Size = new System.Drawing.Size(384, 48);
            this.pnSampleText.TabIndex = 14;

            // laSampleText
            this.laSampleText.AutoSize = true;
            this.laSampleText.Location = new System.Drawing.Point(160, 16);
            this.laSampleText.Name = "laSampleText";
            this.laSampleText.Size = new System.Drawing.Size(57, 13);
            this.laSampleText.TabIndex = 0;
            this.laSampleText.Text = "AaBbYyZz";

            // laSample
            this.laSample.AutoSize = true;
            this.laSample.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laSample.Location = new System.Drawing.Point(8, 289);
            this.laSample.Name = "laSample";
            this.laSample.Size = new System.Drawing.Size(45, 13);
            this.laSample.TabIndex = 13;
            this.laSample.Text = "Sample:";

            // laSize
            this.laSize.AutoSize = true;
            this.laSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laSize.Location = new System.Drawing.Point(152, 96);
            this.laSize.Name = "laSize";
            this.laSize.Size = new System.Drawing.Size(30, 13);
            this.laSize.TabIndex = 2;
            this.laSize.Text = "&Size:";

            // laFont
            this.laFont.AutoSize = true;
            this.laFont.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laFont.Location = new System.Drawing.Point(8, 96);
            this.laFont.Name = "laFont";
            this.laFont.Size = new System.Drawing.Size(31, 13);
            this.laFont.TabIndex = 0;
            this.laFont.Text = "Font:";

            // cbFontName
            this.cbFontName.Location = new System.Drawing.Point(8, 112);
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new System.Drawing.Size(121, 21);
            this.cbFontName.TabIndex = 1;
            this.cbFontName.SelectedIndexChanged += new System.EventHandler(this.FontNameComboBox_SelectedIndexChanged);

            // laDescription
            this.laDescription.AutoSize = true;
            this.laDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laDescription.Location = new System.Drawing.Point(152, 144);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(63, 13);
            this.laDescription.TabIndex = 6;
            this.laDescription.Text = "Description:";

            // tbDescription
            this.tbDescription.Location = new System.Drawing.Point(152, 160);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(240, 20);
            this.tbDescription.TabIndex = 7;

            // laBackColor
            this.laBackColor.AutoSize = true;
            this.laBackColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laBackColor.Location = new System.Drawing.Point(152, 240);
            this.laBackColor.Name = "laBackColor";
            this.laBackColor.Size = new System.Drawing.Size(62, 13);
            this.laBackColor.TabIndex = 10;
            this.laBackColor.Text = "Back Color:";

            // laForeColor
            this.laForeColor.AutoSize = true;
            this.laForeColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.laForeColor.Location = new System.Drawing.Point(152, 192);
            this.laForeColor.Name = "laForeColor";
            this.laForeColor.Size = new System.Drawing.Size(58, 13);
            this.laForeColor.TabIndex = 8;
            this.laForeColor.Text = "Fore Color:";

            // lbStyles
            this.lbStyles.Location = new System.Drawing.Point(8, 160);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new System.Drawing.Size(120, 121);
            this.lbStyles.TabIndex = 5;

            // gbFontAttributes
            this.gbFontAttributes.Controls.Add(this.chbUnderline);
            this.gbFontAttributes.Controls.Add(this.chbItalic);
            this.gbFontAttributes.Controls.Add(this.chbBold);
            this.gbFontAttributes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbFontAttributes.Location = new System.Drawing.Point(280, 192);
            this.gbFontAttributes.Name = "gbFontAttributes";
            this.gbFontAttributes.Size = new System.Drawing.Size(112, 92);
            this.gbFontAttributes.TabIndex = 12;
            this.gbFontAttributes.TabStop = false;
            this.gbFontAttributes.Text = "Attributes:";

            // chbUnderline
            this.chbUnderline.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbUnderline.Location = new System.Drawing.Point(8, 64);
            this.chbUnderline.Name = "chbUnderline";
            this.chbUnderline.Size = new System.Drawing.Size(100, 24);
            this.chbUnderline.TabIndex = 2;
            this.chbUnderline.Text = "Underline";

            // chbItalic
            this.chbItalic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbItalic.Location = new System.Drawing.Point(8, 40);
            this.chbItalic.Name = "chbItalic";
            this.chbItalic.Size = new System.Drawing.Size(100, 24);
            this.chbItalic.TabIndex = 1;
            this.chbItalic.Text = "Italic";

            // chbBold
            this.chbBold.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chbBold.Location = new System.Drawing.Point(8, 16);
            this.chbBold.Name = "chbBold";
            this.chbBold.Size = new System.Drawing.Size(100, 24);
            this.chbBold.TabIndex = 0;
            this.chbBold.Text = "Bold";

            // tpKeyboard
            this.tpKeyboard.Controls.Add(this.pnKeyboard);
            this.tpKeyboard.Location = new System.Drawing.Point(4, 22);
            this.tpKeyboard.Name = "tpKeyboard";
            this.tpKeyboard.Size = new System.Drawing.Size(408, 334);
            this.tpKeyboard.TabIndex = 3;
            this.tpKeyboard.Text = "Keyboard";

            // pnKeyboard
            this.pnKeyboard.Controls.Add(this.cbShortcuts);
            this.pnKeyboard.Controls.Add(this.UpdateShortcutButton);
            this.pnKeyboard.Controls.Add(this.laShortcuts);
            this.pnKeyboard.Controls.Add(this.lbEventHandlers);
            this.pnKeyboard.Controls.Add(this.tbShowCommands);
            this.pnKeyboard.Controls.Add(this.laShowCommands);
            this.pnKeyboard.Controls.Add(this.btDeleteScheme);
            this.pnKeyboard.Controls.Add(this.btSaveSchemeAs);
            this.pnKeyboard.Controls.Add(this.cbKeyboardSchemes);
            this.pnKeyboard.Controls.Add(this.laKeyboardMappingScheme);
            this.pnKeyboard.Location = new System.Drawing.Point(0, 0);
            this.pnKeyboard.Name = "pnKeyboard";
            this.pnKeyboard.Size = new System.Drawing.Size(400, 331);
            this.pnKeyboard.TabIndex = 0;
            this.pnKeyboard.Visible = false;

            // cbShortcuts
            this.cbShortcuts.Location = new System.Drawing.Point(8, 291);
            this.cbShortcuts.Name = "cbShortcuts";
            this.cbShortcuts.Size = new System.Drawing.Size(304, 21);
            this.cbShortcuts.TabIndex = 8;
            this.cbShortcuts.SelectedIndexChanged += CbShortcuts_SelectedIndexChanged;

            // UpdateShortcutButton
            this.UpdateShortcutButton.Location = new System.Drawing.Point(8, 319);
            this.UpdateShortcutButton.Name = "UpdateShortcutButton";
            this.UpdateShortcutButton.Size = new System.Drawing.Size(100, 21);
            this.UpdateShortcutButton.Text = "Update shortcut";
            this.UpdateShortcutButton.TabIndex = 9;
            this.UpdateShortcutButton.Click += UpdateShortcutButton_Click;

            // laShortcuts
            this.laShortcuts.AutoSize = true;
            this.laShortcuts.Location = new System.Drawing.Point(8, 275);
            this.laShortcuts.Name = "laShortcuts";
            this.laShortcuts.Size = new System.Drawing.Size(168, 13);
            this.laShortcuts.TabIndex = 7;
            this.laShortcuts.Text = "Shor&tcut(s) for selected command:";

            // lbEventHandlers
            this.lbEventHandlers.Location = new System.Drawing.Point(8, 88);
            this.lbEventHandlers.Name = "lbEventHandlers";
            this.lbEventHandlers.Size = new System.Drawing.Size(384, 173);
            this.lbEventHandlers.TabIndex = 6;
            this.lbEventHandlers.SelectedIndexChanged += new System.EventHandler(this.EventHandlersLisoxTextBox_SelectedIndexChanged);

            // tbShowCommands
            this.tbShowCommands.Location = new System.Drawing.Point(8, 64);
            this.tbShowCommands.Name = "tbShowCommands";
            this.tbShowCommands.Size = new System.Drawing.Size(384, 20);
            this.tbShowCommands.TabIndex = 5;
            this.tbShowCommands.TextChanged += new System.EventHandler(this.ShowCommandsTextBox_TextChanged);

            // laShowCommands
            this.laShowCommands.AutoSize = true;
            this.laShowCommands.Location = new System.Drawing.Point(8, 48);
            this.laShowCommands.Name = "laShowCommands";
            this.laShowCommands.Size = new System.Drawing.Size(143, 13);
            this.laShowCommands.TabIndex = 4;
            this.laShowCommands.Text = "Show &commands containing:";

            // btDeleteScheme
            this.btDeleteScheme.Enabled = false;
            this.btDeleteScheme.Location = new System.Drawing.Point(318, 22);
            this.btDeleteScheme.Name = "btDeleteScheme";
            this.btDeleteScheme.Size = new System.Drawing.Size(75, 23);
            this.btDeleteScheme.TabIndex = 3;
            this.btDeleteScheme.Text = "&Delete";
            this.btDeleteScheme.Visible = false;

            // btSaveSchemeAs
            this.btSaveSchemeAs.Enabled = false;
            this.btSaveSchemeAs.Location = new System.Drawing.Point(208, 22);
            this.btSaveSchemeAs.Name = "btSaveSchemeAs";
            this.btSaveSchemeAs.Size = new System.Drawing.Size(104, 23);
            this.btSaveSchemeAs.TabIndex = 2;
            this.btSaveSchemeAs.Text = "&Save As...";
            this.btSaveSchemeAs.Visible = false;

            // cbKeyboardSchemes
            this.cbKeyboardSchemes.Location = new System.Drawing.Point(8, 24);
            this.cbKeyboardSchemes.Name = "cbKeyboardSchemes";
            this.cbKeyboardSchemes.Size = new System.Drawing.Size(192, 21);
            this.cbKeyboardSchemes.TabIndex = 1;

            // laKeyboardMappingScheme
            this.laKeyboardMappingScheme.AutoSize = true;
            this.laKeyboardMappingScheme.Location = new System.Drawing.Point(8, 8);
            this.laKeyboardMappingScheme.Name = "laKeyboardMappingScheme";
            this.laKeyboardMappingScheme.Size = new System.Drawing.Size(138, 13);
            this.laKeyboardMappingScheme.TabIndex = 0;
            this.laKeyboardMappingScheme.Text = "Keyboard &mapping scheme:";

            // pnTree
            this.pnTree.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left);
            this.pnTree.Controls.Add(this.tvProperties);
            this.pnTree.Location = new System.Drawing.Point(0, 0);
            this.pnTree.Name = "pnTree";
            this.pnTree.Size = new System.Drawing.Size(136, 360);
            this.pnTree.TabIndex = 0;

            // tvProperties
            this.tvProperties.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            this.tvProperties.Location = new System.Drawing.Point(0, 0);
            this.tvProperties.Name = "tvProperties";
            treeNode1.Name = string.Empty;
            treeNode1.Text = "General";
            treeNode2.Name = string.Empty;
            treeNode2.Text = "Fonts and Colors";
            treeNode3.Name = string.Empty;
            treeNode3.Text = "Additional";
            treeNode4.Name = string.Empty;
            treeNode4.Text = "Keyboard";
            treeNode5.ImageIndex = 1;
            treeNode5.Name = string.Empty;
            treeNode5.SelectedImageIndex = 0;
            treeNode5.Text = "Options";
            this.tvProperties.Nodes.AddRange(new System.Windows.Forms.TreeNode[]
            {
            treeNode5,
            });
            this.tvProperties.Scrollable = false;
            this.tvProperties.ShowLines = false;
            this.tvProperties.ShowPlusMinus = false;
            this.tvProperties.ShowRootLines = false;
            this.tvProperties.Size = new System.Drawing.Size(136, 360);
            this.tvProperties.TabIndex = 0;
            this.tvProperties.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PropertiesTreeView_AfterSelect);

            // DlgSyntaxSettings
            this.AcceptButton = this.btOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(552, 398);
            this.Controls.Add(this.pnMain);
            this.Controls.Add(this.pnButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSyntaxSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Syntax Settings";
            this.Activated += new System.EventHandler(this.DlgSyntaxSettings_Activated);
            this.Load += new System.EventHandler(this.DlgSyntaxSettings_Load);
            this.pnButtons.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.pnManage.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.pnGeneral.ResumeLayout(false);
            this.gbLineNumbers.ResumeLayout(false);
            this.gbGutterMargin.ResumeLayout(false);
            this.gbGutterMargin.PerformLayout();
            this.gbDocument.ResumeLayout(false);
            this.tpAdditional.ResumeLayout(false);
            this.pnAdditional.ResumeLayout(false);
            this.gbTabOptions.ResumeLayout(false);
            this.gbTabOptions.PerformLayout();
            this.gbOutlineOptions.ResumeLayout(false);
            this.gbNavigateOptions.ResumeLayout(false);
            this.tpFontsAndColors.ResumeLayout(false);
            this.pnFontsColors.ResumeLayout(false);
            this.pnFontsColors.PerformLayout();
            this.gbVisualThemes.ResumeLayout(false);
            this.pnSampleText.ResumeLayout(false);
            this.pnSampleText.PerformLayout();
            this.gbFontAttributes.ResumeLayout(false);
            this.tpKeyboard.ResumeLayout(false);
            this.pnKeyboard.ResumeLayout(false);
            this.pnKeyboard.PerformLayout();
            this.pnTree.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void CbShortcuts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbShortcuts.SelectedIndex >= 0)
            {
                currentKeys = cbShortcuts.SelectedIndex;
            }
        }

        #endregion

        #region Private Fields

#pragma warning disable SA1201 // Elements should appear in the correct order
        private ISyntaxSettings syntaxSettings;
#pragma warning restore SA1201 // Elements should appear in the correct order
        private Color curForeColor;
        private Color curBkColor;
        private FontStyle curFontStyle;
        private string curDesc;
        private bool isControlUpdating;
        private bool isFontControlsUpdating = false;

        private TreeNode rootNode;
        private TreeNode generalNode;
        private TreeNode fontsNode;
        private TreeNode keyboardNode;
        private TreeNode additionalNode;

        private string sringControl = "control";
        private string stringCtrl = "CTRL";
        private string stringAlt = "alt";
        private string stringShift = "shift";
        private int currentKeys = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>DlgSyntaxSettings</c> class with default settings.
        /// </summary>
        public DlgSyntaxSettings()
        {
            InitializeComponent();
            syntaxSettings = CreateSyntaxSettings();
            rootNode = tvProperties.Nodes[0];
            generalNode = rootNode.Nodes[0];
            fontsNode = rootNode.Nodes[1];
            additionalNode = rootNode.Nodes[2];
            keyboardNode = rootNode.Nodes[3];
        }

        /// <summary>
        /// Initializes a new instance of the <c>DlgSyntaxSettings</c> class with specified settings.
        /// </summary>
        /// <param name="hiddenTabs">Specifies tabs not to show in the dialog box.</param>
        public DlgSyntaxSettings(EditorSettingsTab hiddenTabs)
            : this()
        {
            UpdateHiddenTabs(hiddenTabs);
        }

        #endregion

        #region IEditorSettingsDialog Members

        /// <summary>
        /// Gets or sets object that implements <c>ISyntaxSettings</c> for this dialog.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISyntaxSettings SyntaxSettings
        {
            get
            {
                return syntaxSettings;
            }

            set
            {
                syntaxSettings.Assign(value);
            }
        }

        /// <summary>
        /// Initializes and runs a editor settings dialog box.
        /// </summary>
        /// <param name="hiddenTabs">specifies hidden tabs in the syntax settings dialog</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        public DialogResult Execute(EditorSettingsTab hiddenTabs)
        {
            return Execute(hiddenTabs, null);
        }

        /// <summary>
        /// When implemented by a class, initializes and runs a editor settings dialog box.
        /// </summary>
        /// <param name="hiddenTabs">specifies hidden tabs in the syntax settings dialog</param>
        /// <param name="owner">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; otherwise, DialogResult.Cancel.</returns>
        public DialogResult Execute(EditorSettingsTab hiddenTabs, IWin32Window owner)
        {
            UpdateHiddenTabs(hiddenTabs);
            return (owner != null) ? ShowDialog(owner) : ShowDialog();
        }

        #endregion

        #region Protected Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected virtual ISyntaxSettings CreateSyntaxSettings()
        {
            return new SyntaxSettings();
        }

        #endregion

        #region Private Methods

        private void UpdateHiddenTabs(EditorSettingsTab hiddenTabs)
        {
            if ((hiddenTabs & EditorSettingsTab.General) != 0)
                rootNode.Nodes.Remove(generalNode);
            if ((hiddenTabs & EditorSettingsTab.FontsAndColors) != 0)
                rootNode.Nodes.Remove(fontsNode);
            if ((hiddenTabs & EditorSettingsTab.Additional) != 0)
                rootNode.Nodes.Remove(additionalNode);
            if ((hiddenTabs & EditorSettingsTab.Keymapping) != 0)
                rootNode.Nodes.Remove(keyboardNode);
        }

        private string[] GetFonts()
        {
            FontFamily[] fonts = FontFamily.Families;
            string[] result = new string[fonts.Length];
            for (int i = 0; i < fonts.Length; i++)
                result[i] = fonts[i].Name;
            return result;
        }

        private int GetInt(string s, int defaultValue)
        {
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return defaultValue;
            }
        }

        private void FillEventHandlers()
        {
            UpdateEventHandlers();
        }

        private void UpdateEventHandlers()
        {
            lbEventHandlers.BeginUpdate();
            try
            {
                lbEventHandlers.Sorted = false;
                lbEventHandlers.Items.Clear();
                foreach (IKeyData keyData in syntaxSettings.EventDataList)
                {
                    if (string.IsNullOrEmpty(keyData.EventName))
                        continue;
                    string s = (keyData.Param != null) ? string.Format("{0}{1}", keyData.EventName, keyData.Param.ToString()) : keyData.EventName;
                    if (s != string.Empty)
                    {
                        if (tbShowCommands.Text != string.Empty)
                        {
                            if ((s.IndexOf(tbShowCommands.Text) >= 0) && (lbEventHandlers.Items.IndexOf(s) < 0))
                                lbEventHandlers.Items.Add(s);
                        }
                        else
                            if (lbEventHandlers.Items.IndexOf(s) < 0)
                                lbEventHandlers.Items.Add(s);
                    }
                }

                lbEventHandlers.Sorted = true;
                if (lbEventHandlers.Items.Count > 0)
                    lbEventHandlers.SelectedIndex = 0;
            }
            finally
            {
                lbEventHandlers.EndUpdate();
            }
        }

        private void UpdateShortcut(int index)
        {
            cbShortcuts.Text = string.Empty;
            cbShortcuts.Items.Clear();
            string eventName = index >= 0 ? lbEventHandlers.Items[index].ToString() : string.Empty;
            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;

                if (eventName.StartsWith(keyData.EventName))
                {
                    string parName = (eventName.Length > keyData.EventName.Length) ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
                    if (((keyData.Param == null) && string.IsNullOrEmpty(parName)) || ((keyData.Param != null) && keyData.Param.ToString() == parName))
                    {
                        string s = ApplyKeyState(keyData);
                        string ss = (s != string.Empty) ? string.Format("{0}, {1}", s, KeyDataToString(keyData.Keys)) : KeyDataToString(keyData.Keys);
                        cbShortcuts.Items.Add(ss);
                    }
                }
            }

            if (cbShortcuts.Items.Count > 0)
                cbShortcuts.SelectedIndex = 0;
        }

        private string KeyDataToString(Keys keyData)
        {
            string result = keyData.ToString();
            string[] s = result.Split(',');
            bool isCtrl = false;
            bool isAlt = false;
            bool isShift = false;
            result = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].IndexOf(sringControl, StringComparison.OrdinalIgnoreCase) >= 0)
                    isCtrl = true;
                else
                    if (s[i].IndexOf(stringAlt, StringComparison.OrdinalIgnoreCase) >= 0)
                        isAlt = true;
                    else
                        if (s[i].IndexOf(stringShift, StringComparison.OrdinalIgnoreCase) >= 0)
                            isShift = true;
                        else
                            result = (result != string.Empty) ? string.Format("{0} + {1}", result, s[i]) : s[i];
            }

            if (isAlt)
                result = (result != string.Empty) ? string.Format("{0} + {1}", stringAlt.ToUpper(), result) : stringAlt.ToUpper();
            if (isShift)
                result = (result != string.Empty) ? string.Format("{0} + {1}", stringShift.ToUpper(), result) : stringShift.ToUpper();
            if (isCtrl)
                result = (result != string.Empty) ? string.Format("{0} + {1}", stringCtrl, result) : stringCtrl;
            return result;
        }

        private string ApplyKeyState(IKeyData key)
        {
            string result = string.Empty;
            if (key.State > 0)
            {
                IKeyData[] keys = syntaxSettings.EventDataList.ToArray();
                foreach (IKeyData keyData in keys)
                {
                    if ((keyData.LeaveState == key.State) && (keyData.State == 0))
                    {
                        result = KeyDataToString(keyData.Keys);
                        break;
                    }
                }
            }

            return result;
        }

        private void ControlsFromSettings()
        {
            FillStyles();
            FillVisualThemes();
            cbFontName.Items.AddRange(GetFonts());
            cbKeyboardSchemes.Items.Clear();
            cbKeyboardSchemes.Items.Add("Default Settings");
            cbKeyboardSchemes.SelectedIndex = 0;
            FillEventHandlers();
            chbDragAndDrop.Checked = (syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == 0;
            switch (syntaxSettings.ScrollBars)
            {
                case RichTextBoxScrollBars.ForcedBoth:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = true;
                        chbForced.Checked = true;
                        break;
                    }

                case RichTextBoxScrollBars.Both:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = true;
                        chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.ForcedHorizontal:
                case RichTextBoxScrollBars.Horizontal:
                    {
                        chbVertScrollBar.Checked = false;
                        chbHorzScrollBar.Checked = true;
                        chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.ForcedVertical:
                case RichTextBoxScrollBars.Vertical:
                    {
                        chbVertScrollBar.Checked = true;
                        chbHorzScrollBar.Checked = false;
                        chbForced.Checked = false;
                        break;
                    }

                case RichTextBoxScrollBars.None:
                    {
                        chbVertScrollBar.Checked = false;
                        chbHorzScrollBar.Checked = false;
                        chbForced.Checked = false;
                        break;
                    }
            }

            chbShowMargin.Checked = syntaxSettings.ShowMargin;
            chbWordWrap.Checked = syntaxSettings.WordWrap;
            chbLineNumbers.Checked = (syntaxSettings.GutterOptions & GutterOptions.PaintLineNumbers) != 0;
            chbLineNumbersOnGutter.Checked = (syntaxSettings.GutterOptions & GutterOptions.PaintLinesOnGutter) != 0;
            chbShowGutter.Checked = syntaxSettings.ShowGutter;
            tbGutterWidth.Text = syntaxSettings.GutterWidth.ToString();
            tbMarginPosition.Text = syntaxSettings.MarginPos.ToString();
            chbBeyondEol.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != 0;
            chbBeyondEof.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != 0;
            chbMoveOnRightButton.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != 0;
            chbHighlightUrls.Checked = syntaxSettings.HighlightHyperText;
            chbAllowOutlining.Checked = syntaxSettings.AllowOutlining;
            chbShowHints.Checked = (syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != 0;
            rbInsertSpaces.Checked = syntaxSettings.UseSpaces;
            rbKeepTabs.Checked = !syntaxSettings.UseSpaces;
            chbWhiteSpace.Checked = syntaxSettings.WhiteSpaceVisible;
            chbLineModificator.Checked = ((syntaxSettings.GutterOptions & GutterOptions.PaintLineModificators) != 0) ? true : false;
            chbLineSeparator.Checked = ((syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != 0) ? true : false;

            string[] s = new string[syntaxSettings.TabStops.Length];
            for (int i = 0; i < syntaxSettings.TabStops.Length; i++)
                s[i] = syntaxSettings.TabStops[i].ToString();
            tbTabStops.Text = string.Join(",", s);
            UpdateFontControls();
        }

        private void UpdateFontControls()
        {
            try
            {
                isFontControlsUpdating = true;

                cbFontName.SelectedIndex = cbFontName.Items.IndexOf(syntaxSettings.Font.Name);
                tbFontSize.Text = syntaxSettings.Font.Size.ToString();
            }
            finally
            {
                isFontControlsUpdating = false;
            }

            FontNameChanged(this, new EventArgs());
            OnStyleSelected(this, new EventArgs());
        }

        private void SettingsFromControl()
        {
            bool vert = chbVertScrollBar.Checked;
            bool horz = chbHorzScrollBar.Checked;
            bool forced = chbForced.Checked;
            if (horz)
            {
                if (vert)
                    syntaxSettings.ScrollBars = forced ? RichTextBoxScrollBars.ForcedBoth : RichTextBoxScrollBars.Both;
                else
                    syntaxSettings.ScrollBars = forced ? RichTextBoxScrollBars.ForcedHorizontal : RichTextBoxScrollBars.Horizontal;
            }
            else
            {
                if (vert)
                    syntaxSettings.ScrollBars = forced ? RichTextBoxScrollBars.ForcedVertical : RichTextBoxScrollBars.Vertical;
                else
                    syntaxSettings.ScrollBars = RichTextBoxScrollBars.None;
            }

            if (chbDragAndDrop.Checked)
                syntaxSettings.SelectionOptions = syntaxSettings.SelectionOptions & ~SelectionOptions.DisableDragging;
            else
                syntaxSettings.SelectionOptions = syntaxSettings.SelectionOptions | SelectionOptions.DisableDragging;
            syntaxSettings.ShowMargin = chbShowMargin.Checked;
            syntaxSettings.WordWrap = chbWordWrap.Checked;
            syntaxSettings.WhiteSpaceVisible = chbWhiteSpace.Checked;
            syntaxSettings.ShowGutter = chbShowGutter.Checked;
            syntaxSettings.GutterWidth = GetInt(tbGutterWidth.Text, EditConsts.DefaultGutterWidth);
            syntaxSettings.MarginPos = GetInt(tbMarginPosition.Text, EditConsts.DefaultMarginPosition);
            if (chbLineNumbers.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLineNumbers;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineNumbers;
            if (chbLineNumbersOnGutter.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLinesOnGutter;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLinesOnGutter;
            if (chbLineModificator.Checked)
                syntaxSettings.GutterOptions |= GutterOptions.PaintLineModificators;
            else
                syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineModificators;

            if (chbLineSeparator.Checked)
                syntaxSettings.SeparatorOptions |= SeparatorOptions.SeparateLines;
            else
                syntaxSettings.SeparatorOptions &= ~SeparatorOptions.SeparateLines;
            if (chbBeyondEol.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;
            if (chbBeyondEof.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;
            if (chbMoveOnRightButton.Checked)
                syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
            else
                syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;
            if (chbShowHints.Checked)
                syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
            else
                syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;

            syntaxSettings.HighlightHyperText = chbHighlightUrls.Checked;
            syntaxSettings.AllowOutlining = chbAllowOutlining.Checked;
            syntaxSettings.UseSpaces = rbInsertSpaces.Checked;

            string[] s = tbTabStops.Text.Split(',');
            int[] tabs = new int[s.Length];
            int j = 0;
            for (int i = 0; i < s.Length; i++)
            {
                j = GetInt(s[i], EditConsts.DefaultTabStop);
                tabs[i] = (j <= 0) ? EditConsts.DefaultTabStop : j;
            }

            syntaxSettings.TabStops = tabs;

            syntaxSettings.Font = new Font(syntaxSettings.Font.Name, Math.Max(Math.Min(GetInt(tbFontSize.Text, 10), EditConsts.MaxFontSize), 1), syntaxSettings.Font.Style);
        }

        private void FillStyles()
        {
            lbStyles.Items.Clear();
            for (int i = 0; i < syntaxSettings.LexStyles.Count; i++)
                lbStyles.Items.Add(syntaxSettings.LexStyles[i].Desc);
        }

        private void FillVisualThemes()
        {
            cbVisualThemes.BeginUpdate();
            cbVisualThemes.Items.Clear();

            foreach (IVisualTheme colorTheme in SyntaxSettings.VisualThemes)
            {
                cbVisualThemes.Items.Add(colorTheme.Name);
            }

            cbVisualThemes.SelectedIndex = SyntaxSettings.VisualThemes.ActiveThemeIndex;

            cbVisualThemes.EndUpdate();
        }

        private void PropertiesTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            Panel panel = GetCurrentPanel();
            if (panel != null)
            {
                panel.Visible = true;
                panel.BringToFront();
                UpdateImages();
            }
        }

        private void UpdateImages()
        {
            if (tvProperties.SelectedNode == rootNode)
                generalNode.ImageIndex = SelectedImage;
            else
                generalNode.ImageIndex = UnSelectedImage;
        }

        private int GetNodeLevel(TreeNode node)
        {
            int result = 0;
            if (node != null)
            {
                while (node.Parent != null)
                {
                    node = node.Parent;
                    result++;
                }
            }

            return result;
        }

        private Panel GetCurrentPanel()
        {
            Panel result = null;
            TreeNode node = tvProperties.SelectedNode;
            if (node != null)
            {
                switch (GetNodeLevel(node))
                {
                    case 0:
                        {
                            result = rootNode.Nodes.Contains(generalNode) ? pnGeneral : null;
                            break;
                        }

                    case 1:
                        {
                            if (node.Equals(generalNode))
                                result = pnGeneral;
                            else
                                if (node.Equals(fontsNode))
                                    result = pnFontsColors;
                                else
                                    if (node.Equals(additionalNode))
                                        result = pnAdditional;
                                    else
                                        if (node.Equals(keyboardNode))
                                            result = pnKeyboard;
                                        else
                                            result = null;
                            break;
                        }
                }
            }

            if (result != null)
            {
                result.Location = new Point(0, 0);
                result.Size = pnManage.Size;
                result.Dock = DockStyle.Fill;
            }

            return result;
        }

        private void DlgSyntaxSettings_Activated(object sender, System.EventArgs e)
        {
            tvProperties.Focus();
        }

        private void DlgSyntaxSettings_Load(object sender, System.EventArgs e)
        {
            LoadFromResource();
            ControlsFromSettings();
            lbStyles.SelectedIndexChanged += new EventHandler(OnStyleSelected);
            lbStyles.SelectedIndex = 0;
            chbBold.CheckedChanged += new EventHandler(FontStyleChange);
            chbItalic.CheckedChanged += new EventHandler(FontStyleChange);
            chbUnderline.CheckedChanged += new EventHandler(FontStyleChange);
            tbDescription.TextChanged += new EventHandler(DescriptionChanged);
            cbFontName.SelectedIndexChanged += new EventHandler(FontNameChanged);
            tbFontSize.TextChanged += new EventHandler(FontSizeChanged);
            pnManage.Controls.Add(pnGeneral);
            pnManage.Controls.Add(pnFontsColors);
            pnManage.Controls.Add(pnAdditional);
            pnManage.Controls.Add(pnKeyboard);
            tvProperties.Nodes[0].ImageIndex = OpenFolderImage;
            tvProperties.Nodes[0].Expand();
            tvProperties.SelectedNode = (tvProperties.Nodes[0].Nodes.Count > 0) ? tvProperties.Nodes[0].Nodes[0] : tvProperties.Nodes[0];
        }

        private void LoadFromResource()
        {
            Text = DlgSyntaxSettingsConsts.DlgSyntaxSettingsCaption;
            if (tvProperties.Nodes.Count > 0)
            {
                rootNode.Text = DlgSyntaxSettingsConsts.PropertiesOptionsCaption;
                if (tvProperties.Nodes[0].Nodes.Count >= 3)
                {
                    generalNode.Text = DlgSyntaxSettingsConsts.PropertiesGeneralCaption;
                    fontsNode.Text = DlgSyntaxSettingsConsts.PropertiesFontsColorsCaption;
                    additionalNode.Text = DlgSyntaxSettingsConsts.PropertiesAdditionalCaption;
                    keyboardNode.Text = DlgSyntaxSettingsConsts.PropertiesKeyboradCaption;
                }
            }

            tpGeneral.Text = DlgSyntaxSettingsConsts.GeneralCaption;
            tpFontsAndColors.Text = DlgSyntaxSettingsConsts.FontsAndColorsCaption;
            tpAdditional.Text = DlgSyntaxSettingsConsts.AdditionalCaption;
            gbDocument.Text = DlgSyntaxSettingsConsts.DocumentCaption;
            gbGutterMargin.Text = DlgSyntaxSettingsConsts.GutterMarginCaption;
            gbLineNumbers.Text = DlgSyntaxSettingsConsts.GroupBoxLineNumbersCaptionSyntaxSettingsDlg;
            chbWordWrap.Text = DlgSyntaxSettingsConsts.WordWrapCaptionSyntaxSettingsDlg;
            chbHighlightUrls.Text = DlgSyntaxSettingsConsts.HighlightUrlsCaption;
            chbDragAndDrop.Text = DlgSyntaxSettingsConsts.DragAndDropCaption;
            chbVertScrollBar.Text = DlgSyntaxSettingsConsts.VertScrollBarCaption;
            chbHorzScrollBar.Text = DlgSyntaxSettingsConsts.HorzScrollBarCaption;
            chbForced.Text = DlgSyntaxSettingsConsts.ForcedCaption;
            chbShowGutter.Text = DlgSyntaxSettingsConsts.ShowGutterCaption;
            chbShowMargin.Text = DlgSyntaxSettingsConsts.ShowMarginCaption;
            laGutterWidth.Text = DlgSyntaxSettingsConsts.GutterWidthCaption;
            laMarginPosition.Text = DlgSyntaxSettingsConsts.MarginPositionCaption;
            chbLineNumbers.Text = DlgSyntaxSettingsConsts.CheckBoxLineNumbersCaptionSyntaxSettingsDlg;
            chbLineNumbersOnGutter.Text = DlgSyntaxSettingsConsts.LineNumbersOnGutterCaption;
            laFont.Text = DlgSyntaxSettingsConsts.FontCaption;
            laSize.Text = DlgSyntaxSettingsConsts.SizeCaption;
            btAddVisualTheme.Text = DlgSyntaxSettingsConsts.AddVisualTheme;
            btDeleteVisualTheme.Text = DlgSyntaxSettingsConsts.DeleteVisualTheme;
            laDisplayItems.Text = DlgSyntaxSettingsConsts.DisplayItemsCaption;
            laDescription.Text = DlgSyntaxSettingsConsts.DescriptionCaption;
            laForeColor.Text = DlgSyntaxSettingsConsts.ForeColorCaption;
            laBackColor.Text = DlgSyntaxSettingsConsts.BackColorCaption;
            gbFontAttributes.Text = DlgSyntaxSettingsConsts.FontAttributesCaption;
            chbBold.Text = DlgSyntaxSettingsConsts.BoldCaption;
            chbItalic.Text = DlgSyntaxSettingsConsts.ItalicCaption;
            chbUnderline.Text = DlgSyntaxSettingsConsts.UnderlineCaption;
            laSample.Text = DlgSyntaxSettingsConsts.SampleCaption;
            laSampleText.Text = DlgSyntaxSettingsConsts.SampleTextCaption;
            gbNavigateOptions.Text = DlgSyntaxSettingsConsts.NavigateOptionsCaption;
            gbOutlineOptions.Text = DlgSyntaxSettingsConsts.OutlineOptionsCaption;
            gbTabOptions.Text = DlgSyntaxSettingsConsts.TabOptionsCaption;
            chbBeyondEol.Text = DlgSyntaxSettingsConsts.BeyondEolCaption;
            chbBeyondEof.Text = DlgSyntaxSettingsConsts.BeyondEofCaption;
            chbMoveOnRightButton.Text = DlgSyntaxSettingsConsts.MoveOnRightButtonCaption;
            chbAllowOutlining.Text = DlgSyntaxSettingsConsts.AllowOutliningCaption;
            chbShowHints.Text = DlgSyntaxSettingsConsts.ShowHintsCaption;
            laTabSizes.Text = DlgSyntaxSettingsConsts.TabSizesCaption;
            rbInsertSpaces.Text = DlgSyntaxSettingsConsts.InsertSpacesCaption;
            rbKeepTabs.Text = DlgSyntaxSettingsConsts.KeepTabsCaption;
            btOK.Text = DlgSyntaxSettingsConsts.OKCaptionSyntaxSettingsDlg;
            btCancel.Text = DlgSyntaxSettingsConsts.CancelCaptionSyntaxSettingsDlg;
            chbWhiteSpace.Text = DlgSyntaxSettingsConsts.WhiteSpaceCaptionSyntaxSettingsDlg;
            chbLineModificator.Text = DlgSyntaxSettingsConsts.LineModificatorCaptionSyntaxSettingsDlg;
            chbLineSeparator.Text = DlgSyntaxSettingsConsts.LineSeparatorCaptionSyntaxSettingsDlg;

            laKeyboardMappingScheme.Text = DlgSyntaxSettingsConsts.KeyboardMappingSchemeCaption;
            laShowCommands.Text = DlgSyntaxSettingsConsts.ShowCommandsCaption;
            laShortcuts.Text = DlgSyntaxSettingsConsts.ShortcutsCaption;
            btSaveSchemeAs.Text = DlgSyntaxSettingsConsts.SaveSchemeAsCaption;
            btDeleteScheme.Text = DlgSyntaxSettingsConsts.DeleteSchemeCaption;
        }

        private void DescriptionChanged(object sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curDesc = tbDescription.Text;
            StyleFromControl();
        }

        private void FontStyleChange(object sender, EventArgs e)
        {
            if (isControlUpdating)
                return;
            curFontStyle = FontStyle.Regular;
            if (chbBold.Checked)
                curFontStyle |= FontStyle.Bold;
            else
                curFontStyle &= ~FontStyle.Bold;
            if (chbItalic.Checked)
                curFontStyle |= FontStyle.Italic;
            else
                curFontStyle &= ~FontStyle.Italic;
            if (chbUnderline.Checked)
                curFontStyle |= FontStyle.Underline;
            else
                curFontStyle &= ~FontStyle.Underline;
            StyleFromControl();
        }

        private void StyleFromControl()
        {
            if (lbStyles.SelectedItem == null)
                return;
            ILexStyle style = GetSelectedStyle();
            style.ForeColor = curForeColor;
            style.BackColor = curBkColor;
            style.FontStyle = curFontStyle;
            style.Desc = curDesc;
            UpdateStyleControls();
        }

        private void StyleSelected()
        {
            tbDescription.Enabled = syntaxSettings.IsDescriptionEnabled(lbStyles.SelectedIndex);
            laDescription.Enabled = syntaxSettings.IsDescriptionEnabled(lbStyles.SelectedIndex);
            gbFontAttributes.Enabled = syntaxSettings.IsFontStyleEnabled(lbStyles.SelectedIndex);
            laBackColor.Enabled = syntaxSettings.IsBackColorEnabled(lbStyles.SelectedIndex);
            cbBackColor.Enabled = syntaxSettings.IsBackColorEnabled(lbStyles.SelectedIndex);
            ILexStyle style = GetSelectedStyle();
            if (style != null)
            {
                laDescription.Enabled = true;
                tbDescription.Enabled = true;

                laForeColor.Enabled = style.ForeColorEnabled;
                cbForeColor.Enabled = style.ForeColorEnabled;
                laBackColor.Enabled = style.BackColorEnabled;
                cbBackColor.Enabled = style.BackColorEnabled;
                chbBold.Enabled = style.BoldEnabled;
                chbItalic.Enabled = style.ItalicEnabled;
                chbUnderline.Enabled = style.UnderlineEnabled;

                curForeColor = style.ForeColor;
                curBkColor = style.BackColor;
                curFontStyle = style.FontStyle;
                curDesc = style.Desc;
            }

            UpdateStyleControls();
        }

        private void OnStyleSelected(object sender, EventArgs e)
        {
            StyleSelected();
        }

        private ILexStyle GetSelectedStyle()
        {
            if (lbStyles.SelectedItem != null)
                return syntaxSettings.LexStyles[lbStyles.SelectedIndex];
            return null;
        }

        private void UpdateStyleControls()
        {
            isControlUpdating = true;
            try
            {
                chbBold.Checked = (curFontStyle & FontStyle.Bold) != 0;
                chbItalic.Checked = (curFontStyle & FontStyle.Italic) != 0;
                chbUnderline.Checked = (curFontStyle & FontStyle.Underline) != 0;
                tbDescription.Text = curDesc;
                laSampleText.Font = new Font(laSampleText.Font.Name, (int)laSampleText.Font.Size, curFontStyle);
                cbForeColor.SelectedColor = curForeColor;
                cbBackColor.SelectedColor = curBkColor;
                laSampleText.ForeColor = curForeColor;
                pnSampleText.BackColor = (curBkColor != Color.Empty) ? curBkColor : syntaxSettings.VisualThemes.ActiveTheme[VisualThemeConsts.WindowColorInternalName].BackColor;
                WriteSampleText();
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void FontNameChanged(object sender, System.EventArgs e)
        {
            if (isControlUpdating || (cbFontName.SelectedItem == null))
                return;
            try
            {
                laSampleText.Font = new Font(cbFontName.Text, laSampleText.Font.Size, curFontStyle);
            }
            catch
            {
            }

            WriteSampleText();
        }

        private void FontSizeChanged(object sender, System.EventArgs e)
        {
            if (isControlUpdating)
                return;
            isControlUpdating = true;
            try
            {
                int fontSize = Math.Max(Math.Min(GetInt(tbFontSize.Text, 10), EditConsts.MaxFontSize), 1);
                if (tbFontSize.Text != fontSize.ToString())
                    tbFontSize.Text = fontSize.ToString();
                laSampleText.Font = new Font(laSampleText.Font.Name, fontSize, curFontStyle);
                WriteSampleText();
            }
            finally
            {
                isControlUpdating = false;
            }
        }

        private void WriteSampleText()
        {
            laSampleText.Location = new Point(
                (pnSampleText.Width - laSampleText.Width) / 2,
                (pnSampleText.Height - laSampleText.Height) / 2);
        }

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            SettingsFromControl();
        }

        private void ForeColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isControlUpdating)
                return;
            curForeColor = cbForeColor.SelectedColor;
            StyleFromControl();
        }

        private void BackColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (isControlUpdating)
                return;
            curBkColor = cbBackColor.SelectedColor;
            StyleFromControl();
        }

        private void MarginPositionTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
                if (!e.Handled)
                {
                    try
                    {
                        string s = tbMarginPosition.Text;
                        int.Parse(s.Insert(Math.Min(tbMarginPosition.SelectionStart, s.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                    OSUtils.MessageBeep();
            }
        }

        private void GutterWidthTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar > ' ')
            {
                e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
                if (!e.Handled)
                {
                    try
                    {
                        string s = tbGutterWidth.Text;
                        int.Parse(s.Insert(Math.Min(tbGutterWidth.SelectionStart, s.Length), e.KeyChar.ToString()));
                    }
                    catch
                    {
                        e.Handled = true;
                        OSUtils.MessageBeep();
                    }
                }
                else
                    OSUtils.MessageBeep();
            }
        }

        private void ShowCommandsTextBox_TextChanged(object sender, System.EventArgs e)
        {
            UpdateEventHandlers();
        }

        private void EventHandlersLisoxTextBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateShortcut(lbEventHandlers.SelectedIndex);
        }

        private void VisualThemesComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((cbVisualThemes.SelectedIndex >= 0) && (cbVisualThemes.SelectedIndex < syntaxSettings.VisualThemes.Count))
            {
                syntaxSettings.VisualThemes.ActiveThemeIndex = cbVisualThemes.SelectedIndex;
                btDeleteVisualTheme.Enabled = !syntaxSettings.VisualThemes.ActiveTheme.ReadOnly;
                UpdateFontControls();
                FillStyles();
                StyleSelected();
            }
        }

        private void AddVisualThemeButton_Click(object sender, System.EventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;

            VisualTheme theme = new VisualTheme();

            ISerializationInfo info = colorThemes.ActiveTheme.GetSerializationInfo();
            info.Load();
            theme.SetSerializationInfo(info);
            colorThemes.Add(theme);

            int newVisualThemeIndex = colorThemes.Count - 1;
            string name = colorThemes[newVisualThemeIndex].Name;
            colorThemes[newVisualThemeIndex].Name = "Copy of " + name;
            colorThemes[newVisualThemeIndex].ReadOnly = false;
            FillVisualThemes();
            colorThemes.ActiveThemeIndex = newVisualThemeIndex;
            cbVisualThemes.SelectedIndex = newVisualThemeIndex;
        }

        private void DeleteVisualThemeButton_Click(object sender, System.EventArgs e)
        {
            IVisualThemes colorThemes = syntaxSettings.VisualThemes;

            if (colorThemes.ActiveThemeIndex != -1)
            {
                if (!colorThemes[colorThemes.ActiveThemeIndex].ReadOnly)
                {
                    colorThemes.RemoveAt(colorThemes.ActiveThemeIndex);
                    colorThemes.ActiveThemeIndex--;
                    FillVisualThemes();
                    cbVisualThemes.SelectedIndex = colorThemes.ActiveThemeIndex;
                }
            }
        }

        private void VisualThemesComboBox_Leave(object sender, System.EventArgs e)
        {
            if (cbVisualThemes.SelectedIndex == -1)
            {
                if ((!syntaxSettings.VisualThemes.ActiveTheme.ReadOnly) && (cbVisualThemes.Text != string.Empty))
                {
                    syntaxSettings.VisualThemes.ActiveTheme.Name = cbVisualThemes.Text;
                    FillVisualThemes();
                }
            }
        }

        private void UpdateActiveVisualThemeFont()
        {
            if (isFontControlsUpdating)
                return;

            if ((cbFontName.SelectedIndex != -1) && (tbFontSize.Text != null))
            {
                string fontName = cbFontName.SelectedItem.ToString();
                syntaxSettings.Font = new Font(fontName, GetInt(tbFontSize.Text, 10), FontInfos.GetAvailableFontStyle(new FontFamily(fontName), FontStyle.Regular));
            }
        }

        private void FontNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateActiveVisualThemeFont();
        }

        private void FontSizeTextBox_Leave(object sender, System.EventArgs e)
        {
            UpdateActiveVisualThemeFont();
        }

        private void UpdateShortcutButton_Click(object sender, EventArgs e)
        {
            int index = lbEventHandlers.SelectedIndex;
            string oldText = cbShortcuts.Items[currentKeys].ToString();
            string newText = cbShortcuts.Text;
            if (string.Compare(oldText, newText) == 0)
            {
                return;
            }

            var keys = KeyUtils.KeyDataFromString(oldText);
            string eventName = index >= 0 ? lbEventHandlers.Items[index].ToString() : string.Empty;
            foreach (IKeyData keyData in syntaxSettings.EventDataList)
            {
                if (string.IsNullOrEmpty(keyData.EventName))
                    continue;
                if (eventName.StartsWith(keyData.EventName))
                {
                    string parName = (eventName.Length > keyData.EventName.Length) ? eventName.Remove(0, keyData.EventName.Length) : string.Empty;
                    if ((keyData.Param == null) || (keyData.Param.ToString() == parName))
                    {
                        if (keyData.Keys == keys)
                        {
                            keyData.Keys = KeyUtils.KeyDataFromString(newText.Replace("+", string.Empty));
                            cbShortcuts.Items[currentKeys] = newText;
                        }
                    }
                }
            }
        }

        #endregion
    }
    #endregion
}
