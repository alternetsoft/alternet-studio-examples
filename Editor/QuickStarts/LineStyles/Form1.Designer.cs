namespace LineStyles
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbLineStyleColor = new Alternet.Editor.Common.ColorBox(this.components);
            this.chbLineStyleBeyondEol = new System.Windows.Forms.CheckBox();
            this.laLineStyleColor = new System.Windows.Forms.Label();
            this.btSetBreakpoint = new System.Windows.Forms.Button();
            this.btStepOver = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.contextMenu1 = new System.Windows.Forms.ContextMenuStrip();
            this.cmStart = new System.Windows.Forms.ToolStripMenuItem();
            this.cmStepOver = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSetBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.groupBox1);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 125);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbLineStyleColor);
            this.groupBox1.Controls.Add(this.chbLineStyleBeyondEol);
            this.groupBox1.Controls.Add(this.laLineStyleColor);
            this.groupBox1.Controls.Add(this.btSetBreakpoint);
            this.groupBox1.Controls.Add(this.btStepOver);
            this.groupBox1.Controls.Add(this.btStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 76);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Line Styles";
            // 
            // cbLineStyleColor
            // 
            this.cbLineStyleColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbLineStyleColor.FormattingEnabled = true;
            this.cbLineStyleColor.Location = new System.Drawing.Point(385, 40);
            this.cbLineStyleColor.Name = "cbLineStyleColor";
            this.cbLineStyleColor.SelectedColor = System.Drawing.Color.Empty;
            this.cbLineStyleColor.Size = new System.Drawing.Size(121, 21);
            this.cbLineStyleColor.TabIndex = 5;
            this.cbLineStyleColor.SelectedIndexChanged += new System.EventHandler(this.LineStyleColorComboBox_SelectedIndexChanged);
            this.cbLineStyleColor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineStyleColorComboBox_MouseMove);
            // 
            // chbLineStyleBeyondEol
            // 
            this.chbLineStyleBeyondEol.Location = new System.Drawing.Point(298, 16);
            this.chbLineStyleBeyondEol.Name = "chbLineStyleBeyondEol";
            this.chbLineStyleBeyondEol.Size = new System.Drawing.Size(144, 24);
            this.chbLineStyleBeyondEol.TabIndex = 3;
            this.chbLineStyleBeyondEol.Text = "Line Style Beyond Eol";
            this.chbLineStyleBeyondEol.CheckedChanged += new System.EventHandler(this.LineStyleBeyondEolCheckBox_CheckedChanged);
            this.chbLineStyleBeyondEol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineStyleBeyondEolCheckBox_MouseMove);
            // 
            // laLineStyleColor
            // 
            this.laLineStyleColor.AutoSize = true;
            this.laLineStyleColor.Location = new System.Drawing.Point(296, 43);
            this.laLineStyleColor.Name = "laLineStyleColor";
            this.laLineStyleColor.Size = new System.Drawing.Size(83, 13);
            this.laLineStyleColor.TabIndex = 4;
            this.laLineStyleColor.Text = "Line Style Color:";
            // 
            // btSetBreakpoint
            // 
            this.btSetBreakpoint.Location = new System.Drawing.Point(168, 24);
            this.btSetBreakpoint.Name = "btSetBreakpoint";
            this.btSetBreakpoint.Size = new System.Drawing.Size(106, 23);
            this.btSetBreakpoint.TabIndex = 2;
            this.btSetBreakpoint.Text = "Toggle Breakpoint";
            this.btSetBreakpoint.Click += new System.EventHandler(this.SereakpointTextBoxButton_Click);
            this.btSetBreakpoint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SereakpointTextBoxButton_MouseMove);
            // 
            // btStepOver
            // 
            this.btStepOver.Enabled = false;
            this.btStepOver.Location = new System.Drawing.Point(88, 24);
            this.btStepOver.Name = "btStepOver";
            this.btStepOver.Size = new System.Drawing.Size(75, 23);
            this.btStepOver.TabIndex = 1;
            this.btStepOver.Text = "Step Over";
            this.btStepOver.Click += new System.EventHandler(this.StepOverButton_Click);
            this.btStepOver.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StepOverButton_MouseMove);
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(8, 24);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start";
            this.btStart.Click += new System.EventHandler(this.StartButton_Click);
            this.btStart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StartButton_MouseMove);
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
            this.laDescription.Text = "Single line or continuous text range can be associated with the line style repres" +
    "ented by visual indicator in gutter area and different background color.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenu1
            // 
            this.contextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmStart,
            this.cmStepOver,
            this.cmSetBreakpoint});
            // 
            // cmStart
            // 
            this.cmStart.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.cmStart.Text = "Start";
            this.cmStart.Click += new System.EventHandler(this.StartMenuItem_Click);
            // 
            // cmStepOver
            // 
            this.cmStepOver.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.cmStepOver.Text = "Step Over";
            this.cmStepOver.Click += new System.EventHandler(this.StepOverMenuItem_Click);
            // 
            // cmSetBreakpoint
            // 
            this.cmSetBreakpoint.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.cmSetBreakpoint.Text = "Set Breakpoint";
            this.cmSetBreakpoint.Click += new System.EventHandler(this.SereakpointTextBoxMenuItem_Click);
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 125);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.TabIndex = 11;
            this.syntaxEdit1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 384);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Line Styles";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbLineStyleBeyondEol;
        private System.Windows.Forms.Label laLineStyleColor;
        private System.Windows.Forms.Button btSetBreakpoint;
        private System.Windows.Forms.Button btStepOver;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.Common.ColorBox cbLineStyleColor;
        private System.Windows.Forms.ContextMenuStrip contextMenu1;
        private System.Windows.Forms.ToolStripMenuItem cmStart;
        private System.Windows.Forms.ToolStripMenuItem cmStepOver;
        private System.Windows.Forms.ToolStripMenuItem cmSetBreakpoint;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
    }
}