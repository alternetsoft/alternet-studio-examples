#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Lsp;
using LspMultipleFiles.Servers;

namespace LspMultipleFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LspWorkspaceScope workspaceScope;
        private ObservableCollection<string> servers = new ObservableCollection<string>();
        private IDictionary<string, Server> serverList = new Dictionary<string, Server>();

        public MainWindow()
        {
            InitializeComponent();
            FillLspServerComboBox();
            findReferencesResultsControl.NavigateToResultRequested += FindReferencesResultsForm_NavigateToResultRequested;
        }

        private Server SelectedServer
        {
            get
            {
                var serverDesc = lspServerComboBox.SelectedItem as string;
                if (!string.IsNullOrEmpty(serverDesc) && serverList.ContainsKey(serverDesc))
                {
                    return serverList[serverDesc];
                }

                return null;
            }
        }

        private static TextEditorExt GetTabEditor(TabItem page) => (TextEditorExt)page.Content;

        private void ApplySelectedLspServer()
        {
            var server = SelectedServer;

            if (server == null)
                return;

            workspaceScope = null;
            CloseAllFiles();
            filesControl.RootDirectory = null;
            findReferencesResultsControl.FindResults = null;

            filesControl.SearchPatterns = server.SourceFilesSearchPatterns;
            filesControl.RootDirectory = server.GetSourceRootDirectory();
            workspaceScope = new LspWorkspaceScope(x => server.InitializeWorkspace(x));
            OpenAllFiles();
        }

        private void FillLspServerComboBox()
        {
            servers.Clear();
            serverList.Clear();
            AddServers(new Server[] { new PythonServer(), new CppServer() });

            this.lspServerComboBox.ItemsSource = servers;
            lspServerComboBox.SelectedIndex = 0;
        }

        private void AddServers(Server[] lspServers)
        {
            foreach (Server server in lspServers)
            {
                serverList.Add(server.Description, server);
                servers.Add(server.Description);
            }
        }

        private void OpenAllFiles()
        {
            foreach (var file in filesControl.GetFiles())
                EnsureFileOpened(file);
        }

        private void CloseAllFiles()
        {
            while (editorsTabControl.Items.Count > 0)
            {
                var tab = (TabItem)editorsTabControl.Items[editorsTabControl.Items.Count - 1];
                if (tab == null)
                    return;
                CloseEditorPage(tab);
            }
        }

        private void CloseEditorPage(TabItem page)
        {
            editorsTabControl.Items.Remove(page);

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

        private TextEditor EnsureFileOpened(string fileName)
        {
            fileName = Path.GetFullPath(fileName);

            var openTab = TryFindOpenTab(fileName);
            if (openTab != null)
            {
                editorsTabControl.SelectedItem = openTab;
                return GetTabEditor(openTab);
            }

            return OpenFile(fileName);
        }

        private TabItem TryFindOpenTab(string fileName) =>
            editorsTabControl.Items.Cast<TabItem>().FirstOrDefault(x => ((string)x.Tag).Equals(fileName, StringComparison.OrdinalIgnoreCase));

        private TextEditor OpenFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            fileName = Path.GetFullPath(fileName);

            var tabPage = new TabItem();
            tabPage.Header = new TextBlock { Text = Path.GetFileName(fileName), ToolTip = fileName };
            tabPage.Tag = fileName;

            var server = SelectedServer;

            var parser = server.CreateParser();
            parser.WorkspaceScope = workspaceScope;

            var edit = new TextEditorExt
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Lexer = parser,
            };

            edit.Source.Lines.UseSpaces = true;
            edit.Source.LoadFile(fileName);
            edit.Source.FileName = fileName;
            edit.Source.HighlightReferences = true;
            edit.Outlining.AllowOutlining = true;
            edit.GoToDefinitionComplete += Edit_GoToDefinitionComplete;
            edit.FindReferencesComplete += Edit_FindReferencesComplete;
            tabPage.Content = edit;

            editorsTabControl.Items.Add(tabPage);
            editorsTabControl.SelectedItem = tabPage;
            return edit;
        }

        private void FindReferencesResultsForm_NavigateToResultRequested(object sender, FindResultsControl.FileRangeEventArgs e)
        {
            var edit = EnsureFileOpened(e.Range.FileName);
            edit.Position = e.Range.StartPoint;
            edit.Focus();
        }

        private void Edit_FindReferencesComplete(object sender, TextEditorExt.RangeListEventArgs e)
        {
            if (!e.Ranges.OfType<IFileRange>().Any())
                return;

            findReferencesResultsControl.FindResults = e.Ranges;
        }

        private void Edit_GoToDefinitionComplete(object sender, TextEditorExt.SymbolLocationEventArgs e)
        {
            var definitionEdit = EnsureFileOpened(e.SymbolLocation.FileName);
            definitionEdit.MakeVisible(new System.Drawing.Point(e.SymbolLocation.Column, e.SymbolLocation.Line), true);
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "LspMultipleFiles Demo. A part of AlterNET Studio. Copyright AlterNET Software.", "LspMultipleFiles Demo");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LspServerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySelectedLspServer();
        }
    }
}
