namespace DesignAndRun
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
            this.pnDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(771, 43);
            this.pnDescription.TabIndex = 10;
            // 
            // btRun
            // 
            this.btRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRun.Location = new System.Drawing.Point(699, 6);
            this.btRun.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(67, 24);
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
            this.laDescription.Size = new System.Drawing.Size(771, 43);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "Form Designer can run form being designed.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGridControl1
            // 
            this.propertyGridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGridControl1.FormDesignerControl = null;
            this.propertyGridControl1.LineColor = System.Drawing.SystemColors.Control;
            this.propertyGridControl1.Location = new System.Drawing.Point(0, 43);
            this.propertyGridControl1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.propertyGridControl1.Name = "propertyGridControl1";
            this.propertyGridControl1.Size = new System.Drawing.Size(189, 448);
            this.propertyGridControl1.TabIndex = 12;
            // 
            // toolboxControl1
            // 
            this.toolboxControl1.AutoScroll = true;
            this.toolboxControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolboxControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolboxControl1.FormDesignerControl = null;
            this.toolboxControl1.Location = new System.Drawing.Point(603, 43);
            this.toolboxControl1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.toolboxControl1.Name = "toolboxControl1";
            this.toolboxControl1.Size = new System.Drawing.Size(168, 448);
            this.toolboxControl1.TabIndex = 13;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(189, 43);
            this.splitter1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 448);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(602, 43);
            this.splitter2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1, 448);
            this.splitter2.TabIndex = 15;
            this.splitter2.TabStop = false;
            // 
            // contentTabControl
            // 
            this.contentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTabControl.Location = new System.Drawing.Point(190, 43);
            this.contentTabControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.contentTabControl.Name = "contentTabControl";
            this.contentTabControl.SelectedIndex = 0;
            this.contentTabControl.Size = new System.Drawing.Size(412, 448);
            this.contentTabControl.TabIndex = 16;
            this.contentTabControl.SelectedIndexChanged += new System.EventHandler(this.ContentTabControl_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 491);
            this.Controls.Add(this.contentTabControl);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.toolboxControl1);
            this.Controls.Add(this.propertyGridControl1);
            this.Controls.Add(this.pnDescription);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Design & Run";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).EndInit();
            this.ResumeLayout(false);

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
    }
}