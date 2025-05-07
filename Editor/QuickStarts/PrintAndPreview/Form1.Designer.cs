namespace PrintAndPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pnSettings = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btPrintOptions = new System.Windows.Forms.Button();
            this.btPageSetup = new System.Windows.Forms.Button();
            this.btPrint = new System.Windows.Forms.Button();
            this.btPrintPreview = new System.Windows.Forms.Button();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
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
            this.pnSettings.Size = new System.Drawing.Size(667, 102);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btPrintOptions);
            this.groupBox1.Controls.Add(this.btPageSetup);
            this.groupBox1.Controls.Add(this.btPrint);
            this.groupBox1.Controls.Add(this.btPrintPreview);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 53);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printing";
            // 
            // btPrintOptions
            // 
            this.btPrintOptions.BackColor = System.Drawing.SystemColors.Control;
            this.btPrintOptions.Location = new System.Drawing.Point(272, 19);
            this.btPrintOptions.Name = "btPrintOptions";
            this.btPrintOptions.Size = new System.Drawing.Size(80, 23);
            this.btPrintOptions.TabIndex = 3;
            this.btPrintOptions.Text = "Print Options";
            this.btPrintOptions.UseVisualStyleBackColor = false;
            this.btPrintOptions.Click += new System.EventHandler(this.PrintOptionsButton_Click);
            this.btPrintOptions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintOptionsButton_MouseMove);
            // 
            // btPageSetup
            // 
            this.btPageSetup.BackColor = System.Drawing.SystemColors.Control;
            this.btPageSetup.Location = new System.Drawing.Point(184, 19);
            this.btPageSetup.Name = "btPageSetup";
            this.btPageSetup.Size = new System.Drawing.Size(80, 23);
            this.btPageSetup.TabIndex = 2;
            this.btPageSetup.Text = "Page Setup";
            this.btPageSetup.UseVisualStyleBackColor = false;
            this.btPageSetup.Click += new System.EventHandler(this.PageSetupButton_Click);
            this.btPageSetup.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageSetupButton_MouseMove);
            // 
            // btPrint
            // 
            this.btPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btPrint.Location = new System.Drawing.Point(8, 19);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(80, 23);
            this.btPrint.TabIndex = 0;
            this.btPrint.Text = "Print";
            this.btPrint.UseVisualStyleBackColor = false;
            this.btPrint.Click += new System.EventHandler(this.PrintButton_Click);
            this.btPrint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintButton_MouseMove);
            // 
            // btPrintPreview
            // 
            this.btPrintPreview.BackColor = System.Drawing.SystemColors.Control;
            this.btPrintPreview.Location = new System.Drawing.Point(96, 19);
            this.btPrintPreview.Name = "btPrintPreview";
            this.btPrintPreview.Size = new System.Drawing.Size(80, 23);
            this.btPrintPreview.TabIndex = 1;
            this.btPrintPreview.Text = "Print Preview";
            this.btPrintPreview.UseVisualStyleBackColor = false;
            this.btPrintPreview.Click += new System.EventHandler(this.PrintPreviewButton_Click);
            this.btPrintPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintPreviewButton_MouseMove);
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
            this.laDescription.Text = "Editor Content can be send to printer directly or via print preview dialog.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 102);
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
            this.ClientSize = new System.Drawing.Size(667, 361);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print & Preview";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btPrintOptions;
        private System.Windows.Forms.Button btPageSetup;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.Button btPrintPreview;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}