namespace XmlParsing
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
            this.cbXmls = new System.Windows.Forms.ComboBox();
            this.laXmls = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.syntaxEdit1 = new Alternet.Editor.SyntaxEdit(this.components);
            this.textSource1 = new Alternet.Editor.TextSource.TextSource(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnSettings.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSettings
            // 
           this.pnSettings.Controls.Add(this.cbXmls);
            this.pnSettings.Controls.Add(this.laXmls);
            this.pnSettings.Controls.Add(this.pnDescription);
            this.pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSettings.Location = new System.Drawing.Point(0, 0);
            this.pnSettings.Name = "pnSettings";
            this.pnSettings.Padding = new System.Windows.Forms.Padding(5);
            this.pnSettings.Size = new System.Drawing.Size(667, 99);
            this.pnSettings.TabIndex = 5;
            // 
            // cbXmls
            // 
            this.cbXmls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbXmls.Items.AddRange(new object[] {
            "books",
            "order"});
            this.cbXmls.Location = new System.Drawing.Point(76, 57);
            this.cbXmls.Name = "cbXmls";
            this.cbXmls.Size = new System.Drawing.Size(121, 21);
            this.cbXmls.TabIndex = 10;
            this.cbXmls.SelectedIndexChanged += new System.EventHandler(this.XmlComboBox_SelectedIndexChanged);
            this.cbXmls.MouseMove += new System.Windows.Forms.MouseEventHandler(this.XmlComboBox_MouseMove);
            // 
            // laXmls
            // 
            this.laXmls.AutoSize = true;
            this.laXmls.Location = new System.Drawing.Point(12, 60);
            this.laXmls.Name = "laXmls";
            this.laXmls.Size = new System.Drawing.Size(58, 13);
            this.laXmls.TabIndex = 9;
            this.laXmls.Text = "Xml:";
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
            this.laDescription.Text = @"This demo shows advanced code-editing capabilities, including Code Outlining, Code Completion, and syntax error checking for the XML language. These features are driven by an XSD schema definition.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syntaxEdit1
            // 
            this.syntaxEdit1.BackColor = System.Drawing.SystemColors.Window;
            this.syntaxEdit1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.syntaxEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEdit1.Location = new System.Drawing.Point(0, 99);
            this.syntaxEdit1.Name = "syntaxEdit1";
            this.syntaxEdit1.Outlining.AllowOutlining = true;
            this.syntaxEdit1.Outlining.ImageSize = 8;
            this.syntaxEdit1.SearchGlobal = false;
            this.syntaxEdit1.Size = new System.Drawing.Size(667, 259);
            this.syntaxEdit1.Source = this.textSource1;
            this.syntaxEdit1.TabIndex = 6;
            this.syntaxEdit1.WordWrap = true;
            // 
            // textSource1
            // 
            this.textSource1.OptimizedForMemory = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 358);
            this.Controls.Add(this.syntaxEdit1);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xml Parsing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.pnSettings.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.ComboBox cbXmls;
        private System.Windows.Forms.Label laXmls;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private Alternet.Editor.SyntaxEdit syntaxEdit1;
        private Alternet.Editor.TextSource.TextSource textSource1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}