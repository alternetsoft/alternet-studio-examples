namespace Bookmarks
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
            this.btPrevBookmark = new System.Windows.Forms.Button();
            this.btNextBookmark = new System.Windows.Forms.Button();
            this.chbShowBookmarks = new System.Windows.Forms.CheckBox();
            this.chbLineBookmarks = new System.Windows.Forms.CheckBox();
            this.btSetBookmark = new System.Windows.Forms.Button();
            this.btClearBookmarks = new System.Windows.Forms.Button();
            this.btSetCustomBookmark = new System.Windows.Forms.Button();
            this.btSetUnindexedBookmarks = new System.Windows.Forms.Button();
            this.pnDescription = new System.Windows.Forms.Panel();
            this.laDescription = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
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
            this.pnSettings.Size = new System.Drawing.Size(667, 127);
            this.pnSettings.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btPrevBookmark);
            this.groupBox1.Controls.Add(this.btNextBookmark);
            this.groupBox1.Controls.Add(this.chbShowBookmarks);
            this.groupBox1.Controls.Add(this.chbLineBookmarks);
            this.groupBox1.Controls.Add(this.btSetBookmark);
            this.groupBox1.Controls.Add(this.btClearBookmarks);
            this.groupBox1.Controls.Add(this.btSetCustomBookmark);
            this.groupBox1.Controls.Add(this.btSetUnindexedBookmarks);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 78);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bookmarks";
            // 
            // btPrevBookmark
            // 
            this.btPrevBookmark.Location = new System.Drawing.Point(476, 45);
            this.btPrevBookmark.Name = "btPrevBookmark";
            this.btPrevBookmark.Size = new System.Drawing.Size(152, 23);
            this.btPrevBookmark.TabIndex = 17;
            this.btPrevBookmark.Text = "Previous Bookmark";
            this.btPrevBookmark.Click += new System.EventHandler(this.PrevBookmarkButton_Click);
            this.btPrevBookmark.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrevBookmarkButton_MouseMove);
            // 
            // btNextBookmark
            // 
            this.btNextBookmark.Location = new System.Drawing.Point(476, 16);
            this.btNextBookmark.Name = "btNextBookmark";
            this.btNextBookmark.Size = new System.Drawing.Size(152, 23);
            this.btNextBookmark.TabIndex = 16;
            this.btNextBookmark.Text = "Next Bookmark";
            this.btNextBookmark.Click += new System.EventHandler(this.NexookmarkTextBoxButton_Click);
            this.btNextBookmark.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NexookmarkTextBoxButton_MouseMove);
            // 
            // chbShowBookmarks
            // 
            this.chbShowBookmarks.Location = new System.Drawing.Point(16, 16);
            this.chbShowBookmarks.Name = "chbShowBookmarks";
            this.chbShowBookmarks.Size = new System.Drawing.Size(112, 24);
            this.chbShowBookmarks.TabIndex = 9;
            this.chbShowBookmarks.Text = "Draw Bookmarks";
            this.chbShowBookmarks.CheckedChanged += new System.EventHandler(this.ShowBookmarksCheckBox_CheckedChanged);
            this.chbShowBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowBookmarksCheckBox_MouseMove);
            // 
            // chbLineBookmarks
            // 
            this.chbLineBookmarks.Location = new System.Drawing.Point(16, 45);
            this.chbLineBookmarks.Name = "chbLineBookmarks";
            this.chbLineBookmarks.Size = new System.Drawing.Size(136, 24);
            this.chbLineBookmarks.TabIndex = 10;
            this.chbLineBookmarks.Text = "Draw Line Bookmarks";
            this.chbLineBookmarks.CheckedChanged += new System.EventHandler(this.LineBookmarksCheckBox_CheckedChanged);
            this.chbLineBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineBookmarksCheckBox_MouseMove);
            // 
            // btSetBookmark
            // 
            this.btSetBookmark.Location = new System.Drawing.Point(160, 16);
            this.btSetBookmark.Name = "btSetBookmark";
            this.btSetBookmark.Size = new System.Drawing.Size(152, 23);
            this.btSetBookmark.TabIndex = 12;
            this.btSetBookmark.Text = "Set Indexed Bookmark";
            this.btSetBookmark.Click += new System.EventHandler(this.SeookmarkTextBoxButton_Click);
            this.btSetBookmark.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SeookmarkTextBoxButton_MouseMove);
            // 
            // btClearBookmarks
            // 
            this.btClearBookmarks.Location = new System.Drawing.Point(160, 45);
            this.btClearBookmarks.Name = "btClearBookmarks";
            this.btClearBookmarks.Size = new System.Drawing.Size(152, 23);
            this.btClearBookmarks.TabIndex = 14;
            this.btClearBookmarks.Text = "Clear Bookmarks";
            this.btClearBookmarks.Click += new System.EventHandler(this.ClearBookmarksButton_Click);
            this.btClearBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClearBookmarksButton_MouseMove);
            // 
            // btSetCustomBookmark
            // 
            this.btSetCustomBookmark.Location = new System.Drawing.Point(318, 45);
            this.btSetCustomBookmark.Name = "btSetCustomBookmark";
            this.btSetCustomBookmark.Size = new System.Drawing.Size(152, 23);
            this.btSetCustomBookmark.TabIndex = 15;
            this.btSetCustomBookmark.Text = "Set Custom Bookmark";
            this.btSetCustomBookmark.Click += new System.EventHandler(this.SetCustomBookmarkButton_Click);
            this.btSetCustomBookmark.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SetCustomBookmarkButton_MouseMove);
            // 
            // btSetUnindexedBookmarks
            // 
            this.btSetUnindexedBookmarks.Location = new System.Drawing.Point(318, 16);
            this.btSetUnindexedBookmarks.Name = "btSetUnindexedBookmarks";
            this.btSetUnindexedBookmarks.Size = new System.Drawing.Size(152, 23);
            this.btSetUnindexedBookmarks.TabIndex = 13;
            this.btSetUnindexedBookmarks.Text = "Set Unindexed Bookmarks";
            this.btSetUnindexedBookmarks.Click += new System.EventHandler(this.SetUnindexedBookmarksButton_Click);
            this.btSetUnindexedBookmarks.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SetUnindexedBookmarksButton_MouseMove);
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
            this.laDescription.Text = "Bookmarks are used to simplify navigation through the text.";
            this.laDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tcEditors
            // 
            this.tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditors.Location = new System.Drawing.Point(0, 127);
            this.tcEditors.Name = "tcEditors";
            this.tcEditors.SelectedIndex = 0;
            this.tcEditors.Size = new System.Drawing.Size(667, 259);
            this.tcEditors.TabIndex = 39;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 386);
            this.Controls.Add(this.tcEditors);
            this.Controls.Add(this.pnSettings);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bookmarks";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbShowBookmarks;
        private System.Windows.Forms.CheckBox chbLineBookmarks;
        private System.Windows.Forms.Button btSetBookmark;
        private System.Windows.Forms.Button btClearBookmarks;
        private System.Windows.Forms.Button btSetCustomBookmark;
        private System.Windows.Forms.Button btSetUnindexedBookmarks;
        private System.Windows.Forms.Panel pnDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btPrevBookmark;
        private System.Windows.Forms.Button btNextBookmark;
        private System.Windows.Forms.TabControl tcEditors;
    }
}