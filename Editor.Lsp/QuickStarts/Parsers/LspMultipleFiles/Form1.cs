#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Syntax.Parsers.Lsp;
using LspMultipleFiles.Servers;

namespace LspMultipleFiles
{
    public partial class Form1 : Form
    {
        private LspWorkspaceScope workspaceScope;

        private FindResultsControl findReferencesResultsControl;

        public Form1()
        {
            InitializeComponent();
            CreateFindReferencesResultsControl();
            ScaleControls();

            FillLspServerComboBox();
        }

        private static SyntaxEditExt GetTabEditor(TabPage page) => (SyntaxEditExt)page.Controls[0];

        private void ApplySelectedLspServer()
        {
            var server = SelectedServer;

            workspaceScope = null;
            CloseAllFiles();
            filesControl.RootDirectory = null;
            findReferencesResultsControl.FindResults = null;

            filesControl.SearchPatterns = server.SourceFilesSearchPatterns;
            filesControl.RootDirectory = server.GetSourceRootDirectory();
            workspaceScope = new LspWorkspaceScope(x => server.InitializeWorkspace(x));
            OpenAllFiles();
        }

        private Server SelectedServer => (Server)lspServerComboBox.SelectedItem;

        private void FillLspServerComboBox()
        {
            var servers = new Server[] { new PythonServer(), new CppServer() };
            lspServerComboBox.ComboBox.DataSource = servers;
            lspServerComboBox.ComboBox.DisplayMember = "Description";
        }

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            WindowState = FormWindowState.Maximized;
            filesControl.Width = DisplayScaling.AutoScale(filesControl.Width);
            findResultsPanel.Height = DisplayScaling.AutoScale(findResultsPanel.Height);
            bottomPanelSplitter.Height = DisplayScaling.AutoScale(bottomPanelSplitter.Height);
            leftPanelSplitter.Width = DisplayScaling.AutoScale(leftPanelSplitter.Width);

            lspServerComboBox.FlatStyle = FlatStyle.System;
            lspServerComboBox.ComboBox.Width = DisplayScaling.AutoScale(lspServerComboBox.Width);

            menuStrip.Location = new Point();
            lspServerToolStrip.Location = new Point(0, lspServerToolStrip.Height);
            lspServerToolStrip.Padding = appDescriptionToolStrip.Padding = new Padding(DisplayScaling.AutoScale(2));
        }

        private void CreateFindReferencesResultsControl()
        {
            findReferencesResultsControl = new FindResultsControl
            {
                Text = "Find Reference Results",
                Dock = DockStyle.Fill,
            };

            findReferencesResultsControl.NavigateToResultRequested += FindReferencesResultsForm_NavigateToResultRequested;
            findResultsPanel.Controls.Add(findReferencesResultsControl);
            findReferencesResultsControl.BringToFront();
        }

        private void OpenAllFiles()
        {
            foreach (var file in filesControl.GetFiles())
                EnsureFileOpened(file);
        }

        private void CloseAllFiles()
        {
            foreach (var page in editorsTabControl.TabPages.Cast<TabPage>())
                CloseEditorPage(page);
        }

        private void CloseEditorPage(TabPage page)
        {
            editorsTabControl.TabPages.Remove(page);

            var edit = GetTabEditor(page);
            edit.GoToDefinitionComplete -= Edit_GoToDefinitionComplete;
            edit.FindReferencesComplete -= Edit_FindReferencesComplete;
            ((LspParser)edit.Lexer).Dispose();
            edit.Lexer = null;
        }

        private void FilesControl_OpenFileRequested(object sender, FilesControl.OpenFileRequestEventArgs e)
        {
            EnsureFileOpened(e.FileName);
        }

        private SyntaxEdit EnsureFileOpened(string fileName)
        {
            fileName = Path.GetFullPath(fileName);

            var openTab = TryFindOpenTab(fileName);
            if (openTab != null)
            {
                editorsTabControl.SelectedTab = openTab;
                return GetTabEditor(openTab);
            }

            return OpenFile(fileName);
        }

        private TabPage TryFindOpenTab(string fileName) =>
            editorsTabControl.TabPages.Cast<TabPage>().FirstOrDefault(x => ((string)x.Tag).Equals(fileName, StringComparison.OrdinalIgnoreCase));

        private SyntaxEdit OpenFile(string fileName)
        {
            fileName = Path.GetFullPath(fileName);

            var tabPage = new TabPage(Path.GetFileName(fileName)) { Tag = fileName };

            var server = SelectedServer;

            var parser = server.CreateParser();
            parser.WorkspaceScope = workspaceScope;

            var edit = new SyntaxEditExt
            {
                Dock = DockStyle.Fill,
                Lexer = parser,
            };

            edit.Source.Lines.UseSpaces = true;
            edit.Source.LoadFile(fileName);
            edit.Source.FileName = fileName;
            edit.Source.HighlightReferences = true;
            edit.Outlining.AllowOutlining = true;
            edit.GoToDefinitionComplete += Edit_GoToDefinitionComplete;
            edit.FindReferencesComplete += Edit_FindReferencesComplete;
            tabPage.Controls.Add(edit);

            editorsTabControl.TabPages.Add(tabPage);
            editorsTabControl.SelectedTab = tabPage;
            return edit;
        }

        private void FindReferencesResultsForm_NavigateToResultRequested(object sender, FindResultsControl.FileRangeEventArgs e)
        {
            var edit = EnsureFileOpened(e.Range.FileName);
            edit.Position = e.Range.StartPoint;
            edit.Focus();
        }

        private void Edit_FindReferencesComplete(object sender, SyntaxEditExt.RangeListEventArgs e)
        {
            if (!e.Ranges.OfType<IFileRange>().Any())
                return;

            findReferencesResultsControl.FindResults = e.Ranges;
        }

        private void Edit_GoToDefinitionComplete(object sender, SyntaxEditExt.SymbolLocationEventArgs e)
        {
            var definitionEdit = EnsureFileOpened(e.SymbolLocation.FileName);
            definitionEdit.MoveTo(new Point(e.SymbolLocation.Column, e.SymbolLocation.Line));
            definitionEdit.MakeVisible(new Point(e.SymbolLocation.Column, e.SymbolLocation.Line), true);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "LspMultipleFiles Demo. A part of AlterNET Studio. Copyright AlterNET Software.", "LspMultipleFiles Demo");
        }

        private void LspServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySelectedLspServer();
        }
    }
}