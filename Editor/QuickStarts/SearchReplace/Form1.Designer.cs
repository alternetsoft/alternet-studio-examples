namespace SearchReplace
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
            this.chbSearchMultiDoc = new System.Windows.Forms.CheckBox();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.laLanguages = new System.Windows.Forms.Label();
            this.btFind = new System.Windows.Forms.Button();
            this.btReplace = new System.Windows.Forms.Button();
            this.btGoto = new System.Windows.Forms.Button();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tcEditors = new System.Windows.Forms.TabControl();
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
            this.pnSettings.Size = new System.Drawing.Size(667, 99);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbSearchMultiDoc);
            this.groupBox1.Controls.Add(this.cbLanguages);
            this.groupBox1.Controls.Add(this.laLanguages);
            this.groupBox1.Controls.Add(this.btFind);
            this.groupBox1.Controls.Add(this.btReplace);
            this.groupBox1.Controls.Add(this.btGoto);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 50);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dialogs";
            // 
            // chbSearchMultiDoc
            // 
            this.chbSearchMultiDoc.AutoSize = true;
            this.chbSearchMultiDoc.Location = new System.Drawing.Point(503, 21);
            this.chbSearchMultiDoc.Name = "chbSearchMultiDoc";
            this.chbSearchMultiDoc.Size = new System.Drawing.Size(137, 17);
            this.chbSearchMultiDoc.TabIndex = 5;
            this.chbSearchMultiDoc.Text = "Multi-Document Search";
            this.chbSearchMultiDoc.UseVisualStyleBackColor = true;
            this.chbSearchMultiDoc.CheckedChanged += new System.EventHandler(this.SearchMultiDocCheckBox_CheckedChanged);
            this.chbSearchMultiDoc.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SearchMultiDocCheckBox_MouseMove);
            // 
            // cbLanguages
            // 
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.Items.AddRange(new object[] {
            "Default",
            "English",
            "French",
            "German",
            "Spanish",
            "Russian",
            "Ukrainian"});
            this.cbLanguages.Location = new System.Drawing.Point(118, 18);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(121, 21);
            this.cbLanguages.TabIndex = 4;
            this.cbLanguages.SelectedIndexChanged += new System.EventHandler(this.LanguagesComboBox_SelectedIndexChanged);
            this.cbLanguages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LanguagesComboBox_MouseMove);
            // 
            // laLanguages
            // 
            this.laLanguages.AutoSize = true;
            this.laLanguages.Location = new System.Drawing.Point(16, 21);
            this.laLanguages.Name = "laLanguages";
            this.laLanguages.Size = new System.Drawing.Size(103, 13);
            this.laLanguages.TabIndex = 3;
            this.laLanguages.Text = "Selected Language:";
            // 
            // btFind
            // 
            this.btFind.BackColor = System.Drawing.SystemColors.Control;
            this.btFind.Location = new System.Drawing.Point(245, 16);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(80, 23);
            this.btFind.TabIndex = 0;
            this.btFind.Text = "Find";
            this.btFind.UseVisualStyleBackColor = false;
            this.btFind.Click += new System.EventHandler(this.FindButton_Click);
            this.btFind.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FindButton_MouseMove);
            // 
            // btReplace
            // 
            this.btReplace.BackColor = System.Drawing.SystemColors.Control;
            this.btReplace.Location = new System.Drawing.Point(331, 16);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(80, 23);
            this.btReplace.TabIndex = 1;
            this.btReplace.Text = "Replace";
            this.btReplace.UseVisualStyleBackColor = false;
            this.btReplace.Click += new System.EventHandler(this.ReplaceButton_Click);
            this.btReplace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ReplaceButton_MouseMove);
            // 
            // btGoto
            // 
            this.btGoto.BackColor = System.Drawing.SystemColors.Control;
            this.btGoto.Location = new System.Drawing.Point(417, 16);
            this.btGoto.Name = "btGoto";
            this.btGoto.Size = new System.Drawing.Size(80, 23);
            this.btGoto.TabIndex = 2;
            this.btGoto.Text = "Go to Line";
            this.btGoto.UseVisualStyleBackColor = false;
            this.btGoto.Click += new System.EventHandler(this.GotoButton_Click);
            this.btGoto.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GotoButton_MouseMove);
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
            this.laDescription.Text = "This demo shows how to use built-in Search, Replace and Go To Line dialogs.  All " +
    "dialogs can be localized.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tcEditors
            // 
            this.tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditors.Location = new System.Drawing.Point(0, 99);
            this.tcEditors.Name = "tcEditors";
            this.tcEditors.SelectedIndex = 0;
            this.tcEditors.Size = new System.Drawing.Size(667, 259);
            this.tcEditors.TabIndex = 40;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 358);
            this.Controls.Add(this.tcEditors);
            this.Controls.Add(this.pnSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search & Replace";
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
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button btGoto;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tcEditors;
        private System.Windows.Forms.CheckBox chbSearchMultiDoc;
    }
}
