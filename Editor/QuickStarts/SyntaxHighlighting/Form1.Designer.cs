namespace SyntaxHighlighting
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
            this.pnDescription = new System.Windows.Forms.Panel();
            this.btLoadScheme = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbLanguages = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.laLanguages = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 45);
            this.pnSettings.TabIndex = 5;
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.btLoadScheme);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(5, 5);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(657, 39);
            this.pnDescription.TabIndex = 8;
            // 
            // btLoadScheme
            // 
            this.btLoadScheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLoadScheme.Location = new System.Drawing.Point(562, 7);
            this.btLoadScheme.Name = "btLoadScheme";
            this.btLoadScheme.Size = new System.Drawing.Size(88, 23);
            this.btLoadScheme.TabIndex = 11;
            this.btLoadScheme.Text = "Load Scheme";
            this.btLoadScheme.Click += new System.EventHandler(this.LoadSchemeButton_Click);
            this.btLoadScheme.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadSchemeButton_MouseMove);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(533, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo shows how to use predefined syntax highlighting schemes for various lan" +
    "guages";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbLanguages);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 259);
            this.panel1.TabIndex = 6;
            // 
            // lbLanguages
            // 
            this.lbLanguages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLanguages.IntegralHeight = false;
            this.lbLanguages.Location = new System.Drawing.Point(0, 24);
            this.lbLanguages.Name = "lbLanguages";
            this.lbLanguages.Size = new System.Drawing.Size(150, 235);
            this.lbLanguages.TabIndex = 2;
            this.lbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesLisoxTextBox_SelectedIndexChanged);
            this.lbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesLisoxTextBox_MouseMove);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.laLanguages);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 24);
            this.panel2.TabIndex = 1;
            // 
            // laLanguages
            // 
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(4, 4);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(60, 13);
            this.laLanguages.TabIndex = 0;
            this.laLanguages.Text = "Languages";
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(150, 45);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(517, 259);
            this.syntaxEdit1.TabIndex = 7;
            this.syntaxEdit1.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Xml files (*.xml)|*.xml";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Xml files (*.xml)|*.xml";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 304);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Syntax Highlighting";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.pnDescription.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lbLanguages;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label laLanguages;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private System.Windows.Forms.Button btLoadScheme;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}