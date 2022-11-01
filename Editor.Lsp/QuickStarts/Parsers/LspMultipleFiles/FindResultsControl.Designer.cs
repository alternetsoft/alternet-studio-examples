
namespace LspMultipleFiles
{
    partial class FindResultsControl
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
            this.findResultsListView = new System.Windows.Forms.ListView();
            this.fileColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lineColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.codeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // findResultsListView
            // 
            this.findResultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileColumnHeader,
            this.lineColumnHeader,
            this.codeColumnHeader});
            this.findResultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findResultsListView.FullRowSelect = true;
            this.findResultsListView.HideSelection = false;
            this.findResultsListView.Location = new System.Drawing.Point(0, 0);
            this.findResultsListView.MultiSelect = false;
            this.findResultsListView.Name = "findResultsListView";
            this.findResultsListView.Size = new System.Drawing.Size(730, 201);
            this.findResultsListView.TabIndex = 0;
            this.findResultsListView.UseCompatibleStateImageBehavior = false;
            this.findResultsListView.View = System.Windows.Forms.View.Details;
            this.findResultsListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FindResultsListView_KeyUp);
            this.findResultsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FindResultsListView_MouseDoubleClick);
            // 
            // fileColumnHeader
            // 
            this.fileColumnHeader.Text = "File";
            this.fileColumnHeader.Width = 100;
            // 
            // lineColumnHeader
            // 
            this.lineColumnHeader.Text = "Line";
            // 
            // codeColumnHeader
            // 
            this.codeColumnHeader.Text = "Code";
            this.codeColumnHeader.Width = 500;
            // 
            // FindResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 201);
            this.Controls.Add(this.findResultsListView);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView findResultsListView;
        private System.Windows.Forms.ColumnHeader fileColumnHeader;
        private System.Windows.Forms.ColumnHeader lineColumnHeader;
        private System.Windows.Forms.ColumnHeader codeColumnHeader;
    }
}