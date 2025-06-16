#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

namespace SearchReplace
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Designer generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Designer generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Designer generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Designer generated code")]
    public partial class DlgSearchInFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            chbMatchWholeWord = new System.Windows.Forms.CheckBox();
            btReplacePopup = new System.Windows.Forms.Button();
            chbMatchCase = new System.Windows.Forms.CheckBox();
            chbUseRegularExpressions = new System.Windows.Forms.CheckBox();
            cmReplace = new System.Windows.Forms.ContextMenuStrip(components);
            miFindWhatText = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression1 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression2 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression3 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression4 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression5 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression6 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression7 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression8 = new System.Windows.Forms.ToolStripMenuItem();
            miTaggedExpression9 = new System.Windows.Forms.ToolStripMenuItem();
            miTag = new System.Windows.Forms.ToolStripMenuItem();
            laFindWhat = new System.Windows.Forms.Label();
            cbReplaceWith = new System.Windows.Forms.ComboBox();
            laReplaceWith = new System.Windows.Forms.Label();
            miEscape = new System.Windows.Forms.ToolStripMenuItem();
            btReplaceAll = new System.Windows.Forms.Button();
            btMarkAll = new System.Windows.Forms.Button();
            btReplace = new System.Windows.Forms.Button();
            btFindNext = new System.Windows.Forms.Button();
            cmFind = new System.Windows.Forms.ContextMenuStrip(components);
            miSingleChar = new System.Windows.Forms.ToolStripMenuItem();
            miZeroOrMore = new System.Windows.Forms.ToolStripMenuItem();
            miOneOrMore = new System.Windows.Forms.ToolStripMenuItem();
            menuItem4 = new System.Windows.Forms.ToolStripSeparator();
            miBeginLine = new System.Windows.Forms.ToolStripMenuItem();
            miEndLine = new System.Windows.Forms.ToolStripMenuItem();
            miLineBreak = new System.Windows.Forms.ToolStripMenuItem();
            menuItem10 = new System.Windows.Forms.ToolStripSeparator();
            miOneCharInSet = new System.Windows.Forms.ToolStripMenuItem();
            miOneCharNotInSet = new System.Windows.Forms.ToolStripMenuItem();
            miOr = new System.Windows.Forms.ToolStripMenuItem();
            btPopup = new System.Windows.Forms.Button();
            cbFindWhat = new System.Windows.Forms.ComboBox();
            pnButtons = new System.Windows.Forms.Panel();
            btFindPrevious = new System.Windows.Forms.Button();
            FindAllButton = new System.Windows.Forms.Button();
            btClose = new System.Windows.Forms.Button();
            pnMain = new System.Windows.Forms.Panel();
            FileTypesLabel = new System.Windows.Forms.Label();
            FileTypesComboBox = new System.Windows.Forms.ComboBox();
            pnFindOptions = new System.Windows.Forms.Panel();
            laFindOptions = new System.Windows.Forms.Label();
            btFindOptions = new System.Windows.Forms.Button();
            laLookIn = new System.Windows.Forms.Label();
            cbLookIn = new System.Windows.Forms.ComboBox();
            gbFindOptions = new System.Windows.Forms.GroupBox();
            tbSearch = new System.Windows.Forms.TabControl();
            tbUseFind = new System.Windows.Forms.TabPage();
            tbUseReplace = new System.Windows.Forms.TabPage();
            cmReplace.SuspendLayout();
            cmFind.SuspendLayout();
            pnButtons.SuspendLayout();
            pnMain.SuspendLayout();
            gbFindOptions.SuspendLayout();
            tbSearch.SuspendLayout();
            SuspendLayout();
            // 
            // chbMatchWholeWord
            // 
            chbMatchWholeWord.FlatStyle = System.Windows.Forms.FlatStyle.System;
            chbMatchWholeWord.Location = new System.Drawing.Point(10, 41);
            chbMatchWholeWord.Margin = new System.Windows.Forms.Padding(4);
            chbMatchWholeWord.Name = "chbMatchWholeWord";
            chbMatchWholeWord.Size = new System.Drawing.Size(253, 29);
            chbMatchWholeWord.TabIndex = 12;
            chbMatchWholeWord.Text = "Match &whole word";
            chbMatchWholeWord.CheckedChanged += chbMatchWholeWord_CheckedChanged;
            // 
            // btReplacePopup
            // 
            btReplacePopup.BackColor = System.Drawing.SystemColors.Control;
            btReplacePopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btReplacePopup.ImageIndex = 0;
            btReplacePopup.Location = new System.Drawing.Point(350, 69);
            btReplacePopup.Margin = new System.Windows.Forms.Padding(4);
            btReplacePopup.Name = "btReplacePopup";
            btReplacePopup.Size = new System.Drawing.Size(20, 23);
            btReplacePopup.TabIndex = 32;
            btReplacePopup.Text = ">";
            btReplacePopup.UseVisualStyleBackColor = false;
            btReplacePopup.Visible = false;
            btReplacePopup.Click += btReplacePopup_Click;
            // 
            // chbMatchCase
            // 
            chbMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
            chbMatchCase.Location = new System.Drawing.Point(10, 19);
            chbMatchCase.Margin = new System.Windows.Forms.Padding(4);
            chbMatchCase.Name = "chbMatchCase";
            chbMatchCase.Size = new System.Drawing.Size(253, 29);
            chbMatchCase.TabIndex = 11;
            chbMatchCase.Text = "Match &case";
            chbMatchCase.CheckedChanged += chbMatchCase_CheckedChanged;
            // 
            // chbUseRegularExpressions
            // 
            chbUseRegularExpressions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            chbUseRegularExpressions.Location = new System.Drawing.Point(10, 64);
            chbUseRegularExpressions.Margin = new System.Windows.Forms.Padding(4);
            chbUseRegularExpressions.Name = "chbUseRegularExpressions";
            chbUseRegularExpressions.Size = new System.Drawing.Size(253, 29);
            chbUseRegularExpressions.TabIndex = 15;
            chbUseRegularExpressions.Text = "Use &regular expressions";
            chbUseRegularExpressions.CheckedChanged += chbUseRegularExpressions_Click;
            // 
            // cmReplace
            // 
            cmReplace.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmReplace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miFindWhatText, miTaggedExpression1, miTaggedExpression2, miTaggedExpression3, miTaggedExpression4, miTaggedExpression5, miTaggedExpression6, miTaggedExpression7, miTaggedExpression8, miTaggedExpression9 });
            cmReplace.Name = "cmReplace";
            cmReplace.Size = new System.Drawing.Size(196, 224);
            // 
            // miFindWhatText
            // 
            miFindWhatText.Name = "miFindWhatText";
            miFindWhatText.Size = new System.Drawing.Size(195, 22);
            miFindWhatText.Text = "$0 Find What Text";
            miFindWhatText.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression1
            // 
            miTaggedExpression1.Name = "miTaggedExpression1";
            miTaggedExpression1.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression1.Text = "$1 Tagged Expression 1";
            miTaggedExpression1.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression2
            // 
            miTaggedExpression2.Name = "miTaggedExpression2";
            miTaggedExpression2.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression2.Text = "$2 Tagged Expression 2";
            miTaggedExpression2.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression3
            // 
            miTaggedExpression3.Name = "miTaggedExpression3";
            miTaggedExpression3.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression3.Text = "$3 Tagged Expression 3";
            miTaggedExpression3.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression4
            // 
            miTaggedExpression4.Name = "miTaggedExpression4";
            miTaggedExpression4.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression4.Text = "$4 Tagged Expression 4";
            miTaggedExpression4.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression5
            // 
            miTaggedExpression5.Name = "miTaggedExpression5";
            miTaggedExpression5.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression5.Text = "$5 Tagged Expression 5";
            miTaggedExpression5.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression6
            // 
            miTaggedExpression6.Name = "miTaggedExpression6";
            miTaggedExpression6.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression6.Text = "$6 Tagged Expression 6";
            miTaggedExpression6.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression7
            // 
            miTaggedExpression7.Name = "miTaggedExpression7";
            miTaggedExpression7.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression7.Text = "$7 Tagged Expression 7";
            miTaggedExpression7.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression8
            // 
            miTaggedExpression8.Name = "miTaggedExpression8";
            miTaggedExpression8.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression8.Text = "$8 Tagged Expression 8";
            miTaggedExpression8.Click += miFindWhatText_Click;
            // 
            // miTaggedExpression9
            // 
            miTaggedExpression9.Name = "miTaggedExpression9";
            miTaggedExpression9.Size = new System.Drawing.Size(195, 22);
            miTaggedExpression9.Text = "$9 Tagged Expression 9";
            miTaggedExpression9.Click += miFindWhatText_Click;
            // 
            // miTag
            // 
            miTag.Name = "miTag";
            miTag.Size = new System.Drawing.Size(261, 22);
            miTag.Text = "{} Tag expression";
            miTag.Click += miBeginWord_Click;
            // 
            // laFindWhat
            // 
            laFindWhat.AutoSize = true;
            laFindWhat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            laFindWhat.Location = new System.Drawing.Point(10, 4);
            laFindWhat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laFindWhat.Name = "laFindWhat";
            laFindWhat.Size = new System.Drawing.Size(62, 15);
            laFindWhat.TabIndex = 20;
            laFindWhat.Text = "Fi&nd what:";
            // 
            // cbReplaceWith
            // 
            cbReplaceWith.Location = new System.Drawing.Point(10, 69);
            cbReplaceWith.Margin = new System.Windows.Forms.Padding(4);
            cbReplaceWith.Name = "cbReplaceWith";
            cbReplaceWith.Size = new System.Drawing.Size(329, 23);
            cbReplaceWith.TabIndex = 24;
            cbReplaceWith.Visible = false;
            // 
            // laReplaceWith
            // 
            laReplaceWith.AutoSize = true;
            laReplaceWith.FlatStyle = System.Windows.Forms.FlatStyle.System;
            laReplaceWith.Location = new System.Drawing.Point(10, 51);
            laReplaceWith.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laReplaceWith.Name = "laReplaceWith";
            laReplaceWith.Size = new System.Drawing.Size(77, 15);
            laReplaceWith.TabIndex = 22;
            laReplaceWith.Text = "Re&place with:";
            laReplaceWith.Visible = false;
            // 
            // miEscape
            // 
            miEscape.Name = "miEscape";
            miEscape.Size = new System.Drawing.Size(261, 22);
            miEscape.Text = "\\ Escape Special Character";
            miEscape.Click += miBeginWord_Click;
            // 
            // btReplaceAll
            // 
            btReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btReplaceAll.Location = new System.Drawing.Point(255, 40);
            btReplaceAll.Margin = new System.Windows.Forms.Padding(4);
            btReplaceAll.Name = "btReplaceAll";
            btReplaceAll.Size = new System.Drawing.Size(115, 26);
            btReplaceAll.TabIndex = 28;
            btReplaceAll.Text = "Replace &All";
            btReplaceAll.Visible = false;
            btReplaceAll.Click += btReplaceAll_Click;
            // 
            // btMarkAll
            // 
            btMarkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btMarkAll.Location = new System.Drawing.Point(255, 9);
            btMarkAll.Margin = new System.Windows.Forms.Padding(4);
            btMarkAll.Name = "btMarkAll";
            btMarkAll.Size = new System.Drawing.Size(115, 26);
            btMarkAll.TabIndex = 28;
            btMarkAll.Text = "Bookmark All";
            btMarkAll.Click += btMarkAll_Click;
            // 
            // btReplace
            // 
            btReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            btReplace.Location = new System.Drawing.Point(133, 40);
            btReplace.Margin = new System.Windows.Forms.Padding(4);
            btReplace.Name = "btReplace";
            btReplace.Size = new System.Drawing.Size(115, 26);
            btReplace.TabIndex = 27;
            btReplace.Text = "&Replace";
            btReplace.Visible = false;
            btReplace.Click += btReplace_Click;
            // 
            // btFindNext
            // 
            btFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btFindNext.Location = new System.Drawing.Point(133, 9);
            btFindNext.Margin = new System.Windows.Forms.Padding(4);
            btFindNext.Name = "btFindNext";
            btFindNext.Size = new System.Drawing.Size(115, 26);
            btFindNext.TabIndex = 25;
            btFindNext.Text = "&Find Next";
            btFindNext.Click += btFindNext_Click;
            // 
            // cmFind
            // 
            cmFind.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmFind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miSingleChar, miZeroOrMore, miOneOrMore, menuItem4, miBeginLine, miEndLine, miLineBreak, menuItem10, miOneCharInSet, miOneCharNotInSet, miOr, miEscape, miTag });
            cmFind.Name = "cmFind";
            cmFind.Size = new System.Drawing.Size(262, 258);
            // 
            // miSingleChar
            // 
            miSingleChar.Name = "miSingleChar";
            miSingleChar.Size = new System.Drawing.Size(261, 22);
            miSingleChar.Text = ". Any single character";
            miSingleChar.Click += miBeginWord_Click;
            // 
            // miZeroOrMore
            // 
            miZeroOrMore.Name = "miZeroOrMore";
            miZeroOrMore.Size = new System.Drawing.Size(261, 22);
            miZeroOrMore.Text = "* Zero or more";
            miZeroOrMore.Click += miBeginWord_Click;
            // 
            // miOneOrMore
            // 
            miOneOrMore.Name = "miOneOrMore";
            miOneOrMore.Size = new System.Drawing.Size(261, 22);
            miOneOrMore.Text = "+ One or more";
            miOneOrMore.Click += miBeginWord_Click;
            // 
            // menuItem4
            // 
            menuItem4.Name = "menuItem4";
            menuItem4.Size = new System.Drawing.Size(258, 6);
            // 
            // miBeginLine
            // 
            miBeginLine.Name = "miBeginLine";
            miBeginLine.Size = new System.Drawing.Size(261, 22);
            miBeginLine.Text = "^ Beginning of line";
            miBeginLine.Click += miBeginWord_Click;
            // 
            // miEndLine
            // 
            miEndLine.Name = "miEndLine";
            miEndLine.Size = new System.Drawing.Size(261, 22);
            miEndLine.Text = "$ End of line";
            miEndLine.Click += miBeginWord_Click;
            // 
            // miLineBreak
            // 
            miLineBreak.Name = "miLineBreak";
            miLineBreak.Size = new System.Drawing.Size(261, 22);
            miLineBreak.Text = "\\n Line break";
            miLineBreak.Click += miBeginWord_Click;
            // 
            // menuItem10
            // 
            menuItem10.Name = "menuItem10";
            menuItem10.Size = new System.Drawing.Size(258, 6);
            // 
            // miOneCharInSet
            // 
            miOneCharInSet.Name = "miOneCharInSet";
            miOneCharInSet.Size = new System.Drawing.Size(261, 22);
            miOneCharInSet.Text = "[] Any one character in the set";
            miOneCharInSet.Click += miBeginWord_Click;
            // 
            // miOneCharNotInSet
            // 
            miOneCharNotInSet.Name = "miOneCharNotInSet";
            miOneCharNotInSet.Size = new System.Drawing.Size(261, 22);
            miOneCharNotInSet.Text = "[^] Any one character not in the set";
            miOneCharNotInSet.Click += miBeginWord_Click;
            // 
            // miOr
            // 
            miOr.Name = "miOr";
            miOr.Size = new System.Drawing.Size(261, 22);
            miOr.Text = "| Or";
            miOr.Click += miBeginWord_Click;
            // 
            // btPopup
            // 
            btPopup.BackColor = System.Drawing.SystemColors.Control;
            btPopup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btPopup.ImageIndex = 0;
            btPopup.Location = new System.Drawing.Point(350, 22);
            btPopup.Margin = new System.Windows.Forms.Padding(4);
            btPopup.Name = "btPopup";
            btPopup.Size = new System.Drawing.Size(20, 23);
            btPopup.TabIndex = 23;
            btPopup.Text = ">";
            btPopup.UseVisualStyleBackColor = false;
            btPopup.Click += btPopup_Click;
            // 
            // cbFindWhat
            // 
            cbFindWhat.Location = new System.Drawing.Point(10, 22);
            cbFindWhat.Margin = new System.Windows.Forms.Padding(4);
            cbFindWhat.Name = "cbFindWhat";
            cbFindWhat.Size = new System.Drawing.Size(329, 23);
            cbFindWhat.TabIndex = 21;
            cbFindWhat.TextChanged += cbFindWhat_TextChanged;
            // 
            // pnButtons
            // 
            pnButtons.Controls.Add(btFindPrevious);
            pnButtons.Controls.Add(FindAllButton);
            pnButtons.Controls.Add(btClose);
            pnButtons.Controls.Add(btFindNext);
            pnButtons.Controls.Add(btReplace);
            pnButtons.Controls.Add(btReplaceAll);
            pnButtons.Controls.Add(btMarkAll);
            pnButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnButtons.Location = new System.Drawing.Point(0, 334);
            pnButtons.Margin = new System.Windows.Forms.Padding(4);
            pnButtons.Name = "pnButtons";
            pnButtons.Size = new System.Drawing.Size(380, 75);
            pnButtons.TabIndex = 34;
            // 
            // btFindPrevious
            // 
            btFindPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btFindPrevious.Location = new System.Drawing.Point(10, 9);
            btFindPrevious.Margin = new System.Windows.Forms.Padding(4);
            btFindPrevious.Name = "btFindPrevious";
            btFindPrevious.Size = new System.Drawing.Size(115, 26);
            btFindPrevious.TabIndex = 24;
            btFindPrevious.Text = "&Find Previous";
            btFindPrevious.Click += btFindPrevious_Click;
            // 
            // FindAllButton
            // 
            FindAllButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            FindAllButton.Location = new System.Drawing.Point(255, 9);
            FindAllButton.Margin = new System.Windows.Forms.Padding(4);
            FindAllButton.Name = "FindAllButton";
            FindAllButton.Size = new System.Drawing.Size(115, 26);
            FindAllButton.TabIndex = 26;
            FindAllButton.Text = "Find All";
            FindAllButton.Click += FindAllButton_Click;
            // 
            // btClose
            // 
            btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btClose.Location = new System.Drawing.Point(224, 274);
            btClose.Margin = new System.Windows.Forms.Padding(4);
            btClose.Name = "btClose";
            btClose.Size = new System.Drawing.Size(115, 26);
            btClose.TabIndex = 29;
            btClose.Text = "Close";
            btClose.Click += btClose_Click;
            // 
            // pnMain
            // 
            pnMain.Controls.Add(FileTypesLabel);
            pnMain.Controls.Add(FileTypesComboBox);
            pnMain.Controls.Add(pnFindOptions);
            pnMain.Controls.Add(laFindOptions);
            pnMain.Controls.Add(btFindOptions);
            pnMain.Controls.Add(laLookIn);
            pnMain.Controls.Add(cbLookIn);
            pnMain.Controls.Add(gbFindOptions);
            pnMain.Controls.Add(btReplacePopup);
            pnMain.Controls.Add(laFindWhat);
            pnMain.Controls.Add(cbReplaceWith);
            pnMain.Controls.Add(laReplaceWith);
            pnMain.Controls.Add(btPopup);
            pnMain.Controls.Add(cbFindWhat);
            pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            pnMain.Location = new System.Drawing.Point(0, 28);
            pnMain.Margin = new System.Windows.Forms.Padding(4);
            pnMain.Name = "pnMain";
            pnMain.Size = new System.Drawing.Size(380, 306);
            pnMain.TabIndex = 33;
            // 
            // FileTypesLabel
            // 
            FileTypesLabel.AutoSize = true;
            FileTypesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            FileTypesLabel.Location = new System.Drawing.Point(10, 143);
            FileTypesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            FileTypesLabel.Name = "FileTypesLabel";
            FileTypesLabel.Size = new System.Drawing.Size(59, 15);
            FileTypesLabel.TabIndex = 43;
            FileTypesLabel.Text = "File types:";
            // 
            // FileTypesComboBox
            // 
            FileTypesComboBox.FormattingEnabled = true;
            FileTypesComboBox.Items.AddRange(new object[] { "*.cs;*.vb;*.txt", "*.*" });
            FileTypesComboBox.Location = new System.Drawing.Point(10, 162);
            FileTypesComboBox.Margin = new System.Windows.Forms.Padding(4);
            FileTypesComboBox.Name = "FileTypesComboBox";
            FileTypesComboBox.Size = new System.Drawing.Size(360, 23);
            FileTypesComboBox.TabIndex = 38;
            // 
            // pnFindOptions
            // 
            pnFindOptions.BackColor = System.Drawing.SystemColors.ScrollBar;
            pnFindOptions.Location = new System.Drawing.Point(105, 192);
            pnFindOptions.Margin = new System.Windows.Forms.Padding(4);
            pnFindOptions.Name = "pnFindOptions";
            pnFindOptions.Size = new System.Drawing.Size(260, 1);
            pnFindOptions.TabIndex = 41;
            pnFindOptions.Visible = false;
            // 
            // laFindOptions
            // 
            laFindOptions.AutoSize = true;
            laFindOptions.Location = new System.Drawing.Point(31, 189);
            laFindOptions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laFindOptions.Name = "laFindOptions";
            laFindOptions.Size = new System.Drawing.Size(75, 15);
            laFindOptions.TabIndex = 40;
            laFindOptions.Text = "Find Options";
            laFindOptions.Visible = false;
            // 
            // btFindOptions
            // 
            btFindOptions.BackColor = System.Drawing.SystemColors.Control;
            btFindOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            btFindOptions.Font = new System.Drawing.Font("Symbol", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btFindOptions.ImageIndex = 0;
            btFindOptions.Location = new System.Drawing.Point(10, 192);
            btFindOptions.Margin = new System.Windows.Forms.Padding(4);
            btFindOptions.Name = "btFindOptions";
            btFindOptions.Size = new System.Drawing.Size(21, 21);
            btFindOptions.TabIndex = 39;
            btFindOptions.Text = "-";
            btFindOptions.UseVisualStyleBackColor = false;
            btFindOptions.Click += btFindOptions_Click;
            // 
            // laLookIn
            // 
            laLookIn.AutoSize = true;
            laLookIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            laLookIn.Location = new System.Drawing.Point(10, 97);
            laLookIn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laLookIn.Name = "laLookIn";
            laLookIn.Size = new System.Drawing.Size(49, 15);
            laLookIn.TabIndex = 38;
            laLookIn.Text = "Look In:";
            // 
            // cbLookIn
            // 
            cbLookIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbLookIn.FormattingEnabled = true;
            cbLookIn.Location = new System.Drawing.Point(10, 116);
            cbLookIn.Margin = new System.Windows.Forms.Padding(4);
            cbLookIn.Name = "cbLookIn";
            cbLookIn.Size = new System.Drawing.Size(360, 23);
            cbLookIn.TabIndex = 37;
            // 
            // gbFindOptions
            // 
            gbFindOptions.Controls.Add(chbMatchCase);
            gbFindOptions.Controls.Add(chbMatchWholeWord);
            gbFindOptions.Controls.Add(chbUseRegularExpressions);
            gbFindOptions.Location = new System.Drawing.Point(10, 202);
            gbFindOptions.Margin = new System.Windows.Forms.Padding(4);
            gbFindOptions.Name = "gbFindOptions";
            gbFindOptions.Padding = new System.Windows.Forms.Padding(4);
            gbFindOptions.Size = new System.Drawing.Size(360, 100);
            gbFindOptions.TabIndex = 39;
            gbFindOptions.TabStop = false;
            gbFindOptions.Text = "    Find Options";
            // 
            // tbSearch
            // 
            tbSearch.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            tbSearch.Controls.Add(tbUseFind);
            tbSearch.Controls.Add(tbUseReplace);
            tbSearch.Dock = System.Windows.Forms.DockStyle.Top;
            tbSearch.ItemSize = new System.Drawing.Size(80, 22);
            tbSearch.Location = new System.Drawing.Point(0, 0);
            tbSearch.Margin = new System.Windows.Forms.Padding(4);
            tbSearch.Name = "tbSearch";
            tbSearch.SelectedIndex = 0;
            tbSearch.Size = new System.Drawing.Size(380, 28);
            tbSearch.TabIndex = 17;
            tbSearch.Selected += tbSearch_Selected;
            tbSearch.Enter += tbSearch_Enter;
            // 
            // tbUseFind
            // 
            tbUseFind.ImageIndex = 0;
            tbUseFind.Location = new System.Drawing.Point(4, 26);
            tbUseFind.Margin = new System.Windows.Forms.Padding(4);
            tbUseFind.Name = "tbUseFind";
            tbUseFind.Padding = new System.Windows.Forms.Padding(4);
            tbUseFind.Size = new System.Drawing.Size(372, 0);
            tbUseFind.TabIndex = 0;
            tbUseFind.Text = "Find in Files";
            tbUseFind.UseVisualStyleBackColor = true;
            // 
            // tbUseReplace
            // 
            tbUseReplace.ImageIndex = 1;
            tbUseReplace.Location = new System.Drawing.Point(4, 26);
            tbUseReplace.Margin = new System.Windows.Forms.Padding(4);
            tbUseReplace.Name = "tbUseReplace";
            tbUseReplace.Padding = new System.Windows.Forms.Padding(4);
            tbUseReplace.Size = new System.Drawing.Size(372, 0);
            tbUseReplace.TabIndex = 1;
            tbUseReplace.Text = "Replace in Files";
            tbUseReplace.UseVisualStyleBackColor = true;
            tbUseReplace.Enter += tbUseReplace_Enter;
            // 
            // DlgSearchInFiles
            // 
            AcceptButton = btFindNext;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btClose;
            ClientSize = new System.Drawing.Size(380, 409);
            Controls.Add(pnMain);
            Controls.Add(tbSearch);
            Controls.Add(pnButtons);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "DlgSearchInFiles";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Find";
            Load += DlgSearch_Load;
            Shown += DlgSearch_Shown;
            cmReplace.ResumeLayout(false);
            cmFind.ResumeLayout(false);
            pnButtons.ResumeLayout(false);
            pnMain.ResumeLayout(false);
            pnMain.PerformLayout();
            gbFindOptions.ResumeLayout(false);
            tbSearch.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        public System.Windows.Forms.CheckBox chbMatchWholeWord;
        public System.Windows.Forms.Button btReplacePopup;
        public System.Windows.Forms.CheckBox chbMatchCase;
        public System.Windows.Forms.CheckBox chbUseRegularExpressions;
        private System.Windows.Forms.ContextMenuStrip cmReplace;
        private System.Windows.Forms.ToolStripMenuItem miFindWhatText;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression1;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression2;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression3;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression4;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression5;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression6;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression7;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression8;
        private System.Windows.Forms.ToolStripMenuItem miTaggedExpression9;
        public System.Windows.Forms.ToolStripMenuItem miTag;
        public System.Windows.Forms.Label laFindWhat;
        public System.Windows.Forms.ComboBox cbReplaceWith;
        public System.Windows.Forms.Label laReplaceWith;
        public System.Windows.Forms.ToolStripMenuItem miEscape;
        public System.Windows.Forms.Button btReplaceAll;
        public System.Windows.Forms.Button btMarkAll;
        public System.Windows.Forms.Button btReplace;
        public System.Windows.Forms.Button btFindNext;
        public System.Windows.Forms.ContextMenuStrip cmFind;
        public System.Windows.Forms.ToolStripMenuItem miSingleChar;
        public System.Windows.Forms.ToolStripMenuItem miZeroOrMore;
        public System.Windows.Forms.ToolStripMenuItem miOneOrMore;
        public System.Windows.Forms.ToolStripSeparator menuItem4;
        public System.Windows.Forms.ToolStripMenuItem miBeginLine;
        public System.Windows.Forms.ToolStripMenuItem miEndLine;
        private System.Windows.Forms.ToolStripMenuItem miLineBreak;
        public System.Windows.Forms.ToolStripSeparator menuItem10;
        public System.Windows.Forms.ToolStripMenuItem miOneCharInSet;
        public System.Windows.Forms.ToolStripMenuItem miOneCharNotInSet;
        public System.Windows.Forms.ToolStripMenuItem miOr;
        public System.Windows.Forms.Button btPopup;
        public System.Windows.Forms.ComboBox cbFindWhat;
        private System.Windows.Forms.Panel pnButtons;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.GroupBox gbFindOptions;
        public System.Windows.Forms.Button btClose;
        private System.Windows.Forms.ComboBox cbLookIn;
        public System.Windows.Forms.Label laLookIn;
        public System.Windows.Forms.Button btFindOptions;
        private System.Windows.Forms.Label laFindOptions;
        private System.Windows.Forms.Panel pnFindOptions;
        private System.Windows.Forms.TabPage tbUseFind;
        private System.Windows.Forms.TabPage tbUseReplace;
        public System.Windows.Forms.TabControl tbSearch;
        public System.Windows.Forms.Button FindAllButton;
        public System.Windows.Forms.Label FileTypesLabel;
        private System.Windows.Forms.ComboBox FileTypesComboBox;
        public System.Windows.Forms.Button btFindPrevious;
    }
}
