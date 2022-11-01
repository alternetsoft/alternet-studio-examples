namespace AlternetStudio.Demo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Implementing interface method in the separate region")]
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
            this.filesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.cmReferences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addReferenceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeReferenceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFormMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.openProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.printPreviewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.undoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDesignerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startWithoutDebugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToCursorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoDefinitionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runnerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.standardToolStrip = new System.Windows.Forms.ToolStrip();
            this.debuggerControlToolbar = new Alternet.Scripter.Debugger.UI.DebuggerControlToolbar();
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
            this.historyForwardToolButton = new System.Windows.Forms.ToolStripButton();
            this.historyBackwardToolSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.historyBackwardContextMenu = new System.Windows.Forms.ContextMenuStrip();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.positionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.modifiedStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.overwriteStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.projectExplorerTreeView = new System.Windows.Forms.TreeView();
            this.codeNavigationBarPanel = new System.Windows.Forms.Panel();
            this.methodsComboBox = new System.Windows.Forms.ComboBox();
            this.classesComboBox = new System.Windows.Forms.ComboBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.editorsTabControl = new System.Windows.Forms.TabControl();
            this.breakpointsTabPage = new System.Windows.Forms.TabPage();
            this.breakpointsControl = new Alternet.Scripter.Debugger.UI.Breakpoints();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.outputControl = new Alternet.Scripter.Debugger.UI.Output();
            this.findResultsTabPage = new System.Windows.Forms.TabPage();
            this.findResultsControl = new Alternet.Editor.Common.FindResults();
            this.bottomTabControl = new System.Windows.Forms.TabControl();
            this.callStackTabPage = new System.Windows.Forms.TabPage();
            this.callStackControl = new Alternet.Scripter.Debugger.UI.CallStack();
            this.watchesTabPage = new System.Windows.Forms.TabPage();
            this.watchesControl = new Alternet.Scripter.Debugger.UI.Watches();
            this.errorsTabPage = new System.Windows.Forms.TabPage();
            this.errorsControl = new Alternet.Scripter.Debugger.UI.Errors();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.rightTabControl = new System.Windows.Forms.TabControl();
            this.projectExplorerTabPage = new System.Windows.Forms.TabPage();
            this.codeExplorerTabPage = new System.Windows.Forms.TabPage();
            this.codeExplorerTreeView = new System.Windows.Forms.TreeView();
            this.propertiesTabPage = new System.Windows.Forms.TabPage();
            this.toolboxTabPage = new System.Windows.Forms.TabPage();
            this.debugMenu = new Alternet.Scripter.Debugger.UI.DebugMenu();
            this.cmReferences.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.standardToolStrip.SuspendLayout();
            this.debuggerControlToolbar.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.codeNavigationBarPanel.SuspendLayout();
            this.breakpointsTabPage.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.findResultsTabPage.SuspendLayout();
            this.bottomTabControl.SuspendLayout();
            this.callStackTabPage.SuspendLayout();
            this.watchesTabPage.SuspendLayout();
            this.errorsTabPage.SuspendLayout();
            this.rightTabControl.SuspendLayout();
            this.projectExplorerTabPage.SuspendLayout();
            this.codeExplorerTabPage.SuspendLayout();
            this.propertiesTabPage.SuspendLayout();
            this.toolboxTabPage.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.standardToolStrip);
            this.flowLayoutPanel1.Controls.Add(this.debuggerControlToolbar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 26);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Typescript files (*.ts)|*.ts|Javascript files (*.js)|*.js|Project Files (*.json)|" +
    "*.json|Any files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "doc1";
            this.saveFileDialog.Filter = "Typescript files (*.ts)|*.ts|Javascript files (*.js)|*.js|Project Files (*.json)|" +
    "*.json|Any files (*.*)|*.*";
            // 
            // filesMenuStrip
            // 
            this.filesMenuStrip.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.filesMenuStrip.Name = "filesMenuStrip";
            this.filesMenuStrip.OwnerItem = this.newStripSplitButton;
            this.filesMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // newStripSplitButton
            // 
            this.newStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newStripSplitButton.DropDown = this.filesMenuStrip;
            this.newStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newStripSplitButton.Name = "newStripSplitButton";
            this.newStripSplitButton.Size = new System.Drawing.Size(32, 22);
            this.newStripSplitButton.ToolTipText = "New";
            // 
            // cmReferences
            // 
            this.cmReferences.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.cmReferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileMenuItem,
            this.removeFileMenuItem,
            this.addReferenceMenuItem,
            this.removeReferenceMenuItem,
            this.moveUpMenuItem,
            this.moveDownMenuItem});
            this.cmReferences.Name = "cmReferences";
            this.cmReferences.Size = new System.Drawing.Size(173, 136);
            this.cmReferences.Opening += new System.ComponentModel.CancelEventHandler(this.ReferencesContextMenu_Opening);
            // 
            // addFileMenuItem
            // 
            this.addFileMenuItem.Name = "addFileMenuItem";
            this.addFileMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addFileMenuItem.Text = "Add File";
            this.addFileMenuItem.Click += new System.EventHandler(this.AddFileMenuItem_Click);
            // 
            // removeFileMenuItem
            // 
            this.removeFileMenuItem.Name = "removeFileMenuItem";
            this.removeFileMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeFileMenuItem.Text = "Remove File";
            this.removeFileMenuItem.Click += new System.EventHandler(this.RemoveFileMenuItem_Click);
            // 
            // addReferenceMenuItem
            // 
            this.addReferenceMenuItem.Name = "addReferenceMenuItem";
            this.addReferenceMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addReferenceMenuItem.Text = "Add Reference";
            this.addReferenceMenuItem.Click += new System.EventHandler(this.AddReferenceMenuItem_Click);
            // 
            // removeReferenceMenuItem
            // 
            this.removeReferenceMenuItem.Name = "removeReferenceMenuItem";
            this.removeReferenceMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeReferenceMenuItem.Text = "Remove Reference";
            this.removeReferenceMenuItem.Click += new System.EventHandler(this.RemoveReferenceMenuItem_Click);
            // 
            // moveUpMenuItem
            // 
            this.moveUpMenuItem.Name = "moveUpMenuItem";
            this.moveUpMenuItem.Size = new System.Drawing.Size(172, 22);
            this.moveUpMenuItem.Text = "Move Up";
            this.moveUpMenuItem.Click += new System.EventHandler(this.MoveUpMenuItem_Click);
            // 
            // moveDownMenuItem
            // 
            this.moveDownMenuItem.Name = "moveDownMenuItem";
            this.moveDownMenuItem.Size = new System.Drawing.Size(172, 22);
            this.moveDownMenuItem.Text = "Move Down";
            this.moveDownMenuItem.Click += new System.EventHandler(this.MoveDownMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.viewMenuItem,
            this.miSearch,
            this.debugMenu,
            this.runnerMenuItem,
            this.helpMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(1605, 24);
            this.mainMenu.TabIndex = 5;
            this.mainMenu.Text = "menuStrip1";
            // 
            // debugMenu
            // 
            this.debugMenu.Debugger = null;
            this.debugMenu.Name = "miDebug";
            this.debugMenu.Size = new System.Drawing.Size(54, 20);
            this.debugMenu.Text = "Debug";
            this.debugMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToCursorMenuItem,
            this.gotoDefinitionMenuItem,
            this.findReferencesMenuItem});    
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.closeFileMenuItem,
            this.toolStripSeparator5,
            this.openProjectMenuItem,
            this.saveProjectMenuItem,
            this.closeProjectMenuItem,
            this.toolStripMenuItem1,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.saveAllMenuItem,
            this.toolStripMenuItem2,
            this.printPreviewMenuItem,
            this.printMenuItem,
            this.toolStripMenuItem3,
            this.exitMenuItem});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newMenuItem.Text = "&New";
            this.newMenuItem.DropDown = this.filesMenuStrip;
            // 
            // newFormMenuItem
            // 
            this.newFormMenuItem.Name = "newFormMenuItem";
            this.newFormMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newFormMenuItem.Text = "New Form...";
            // 
            // newProjectMenuItem
            // 
            this.newProjectMenuItem.Name = "newProjectMenuItem";
            this.newProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newProjectMenuItem.Text = "New Project";
            this.newProjectMenuItem.Click += new System.EventHandler(this.NewProjectMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(149, 6);
            // 
            // openMenuItem
            // 
            this.openMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openMenuItem.Text = "&Open...";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // closeFileMenuItem
            // 
            this.closeFileMenuItem.Name = "closeFileMenuItem";
            this.closeFileMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeFileMenuItem.Text = "Close";
            this.closeFileMenuItem.Click += new System.EventHandler(this.CloseFileMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // openProjectMenuItem
            // 
            this.openProjectMenuItem.Name = "openProjectMenuItem";
            this.openProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openProjectMenuItem.Text = "Open Project...";
            this.openProjectMenuItem.Click += new System.EventHandler(this.OpenProjectMenuItem_Click);
            // 
            // saveProjectMenuItem
            // 
            this.saveProjectMenuItem.Name = "saveProjectMenuItem";
            this.saveProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveProjectMenuItem.Text = "Save Project";
            this.saveProjectMenuItem.Click += new System.EventHandler(this.SaveProjectMenuItem_Click);
            // 
            // closeProjectMenuItem
            // 
            this.closeProjectMenuItem.Name = "closeProjectMenuItem";
            this.closeProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProjectMenuItem.Text = "Close Project";
            this.closeProjectMenuItem.Click += new System.EventHandler(this.CloseProjectMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsMenuItem.Text = "Save &As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // saveAllMenuItem
            // 
            this.saveAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAllMenuItem.Name = "saveAllMenuItem";
            this.saveAllMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAllMenuItem.Text = "Save All";
            this.saveAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S)));
            this.saveAllMenuItem.Click += new System.EventHandler(this.SaveAllMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // printPreviewMenuItem
            // 
            this.printPreviewMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.printPreviewMenuItem.Name = "printPreviewMenuItem";
            this.printPreviewMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printPreviewMenuItem.Text = "Print Preview...";
            this.printPreviewMenuItem.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
            // 
            // printMenuItem
            // 
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printMenuItem.Text = "Print...";
            this.printMenuItem.Click += new System.EventHandler(this.PrintMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.toolStripMenuItem4,
            this.cutMenuItem,
            this.copyMenuItem,
            this.pasteMenuItem,
            this.toolStripMenuItem5,
            this.deleteMenuItem,
            this.toolStripSeparator6,
            this.selectAllMenuItem});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(39, 20);
            this.miEdit.Text = "&Edit";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Name = "undoMenuItem";
            this.undoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoMenuItem.Text = "&Undo";
            this.undoMenuItem.Click += new System.EventHandler(this.UndoMenuItem_Click);
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Name = "redoMenuItem";
            this.redoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoMenuItem.Text = "Redo";
            this.redoMenuItem.Click += new System.EventHandler(this.RedoMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(161, 6);
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Name = "cutMenuItem";
            this.cutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cutMenuItem.Text = "Cut";
            this.cutMenuItem.Click += new System.EventHandler(this.CutMenuItem_Click);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyMenuItem.Text = "Copy";
            this.copyMenuItem.Click += new System.EventHandler(this.CopyMenuItem_Click);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteMenuItem.Text = "Paste";
            this.pasteMenuItem.Click += new System.EventHandler(this.PasteMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(161, 6);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Name = "selectAllMenuItem";
            this.selectAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllMenuItem.Text = "Select All";
            this.selectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewCodeMenuItem,
            this.viewDesignerMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewMenuItem.Text = "&View";
            // 
            // viewCodeMenuItem
            // 
            this.viewCodeMenuItem.Name = "viewCodeMenuItem";
            this.viewCodeMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.viewCodeMenuItem.Size = new System.Drawing.Size(171, 22);
            this.viewCodeMenuItem.Text = "&Code";
            // 
            // viewDesignerMenuItem
            // 
            this.viewDesignerMenuItem.Name = "viewDesignerMenuItem";
            this.viewDesignerMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F7)));
            this.viewDesignerMenuItem.Size = new System.Drawing.Size(171, 22);
            this.viewDesignerMenuItem.Text = "&Designer";
            // 
            // miSearch
            // 
            this.miSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findMenuItem,
            this.replaceMenuItem,
            this.gotoMenuItem});
            this.miSearch.Name = "miSearch";
            this.miSearch.Size = new System.Drawing.Size(54, 20);
            this.miSearch.Text = "&Search";
            // 
            // findMenuItem
            // 
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(226, 22);
            this.findMenuItem.Text = "Find...";
            this.findMenuItem.Click += new System.EventHandler(this.FindMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(226, 22);
            this.replaceMenuItem.Text = "Replace...";
            this.replaceMenuItem.Click += new System.EventHandler(this.ReplaceMenuItem_Click);
            // 
            // gotoMenuItem
            // 
            this.gotoMenuItem.Name = "gotoMenuItem";
            this.gotoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gotoMenuItem.Size = new System.Drawing.Size(226, 22);
            this.gotoMenuItem.Text = "Go to Line Number...";
            this.gotoMenuItem.Click += new System.EventHandler(this.GotoMenuItem_Click);
            // 
            // startWithoutDebugMenuItem
            // 
            this.startWithoutDebugMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.startWithoutDebugMenuItem.Name = "startWithoutDebugMenuItem";
            this.startWithoutDebugMenuItem.Size = new System.Drawing.Size(214, 22);
            this.startWithoutDebugMenuItem.Text = "Start Without Debugging";
            this.startWithoutDebugMenuItem.Click += new System.EventHandler(this.StartWithoutDebugMenuItem_Click);
            // 
            // runToCursorMenuItem
            // 
            this.runToCursorMenuItem.Name = "runToCursorMenuItem";
            this.runToCursorMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F10)));
            this.runToCursorMenuItem.Size = new System.Drawing.Size(214, 22);
            this.runToCursorMenuItem.Text = "Run To Cursor";
            this.runToCursorMenuItem.Click += new System.EventHandler(this.RunToCursorMenuItem_Click);
            // 
            // gotoDefinitionMenuItem
            // 
            this.gotoDefinitionMenuItem.Name = "gotoDefinitionMenuItem";
            this.gotoDefinitionMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.gotoDefinitionMenuItem.Size = new System.Drawing.Size(214, 22);
            this.gotoDefinitionMenuItem.Text = "Go to Definition";
            this.gotoDefinitionMenuItem.Visible = false;
            this.gotoDefinitionMenuItem.Click += new System.EventHandler(this.GotoDefinitionMenuItem_Click);
            // 
            // findReferencesMenuItem
            // 
            this.findReferencesMenuItem.Name = "findReferencesMenuItem";
            this.findReferencesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F12)));
            this.findReferencesMenuItem.Size = new System.Drawing.Size(214, 22);
            this.findReferencesMenuItem.Text = "Find References";
            this.findReferencesMenuItem.Visible = false;
            this.findReferencesMenuItem.Click += new System.EventHandler(this.FindReferencesMenuItem_Click);
            // 
            // runnerMenuItem
            // 
            this.runnerMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptRunMenuItem});
            this.runnerMenuItem.Name = "runnerMenuItem";
            this.runnerMenuItem.Size = new System.Drawing.Size(57, 20);
            this.runnerMenuItem.Text = "Runner";
            this.runnerMenuItem.Visible = false;
            // 
            // scriptRunMenuItem
            // 
            this.scriptRunMenuItem.Name = "scriptRunMenuItem";
            this.scriptRunMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.scriptRunMenuItem.Size = new System.Drawing.Size(174, 22);
            this.scriptRunMenuItem.Text = "Run Script";
            this.scriptRunMenuItem.Click += new System.EventHandler(this.StartWithoutDebugMenuItem_Click);
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
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutMenuItem.Text = "About AlterNET Studio";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // standardToolStrip
            // 
            this.standardToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripSeparator4,
            this.historyBackwardToolSplitButton,
            this.historyForwardToolButton});
            this.standardToolStrip.Location = new System.Drawing.Point(0, 24);
            this.standardToolStrip.Name = "standardToolStrip";
            this.standardToolStrip.Size = new System.Drawing.Size(1605, 26);
            this.standardToolStrip.TabIndex = 3;
            this.standardToolStrip.Text = "toolStrip1";
            this.standardToolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.standardToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StandardToolStrip_ItemClicked);
            // 
            // debuggerControlToolbar
            // 
            this.debuggerControlToolbar.Location = new System.Drawing.Point(0, 24);
            this.debuggerControlToolbar.Name = "debuggerControlToolbar";
            this.debuggerControlToolbar.Size = new System.Drawing.Size(1605, 25);
            this.debuggerControlToolbar.TabIndex = 4;
            this.debuggerControlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            // 
            // openToolButton
            // 
            this.openToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolButton.Name = "openToolButton";
            this.openToolButton.Size = new System.Drawing.Size(23, 22);
            this.openToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.openToolButton.ToolTipText = "Open";
            // 
            // saveToolButton
            // 
            this.saveToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolButton.Name = "saveToolButton";
            this.saveToolButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.saveToolButton.ToolTipText = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cutToolButton
            // 
            this.cutToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolButton.Name = "cutToolButton";
            this.cutToolButton.Size = new System.Drawing.Size(23, 22);
            this.cutToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.cutToolButton.ToolTipText = "Cut";
            // 
            // copyToolButton
            // 
            this.copyToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolButton.Name = "copyToolButton";
            this.copyToolButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.copyToolButton.ToolTipText = "Copy";
            // 
            // pasteToolButton
            // 
            this.pasteToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolButton.Name = "pasteToolButton";
            this.pasteToolButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.pasteToolButton.ToolTipText = "Paste";
            // 
            // undoToolButton
            // 
            this.undoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolButton.Name = "undoToolButton";
            this.undoToolButton.Size = new System.Drawing.Size(23, 22);
            this.undoToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.undoToolButton.ToolTipText = "Undo";
            // 
            // redoToolButton
            // 
            this.redoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoToolButton.Name = "redoToolButton";
            this.redoToolButton.Size = new System.Drawing.Size(23, 22);
            this.redoToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.redoToolButton.ToolTipText = "Redo";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // findToolButton
            // 
            this.findToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findToolButton.Name = "findToolButton";
            this.findToolButton.Size = new System.Drawing.Size(23, 22);
            this.findToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.findToolButton.ToolTipText = "Find";
            // 
            // replaceToolButton
            // 
            this.replaceToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.replaceToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.replaceToolButton.Name = "replaceToolButton";
            this.replaceToolButton.Size = new System.Drawing.Size(23, 22);
            this.replaceToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.replaceToolButton.ToolTipText = "Replace";
            // 
            // gotoToolButton
            // 
            this.gotoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.gotoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gotoToolButton.Name = "gotoToolButton";
            this.gotoToolButton.Size = new System.Drawing.Size(23, 22);
            this.gotoToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.gotoToolButton.ToolTipText = "Goto";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // printPreviewToolButton
            // 
            this.printPreviewToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printPreviewToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolButton.Name = "printPreviewToolButton";
            this.printPreviewToolButton.Size = new System.Drawing.Size(23, 22);
            this.printPreviewToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.printPreviewToolButton.ToolTipText = "Print Preview";
            // 
            // printToolButton
            // 
            this.printToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolButton.Name = "printToolButton";
            this.printToolButton.Size = new System.Drawing.Size(23, 22);
            this.printToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.printToolButton.ToolTipText = "Print";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // historyForwardToolButton
            // 
            this.historyForwardToolButton.Name = "historyForwardToolButton";
            this.historyForwardToolButton.Size = new System.Drawing.Size(23, 22);
            this.historyForwardToolButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.historyForwardToolButton.ToolTipText = "Navigate Forward";
            // 
            // historyBackwardToolSplitButton
            // 
            this.historyBackwardToolSplitButton.Name = "historyBackwardToolSplitButton";
            this.historyBackwardToolSplitButton.Size = new System.Drawing.Size(23, 22);
            this.historyBackwardToolSplitButton.ToolTipText = "Navigate Backward";
            this.historyBackwardToolSplitButton.Margin = new System.Windows.Forms.Padding(2, 6, 2, 8);
            this.historyBackwardToolSplitButton.DropDown = historyBackwardContextMenu;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "CS_ProjectSENode_16x.png");
            this.imageList.Images.SetKeyName(1, "Reference.png");
            this.imageList.Images.SetKeyName(2, "Property.png");
            this.imageList.Images.SetKeyName(3, "CS_16x.png");
            this.imageList.Images.SetKeyName(4, "GenerateFile_16x.png");
            this.imageList.Images.SetKeyName(5, "WindowsForm_16x.png");
            this.imageList.Images.SetKeyName(6, "FileGroup_16x.png");
            this.imageList.Images.SetKeyName(7, "FolderClosed.png");
            this.imageList.Images.SetKeyName(8, "FolderOpened.png");
            this.imageList.Images.SetKeyName(9, "VB_16x.png");
            this.imageList.Images.SetKeyName(10, "WPFApplication_16x.png");
            this.imageList.Images.SetKeyName(11, "");
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.positionStatusLabel,
            this.modifiedStatusLabel,
            this.overwriteStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 891);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1605, 26);
            this.statusStrip.TabIndex = 19;
            // 
            // positionStatusLabel
            // 
            this.positionStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.positionStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.positionStatusLabel.Name = "positionStatusLabel";
            this.positionStatusLabel.Size = new System.Drawing.Size(144, 21);
            // 
            // modifiedStatusLabel
            // 
            this.modifiedStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.modifiedStatusLabel.Name = "modifiedStatusLabel";
            this.modifiedStatusLabel.Size = new System.Drawing.Size(95, 21);
            // 
            // overwriteStatusLabel
            // 
            this.overwriteStatusLabel.Name = "overwriteStatusLabel";
            this.overwriteStatusLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // projectExplorerTreeView
            // 
            this.projectExplorerTreeView.ContextMenuStrip = this.cmReferences;
            this.projectExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectExplorerTreeView.ImageIndex = 0;
            this.projectExplorerTreeView.ImageList = this.imageList;
            this.projectExplorerTreeView.Location = new System.Drawing.Point(3, 3);
            this.projectExplorerTreeView.Name = "projectExplorerTreeView";
            this.projectExplorerTreeView.SelectedImageIndex = 0;
            this.projectExplorerTreeView.Size = new System.Drawing.Size(633, 448);
            this.projectExplorerTreeView.TabIndex = 2;
            this.projectExplorerTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectExplorerTreeView_BeforeCollapse);
            this.projectExplorerTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectExplorerTreeView_BeforeExpand);
            this.projectExplorerTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectExplorerTreeView_NodeMouseDoubleClick);
            this.projectExplorerTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectExplorerTreeView_MouseDown);
            // 
            // codeNavigationBarPanel
            // 
            this.codeNavigationBarPanel.Controls.Add(this.methodsComboBox);
            this.codeNavigationBarPanel.Controls.Add(this.classesComboBox);
            this.codeNavigationBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.codeNavigationBarPanel.Location = new System.Drawing.Point(0, 49);
            this.codeNavigationBarPanel.Name = "codeNavigationBarPanel";
            this.codeNavigationBarPanel.Size = new System.Drawing.Size(953, 25);
            this.codeNavigationBarPanel.TabIndex = 30;
            this.codeNavigationBarPanel.Resize += new System.EventHandler(this.Panel1_Resize);
            // 
            // methodsComboBox
            // 
            this.methodsComboBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.methodsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.methodsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.methodsComboBox.FormattingEnabled = true;
            this.methodsComboBox.Location = new System.Drawing.Point(620, 0);
            this.methodsComboBox.Name = "methodsComboBox";
            this.methodsComboBox.Size = new System.Drawing.Size(333, 24);
            this.methodsComboBox.Sorted = true;
            this.methodsComboBox.TabIndex = 1;
            // 
            // classesComboBox
            // 
            this.classesComboBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.classesComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.classesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classesComboBox.FormattingEnabled = true;
            this.classesComboBox.Location = new System.Drawing.Point(0, 0);
            this.classesComboBox.Name = "classesComboBox";
            this.classesComboBox.Size = new System.Drawing.Size(333, 24);
            this.classesComboBox.Sorted = true;
            this.classesComboBox.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(953, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 482);
            this.splitter1.TabIndex = 34;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 531);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1605, 4);
            this.splitter2.TabIndex = 36;
            this.splitter2.TabStop = false;
            // 
            // editorsTabControl
            // 
            this.editorsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorsTabControl.Location = new System.Drawing.Point(0, 74);
            this.editorsTabControl.Name = "editorsTabControl";
            this.editorsTabControl.SelectedIndex = 0;
            this.editorsTabControl.Size = new System.Drawing.Size(953, 457);
            this.editorsTabControl.TabIndex = 37;
            this.editorsTabControl.SelectedIndexChanged += new System.EventHandler(this.EditorsTabControl_SelectedIndexChanged);
            this.editorsTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.EditorsTabControl_Selecting);
            // 
            // breakpointsTabPage
            // 
            this.breakpointsTabPage.Controls.Add(this.breakpointsControl);
            this.breakpointsTabPage.Location = new System.Drawing.Point(4, 24);
            this.breakpointsTabPage.Name = "breakpointsTabPage";
            this.breakpointsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.breakpointsTabPage.Size = new System.Drawing.Size(1597, 328);
            this.breakpointsTabPage.TabIndex = 3;
            this.breakpointsTabPage.Text = "Breakpoints";
            this.breakpointsTabPage.UseVisualStyleBackColor = true;
            // 
            // breakpointsControl
            // 
            this.breakpointsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.breakpointsControl.Location = new System.Drawing.Point(3, 3);
            this.breakpointsControl.Margin = new System.Windows.Forms.Padding(26);
            this.breakpointsControl.Name = "breakpointsControl";
            this.breakpointsControl.Size = new System.Drawing.Size(1591, 322);
            this.breakpointsControl.TabIndex = 0;
            this.breakpointsControl.BreakpointClick += new System.EventHandler<Alternet.Scripter.Debugger.UI.BreakpointClickEventArgs>(this.BreakpointsControl_BreakpointClick);
            this.breakpointsControl.BreakpointStateChanged += new System.EventHandler<Alternet.Scripter.Debugger.UI.BreakpointChangedEventArgs>(this.BreakpointsControl_BreakpointStateChanged);
            this.breakpointsControl.BreakpointDeleted += new System.EventHandler<Alternet.Scripter.Debugger.UI.BreakpointChangedEventArgs>(this.BreakpointsControl_BreakpointDeleted);
            // 
            // outputTabPage
            // 
            this.outputTabPage.Controls.Add(this.outputControl);
            this.outputTabPage.Location = new System.Drawing.Point(4, 24);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outputTabPage.Size = new System.Drawing.Size(1597, 328);
            this.outputTabPage.TabIndex = 2;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // outputControl
            // 
            this.outputControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputControl.Location = new System.Drawing.Point(3, 3);
            this.outputControl.Margin = new System.Windows.Forms.Padding(26);
            this.outputControl.Name = "outputControl";
            this.outputControl.Size = new System.Drawing.Size(1591, 322);
            this.outputControl.TabIndex = 0;
            // 
            // findResultsTabPage
            // 
            this.findResultsTabPage.Controls.Add(this.findResultsControl);
            this.findResultsTabPage.Location = new System.Drawing.Point(4, 24);
            this.findResultsTabPage.Name = "findResultsTabPage";
            this.findResultsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.findResultsTabPage.Size = new System.Drawing.Size(1597, 328);
            this.findResultsTabPage.TabIndex = 2;
            this.findResultsTabPage.Text = "FindResults";
            this.findResultsTabPage.UseVisualStyleBackColor = true;
            // 
            // findResultsControl
            // 
            this.findResultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findResultsControl.Location = new System.Drawing.Point(3, 3);
            this.findResultsControl.Margin = new System.Windows.Forms.Padding(26);
            this.findResultsControl.Name = "findResultsControl";
            this.findResultsControl.Size = new System.Drawing.Size(1591, 322);
            this.findResultsControl.TabIndex = 0;
            // 
            // bottomTabControl
            // 
            this.bottomTabControl.Controls.Add(this.outputTabPage);
            this.bottomTabControl.Controls.Add(this.breakpointsTabPage);
            this.bottomTabControl.Controls.Add(this.callStackTabPage);
            this.bottomTabControl.Controls.Add(this.watchesTabPage);
            this.bottomTabControl.Controls.Add(this.errorsTabPage);
            this.bottomTabControl.Controls.Add(this.findResultsTabPage);
            this.bottomTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomTabControl.Location = new System.Drawing.Point(0, 535);
            this.bottomTabControl.Name = "bottomTabControl";
            this.bottomTabControl.SelectedIndex = 0;
            this.bottomTabControl.Size = new System.Drawing.Size(1605, 356);
            this.bottomTabControl.TabIndex = 35;
            this.bottomTabControl.SelectedIndexChanged += new System.EventHandler(this.BottomTabControl_SelectedIndexChanged);
            // 
            // callStackTabPage
            // 
            this.callStackTabPage.Controls.Add(this.callStackControl);
            this.callStackTabPage.Location = new System.Drawing.Point(4, 24);
            this.callStackTabPage.Name = "callStackTabPage";
            this.callStackTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.callStackTabPage.Size = new System.Drawing.Size(1597, 328);
            this.callStackTabPage.TabIndex = 4;
            this.callStackTabPage.Text = "Call Stack";
            this.callStackTabPage.UseVisualStyleBackColor = true;
            // 
            // callStackControl
            // 
            this.callStackControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callStackControl.Location = new System.Drawing.Point(3, 3);
            this.callStackControl.Margin = new System.Windows.Forms.Padding(26);
            this.callStackControl.Name = "callStackControl";
            this.callStackControl.Size = new System.Drawing.Size(1591, 322);
            this.callStackControl.TabIndex = 0;
            // 
            // watchesTabPage
            // 
            this.watchesTabPage.Controls.Add(this.watchesControl);
            this.watchesTabPage.Location = new System.Drawing.Point(4, 24);
            this.watchesTabPage.Name = "watchesTabPage";
            this.watchesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.watchesTabPage.Size = new System.Drawing.Size(1597, 328);
            this.watchesTabPage.TabIndex = 6;
            this.watchesTabPage.Text = "Watches";
            this.watchesTabPage.UseVisualStyleBackColor = true;
            // 
            // watchesControl
            // 
            this.watchesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchesControl.Location = new System.Drawing.Point(3, 3);
            this.watchesControl.Margin = new System.Windows.Forms.Padding(26);
            this.watchesControl.Name = "watchesControl";
            this.watchesControl.Size = new System.Drawing.Size(1591, 322);
            this.watchesControl.TabIndex = 0;
            // 
            // errorsTabPage
            // 
            this.errorsTabPage.Controls.Add(this.errorsControl);
            this.errorsTabPage.Location = new System.Drawing.Point(4, 24);
            this.errorsTabPage.Name = "errorsTabPage";
            this.errorsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.errorsTabPage.Size = new System.Drawing.Size(1597, 328);
            this.errorsTabPage.TabIndex = 7;
            this.errorsTabPage.Text = "Error List";
            this.errorsTabPage.UseVisualStyleBackColor = true;
            // 
            // errorsControl
            // 
            this.errorsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsControl.Location = new System.Drawing.Point(3, 3);
            this.errorsControl.Margin = new System.Windows.Forms.Padding(26);
            this.errorsControl.Name = "errorsControl";
            this.errorsControl.Size = new System.Drawing.Size(1591, 322);
            this.errorsControl.TabIndex = 0;
            this.errorsControl.ErrorClick += new System.EventHandler<Alternet.Scripter.Debugger.UI.ErrorClickEventArgs>(this.ErrorsControl_ErrorClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            this.imageList1.Images.SetKeyName(19, "");
            this.imageList1.Images.SetKeyName(20, "");
            this.imageList1.Images.SetKeyName(21, "");
            this.imageList1.Images.SetKeyName(22, "");
            this.imageList1.Images.SetKeyName(23, "");
            this.imageList1.Images.SetKeyName(24, "");
            this.imageList1.Images.SetKeyName(25, "");
            this.imageList1.Images.SetKeyName(26, "");
            this.imageList1.Images.SetKeyName(27, "");
            this.imageList1.Images.SetKeyName(28, "");
            this.imageList1.Images.SetKeyName(29, "");
            this.imageList1.Images.SetKeyName(30, "");
            this.imageList1.Images.SetKeyName(31, "");
            this.imageList1.Images.SetKeyName(32, "");
            this.imageList1.Images.SetKeyName(33, "");
            this.imageList1.Images.SetKeyName(34, "");
            this.imageList1.Images.SetKeyName(35, "");
            this.imageList1.Images.SetKeyName(36, "");
            this.imageList1.Images.SetKeyName(37, "");
            this.imageList1.Images.SetKeyName(38, "");
            this.imageList1.Images.SetKeyName(39, "");
            this.imageList1.Images.SetKeyName(40, "");
            this.imageList1.Images.SetKeyName(41, "");
            this.imageList1.Images.SetKeyName(42, "");
            this.imageList1.Images.SetKeyName(43, "");
            // 
            // rightTabControl
            // 
            this.rightTabControl.Controls.Add(this.projectExplorerTabPage);
            this.rightTabControl.Controls.Add(this.codeExplorerTabPage);
            this.rightTabControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightTabControl.Location = new System.Drawing.Point(958, 49);
            this.rightTabControl.Name = "rightTabControl";
            this.rightTabControl.SelectedIndex = 0;
            this.rightTabControl.Size = new System.Drawing.Size(647, 482);
            this.rightTabControl.TabIndex = 38;
            // 
            // projectExplorerTabPage
            // 
            this.projectExplorerTabPage.Controls.Add(this.projectExplorerTreeView);
            this.projectExplorerTabPage.Location = new System.Drawing.Point(4, 24);
            this.projectExplorerTabPage.Name = "projectExplorerTabPage";
            this.projectExplorerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.projectExplorerTabPage.Size = new System.Drawing.Size(639, 454);
            this.projectExplorerTabPage.TabIndex = 0;
            this.projectExplorerTabPage.Text = "Project Explorer";
            this.projectExplorerTabPage.UseVisualStyleBackColor = true;
            // 
            // codeExplorerTabPage
            // 
            this.codeExplorerTabPage.Controls.Add(this.codeExplorerTreeView);
            this.codeExplorerTabPage.Location = new System.Drawing.Point(4, 24);
            this.codeExplorerTabPage.Name = "codeExplorerTabPage";
            this.codeExplorerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.codeExplorerTabPage.Size = new System.Drawing.Size(639, 454);
            this.codeExplorerTabPage.TabIndex = 3;
            this.codeExplorerTabPage.Text = "Code Explorer";
            this.codeExplorerTabPage.UseVisualStyleBackColor = true;
            // 
            // codeExplorerTreeView
            // 
            this.codeExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeExplorerTreeView.ImageIndex = 0;
            this.codeExplorerTreeView.ImageList = this.imageList1;
            this.codeExplorerTreeView.Location = new System.Drawing.Point(3, 3);
            this.codeExplorerTreeView.Name = "codeExplorerTreeView";
            this.codeExplorerTreeView.SelectedImageIndex = 0;
            this.codeExplorerTreeView.Size = new System.Drawing.Size(633, 448);
            this.codeExplorerTreeView.TabIndex = 4;
            // 
            // propertiesTabPage
            // 
            this.propertiesTabPage.Location = new System.Drawing.Point(4, 24);
            this.propertiesTabPage.Name = "propertiesTabPage";
            this.propertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertiesTabPage.Size = new System.Drawing.Size(639, 454);
            this.propertiesTabPage.TabIndex = 1;
            this.propertiesTabPage.Text = "Properties";
            this.propertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // toolboxTabPage
            // 
            this.toolboxTabPage.Location = new System.Drawing.Point(4, 24);
            this.toolboxTabPage.Name = "toolboxTabPage";
            this.toolboxTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.toolboxTabPage.Size = new System.Drawing.Size(639, 454);
            this.toolboxTabPage.TabIndex = 2;
            this.toolboxTabPage.Text = "Toolbox";
            this.toolboxTabPage.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1605, 917);
            this.Controls.Add(this.editorsTabControl);
            this.Controls.Add(this.codeNavigationBarPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.rightTabControl);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.bottomTabControl);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AlterNET Studio - TypeScript";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.cmReferences.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.standardToolStrip.ResumeLayout(false);
            this.standardToolStrip.PerformLayout();
            this.debuggerControlToolbar.ResumeLayout(false);
            this.debuggerControlToolbar.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.codeNavigationBarPanel.ResumeLayout(false);
            this.breakpointsTabPage.ResumeLayout(false);
            this.outputTabPage.ResumeLayout(false);
            this.findResultsTabPage.ResumeLayout(false);
            this.bottomTabControl.ResumeLayout(false);
            this.callStackTabPage.ResumeLayout(false);
            this.watchesTabPage.ResumeLayout(false);
            this.errorsTabPage.ResumeLayout(false);
            this.rightTabControl.ResumeLayout(false);
            this.projectExplorerTabPage.ResumeLayout(false);
            this.codeExplorerTabPage.ResumeLayout(false);
            this.propertiesTabPage.ResumeLayout(false);
            this.toolboxTabPage.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ContextMenuStrip filesMenuStrip;
        private System.Windows.Forms.ContextMenuStrip cmReferences;
        private System.Windows.Forms.ToolStripSplitButton newStripSplitButton;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFormMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem printPreviewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem undoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem cutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSearch;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runnerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStrip standardToolStrip;
        private Alternet.Scripter.Debugger.UI.DebuggerControlToolbar debuggerControlToolbar;
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
        private System.Windows.Forms.ToolStripButton historyForwardToolButton;
        private System.Windows.Forms.ToolStripSplitButton historyBackwardToolSplitButton;
        private System.Windows.Forms.ContextMenuStrip historyBackwardContextMenu;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel positionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel modifiedStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel overwriteStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem gotoMenuItem;
        private System.Windows.Forms.TreeView projectExplorerTreeView;
        private System.Windows.Forms.Panel codeNavigationBarPanel;
        private System.Windows.Forms.ComboBox methodsComboBox;
        private System.Windows.Forms.ComboBox classesComboBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TabControl editorsTabControl;

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

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem openProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startWithoutDebugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptRunMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addReferenceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeReferenceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFileMenuItem;
        private System.Windows.Forms.TabPage breakpointsTabPage;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.TabPage findResultsTabPage;
        private Alternet.Scripter.Debugger.UI.Output outputControl;
        private Alternet.Editor.Common.FindResults findResultsControl;
        private System.Windows.Forms.TabControl bottomTabControl;
        private System.Windows.Forms.TabPage callStackTabPage;
        private System.Windows.Forms.TabPage watchesTabPage;
        private System.Windows.Forms.TabPage errorsTabPage;
        private Alternet.Scripter.Debugger.UI.Breakpoints breakpointsControl;
        private Alternet.Scripter.Debugger.UI.CallStack callStackControl;
        private Alternet.Scripter.Debugger.UI.Watches watchesControl;
        private Alternet.Scripter.Debugger.UI.Errors errorsControl;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabControl rightTabControl;
        private System.Windows.Forms.TabPage projectExplorerTabPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem gotoDefinitionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findReferencesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewCodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewDesignerMenuItem;
        private System.Windows.Forms.TabPage codeExplorerTabPage;
        private System.Windows.Forms.TreeView codeExplorerTreeView;
        private System.Windows.Forms.ToolStripMenuItem runToCursorMenuItem;
        private System.Windows.Forms.TabPage propertiesTabPage;
        private System.Windows.Forms.TabPage toolboxTabPage;
        private Alternet.Scripter.Debugger.UI.DebugMenu debugMenu;
    }
}