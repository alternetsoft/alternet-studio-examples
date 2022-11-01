namespace WordWrap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Supress for Visual Studio-generated code")]
    partial class Form1
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbWordWrap = new System.Windows.Forms.CheckBox();
            this.chbWrapAtMargin = new System.Windows.Forms.CheckBox();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.labDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.pnlSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.groupBox1);
            this.pnlSettings.Controls.Add(this.pnlDescription);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnlSettings.Size = new System.Drawing.Size(667, 117);
            this.pnlSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbWordWrap);
            this.groupBox1.Controls.Add(this.chbWrapAtMargin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 68);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Word Wrap";
            // 
            // chbWordWrap
            // 
            this.chbWordWrap.AutoSize = true;
            this.chbWordWrap.Location = new System.Drawing.Point(16, 19);
            this.chbWordWrap.Name = "chbWordWrap";
            this.chbWordWrap.Size = new System.Drawing.Size(81, 17);
            this.chbWordWrap.TabIndex = 9;
            this.chbWordWrap.Text = "Word Wrap";
            this.chbWordWrap.CheckedChanged += new System.EventHandler(this.WordWrapCheckBox_CheckedChanged);
            this.chbWordWrap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WordWrapCheckBox_MouseMove);
            // 
            // chbWrapAtMargin
            // 
            this.chbWrapAtMargin.AutoSize = true;
            this.chbWrapAtMargin.Location = new System.Drawing.Point(16, 43);
            this.chbWrapAtMargin.Name = "chbWrapAtMargin";
            this.chbWrapAtMargin.Size = new System.Drawing.Size(99, 17);
            this.chbWrapAtMargin.TabIndex = 10;
            this.chbWrapAtMargin.Text = "Wrap at Margin";
            this.chbWrapAtMargin.CheckedChanged += new System.EventHandler(this.WrapAtMarginCheckBox_CheckedChanged);
            this.chbWrapAtMargin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WrapAtMarginCheckBox_MouseMove);
            // 
            // pnlDescription
            // 
            this.pnlDescription.Controls.Add(this.labDescription);
            this.pnlDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDescription.Location = new System.Drawing.Point(5, 5);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(657, 39);
            this.pnlDescription.TabIndex = 8;
            // 
            // labDescription
            // 
            this.labDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDescription.Location = new System.Drawing.Point(0, 0);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new System.Drawing.Size(507, 39);
            this.labDescription.TabIndex = 1;
            this.labDescription.Text = "Code Editor allows to automatically wrap words to the beginning of the  next line" +
    " when necessary.";
            this.labDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.EditMargin.Position = 60;
            this.syntaxEdit1.EditMargin.Visible = true;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 117);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.AllowOutlining = true;
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 5;
            this.syntaxEdit1.Text = "";
            this.syntaxEdit1.WordWrap = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 376);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnlSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Word Warp";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbWordWrap;
        private System.Windows.Forms.CheckBox chbWrapAtMargin;
        private System.Windows.Forms.Panel pnlDescription;
        private System.Windows.Forms.Label labDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}