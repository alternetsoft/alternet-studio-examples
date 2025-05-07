namespace AlternetStudio.Demo
{
    partial class AddReferenceDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ReferencesTreeView = new System.Windows.Forms.TreeView();
            this.AssembliesListView = new System.Windows.Forms.ListView();
            this.NameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.FilterAssembliesTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.ButtonOK);
            this.panel1.Controls.Add(this.BrowseButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 397);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 53);
            this.panel1.TabIndex = 1;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(709, 14);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(628, 14);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(547, 14);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 17);
            this.panel2.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 17);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ReferencesTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AssembliesListView);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Size = new System.Drawing.Size(797, 380);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 5;
            // 
            // ReferencesTreeView
            // 
            this.ReferencesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReferencesTreeView.Location = new System.Drawing.Point(0, 0);
            this.ReferencesTreeView.Name = "ReferencesTreeView";
            this.ReferencesTreeView.ShowLines = false;
            this.ReferencesTreeView.ShowPlusMinus = false;
            this.ReferencesTreeView.ShowRootLines = false;
            this.ReferencesTreeView.Size = new System.Drawing.Size(198, 380);
            this.ReferencesTreeView.TabIndex = 2;
            this.ReferencesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ReferencesTreeView_AfterSelect);
            // 
            // AssembliesListView
            // 
            this.AssembliesListView.CheckBoxes = true;
            this.AssembliesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameHeader,
            this.VersionHeader});
            this.AssembliesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssembliesListView.HideSelection = false;
            this.AssembliesListView.Location = new System.Drawing.Point(0, 24);
            this.AssembliesListView.Name = "AssembliesListView";
            this.AssembliesListView.Size = new System.Drawing.Size(595, 356);
            this.AssembliesListView.TabIndex = 8;
            this.AssembliesListView.UseCompatibleStateImageBehavior = false;
            this.AssembliesListView.View = System.Windows.Forms.View.Details;
            // 
            // NameHeader
            // 
            this.NameHeader.Text = "Name";
            this.NameHeader.Width = 404;
            // 
            // VersionHeader
            // 
            this.VersionHeader.Text = "Version";
            this.VersionHeader.Width = 200;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.FilterAssembliesTextBox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(595, 24);
            this.panel4.TabIndex = 2;
            // 
            // FilterAssembliesTextBox
            // 
            this.FilterAssembliesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterAssembliesTextBox.Location = new System.Drawing.Point(0, 0);
            this.FilterAssembliesTextBox.Name = "FilterAssembliesTextBox";
            this.FilterAssembliesTextBox.Size = new System.Drawing.Size(595, 22);
            this.FilterAssembliesTextBox.TabIndex = 1;
            this.FilterAssembliesTextBox.TextChanged += new System.EventHandler(this.FilterAssembliesTextBox_TextChanged);
            // 
            // AddReferenceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AddReferenceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reference Manager";
            this.Load += new System.EventHandler(this.AddReferenceDialog_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView ReferencesTreeView;
        private System.Windows.Forms.ListView AssembliesListView;
        private System.Windows.Forms.ColumnHeader NameHeader;
        private System.Windows.Forms.ColumnHeader VersionHeader;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox FilterAssembliesTextBox;
    }
}