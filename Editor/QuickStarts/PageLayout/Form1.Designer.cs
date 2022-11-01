namespace PageLayout
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
            this.gbRulers = new System.Windows.Forms.GroupBox();
            this.chbVertRuler = new System.Windows.Forms.CheckBox();
            this.chbHorzRuler = new System.Windows.Forms.CheckBox();
            this.cbRulerUnits = new System.Windows.Forms.ComboBox();
            this.laRulerUnits = new System.Windows.Forms.Label();
            this.chbRulerDisplayDragLines = new System.Windows.Forms.CheckBox();
            this.chbRulerDiscrete = new System.Windows.Forms.CheckBox();
            this.chbRulerAllowDrag = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbPages = new System.Windows.Forms.GroupBox();
            this.cbPageSize = new System.Windows.Forms.ComboBox();
            this.laPageSize = new System.Windows.Forms.Label();
            this.cbPageLayout = new System.Windows.Forms.ComboBox();
            this.laPageLayout = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.gbRulers.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbPages.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.gbRulers);
            this.pnSettings.Controls.Add(this.panel1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 143);
            this.pnSettings.TabIndex = 4;
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
            this.gbRulers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRulers.Location = new System.Drawing.Point(205, 44);
            this.gbRulers.Name = "gbRulers";
            this.gbRulers.Size = new System.Drawing.Size(457, 94);
            this.gbRulers.TabIndex = 16;
            this.gbRulers.TabStop = false;
            this.gbRulers.Text = "Rulers";
            // 
            // chbVertRuler
            // 
            this.chbVertRuler.AutoSize = true;
            this.chbVertRuler.Location = new System.Drawing.Point(16, 40);
            this.chbVertRuler.Name = "chbVertRuler";
            this.chbVertRuler.Size = new System.Drawing.Size(89, 17);
            this.chbVertRuler.TabIndex = 2;
            this.chbVertRuler.Text = "Vertical Ruler";
            this.chbVertRuler.CheckedChanged += new System.EventHandler(this.VertRulerCheckBox_CheckedChanged);
            this.chbVertRuler.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VertRulerCheckBox_MouseMove);
            // 
            // chbHorzRuler
            // 
            this.chbHorzRuler.AutoSize = true;
            this.chbHorzRuler.Location = new System.Drawing.Point(16, 16);
            this.chbHorzRuler.Name = "chbHorzRuler";
            this.chbHorzRuler.Size = new System.Drawing.Size(101, 17);
            this.chbHorzRuler.TabIndex = 1;
            this.chbHorzRuler.Text = "Horizontal Ruler";
            this.chbHorzRuler.CheckedChanged += new System.EventHandler(this.HorzRulerCheckBox_CheckedChanged);
            this.chbHorzRuler.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HorzRulerCheckBox_MouseMove);
            // 
            // cbRulerUnits
            // 
            this.cbRulerUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRulerUnits.Location = new System.Drawing.Point(78, 63);
            this.cbRulerUnits.Name = "cbRulerUnits";
            this.cbRulerUnits.Size = new System.Drawing.Size(96, 21);
            this.cbRulerUnits.TabIndex = 4;
            this.cbRulerUnits.SelectedIndexChanged += new System.EventHandler(this.RulerUnitsComboBox_SelectedIndexChanged);
            this.cbRulerUnits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerUnitsComboBox_MouseMove);
            // 
            // laRulerUnits
            // 
            this.laRulerUnits.AutoSize = true;
            this.laRulerUnits.Location = new System.Drawing.Point(12, 67);
            this.laRulerUnits.Name = "laRulerUnits";
            this.laRulerUnits.Size = new System.Drawing.Size(62, 13);
            this.laRulerUnits.TabIndex = 3;
            this.laRulerUnits.Text = "Ruler Units:";
            // 
            // chbRulerDisplayDragLines
            // 
            this.chbRulerDisplayDragLines.AutoSize = true;
            this.chbRulerDisplayDragLines.Location = new System.Drawing.Point(202, 65);
            this.chbRulerDisplayDragLines.Name = "chbRulerDisplayDragLines";
            this.chbRulerDisplayDragLines.Size = new System.Drawing.Size(114, 17);
            this.chbRulerDisplayDragLines.TabIndex = 7;
            this.chbRulerDisplayDragLines.Text = "Display Drag Lines";
            this.chbRulerDisplayDragLines.CheckedChanged += new System.EventHandler(this.RulerDisplayDragLinesCheckBox_CheckedChanged);
            this.chbRulerDisplayDragLines.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerDisplayDragLinesCheckBox_MouseMove);
            // 
            // chbRulerDiscrete
            // 
            this.chbRulerDiscrete.Location = new System.Drawing.Point(202, 40);
            this.chbRulerDiscrete.Name = "chbRulerDiscrete";
            this.chbRulerDiscrete.Size = new System.Drawing.Size(104, 17);
            this.chbRulerDiscrete.TabIndex = 6;
            this.chbRulerDiscrete.Text = "Discrete Drag";
            this.chbRulerDiscrete.CheckedChanged += new System.EventHandler(this.RulerDiscreteCheckBox_CheckedChanged);
            this.chbRulerDiscrete.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerDiscreteCheckBox_MouseMove);
            // 
            // chbRulerAllowDrag
            // 
            this.chbRulerAllowDrag.AutoSize = true;
            this.chbRulerAllowDrag.Location = new System.Drawing.Point(202, 16);
            this.chbRulerAllowDrag.Name = "chbRulerAllowDrag";
            this.chbRulerAllowDrag.Size = new System.Drawing.Size(77, 17);
            this.chbRulerAllowDrag.TabIndex = 5;
            this.chbRulerAllowDrag.Text = "Allow Drag";
            this.chbRulerAllowDrag.CheckedChanged += new System.EventHandler(this.RulerAllowDragCheckBox_CheckedChanged);
            this.chbRulerAllowDrag.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RulerAllowDragCheckBox_MouseMove);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbPages);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(5, 44);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panel1.Size = new System.Drawing.Size(200, 94);
            this.panel1.TabIndex = 17;
            // 
            // gbPages
            // 
            this.gbPages.Controls.Add(this.cbPageSize);
            this.gbPages.Controls.Add(this.laPageSize);
            this.gbPages.Controls.Add(this.cbPageLayout);
            this.gbPages.Controls.Add(this.laPageLayout);
            this.gbPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPages.Location = new System.Drawing.Point(0, 0);
            this.gbPages.Name = "gbPages";
            this.gbPages.Size = new System.Drawing.Size(195, 94);
            this.gbPages.TabIndex = 15;
            this.gbPages.TabStop = false;
            this.gbPages.Text = "Pages";
            // 
            // cbPageSize
            // 
            this.cbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPageSize.Location = new System.Drawing.Point(80, 48);
            this.cbPageSize.Name = "cbPageSize";
            this.cbPageSize.Size = new System.Drawing.Size(96, 21);
            this.cbPageSize.TabIndex = 4;
            this.cbPageSize.SelectedIndexChanged += new System.EventHandler(this.PageSizeComboBox_SelectedIndexChanged);
            this.cbPageSize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageSizeComboBox_MouseMove);
            // 
            // laPageSize
            // 
            this.laPageSize.AutoSize = true;
            this.laPageSize.Location = new System.Drawing.Point(8, 51);
            this.laPageSize.Name = "laPageSize";
            this.laPageSize.Size = new System.Drawing.Size(58, 13);
            this.laPageSize.TabIndex = 3;
            this.laPageSize.Text = "Page Size:";
            // 
            // cbPageLayout
            // 
            this.cbPageLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPageLayout.Location = new System.Drawing.Point(80, 24);
            this.cbPageLayout.Name = "cbPageLayout";
            this.cbPageLayout.Size = new System.Drawing.Size(96, 21);
            this.cbPageLayout.TabIndex = 2;
            this.cbPageLayout.SelectedIndexChanged += new System.EventHandler(this.PageLayoutComboBox_SelectedIndexChanged);
            this.cbPageLayout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageLayoutComboBox_MouseMove);
            // 
            // laPageLayout
            // 
            this.laPageLayout.AutoSize = true;
            this.laPageLayout.Location = new System.Drawing.Point(8, 27);
            this.laPageLayout.Name = "laPageLayout";
            this.laPageLayout.Size = new System.Drawing.Size(70, 13);
            this.laPageLayout.TabIndex = 1;
            this.laPageLayout.Text = "Page Layout:";
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
            this.laDescription.Text = "Code Editor supports page layout mode making it easy to see how text will be posi" +
    "tioned on the printed page.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 143);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 10;
            this.syntaxEdit1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 402);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Page Layout";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.gbRulers.ResumeLayout(false);
            this.gbRulers.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbPages.ResumeLayout(false);
            this.gbPages.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox gbRulers;
        private System.Windows.Forms.CheckBox chbVertRuler;
        private System.Windows.Forms.CheckBox chbHorzRuler;
        private System.Windows.Forms.ComboBox cbRulerUnits;
        private System.Windows.Forms.Label laRulerUnits;
        private System.Windows.Forms.CheckBox chbRulerDisplayDragLines;
        private System.Windows.Forms.CheckBox chbRulerDiscrete;
        private System.Windows.Forms.CheckBox chbRulerAllowDrag;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbPages;
        private System.Windows.Forms.ComboBox cbPageSize;
        private System.Windows.Forms.Label laPageSize;
        private System.Windows.Forms.ComboBox cbPageLayout;
        private System.Windows.Forms.Label laPageLayout;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}