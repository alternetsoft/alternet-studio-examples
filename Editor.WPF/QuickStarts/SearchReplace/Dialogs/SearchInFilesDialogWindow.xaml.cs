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
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Alternet.Common;
using Alternet.Common.Wpf;
using Alternet.Editor.Wpf;

namespace SearchReplace
{
    /// <summary>
    /// Represents a dialog box used to provide search and replace dialog.
    /// </summary>
    [DesignTimeVisible(false)]
    public partial class SearchInFilesDialogWindow : Window, IDlgSeparateWindow, INotifyPropertyChanged
    {
        private bool isReplace;
        private ISearch search;
        private bool findTextAtCursor;
        private SearchOptions options;
        private bool clearBookmarks;
        private bool firstSearch = true;
        private SearchOptions saveOptions;
        private string saveText = string.Empty;
        private bool optionsVisible = false;
        private bool selectionEnabled = false;
        private int updateCount;
        private string imagesResourceNameSuffix;
        private ResourceDictionary currentTheme;

        /// <summary>
        /// Initializes a new instance of the <c>SearchDialogWindow</c> class with default settings.
        /// </summary>
        public SearchInFilesDialogWindow()
        {
            InitializeComponent();
            FileTypesComboBox.Items.Clear();
            FileTypesComboBox.Items.Add("*.cs;*.vb;*.txt");
            FileTypesComboBox.Items.Add("*.*");
            FileTypesComboBox.SelectedIndex = 0;
            SetImageList();
            Closing += SearchDialogWindow_Closing;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a dictionary containing additional resources for the dialog window, such as control styles.
        /// </summary>
        public ResourceDictionary AdditionalResources { get; set; }

        /// <summary>
        /// Indicates whether all unnumbered bookmarks should be removed from the bookmarks collection.
        /// </summary>
        public bool ClearBookmarks
        {
            get
            {
                return clearBookmarks;
            }

            set
            {
                clearBookmarks = value;
            }
        }

        /// <summary>
        /// Gets or sets <c>ISearch</c> interface owning the dialog.
        /// </summary>
        public ISearch Search
        {
            get
            {
                return SearchShared ? SearchManager.SharedSearch.Search : search;
            }

            set
            {
                if (search == value)
                    return;

                Search?.UnhighlightAll();
                UpdateSearch(value, true);
                HighlightAll();
            }
        }

        /// <summary>
        /// Gets or sets the painter used to draw dialog's controls.
        /// </summary>
        public PopupPanelSearchDialogPainter Painter { get; set; }

        /// <summary>
        /// Gets or sets options that defines search and replace behavior.
        /// </summary>
        public SearchOptions Options
        {
            get
            {
                SearchOptions result = ((bool)MatchCaseCheckBox.IsChecked ? SearchOptions.CaseSensitive : 0) |
                    ((bool)MatchWholeWordCheckBox.IsChecked ? SearchOptions.WholeWordsOnly : 0) |
                    ((bool)UseRegularExpressionsCheckBox.IsChecked ? SearchOptions.RegularExpressions : 0) |
                    (findTextAtCursor ? SearchOptions.FindTextAtCursor : 0) |
                    (((options & SearchOptions.FindSelectedText) != 0) ? SearchOptions.FindSelectedText : 0) |
                    (((options & SearchOptions.CycledSearch) != 0) ? SearchOptions.CycledSearch : 0) |
                    (((options & SearchOptions.SilentSearch) != 0) ? SearchOptions.SilentSearch : 0);

                if ((search != null) && search.SearchGlobal)
                {
                    result |= (LookInComboBox.SelectedIndex == 1 ? SearchOptions.AllDocuments : 0) |
                    (LookInComboBox.SelectedIndex == 2 ? SearchOptions.CurrentProject : 0) |
                    (LookInComboBox.SelectedIndex == 3 ? SearchOptions.SelectionOnly : 0);
                }
                else
                {
                    result |= LookInComboBox.SelectedIndex == 1 ? SearchOptions.SelectionOnly : 0;
                }

                if ((result & SearchOptions.SelectionOnly) != 0)
                {
                    if ((options & SearchOptions.AllDocuments) != 0)
                        result |= SearchOptions.AllDocuments;

                    if ((options & SearchOptions.CurrentProject) != 0)
                        result |= SearchOptions.CurrentProject;

                    if ((options & SearchOptions.EntireScope) != 0)
                        result |= SearchOptions.EntireScope;
                }

                if (Search != null)
                {
                    if ((Search.SearchOptions & SearchOptions.DisplayIncrementalSearchDialog) != 0)
                        result |= SearchOptions.DisplayIncrementalSearchDialog;
                }
                else
                {
                    result |= EditConsts.DefaultIncrementalSearchOptions;
                }

                return result;
            }

            set
            {
                options = value;
                UpdateSearch();
                MatchCaseCheckBox.IsChecked = (value & SearchOptions.CaseSensitive) != 0;
                MatchWholeWordCheckBox.IsChecked = (value & SearchOptions.WholeWordsOnly) != 0;
                UseRegularExpressionsCheckBox.IsChecked = (value & SearchOptions.RegularExpressions) != 0;

                if ((search != null) && search.SearchGlobal)
                {
                    if ((value & SearchOptions.AllDocuments) != 0)
                        LookInComboBox.SelectedIndex = 1;
                    else
                    if ((value & SearchOptions.CurrentProject) != 0)
                        LookInComboBox.SelectedIndex = 2;

                    if ((value & SearchOptions.SelectionOnly) != 0)
                        LookInComboBox.SelectedIndex = LookInComboBox.Items.Count > 3 ? 3 : 0;
                }
                else
                {
                    if ((value & SearchOptions.SelectionOnly) != 0)
                        LookInComboBox.SelectedIndex = LookInComboBox.Items.Count > 1 ? 1 : 0;
                }

                SearchHintPopupButton.Visibility = (bool)UseRegularExpressionsCheckBox.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                findTextAtCursor = (value & SearchOptions.FindTextAtCursor) != 0;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether search can be executed through selected text.
        /// </summary>
        public bool SelectionEnabled
        {
            get
            {
                return selectionEnabled;
            }

            set
            {
                selectionEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether search or replace dialog should be executed.
        /// </summary>
        public bool IsReplace
        {
            get
            {
                return isReplace;
            }

            set
            {
                if (isReplace != value)
                {
                    isReplace = value;
                    ReplaceChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether options group box should be visible.
        /// </summary>
        public bool OptionsVisible
        {
            get
            {
                return optionsVisible;
            }

            set
            {
                if (optionsVisible == value)
                    return;

                optionsVisible = value;

                UpdateOptionsVisibility();
            }
        }

        /// <summary>
        /// Gets or sets the suffix of the resource name to determine images for the specific color theme.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImagesResourceNameSuffix
        {
            get => imagesResourceNameSuffix;

            set
            {
                if (imagesResourceNameSuffix == value)
                    return;

                imagesResourceNameSuffix = value;
                SetImageList();
            }
        }

        /// <summary>
        /// Represents the search history for the search dialog.
        /// </summary>
        public IList SearchList => FindWhatComboBox.Items;

        /// <summary>
        /// Represents the replace history for the replace dialog.
        /// </summary>
        public IList ReplaceList => ReplaceWithComboBox.Items;

        /// <summary>
        /// Represents the parent control.
        /// </summary>
        public Control ParentEditorControl { get; private set; }

        /// <summary>
        /// Gets or sets a dictionary containing theme-related resources for the dialog window, such as combobox or button background.
        /// </summary>
        public ResourceDictionary CurrentTheme
        {
            get
            {
                return currentTheme;
            }

            set
            {
                if (currentTheme == value)
                    return;

                currentTheme = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTheme)));
            }
        }

        protected virtual bool SearchShared
        {
            get
            {
                return SearchManager.SharedSearch.Shared;
            }

            set
            {
                if (SearchManager.SharedSearch.Shared != value)
                {
                    SearchManager.SharedSearch.Shared = value;
                    SearchManager.SharedSearch.FirstSearch = true;
                    SearchManager.SharedSearch.Init(ref search, Options);
                }
            }
        }

        protected virtual bool FirstSearch
        {
            get
            {
                return SearchShared ? SearchManager.SharedSearch.FirstSearch : search.FirstSearch;
            }
        }

        /// <summary>
        /// Resets <c>DlgSearch</c> to the start of search.
        /// </summary>
        public void Init()
        {
            firstSearch = true;
            saveOptions = SearchOptions.None;
            saveText = string.Empty;
            UpdateSearch();
        }

        /// <summary>
        /// Resets <c>IDlgSearch</c> to the start of search.
        /// <param name="search">New <c>ISearch</c> object that performs search.</param>
        /// <param name="options">New search options.</param>
        /// </summary>
        public void Init(ISearch search, SearchOptions options)
        {
            updateCount++;
            try
            {
                Search = search;
                Init();
                if (options != SearchOptions.None)
                    Options = options;
            }
            finally
            {
                updateCount--;
            }
        }

        /// <summary>
        /// Updates text to find.
        /// </summary>
        /// <param name="text">New text to search.</param>
        public void UpdateFindText(string text)
        {
            FindWhatComboBox.Text = text;
        }

        /// <summary>
        /// Highlights all occurrences of the search text.
        /// </summary>
        public void HighlightAll()
        {
            if (updateCount > 0)
                return;

            if (Search != null && Search.HighlightSearchResults && !Search.InIncrementalSearch)
                HighlightAll(FindWhatComboBox.Text, Options, GetExpression((Options & SearchOptions.BackwardSearch) != 0));
        }

        /// <summary>
        /// Toggles searching for whole words on/off.
        /// </summary>
        public void ToggleWholeWord()
        {
            MatchWholeWordCheckBox.IsChecked = !MatchWholeWordCheckBox.IsChecked;
        }

        /// <summary>
        /// Toggles using regular expressions on/off.
        /// </summary>
        public void ToggleRegularExpressions()
        {
            UseRegularExpressionsCheckBox.IsChecked = !UseRegularExpressionsCheckBox.IsChecked;
        }

        /// <summary>
        /// Toggles case sensitive searching on/off.
        /// </summary>
        public void ToggleMatchCase()
        {
            MatchCaseCheckBox.IsChecked = !MatchCaseCheckBox.IsChecked;
        }

        /// <summary>
        /// Toggles prompting before replacing on/off.
        /// </summary>
        public void TogglePromptOnReplace()
        {
        }

        /// <summary>
        /// Toggles searching direction towards/backwards.
        /// </summary>
        public void ToggleSearchUp()
        {
        }

        /// <summary>
        /// Toggles searching through hidden text on/off.
        /// </summary>
        public void ToggleHiddenText()
        {
        }

        /// <summary>
        /// Enables/disables regular expressions.
        /// </summary>
        /// <param name="enable">Specifies whether regular expressions checkbox should be enabled.</param>
        public void EnableRegularExpressions(bool enable)
        {
            UseRegularExpressionsCheckBox.IsEnabled = enable;
        }

        /// <summary>
        /// Shows/hide regular expressions.
        /// </summary>
        /// <param name="show">Specifies whether regular expressions checkbox should be visible.</param>
        public void ShowRegularExpressions(bool show)
        {
            UseRegularExpressionsCheckBox.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Show(Control parentEditorControl, PopupPanelSearchDialogPainter painter)
        {
            Show();
        }

        /// <summary>
        /// Updates search engine.
        /// </summary>
        /// <param name="newSearch">New <c>ISearch</c> object that performs search.</param>
        /// <param name="update">True if given search engine should be set as current search.</param>
        public void UpdateSearch(ISearch newSearch, bool update)
        {
            if (search == newSearch)
                return;

            search = newSearch;

            if (SearchShared)
                SearchManager.SharedSearch.UpdateSearch(newSearch, update);

            if (search != null)
            {
                var visibility = search.InIncrementalSearch ? Visibility.Collapsed : Visibility.Visible;
                SearchReplaceButtonsToolbar.Visibility = visibility;
                OptionsGrid.Visibility = visibility;
                LookInComboBox.Visibility = visibility;
                LookInPromptLabel.Visibility = visibility;
                var replaceVisibility = isReplace && !search.InIncrementalSearch ? Visibility.Visible : Visibility.Collapsed;
                ReplaceButton.Visibility = replaceVisibility;
                ReplaceAllButton.Visibility = replaceVisibility;
                ReplaceWithComboBox.Visibility = replaceVisibility;
                ReplaceHintPopupButton.Visibility = isReplace && !search.InIncrementalSearch && (bool)UseRegularExpressionsCheckBox.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                SearchHintPopupButton.Visibility = (bool)UseRegularExpressionsCheckBox.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        protected void ShowNotFound()
        {
            ShowNotFound(StringConsts.DlgSearchReplaceCaption);
            FindWhatComboBox.Focus();
        }

        protected virtual bool NeedReplaceCurrent(out Match match)
        {
            return SearchShared ? SearchManager.SharedSearch.NeedReplaceCurrent(out match) : Search.NeedReplaceCurrent(out match);
        }

        protected virtual bool ReplaceCurrent(string replaceWith, SearchOptions options, Match match)
        {
            return SearchShared ? SearchManager.SharedSearch.ReplaceCurrent(replaceWith, options, match) : Search.ReplaceCurrent(replaceWith, options, match);
        }

        protected virtual bool FindPrevious()
        {
            return SearchShared ? SearchManager.SharedSearch.FindPrevious() : Search.FindPrevious();
        }

        protected virtual bool FindNext()
        {
            return SearchShared ? SearchManager.SharedSearch.FindNext() : Search.FindNext();
        }

        protected virtual bool Find(string text, SearchOptions options, Regex expression)
        {
            return SearchShared ? SearchManager.SharedSearch.Find(Search, text, options, expression) : Search.Find(text, options, expression);
        }

        protected virtual int MarkAll(string text, SearchOptions options, Regex expression, bool clearPrevious)
        {
            return SearchShared ? SearchManager.SharedSearch.MarkAll(Search, text, options, expression, clearPrevious) : Search.MarkAll(text, options, expression, clearPrevious);
        }

        protected virtual int HighlightAll(string text, SearchOptions options, Regex expression)
        {
            return Search.HighlightAll(text, options, expression);
        }

        protected virtual bool ReplaceAll(string text, string replaceWith, SearchOptions options, Regex expression, out int count)
        {
            bool abort;
            return SearchShared ? SearchManager.SharedSearch.ReplaceAll(Search, text, replaceWith, options, expression, out count, out abort) : Search.ReplaceAll(text, replaceWith, options, expression, out count, out abort);
        }

        protected virtual void ShowNotFound(string caption)
        {
            if (SearchShared)
                SearchManager.SharedSearch.ShowNotFound(Search, caption);
            else
                Search.ShowNotFound(caption);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = HandleKeyDown(e.Key);
            base.OnPreviewKeyDown(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowHelper.RemoveIcon(this);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            var textBox = FindWhatComboBox.FindChild<TextBox>("PART_EditableTextBox");

            if (textBox != null)
                textBox.SelectAll();

            FindWhatComboBox.Focus();

            ReplaceChanged();
            UpdateOptionsVisibility();
        }

        protected void ReplaceChanged()
        {
            var rep = isReplace ? Visibility.Visible : Visibility.Collapsed;
            var popupRep = (bool)UseRegularExpressionsCheckBox.IsChecked && isReplace ? Visibility.Visible : Visibility.Collapsed;
            var inc = Search.InIncrementalSearch;
            ReplacePromptLabel.Visibility = rep;
            ReplaceWithComboBox.Visibility = rep;
            ReplaceHintPopupButton.Visibility = popupRep;
            ReplaceButton.Visibility = rep;
            ReplaceAllButton.Visibility = rep;

            SearchHintPopupButton.Visibility = (bool)UseRegularExpressionsCheckBox.IsChecked ? Visibility.Visible : Visibility.Collapsed;

            FindModeButton.IsChecked = !isReplace;
            ReplaceModeButton.IsChecked = isReplace;
            FindButton.IsDefault = !isReplace;
            ReplaceButton.IsDefault = isReplace;
        }

        private static void PerformClick(Button button)
        {
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private bool HandleKeyDown(Key keyData)
        {
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (keyData == Key.Return || keyData == Key.Enter || (keyData == Key.F3 && !shift))
            {
                if (ReplaceWithComboBox.IsKeyboardFocusWithin)
                {
                    PerformClick(ReplaceButton);
                    return true;
                }

                if (FindWhatComboBox.IsKeyboardFocusWithin)
                {
                    if (Search != null && Search.InIncrementalSearch)
                        Close();
                    else
                        PerformClick(FindButton);

                    return true;
                }
            }

            return false;
        }

        private RegexOptions GetRegexOptions(string text, bool searchUp = false)
        {
            return (searchUp ? RegexOptions.RightToLeft : 0) |
                (!MatchCaseCheckBox.IsChecked.Value ? RegexOptions.IgnoreCase : 0) |
                ((text != null) && ((text.IndexOf(@"\r") >= 0) || (text.IndexOf(@"\n") >= 0)) ? RegexOptions.Multiline : 0);
        }

        private bool IsEscapeChar(string s, int index)
        {
            bool isEscape = false;
            while ((index > 0) && (s[index - 1] == '\\'))
            {
                if (s[index - 1] == '\\')
                    isEscape = !isEscape;
                index--;
            }

            return isEscape;
        }

        private void RemoveChar(ref string s, char ch)
        {
            int i = s.Length - 1;
            while (i > 0)
            {
                if ((s[i] == ch) && IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);
                }

                i--;
            }
        }

        private void ReplaceChar(ref string s, char ch, string replace)
        {
            int i = s.Length - 1;
            while (i > 0)
            {
                if ((s[i] == ch) && IsEscapeChar(s, i))
                {
                    s = s.Remove(i - 1, 2);

                    if (replace != string.Empty)
                        s = s.Insert(i - 1, replace);
                }

                i--;
            }
        }

        private void FixCarriageReturn(ref string s)
        {
            ReplaceChar(ref s, 'r', string.Empty);
            ReplaceChar(ref s, 'n', @"\r\n");
        }

        private Regex GetExpression(bool searchUp = false)
        {
            Regex result = null;

            if (UseRegularExpressionsCheckBox.IsChecked.Value)
            {
                try
                {
                    string s = FindWhatComboBox.Text;
                    FixCarriageReturn(ref s);
                    result = new Regex(s, GetRegexOptions(FindWhatComboBox.Text, searchUp));
                }
                catch (Exception e)
                {
                    result = null;
                    ErrorHandler.Error(e);
                }
            }

            return result;
        }

        private void AddToHistory(IList list, string s)
        {
            updateCount++;
            try
            {
                if (s != string.Empty)
                {
                    int idx = list.IndexOf(s);
                    while (idx >= 0)
                    {
                        list.RemoveAt(idx);
                        idx = list.IndexOf(s);
                    }

                    list.Insert(0, s);
                }
            }
            finally
            {
                updateCount--;
            }
        }

        private void AddToHistory()
        {
            updateCount++;
            try
            {
                if (FindWhatComboBox.Text != string.Empty)
                {
                    AddToHistory(FindWhatComboBox.Items, FindWhatComboBox.Text);
                    FindWhatComboBox.SelectedIndex = 0;
                }

                if (isReplace && (ReplaceWithComboBox.Text != string.Empty))
                {
                    AddToHistory(ReplaceWithComboBox.Items, ReplaceWithComboBox.Text);
                    ReplaceWithComboBox.SelectedIndex = 0;
                }
            }
            finally
            {
                updateCount--;
            }
        }

        private void TextFound()
        {
            search = Search;
            firstSearch = false;
            saveOptions = Options;
            saveText = FindWhatComboBox.Text;
        }

        private void UpdateSearch()
        {
            int saveIndex = LookInComboBox.SelectedIndex;

            try
            {
                LookInComboBox.Items.Clear();
                LookInComboBox.Items.Add(StringConsts.CurrentDocumentCaption);

                if ((search != null) && search.SearchGlobal)
                {
                    LookInComboBox.Items.Add(StringConsts.AllOpenDocumentsScope);
                    LookInComboBox.Items.Add(StringConsts.CurrentProjectScope);
                }

                if (selectionEnabled)
                    LookInComboBox.Items.Add(StringConsts.SelectionOnlyCaption.Replace("&", string.Empty));
            }
            finally
            {
                LookInComboBox.SelectedIndex = (saveIndex >= 0) && (saveIndex < LookInComboBox.Items.Count) ? saveIndex : 0;
            }
        }

        private void SetImage(System.Drawing.Image source, Image dest, Grid grid)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            source.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            dest.Source = bi;
        }

        private void SetImageList()
        {
            Func<string, System.Windows.Forms.ImageList> load = suffix =>
            {
                string name = $"Alternet.Editor.Wpf.SearchDialogImages{ImagesResourceNameSuffix}";
                return ImageListHelper.LoadImageListFromStrip(
                    typeof(TextEditor),
                    string.Format("{0}{1}.png", name, suffix));
            };

            System.Windows.Forms.ImageList list = new DisplayScaledImages(() => load(string.Empty), () => load("HighDpi")).Images;
            if (list.Images.Count > 1)
            {
                SetImage(list.Images[0], FindImage, FindGrid);
                SetImage(list.Images[1], ReplaceImage, ReplaceGrid);
            }
        }

        private void UpdateOptionsVisibility()
        {
            OptionsVisibleButton.Content = FindResource(optionsVisible ? "OptionsExpandedMinusGlyph" : "OptionsCollapsedPlusGlyph");
            OptionsGroupBox.Visibility = optionsVisible ? Visibility.Visible : Visibility.Collapsed;
            OptionsCollapsedDecoration.Visibility = !optionsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FindPrevButton_Click(object sender, RoutedEventArgs e)
        {
            DoFindNext(true);
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            DoFindNext();
        }

        private void DoFindNext(bool searchUp = false)
        {
            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
            {
                SearchOptions opt = Options;
                bool res;

                if (!(FirstSearch || firstSearch) && (FindWhatComboBox.Text == saveText) && (opt == saveOptions))
                {
                    if (searchUp)
                        res = FindPrevious();
                    else
                        res = FindNext();
                }
                else
                    res = Find(FindWhatComboBox.Text, searchUp ? opt | SearchOptions.BackwardSearch : opt, GetExpression(searchUp));

                if (res)
                    TextFound();
                else
                    ShowNotFound();
            }
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isReplace)
                IsReplace = true;
            else
            {
                AddToHistory();
                UpdateSearchMode();

                if (Search != null)
                {
                    Match match = null;
                    SearchOptions opt = Options;
                    bool res;

                    if (!FirstSearch && (FindWhatComboBox.Text == saveText) && NeedReplaceCurrent(out match))
                    {
                        res = true;
                    }
                    else
                    {
                        res = Find(FindWhatComboBox.Text, opt, GetExpression((opt & SearchOptions.BackwardSearch) != 0));
                    }

                    if (res)
                    {
                        if (ReplaceCurrent(ReplaceWithComboBox.Text, opt, match))
                        {
                            if ((Options & SearchOptions.BackwardSearch) != 0)
                                res = FindPrevious();
                            else
                                res = FindNext();
                        }
                        else
                            return;
                    }
                    else
                        res = Find(FindWhatComboBox.Text, opt, GetExpression((opt & SearchOptions.BackwardSearch) != 0));

                    if (res)
                        TextFound();
                    else
                        ShowNotFound();
                }
            }
        }

        private void ReplaceAllButton_Click(object sender, RoutedEventArgs e)
        {
            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
            {
                SearchOptions opt = Options;
                int count;

                if (!ReplaceAll(FindWhatComboBox.Text, ReplaceWithComboBox.Text, Options, GetExpression((Options & SearchOptions.BackwardSearch) != 0), out count))
                    ShowNotFound();
                else
                    MessageBoxWpfHandler.Show(string.Format(StringConsts.OccurrencesReplaced, count), StringConsts.DlgSearchReplaceCaption);
            }
        }

        private void FindAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchManager.SharedSearch != null)
            {
                SearchManager.SharedSearch.FindAll(Search, FindWhatComboBox.Text, Options & ~SearchOptions.BackwardSearch, GetExpression(), FileTypesComboBox.Text);
            }

            Close();
        }

        private void UseRegularExpressionsCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var popupRep = (bool)UseRegularExpressionsCheckBox.IsChecked && isReplace ? Visibility.Visible : Visibility.Collapsed;

            SearchHintPopupButton.Visibility = UseRegularExpressionsCheckBox.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
            ReplaceHintPopupButton.Visibility = popupRep;

            HighlightAll();
        }

        private void UpdateMode()
        {
            IsReplace = ReplaceModeButton.IsChecked.Value;
        }

        private void FindModeButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            ReplaceModeButton.IsChecked = !FindModeButton.IsChecked;
            UpdateMode();
        }

        private void ReplaceModeButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            FindModeButton.IsChecked = !ReplaceModeButton.IsChecked;
            UpdateMode();
        }

        private void OptionsVisibleButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsVisible = !optionsVisible;
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;

            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;

            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        private void UpdateSearchMode()
        {
            switch (LookInComboBox.SelectedIndex)
            {
                case 1:
                case 2:
                    SearchShared = ((search != null) && search.SearchGlobal) ? true : false;

                    // SearchShared = true;
                    break;

                default:
                    SearchShared = false;
                    break;
            }
        }

        private void FindWhatComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (updateCount > 0)
                return;

            if ((Search != null) && Search.InIncrementalSearch)
            {
                Search.IncrementalSearch(FindWhatComboBox.Text);
            }
            else
                HighlightAll();
        }

        private void MatchCaseCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            HighlightAll();
        }

        private void MatchWholeWordCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            HighlightAll();
        }

        private void SearchDialogWindow_Closing(object sender, CancelEventArgs e)
        {
            if (Search != null && Search.InIncrementalSearch)
                Search.FinishIncrementalSearch();
        }
    }
}
