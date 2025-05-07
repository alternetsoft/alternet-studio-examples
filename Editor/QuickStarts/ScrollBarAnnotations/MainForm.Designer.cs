namespace ScrollBarAnnotations
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.changeErrorsAppearanceCheckBox = new System.Windows.Forms.CheckBox();
            this.customAnnotationsCheckBox = new System.Windows.Forms.CheckBox();
            this.scrollBarsVisualStyleComboBox = new System.Windows.Forms.ComboBox();
            this.separatorTextBox = new System.Windows.Forms.TextBox();
            this.scrollBarsVisualStyleLabel = new System.Windows.Forms.Label();
            this.customTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.cursorPositionTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.syntaxErrorsTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.searchResultsTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.bookmarksTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.changedLinesTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.scrollBarAnnotationsEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.syntaxEdit = new Alternet.Editor.SyntaxEdit(this.components);
            this.csParser1 = new Alternet.Syntax.Parsers.Roslyn.CsParser();
            this.textSource = new Alternet.Editor.TextSource.TextSource(this.components);
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.changeErrorsAppearanceCheckBox);
            this.controlsPanel.Controls.Add(this.customAnnotationsCheckBox);
            this.controlsPanel.Controls.Add(this.scrollBarsVisualStyleComboBox);
            this.controlsPanel.Controls.Add(this.separatorTextBox);
            this.controlsPanel.Controls.Add(this.scrollBarsVisualStyleLabel);
            this.controlsPanel.Controls.Add(this.customTypeCheckBox);
            this.controlsPanel.Controls.Add(this.cursorPositionTypeCheckBox);
            this.controlsPanel.Controls.Add(this.syntaxErrorsTypeCheckBox);
            this.controlsPanel.Controls.Add(this.searchResultsTypeCheckBox);
            this.controlsPanel.Controls.Add(this.bookmarksTypeCheckBox);
            this.controlsPanel.Controls.Add(this.changedLinesTypeCheckBox);
            this.controlsPanel.Controls.Add(this.scrollBarAnnotationsEnabledCheckBox);
            this.controlsPanel.Controls.Add(this.saveButton);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.controlsPanel.Location = new System.Drawing.Point(0, 0);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new System.Drawing.Size(208, 598);
            this.controlsPanel.TabIndex = 1;
            // 
            // changeErrorsAppearanceCheckBox
            // 
            this.changeErrorsAppearanceCheckBox.AutoSize = true;
            this.changeErrorsAppearanceCheckBox.Location = new System.Drawing.Point(17, 272);
            this.changeErrorsAppearanceCheckBox.Name = "changeErrorsAppearanceCheckBox";
            this.changeErrorsAppearanceCheckBox.Size = new System.Drawing.Size(185, 17);
            this.changeErrorsAppearanceCheckBox.TabIndex = 12;
            this.changeErrorsAppearanceCheckBox.Text = "Change Errors Appearance Demo";
            this.changeErrorsAppearanceCheckBox.UseVisualStyleBackColor = true;
            this.changeErrorsAppearanceCheckBox.CheckedChanged += new System.EventHandler(this.ChangeErrorsAppearanceCheckBox_CheckedChanged);
            // 
            // customAnnotationsCheckBox
            // 
            this.customAnnotationsCheckBox.AutoSize = true;
            this.customAnnotationsCheckBox.Location = new System.Drawing.Point(17, 250);
            this.customAnnotationsCheckBox.Name = "customAnnotationsCheckBox";
            this.customAnnotationsCheckBox.Size = new System.Drawing.Size(151, 17);
            this.customAnnotationsCheckBox.TabIndex = 11;
            this.customAnnotationsCheckBox.Text = "Custom Annotations Demo";
            this.customAnnotationsCheckBox.UseVisualStyleBackColor = true;
            this.customAnnotationsCheckBox.CheckedChanged += new System.EventHandler(this.CustomAnnotationsCheckBox_CheckedChanged);
            // 
            // scrollBarsVisualStyleComboBox
            // 
            this.scrollBarsVisualStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scrollBarsVisualStyleComboBox.FormattingEnabled = true;
            this.scrollBarsVisualStyleComboBox.Location = new System.Drawing.Point(17, 214);
            this.scrollBarsVisualStyleComboBox.Name = "scrollBarsVisualStyleComboBox";
            this.scrollBarsVisualStyleComboBox.Size = new System.Drawing.Size(169, 21);
            this.scrollBarsVisualStyleComboBox.TabIndex = 10;
            this.scrollBarsVisualStyleComboBox.SelectedIndexChanged += new System.EventHandler(this.ScrolarsVisualStyleComboBoxLisoxTextBox_SelectedIndexChanged);
            // 
            // separatorTextBox
            // 
            this.separatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.separatorTextBox.Enabled = false;
            this.separatorTextBox.Location = new System.Drawing.Point(17, 175);
            this.separatorTextBox.Multiline = true;
            this.separatorTextBox.Name = "separatorTextBox";
            this.separatorTextBox.Size = new System.Drawing.Size(169, 1);
            this.separatorTextBox.TabIndex = 9;
            // 
            // scrollBarsVisualStyleLabel
            // 
            this.scrollBarsVisualStyleLabel.AutoSize = true;
            this.scrollBarsVisualStyleLabel.Location = new System.Drawing.Point(15, 194);
            this.scrollBarsVisualStyleLabel.Name = "scrollBarsVisualStyleLabel";
            this.scrollBarsVisualStyleLabel.Size = new System.Drawing.Size(117, 13);
            this.scrollBarsVisualStyleLabel.TabIndex = 8;
            this.scrollBarsVisualStyleLabel.Text = "Scroll Bars Visual Style:";
            // 
            // customTypeCheckBox
            // 
            this.customTypeCheckBox.AutoSize = true;
            this.customTypeCheckBox.Checked = true;
            this.customTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customTypeCheckBox.Location = new System.Drawing.Point(40, 144);
            this.customTypeCheckBox.Name = "customTypeCheckBox";
            this.customTypeCheckBox.Size = new System.Drawing.Size(61, 17);
            this.customTypeCheckBox.TabIndex = 7;
            this.customTypeCheckBox.Text = "Custom";
            this.customTypeCheckBox.UseVisualStyleBackColor = true;
            this.customTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // cursorPositionTypeCheckBox
            // 
            this.cursorPositionTypeCheckBox.AutoSize = true;
            this.cursorPositionTypeCheckBox.Checked = true;
            this.cursorPositionTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cursorPositionTypeCheckBox.Location = new System.Drawing.Point(40, 123);
            this.cursorPositionTypeCheckBox.Name = "cursorPositionTypeCheckBox";
            this.cursorPositionTypeCheckBox.Size = new System.Drawing.Size(96, 17);
            this.cursorPositionTypeCheckBox.TabIndex = 6;
            this.cursorPositionTypeCheckBox.Text = "Cursor Position";
            this.cursorPositionTypeCheckBox.UseVisualStyleBackColor = true;
            this.cursorPositionTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // syntaxErrorsTypeCheckBox
            // 
            this.syntaxErrorsTypeCheckBox.AutoSize = true;
            this.syntaxErrorsTypeCheckBox.Checked = true;
            this.syntaxErrorsTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.syntaxErrorsTypeCheckBox.Location = new System.Drawing.Point(40, 101);
            this.syntaxErrorsTypeCheckBox.Name = "syntaxErrorsTypeCheckBox";
            this.syntaxErrorsTypeCheckBox.Size = new System.Drawing.Size(88, 17);
            this.syntaxErrorsTypeCheckBox.TabIndex = 5;
            this.syntaxErrorsTypeCheckBox.Text = "Syntax Errors";
            this.syntaxErrorsTypeCheckBox.UseVisualStyleBackColor = true;
            this.syntaxErrorsTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // searchResultsTypeCheckBox
            // 
            this.searchResultsTypeCheckBox.AutoSize = true;
            this.searchResultsTypeCheckBox.Checked = true;
            this.searchResultsTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.searchResultsTypeCheckBox.Location = new System.Drawing.Point(40, 79);
            this.searchResultsTypeCheckBox.Name = "searchResultsTypeCheckBox";
            this.searchResultsTypeCheckBox.Size = new System.Drawing.Size(98, 17);
            this.searchResultsTypeCheckBox.TabIndex = 4;
            this.searchResultsTypeCheckBox.Text = "Search Results";
            this.searchResultsTypeCheckBox.UseVisualStyleBackColor = true;
            this.searchResultsTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // bookmarksTypeCheckBox
            // 
            this.bookmarksTypeCheckBox.AutoSize = true;
            this.bookmarksTypeCheckBox.Checked = true;
            this.bookmarksTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bookmarksTypeCheckBox.Location = new System.Drawing.Point(40, 58);
            this.bookmarksTypeCheckBox.Name = "bookmarksTypeCheckBox";
            this.bookmarksTypeCheckBox.Size = new System.Drawing.Size(79, 17);
            this.bookmarksTypeCheckBox.TabIndex = 3;
            this.bookmarksTypeCheckBox.Text = "Bookmarks";
            this.bookmarksTypeCheckBox.UseVisualStyleBackColor = true;
            this.bookmarksTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // changedLinesTypeCheckBox
            // 
            this.changedLinesTypeCheckBox.AutoSize = true;
            this.changedLinesTypeCheckBox.Checked = true;
            this.changedLinesTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.changedLinesTypeCheckBox.Location = new System.Drawing.Point(40, 36);
            this.changedLinesTypeCheckBox.Name = "changedLinesTypeCheckBox";
            this.changedLinesTypeCheckBox.Size = new System.Drawing.Size(97, 17);
            this.changedLinesTypeCheckBox.TabIndex = 2;
            this.changedLinesTypeCheckBox.Text = "Changed Lines";
            this.changedLinesTypeCheckBox.UseVisualStyleBackColor = true;
            this.changedLinesTypeCheckBox.CheckedChanged += new System.EventHandler(this.AnnotationTypeCheckBox_CheckedChanged);
            // 
            // scrollBarAnnotationsEnabledCheckBox
            // 
            this.scrollBarAnnotationsEnabledCheckBox.AutoSize = true;
            this.scrollBarAnnotationsEnabledCheckBox.Checked = true;
            this.scrollBarAnnotationsEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scrollBarAnnotationsEnabledCheckBox.Location = new System.Drawing.Point(17, 14);
            this.scrollBarAnnotationsEnabledCheckBox.Name = "scrollBarAnnotationsEnabledCheckBox";
            this.scrollBarAnnotationsEnabledCheckBox.Size = new System.Drawing.Size(172, 17);
            this.scrollBarAnnotationsEnabledCheckBox.TabIndex = 1;
            this.scrollBarAnnotationsEnabledCheckBox.Text = "Scroll Bar Annotations Enabled";
            this.scrollBarAnnotationsEnabledCheckBox.UseVisualStyleBackColor = true;
            this.scrollBarAnnotationsEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ScrolarAnnotationsEnabledCheckBoxLisoxTextBox_CheckedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(17, 304);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(107, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save Text Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // syntaxEdit
            // 
            this.syntaxEdit.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit.Lexer = this.csParser1;
            this.syntaxEdit.Location = new System.Drawing.Point(208, 0);
            this.syntaxEdit.Name = "syntaxEdit";
            this.syntaxEdit.Scrolling.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.syntaxEdit.Size = new System.Drawing.Size(693, 598);
            this.syntaxEdit.Source = this.textSource;
            this.syntaxEdit.TabIndex = 0;
            // 
            // csParser1
            // 
            this.csParser1.Options = ((Alternet.Syntax.SyntaxOptions)((((((((((Alternet.Syntax.SyntaxOptions.Outline | Alternet.Syntax.SyntaxOptions.SmartIndent) 
            | Alternet.Syntax.SyntaxOptions.CodeCompletion) 
            | Alternet.Syntax.SyntaxOptions.SyntaxErrors) 
            | Alternet.Syntax.SyntaxOptions.QuickInfoTips) 
            | Alternet.Syntax.SyntaxOptions.AutoComplete) 
            | Alternet.Syntax.SyntaxOptions.FormatCase) 
            | Alternet.Syntax.SyntaxOptions.FormatSpaces) 
            | Alternet.Syntax.SyntaxOptions.EvaluateConditionals) 
            | Alternet.Syntax.SyntaxOptions.NotifyOnParse)));
            this.csParser1.ParseInterval = 200;
            this.csParser1.XmlScheme = @"<?xml version=""1.0"" encoding=""utf-16""?>
<LexScheme xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Author>AlterNET Software</Author>
  <Copyright>Copyright (c) 2016-2025 Alternet Software</Copyright>
  <FileType>c#</FileType>
  <Version>1.5</Version>
  <Styles>
    <Style>
      <Name>idents</Name>
      <ForeColor>ControlText</ForeColor>
    </Style>
    <Style>
      <Name>numbers</Name>
      <ForeColor>ControlText</ForeColor>
    </Style>
    <Style>
      <Name>reswords</Name>
      <ForeColor>Blue</ForeColor>
    </Style>
    <Style>
      <Name>comments</Name>
      <ForeColor>Green</ForeColor>
      <PlainText>true</PlainText>
    </Style>
    <Style>
      <Name>xmlcomments</Name>
      <ForeColor>Gray</ForeColor>
    </Style>
    <Style>
      <Name>symbols</Name>
      <ForeColor>WindowText</ForeColor>
    </Style>
    <Style>
      <Name>whitespace</Name>
      <ForeColor>WindowText</ForeColor>
    </Style>
    <Style>
      <Name>strings</Name>
      <ForeColor>Maroon</ForeColor>
      <PlainText>true</PlainText>
    </Style>
    <Style>
      <Name>directives</Name>
      <ForeColor>Blue</ForeColor>
    </Style>
    <Style>
      <Name>htmlparams</Name>
      <ForeColor>Red</ForeColor>
    </Style>
    <Style>
      <Name>syntax errors</Name>
      <ForeColor>Red</ForeColor>
    </Style>
    <Style>
      <Name>code snippets</Name>
      <ForeColor>Black</ForeColor>
      <BackColor>255:180:228:180</BackColor>
    </Style>
    <Style>
      <Name>Types</Name>
      <ForeColor>Teal</ForeColor>
    </Style>
    <Style>
      <Name>Warnings</Name>
      <ForeColor>Navy</ForeColor>
    </Style>
    <Style>
      <Name>XmlParams</Name>
      <ForeColor>Black</ForeColor>
    </Style>
  </Styles>
</LexScheme>";
            // 
            // textSource
            // 
            this.textSource.Lexer = this.csParser1;
            this.textSource.OptimizedForMemory = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 598);
            this.Controls.Add(this.syntaxEdit);
            this.Controls.Add(this.controlsPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scroll Bar Annotations";
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Alternet.Editor.SyntaxEdit syntaxEdit;
        private Alternet.Syntax.Parsers.Roslyn.CsParser csParser1;
        private Alternet.Editor.TextSource.TextSource textSource;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox scrollBarAnnotationsEnabledCheckBox;
        private System.Windows.Forms.CheckBox changedLinesTypeCheckBox;
        private System.Windows.Forms.CheckBox searchResultsTypeCheckBox;
        private System.Windows.Forms.CheckBox bookmarksTypeCheckBox;
        private System.Windows.Forms.CheckBox syntaxErrorsTypeCheckBox;
        private System.Windows.Forms.CheckBox cursorPositionTypeCheckBox;
        private System.Windows.Forms.CheckBox customTypeCheckBox;
        private System.Windows.Forms.TextBox separatorTextBox;
        private System.Windows.Forms.Label scrollBarsVisualStyleLabel;
        private System.Windows.Forms.ComboBox scrollBarsVisualStyleComboBox;
        private System.Windows.Forms.CheckBox customAnnotationsCheckBox;
        private System.Windows.Forms.CheckBox changeErrorsAppearanceCheckBox;
    }
}
