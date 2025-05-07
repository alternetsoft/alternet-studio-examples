namespace DesignInMemory
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Designer generated code.")]
    public partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnDescription = new System.Windows.Forms.Panel();
            this.btRun = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.propertyGridControl1 = new Alternet.FormDesigner.WinForms.PropertyGridControl();
            this.toolboxControl1 = new Alternet.FormDesigner.WinForms.ToolboxControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.contentTabControl = new System.Windows.Forms.TabControl();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 24);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(1);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(1028, 53);
            this.pnDescription.TabIndex = 10;
            // 
            // btRun
            // 
            this.btRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRun.Location = new System.Drawing.Point(932, 7);
            this.btRun.Margin = new System.Windows.Forms.Padding(1);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(89, 30);
            this.btRun.TabIndex = 12;
            this.btRun.Text = "Run";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(1028, 53);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Form Designer can run form being designed.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGridControl1
            // 
            this.propertyGridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGridControl1.Location = new System.Drawing.Point(0, 77);
            this.propertyGridControl1.Margin = new System.Windows.Forms.Padding(1);
            this.propertyGridControl1.Name = "propertyGridControl1";
            this.propertyGridControl1.Size = new System.Drawing.Size(252, 527);
            this.propertyGridControl1.TabIndex = 12;
            // 
            // toolboxControl1
            // 
            this.toolboxControl1.AutoScroll = true;
            this.toolboxControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolboxControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolboxControl1.Location = new System.Drawing.Point(804, 77);
            this.toolboxControl1.Margin = new System.Windows.Forms.Padding(1);
            this.toolboxControl1.Name = "toolboxControl1";
            this.toolboxControl1.Size = new System.Drawing.Size(224, 527);
            this.toolboxControl1.TabIndex = 13;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(252, 77);
            this.splitter1.Margin = new System.Windows.Forms.Padding(1);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 527);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(803, 77);
            this.splitter2.Margin = new System.Windows.Forms.Padding(1);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 527);
            this.splitter2.TabIndex = 15;
            this.splitter2.TabStop = false;
            // 
            // contentTabControl
            // 
            this.contentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTabControl.Location = new System.Drawing.Point(253, 77);
            this.contentTabControl.Margin = new System.Windows.Forms.Padding(1);
            this.contentTabControl.Name = "contentTabControl";
            this.contentTabControl.SelectedIndex = 0;
            this.contentTabControl.Size = new System.Drawing.Size(550, 527);
            this.contentTabControl.TabIndex = 16;
            this.contentTabControl.SelectedIndexChanged += new System.EventHandler(this.ContentTabControl_SelectedIndexChanged);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(56, 28);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 604);
            this.Controls.Add(this.contentTabControl);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.toolboxControl1);
            this.Controls.Add(this.propertyGridControl1);
            this.Controls.Add(this.pnDescription);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In-Memory Form Designer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Button btRun;
        private Alternet.FormDesigner.WinForms.PropertyGridControl propertyGridControl1;
        private Alternet.FormDesigner.WinForms.ToolboxControl toolboxControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TabControl contentTabControl;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}