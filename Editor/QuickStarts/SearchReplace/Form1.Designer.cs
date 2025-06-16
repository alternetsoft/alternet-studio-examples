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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pnSettings = new System.Windows.Forms.Panel();
            groupBox1 = new System.Windows.Forms.GroupBox();
            chbSearchMultiDoc = new System.Windows.Forms.CheckBox();
            cbLanguages = new System.Windows.Forms.ComboBox();
            laLanguages = new System.Windows.Forms.Label();
            FindNextButton = new System.Windows.Forms.Button();
            btReplace = new System.Windows.Forms.Button();
            btGoto = new System.Windows.Forms.Button();
            FindInFilesButton = new System.Windows.Forms.Button();
            pnDescription = new System.Windows.Forms.Panel();
            laDescription = new System.Windows.Forms.Label();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            BottomPanel = new System.Windows.Forms.Panel();
            FindResultsListView = new System.Windows.Forms.ListView();
            FileColumn = new System.Windows.Forms.ColumnHeader();
            LineColumn = new System.Windows.Forms.ColumnHeader();
            CodeColumn = new System.Windows.Forms.ColumnHeader();
            tcEditors = new System.Windows.Forms.TabControl();
            pnSettings.SuspendLayout();
            groupBox1.SuspendLayout();
            pnDescription.SuspendLayout();
            BottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // pnSettings
            // 
            pnSettings.Controls.Add(groupBox1);
            pnSettings.Controls.Add(pnDescription);
            pnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            pnSettings.Location = new System.Drawing.Point(0, 0);
            pnSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnSettings.Name = "pnSettings";
            pnSettings.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            pnSettings.Size = new System.Drawing.Size(1000, 152);
            pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chbSearchMultiDoc);
            groupBox1.Controls.Add(cbLanguages);
            groupBox1.Controls.Add(laLanguages);
            groupBox1.Controls.Add(FindNextButton);
            groupBox1.Controls.Add(btReplace);
            groupBox1.Controls.Add(btGoto);
            groupBox1.Controls.Add(FindInFilesButton);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(7, 68);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBox1.Size = new System.Drawing.Size(986, 76);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dialogs";
            // 
            // chbSearchMultiDoc
            // 
            chbSearchMultiDoc.AutoSize = true;
            chbSearchMultiDoc.Location = new System.Drawing.Point(785, 32);
            chbSearchMultiDoc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chbSearchMultiDoc.Name = "chbSearchMultiDoc";
            chbSearchMultiDoc.Size = new System.Drawing.Size(188, 24);
            chbSearchMultiDoc.TabIndex = 5;
            chbSearchMultiDoc.Text = "Multi-Document Search";
            chbSearchMultiDoc.Checked = true;
            chbSearchMultiDoc.UseVisualStyleBackColor = true;
            chbSearchMultiDoc.CheckedChanged += SearchMultiDocCheckBox_CheckedChanged;
            chbSearchMultiDoc.MouseMove += SearchMultiDocCheckBox_MouseMove;
            // 
            // cbLanguages
            // 
            cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbLanguages.Items.AddRange(new object[] { "Default", "English", "French", "German", "Spanish", "Russian", "Ukrainian" });
            cbLanguages.Location = new System.Drawing.Point(157, 28);
            cbLanguages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbLanguages.Name = "cbLanguages";
            cbLanguages.Size = new System.Drawing.Size(160, 28);
            cbLanguages.TabIndex = 4;
            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;
            cbLanguages.MouseMove += LanguagesComboBox_MouseMove;
            // 
            // laLanguages
            // 
            laLanguages.AutoSize = true;
            laLanguages.Location = new System.Drawing.Point(21, 32);
            laLanguages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laLanguages.Name = "laLanguages";
            laLanguages.Size = new System.Drawing.Size(138, 20);
            laLanguages.TabIndex = 3;
            laLanguages.Text = "Selected Language:";
            // 
            // FindNextButton
            // 
            FindNextButton.BackColor = System.Drawing.SystemColors.Control;
            FindNextButton.Location = new System.Drawing.Point(327, 25);
            FindNextButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            FindNextButton.Name = "FindNextButton";
            FindNextButton.Size = new System.Drawing.Size(107, 35);
            FindNextButton.TabIndex = 0;
            FindNextButton.Text = "Find Next";
            FindNextButton.UseVisualStyleBackColor = false;
            FindNextButton.Click += FindButton_Click;
            FindNextButton.MouseMove += FindButton_MouseMove;
            // 
            // btReplace
            // 
            btReplace.BackColor = System.Drawing.SystemColors.Control;
            btReplace.Location = new System.Drawing.Point(441, 25);
            btReplace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btReplace.Name = "btReplace";
            btReplace.Size = new System.Drawing.Size(107, 35);
            btReplace.TabIndex = 1;
            btReplace.Text = "Replace";
            btReplace.UseVisualStyleBackColor = false;
            btReplace.Click += ReplaceButton_Click;
            btReplace.MouseMove += ReplaceButton_MouseMove;
            // 
            // btGoto
            // 
            btGoto.BackColor = System.Drawing.SystemColors.Control;
            btGoto.Location = new System.Drawing.Point(556, 25);
            btGoto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btGoto.Name = "btGoto";
            btGoto.Size = new System.Drawing.Size(107, 35);
            btGoto.TabIndex = 2;
            btGoto.Text = "Go to Line";
            btGoto.UseVisualStyleBackColor = false;
            btGoto.Click += GotoButton_Click;
            btGoto.MouseMove += GotoButton_MouseMove;
            // 
            // FindInFilesButton
            // 
            FindInFilesButton.BackColor = System.Drawing.SystemColors.Control;
            FindInFilesButton.Location = new System.Drawing.Point(671, 25);
            FindInFilesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            FindInFilesButton.Name = "FindInFilesButton";
            FindInFilesButton.Size = new System.Drawing.Size(107, 35);
            FindInFilesButton.TabIndex = 2;
            FindInFilesButton.Text = "Find In Files";
            FindInFilesButton.UseVisualStyleBackColor = false;
            FindInFilesButton.Click += FindInFilesButton_Click;
            FindInFilesButton.MouseMove += FindInFilesButton_MouseMove;
            // 
            // pnDescription
            // 
            pnDescription.Controls.Add(laDescription);
            pnDescription.Dock = System.Windows.Forms.DockStyle.Top;
            pnDescription.Location = new System.Drawing.Point(7, 8);
            pnDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnDescription.Name = "pnDescription";
            pnDescription.Size = new System.Drawing.Size(986, 60);
            pnDescription.TabIndex = 8;
            // 
            // laDescription
            // 
            laDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            laDescription.Location = new System.Drawing.Point(0, 0);
            laDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laDescription.Name = "laDescription";
            laDescription.Size = new System.Drawing.Size(986, 60);
            laDescription.TabIndex = 1;
            laDescription.Text = "This demo shows how to use built-in Search, Replace and Go To Line dialogs.  All dialogs can be localized.";
            laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BottomPanel
            // 
            BottomPanel.Controls.Add(FindResultsListView);
            BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            BottomPanel.Location = new System.Drawing.Point(0, 342);
            BottomPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new System.Drawing.Size(1000, 209);
            BottomPanel.TabIndex = 5;
            // 
            // FindResultsListView
            // 
            FindResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { FileColumn, LineColumn, CodeColumn });
            FindResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            FindResultsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            FindResultsListView.Location = new System.Drawing.Point(0, 0);
            FindResultsListView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            FindResultsListView.Name = "FindResultsListView";
            FindResultsListView.Size = new System.Drawing.Size(1000, 209);
            FindResultsListView.TabIndex = 0;
            FindResultsListView.UseCompatibleStateImageBehavior = false;
            FindResultsListView.View = System.Windows.Forms.View.Details;
            FindResultsListView.MouseDoubleClick += FindResultsListView_MouseDoubleClick;
            // 
            // FileColumn
            // 
            FileColumn.Text = "File";
            FileColumn.Width = 120;
            // 
            // LineColumn
            // 
            LineColumn.Text = "Line";
            // 
            // CodeColumn
            // 
            CodeColumn.Text = "Code";
            CodeColumn.Width = 1000;
            // 
            // tcEditors
            // 
            tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            tcEditors.Location = new System.Drawing.Point(0, 152);
            tcEditors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tcEditors.Name = "tcEditors";
            tcEditors.SelectedIndex = 0;
            tcEditors.Size = new System.Drawing.Size(1000, 190);
            tcEditors.TabIndex = 43;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1000, 581);
            Controls.Add(tcEditors);
            Controls.Add(BottomPanel);
            Controls.Add(pnSettings);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Search & Replace";
            Load += Form1_Load;
            pnSettings.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            pnDescription.ResumeLayout(false);
            BottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button FindNextButton;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button btGoto;
        private System.Windows.Forms.Button FindInFilesButton;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.ListView FindResultsListView;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label laLanguages;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chbSearchMultiDoc;
        private System.Windows.Forms.ColumnHeader FileColumn;
        private System.Windows.Forms.ColumnHeader LineColumn;
        private System.Windows.Forms.ColumnHeader CodeColumn;
        private System.Windows.Forms.TabControl tcEditors;
    }
}
