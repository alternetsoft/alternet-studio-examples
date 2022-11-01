namespace Alternet.FormDesigner.Demo
{
    partial class FilesUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesUserControl));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newFormToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.filesTreeView = new System.Windows.Forms.TreeView();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFormToolStripButton,
            this.refreshToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(211, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // newFormToolStripButton
            // 
            this.newFormToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newFormToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newFormToolStripButton.Image")));
            this.newFormToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newFormToolStripButton.Name = "newFormToolStripButton";
            this.newFormToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newFormToolStripButton.Text = "New Form...";
            this.newFormToolStripButton.Click += new System.EventHandler(this.NewFormToolStripButton_Click);
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshToolStripButton.Image")));
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshToolStripButton.Text = "Refresh";
            this.refreshToolStripButton.Click += new System.EventHandler(this.RefreshToolStripButton_Click);
            // 
            // filesTreeView
            // 
            this.filesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesTreeView.Location = new System.Drawing.Point(0, 25);
            this.filesTreeView.Name = "filesTreeView";
            this.filesTreeView.Size = new System.Drawing.Size(211, 445);
            this.filesTreeView.TabIndex = 1;
            this.filesTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.FilesTreeView_BeforeCollapse);
            this.filesTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.FilesTreeView_BeforeExpand);
            this.filesTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FilesTreeView_NodeMouseDoubleClick);
            this.filesTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FilesTreeView_MouseDown);
            // 
            // FilesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filesTreeView);
            this.Controls.Add(this.toolStrip);
            this.Name = "FilesUserControl";
            this.Size = new System.Drawing.Size(211, 470);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.TreeView filesTreeView;
        private System.Windows.Forms.ToolStripButton newFormToolStripButton;
    }
}
