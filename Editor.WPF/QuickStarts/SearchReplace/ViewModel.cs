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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace SearchReplace
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private MainWindow window;
        private TabControl tcEditors;
        private ListView findResultsListView;
        private string language = string.Empty;
        private bool searchMulti = true;
        private IDictionary<TabItem, TextEditor> editors = new Dictionary<TabItem, TextEditor>();
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private SearchInFilesDialog searchInFilesDialog = new SearchInFilesDialog();
        private ObservableCollection<ListViewItemData> source = new ObservableCollection<ListViewItemData>();

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            languages.Add("Default");
            languages.Add("English");
            languages.Add("French");
            languages.Add("German");
            languages.Add("Spanish");
            languages.Add("Russian");
            languages.Add("Ukrainian");

            FindCommand = new RelayCommand(FindClick);
            ReplaceCommand = new RelayCommand(ReplaceClick);
            GotoCommand = new RelayCommand(GotoClick);
            FindInFilesCommand = new RelayCommand(FindInFilesClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            this.tcEditors = window.tcEditors;
            this.findResultsListView = window.FindResultsListView;
            findResultsListView.MouseDoubleClick += FindResultsListView_MouseDoubleClick;
            SearchManager.SharedSearch.InitSearch += new InitSearchEvent(DoInitSearch);
            SearchManager.SharedSearch.GetSearch += new GetSearchEvent(DoGetSearch);
            SearchManager.SharedSearch.TextFound += new TextFoundEvent(DoTextFound);
            SearchManager.SharedSearch.SearchResultsAvailable += SearchManager_SearchResultsAvailable;
            SearchManager.SharedSearch.Shared = searchMulti;
            OpenProject();
            Language = "Default";

            foreach (var editor in editors.Values)
            {
                (editor.Lexer as CsParser)?.ReparseText();
            }

            tcEditors.SelectionChanged += (s, x) =>
            {
                var editor = GetEditor(tcEditors.SelectedItem as TabItem);
                var parser = editor?.Lexer as CsParser;
                parser?.ReparseText();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return language;
            }

            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged("Language");
                    Localize();
                }
            }
        }

        public bool SearchMulti
        {
            get
            {
                return searchMulti;
            }

            set
            {
                if (searchMulti != value)
                {
                    searchMulti = value;
                    OnPropertyChanged("SearchMulti");
                    UpdateSearchModel();
                }
            }
        }

        public ICommand FindCommand { get; set; }

        public ICommand ReplaceCommand { get; set; }

        public ICommand GotoCommand { get; set; }

        public ICommand FindInFilesCommand { get; set; }

        private IFileRange SelectedRange
        {
            get
            {
                object obj = findResultsListView.SelectedItem;
                if ((obj != null) && (obj is ListViewItemData))
                {
                    return ((ListViewItemData)obj).Tag as IFileRange;
                }

                return null;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TextEditor GetEditor(TabItem key)
        {
            TextEditor result;
            if (key != null && editors.TryGetValue(key, out result))
                return result;
            return null;
        }

        private void UpdateSearchModel()
        {
            SearchManager.SharedSearch.Shared = searchMulti;
            foreach (TabItem page in tcEditors.Items)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null)
                {
                    edit.SearchDialog?.Close();
                    edit.SearchGlobal = searchMulti;
                }
            }
        }

        private void DoGetSearch(object sender, GetSearchEventArgs e)
        {
            foreach (TabItem page in editors.Keys)
            {
                TextEditor edit = GetEditor(page);
                if (edit != null && edit.Source != null && string.Compare(edit.Source.FileName, e.FileName, true) == 0)
                {
                    tcEditors.SelectedItem = page;
                    UpdateSearch();
                    e.Search = edit;
                    break;
                }
            }
        }

        private void DoInitSearch(object sender, InitSearchEventArgs e)
        {
            e.Search = GetActiveSyntaxEdit();
            foreach (TextEditor edit in editors.Values)
            {
                edit.SearchGlobal = searchMulti;
                if (edit.Source != null)
                    e.SearchList.Add(edit.Source.FileName);
            }
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

        private void NavigateToRange()
        {
            IFileRange range = SelectedRange;
            TextEditor edit = OpenFile(range.FileName);
            if (edit != null)
            {
                var position = new System.Drawing.Point(range.StartPoint.X, range.StartPoint.Y);
                edit.MakeVisible(position, true);
                edit.Position = position;
                edit.Focus();
            }
        }

        private TextEditor OpenFile(string fileName)
        {
            TextEditor edit = FindFile(fileName);
            if ((edit != null) && (edit.Parent is TabItem))
            {
                tcEditors.SelectedItem = (TabItem)edit.Parent;
                return edit;
            }

            return null;
        }

        private TextEditor FindFile(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabItem tabPage in tcEditors.Items)
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

        private void SearchManager_SearchResultsAvailable(object sender, SearchResultsEventArgs e)
        {
            AddSearchResults(e.Ranges);
        }

        private void AddSearchResults(IList<IFileRange> references)
        {
            source.Clear();

            foreach (var range in references)
            {
                var fileRange = range as IFileRange;
                if (fileRange != null)
                    AddFindResultCore(fileRange);
            }

            findResultsListView.ItemsSource = source;
        }

        private void AddFindResultCore(IFileRange range)
        {
            if (source != null)
            {
                ListViewItemData data = new ListViewItemData();
                data.File = Path.GetFileName(range.FileName);
                data.Line = (range.StartPoint.Y + 1).ToString();
                data.SourceText = range.SourceText.Trim();
                data.Tag = range;
                source.Add(data);
            }
        }

        private void OpenProject()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                return;
            }

            NewFile(dir + @"Resources\Editor\Text\child1.cs");
            NewFile(dir + @"Resources\Editor\Text\child2.cs");
            NewFile(dir + @"Resources\Editor\Text\main.cs");
        }

        private string ExtractFileName(string fileName)
        {
            if (fileName != string.Empty)
            {
                FileInfo info = new FileInfo(fileName);
                return info.Name;
            }
            else
                return string.Empty;
        }

        private void NewFile(string fileName)
        {
            CsParser parser = new CsParser();

            TabItem page = new TabItem();
            page.Header = ExtractFileName(fileName);
            tcEditors.Items.Add(page);

            TextEditor edit = new TextEditor();
            edit.LineNumbersVisible = true;
            TextSource source = new TextSource();
            edit.SearchGlobal = searchMulti;
            editors.Add(page, edit);
            page.Content = edit;
            edit.Source = source;
            edit.Source.Lexer = parser;

            UpdateSearch();

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                parser.FileName = fileName;
                edit.Source.LoadFile(fileName);
                edit.Source.FileName = fileName;
            }

            tcEditors.SelectedIndex = tcEditors.Items.Count - 1;
        }

        private void UpdateSearch()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                ISearch search = (ISearch)edit;
                search.SearchGlobal = searchMulti;
            }
        }

        private TextEditor GetActiveSyntaxEdit()
        {
            // getting syntaxedit being focused
            return GetEditor(tcEditors.SelectedItem as TabItem);
        }

        private void FindClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.Focus();
                edit.DisplaySearchDialog();
            }
        }

        private void ReplaceClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.Focus();
                edit.DisplayReplaceDialog();
            }
        }

        private void GotoClick()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                edit.DisplayGotoLineDialog();
            }
        }

        private void FindResultsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigateToRange();
        }

        private void FindInFilesClick()
        {
            DisplaySearchInFilesDialog();
        }

        private void DisplaySearchInFilesDialog()
        {
            TextEditor edit = GetActiveSyntaxEdit();
            if (edit != null)
            {
                searchInFilesDialog.Execute(edit, true, false, null);
            }
        }

        private void Localize()
        {
            CultureInfo oldcInfo = Thread.CurrentThread.CurrentUICulture;
            switch (language)
            {
                case "Default":
                    StringConsts.Localize();
                    break;
                case "English":
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
                case "French":
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
                case "German":
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
                case "Spanish":
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
                case "Russian":
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
                case "Ukrainian":
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

        protected class ListViewItemData
        {
            public string File { get; set; }

            public string Line { get; set; }

            public string SourceText { get; set; }

            public object Tag { get; set; }
        }
    }
}
