namespace LoadAndSave
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
            this.btSave = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.propertyGridControl1 = new Alternet.FormDesigner.WinForms.PropertyGridControl();
            this.formDesignerControl1 = new Alternet.FormDesigner.WinForms.FormDesignerControl();
            this.toolboxControl1 = new Alternet.FormDesigner.WinForms.ToolboxControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btSave);
            this.pnDescription.Controls.Add(this.btLoad);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(763, 38);
            this.pnDescription.TabIndex = 9;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(676, 7);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 11;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // btLoad
            // 
            this.btLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLoad.Location = new System.Drawing.Point(595, 7);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(75, 23);
            this.btLoad.TabIndex = 9;
            this.btLoad.Text = "Load...";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(763, 38);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "From Designer can load and save its content";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // propertyGridControl1
            // 
            this.propertyGridControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGridControl1.FormDesignerControl = this.formDesignerControl1;
            this.propertyGridControl1.LineColor = System.Drawing.SystemColors.Control;
            this.propertyGridControl1.Location = new System.Drawing.Point(0, 38);
            this.propertyGridControl1.Name = "propertyGridControl1";
            this.propertyGridControl1.Size = new System.Drawing.Size(171, 436);
            this.propertyGridControl1.TabIndex = 11;
            // 
            // formDesignerControl1
            // 
            this.formDesignerControl1.AutoSaveToSource = false;
            this.formDesignerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formDesignerControl1.Location = new System.Drawing.Point(174, 38);
            this.formDesignerControl1.Name = "formDesignerControl1";
            this.formDesignerControl1.Size = new System.Drawing.Size(399, 436);
            this.formDesignerControl1.TabIndex = 15;
            this.formDesignerControl1.Text = "formDesignerControl1";
            this.formDesignerControl1.ToolboxControl = this.toolboxControl1;
            // 
            // toolboxControl1
            // 
            this.toolboxControl1.AutoScroll = true;
            this.toolboxControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.toolboxControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolboxControl1.FormDesignerControl = this.formDesignerControl1;
            this.toolboxControl1.Location = new System.Drawing.Point(576, 38);
            this.toolboxControl1.Name = "toolboxControl1";
            this.toolboxControl1.Size = new System.Drawing.Size(187, 436);
            this.toolboxControl1.TabIndex = 12;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(171, 38);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 436);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(573, 38);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 436);
            this.splitter2.TabIndex = 14;
            this.splitter2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 474);
            this.Controls.Add(this.formDesignerControl1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.toolboxControl1);
            this.Controls.Add(this.propertyGridControl1);
            this.Controls.Add(this.pnDescription);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load & Save Designer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Button btSave;
        private Alternet.FormDesigner.WinForms.PropertyGridControl propertyGridControl1;
        private Alternet.FormDesigner.WinForms.ToolboxControl toolboxControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private Alternet.FormDesigner.WinForms.FormDesignerControl formDesignerControl1;
    }
}