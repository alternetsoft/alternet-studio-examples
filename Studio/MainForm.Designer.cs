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
            this.referencesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSolutionFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameSolutionFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filePropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addReferenceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addProjectFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeProjectItemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDefaultProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.searchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugMenu = new Alternet.Scripter.Debugger.UI.DebugMenu();
            this.startWithoutDebugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoDefinitionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findImplementationsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToProcessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runnerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbStartWithoutDebug = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.standardToolStrip = new System.Windows.Forms.ToolStrip();
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
            this.historyBackwardToolSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.historyBackwardContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.historyForwardToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleBookmarkToolButton = new System.Windows.Forms.ToolStripButton();
            this.prevBookmarkToolButton = new System.Windows.Forms.ToolStripButton();
            this.nextBookmarkToolButton = new System.Windows.Forms.ToolStripButton();
            this.clearAllBookmarksToolButton = new System.Windows.Forms.ToolStripButton();
            this.debuggerControlToolbar = new Alternet.Scripter.Debugger.UI.DebuggerControlToolbar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.positionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.modifiedStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.overwriteStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.projectExplorerTreeView = new System.Windows.Forms.TreeView();
            this.codeNavigationBarPanel = new System.Windows.Forms.Panel();
            this.methodsComboBox = new System.Windows.Forms.ComboBox();
            this.classesComboBox = new System.Windows.Forms.ComboBox();
            this.ProjectFrameworksComboBox = new System.Windows.Forms.ComboBox();
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
            this.localsTabPage = new System.Windows.Forms.TabPage();
            this.localsControl = new Alternet.Scripter.Debugger.UI.Locals();
            this.watchesTabPage = new System.Windows.Forms.TabPage();
            this.watchesControl = new Alternet.Scripter.Debugger.UI.Watches();
            this.errorsTabPage = new System.Windows.Forms.TabPage();
            this.errorsControl = new Alternet.Scripter.Debugger.UI.Errors();
            this.threadsTabPage = new System.Windows.Forms.TabPage();
            this.threadsControl = new Alternet.Scripter.Debugger.UI.Threads();
            this.rightTabControl = new System.Windows.Forms.TabControl();
            this.projectExplorerTabPage = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.codeExplorerTabPage = new System.Windows.Forms.TabPage();
            this.codeExplorerTreeView = new System.Windows.Forms.TreeView();
            this.propertiesTabPage = new System.Windows.Forms.TabPage();
            this.toolboxTabPage = new System.Windows.Forms.TabPage();
            this.outlineTabPage = new System.Windows.Forms.TabPage();
            this.labelProperties = new System.Windows.Forms.Label();
this.panelProperties = new System.Windows.Forms.Panel();
            this.splitterProperties = new System.Windows.Forms.Splitter();
            this.referencesContextMenu.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.standardToolStrip.SuspendLayout();
            this.ProjectFrameworksComboBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.codeNavigationBarPanel.SuspendLayout();
            this.breakpointsTabPage.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.findResultsTabPage.SuspendLayout();
            this.bottomTabControl.SuspendLayout();
            this.callStackTabPage.SuspendLayout();
            this.localsTabPage.SuspendLayout();
            this.watchesTabPage.SuspendLayout();
            this.errorsTabPage.SuspendLayout();
            this.threadsTabPage.SuspendLayout();
            this.rightTabControl.SuspendLayout();
            this.projectExplorerTabPage.SuspendLayout();
            this.codeExplorerTabPage.SuspendLayout();
            this.propertiesTabPage.SuspendLayout();
            this.toolboxTabPage.SuspendLayout();
            this.outlineTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|CS script files (*.csx) |*.csx| All project files (*.csproj; *.vbproj; *.sln)|*.csproj; *.vbproj; *.sln| Any files (*.*) | *.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "doc1";
            this.saveFileDialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|CS script files (*.csx) |*" +
    ".csx| All project files (*.csproj; *.vbproj)|*.csproj; *.vbproj";
            // 
            // filesMenuStrip
            // 
            this.filesMenuStrip.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.filesMenuStrip.Name = "mnuFiles";
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
            this.newStripSplitButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.newStripSplitButton.Text = "New";
            this.newStripSplitButton.ToolTipText = "New";
            // 
            // referencesContextMenu
            // 
            this.referencesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProjectMenuItem,
            this.addSolutionFolderMenuItem,
            this.renameSolutionFolderMenuItem,
            this.addFileMenuItem,
            this.addProjectFolderMenuItem,
            this.addReferenceMenuItem,
            this.removeProjectItemMenuItem,
            this.setDefaultProjectMenuItem,
            this.filePropertiesMenuItem});
            this.referencesContextMenu.Name = "cmReferences";
            this.referencesContextMenu.Size = new System.Drawing.Size(173, 158);
            this.referencesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ReferencesContextMenu_Opening);
            // 
            // addProjectMenuItem
            // 
            this.addProjectMenuItem.Name = "addProjectMenuItem";
            this.addProjectMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addProjectMenuItem.Text = "Add Project";
            this.addProjectMenuItem.Click += new System.EventHandler(this.AddProjectMenuItem_Click);
            // 
            // addSolutionFolderMenuItem
            // 
            this.addSolutionFolderMenuItem.Name = "addSolutionFolderMenuItem";
            this.addSolutionFolderMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addSolutionFolderMenuItem.Text = "Add Solution Folder";
            this.addSolutionFolderMenuItem.Click += new System.EventHandler(this.AddSolutionFolderMenuItem_Click);
            // 
            // renameSolutionFolderMenuItem
            // 
            this.renameSolutionFolderMenuItem.Name = "renameSolutionFolderMenuItem";
            this.renameSolutionFolderMenuItem.Size = new System.Drawing.Size(172, 22);
            this.renameSolutionFolderMenuItem.Text = "Rename";
            this.renameSolutionFolderMenuItem.Click += new System.EventHandler(this.RenameSolutionFolderMenuItem_Click);
            // 
            // addFileMenuItem
            // 
            this.addFileMenuItem.Name = "addFileMenuItem";
            this.addFileMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addFileMenuItem.Text = "Add File";
            this.addFileMenuItem.Click += new System.EventHandler(this.AddFileMenuItem_Click);
            // 
            // addProjectFolderMenuItem
            // 
            this.addProjectFolderMenuItem.Name = "addProjectFolderMenuItem";
            this.addProjectFolderMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addProjectFolderMenuItem.Text = "Add New Folder";
            this.addProjectFolderMenuItem.Click += new System.EventHandler(this.AddProjectFolderMenuItem_Click);
            //
            // filePropertiesMenuItem
            // 
            this.filePropertiesMenuItem.Name = "filePropertiesMenuItem";
            this.filePropertiesMenuItem.Size = new System.Drawing.Size(172, 22);
            this.filePropertiesMenuItem.Text = "Properties";
            this.filePropertiesMenuItem.Checked = true;
            this.filePropertiesMenuItem.CheckOnClick = true;
            this.filePropertiesMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.filePropertiesMenuItem.Click += FilePropertiesMenuItem_Click;
            // 
            // addReferenceMenuItem
            // 
            this.addReferenceMenuItem.Name = "addReferenceMenuItem";
            this.addReferenceMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addReferenceMenuItem.Text = "Add Reference";
            this.addReferenceMenuItem.Click += new System.EventHandler(this.AddReferenceMenuItem_Click);
            // 
            // removeProjectItemMenuItem
            // 
            this.removeProjectItemMenuItem.Name = "removeProjectItemMenuItem";
            this.removeProjectItemMenuItem.Size = new System.Drawing.Size(172, 22);
            this.removeProjectItemMenuItem.Text = "Remove";
            this.removeProjectItemMenuItem.Click += new System.EventHandler(this.RemoveProjectItemMenuItem_Click);
            // 
            // setDefaultProjectMenuItem
            // 
            this.setDefaultProjectMenuItem.Name = "setDefaultProjectMenuItem";
            this.setDefaultProjectMenuItem.Size = new System.Drawing.Size(172, 22);
            this.setDefaultProjectMenuItem.Text = "Set Default Project";
            this.setDefaultProjectMenuItem.Click += new System.EventHandler(this.SetDefaultProjectMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.viewMenuItem,
            this.searchMenuItem,
            //this.bookmarksMenuItem,
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
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.newMenuItem.DropDown = this.filesMenuStrip;
            // 
            // newFormMenuItem
            // 
            this.newFormMenuItem.Name = "newFormMenuItem";
            this.newFormMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newFormMenuItem.Text = "New Form...";
            // 
            // newProjectMenuItem
            // 
            this.newProjectMenuItem.Name = "newProjectMenuItem";
            this.newProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newProjectMenuItem.Text = "New Project";
            this.newProjectMenuItem.Click += new System.EventHandler(this.NewProjectMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(184, 6);
            // 
            // openMenuItem
            // 
            this.openMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openMenuItem.Text = "&Open...";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // closeFileMenuItem
            // 
            this.closeFileMenuItem.Name = "closeFileMenuItem";
            this.closeFileMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeFileMenuItem.Text = "Close";
            this.closeFileMenuItem.Click += new System.EventHandler(this.CloseFileMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(184, 6);
            // 
            // openProjectMenuItem
            // 
            this.openProjectMenuItem.Name = "openProjectMenuItem";
            this.openProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openProjectMenuItem.Text = "Open Project...";
            this.openProjectMenuItem.Click += new System.EventHandler(this.OpenProjectMenuItem_Click);
            // 
            // saveProjectMenuItem
            // 
            this.saveProjectMenuItem.Name = "saveProjectMenuItem";
            this.saveProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveProjectMenuItem.Text = "Save Project";
            this.saveProjectMenuItem.Click += new System.EventHandler(this.SaveProjectMenuItem_Click);
            // 
            // closeProjectMenuItem
            // 
            this.closeProjectMenuItem.Name = "closeProjectMenuItem";
            this.closeProjectMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeProjectMenuItem.Text = "Close Project";
            this.closeProjectMenuItem.Click += new System.EventHandler(this.CloseProjectMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveMenuItem.Text = "&Save";
            this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsMenuItem.Text = "Save &As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // saveAllMenuItem
            // 
            this.saveAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.saveAllMenuItem.Name = "saveAllMenuItem";
            this.saveAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAllMenuItem.Text = "Save All";
            this.saveAllMenuItem.Click += new System.EventHandler(this.SaveAllMenuItem_Click);
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
            this.printPreviewMenuItem.Click += new System.EventHandler(this.PrintPreviewMenuItem_Click);
            // 
            // printMenuItem
            // 
            //this.printMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printMenuItem.Image")));
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printMenuItem.Size = new System.Drawing.Size(187, 22);
            this.printMenuItem.Text = "Print...";
            this.printMenuItem.Click += new System.EventHandler(this.PrintMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(184, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
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
            this.deleteMenuItem,
            this.toolStripSeparator6,
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
            // searchMenuItem
            // 
            this.searchMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findMenuItem,
            this.replaceMenuItem,
            this.gotoMenuItem,
            this.toolStripSeparator16,
            this.gotoDefinitionMenuItem,
            this.findReferencesMenuItem,
            this.findImplementationsMenuItem});
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
            // debugMenu
            // 
            this.debugMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator17,
            this.runParametersMenuItem,
            this.attachToProcessMenuItem});
            this.debugMenu.Name = "debugMenu";
            this.debugMenu.Size = new System.Drawing.Size(54, 20);
            this.debugMenu.Text = "Debug";
            // 
            // startWithoutDebugMenuItem
            // 
            this.startWithoutDebugMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.startWithoutDebugMenuItem.Name = "startWithoutDebugMenuItem";
            this.startWithoutDebugMenuItem.Size = new System.Drawing.Size(276, 22);
            this.startWithoutDebugMenuItem.Text = "Start Without Debugging";
            this.startWithoutDebugMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.startWithoutDebugMenuItem.Click += new System.EventHandler(this.StartWithoutDebugMenuItem_Click);
            // 
            // runParametersMenuItem
            // 
            this.runParametersMenuItem.Name = "runParametersMenuItem";
            this.runParametersMenuItem.Size = new System.Drawing.Size(276, 22);
            this.runParametersMenuItem.Text = "Run Paremeters...";
            this.runParametersMenuItem.Click += new System.EventHandler(this.RunParametersMenuItem_Click);
            // 
            // gotoDefinitionMenuItem
            // 
            this.gotoDefinitionMenuItem.Name = "gotoDefinitionMenuItem";
            this.gotoDefinitionMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.gotoDefinitionMenuItem.Size = new System.Drawing.Size(276, 22);
            this.gotoDefinitionMenuItem.Text = "Go to Definition";
            this.gotoDefinitionMenuItem.Click += new System.EventHandler(this.GotoDefinitionMenuItem_Click);
            // 
            // findReferencesMenuItem
            // 
            this.findReferencesMenuItem.Name = "findReferencesMenuItem";
            this.findReferencesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F12)));
            this.findReferencesMenuItem.Size = new System.Drawing.Size(276, 22);
            this.findReferencesMenuItem.Text = "Find References";
            this.findReferencesMenuItem.Click += new System.EventHandler(this.FindReferencesMenuItem_Click);
            // 
            // findImplementationsMenuItem
            // 
            this.findImplementationsMenuItem.Name = "findImplementationsMenuItem";
            this.findImplementationsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F12)));
            this.findImplementationsMenuItem.Size = new System.Drawing.Size(276, 22);
            this.findImplementationsMenuItem.Text = "Go To Implementation";
            this.findImplementationsMenuItem.Click += new System.EventHandler(this.DebugEdit_FindAllImplementations);
            // 
            // attachToProcessMenuItem
            // 
            this.attachToProcessMenuItem.Name = "attachToProcessMenuItem";
            this.attachToProcessMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
            | System.Windows.Forms.Keys.P)));
            this.attachToProcessMenuItem.Size = new System.Drawing.Size(276, 22);
            this.attachToProcessMenuItem.Text = "Attach to Process...";
            this.attachToProcessMenuItem.Click += new System.EventHandler(this.AttachToProcessMenuItem_Click);
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
            // tsbStartWithoutDebug
            // 
            this.tsbStartWithoutDebug.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsbStartWithoutDebug.Name = "tsbStartWithoutDebug";
            this.tsbStartWithoutDebug.Size = new System.Drawing.Size(23, 22);
            this.tsbStartWithoutDebug.ToolTipText = "Start Without Debugging";
            this.tsbStartWithoutDebug.Click += new System.EventHandler(this.StartWithoutDebugMenuItem_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.standardToolStrip);
            this.flowLayoutPanel1.Controls.Add(this.ProjectFrameworksComboBox);
            this.flowLayoutPanel1.Controls.Add(this.debuggerControlToolbar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1605, 26);
            this.flowLayoutPanel1.TabIndex = 5;
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
            this.toggleBookmarkToolButton,
            this.prevBookmarkToolButton,
            this.nextBookmarkToolButton,
            this.clearAllBookmarksToolButton,
            this.toolStripSeparator15,
            this.historyBackwardToolSplitButton,
            this.historyForwardToolButton});
            this.standardToolStrip.Location = new System.Drawing.Point(0, 0);
            this.standardToolStrip.AutoSize = true;
            this.standardToolStrip.Name = "standardToolStrip";
            this.standardToolStrip.Size = new System.Drawing.Size(399, 26);
            this.standardToolStrip.TabIndex = 3;
            this.standardToolStrip.Text = "toolStrip1";
            this.standardToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StandardToolStrip_ItemClicked);
            // 
            // openToolButton
            // 
            this.openToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolButton.Name = "openToolButton";
            this.openToolButton.Size = new System.Drawing.Size(23, 22);
            this.openToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.openToolButton.ToolTipText = "Open";
            // 
            // saveToolButton
            // 
            this.saveToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolButton.Name = "saveToolButton";
            this.saveToolButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.cutToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cutToolButton.ToolTipText = "Cut";
            // 
            // copyToolButton
            // 
            this.copyToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolButton.Name = "copyToolButton";
            this.copyToolButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.copyToolButton.ToolTipText = "Copy";
            // 
            // pasteToolButton
            // 
            this.pasteToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.pasteToolButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolButton.Image")));
            this.pasteToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolButton.Name = "pasteToolButton";
            this.pasteToolButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pasteToolButton.ToolTipText = "Paste";
            // 
            // undoToolButton
            // 
            this.undoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolButton.Name = "undoToolButton";
            this.undoToolButton.Size = new System.Drawing.Size(23, 22);
            this.undoToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.undoToolButton.ToolTipText = "Undo";
            // 
            // redoToolButton
            // 
            this.redoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoToolButton.Name = "redoToolButton";
            this.redoToolButton.Size = new System.Drawing.Size(23, 22);
            this.redoToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.findToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.findToolButton.ToolTipText = "Find";
            // 
            // replaceToolButton
            // 
            this.replaceToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.replaceToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.replaceToolButton.Name = "replaceToolButton";
            this.replaceToolButton.Size = new System.Drawing.Size(23, 22);
            this.replaceToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.replaceToolButton.ToolTipText = "Replace";
            // 
            // gotoToolButton
            // 
            this.gotoToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.gotoToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gotoToolButton.Name = "gotoToolButton";
            this.gotoToolButton.Size = new System.Drawing.Size(23, 22);
            this.gotoToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.printPreviewToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.printPreviewToolButton.ToolTipText = "Print Preview";
            // 
            // printToolButton
            // 
            this.printToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolButton.Name = "printToolButton";
            this.printToolButton.Size = new System.Drawing.Size(23, 22);
            this.printToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.printToolButton.ToolTipText = "Print";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // historyBackwardToolSplitButton
            // 
            this.historyBackwardToolSplitButton.DropDown = this.historyBackwardContextMenu;
            this.historyBackwardToolSplitButton.Name = "historyBackwardToolSplitButton";
            this.historyBackwardToolSplitButton.Size = new System.Drawing.Size(32, 22);
            this.historyBackwardToolSplitButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.historyBackwardToolSplitButton.ToolTipText = "Navigate Backward";
            // 
            // historyBackwardContextMenu
            // 
            this.historyBackwardContextMenu.Name = "tsdBackward";
            this.historyBackwardContextMenu.OwnerItem = this.historyBackwardToolSplitButton;
            this.historyBackwardContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // historyForwardToolButton
            // 
            this.historyForwardToolButton.Name = "historyForwardToolButton";
            this.historyForwardToolButton.Size = new System.Drawing.Size(23, 22);
            this.historyForwardToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.historyForwardToolButton.ToolTipText = "Navigate Forward";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // toggleBookmarkToolButton
            // 
            this.toggleBookmarkToolButton.Name = "toggleBookmarkToolButton";
            this.toggleBookmarkToolButton.Size = new System.Drawing.Size(23, 22);
            this.toggleBookmarkToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toggleBookmarkToolButton.ToolTipText = "Toggle a bookmark on the current line. (Ctrl + K, Ctrl + K)";
            this.toggleBookmarkToolButton.Click += ToggleBookmarkToolButton_Click;
            // 
            // prevBookmarkToolButton
            // 
            this.prevBookmarkToolButton.Name = "prevBookmarkToolButton";
            this.prevBookmarkToolButton.Size = new System.Drawing.Size(23, 22);
            this.prevBookmarkToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.prevBookmarkToolButton.ToolTipText = "Move the caret to the previous bookmark. (Ctrl + K, Ctrl + P)";
            this.prevBookmarkToolButton.Click += PrevBookmarkToolButton_Click;
            // 
            // nextBookmarkToolButton
            // 
            this.nextBookmarkToolButton.Name = "nextBookmarkToolButton";
            this.nextBookmarkToolButton.Size = new System.Drawing.Size(23, 22);
            this.nextBookmarkToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nextBookmarkToolButton.ToolTipText = "Move the caret to the next bookmark. (Ctrl + K, Ctrl + N)";
            this.nextBookmarkToolButton.Click += NextBookmarkToolButton_Click;
            // 
            // clearAllBookmarksToolButton
            // 
            this.clearAllBookmarksToolButton.Name = "clearAllBookmarksToolButton";
            this.clearAllBookmarksToolButton.Size = new System.Drawing.Size(23, 22);
            this.clearAllBookmarksToolButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.clearAllBookmarksToolButton.ToolTipText = "Clear all bookmarks in all files. (Ctrl + K, Ctrl + L)";
            this.clearAllBookmarksToolButton.Click += ClearAllBookmarksToolButton_Click;
            // 
            // debuggerControlToolbar
            // 
            this.debuggerControlToolbar.Location = new System.Drawing.Point(399, 0);
            this.debuggerControlToolbar.Name = "debuggerControlToolbar";
            this.debuggerControlToolbar.Size = new System.Drawing.Size(127, 25);
            this.debuggerControlToolbar.TabIndex = 4;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.positionStatusLabel,
            this.modifiedStatusLabel,
            this.overwriteStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 895);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1605, 22);
            this.statusStrip.TabIndex = 19;
            // 
            // positionStatusLabel
            // 
            this.positionStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.positionStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.positionStatusLabel.Name = "positionStatusLabel";
            this.positionStatusLabel.Size = new System.Drawing.Size(4, 17);
            // 
            // modifiedStatusLabel
            // 
            this.modifiedStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.modifiedStatusLabel.Name = "modifiedStatusLabel";
            this.modifiedStatusLabel.Size = new System.Drawing.Size(4, 17);
            // 
            // overwriteStatusLabel
            // 
            this.overwriteStatusLabel.Name = "overwriteStatusLabel";
            this.overwriteStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // projectExplorerTreeView
            // 
            this.projectExplorerTreeView.ContextMenuStrip = this.referencesContextMenu;
            this.projectExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectExplorerTreeView.ImageIndex = 0;
            this.projectExplorerTreeView.Location = new System.Drawing.Point(3, 3);
            this.projectExplorerTreeView.Name = "projectExplorerTreeView";
            this.projectExplorerTreeView.SelectedImageIndex = 0;
            this.projectExplorerTreeView.Size = new System.Drawing.Size(633, 452);
            this.projectExplorerTreeView.TabIndex = 2;
            this.projectExplorerTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectExplorerTreeView_BeforeCollapse);
            this.projectExplorerTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ProjectExplorerTreeView_BeforeExpand);
            this.projectExplorerTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectExplorerTreeView_NodeMouseDoubleClick);
            this.projectExplorerTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectExplorerTreeView_MouseDown);
            this.projectExplorerTreeView.AfterSelect += ProjectExplorerTreeView_AfterSelect;
            this.projectExplorerTreeView.AfterLabelEdit += ProjectExplorerTreeView_AfterLabelEdit;
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
            this.codeNavigationBarPanel.Resize += new System.EventHandler(this.CodeNavigationBarPanel_Resize);
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
            // ProjectFrameworksComboBox
            // 
            this.ProjectFrameworksComboBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.ProjectFrameworksComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ProjectFrameworksComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectFrameworksComboBox.FormattingEnabled = true;
            this.ProjectFrameworksComboBox.Location = new System.Drawing.Point(0, 0);
            this.ProjectFrameworksComboBox.Name = "ProjectFrameworksComboBox";
            this.ProjectFrameworksComboBox.Size = new System.Drawing.Size(120, 24);
            this.ProjectFrameworksComboBox.Sorted = false;
            this.ProjectFrameworksComboBox.TabIndex = 0;
            this.ProjectFrameworksComboBox.Margin = new System.Windows.Forms.Padding(3,0,3,8);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(953, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 486);
            this.splitter1.TabIndex = 34;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 535);
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
            this.editorsTabControl.Size = new System.Drawing.Size(953, 461);
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
            this.bottomTabControl.Controls.Add(this.localsTabPage);
            this.bottomTabControl.Controls.Add(this.watchesTabPage);
            this.bottomTabControl.Controls.Add(this.errorsTabPage);
            this.bottomTabControl.Controls.Add(this.threadsTabPage);
            this.bottomTabControl.Controls.Add(this.findResultsTabPage);
            this.bottomTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomTabControl.Location = new System.Drawing.Point(0, 489);
            this.bottomTabControl.Name = "bottomTabControl";
            this.bottomTabControl.SelectedIndex = 0;
            this.bottomTabControl.Size = new System.Drawing.Size(1605, 306);
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
            this.callStackControl.CallStackClick += new System.EventHandler<Alternet.Scripter.Debugger.UI.CallStackClickEventArgs>(this.CallStackControl_CallStackClick);
            // 
            // localsTabPage
            // 
            this.localsTabPage.Controls.Add(this.localsControl);
            this.localsTabPage.Location = new System.Drawing.Point(4, 24);
            this.localsTabPage.Name = "localsTabPage";
            this.localsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.localsTabPage.Size = new System.Drawing.Size(1597, 328);
            this.localsTabPage.TabIndex = 5;
            this.localsTabPage.Text = "Locals";
            this.localsTabPage.UseVisualStyleBackColor = true;
            // 
            // localsControl
            // 
            this.localsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localsControl.Location = new System.Drawing.Point(3, 3);
            this.localsControl.Margin = new System.Windows.Forms.Padding(26);
            this.localsControl.Name = "localsControl";
            this.localsControl.Size = new System.Drawing.Size(1591, 322);
            this.localsControl.TabIndex = 0;
            this.localsControl.AddToWatchClick += new System.EventHandler<Alternet.Scripter.Debugger.UI.AddToWatchClickEventArgs>(this.LocalsControl_AddToWatchClick);
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
            // threadsTabPage
            // 
            this.threadsTabPage.Controls.Add(this.threadsControl);
            this.threadsTabPage.Location = new System.Drawing.Point(4, 24);
            this.threadsTabPage.Name = "threadsTabPage";
            this.threadsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.threadsTabPage.Size = new System.Drawing.Size(1597, 328);
            this.threadsTabPage.TabIndex = 8;
            this.threadsTabPage.Text = "Threads";
            this.threadsTabPage.UseVisualStyleBackColor = true;
            // 
            // threadsControl
            // 
            this.threadsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.threadsControl.Location = new System.Drawing.Point(3, 3);
            this.threadsControl.Margin = new System.Windows.Forms.Padding(7);
            this.threadsControl.Name = "threadsControl";
            this.threadsControl.Size = new System.Drawing.Size(1591, 322);
            this.threadsControl.TabIndex = 0;
            // 
            // rightTabControl
            // 
            this.rightTabControl.Controls.Add(this.projectExplorerTabPage);
            this.rightTabControl.Controls.Add(this.codeExplorerTabPage);
            this.rightTabControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightTabControl.Location = new System.Drawing.Point(1058, 49);
            this.rightTabControl.Name = "rightTabControl";
            this.rightTabControl.SelectedIndex = 0;
            this.rightTabControl.Size = new System.Drawing.Size(547, 486);
            this.rightTabControl.TabIndex = 38;
            // 
            // projectExplorerTabPage
            // 
            this.projectExplorerTabPage.Controls.Add(this.projectExplorerTreeView);
            this.projectExplorerTabPage.Controls.Add(this.splitterProperties);
            this.projectExplorerTabPage.Controls.Add(this.panelProperties);
            this.projectExplorerTabPage.Location = new System.Drawing.Point(4, 24);
            this.projectExplorerTabPage.Name = "projectExplorerTabPage";
            this.projectExplorerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.projectExplorerTabPage.Size = new System.Drawing.Size(639, 458);
            this.projectExplorerTabPage.TabIndex = 0;
            this.projectExplorerTabPage.Text = "Project Explorer";
            this.projectExplorerTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 347);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(633, 110);
            this.propertyGrid.TabIndex = 5;
            // 
            // codeExplorerTabPage
            // 
            this.codeExplorerTabPage.Controls.Add(this.codeExplorerTreeView);
            this.codeExplorerTabPage.Location = new System.Drawing.Point(4, 24);
            this.codeExplorerTabPage.Name = "codeExplorerTabPage";
            this.codeExplorerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.codeExplorerTabPage.Size = new System.Drawing.Size(639, 458);
            this.codeExplorerTabPage.TabIndex = 3;
            this.codeExplorerTabPage.Text = "Code Explorer";
            this.codeExplorerTabPage.UseVisualStyleBackColor = true;
            // 
            // codeExplorerTreeView
            // 
            this.codeExplorerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeExplorerTreeView.ImageIndex = 0;
            this.codeExplorerTreeView.Location = new System.Drawing.Point(3, 3);
            this.codeExplorerTreeView.Name = "codeExplorerTreeView";
            this.codeExplorerTreeView.SelectedImageIndex = 0;
            this.codeExplorerTreeView.Size = new System.Drawing.Size(633, 452);
            this.codeExplorerTreeView.TabIndex = 4;
            // 
            // propertiesTabPage
            // 
            this.propertiesTabPage.Location = new System.Drawing.Point(4, 24);
            this.propertiesTabPage.Name = "propertiesTabPage";
            this.propertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertiesTabPage.Size = new System.Drawing.Size(639, 458);
            this.propertiesTabPage.TabIndex = 1;
            this.propertiesTabPage.Text = "Properties";
            this.propertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.propertyGrid);
            this.panelProperties.Controls.Add(this.labelProperties);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProperties.Location = new System.Drawing.Point(3, 330);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(633, 200);
            this.panelProperties.TabIndex = 7;
            // 
            // toolboxTabPage
            // 
            this.toolboxTabPage.Location = new System.Drawing.Point(4, 24);
            this.toolboxTabPage.Name = "toolboxTabPage";
            this.toolboxTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.toolboxTabPage.Size = new System.Drawing.Size(639, 458);
            this.toolboxTabPage.TabIndex = 2;
            this.toolboxTabPage.Text = "Toolbox";
            this.toolboxTabPage.UseVisualStyleBackColor = true;
            // 
            // outlineTabPage
            // 
            this.outlineTabPage.Location = new System.Drawing.Point(4, 24);
            this.outlineTabPage.Name = "outlineTabPage";
            this.outlineTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outlineTabPage.Size = new System.Drawing.Size(639, 458);
            this.outlineTabPage.TabIndex = 4;
            this.outlineTabPage.Text = "Outline";
            this.outlineTabPage.UseVisualStyleBackColor = true;
            // 
            // splitter3
            // 
            this.splitterProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterProperties.Location = new System.Drawing.Point(4, 343);
            //this.splitter3.BackColor = System.Drawing.Color.Red;
            this.splitterProperties.Name = "splitter3";
            this.splitterProperties.Size = new System.Drawing.Size(633, 4);
            this.splitterProperties.TabIndex = 8;
            this.splitterProperties.TabStop = false;
            // 
            // label1
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProperties.Location = new System.Drawing.Point(0, 0);
            this.labelProperties.Name = "label1";
            this.labelProperties.Size = new System.Drawing.Size(59, 15);
            this.labelProperties.TabIndex = 7;
            this.labelProperties.Text = "Properties";
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
            this.Name = "MainForm";
            this.Text = "AlterNET Studio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.referencesContextMenu.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.standardToolStrip.ResumeLayout(false);
            this.standardToolStrip.PerformLayout();
            this.ProjectFrameworksComboBox.ResumeLayout(false);
            this.ProjectFrameworksComboBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.codeNavigationBarPanel.ResumeLayout(false);
            this.breakpointsTabPage.ResumeLayout(false);
            this.outputTabPage.ResumeLayout(false);
            this.findResultsTabPage.ResumeLayout(false);
            this.bottomTabControl.ResumeLayout(false);
            this.callStackTabPage.ResumeLayout(false);
            this.localsTabPage.ResumeLayout(false);
            this.watchesTabPage.ResumeLayout(false);
            this.errorsTabPage.ResumeLayout(false);
            this.threadsTabPage.ResumeLayout(false);
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            this.rightTabControl.ResumeLayout(false);
            this.projectExplorerTabPage.ResumeLayout(false);
            this.codeExplorerTabPage.ResumeLayout(false);
            this.propertiesTabPage.ResumeLayout(false);
            this.toolboxTabPage.ResumeLayout(false);
            this.outlineTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ContextMenuStrip filesMenuStrip;
        private System.Windows.Forms.ContextMenuStrip referencesContextMenu;
        private System.Windows.Forms.ToolStripSplitButton newStripSplitButton;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripButton toggleBookmarkToolButton;
        private System.Windows.Forms.ToolStripButton prevBookmarkToolButton;
        private System.Windows.Forms.ToolStripButton nextBookmarkToolButton;
        private System.Windows.Forms.ToolStripButton clearAllBookmarksToolButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel positionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel modifiedStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel overwriteStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem gotoMenuItem;
        private System.Windows.Forms.TreeView projectExplorerTreeView;
        private System.Windows.Forms.Panel codeNavigationBarPanel;
        private System.Windows.Forms.ComboBox methodsComboBox;
        private System.Windows.Forms.ComboBox classesComboBox;
        private System.Windows.Forms.ComboBox ProjectFrameworksComboBox;
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
        private System.Windows.Forms.ToolStripMenuItem addProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSolutionFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameSolutionFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addReferenceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addProjectFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeProjectItemMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filePropertiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDefaultProjectMenuItem;
        private System.Windows.Forms.ToolStripButton tsbStartWithoutDebug;
        private System.Windows.Forms.TabPage breakpointsTabPage;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.TabPage findResultsTabPage;
        private Alternet.Scripter.Debugger.UI.Output outputControl;
        private Alternet.Editor.Common.FindResults findResultsControl;
        private System.Windows.Forms.TabControl bottomTabControl;
        private System.Windows.Forms.TabPage callStackTabPage;
        private System.Windows.Forms.TabPage localsTabPage;
        private System.Windows.Forms.TabPage watchesTabPage;
        private System.Windows.Forms.TabPage errorsTabPage;
        private Alternet.Scripter.Debugger.UI.Breakpoints breakpointsControl;
        private Alternet.Scripter.Debugger.UI.CallStack callStackControl;
        private Alternet.Scripter.Debugger.UI.Locals localsControl;
        private Alternet.Scripter.Debugger.UI.Watches watchesControl;
        private Alternet.Scripter.Debugger.UI.Errors errorsControl;
        private System.Windows.Forms.TabPage threadsTabPage;
        private Alternet.Scripter.Debugger.UI.Threads threadsControl;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenuItem;
        private System.Windows.Forms.TabControl rightTabControl;
        private System.Windows.Forms.TabPage projectExplorerTabPage;
        private System.Windows.Forms.TabPage propertiesTabPage;
        private System.Windows.Forms.TabPage toolboxTabPage;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem runParametersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoDefinitionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findReferencesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findImplementationsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFormMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewCodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewDesignerMenuItem;
        private System.Windows.Forms.TabPage codeExplorerTabPage;
        private System.Windows.Forms.TreeView codeExplorerTreeView;
        private System.Windows.Forms.TabPage outlineTabPage;
        private Alternet.Scripter.Debugger.UI.DebugMenu debugMenu;
        private System.Windows.Forms.ToolStripMenuItem attachToProcessMenuItem;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.Splitter splitterProperties;
    }
}