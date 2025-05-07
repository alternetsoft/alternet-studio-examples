namespace CustomAssembly.TypeScript
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1120:CommentsMustContainText", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Implementing interface method in the separate region")]
    public partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
            this.pnDescription = new System.Windows.Forms.Panel();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.btRun = new System.Windows.Forms.Button();
            this.laDescription = new System.Windows.Forms.Label();
            this.tcEditors = new System.Windows.Forms.TabControl();
            this.tpEditor = new System.Windows.Forms.TabPage();
            this.tpExternal = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.scriptRun = new Alternet.Scripter.TypeScript.ScriptRun(this.components);
            this.pnDescription.SuspendLayout();
            this.tcEditors.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDescription
            // 
            this.pnDescription.Controls.Add(this.cbLanguages);
            this.pnDescription.Controls.Add(this.laLanguages);
            this.pnDescription.Controls.Add(this.btRun);
            this.pnDescription.Controls.Add(this.laDescription);
            this.pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDescription.Location = new System.Drawing.Point(0, 0);
            this.pnDescription.Name = "pnDescription";
            this.pnDescription.Size = new System.Drawing.Size(615, 34);
            this.pnDescription.TabIndex = 10;
            // 
            // cbLanguages
            // 
            this.cbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "TypeScript",
            "JavaScript"});
            this.cbLanguages.Location = new System.Drawing.Point(513, 8);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(98, 21);
            this.cbLanguages.TabIndex = 12;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            this.cbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesComboBox_MouseMove);
            // 
            // laLanguages
            // 
            this.laLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(449, 11);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(58, 13);
            this.laLanguages.TabIndex = 11;
            this.laLanguages.Text = "Language:";
            // 
            // btRun
            // 
            this.btRun.Location = new System.Drawing.Point(306, 8);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(75, 23);
            this.btRun.TabIndex = 2;
            this.btRun.Text = "Run script";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // laDescription
            // 
            this.laDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.laDescription.Location = new System.Drawing.Point(0, 0);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new System.Drawing.Size(615, 39);
            this.laDescription.TabIndex = 1;
            this.laDescription.Text = "This demo shows how to use external assemblies in scripting.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tcEditors
            // 
            this.tcEditors.Controls.Add(this.tpEditor);
            this.tcEditors.Controls.Add(this.tpExternal);
            this.tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditors.Location = new System.Drawing.Point(0, 34);
            this.tcEditors.Name = "tcEditors";
            this.tcEditors.SelectedIndex = 0;
            this.tcEditors.Size = new System.Drawing.Size(615, 349);
            this.tcEditors.TabIndex = 11;
            // 
            // tpEditor
            // 
            this.tpEditor.Location = new System.Drawing.Point(4, 22);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tpEditor.Size = new System.Drawing.Size(607, 323);
            this.tpEditor.TabIndex = 0;
            this.tpEditor.Text = "Source Code";
            this.tpEditor.UseVisualStyleBackColor = true;
            // 
            // tpExternal
            // 
            this.tpExternal.Location = new System.Drawing.Point(4, 22);
            this.tpExternal.Name = "tpExternal";
            this.tpExternal.Padding = new System.Windows.Forms.Padding(3);
            this.tpExternal.Size = new System.Drawing.Size(607, 323);
            this.tpExternal.TabIndex = 1;
            this.tpExternal.Text = "External Code";
            this.tpExternal.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 383);
            this.Controls.Add(this.tcEditors);
            this.Controls.Add(this.pnDescription);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom assembly - TypeScript";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnDescription.ResumeLayout(false);
            this.pnDescription.PerformLayout();
            this.tcEditors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.TabControl tcEditors;
        private System.Windows.Forms.TabPage tpEditor;
        private System.Windows.Forms.TabPage tpExternal;
        private Alternet.Scripter.TypeScript.ScriptRun scriptRun;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}