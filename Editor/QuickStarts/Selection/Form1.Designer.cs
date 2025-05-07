namespace Selection
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
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.cbSelectionBorderColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbSelectionBackColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.cbSelectionForeColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.laSelectionBorderColor = new System.Windows.Forms.Label();
            this.laSelectionBackColor = new System.Windows.Forms.Label();
            this.laSelectionForeColor = new System.Windows.Forms.Label();
            this.chbOverwriteBlocks = new System.Windows.Forms.CheckBox();
            this.chbPersistentBlocks = new System.Windows.Forms.CheckBox();
            this.HighlightSelectedWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.chbSelectLineOnDblClick = new System.Windows.Forms.CheckBox();
            this.chbHideSelection = new System.Windows.Forms.CheckBox();
            this.chbUseColors = new System.Windows.Forms.CheckBox();
            this.chbSelectBeyondEol = new System.Windows.Forms.CheckBox();
            this.chbDisableDragging = new System.Windows.Forms.CheckBox();
            this.chbDisableSelection = new System.Windows.Forms.CheckBox();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.gbSelection.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.gbSelection);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 146);
            this.pnSettings.TabIndex = 4;
            // 
            // gbSelection
            // 
            this.gbSelection.Controls.Add(this.cbSelectionBorderColor);
            this.gbSelection.Controls.Add(this.cbSelectionBackColor);
            this.gbSelection.Controls.Add(this.cbSelectionForeColor);
            this.gbSelection.Controls.Add(this.laSelectionBorderColor);
            this.gbSelection.Controls.Add(this.laSelectionBackColor);
            this.gbSelection.Controls.Add(this.laSelectionForeColor);
            this.gbSelection.Controls.Add(this.chbOverwriteBlocks);
            this.gbSelection.Controls.Add(this.chbPersistentBlocks);
            this.gbSelection.Controls.Add(this.HighlightSelectedWordsCheckBox);
            this.gbSelection.Controls.Add(this.chbSelectLineOnDblClick);
            this.gbSelection.Controls.Add(this.chbHideSelection);
            this.gbSelection.Controls.Add(this.chbUseColors);
            this.gbSelection.Controls.Add(this.chbSelectBeyondEol);
            this.gbSelection.Controls.Add(this.chbDisableDragging);
            this.gbSelection.Controls.Add(this.chbDisableSelection);
            this.gbSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelection.Location = new System.Drawing.Point(5, 44);
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.Size = new System.Drawing.Size(657, 97);
            this.gbSelection.TabIndex = 9;
            this.gbSelection.TabStop = false;
            this.gbSelection.Text = "Selection Options";
            // 
            // cbSelectionBorderColor
            // 
            this.cbSelectionBorderColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSelectionBorderColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectionBorderColor.FormattingEnabled = true;
            this.cbSelectionBorderColor.Location = new System.Drawing.Point(529, 66);
            this.cbSelectionBorderColor.Name = "cbSelectionBorderColor";
            this.cbSelectionBorderColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSelectionBorderColor.Size = new System.Drawing.Size(121, 21);
            this.cbSelectionBorderColor.TabIndex = 24;
            this.cbSelectionBorderColor.SelectedIndexChanged += new System.EventHandler(this.SelectionBorderColorComboBox_SelectedIndexChanged);
            this.cbSelectionBorderColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectionBorderColorComboBox_MouseMove);
            // 
            // cbSelectionBackColor
            // 
            this.cbSelectionBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSelectionBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectionBackColor.FormattingEnabled = true;
            this.cbSelectionBackColor.Location = new System.Drawing.Point(529, 42);
            this.cbSelectionBackColor.Name = "cbSelectionBackColor";
            this.cbSelectionBackColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSelectionBackColor.Size = new System.Drawing.Size(121, 21);
            this.cbSelectionBackColor.TabIndex = 23;
            this.cbSelectionBackColor.SelectedIndexChanged += new System.EventHandler(this.SelectionBackColorComboBox_SelectedIndexChanged);
            this.cbSelectionBackColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectionBackColorComboBox_MouseMove);
            // 
            // cbSelectionForeColor
            // 
            this.cbSelectionForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSelectionForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectionForeColor.FormattingEnabled = true;
            this.cbSelectionForeColor.Location = new System.Drawing.Point(529, 18);
            this.cbSelectionForeColor.Name = "cbSelectionForeColor";
            this.cbSelectionForeColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbSelectionForeColor.Size = new System.Drawing.Size(121, 21);
            this.cbSelectionForeColor.TabIndex = 22;
            this.cbSelectionForeColor.SelectedIndexChanged += new System.EventHandler(this.SelectionForeColorComboBox_SelectedIndexChanged);
            this.cbSelectionForeColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectionForeColorComboBox_MouseMove);
            // 
            // laSelectionBorderColor
            // 
            this.laSelectionBorderColor.AutoSize = true;
            this.laSelectionBorderColor.Location = new System.Drawing.Point(454, 69);
            this.laSelectionBorderColor.Name = "laSelectionBorderColor";
            this.laSelectionBorderColor.Size = new System.Drawing.Size(68, 13);
            this.laSelectionBorderColor.TabIndex = 21;
            this.laSelectionBorderColor.Text = "Border Color:";
            // 
            // laSelectionBackColor
            // 
            this.laSelectionBackColor.AutoSize = true;
            this.laSelectionBackColor.Location = new System.Drawing.Point(454, 45);
            this.laSelectionBackColor.Name = "laSelectionBackColor";
            this.laSelectionBackColor.Size = new System.Drawing.Size(62, 13);
            this.laSelectionBackColor.TabIndex = 20;
            this.laSelectionBackColor.Text = "Back Color:";
            // 
            // laSelectionForeColor
            // 
            this.laSelectionForeColor.AutoSize = true;
            this.laSelectionForeColor.Location = new System.Drawing.Point(454, 21);
            this.laSelectionForeColor.Name = "laSelectionForeColor";
            this.laSelectionForeColor.Size = new System.Drawing.Size(58, 13);
            this.laSelectionForeColor.TabIndex = 19;
            this.laSelectionForeColor.Text = "Fore Color:";
            // 
            // chbOverwriteBlocks
            // 
            this.chbOverwriteBlocks.AutoSize = true;
            this.chbOverwriteBlocks.Location = new System.Drawing.Point(308, 68);
            this.chbOverwriteBlocks.Name = "chbOverwriteBlocks";
            this.chbOverwriteBlocks.Size = new System.Drawing.Size(106, 17);
            this.chbOverwriteBlocks.TabIndex = 13;
            this.chbOverwriteBlocks.Text = "Overwrite Blocks";
            this.chbOverwriteBlocks.CheckedChanged += new System.EventHandler(this.OverwriteBlocksCheckBox_CheckedChanged);
            this.chbOverwriteBlocks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OverwriteBlocksCheckBox_MouseMove);
            // 
            // chbPersistentBlocks
            // 
            this.chbPersistentBlocks.AutoSize = true;
            this.chbPersistentBlocks.Location = new System.Drawing.Point(308, 44);
            this.chbPersistentBlocks.Name = "chbPersistentBlocks";
            this.chbPersistentBlocks.Size = new System.Drawing.Size(107, 17);
            this.chbPersistentBlocks.TabIndex = 12;
            this.chbPersistentBlocks.Text = "Persistent Blocks";
            this.chbPersistentBlocks.CheckedChanged += new System.EventHandler(this.PersistenlocksCheckBoxTextBox_CheckedChanged);
            this.chbPersistentBlocks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PersistenlocksCheckBoxTextBox_MouseMove);
            // 
            // HighlightSelectedWordsCheckBox
            // 
            this.HighlightSelectedWordsCheckBox.AutoSize = true;
            this.HighlightSelectedWordsCheckBox.Location = new System.Drawing.Point(308, 20);
            this.HighlightSelectedWordsCheckBox.Name = "HighlightSelectedWordsCheckBox";
            this.HighlightSelectedWordsCheckBox.Size = new System.Drawing.Size(110, 17);
            this.HighlightSelectedWordsCheckBox.TabIndex = 11;
            this.HighlightSelectedWordsCheckBox.Text = "Highlight Selected Words";
            this.HighlightSelectedWordsCheckBox.CheckedChanged += new System.EventHandler(this.HighlightSelectedWordsCheckBox_CheckedChanged);
            this.HighlightSelectedWordsCheckBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HighlightSelectedWordsCheckBox_MouseMove);
            // 
            // chbSelectLineOnDblClick
            // 
            this.chbSelectLineOnDblClick.AutoSize = true;
            this.chbSelectLineOnDblClick.Location = new System.Drawing.Point(144, 68);
            this.chbSelectLineOnDblClick.Name = "chbSelectLineOnDblClick";
            this.chbSelectLineOnDblClick.Size = new System.Drawing.Size(157, 17);
            this.chbSelectLineOnDblClick.TabIndex = 10;
            this.chbSelectLineOnDblClick.Text = "Select Line on Double Click";
            this.chbSelectLineOnDblClick.CheckedChanged += new System.EventHandler(this.SelectLineOnDblClickCheckBox_CheckedChanged);
            this.chbSelectLineOnDblClick.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectLineOnDblClickCheckBox_MouseMove);
            // 
            // chbHideSelection
            // 
            this.chbHideSelection.AutoSize = true;
            this.chbHideSelection.Location = new System.Drawing.Point(144, 44);
            this.chbHideSelection.Name = "chbHideSelection";
            this.chbHideSelection.Size = new System.Drawing.Size(95, 17);
            this.chbHideSelection.TabIndex = 9;
            this.chbHideSelection.Text = "Hide Selection";
            this.chbHideSelection.CheckedChanged += new System.EventHandler(this.HideSelectionCheckBox_CheckedChanged);
            this.chbHideSelection.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HideSelectionCheckBox_MouseMove);
            // 
            // chbUseColors
            // 
            this.chbUseColors.AutoSize = true;
            this.chbUseColors.Location = new System.Drawing.Point(144, 20);
            this.chbUseColors.Name = "chbUseColors";
            this.chbUseColors.Size = new System.Drawing.Size(77, 17);
            this.chbUseColors.TabIndex = 8;
            this.chbUseColors.Text = "Use Colors";
            this.chbUseColors.CheckedChanged += new System.EventHandler(this.UseColorsCheckBox_CheckedChanged);
            this.chbUseColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UseColorsCheckBox_MouseMove);
            // 
            // chbSelectBeyondEol
            // 
            this.chbSelectBeyondEol.AutoSize = true;
            this.chbSelectBeyondEol.Location = new System.Drawing.Point(8, 68);
            this.chbSelectBeyondEol.Name = "chbSelectBeyondEol";
            this.chbSelectBeyondEol.Size = new System.Drawing.Size(119, 17);
            this.chbSelectBeyondEol.TabIndex = 7;
            this.chbSelectBeyondEol.Text = "Select Beyond EOL";
            this.chbSelectBeyondEol.CheckedChanged += new System.EventHandler(this.SeleceyondEolCheckBoxTextBox_CheckedChanged);
            this.chbSelectBeyondEol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SeleceyondEolCheckBoxTextBox_MouseMove);
            // 
            // chbDisableDragging
            // 
            this.chbDisableDragging.AutoSize = true;
            this.chbDisableDragging.Location = new System.Drawing.Point(8, 44);
            this.chbDisableDragging.Name = "chbDisableDragging";
            this.chbDisableDragging.Size = new System.Drawing.Size(107, 17);
            this.chbDisableDragging.TabIndex = 6;
            this.chbDisableDragging.Text = "Disable Dragging";
            this.chbDisableDragging.CheckedChanged += new System.EventHandler(this.DisableDraggingCheckBox_CheckedChanged);
            this.chbDisableDragging.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisableDraggingCheckBox_MouseMove);
            // 
            // chbDisableSelection
            // 
            this.chbDisableSelection.AutoSize = true;
            this.chbDisableSelection.Location = new System.Drawing.Point(8, 20);
            this.chbDisableSelection.Name = "chbDisableSelection";
            this.chbDisableSelection.Size = new System.Drawing.Size(108, 17);
            this.chbDisableSelection.TabIndex = 5;
            this.chbDisableSelection.Text = "Disable Selection";
            this.chbDisableSelection.CheckedChanged += new System.EventHandler(this.DisableSelectionCheckBox_CheckedChanged);
            this.chbDisableSelection.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisableSelectionCheckBox_MouseMove);
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
            this.laDescription.Text = "This demo shows how to customize appearance and behavior of selected text within " +
    "the edit control content";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 146);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 405);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Selection";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.gbSelection.ResumeLayout(false);
            this.gbSelection.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.CheckBox chbOverwriteBlocks;
        private System.Windows.Forms.CheckBox chbPersistentBlocks;
        private System.Windows.Forms.CheckBox HighlightSelectedWordsCheckBox;
        private System.Windows.Forms.CheckBox chbSelectLineOnDblClick;
        private System.Windows.Forms.CheckBox chbHideSelection;
        private System.Windows.Forms.CheckBox chbUseColors;
        private System.Windows.Forms.CheckBox chbSelectBeyondEol;
        private System.Windows.Forms.CheckBox chbDisableDragging;
        private System.Windows.Forms.CheckBox chbDisableSelection;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label laSelectionBorderColor;
        private System.Windows.Forms.Label laSelectionBackColor;
        private System.Windows.Forms.Label laSelectionForeColor;
        private Alternet.Editor.Common.ColorBox cbSelectionBorderColor;
        private Alternet.Editor.Common.ColorBox cbSelectionBackColor;
        private Alternet.Editor.Common.ColorBox cbSelectionForeColor;
    }
}