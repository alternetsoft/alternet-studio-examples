namespace ScriptHostObject
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Designer generated code")]
    public partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExpressionLabel = new System.Windows.Forms.Label();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.btRun = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.scriptRun = new Alternet.Scripter.ScriptRun(this.components);
            this.pnEdit = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.pnDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ExpressionLabel);
            this.panel1.Controls.Add(this.pnDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(957, 101);
            this.panel1.TabIndex = 0;
            // 
            // ExpressionLabel
            // 
            this.ExpressionLabel.AutoSize = true;
            this.ExpressionLabel.Location = new System.Drawing.Point(3, 61);
            this.ExpressionLabel.Name = "ExpressionLabel";
            this.ExpressionLabel.Size = new System.Drawing.Size(74, 16);
            this.ExpressionLabel.TabIndex = 3;
            this.ExpressionLabel.Text = "Expression (X+Y)";
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.cbLanguages);
            this.pnDescription.Controls.Add(this.laLanguages);
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Margin = new System.Windows.Forms.Padding(4);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(955, 48);
            this.pnDescription.TabIndex = 2;
            // 
            // cbLanguages
            // 
            this.cbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "C#",
            "Visual Basic"});
            this.cbLanguages.Location = new System.Drawing.Point(822, 13);
            this.cbLanguages.Margin = new System.Windows.Forms.Padding(4);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(129, 24);
            this.cbLanguages.TabIndex = 20;
            this.cbLanguages.Visible = false;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            // 
            // laLanguages
            // 
            this.laLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(737, 17);
            this.laLanguages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(71, 16);
            this.laLanguages.TabIndex = 19;
            this.laLanguages.Text = "Language:";
            this.laLanguages.Visible = false;
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(631, 10);
            this.btRun.Margin = new System.Windows.Forms.Padding(4);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(100, 28);
            this.btRun.TabIndex = 4;
            this.btRun.Text = "Run script";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(612, 48);
            this.laDescription.TabIndex = 3;
            this.laDescription.Text = "This demo shows how to access fields of the objects hosted in the application in " +
    "the class-less script.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnEdit
            // 
            this.pnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEdit.Location = new System.Drawing.Point(0, 101);
            this.pnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.pnEdit.Name = "pnEdit";
            this.pnEdit.Size = new System.Drawing.Size(957, 443);
            this.pnEdit.TabIndex = 16;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 544);
            this.Controls.Add(this.pnEdit);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Host Object";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Alternet.Scripter.ScriptRun scriptRun;
        private System.Windows.Forms.Panel pnEdit;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label ExpressionLabel;
    }
}