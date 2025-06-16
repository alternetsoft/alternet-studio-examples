namespace CodeOutlining
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
            this.gbOutlining = new System.Windows.Forms.GroupBox();
            this.cbAutomatic = new System.Windows.Forms.ComboBox();
            this.chbAllowOutlining = new System.Windows.Forms.CheckBox();
            this.chbDrawButtons = new System.Windows.Forms.CheckBox();
            this.chbDrawLines = new System.Windows.Forms.CheckBox();
            this.chbDrawOnGutter = new System.Windows.Forms.CheckBox();
            this.chbShowHints = new System.Windows.Forms.CheckBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.textSource1 = new Alternet.Editor.TextSource.TextSource(this.components);
            this.textSource2 = new Alternet.Editor.TextSource.TextSource(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.gbOutlining.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.gbOutlining);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 121);
            this.pnSettings.TabIndex = 4;
            // 
            // gbOutlining
            // 
            this.gbOutlining.BackColor = System.Drawing.SystemColors.Control;
            this.gbOutlining.Controls.Add(this.cbAutomatic);
            this.gbOutlining.Controls.Add(this.chbAllowOutlining);
            this.gbOutlining.Controls.Add(this.chbDrawButtons);
            this.gbOutlining.Controls.Add(this.chbDrawLines);
            this.gbOutlining.Controls.Add(this.chbDrawOnGutter);
            this.gbOutlining.Controls.Add(this.chbShowHints);
            this.gbOutlining.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOutlining.Location = new System.Drawing.Point(5, 44);
            this.gbOutlining.Name = "gbOutlining";
            this.gbOutlining.Size = new System.Drawing.Size(657, 72);
            this.gbOutlining.TabIndex = 10;
            this.gbOutlining.TabStop = false;
            this.gbOutlining.Text = "Code Outlining";
            // 
            // cbAutomatic
            // 
            this.cbAutomatic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutomatic.Items.AddRange(new object[] {
            "Automatic",
            "Custom"});
            this.cbAutomatic.Location = new System.Drawing.Point(16, 18);
            this.cbAutomatic.Name = "cbAutomatic";
            this.cbAutomatic.Size = new System.Drawing.Size(96, 21);
            this.cbAutomatic.TabIndex = 0;
            this.cbAutomatic.SelectedIndexChanged += new System.EventHandler(this.AutomaticComboBox_SelectedIndexChanged);
            this.cbAutomatic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AutomaticComboBox_MouseMove);
            // 
            // chbAllowOutlining
            // 
            this.chbAllowOutlining.BackColor = System.Drawing.SystemColors.Control;
            this.chbAllowOutlining.Location = new System.Drawing.Point(16, 40);
            this.chbAllowOutlining.Name = "chbAllowOutlining";
            this.chbAllowOutlining.Size = new System.Drawing.Size(104, 24);
            this.chbAllowOutlining.TabIndex = 1;
            this.chbAllowOutlining.Text = "Allow Outlining";
            this.chbAllowOutlining.UseVisualStyleBackColor = false;
            this.chbAllowOutlining.CheckedChanged += new System.EventHandler(this.AllowOutliningCheckBox_CheckedChanged);
            this.chbAllowOutlining.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AllowOutliningCheckBox_MouseMove);
            // 
            // chbDrawButtons
            // 
            this.chbDrawButtons.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawButtons.Location = new System.Drawing.Point(240, 16);
            this.chbDrawButtons.Name = "chbDrawButtons";
            this.chbDrawButtons.Size = new System.Drawing.Size(104, 24);
            this.chbDrawButtons.TabIndex = 4;
            this.chbDrawButtons.Text = "Draw Buttons";
            this.chbDrawButtons.UseVisualStyleBackColor = false;
            this.chbDrawButtons.CheckedChanged += new System.EventHandler(this.DrawButtonsCheckBox_CheckedChanged);
            this.chbDrawButtons.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawButtonsCheckBox_MouseMove);
            // 
            // chbDrawLines
            // 
            this.chbDrawLines.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawLines.Location = new System.Drawing.Point(128, 40);
            this.chbDrawLines.Name = "chbDrawLines";
            this.chbDrawLines.Size = new System.Drawing.Size(104, 24);
            this.chbDrawLines.TabIndex = 3;
            this.chbDrawLines.Text = "Draw Lines";
            this.chbDrawLines.UseVisualStyleBackColor = false;
            this.chbDrawLines.CheckedChanged += new System.EventHandler(this.DrawLinesCheckBox_CheckedChanged);
            this.chbDrawLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawLinesCheckBox_MouseMove);
            // 
            // chbDrawOnGutter
            // 
            this.chbDrawOnGutter.BackColor = System.Drawing.SystemColors.Control;
            this.chbDrawOnGutter.Location = new System.Drawing.Point(128, 16);
            this.chbDrawOnGutter.Name = "chbDrawOnGutter";
            this.chbDrawOnGutter.Size = new System.Drawing.Size(104, 24);
            this.chbDrawOnGutter.TabIndex = 2;
            this.chbDrawOnGutter.Text = "Draw on Gutter";
            this.chbDrawOnGutter.UseVisualStyleBackColor = false;
            this.chbDrawOnGutter.CheckedChanged += new System.EventHandler(this.DrawOnGutterCheckBox_CheckedChanged);
            this.chbDrawOnGutter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawOnGutterCheckBox_MouseMove);
            // 
            // chbShowHints
            // 
            this.chbShowHints.BackColor = System.Drawing.SystemColors.Control;
            this.chbShowHints.Location = new System.Drawing.Point(240, 40);
            this.chbShowHints.Name = "chbShowHints";
            this.chbShowHints.Size = new System.Drawing.Size(104, 24);
            this.chbShowHints.TabIndex = 5;
            this.chbShowHints.Text = "Display Hints";
            this.chbShowHints.UseVisualStyleBackColor = false;
            this.chbShowHints.CheckedChanged += new System.EventHandler(this.ShowHintsCheckBox_CheckedChanged);
            this.chbShowHints.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowHintsCheckBox_MouseMove);
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 39);
            this.pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(657, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Code outlining is a text navigation feature that can make navigation of large str" +
    "uctured texts more comfortable and effective.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 121);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.AllowOutlining = true;
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.Source = this.textSource1;
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.SourceStateChanged += new Alternet.Editor.NotifyEvent(this.SyntaxEdit1_SourceStateChanged);
            // 
            // textSource1
            // 
            this.textSource1.OptimizedForMemory = false;
            // 
            // textSource2
            // 
            this.textSource2.OptimizedForMemory = false;
            this.textSource2.Text = @"[connect default]
;If we want to disable unknown connect values, we set Access to NoAccess
Access=NoAccess

[sql default]
;If we want to disable unknown sql values, we set Sql to an invalid query.
Sql="" ""

[connect CustomerDatabase]
Access=ReadWrite
Connect=""DSN=AdvWorks""

[sql CustomerById]
Sql=""SELECT * FROM Customers WHERE CustomerID = ?""

[connect AuthorDatabase]
Access=ReadOnly
Connect=""DSN=MyLibraryInfo;UID=MyUserID;PWD=MyPassword""

[userlist AuthorDatabase]
Administrator=ReadWrite

[sql AuthorById]
Sql=""SELECT * FROM Authors WHERE au_id = ?""";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 380);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Outlining";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.gbOutlining.ResumeLayout(false);
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox gbOutlining;
        private System.Windows.Forms.ComboBox cbAutomatic;
        private System.Windows.Forms.CheckBox chbAllowOutlining;
        private System.Windows.Forms.CheckBox chbDrawButtons;
        private System.Windows.Forms.CheckBox chbDrawLines;
        private System.Windows.Forms.CheckBox chbDrawOnGutter;
        private System.Windows.Forms.CheckBox chbShowHints;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private Alternet.Editor.TextSource.TextSource textSource1;
        private Alternet.Editor.TextSource.TextSource textSource2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}