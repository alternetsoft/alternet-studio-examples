namespace Alternet.CodeEditorSyntax.Demo
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.mnuFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.cmReferences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFileContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addReferenceContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeReferenceContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmEditors = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removePageContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.printPreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.codeExplorerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoMenunItem = new System.Windows.Forms.ToolStripMenuItem();
            this.samplesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sslPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslModified = new System.Windows.Forms.ToolStripStatusLabel();
            this.sslOverwrite = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcEditors = new System.Windows.Forms.TabControl();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.tcBottom = new System.Windows.Forms.TabControl();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.lvErrors = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpEventLog = new System.Windows.Forms.TabPage();
            this.lbEvents = new System.Windows.Forms.ListBox();
            this.tpFindResults = new System.Windows.Forms.TabPage();
            this.ucFindResults = new Alternet.Editor.Common.FindResults();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnCombo = new System.Windows.Forms.TableLayoutPanel();
            this.cbMethods = new System.Windows.Forms.ComboBox();
            this.cbClasses = new System.Windows.Forms.ComboBox();
            this.gbExplorer = new System.Windows.Forms.GroupBox();
            this.tvSyntax = new System.Windows.Forms.TreeView();
            this.tsStandard = new System.Windows.Forms.ToolStrip();
            this.openToolButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolButton = new System.Windows.Forms.ToolStripButton();
            this.undoToolButton = new System.Windows.Forms.ToolStripButton();
            this.redoToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolButton = new System.Windows.Forms.ToolStripButton();
            this.replaceToolButton = new System.Windows.Forms.ToolStripButton();
            this.gotoToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printPreviewToolButton = new System.Windows.Forms.ToolStripButton();
            this.printToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.openContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.findContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.gotoDefinitionContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReferencesContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findImplementationsContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmReferences.SuspendLayout();
            this.cmEditors.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tcBottom.SuspendLayout();
            this.tpMessages.SuspendLayout();
            this.tpEventLog.SuspendLayout();
            this.tpFindResults.SuspendLayout();
            this.pnCombo.SuspendLayout();
            this.gbExplorer.SuspendLayout();
            this.tsStandard.SuspendLayout();
            this.cmMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "doc1";
            // 
            // mnuFiles
            // 
            this.mnuFiles.Name = "mnuFiles";
            this.mnuFiles.OwnerItem = this.newStripSplitButton;
            this.mnuFiles.Size = new System.Drawing.Size(61, 4);
            // 
            // newStripSplitButton
            // 
            this.newStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newStripSplitButton.DropDown = this.mnuFiles;
            this.newStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newStripSplitButton.Name = "tsbNew";
            this.newStripSplitButton.Size = new System.Drawing.Size(32, 22);
            this.newStripSplitButton.ToolTipText = "New";
            // 
            // cmReferences
            // 
            this.cmReferences.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.cmReferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileContextMenuItem,
            this.removeFileContextMenuItem,
            this.addReferenceContextMenuItem,
            this.removeReferenceContextMenuItem});
            this.cmReferences.Name = "cmReferences";
            this.cmReferences.Size = new System.Drawing.Size(173, 92);
            this.cmReferences.Opening += new System.ComponentModel.CancelEventHandler(this.References_MenuItemOpening);
            // 
            // addFileContextMenuItem
            // 
            this.addFileContextMenuItem.Name = "addFileContextMenuItem";
            this.addFileContextMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addFileContextMenuItem.Text = "Add File";
            this.addFileContextMenuItem.Click += new System.EventHandler(this.AddFile_MenuItemClick);
            // 
            // removeFileContextMenuItem
            // 
            this.removeFileContextMenuItem.Name = "removeFileContextMenuItem";
            this.removeFileContextMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeFileContextMenuItem.Text = "Remove File";
            this.removeFileContextMenuItem.Click += new System.EventHandler(this.RemoveFile_MenuItemClick);
            // 
            // addReferenceContextMenuItem
            // 
            this.addReferenceContextMenuItem.Name = "addReferenceContextMenuItem";
            this.addReferenceContextMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addReferenceContextMenuItem.Text = "Add Reference";
            this.addReferenceContextMenuItem.Click += new System.EventHandler(this.AddReference_MenuItemClick);
            // 
            // removeReferenceContextMenuItem
            // 
            this.removeReferenceContextMenuItem.Name = "removeReferenceContextMenuItem";
            this.removeReferenceContextMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeReferenceContextMenuItem.Text = "Remove Reference";
            this.removeReferenceContextMenuItem.Click += new System.EventHandler(this.RemoveReference_MenuItemClick);
            // 
            // cmEditors
            // 
            this.cmEditors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removePageContextMenuItem});
            this.cmEditors.Name = "cmEditors";
            this.cmEditors.Size = new System.Drawing.Size(147, 26);
            this.cmEditors.Opening += new System.ComponentModel.CancelEventHandler(this.Editors_MenuItemOpening);
            // 
            // removePageContextMenuItem
            // 
            this.removePageContextMenuItem.Name = "removePageContextMenuItem";
            this.removePageContextMenuItem.Size = new System.Drawing.Size(146, 22);
            this.removePageContextMenuItem.Text = "Remove Page";
            this.removePageContextMenuItem.Click += new System.EventHandler(this.RemovePage_MenuItemClick);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.searchMenuItem,
            this.samplesMenuItem,
            this.helpMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1924, 24);
            this.mainMenu.TabIndex = 5;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.newProjectMenuItem,
            this.openMenuItem,
            this.closeFileMenuItem,
            this.toolStripMenuItem1,
            this.openProjectMenuItem,
            this.saveProjectMenuItem,
            this.closeProjectMenuItem,
            this.toolStripSeparator6,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.saveAllMenuItem,
            this.toolStripMenuItem2,
            this.printPreviewMenuItem,
            this.printMenuItem,
            this.pageSetupMenuItem,
            this.toolStripMenuItem3,
            this.codeExplorerMenuItem,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "&File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newMenuItem.Text = "&New";
            // 
            // newProjectMenuItem
            // 
            this.newProjectMenuItem.Name = "newProjectMenuItem";
            this.newProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newProjectMenuItem.Text = "New Project...";
            this.newProjectMenuItem.Click += new System.EventHandler(this.NewProject_MenuItemClick);
            // 
            // openMenuItem
            // 
            this.openMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openMenuItem.Text = "&Open...";
            this.openMenuItem.Click += new System.EventHandler(this.Open_MenuItemClick);
            // 
            // closeFileMenuItem
            // 
            this.closeFileMenuItem.Name = "closeFileMenuItem";
            this.closeFileMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeFileMenuItem.Text = "Close";
            this.closeFileMenuItem.Click += new System.EventHandler(this.CloseFile_MenuItemClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // openProjectMenuItem
            // 
            this.openProjectMenuItem.Name = "openProjectMenuItem";
            this.openProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openProjectMenuItem.Text = "Open Project...";
            this.openProjectMenuItem.Click += new System.EventHandler(this.OpenProject_MenuItemClick);
            // 
            // saveProjectMenuItem
            // 
            this.saveProjectMenuItem.Name = "saveProjectMenuItem";
            this.saveProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveProjectMenuItem.Text = "Save Project";
            this.saveProjectMenuItem.Click += new System.EventHandler(this.SaveProject_MenuItemClick);
            // 
            // closeProjectMenuItem
            // 
            this.closeProjectMenuItem.Name = "closeProjectMenuItem";
            this.closeProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeProjectMenuItem.Text = "Close Project";
            this.closeProjectMenuItem.Click += new System.EventHandler(this.CloseProject_MenuItemClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(184, 6);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.Save_MenuItemClick);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsMenuItem.Text = "Save &As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAs_MenuItemClick);
            // 
            // saveAllMenuItem
            // 
            this.saveAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAllMenuItem.Name = "saveAllMenuItem";
            this.saveAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAllMenuItem.Text = "Save All";
            this.saveAllMenuItem.Click += new System.EventHandler(this.SaveAll_MenuItemClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 6);
            // 
            // printPreviewMenuItem
            // 
            this.printPreviewMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.printPreviewMenuItem.Name = "printPreviewMenuItem";
            this.printPreviewMenuItem.Size = new System.Drawing.Size(187, 22);
            this.printPreviewMenuItem.Text = "Print Preview...";
            this.printPreviewMenuItem.Click += new System.EventHandler(this.PrintPreview_MenuItemClick);
            // 
            // printMenuItem
            // 
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
            this.printMenuItem.Size = new System.Drawing.Size(187, 22);
            this.printMenuItem.Text = "Print...";
            this.printMenuItem.Click += new System.EventHandler(this.Print_MenuItemClick);
            // 
            // pageSetupMenuItem
            // 
            this.pageSetupMenuItem.Name = "pageSetupMenuItem";
            this.pageSetupMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pageSetupMenuItem.Text = "Page Setup...";
            this.pageSetupMenuItem.Click += new System.EventHandler(this.PageSetup_MenuItemClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(184, 6);
            // 
            // codeExplorerMenuItem
            // 
            this.codeExplorerMenuItem.Name = "codeExplorerMenuItem";
            this.codeExplorerMenuItem.Size = new System.Drawing.Size(187, 22);
            this.codeExplorerMenuItem.Text = "Close Code Explorer";
            this.codeExplorerMenuItem.Click += new System.EventHandler(this.CodeExplorer_MenuItemClick);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.Exit_MenuItemClick);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.toolStripMenuItem4,
            this.cutMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.toolStripMenuItem5,
            this.selectAllMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editMenuItem.Text = "&Edit";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoMenuItem.Text = "&Undo";
            this.undoMenuItem.Click += new System.EventHandler(this.Undo_MenuItemClick);
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoMenuItem.Text = "Redo";
            this.redoMenuItem.Click += new System.EventHandler(this.Redo_MenuItemClick);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(161, 6);
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Name = "cutMenuItem";
            this.cutMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cutMenuItem.Text = "Cut";
            this.cutMenuItem.Click += new System.EventHandler(this.Cut_MenuItemClick);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.Copy_MenuItemClick);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteMenuItem.Text = "Paste";
            this.pasteMenuItem.Click += new System.EventHandler(this.Paste_MenuItemClick);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Name = "selectAllMenuItem";
            this.selectAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllMenuItem.Text = "Select All";
            this.selectAllMenuItem.Click += new System.EventHandler(this.SelectAll_MenuItemClick);
            // 
            // searchMenuItem
            // 
            this.searchMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findMenuItem,
            this.replaceMenuItem,
            this.gotoMenunItem});
            this.searchMenuItem.Name = "searchMenuItem";
            this.searchMenuItem.Size = new System.Drawing.Size(54, 20);
            this.searchMenuItem.Text = "&Search";
            // 
            // findMenuItem
            // 
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(226, 22);
            this.findMenuItem.Text = "Find...";
            this.findMenuItem.Click += new System.EventHandler(this.Find_MenuItemClick);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(226, 22);
            this.replaceMenuItem.Text = "Replace...";
            this.replaceMenuItem.Click += new System.EventHandler(this.Replace_MenuItemClick);
            // 
            // gotoMenunItem
            // 
            this.gotoMenunItem.Name = "gotoMenunItem";
            this.gotoMenunItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gotoMenunItem.Size = new System.Drawing.Size(226, 22);
            this.gotoMenunItem.Text = "Go to Line Number...";
            this.gotoMenunItem.Click += new System.EventHandler(this.Goto_MenuItemClick);
            // 
            // samplesMenuItem
            // 
            this.samplesMenuItem.Name = "samplesMenuItem";
            this.samplesMenuItem.Size = new System.Drawing.Size(63, 20);
            this.samplesMenuItem.Text = "Samples";
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpMenuItem.Text = "&Help";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutMenuItem.Text = "About CodeEditor";
            this.aboutMenuItem.Click += new System.EventHandler(this.About_MenuItemClick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tcEditors);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitter2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tcBottom);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitter1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pnCombo);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.gbExplorer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1924, 990);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1924, 1037);
            this.toolStripContainer1.TabIndex = 6;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsStandard);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslPosition,
            this.sslModified,
            this.sslOverwrite});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1924, 22);
            this.statusStrip1.TabIndex = 19;
            // 
            // sslPosition
            // 
            this.sslPosition.AutoSize = false;
            this.sslPosition.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.sslPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sslPosition.Name = "sslPosition";
            this.sslPosition.Size = new System.Drawing.Size(144, 17);
            // 
            // sslModified
            // 
            this.sslModified.AutoSize = false;
            this.sslModified.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.sslModified.Name = "sslModified";
            this.sslModified.Size = new System.Drawing.Size(95, 17);
            // 
            // sslOverwrite
            // 
            this.sslOverwrite.Name = "sslOverwrite";
            this.sslOverwrite.Size = new System.Drawing.Size(0, 17);
            // 
            // tcEditors
            // 
            this.tcEditors.ContextMenuStrip = this.cmEditors;
            this.tcEditors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEditors.Location = new System.Drawing.Point(0, 27);
            this.tcEditors.Name = "tcEditors";
            this.tcEditors.SelectedIndex = 0;
            this.tcEditors.Size = new System.Drawing.Size(1407, 720);
            this.tcEditors.TabIndex = 37;
            this.tcEditors.SelectedIndexChanged += new System.EventHandler(this.EditorsTabControl_SelectedIndexChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 747);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1407, 3);
            this.splitter2.TabIndex = 39;
            this.splitter2.TabStop = false;
            // 
            // tcBottom
            // 
            this.tcBottom.Controls.Add(this.tpMessages);
            this.tcBottom.Controls.Add(this.tpEventLog);
            this.tcBottom.Controls.Add(this.tpFindResults);
            this.tcBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tcBottom.Location = new System.Drawing.Point(0, 750);
            this.tcBottom.Name = "tcBottom";
            this.tcBottom.SelectedIndex = 0;
            this.tcBottom.Size = new System.Drawing.Size(1407, 240);
            this.tcBottom.TabIndex = 35;
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.lvErrors);
            this.tpMessages.Location = new System.Drawing.Point(4, 24);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessages.Size = new System.Drawing.Size(1399, 212);
            this.tpMessages.TabIndex = 0;
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // lvErrors
            // 
            this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvErrors.FullRowSelect = true;
            this.lvErrors.GridLines = true;
            this.lvErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvErrors.HideSelection = false;
            this.lvErrors.Location = new System.Drawing.Point(3, 3);
            this.lvErrors.MultiSelect = false;
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new System.Drawing.Size(1393, 206);
            this.lvErrors.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvErrors.TabIndex = 2;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = System.Windows.Forms.View.Details;
            this.lvErrors.DoubleClick += new System.EventHandler(this.ErrorsLisiewTreeView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Line";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Col";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Description";
            this.columnHeader3.Width = 900;
            // 
            // tpEventLog
            // 
            this.tpEventLog.Controls.Add(this.lbEvents);
            this.tpEventLog.Location = new System.Drawing.Point(4, 22);
            this.tpEventLog.Name = "tpEventLog";
            this.tpEventLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpEventLog.Size = new System.Drawing.Size(1464, 214);
            this.tpEventLog.TabIndex = 1;
            this.tpEventLog.Text = "Event Log";
            this.tpEventLog.UseVisualStyleBackColor = true;
            // 
            // lbEvents
            // 
            this.lbEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEvents.FormattingEnabled = true;
            this.lbEvents.ItemHeight = 15;
            this.lbEvents.Location = new System.Drawing.Point(3, 3);
            this.lbEvents.Name = "lbEvents";
            this.lbEvents.Size = new System.Drawing.Size(1458, 208);
            this.lbEvents.TabIndex = 1;
            // 
            // tpFindResults
            // 
            this.tpFindResults.Controls.Add(this.ucFindResults);
            this.tpFindResults.Location = new System.Drawing.Point(4, 22);
            this.tpFindResults.Name = "tpFindResults";
            this.tpFindResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpFindResults.Size = new System.Drawing.Size(1464, 214);
            this.tpFindResults.TabIndex = 2;
            this.tpFindResults.Text = "FindResults";
            this.tpFindResults.UseVisualStyleBackColor = true;
            // 
            // ucFindResults
            // 
            this.ucFindResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucFindResults.Location = new System.Drawing.Point(3, 3);
            this.ucFindResults.Margin = new System.Windows.Forms.Padding(26);
            this.ucFindResults.Name = "ucFindResults";
            this.ucFindResults.Size = new System.Drawing.Size(1458, 208);
            this.ucFindResults.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1407, 27);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 963);
            this.splitter1.TabIndex = 38;
            this.splitter1.TabStop = false;
            // 
            // pnCombo
            // 
            this.pnCombo.AutoSize = true;
            this.pnCombo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnCombo.ColumnCount = 2;
            this.pnCombo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnCombo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnCombo.Controls.Add(this.cbMethods, 1, 0);
            this.pnCombo.Controls.Add(this.cbClasses, 0, 0);
            this.pnCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnCombo.Location = new System.Drawing.Point(0, 0);
            this.pnCombo.Name = "pnCombo";
            this.pnCombo.RowCount = 1;
            this.pnCombo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnCombo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.pnCombo.Size = new System.Drawing.Size(1410, 27);
            this.pnCombo.TabIndex = 31;
            // 
            // cbMethods
            // 
            this.cbMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMethods.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMethods.FormattingEnabled = true;
            this.cbMethods.Location = new System.Drawing.Point(708, 3);
            this.cbMethods.Name = "cbMethods";
            this.cbMethods.Size = new System.Drawing.Size(699, 24);
            this.cbMethods.Sorted = true;
            this.cbMethods.TabIndex = 1;
            this.cbMethods.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MethodsComboBox_DrawItem);
            this.cbMethods.SelectedIndexChanged += new System.EventHandler(this.MethodsComboBox_SelectedIndexChanged);
            this.cbMethods.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.MethodsComboBox_Format);
            // 
            // cbClasses
            // 
            this.cbClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbClasses.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClasses.FormattingEnabled = true;
            this.cbClasses.Location = new System.Drawing.Point(3, 3);
            this.cbClasses.Name = "cbClasses";
            this.cbClasses.Size = new System.Drawing.Size(699, 24);
            this.cbClasses.Sorted = true;
            this.cbClasses.TabIndex = 0;
            this.cbClasses.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MethodsComboBox_DrawItem);
            this.cbClasses.SelectedIndexChanged += new System.EventHandler(this.MethodsComboBox_SelectedIndexChanged);
            this.cbClasses.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.MethodsComboBox_Format);
            // 
            // gbExplorer
            // 
            this.gbExplorer.Controls.Add(this.tvSyntax);
            this.gbExplorer.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbExplorer.Location = new System.Drawing.Point(1410, 0);
            this.gbExplorer.Name = "gbExplorer";
            this.gbExplorer.Size = new System.Drawing.Size(514, 990);
            this.gbExplorer.TabIndex = 29;
            this.gbExplorer.TabStop = false;
            this.gbExplorer.Text = "Project Explorer";
            // 
            // tvSyntax
            // 
            this.tvSyntax.ContextMenuStrip = this.cmReferences;
            this.tvSyntax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSyntax.ImageIndex = 0;
            this.tvSyntax.Location = new System.Drawing.Point(3, 19);
            this.tvSyntax.Name = "tvSyntax";
            this.tvSyntax.SelectedImageIndex = 0;
            this.tvSyntax.Size = new System.Drawing.Size(508, 968);
            this.tvSyntax.TabIndex = 2;
            this.tvSyntax.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.SyntaxTreeView_BeforeCollapse);
            this.tvSyntax.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.SyntaxTreeView_BeforeExpand);
            this.tvSyntax.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SyntaxTreeView_NodeMouseDoubleClick);
            this.tvSyntax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SyntaxTreeView_MouseDown);
            // 
            // tsStandard
            // 
            this.tsStandard.Dock = System.Windows.Forms.DockStyle.None;
            this.tsStandard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStripSplitButton,
            this.openToolButton,
            this.saveToolButton,
            this.toolStripSeparator1,
            this.cutToolButton,
            this.copyToolButton,
            this.pasteToolButton,
            this.undoToolButton,
            this.redoToolButton,
            this.toolStripSeparator2,
            this.findToolButton,
            this.replaceToolButton,
            this.gotoToolButton,
            this.toolStripSeparator3,
            this.printPreviewToolButton,
            this.printToolButton,
            this.toolStripSeparator4});
            this.tsStandard.Location = new System.Drawing.Point(3, 0);
            this.tsStandard.Name = "tsStandard";
            this.tsStandard.Size = new System.Drawing.Size(344, 25);
            this.tsStandard.TabIndex = 3;
            this.tsStandard.Text = "toolStrip1";
            this.tsStandard.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StandardToolStrip_ItemClicked);
            // 
            // tsbOpen
            // 
            this.openToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolButton.Name = "tsbOpen";
            this.openToolButton.Size = new System.Drawing.Size(23, 22);
            this.openToolButton.ToolTipText = "Open";
            // 
            // tsbSave
            // 
            this.saveToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolButton.Name = "tsbSave";
            this.saveToolButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolButton.ToolTipText = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.cutToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolButton.Name = "tsbCut";
            this.cutToolButton.Size = new System.Drawing.Size(23, 22);
            this.cutToolButton.ToolTipText = "Cut";
            // 
            // tsbCopy
            // 
            this.copyToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolButton.Name = "tsbCopy";
            this.copyToolButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolButton.ToolTipText = "Copy";
            // 
            // tsbPaste
            // 
            this.pasteToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolButton.Name = "tsbPaste";
            this.pasteToolButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolButton.ToolTipText = "Paste";
            // 
            // tsbUndo
            // 
            this.undoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolButton.Name = "tsbUndo";
            this.undoToolButton.Size = new System.Drawing.Size(23, 22);
            this.undoToolButton.ToolTipText = "Undo";
            // 
            // tsbRedo
            // 
            this.redoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoToolButton.Name = "tsbRedo";
            this.redoToolButton.Size = new System.Drawing.Size(23, 22);
            this.redoToolButton.ToolTipText = "Redo";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbFind
            // 
            this.findToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findToolButton.Name = "tsbFind";
            this.findToolButton.Size = new System.Drawing.Size(23, 22);
            this.findToolButton.ToolTipText = "Find";
            // 
            // tsbReplace
            // 
            this.replaceToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.replaceToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.replaceToolButton.Name = "tsbReplace";
            this.replaceToolButton.Size = new System.Drawing.Size(23, 22);
            this.replaceToolButton.ToolTipText = "Replace";
            // 
            // tsbGoto
            // 
            this.gotoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.gotoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gotoToolButton.Name = "tsbGoto";
            this.gotoToolButton.Size = new System.Drawing.Size(23, 22);
            this.gotoToolButton.ToolTipText = "Goto";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPrintPreview
            // 
            this.printPreviewToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printPreviewToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolButton.Name = "tsbPrintPreview";
            this.printPreviewToolButton.Size = new System.Drawing.Size(23, 22);
            this.printPreviewToolButton.ToolTipText = "Print Preview";
            // 
            // tsbPrint
            // 
            this.printToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolButton.Name = "tsbPrint";
            this.printToolButton.Size = new System.Drawing.Size(23, 22);
            this.printToolButton.ToolTipText = "Print";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cmMain
            // 
            this.cmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutContextMenuItem,
            this.copyContextMenuItem,
            this.pasteContextMenuItem,
            this.toolStripMenuItem6,
            this.openContextMenuItem,
            this.toolStripMenuItem7,
            this.findContextMenuItem,
            this.replaceContextMenuItem,
            this.gotoContextMenuItem,
            this.toolStripMenuItem11,
            this.gotoDefinitionContextMenuItem,
            this.findReferencesContextMenuItem,
            this.findImplementationsContextMenuItem,
            this.toolStripMenuItem10,
            this.aboutContextMenuItem});
            this.cmMain.Name = "cmMain";
            this.cmMain.Size = new System.Drawing.Size(277, 270);
            this.cmMain.Opening += new System.ComponentModel.CancelEventHandler(this.MainContextMenu_Opening);
            // 
            // cutContextMenuItem
            // 
            this.cutContextMenuItem.Name = "cutContextMenuItem";
            this.cutContextMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.cutContextMenuItem.Text = "Cut";
            this.cutContextMenuItem.Click += new System.EventHandler(this.Cut_MenuItemClick);
            // 
            // copyContextMenuItem
            // 
            this.copyContextMenuItem.Name = "copyContextMenuItem";
            this.copyContextMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.copyContextMenuItem.Text = "Copy";
            this.copyContextMenuItem.Click += new System.EventHandler(this.Copy_MenuItemClick);
            // 
            // pasteContextMenuItem
            // 
            this.pasteContextMenuItem.Name = "pasteContextMenuItem";
            this.pasteContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.pasteContextMenuItem.Text = "Paste";
            this.pasteContextMenuItem.Click += new System.EventHandler(this.Paste_MenuItemClick);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(273, 6);
            // 
            // openContextMenuItem
            // 
            this.openContextMenuItem.Name = "openContextMenuItem";
            this.openContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.openContextMenuItem.Text = "Open...";
            this.openContextMenuItem.Click += new System.EventHandler(this.Open_MenuItemClick);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(273, 6);
            // 
            // findContextMenuItem
            // 
            this.findContextMenuItem.Name = "findContextMenuItem";
            this.findContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.findContextMenuItem.Text = "Find";
            this.findContextMenuItem.Click += new System.EventHandler(this.Find_MenuItemClick);
            // 
            // replaceContextMenuItem
            // 
            this.replaceContextMenuItem.Name = "replaceContextMenuItem";
            this.replaceContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.replaceContextMenuItem.Text = "Replace";
            this.replaceContextMenuItem.Click += new System.EventHandler(this.Replace_MenuItemClick);
            // 
            // gotoContextMenuItem
            // 
            this.gotoContextMenuItem.Name = "gotoContextMenuItem";
            this.gotoContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gotoContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.gotoContextMenuItem.Text = "Go to Line Number";
            this.gotoContextMenuItem.Click += new System.EventHandler(this.Goto_MenuItemClick);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(273, 6);
            // 
            // gotoDefinitionContextMenuItem
            // 
            this.gotoDefinitionContextMenuItem.Name = "gotoDefinitionContextMenuItem";
            this.gotoDefinitionContextMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.gotoDefinitionContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.gotoDefinitionContextMenuItem.Text = "Go to Definition";
            this.gotoDefinitionContextMenuItem.Click += new System.EventHandler(this.GotoDefinition_MenuItemClick);
            // 
            // findReferencesContextMenuItem
            // 
            this.findReferencesContextMenuItem.Name = "findReferencesContextMenuItem";
            this.findReferencesContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F12)));
            this.findReferencesContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.findReferencesContextMenuItem.Text = "Find References";
            this.findReferencesContextMenuItem.Click += new System.EventHandler(this.FindReferences_MenuItemClick);
            // 
            // findImplementationsContextMenuItem
            // 
            this.findImplementationsContextMenuItem.Name = "indImplementationsContextMenuItem";
            this.findImplementationsContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F12)));
            this.findImplementationsContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.findImplementationsContextMenuItem.Text = "Go To Implementation";
            this.findImplementationsContextMenuItem.Click += new System.EventHandler(this.FindImplementations_MenuItemClick);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(273, 6);
            // 
            // aboutContextMenuItem
            // 
            this.aboutContextMenuItem.Name = "aboutContextMenuItem";
            this.aboutContextMenuItem.Size = new System.Drawing.Size(276, 22);
            this.aboutContextMenuItem.Text = "About CodeEditor";
            this.aboutContextMenuItem.Click += new System.EventHandler(this.About_MenuItemClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1061);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.Text = "Syntax Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.cmReferences.ResumeLayout(false);
            this.cmEditors.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tcBottom.ResumeLayout(false);
            this.tpMessages.ResumeLayout(false);
            this.tpEventLog.ResumeLayout(false);
            this.tpFindResults.ResumeLayout(false);
            this.pnCombo.ResumeLayout(false);
            this.gbExplorer.ResumeLayout(false);
            this.tsStandard.ResumeLayout(false);
            this.tsStandard.PerformLayout();
            this.cmMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem openProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ContextMenuStrip mnuFiles;
        private System.Windows.Forms.ContextMenuStrip cmReferences;
        private System.Windows.Forms.ContextMenuStrip cmEditors;
        private System.Windows.Forms.ToolStripSplitButton newStripSplitButton;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem printPreviewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSetupMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem codeExplorerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem cutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem selectAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem samplesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addReferenceContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeReferenceContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFileContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFileContextMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip tsStandard;
        private System.Windows.Forms.ToolStripButton openToolButton;
        private System.Windows.Forms.ToolStripButton saveToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton cutToolButton;
        private System.Windows.Forms.ToolStripButton copyToolButton;
        private System.Windows.Forms.ToolStripButton pasteToolButton;
        private System.Windows.Forms.ToolStripButton undoToolButton;
        private System.Windows.Forms.ToolStripButton redoToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton findToolButton;
        private System.Windows.Forms.ToolStripButton replaceToolButton;
        private System.Windows.Forms.ToolStripButton gotoToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton printPreviewToolButton;
        private System.Windows.Forms.ToolStripButton printToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ContextMenuStrip cmMain;
        private System.Windows.Forms.ToolStripMenuItem cutContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem openContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem findContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem aboutContextMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sslPosition;
        private System.Windows.Forms.ToolStripStatusLabel sslModified;
        private System.Windows.Forms.ToolStripStatusLabel sslOverwrite;
        private System.Windows.Forms.ToolStripMenuItem removePageContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoMenunItem;
        private System.Windows.Forms.ToolStripMenuItem gotoContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoDefinitionContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findReferencesContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findImplementationsContextMenuItem;
        private System.Windows.Forms.GroupBox gbExplorer;
        private System.Windows.Forms.TreeView tvSyntax;
        private System.Windows.Forms.ComboBox cbMethods;
        private System.Windows.Forms.ComboBox cbClasses;
        private System.Windows.Forms.TabControl tcBottom;
        private System.Windows.Forms.TabPage tpMessages;
        private System.Windows.Forms.TabPage tpEventLog;
        private System.Windows.Forms.TabPage tpFindResults;
        private System.Windows.Forms.TabControl tcEditors;
        private System.Windows.Forms.ListView lvErrors;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListBox lbEvents;
        private Alternet.Editor.Common.FindResults ucFindResults;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TableLayoutPanel pnCombo;

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
    }
}