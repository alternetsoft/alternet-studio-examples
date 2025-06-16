namespace Miscellaneous
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnSettings = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbMisc = new System.Windows.Forms.GroupBox();
            this.cbSymbolColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.laSymbolColor = new System.Windows.Forms.Label();
            this.chbWhiteSpaceVisible = new System.Windows.Forms.CheckBox();
            this.chbSeparateLines = new System.Windows.Forms.CheckBox();
            this.cbSpellColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.laSpellColor = new System.Windows.Forms.Label();
            this.chbCheckSpelling = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbBracesColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbFontStyle = new System.Windows.Forms.ComboBox();
            this.laFontStyle = new System.Windows.Forms.Label();
            this.laBracesColor = new System.Windows.Forms.Label();
            this.chbHighlightBounds = new System.Windows.Forms.CheckBox();
            this.chbTempHighlightBraces = new System.Windows.Forms.CheckBox();
            this.chbHighlightBraces = new System.Windows.Forms.CheckBox();
            this.chbUseRoundRect = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBorderStyle = new System.Windows.Forms.ComboBox();
            this.cbGradientEndColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbGradientBeginColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.laGradientEndColor = new System.Windows.Forms.Label();
            this.laGradientBeginColor = new System.Windows.Forms.Label();
            this.laBackgroundStyle = new System.Windows.Forms.Label();
            this.cbBackgroundStyle = new System.Windows.Forms.ComboBox();
            this.chbTransparent = new System.Windows.Forms.CheckBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbMisc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.panel1);
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 244);
            this.pnSettings.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbMisc);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 143);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(657, 96);
            this.panel1.TabIndex = 18;
            // 
            // gbMisc
            // 
            this.gbMisc.Controls.Add(this.cbSymbolColor);
            this.gbMisc.Controls.Add(this.laSymbolColor);
            this.gbMisc.Controls.Add(this.chbWhiteSpaceVisible);
            this.gbMisc.Controls.Add(this.chbSeparateLines);
            this.gbMisc.Controls.Add(this.cbSpellColor);
            this.gbMisc.Controls.Add(this.laSpellColor);
            this.gbMisc.Controls.Add(this.chbCheckSpelling);
            this.gbMisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMisc.Location = new System.Drawing.Point(334, 0);
            this.gbMisc.Name = "gbMisc";
            this.gbMisc.Size = new System.Drawing.Size(323, 96);
            this.gbMisc.TabIndex = 19;
            this.gbMisc.TabStop = false;
            this.gbMisc.Text = "Miscellaneous options";
            // 
            // cbSymbolColor
            // 
            this.cbSymbolColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSymbolColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSymbolColor.FormattingEnabled = true;
            this.cbSymbolColor.Location = new System.Drawing.Point(208, 42);
            this.cbSymbolColor.Name = "cbSymbolColor";
            this.cbSymbolColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSymbolColor.Size = new System.Drawing.Size(107, 21);
            this.cbSymbolColor.TabIndex = 22;
            this.cbSymbolColor.SelectedIndexChanged += new System.EventHandler(this.SymbolColorComboBox_SelectedIndexChanged);
            this.cbSymbolColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SymbolColorComboBox_MouseMove);
            // 
            // laSymbolColor
            // 
            this.laSymbolColor.AutoSize = true;
            this.laSymbolColor.Location = new System.Drawing.Point(131, 45);
            this.laSymbolColor.Name = "laSymbolColor";
            this.laSymbolColor.Size = new System.Drawing.Size(71, 13);
            this.laSymbolColor.TabIndex = 25;
            this.laSymbolColor.Text = "Symbol Color:";
            // 
            // chbWhiteSpaceVisible
            // 
            this.chbWhiteSpaceVisible.AutoSize = true;
            this.chbWhiteSpaceVisible.Location = new System.Drawing.Point(10, 44);
            this.chbWhiteSpaceVisible.Name = "chbWhiteSpaceVisible";
            this.chbWhiteSpaceVisible.Size = new System.Drawing.Size(116, 17);
            this.chbWhiteSpaceVisible.TabIndex = 19;
            this.chbWhiteSpaceVisible.Text = "Display Whitespace";
            this.chbWhiteSpaceVisible.CheckedChanged += new System.EventHandler(this.WhiteSpaceVisibleCheckBox_CheckedChanged);
            this.chbWhiteSpaceVisible.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WhiteSpaceVisibleCheckBox_MouseMove);
            // 
            // chbSeparateLines
            // 
            this.chbSeparateLines.Location = new System.Drawing.Point(10, 70);
            this.chbSeparateLines.Name = "chbSeparateLines";
            this.chbSeparateLines.Size = new System.Drawing.Size(114, 17);
            this.chbSeparateLines.TabIndex = 20;
            this.chbSeparateLines.Text = "Separate Lines";
            this.chbSeparateLines.CheckedChanged += new System.EventHandler(this.SeparateLinesCheckBox_CheckedChanged);
            this.chbSeparateLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SeparateLinesCheckBox_MouseMove);
            // 
            // cbSpellColor
            // 
            this.cbSpellColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSpellColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSpellColor.FormattingEnabled = true;
            this.cbSpellColor.Location = new System.Drawing.Point(208, 17);
            this.cbSpellColor.Name = "cbSpellColor";
            this.cbSpellColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSpellColor.Size = new System.Drawing.Size(107, 21);
            this.cbSpellColor.TabIndex = 21;
            this.cbSpellColor.SelectedIndexChanged += new System.EventHandler(this.SpellColorComboBox_SelectedIndexChanged);
            this.cbSpellColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpellColorComboBox_MouseMove);
            // 
            // laSpellColor
            // 
            this.laSpellColor.AutoSize = true;
            this.laSpellColor.Location = new System.Drawing.Point(142, 20);
            this.laSpellColor.Name = "laSpellColor";
            this.laSpellColor.Size = new System.Drawing.Size(60, 13);
            this.laSpellColor.TabIndex = 21;
            this.laSpellColor.Text = "Spell Color:";
            // 
            // chbCheckSpelling
            // 
            this.chbCheckSpelling.Location = new System.Drawing.Point(10, 19);
            this.chbCheckSpelling.Name = "chbCheckSpelling";
            this.chbCheckSpelling.Size = new System.Drawing.Size(104, 17);
            this.chbCheckSpelling.TabIndex = 18;
            this.chbCheckSpelling.Text = "Check Spelling";
            this.chbCheckSpelling.CheckedChanged += new System.EventHandler(this.CheckSpellingCheckBox_CheckedChanged);
            this.chbCheckSpelling.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CheckSpellingCheckBox_MouseMove);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbBracesColor);
            this.groupBox3.Controls.Add(this.cbFontStyle);
            this.groupBox3.Controls.Add(this.laFontStyle);
            this.groupBox3.Controls.Add(this.laBracesColor);
            this.groupBox3.Controls.Add(this.chbHighlightBounds);
            this.groupBox3.Controls.Add(this.chbTempHighlightBraces);
            this.groupBox3.Controls.Add(this.chbHighlightBraces);
            this.groupBox3.Controls.Add(this.chbUseRoundRect);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(334, 96);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            // 
            // cbBracesColor
            // 
            this.cbBracesColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbBracesColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBracesColor.FormattingEnabled = true;
            this.cbBracesColor.Location = new System.Drawing.Point(204, 70);
            this.cbBracesColor.Name = "cbBracesColor";
            this.cbBracesColor.SelectedColor = System.Drawing.Color.DarkSlateGray;
            this.cbBracesColor.Size = new System.Drawing.Size(124, 21);
            this.cbBracesColor.TabIndex = 17;
            this.cbBracesColor.SelectedIndexChanged += new System.EventHandler(this.BracesColorComboBox_SelectedIndexChanged);
            this.cbBracesColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BracesColorComboBox_MouseMove);
            // 
            // cbFontStyle
            // 
            this.cbFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontStyle.Location = new System.Drawing.Point(204, 42);
            this.cbFontStyle.Name = "cbFontStyle";
            this.cbFontStyle.Size = new System.Drawing.Size(124, 21);
            this.cbFontStyle.TabIndex = 16;
            this.cbFontStyle.SelectedIndexChanged += new System.EventHandler(this.FontStyleComboBox_SelectedIndexChanged);
            this.cbFontStyle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FontStyleComboBox_MouseMove);
            // 
            // laFontStyle
            // 
            this.laFontStyle.AutoSize = true;
            this.laFontStyle.Location = new System.Drawing.Point(145, 45);
            this.laFontStyle.Name = "laFontStyle";
            this.laFontStyle.Size = new System.Drawing.Size(57, 13);
            this.laFontStyle.TabIndex = 13;
            this.laFontStyle.Text = "Font Style:";
            // 
            // laBracesColor
            // 
            this.laBracesColor.AutoSize = true;
            this.laBracesColor.Location = new System.Drawing.Point(145, 70);
            this.laBracesColor.Name = "laBracesColor";
            this.laBracesColor.Size = new System.Drawing.Size(34, 13);
            this.laBracesColor.TabIndex = 14;
            this.laBracesColor.Text = "Color:";
            // 
            // chbHighlightBounds
            // 
            this.chbHighlightBounds.Location = new System.Drawing.Point(14, 19);
            this.chbHighlightBounds.Name = "chbHighlightBounds";
            this.chbHighlightBounds.Size = new System.Drawing.Size(125, 17);
            this.chbHighlightBounds.TabIndex = 13;
            this.chbHighlightBounds.Text = "If caret on the brace";
            this.chbHighlightBounds.CheckedChanged += new System.EventHandler(this.HighlighoundsCheckBoxTextBox_CheckedChanged);
            this.chbHighlightBounds.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlighoundsCheckBoxTextBox_MouseMove);
            // 
            // chbTempHighlightBraces
            // 
            this.chbTempHighlightBraces.Location = new System.Drawing.Point(14, 44);
            this.chbTempHighlightBraces.Name = "chbTempHighlightBraces";
            this.chbTempHighlightBraces.Size = new System.Drawing.Size(112, 17);
            this.chbTempHighlightBraces.TabIndex = 14;
            this.chbTempHighlightBraces.Text = "Temporarily";
            this.chbTempHighlightBraces.CheckedChanged += new System.EventHandler(this.TempHighlighracesCheckBoxTextBox_CheckedChanged);
            this.chbTempHighlightBraces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TempHighlighracesCheckBoxTextBox_MouseMove);
            // 
            // chbHighlightBraces
            // 
            this.chbHighlightBraces.Checked = true;
            this.chbHighlightBraces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbHighlightBraces.Location = new System.Drawing.Point(7, 0);
            this.chbHighlightBraces.Name = "chbHighlightBraces";
            this.chbHighlightBraces.Size = new System.Drawing.Size(158, 17);
            this.chbHighlightBraces.TabIndex = 12;
            this.chbHighlightBraces.Text = "Highlight Matching Braces";
            this.chbHighlightBraces.CheckedChanged += new System.EventHandler(this.HighlighracesCheckBoxTextBox_CheckedChanged);
            this.chbHighlightBraces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlighracesCheckBoxTextBox_MouseMove);
            // 
            // chbUseRoundRect
            // 
            this.chbUseRoundRect.Location = new System.Drawing.Point(145, 19);
            this.chbUseRoundRect.Name = "chbUseRoundRect";
            this.chbUseRoundRect.Size = new System.Drawing.Size(168, 17);
            this.chbUseRoundRect.TabIndex = 15;
            this.chbUseRoundRect.Text = "Draw Frame around braces";
            this.chbUseRoundRect.CheckedChanged += new System.EventHandler(this.UseRoundRectCheckBox_CheckedChanged);
            this.chbUseRoundRect.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UseRoundRectCheckBox_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbBorderStyle);
            this.groupBox1.Controls.Add(this.cbGradientEndColor);
            this.groupBox1.Controls.Add(this.cbGradientBeginColor);
            this.groupBox1.Controls.Add(this.laGradientEndColor);
            this.groupBox1.Controls.Add(this.laGradientBeginColor);
            this.groupBox1.Controls.Add(this.laBackgroundStyle);
            this.groupBox1.Controls.Add(this.cbBackgroundStyle);
            this.groupBox1.Controls.Add(this.chbTransparent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 71);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Border Style:";
            // 
            // cbBorderStyle
            // 
            this.cbBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBorderStyle.Location = new System.Drawing.Point(204, 42);
            this.cbBorderStyle.Name = "cbBorderStyle";
            this.cbBorderStyle.Size = new System.Drawing.Size(124, 21);
            this.cbBorderStyle.TabIndex = 9;
            this.cbBorderStyle.SelectedIndexChanged += new System.EventHandler(this.BorderStyleComboBox_SelectedIndexChanged);
            this.cbBorderStyle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BorderStyleComboBox_MouseMove);
            // 
            // cbGradientEndColor
            // 
            this.cbGradientEndColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGradientEndColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradientEndColor.FormattingEnabled = true;
            this.cbGradientEndColor.Location = new System.Drawing.Point(542, 42);
            this.cbGradientEndColor.Name = "cbGradientEndColor";
            this.cbGradientEndColor.SelectedColor = System.Drawing.Color.DarkSlateGray;
            this.cbGradientEndColor.Size = new System.Drawing.Size(107, 21);
            this.cbGradientEndColor.TabIndex = 11;
            this.cbGradientEndColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GradientEndColorComboBox_MouseMove);
            // 
            // cbGradientBeginColor
            // 
            this.cbGradientBeginColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGradientBeginColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradientBeginColor.FormattingEnabled = true;
            this.cbGradientBeginColor.Location = new System.Drawing.Point(542, 17);
            this.cbGradientBeginColor.Name = "cbGradientBeginColor";
            this.cbGradientBeginColor.SelectedColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.cbGradientBeginColor.Size = new System.Drawing.Size(107, 21);
            this.cbGradientBeginColor.TabIndex = 10;
            this.cbGradientBeginColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GradieneginColorComboBoxTextBox_MouseMove);
            // 
            // laGradientEndColor
            // 
            this.laGradientEndColor.AutoSize = true;
            this.laGradientEndColor.Location = new System.Drawing.Point(437, 45);
            this.laGradientEndColor.Name = "laGradientEndColor";
            this.laGradientEndColor.Size = new System.Drawing.Size(99, 13);
            this.laGradientEndColor.TabIndex = 11;
            this.laGradientEndColor.Text = "Gradient End Color:";
            // 
            // laGradientBeginColor
            // 
            this.laGradientBeginColor.AutoSize = true;
            this.laGradientBeginColor.Location = new System.Drawing.Point(429, 20);
            this.laGradientBeginColor.Name = "laGradientBeginColor";
            this.laGradientBeginColor.Size = new System.Drawing.Size(107, 13);
            this.laGradientBeginColor.TabIndex = 10;
            this.laGradientBeginColor.Text = "Gradient Begin Color:";
            // 
            // laBackgroundStyle
            // 
            this.laBackgroundStyle.AutoSize = true;
            this.laBackgroundStyle.Location = new System.Drawing.Point(11, 20);
            this.laBackgroundStyle.Name = "laBackgroundStyle";
            this.laBackgroundStyle.Size = new System.Drawing.Size(94, 13);
            this.laBackgroundStyle.TabIndex = 9;
            this.laBackgroundStyle.Text = "Background Style:";
            // 
            // cbBackgroundStyle
            // 
            this.cbBackgroundStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackgroundStyle.Items.AddRange(new object[] {
            "Background Image",
            "Gradient",
            "Theme Background"});
            this.cbBackgroundStyle.Location = new System.Drawing.Point(204, 17);
            this.cbBackgroundStyle.Name = "cbBackgroundStyle";
            this.cbBackgroundStyle.Size = new System.Drawing.Size(124, 21);
            this.cbBackgroundStyle.TabIndex = 8;
            this.cbBackgroundStyle.SelectedIndexChanged += new System.EventHandler(this.BackgroundStyleComboBox_SelectedIndexChanged);
            this.cbBackgroundStyle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BackgroundStyleComboBox_MouseMove);
            // 
            // chbTransparent
            // 
            this.chbTransparent.Checked = true;
            this.chbTransparent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTransparent.Location = new System.Drawing.Point(10, 1);
            this.chbTransparent.Name = "chbTransparent";
            this.chbTransparent.Size = new System.Drawing.Size(125, 17);
            this.chbTransparent.TabIndex = 1;
            this.chbTransparent.Text = "Display Background";
            this.chbTransparent.CheckedChanged += new System.EventHandler(this.TransparentCheckBox_CheckedChanged);
            this.chbTransparent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransparentCheckBox_MouseMove);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 67);
            this.pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(657, 67);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = @"Code Editor can display watermarks or background image. Can display white-space symbolx such as spalces, tabs, end-of-line and the end-of-file markers. Supports highlighting of the matching braces. Spell-as-you-type spellchecker intergation with thirt-party spelling engines is supported.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Braces.FontStyle = System.Drawing.FontStyle.Bold;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 244);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.Spelling.CheckSpelling = true;
            this.syntaxEdit1.TabIndex = 8;
            this.syntaxEdit1.Text = "";
            this.syntaxEdit1.Transparent = true;
            this.syntaxEdit1.WordWrap = true;
            this.syntaxEdit1.PaintBackground += new System.Windows.Forms.PaintEventHandler(this.SyntaxEdit1_PaintBackground);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 503);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Miscellaneous";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbMisc.ResumeLayout(false);
            this.gbMisc.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBorderStyle;
        private Alternet.Editor.Common.ColorBox cbGradientEndColor;
        private Alternet.Editor.Common.ColorBox cbGradientBeginColor;
        private System.Windows.Forms.Label laGradientEndColor;
        private System.Windows.Forms.Label laGradientBeginColor;
        private System.Windows.Forms.Label laBackgroundStyle;
        private System.Windows.Forms.ComboBox cbBackgroundStyle;
        private System.Windows.Forms.CheckBox chbTransparent;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbMisc;
        private Alternet.Editor.Common.ColorBox cbSymbolColor;
        private System.Windows.Forms.Label laSymbolColor;
        private System.Windows.Forms.CheckBox chbWhiteSpaceVisible;
        private System.Windows.Forms.CheckBox chbSeparateLines;
        private Alternet.Editor.Common.ColorBox cbSpellColor;
        private System.Windows.Forms.Label laSpellColor;
        private System.Windows.Forms.CheckBox chbCheckSpelling;
        private System.Windows.Forms.GroupBox groupBox3;
        private Alternet.Editor.Common.ColorBox cbBracesColor;
        private System.Windows.Forms.ComboBox cbFontStyle;
        private System.Windows.Forms.Label laFontStyle;
        private System.Windows.Forms.Label laBracesColor;
        private System.Windows.Forms.CheckBox chbHighlightBounds;
        private System.Windows.Forms.CheckBox chbTempHighlightBraces;
        private System.Windows.Forms.CheckBox chbHighlightBraces;
        private System.Windows.Forms.CheckBox chbUseRoundRect;
    }
}