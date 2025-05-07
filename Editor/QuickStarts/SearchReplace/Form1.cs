#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace SearchReplace
{
    public partial class Form1 : Form
    {
        private const string FindDesc = "Display Search Dialog";
        private const string ReplaceDesc = "Display Replace Dialog";
        private const string GotoDesc = "Display Goto Line Dialog";
        private const string FindInFilesDesc = "Display Find In Files Dialog";
        private const string LanguageDesc = "Choose dialogs language";
        private const string SearchMultiDocDesc = "Select search mode one or several documents";

        private string dir = Application.StartupPath + @"\";
        private IDictionary<TabPage, ISyntaxEdit> editors = new Dictionary<TabPage, ISyntaxEdit>();
        private KeyEvent findEvent;
        private bool searchMulti = true;
        SearchInFilesDialog searchInFilesDialog = new SearchInFilesDialog();

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "SearchReplace.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private IFileRange SelectedRange
        {
            get
            {
                return FindResultsListView.SelectedItems.Count > 0 ? FindResultsListView.SelectedItems[0].Tag as IFileRange : null;
            }
        }

        public void DoFind()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplaySearchDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            SearchManager.CreateSharedSearch += SearchManager_CreateSharedSearch;
            SearchManager.SharedSearch.InitSearch += new InitSearchEvent(DoInitSearch);
            SearchManager.SharedSearch.GetSearch += new GetSearchEvent(DoGetSearch);
            SearchManager.SharedSearch.TextFound += new TextFoundEvent(DoTextFound);
            SearchManager.SharedSearch.Shared = searchMulti;
            OpenProject();
            cbLanguages.SelectedIndex = 0;

            foreach (var editor in editors.Values)
            {
                (editor.Lexer as CsParser)?.ReparseText();
            }

            tcEditors.SelectedIndexChanged += (s, x) =>
            {
                var editor = GetEditor(tcEditors.SelectedTab);
                var parser = editor?.Lexer as CsParser;
                parser?.ReparseText();
            };
        }

        private ISyntaxEdit GetEditor(TabPage key)
        {
            ISyntaxEdit result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        private ISyntaxEdit GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedTab);
        }

        private void DoTextFound(object sender, TextFoundEventArgs e)
        {
            if (e.Search != null)
                return;

            var search = OpenFile(e.FileName) as ISearch;
            if (search != null)
            {
                e.Search = search;
                search.OnTextFound(e.Text, e.Options, e.Expression, e.Match, e.Position, e.Len, false, e.MultiLine);
                UpdateSearch();
            }
        }

        private void UpdateSearch()
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                ISearch search = (ISearch)edit;
                search.SearchGlobal = searchMulti;
            }
        }

        private void DoGetSearch(object sender, GetSearchEventArgs e)
        {
            foreach (TabPage page in editors.Keys)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedTab = page;
                    UpdateSearch();
                    e.Search = (ISearch)edit;
                    break;
                }
            }
        }

        private void DoInitSearch(object sender, InitSearchEventArgs e)
        {
            bool FitsOneOfMultipleMasks(string fileName, string fileMasks)
            {
                return fileMasks
                    .Split(
                        new string[] { "\r\n", "\n", ",", "|", ";", " " },
                        StringSplitOptions.RemoveEmptyEntries)
                    .Any(fileMask => FitsMask(fileName, fileMask));
            }

            bool FitsMask(string fileName, string fileMask)
            {
                Regex mask = new Regex(
                    '^' +
                    fileMask
                        .Replace(".", "[.]")
                        .Replace("*", ".*")
                        .Replace("?", ".")
                    + '$',
                    RegexOptions.IgnoreCase);
                return mask.IsMatch(fileName);
            }

            e.Search = GetActiveSyntaxEdit();
            foreach (ISyntaxEdit edit in editors.Values)
            {
                edit.SearchGlobal = searchMulti;
                if (edit.Source != null)
                {
                    if (!string.IsNullOrEmpty(e.Filter) && !string.IsNullOrEmpty(edit.Source.FileName))
                    {
                        if (!FitsOneOfMultipleMasks(edit.Source.FileName, e.Filter))
                            continue;
                    }

                    e.SearchList.Add(edit.Source.FileName);
                }
            }
        }

        private void SearchManager_CreateSharedSearch(object sender, CreateSearchManagerEventArgs e)
        {
            var manager = new SearchManager();
            manager.SearchResultsAvailable += SearchManager_SearchResultsAvailable;
            e.SearchManager = manager;
        }

        private void SearchManager_SearchResultsAvailable(object sender, SearchResultsEventArgs e)
        {
            AddSearchResults(e.Ranges);
        }

        private void AddSearchResults(IList<IFileRange> references)
        {
            FindResultsListView.BeginUpdate();
            try
            {
                FindResultsListView.Items.Clear();
                foreach (var range in references)
                {
                    var fileRange = range as IFileRange;
                    if (fileRange != null)
                        AddSearchResultCore(fileRange);
                }
            }
            finally
            {
                FindResultsListView.EndUpdate();
            }
        }

        private void AddSearchResultCore(IFileRange range)
        {
            IList<string> items = new List<string>();
            items.Add(string.Format("{0}", Path.GetFileName(range.FileName)));
            items.Add(string.Format("{0}", range.StartPoint.Y + 1));
            items.Add(string.Format("{0}", range.SourceText.Trim()));
            ListViewItem item = new ListViewItem(items.ToArray());
            item.Tag = range;

            FindResultsListView.Items.Add(item);
        }

        private void FindResultsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NavigateToRange();
        }

        private void NavigateToRange()
        {
            IFileRange range = SelectedRange;
            ISyntaxEdit edit = OpenFile(range.FileName);
            if (edit != null)
            {
                var position = new Point(range.StartPoint.X, range.StartPoint.Y);
                edit.MakeVisible(position, true);
                edit.Position = position;
                edit.Focus();
            }
        }

        private ISyntaxEdit OpenFile(string fileName)
        {
            ISyntaxEdit edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabPage))
            {
                tcEditors.SelectedTab = (TabPage)edit.Parent;
                return edit;
            }

            return null;
        }

        private ISyntaxEdit FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabPage tabPage in tcEditors.TabPages)
            {
                var edit = GetEditor(tabPage);
                if (edit != null)
                {
                    var path = edit.Source.FileName;
                    path = new Uri(path).LocalPath;
                    if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                        return edit;
                }
            }

            return null;
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                return;
            }

            NewFile(dir + @"Resources\Editor\text\child1.cs");
            NewFile(dir + @"Resources\Editor\text\child2.cs");
            NewFile(dir + @"Resources\Editor\text\main.cs");
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new CsParser();

            TabPage page = new TabPage(ExtractFileName(fileName));
            tcEditors.TabPages.Add(page);

            ISyntaxEdit edit = new SyntaxEdit();
            edit.Selection.Options |= SelectionOptions.DrawBorder;
            TextSource source = new TextSource();
            edit.SearchGlobal = searchMulti;
            editors.Add(page, edit);
            edit.Source = source;
            edit.Dock = DockStyle.Fill;
            edit.Bounds = new Rectangle(0, 0, page.ClientRectangle.Width, page.ClientRectangle.Height);
            page.Controls.Add(edit as Control);
            edit.Source.Lexer = parser;
            findEvent = new KeyEvent(DoFind);
            edit.KeyList.Remove(Keys.F | Keys.Control);
            edit.KeyList.Add(Keys.F | Keys.Control, findEvent);

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.LoadFile(fileName);
                edit.Source.FileName = fileName;
            }

            UpdateSearch();

            tcEditors.SelectedTab = page;
            edit.Selection.Options |= SelectionOptions.HighlightSelectedWords;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplaySearchDialog();
            }
        }

        private void ReplaceButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            edit.DisplayReplaceDialog();
        }

        private void GotoButton_Click(object sender, EventArgs e)
        {
            ISyntaxEdit edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplayGotoLineDialog();
            }
        }

        private void FindInFilesButton_Click(object sender, EventArgs e)
        {
            DisplaySearchInFilesDialog();
        }

        private void DisplaySearchInFilesDialog()
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                searchInFilesDialog.Execute(edit, true, false, null);
            }
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CultureInfo oldcInfo = Thread.CurrentThread.CurrentUICulture;
            switch (cbLanguages.SelectedIndex)
            {
                case 0:
                    StringConsts.Localize();
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 2:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 3:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 4:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 5:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
                case 6:
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");
                    try
                    {
                        StringConsts.Localize();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentUICulture = oldcInfo;
                    }

                    break;
            }
        }

        private void FindButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(FindNextButton);
            if (str != FindDesc)
            {
                toolTip1.SetToolTip(FindNextButton, FindDesc);
            }
        }

        private void ReplaceButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btReplace);
            if (str != ReplaceDesc)
            {
                toolTip1.SetToolTip(btReplace, ReplaceDesc);
            }
        }

        private void GotoButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btGoto);
            if (str != GotoDesc)
            {
                toolTip1.SetToolTip(btGoto, GotoDesc);
            }
        }

        private void FindInFilesButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(FindInFilesButton);
            if (str != FindInFilesDesc)
            {
                toolTip1.SetToolTip(FindInFilesButton, FindInFilesDesc);
            }
        }
        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDesc)
            {
                toolTip1.SetToolTip(cbLanguages, LanguageDesc);
            }
        }

        private void UpdateSearchModel()
        {
            SearchManager.SharedSearch.Shared = searchMulti;
            foreach (TabPage page in tcEditors.TabPages)
            {
                ISyntaxEdit edit = GetEditor(page);
                if (edit != null)
                {
                    edit.SearchDialog?.Close();
                    edit.SearchGlobal = searchMulti;
                }
            }
        }

        private void SearchMultiDocCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            searchMulti = chbSearchMultiDoc.Checked;
            UpdateSearchModel();
        }

        private void SearchMultiDocCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSearchMultiDoc);
            if (str != SearchMultiDocDesc)
            {
                toolTip1.SetToolTip(chbSearchMultiDoc, SearchMultiDocDesc);
            }
        }
    }
}
