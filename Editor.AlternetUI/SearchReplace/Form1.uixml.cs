#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

using Alternet.UI;

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using System.Collections.Generic;
using Alternet.Common;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace SearchReplace
{
    public partial class Form1 : Window
    {
        private readonly CsSolution csSolution = new();
        private readonly Dictionary<AbstractControl, SyntaxEdit> editors = [];

        private bool searchMulti = true;
        //SearchInFilesDialog searchInFilesDialog = new SearchInFilesDialog();
        public Form1()
        {
            InitializeComponent();
            cbLanguages.Items.AddRange([
                "Default",
                "English",
                "French",
                "German",
                "Spanish",
                "Russian",
                "Ukrainian"]);
            cbLanguages.SelectedIndex = 0;

            Form1_Load(this, EventArgs.Empty);

            tcEditors.MinSizeGrowMode = WindowSizeToContentMode.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FindButton.Click += FindButton_Click;
            ReplaceButton.Click += ReplaceButton_Click;
            FindInFiles.Click += FindInFiles_Click;
            GotoButton.Click += GotoButton_Click;
            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;
            chbSearchMultiDoc.CheckedChanged += SearchMultiDocCheckBox_CheckedChanged;
            SearchManager.SharedSearch.InitSearch += new InitSearchEvent(DoInitSearch);
            SearchManager.SharedSearch.GetSearch += new GetSearchEvent(DoGetSearch);
            SearchManager.SharedSearch.TextFound += new TextFoundEvent(DoTextFound);
            SearchManager.SharedSearch.SearchResultsAvailable += SearchManager_SearchResultsAvailable;
            SearchManager.SharedSearch.Shared = searchMulti;
            OpenProject();

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);

            RunWhenIdle(() =>
            {
                foreach (var editor in editors.Values)
                {
                    (editor.Lexer as CsParser)?.ReparseText();
                }
            });

            tcEditors.SelectedPageChanged += (s, e) =>
            {
                var editor = GetEditor(tcEditors.SelectedPage);
                var parser = editor?.Lexer as CsParser;
                parser?.ReparseText();
            };
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void FindInFiles_Click(object? sender, EventArgs e)
        {
            DisplaySearchInFilesDialog();
        }

        private static string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new(fileName);
                return info.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        private SyntaxEdit? GetEditor(AbstractControl? key)
        {
            if (key != null && editors.TryGetValue(key, out SyntaxEdit? result))
                return result;
            return null;
        }

        private SyntaxEdit? GetActiveSyntaxEdit()
        {
            return GetEditor(tcEditors.SelectedControl as TabPage);
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            NewFile(dirInfo.FullName + @"child1.cs");
            NewFile(dirInfo.FullName + @"child2.cs");
            NewFile(dirInfo.FullName + @"main.cs");
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new(csSolution);

            TabPage page = new(ExtractFileName(fileName));
            tcEditors.Pages.Add(page);
            SyntaxEdit edit = new();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                edit.VisualThemeType = VisualThemeType.Dark;
            }

            edit.Gutter.Options |= GutterOptions.PaintLineNumbers;
            TextSource source = new();
            edit.SearchGlobal = searchMulti;
            edit.Parent = page;
            editors.Add(page, edit);
            edit.Source = source;
            edit.Source.Lexer = parser;

            UpdateSearch();

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.Source.LoadFile(fileName);
                edit.Source.FileName = fileName;
                parser.Prepare(fileName, edit.Lines);
                parser.ReparseText();
            }
            else
            {
                edit.Source.Lines.Clear();
                edit.Source.Lines.Add($"File not found: {fileName}");
            }

            tcEditors.SelectedIndex = tcEditors.TabCount - 1;
        }

        private SyntaxEdit? OpenFile(string fileName)
        {
            var edit = FindFile(fileName);
            if (edit?.Parent is TabPage page)
            {
                tcEditors.SelectedPage = page;
                return edit;
            }

            return null;
        }

        private SyntaxEdit? FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (var tabPage in tcEditors.Pages)
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

        private void DoTextFound(object? sender, Alternet.Editor.TextFoundEventArgs e)
        {
            if (e.Search != null)
                return;

            if (OpenFile(e.FileName) is ISearch search)
            {
                e.Search = search;
                search.OnTextFound(
                    e.Text,
                    e.Options,
                    e.Expression,
                    e.Match,
                    e.Position,
                    e.Len,
                    false,
                    e.MultiLine);
                UpdateSearch();
            }
        }

        private void UpdateSearch()
        {
            var edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                ((ISearch)edit).SearchGlobal = searchMulti;
            }
        }

        private void DoGetSearch(object? sender, Alternet.Editor.GetSearchEventArgs e)
        {
            foreach (var page in editors.Keys)
            {
                var edit = GetEditor(page);
                if (edit != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedPage = page;
                    UpdateSearch();
                    e.Search = (ISearch)edit;
                    break;
                }
            }
        }

        private void DoInitSearch(object sender, Alternet.Editor.InitSearchEventArgs e)
        {
            bool FitsOneOfMultipleMasks(string fileName, string fileMasks)
            {
                return fileMasks
                    .Split(
                        ["\r\n", "\n", ",", "|", ";", " "],
                        StringSplitOptions.RemoveEmptyEntries)
                    .Any(fileMask => FitsMask(fileName, fileMask));
            }

            bool FitsMask(string fileName, string fileMask)
            {
                Regex mask = new(
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

        private void SearchManager_CreateSharedSearch(object? sender, CreateSearchManagerEventArgs e)
        {
            var manager = new SearchManager();
            manager.SearchResultsAvailable += SearchManager_SearchResultsAvailable;
            e.SearchManager = manager;
        }

        private void SearchManager_SearchResultsAvailable(object? sender, SearchResultsEventArgs e)
        {
            //AddSearchResults(e.Ranges);
        }

        //private void AddSearchResults(IList<IFileRange> references)
        //{
        //    //FindResultsListView.BeginUpdate();
        //    //try
        //    //{
        //    //    FindResultsListView.Items.Clear();
        //    //    foreach (var range in references)
        //    //    {
        //    //        var fileRange = range as IFileRange;
        //    //        if (fileRange != null)
        //    //            AddSearchResultCore(fileRange);
        //    //    }
        //    //}
        //    //finally
        //    //{
        //    //    FindResultsListView.EndUpdate();
        //    //}
        //}

        //private void AddSearchResultCore(IFileRange range)
        //{
        //    IList<string> items = new List<string>();
        //    items.Add(string.Format("{0}", Path.GetFileName(range.FileName)));
        //    items.Add(string.Format("{0}", range.StartPoint.Y + 1));
        //    items.Add(string.Format("{0}", range.SourceText.Trim()));
        //    ListViewItem item = new ListViewItem(items.ToArray());
        //    item.Tag = range;

        //    //FindResultsListView.Items.Add(item);
        //}

        private void FindButton_Click(object? sender, EventArgs e)
        {
            var edit = GetActiveSyntaxEdit();
            edit?.DisplaySearchDialog();
        }

        private void ReplaceButton_Click(object? sender, EventArgs e)
        {
            var edit = GetActiveSyntaxEdit();
            edit?.DisplayReplaceDialog();
        }

        private void GotoButton_Click(object? sender, EventArgs e)
        {
            var edit = GetActiveSyntaxEdit();
            edit?.DisplayGotoLineDialog();
        }

        private void DisplaySearchInFilesDialog()
        {
            var edit = GetActiveSyntaxEdit();
            edit?.DisplaySearchDialog();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
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

        private void SearchMultiDocCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            searchMulti = chbSearchMultiDoc.Checked;
            UpdateSearchModel();
        }

        private void UpdateSearchModel()
        {
            SearchManager.SharedSearch.Shared = searchMulti;
            foreach (var page in tcEditors.Pages)
            {
                var edit = GetEditor(page);
                if (edit != null)
                {
                    edit.SearchDialog?.Close();
                    edit.SearchGlobal = searchMulti;
                }
            }
        }
    }
}
