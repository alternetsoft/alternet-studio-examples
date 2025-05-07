namespace LspMultipleFiles
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
            this.filesControl = new LspMultipleFiles.FilesControl();
            this.findResultsPanel = new System.Windows.Forms.Panel();
            this.findResultsCaptionLabel = new System.Windows.Forms.Label();
            this.editorsTabControl = new System.Windows.Forms.TabControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lspServerToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.lspServerComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.leftPanelSplitter = new System.Windows.Forms.Splitter();
            this.bottomPanelSplitter = new System.Windows.Forms.Splitter();
            this.appDescriptionToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.findResultsPanel.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.lspServerToolStrip.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.appDescriptionToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // filesControl
            // 
            this.filesControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.filesControl.Location = new System.Drawing.Point(0, 0);
            this.filesControl.Name = "filesControl";
            this.filesControl.RootDirectory = null;
            this.filesControl.SearchPatterns = new string[] {
        "*.*"};
            this.filesControl.Size = new System.Drawing.Size(185, 592);
            this.filesControl.TabIndex = 0;
            this.filesControl.OpenFileRequested += new System.EventHandler<LspMultipleFiles.FilesControl.OpenFileRequestEventArgs>(this.FilesControl_OpenFileRequested);
            // 
            // findResultsPanel
            // 
            this.findResultsPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.findResultsPanel.Controls.Add(this.findResultsCaptionLabel);
            this.findResultsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.findResultsPanel.Location = new System.Drawing.Point(0, 595);
            this.findResultsPanel.Name = "findResultsPanel";
            this.findResultsPanel.Size = new System.Drawing.Size(1200, 156);
            this.findResultsPanel.TabIndex = 1;
            // 
            // findResultsCaptionLabel
            // 
            this.findResultsCaptionLabel.AutoSize = true;
            this.findResultsCaptionLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.findResultsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.findResultsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.findResultsCaptionLabel.Name = "findResultsCaptionLabel";
            this.findResultsCaptionLabel.Padding = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.findResultsCaptionLabel.Size = new System.Drawing.Size(73, 18);
            this.findResultsCaptionLabel.TabIndex = 0;
            this.findResultsCaptionLabel.Text = "Find Results";
            // 
            // editorsTabControl
            // 
            this.editorsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorsTabControl.Location = new System.Drawing.Point(185, 0);
            this.editorsTabControl.Name = "editorsTabControl";
            this.editorsTabControl.SelectedIndex = 0;
            this.editorsTabControl.Size = new System.Drawing.Size(1015, 592);
            this.editorsTabControl.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.lspServerToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.lspServerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.lspServerComboBox});
            this.lspServerToolStrip.Location = new System.Drawing.Point(3, 24);
            this.lspServerToolStrip.Name = "toolStrip";
            this.lspServerToolStrip.Size = new System.Drawing.Size(339, 25);
            this.lspServerToolStrip.TabIndex = 2;
            this.lspServerToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(204, 22);
            this.toolStripLabel1.Text = "Programming Language (LSP Server):";
            // 
            // lspServerComboBox
            // 
            this.lspServerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lspServerComboBox.Name = "lspServerComboBox";
            this.lspServerComboBox.Size = new System.Drawing.Size(121, 25);
            this.lspServerComboBox.SelectedIndexChanged += new System.EventHandler(this.LspServerComboBox_SelectedIndexChanged);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.leftPanelSplitter);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.editorsTabControl);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.filesControl);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.bottomPanelSplitter);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.findResultsPanel);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1200, 751);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1200, 800);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.lspServerToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.appDescriptionToolStrip);
            // 
            // leftPanelSplitter
            // 
            this.leftPanelSplitter.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.leftPanelSplitter.Location = new System.Drawing.Point(185, 0);
            this.leftPanelSplitter.Name = "leftPanelSplitter";
            this.leftPanelSplitter.Size = new System.Drawing.Size(3, 592);
            this.leftPanelSplitter.TabIndex = 4;
            this.leftPanelSplitter.TabStop = false;
            // 
            // bottomPanelSplitter
            // 
            this.bottomPanelSplitter.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.bottomPanelSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanelSplitter.Location = new System.Drawing.Point(0, 592);
            this.bottomPanelSplitter.Name = "bottomPanelSplitter";
            this.bottomPanelSplitter.Size = new System.Drawing.Size(1200, 3);
            this.bottomPanelSplitter.TabIndex = 3;
            this.bottomPanelSplitter.TabStop = false;
            // 
            // toolStrip1
            // 
            this.appDescriptionToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.appDescriptionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2});
            this.appDescriptionToolStrip.Location = new System.Drawing.Point(593, 24);
            this.appDescriptionToolStrip.Name = "toolStrip1";
            this.appDescriptionToolStrip.Size = new System.Drawing.Size(525, 25);
            this.appDescriptionToolStrip.TabIndex = 4;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(513, 22);
            this.toolStripLabel2.Text = "This demo shows how to use several LSP parsers together to edit code from the sam" +
    "e workspace";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LSP Multiple Files Demo";
            this.findResultsPanel.ResumeLayout(false);
            this.findResultsPanel.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.lspServerToolStrip.ResumeLayout(false);
            this.lspServerToolStrip.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.appDescriptionToolStrip.ResumeLayout(false);
            this.appDescriptionToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl editorsTabControl;
        private FilesControl filesControl;
        private System.Windows.Forms.Panel findResultsPanel;
        private System.Windows.Forms.Label findResultsCaptionLabel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip lspServerToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox lspServerComboBox;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Splitter leftPanelSplitter;
        private System.Windows.Forms.Splitter bottomPanelSplitter;
        private System.Windows.Forms.ToolStrip appDescriptionToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}