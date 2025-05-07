#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

namespace Alternet.FormDesigner.Demo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.contentTabControl = new VSControls.UxTabControl();
            this.LeftSplitter = new System.Windows.Forms.Splitter();
            this.leftTabControl = new VSControls.UxTabControl();
            this.propertiesTabPage = new System.Windows.Forms.TabPage();
            this.propertyGridControl = new Alternet.FormDesigner.WinForms.PropertyGridControl();
            this.filesTabPage = new System.Windows.Forms.TabPage();
            this.filesUserControl = new Alternet.FormDesigner.Demo.FilesUserControl();
            this.outlineTabPage = new System.Windows.Forms.TabPage();
            this.outlineControl = new Alternet.FormDesigner.WinForms.OutlineControl();
            this.toolboxControl = new Alternet.FormDesigner.Demo.CustomToolboxControl();
            this.PropertyGridSplitter = new System.Windows.Forms.Splitter();
            this.ToolboxSplitter = new System.Windows.Forms.Splitter();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.topsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeSameSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.HorzSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeEqualHorzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseHorzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseHorzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHorzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vertSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeEqualVertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseVertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseVertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeVertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.conterInFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.orderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.lockControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smartTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.gridSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridSizeSmallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridSizeLargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActiveFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addControlsFromAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolboxFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolboxToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.ToolStripContainer.ContentPanel.SuspendLayout();
            this.ToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer.SuspendLayout();
            this.leftTabControl.SuspendLayout();
            this.propertiesTabPage.SuspendLayout();
            this.filesTabPage.SuspendLayout();
            this.outlineTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStripContainer
            // 
            // 
            // ToolStripContainer.BottomToolStripPanel
            // 
            this.ToolStripContainer.BottomToolStripPanel.Controls.Add(this.StatusStrip);
            // 
            // ToolStripContainer.ContentPanel
            // 
            this.ToolStripContainer.ContentPanel.Controls.Add(this.contentTabControl);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.LeftSplitter);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.leftTabControl);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.PropertyGridSplitter);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.ToolboxSplitter);
            this.ToolStripContainer.ContentPanel.Controls.Add(this.toolboxControl);
            this.ToolStripContainer.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ToolStripContainer.ContentPanel.Size = new System.Drawing.Size(1481, 784);
            this.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.ToolStripContainer.Name = "ToolStripContainer";
            this.ToolStripContainer.Size = new System.Drawing.Size(1481, 830);
            this.ToolStripContainer.TabIndex = 3;
            this.ToolStripContainer.Text = "toolStripContainer1";
            // 
            // ToolStripContainer.TopToolStripPanel
            // 
            this.ToolStripContainer.TopToolStripPanel.Controls.Add(this.MenuStrip);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.StatusStrip.Location = new System.Drawing.Point(0, 0);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1481, 22);
            this.StatusStrip.TabIndex = 0;
            // 
            // contentTabControl
            // 
            this.contentTabControl.Alignment = System.Windows.Forms.TabAlignment.Top;
            this.contentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTabControl.Location = new System.Drawing.Point(350, 0);
            this.contentTabControl.Name = "contentTabControl";
            this.contentTabControl.SelectedIndex = 0;
            this.contentTabControl.Size = new System.Drawing.Size(744, 783);
            this.contentTabControl.TabIndex = 7;
            this.contentTabControl.SelectedIndexChanged += new System.EventHandler(this.ContentTabControl_SelectedIndexChanged);
            // 
            // LeftSplitter
            // 
            this.LeftSplitter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LeftSplitter.Location = new System.Drawing.Point(347, 0);
            this.LeftSplitter.Name = "LeftSplitter";
            this.LeftSplitter.Size = new System.Drawing.Size(3, 783);
            this.LeftSplitter.TabIndex = 6;
            this.LeftSplitter.TabStop = false;
            // 
            // leftTabControl
            // 
            this.leftTabControl.Controls.Add(this.propertiesTabPage);
            this.leftTabControl.Controls.Add(this.filesTabPage);
            this.leftTabControl.Controls.Add(this.outlineTabPage);
            this.leftTabControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftTabControl.Location = new System.Drawing.Point(3, 0);
            this.leftTabControl.Multiline = true;
            this.leftTabControl.Name = "leftTabControl";
            this.leftTabControl.SelectedIndex = 0;
            this.leftTabControl.Size = new System.Drawing.Size(380, 783);
            this.leftTabControl.TabIndex = 5;
            // 
            // propertiesTabPage
            // 
            this.propertiesTabPage.Controls.Add(this.propertyGridControl);
            this.propertiesTabPage.Location = new System.Drawing.Point(4, 4);
            this.propertiesTabPage.Name = "propertiesTabPage";
            this.propertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertiesTabPage.Size = new System.Drawing.Size(336, 755);
            this.propertiesTabPage.TabIndex = 0;
            this.propertiesTabPage.Text = "Properties";
            this.propertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGridControl
            // 
            this.propertyGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControl.Location = new System.Drawing.Point(3, 3);
            this.propertyGridControl.Name = "propertyGridControl";
            this.propertyGridControl.Size = new System.Drawing.Size(330, 749);
            this.propertyGridControl.TabIndex = 3;
            // 
            // filesTabPage
            // 
            this.filesTabPage.Controls.Add(this.filesUserControl);
            this.filesTabPage.Location = new System.Drawing.Point(4, 4);
            this.filesTabPage.Name = "filesTabPage";
            this.filesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.filesTabPage.Size = new System.Drawing.Size(336, 757);
            this.filesTabPage.TabIndex = 1;
            this.filesTabPage.Text = "Files";
            this.filesTabPage.UseVisualStyleBackColor = true;
            // 
            // filesUserControl
            // 
            this.filesUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesUserControl.Location = new System.Drawing.Point(3, 3);
            this.filesUserControl.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.filesUserControl.Name = "filesUserControl";
            this.filesUserControl.Size = new System.Drawing.Size(330, 751);
            this.filesUserControl.TabIndex = 0;
            this.filesUserControl.NewFormButtonClick += new System.EventHandler(this.FilesUserControl_NewFormButtonClick);
            // 
            // outlineTabPage
            // 
            this.outlineTabPage.Controls.Add(this.outlineControl);
            this.outlineTabPage.Location = new System.Drawing.Point(4, 4);
            this.outlineTabPage.Name = "outlineTabPage";
            this.outlineTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outlineTabPage.Size = new System.Drawing.Size(336, 757);
            this.outlineTabPage.TabIndex = 2;
            this.outlineTabPage.Text = "Outline";
            this.outlineTabPage.UseVisualStyleBackColor = true;
            // 
            // outlineControl
            // 
            this.outlineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outlineControl.FormDesignerControl = null;
            this.outlineControl.Location = new System.Drawing.Point(3, 3);
            this.outlineControl.Name = "outlineControl";
            this.outlineControl.Size = new System.Drawing.Size(330, 751);
            this.outlineControl.TabIndex = 0;
            this.outlineControl.Text = "outlineControl1";
            this.outlineControl.Toolbox = this.toolboxControl;
            // 
            // toolboxControl
            // 
            this.toolboxControl.AutoScroll = true;
            this.toolboxControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolboxControl.Location = new System.Drawing.Point(1097, 0);
            this.toolboxControl.Name = "toolboxControl";
            this.toolboxControl.Size = new System.Drawing.Size(384, 783);
            this.toolboxControl.TabIndex = 1;
            this.toolboxControl.PlaceItemAtDefaultLocation += new System.EventHandler<Alternet.FormDesigner.WinForms.PlaceToolboxItemAtDefaultLocationEventArgs>(this.ToolboxControl_PlaceItemAtDefaultLocation);
            // 
            // PropertyGridSplitter
            // 
            this.PropertyGridSplitter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PropertyGridSplitter.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridSplitter.Name = "PropertyGridSplitter";
            this.PropertyGridSplitter.Size = new System.Drawing.Size(3, 783);
            this.PropertyGridSplitter.TabIndex = 4;
            this.PropertyGridSplitter.TabStop = false;
            // 
            // ToolboxSplitter
            // 
            this.ToolboxSplitter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ToolboxSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.ToolboxSplitter.Location = new System.Drawing.Point(1094, 0);
            this.ToolboxSplitter.Name = "ToolboxSplitter";
            this.ToolboxSplitter.Size = new System.Drawing.Size(3, 783);
            this.ToolboxSplitter.TabIndex = 2;
            this.ToolboxSplitter.TabStop = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.runToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1481, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.newToolStripMenuItem.Text = "&New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator1,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator2,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alignToolStripMenuItem,
            this.makeSameSizeToolStripMenuItem,
            this.toolStripSeparator6,
            this.HorzSpacingToolStripMenuItem,
            this.vertSpacingToolStripMenuItem,
            this.toolStripSeparator9,
            this.conterInFormToolStripMenuItem,
            this.toolStripSeparator8,
            this.orderToolStripMenuItem,
            this.toolStripSeparator7,
            this.lockControlsToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // alignToolStripMenuItem
            // 
            this.alignToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftsToolStripMenuItem,
            this.centresToolStripMenuItem,
            this.rightsToolStripMenuItem,
            this.toolStripSeparator4,
            this.topsToolStripMenuItem,
            this.middlesToolStripMenuItem,
            this.bottomsToolStripMenuItem,
            this.toolStripSeparator5,
            this.toGridToolStripMenuItem});
            this.alignToolStripMenuItem.Name = "alignToolStripMenuItem";
            this.alignToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.alignToolStripMenuItem.Text = "Align";
            // 
            // leftsToolStripMenuItem
            // 
            this.leftsToolStripMenuItem.Name = "leftsToolStripMenuItem";
            this.leftsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.leftsToolStripMenuItem.Text = "Lefts";
            this.leftsToolStripMenuItem.Click += new System.EventHandler(this.LeftsToolStripMenuItem_Click);
            // 
            // centresToolStripMenuItem
            // 
            this.centresToolStripMenuItem.Name = "centresToolStripMenuItem";
            this.centresToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.centresToolStripMenuItem.Text = "Centres";
            this.centresToolStripMenuItem.Click += new System.EventHandler(this.CentresToolStripMenuItem_Click);
            // 
            // rightsToolStripMenuItem
            // 
            this.rightsToolStripMenuItem.Name = "rightsToolStripMenuItem";
            this.rightsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.rightsToolStripMenuItem.Text = "Rights";
            this.rightsToolStripMenuItem.Click += new System.EventHandler(this.RightsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(116, 6);
            // 
            // topsToolStripMenuItem
            // 
            this.topsToolStripMenuItem.Name = "topsToolStripMenuItem";
            this.topsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.topsToolStripMenuItem.Text = "Tops";
            this.topsToolStripMenuItem.Click += new System.EventHandler(this.TopsToolStripMenuItem_Click);
            // 
            // middlesToolStripMenuItem
            // 
            this.middlesToolStripMenuItem.Name = "middlesToolStripMenuItem";
            this.middlesToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.middlesToolStripMenuItem.Text = "Middles";
            this.middlesToolStripMenuItem.Click += new System.EventHandler(this.MiddlesToolStripMenuItem_Click);
            // 
            // bottomsToolStripMenuItem
            // 
            this.bottomsToolStripMenuItem.Name = "bottomsToolStripMenuItem";
            this.bottomsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.bottomsToolStripMenuItem.Text = "Bottoms";
            this.bottomsToolStripMenuItem.Click += new System.EventHandler(this.BottomsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(116, 6);
            // 
            // toGridToolStripMenuItem
            // 
            this.toGridToolStripMenuItem.Name = "toGridToolStripMenuItem";
            this.toGridToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.toGridToolStripMenuItem.Text = "to Grid";
            this.toGridToolStripMenuItem.Click += new System.EventHandler(this.ToGridToolStripMenuItem_Click);
            // 
            // makeSameSizeToolStripMenuItem
            // 
            this.makeSameSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widthToolStripMenuItem,
            this.heightToolStripMenuItem,
            this.bothToolStripMenuItem,
            this.sizeToGridToolStripMenuItem});
            this.makeSameSizeToolStripMenuItem.Name = "makeSameSizeToolStripMenuItem";
            this.makeSameSizeToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.makeSameSizeToolStripMenuItem.Text = "Make Same Size";
            // 
            // widthToolStripMenuItem
            // 
            this.widthToolStripMenuItem.Name = "widthToolStripMenuItem";
            this.widthToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.widthToolStripMenuItem.Text = "Width";
            this.widthToolStripMenuItem.Click += new System.EventHandler(this.WidthToolStripMenuItem_Click);
            // 
            // heightToolStripMenuItem
            // 
            this.heightToolStripMenuItem.Name = "heightToolStripMenuItem";
            this.heightToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.heightToolStripMenuItem.Text = "Height";
            this.heightToolStripMenuItem.Click += new System.EventHandler(this.HeightToolStripMenuItem_Click);
            // 
            // bothToolStripMenuItem
            // 
            this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
            this.bothToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.bothToolStripMenuItem.Text = "Both";
            this.bothToolStripMenuItem.Click += new System.EventHandler(this.BothToolStripMenuItem_Click);
            // 
            // sizeToGridToolStripMenuItem
            // 
            this.sizeToGridToolStripMenuItem.Name = "sizeToGridToolStripMenuItem";
            this.sizeToGridToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.sizeToGridToolStripMenuItem.Text = "SizeToGrid";
            this.sizeToGridToolStripMenuItem.Click += new System.EventHandler(this.SizeToGridToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(171, 6);
            // 
            // HorzSpacingToolStripMenuItem
            // 
            this.HorzSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makeEqualHorzToolStripMenuItem,
            this.increaseHorzToolStripMenuItem,
            this.decreaseHorzToolStripMenuItem,
            this.removeHorzToolStripMenuItem});
            this.HorzSpacingToolStripMenuItem.Name = "HorzSpacingToolStripMenuItem";
            this.HorzSpacingToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.HorzSpacingToolStripMenuItem.Text = "Horizontal Spacing";
            // 
            // makeEqualHorzToolStripMenuItem
            // 
            this.makeEqualHorzToolStripMenuItem.Name = "makeEqualHorzToolStripMenuItem";
            this.makeEqualHorzToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.makeEqualHorzToolStripMenuItem.Text = "Make Equal";
            this.makeEqualHorzToolStripMenuItem.Click += new System.EventHandler(this.MakeEqualHorzToolStripMenuItem_Click);
            // 
            // increaseHorzToolStripMenuItem
            // 
            this.increaseHorzToolStripMenuItem.Name = "increaseHorzToolStripMenuItem";
            this.increaseHorzToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.increaseHorzToolStripMenuItem.Text = "Increase";
            this.increaseHorzToolStripMenuItem.Click += new System.EventHandler(this.IncreaseHorzToolStripMenuItem_Click);
            // 
            // decreaseHorzToolStripMenuItem
            // 
            this.decreaseHorzToolStripMenuItem.Name = "decreaseHorzToolStripMenuItem";
            this.decreaseHorzToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.decreaseHorzToolStripMenuItem.Text = "Decrease";
            this.decreaseHorzToolStripMenuItem.Click += new System.EventHandler(this.DecreaseHorzToolStripMenuItem_Click);
            // 
            // removeHorzToolStripMenuItem
            // 
            this.removeHorzToolStripMenuItem.Name = "removeHorzToolStripMenuItem";
            this.removeHorzToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.removeHorzToolStripMenuItem.Text = "Remove";
            this.removeHorzToolStripMenuItem.Click += new System.EventHandler(this.RemoveHorzToolStripMenuItem_Click);
            // 
            // vertSpacingToolStripMenuItem
            // 
            this.vertSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makeEqualVertToolStripMenuItem,
            this.increaseVertToolStripMenuItem,
            this.decreaseVertToolStripMenuItem,
            this.removeVertToolStripMenuItem});
            this.vertSpacingToolStripMenuItem.Name = "vertSpacingToolStripMenuItem";
            this.vertSpacingToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.vertSpacingToolStripMenuItem.Text = "Vertical Spacing";
            // 
            // makeEqualVertToolStripMenuItem
            // 
            this.makeEqualVertToolStripMenuItem.Name = "makeEqualVertToolStripMenuItem";
            this.makeEqualVertToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.makeEqualVertToolStripMenuItem.Text = "Make Equal";
            this.makeEqualVertToolStripMenuItem.Click += new System.EventHandler(this.MakeEqualVertToolStripMenuItem_Click);
            // 
            // increaseVertToolStripMenuItem
            // 
            this.increaseVertToolStripMenuItem.Name = "increaseVertToolStripMenuItem";
            this.increaseVertToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.increaseVertToolStripMenuItem.Text = "Increase";
            this.increaseVertToolStripMenuItem.Click += new System.EventHandler(this.IncreaseVertToolStripMenuItem_Click);
            // 
            // decreaseVertToolStripMenuItem
            // 
            this.decreaseVertToolStripMenuItem.Name = "decreaseVertToolStripMenuItem";
            this.decreaseVertToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.decreaseVertToolStripMenuItem.Text = "Decrease";
            this.decreaseVertToolStripMenuItem.Click += new System.EventHandler(this.DecreaseVertToolStripMenuItem_Click);
            // 
            // removeVertToolStripMenuItem
            // 
            this.removeVertToolStripMenuItem.Name = "removeVertToolStripMenuItem";
            this.removeVertToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.removeVertToolStripMenuItem.Text = "Remove";
            this.removeVertToolStripMenuItem.Click += new System.EventHandler(this.RemoveVertToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(171, 6);
            // 
            // conterInFormToolStripMenuItem
            // 
            this.conterInFormToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontallyToolStripMenuItem,
            this.verticallyToolStripMenuItem});
            this.conterInFormToolStripMenuItem.Name = "conterInFormToolStripMenuItem";
            this.conterInFormToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.conterInFormToolStripMenuItem.Text = "Center in Form";
            // 
            // horizontallyToolStripMenuItem
            // 
            this.horizontallyToolStripMenuItem.Name = "horizontallyToolStripMenuItem";
            this.horizontallyToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.horizontallyToolStripMenuItem.Text = "Horizontally";
            this.horizontallyToolStripMenuItem.Click += new System.EventHandler(this.HorizontallyToolStripMenuItem_Click);
            // 
            // verticallyToolStripMenuItem
            // 
            this.verticallyToolStripMenuItem.Name = "verticallyToolStripMenuItem";
            this.verticallyToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.verticallyToolStripMenuItem.Text = "Vertically";
            this.verticallyToolStripMenuItem.Click += new System.EventHandler(this.VerticallyToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(171, 6);
            // 
            // orderToolStripMenuItem
            // 
            this.orderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bringToFrontToolStripMenuItem,
            this.sendToBackToolStripMenuItem});
            this.orderToolStripMenuItem.Name = "orderToolStripMenuItem";
            this.orderToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.orderToolStripMenuItem.Text = "Order";
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.bringToFrontToolStripMenuItem.Text = "Bring to Front";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.BringToFrontToolStripMenuItem_Click);
            // 
            // sendToBackToolStripMenuItem
            // 
            this.sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            this.sendToBackToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.sendToBackToolStripMenuItem.Text = "Send to Back";
            this.sendToBackToolStripMenuItem.Click += new System.EventHandler(this.SendToBackToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(171, 6);
            // 
            // lockControlsToolStripMenuItem
            // 
            this.lockControlsToolStripMenuItem.Name = "lockControlsToolStripMenuItem";
            this.lockControlsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.lockControlsToolStripMenuItem.Text = "Lock Controls";
            this.lockControlsToolStripMenuItem.Click += new System.EventHandler(this.LockControlsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showGridToolStripMenuItem,
            this.snapToGridToolStripMenuItem,
            this.snapLinesToolStripMenuItem,
            this.smartTagsToolStripMenuItem,
            this.toolStripSeparator10,
            this.gridSizeToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            this.showGridToolStripMenuItem.Click += new System.EventHandler(this.ShowGridToolStripMenuItem_Click);
            // 
            // snapToGridToolStripMenuItem
            // 
            this.snapToGridToolStripMenuItem.CheckOnClick = true;
            this.snapToGridToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.snapToGridToolStripMenuItem.Name = "snapToGridToolStripMenuItem";
            this.snapToGridToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.snapToGridToolStripMenuItem.Text = "Snap To Grid";
            this.snapToGridToolStripMenuItem.Click += new System.EventHandler(this.SnapToGridToolStripMenuItem_Click);
            // 
            // snapLinesToolStripMenuItem
            // 
            this.snapLinesToolStripMenuItem.CheckOnClick = true;
            this.snapLinesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.snapLinesToolStripMenuItem.Name = "snapLinesToolStripMenuItem";
            this.snapLinesToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.snapLinesToolStripMenuItem.Text = "Snap Lines";
            this.snapLinesToolStripMenuItem.Click += new System.EventHandler(this.SnapLinesToolStripMenuItem_Click);
            // 
            // smartTagsToolStripMenuItem
            // 
            this.smartTagsToolStripMenuItem.CheckOnClick = true;
            this.smartTagsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.smartTagsToolStripMenuItem.Name = "smartTagsToolStripMenuItem";
            this.smartTagsToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.smartTagsToolStripMenuItem.Text = "Smart Tags";
            this.smartTagsToolStripMenuItem.Click += new System.EventHandler(this.SmartTagsToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(138, 6);
            // 
            // gridSizeToolStripMenuItem
            // 
            this.gridSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridSizeSmallToolStripMenuItem,
            this.gridSizeLargeToolStripMenuItem});
            this.gridSizeToolStripMenuItem.Name = "gridSizeToolStripMenuItem";
            this.gridSizeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.gridSizeToolStripMenuItem.Text = "Grid Size";
            // 
            // gridSizeSmallToolStripMenuItem
            // 
            this.gridSizeSmallToolStripMenuItem.CheckOnClick = true;
            this.gridSizeSmallToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridSizeSmallToolStripMenuItem.Name = "gridSizeSmallToolStripMenuItem";
            this.gridSizeSmallToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.gridSizeSmallToolStripMenuItem.Text = "Small";
            this.gridSizeSmallToolStripMenuItem.Click += new System.EventHandler(this.GridSizeSmallToolStripMenuItem_Click);
            // 
            // gridSizeLargeToolStripMenuItem
            // 
            this.gridSizeLargeToolStripMenuItem.CheckOnClick = true;
            this.gridSizeLargeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.gridSizeLargeToolStripMenuItem.Name = "gridSizeLargeToolStripMenuItem";
            this.gridSizeLargeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.gridSizeLargeToolStripMenuItem.Text = "Large";
            this.gridSizeLargeToolStripMenuItem.Click += new System.EventHandler(this.GridSizeLargeToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runActiveFormToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "&Run";
            // 
            // runActiveFormToolStripMenuItem
            // 
            this.runActiveFormToolStripMenuItem.Name = "runActiveFormToolStripMenuItem";
            this.runActiveFormToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.runActiveFormToolStripMenuItem.Text = "&Run Active Form";
            this.runActiveFormToolStripMenuItem.Click += new System.EventHandler(this.RunActiveFormToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addControlsFromAssemblyToolStripMenuItem,
            this.resetToolboxToolStripMenuItem,
            this.loadToolboxFromFileToolStripMenuItem,
            this.saveToolboxToFileToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // addControlsFromAssemblyToolStripMenuItem
            // 
            this.addControlsFromAssemblyToolStripMenuItem.Name = "addControlsFromAssemblyToolStripMenuItem";
            this.addControlsFromAssemblyToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.addControlsFromAssemblyToolStripMenuItem.Text = "Add Controls from Assembly...";
            this.addControlsFromAssemblyToolStripMenuItem.Click += new System.EventHandler(this.AddControlsFromAssemblyToolStripMenuItem_Click);
            // 
            // resetToolboxToolStripMenuItem
            // 
            this.resetToolboxToolStripMenuItem.Name = "resetToolboxToolStripMenuItem";
            this.resetToolboxToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.resetToolboxToolStripMenuItem.Text = "Reset Toolbox";
            this.resetToolboxToolStripMenuItem.Click += new System.EventHandler(this.ResetToolboxToolStripMenuItem_Click);
            // 
            // loadToolboxFromFileToolStripMenuItem
            // 
            this.loadToolboxFromFileToolStripMenuItem.Name = "loadToolboxFromFileToolStripMenuItem";
            this.loadToolboxFromFileToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.loadToolboxFromFileToolStripMenuItem.Text = "Load Toolbox From File...";
            this.loadToolboxFromFileToolStripMenuItem.Click += new System.EventHandler(this.LoadToolboxFromFileToolStripMenuItem_Click);
            // 
            // saveToolboxToFileToolStripMenuItem
            // 
            this.saveToolboxToFileToolStripMenuItem.Name = "saveToolboxToFileToolStripMenuItem";
            this.saveToolboxToFileToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.saveToolboxToFileToolStripMenuItem.Text = "Save Toolbox To File...";
            this.saveToolboxToFileToolStripMenuItem.Click += new System.EventHandler(this.SaveToolboxToFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1481, 830);
            this.Controls.Add(this.ToolStripContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Designer Demo";
            this.ToolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer.BottomToolStripPanel.PerformLayout();
            this.ToolStripContainer.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer.ResumeLayout(false);
            this.ToolStripContainer.PerformLayout();
            this.leftTabControl.ResumeLayout(false);
            this.propertiesTabPage.ResumeLayout(false);
            this.filesTabPage.ResumeLayout(false);
            this.outlineTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolboxControl)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private CustomToolboxControl toolboxControl;
        private System.Windows.Forms.ToolStripContainer ToolStripContainer;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.Splitter ToolboxSplitter;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Splitter PropertyGridSplitter;
        private Alternet.FormDesigner.WinForms.PropertyGridControl propertyGridControl;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private VSControls.UxTabControl leftTabControl;
        private System.Windows.Forms.TabPage propertiesTabPage;
        private System.Windows.Forms.TabPage filesTabPage;
        private System.Windows.Forms.Splitter LeftSplitter;
        private VSControls.UxTabControl contentTabControl;
        private Alternet.FormDesigner.Demo.FilesUserControl filesUserControl;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runActiveFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addControlsFromAssemblyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolboxToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolboxFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem lockControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smartTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridSizeSmallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridSizeLargeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem topsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem middlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bottomsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem HorzSpacingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeEqualHorzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseHorzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseHorzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHorzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vertSpacingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeEqualVertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseVertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseVertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeVertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem conterInFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem horizontallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem makeSameSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem widthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToGridToolStripMenuItem;
        private System.Windows.Forms.TabPage outlineTabPage;
        private WinForms.OutlineControl outlineControl;
    }
}

