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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Dialogs;
using Alternet.Editor.TextSource;

namespace SearchReplace
{
    /// <summary>
    /// Represents a windows form used to provide search and replace dialog.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Designer generated code")]
    public partial class DlgSearchInFiles : Form, IFormDlgSearch
    {
        #region Fields

        private const int DefaultTabControlItemWidth = 80;
        private const int DefaultTabControlItemHeight = 22;
        private const int DefaultOptionsHeight = 142;
        private const int DefaultOptionsTop = 80;
        private const int DefaultOptionsReplaceTop = 126;
#if NETCOREAPP
        private const int DefaultButtonsHeigth = 42;
        private const int DefaultButtonsReplaceHeigth = 77;
        private const int DefaultOptionsClosedHeight = 28;
#else
        private const int DefaultButtonsHeigth = 36;
        private const int DefaultButtonsReplaceHeigth = 65;
        private const int DefaultOptionsClosedHeight = 24;
#endif
        private const int DefaultFindLocation = 54;
        private const int DefaultDialogWidthOffset = 14;

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
        private string lastText = string.Empty;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <c>DlgSearch</c> class with default settings.
        /// </summary>
        public DlgSearchInFiles()
        {
            InitializeComponent();
            SetImageList();
        }

        #region DlgSearch Members

        /// <summary>
        /// Gets or sets <c>ISearch</c> interface owning the dialog.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

                lastText = string.Empty;
                Search?.UnhighlightAll();
                UpdateSearch(value, true);
                HighlightAll();
            }
        }

        /// <summary>
        /// Gets or sets options that define search and replace behavior.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SearchOptions Options
        {
            get
            {
                SearchOptions result = (chbMatchCase.Checked ? SearchOptions.CaseSensitive : 0) |
                    (chbMatchWholeWord.Checked ? SearchOptions.WholeWordsOnly : 0) |
                    (chbUseRegularExpressions.Checked ? SearchOptions.RegularExpressions : 0) |
                    (findTextAtCursor ? SearchOptions.FindTextAtCursor : 0) |
                    (((options & SearchOptions.FindSelectedText) != 0) ? SearchOptions.FindSelectedText : 0) |
                    (((options & SearchOptions.CycledSearch) != 0) ? SearchOptions.CycledSearch : 0) |
                    (((options & SearchOptions.SilentSearch) != 0) ? SearchOptions.SilentSearch : 0);

                if ((search != null) && search.SearchGlobal)
                {
                    result |= (cbLookIn.SelectedIndex == 1 ? SearchOptions.AllDocuments : 0) |
                    (cbLookIn.SelectedIndex == 2 ? SearchOptions.CurrentProject : 0) |
                    (cbLookIn.SelectedIndex == 3 ? SearchOptions.SelectionOnly : 0);
                }
                else
                {
                    result |= cbLookIn.SelectedIndex == 1 ? SearchOptions.SelectionOnly : 0;
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
                chbMatchCase.Checked = (value & SearchOptions.CaseSensitive) != 0;
                chbMatchWholeWord.Checked = (value & SearchOptions.WholeWordsOnly) != 0;
                chbUseRegularExpressions.Checked = (value & SearchOptions.RegularExpressions) != 0;

                if ((search != null) && search.SearchGlobal)
                {
                    if ((value & SearchOptions.AllDocuments) != 0)
                        cbLookIn.SelectedIndex = 1;
                    else
                        if ((value & SearchOptions.CurrentProject) != 0)
                        cbLookIn.SelectedIndex = 2;

                    if ((value & SearchOptions.SelectionOnly) != 0)
                        cbLookIn.SelectedIndex = cbLookIn.Items.Count > 3 ? 3 : 0;
                }
                else
                {
                    if ((value & SearchOptions.SelectionOnly) != 0)
                        cbLookIn.SelectedIndex = cbLookIn.Items.Count > 1 ? 1 : 0;
                }

                btPopup.Enabled = chbUseRegularExpressions.Checked;
                findTextAtCursor = (value & SearchOptions.FindTextAtCursor) != 0;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether search can be executed through selected text.
        /// </summary>
        [DefaultValue(false)]
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
        /// Gets or sets a boolean value that indicates whether the search or replace dialog should be executed.
        /// </summary>
        [DefaultValue(false)]
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
        /// Indicates whether the options group box should be visible.
        /// </summary>
        [DefaultValue(false)]
        public bool OptionsVisible
        {
            get
            {
                return optionsVisible;
            }

            set
            {
                optionsVisible = value;
            }
        }

        /// <summary>
        /// Indicates whether all unnumbered bookmarks should be removed from the bookmark collection.
        /// </summary>
        [DefaultValue(false)]
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
        /// Represents the search history for the search dialog.
        /// </summary>
        public IList SearchList => cbFindWhat.Items;

        /// <summary>
        /// Represents the replace history for the replace dialog.
        /// </summary>
        public IList ReplaceList => cbReplaceWith.Items;

        protected virtual bool FirstSearch
        {
            get
            {
                return SearchShared ? SearchManager.SharedSearch.FirstSearch : search.FirstSearch;
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDisposable Painter { get; set; }

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
        /// Highlights all occurrences of specified string in the text content.
        /// </summary>
        public void HighlightAll()
        {
            if (updateCount > 0)
                return;

            if (Search != null && Search.HighlightSearchResults && !Search.InIncrementalSearch)
                HighlightAll(cbFindWhat.Text, Options, GetExpression((Options & SearchOptions.BackwardSearch) != 0));
        }

        /// <summary>
        /// Toggles searching for whole words on/off.
        /// </summary>
        public void ToggleWholeWord()
        {
            chbMatchWholeWord.Checked = !chbMatchWholeWord.Checked;
        }

        /// <summary>
        /// Toggles using regular expressions on/off.
        /// </summary>
        public void ToggleRegularExpressions()
        {
            chbUseRegularExpressions.Checked = !chbUseRegularExpressions.Checked;
        }

        /// <summary>
        /// Toggles case sensitive searching on/off.
        /// </summary>
        public void ToggleMatchCase()
        {
            chbMatchCase.Checked = !chbMatchCase.Checked;
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
            chbUseRegularExpressions.Enabled = enable;
        }

        /// <summary>
        /// Shows/hide regular expressions.
        /// </summary>
        /// <param name="show">Specifies whether regular expressions checkbox should be visible.</param>
        public void ShowRegularExpressions(bool show)
        {
            chbUseRegularExpressions.Visible = show;
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
                ResizeControls();
            }
        }

        /// <summary>
        /// Updates text to find.
        /// </summary>
        /// <param name="text">New text to search.</param>
        public void UpdateFindText(string text)
        {
            cbFindWhat.Text = text;
        }

        /// <summary>
        /// Displays the dialog with the specified parent control.
        /// </summary>
        /// <param name="ownerControl">Parent container of the dialog.</param>
        public void Show(Control ownerControl)
        {
            Show(ownerControl as IWin32Window);
        }

        #endregion

        #region Protected Members

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ResizeControls();
        }

        protected void ReplaceChanged()
        {
            laReplaceWith.Visible = isReplace;
            cbReplaceWith.Visible = isReplace;
            btReplacePopup.Visible = isReplace;
            btReplace.Visible = isReplace;
            btReplaceAll.Visible = isReplace;
            btMarkAll.Visible = !isReplace;

            btPopup.Enabled = chbUseRegularExpressions.Checked;
            btReplacePopup.Enabled = chbUseRegularExpressions.Checked;
            tbSearch.SelectedIndex = isReplace ? 1 : 0;
            AcceptButton = IsReplace ? btReplace : btFindNext;

            ResizeControls();
        }

        protected void ShowNotFound()
        {
            ShowNotFound(Text);
            cbFindWhat.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var incremental = Search.InIncrementalSearch;
            if ((cbFindWhat.Text == string.Empty) && (cbFindWhat.Items.Count > 0) && !incremental)
                cbFindWhat.SelectedIndex = 0;
            FileTypesComboBox.SelectedIndex = 0;
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
            return SearchShared ? SearchManager.SharedSearch.ReplaceAll(Search, text, replaceWith, options & ~SearchOptions.BackwardSearch, expression, out count, out abort) : Search.ReplaceAll(text, replaceWith, options, expression, out count, out abort);
        }

        protected virtual void ShowNotFound(string caption)
        {
            if (SearchShared)
                SearchManager.SharedSearch.ShowNotFound(Search, caption);
            else
                Search.ShowNotFound(caption);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            if (msg.Msg == WM_KEYDOWN)
            {
                return HandleKeyDown(keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Private Methods

        private bool HandleKeyDown(Keys keyData)
        {
            if (keyData == Keys.Return || keyData == Keys.Enter || keyData == Keys.F3)
            {
                if (cbReplaceWith.Focused)
                {
                    btReplace.PerformClick();
                    return true;
                }

                if (cbFindWhat.Focused)
                {
                    if (Search != null && Search.InIncrementalSearch)
                        Close();
                    else
                        btFindNext.PerformClick();

                    return true;
                }
            }

            return false;
        }

        private void UpdateSearch()
        {
            int saveIndex = cbLookIn.SelectedIndex;

            try
            {
                cbLookIn.Items.Clear();
                cbLookIn.Items.Add(StringConsts.CurrentDocumentCaption);

                if ((search != null) && search.SearchGlobal)
                {
                    cbLookIn.Items.Add(StringConsts.AllOpenDocumentsScope);
                    cbLookIn.Items.Add(StringConsts.CurrentProjectScope);
                }

                if (selectionEnabled)
                    cbLookIn.Items.Add(StringConsts.SelectionOnlyCaption.Replace("&", string.Empty));
            }
            finally
            {
                cbLookIn.SelectedIndex = (saveIndex >= 0) && (saveIndex < cbLookIn.Items.Count) ? saveIndex : 0;
            }
        }

        private void SetImageList()
        {
            Func<string, ImageList> load = suffix =>
                ImageListHelper.LoadImageListFromStrip(
                    typeof(DlgSearch),
                    string.Format("Alternet.Editor.Images.SearchDialogImages{0}.png", suffix));

            tbSearch.ImageList = new DisplayScaledImages(() => load(string.Empty), () => load("HighDpi")).Images;
        }

        private string RemoveAmpersand(string str)
        {
            return str.Replace("&", string.Empty);
        }

        private void LoadFromResource()
        {
            laFindWhat.Text = StringConsts.FindWhatCaption;
            laReplaceWith.Text = StringConsts.ReplaceWithCaption;
            chbMatchCase.Text = StringConsts.MatchCaseCaption;
            chbMatchWholeWord.Text = StringConsts.MatchWholeWordCaption;
            chbUseRegularExpressions.Text = StringConsts.UseRegularExpressionsCaption;

            btFindNext.Text = StringConsts.FindNextCaption;
            btReplace.Text = StringConsts.ReplaceCaption + " ";
            btReplaceAll.Text = StringConsts.ReplaceAllCaption;
            btMarkAll.Text = StringConsts.MarkAllCaption;
            gbFindOptions.Text = string.Format("     {0}", StringConsts.FindOptionsCaption);
            laFindOptions.Text = string.Format(StringConsts.FindOptionsCaption);
            laLookIn.Text = StringConsts.LookInCaption;
            Text = StringConsts.DlgSearchReplaceCaption;

            miSingleChar.Text = StringConsts.SingleCharCaption;
            miZeroOrMore.Text = StringConsts.ZeroOrMoreCaption;
            miOneOrMore.Text = StringConsts.OneOrMoreCaption;
            miBeginLine.Text = StringConsts.BeginLineCaption;
            miEndLine.Text = StringConsts.EndLineCaption;
            miLineBreak.Text = StringConsts.LineBreakCaption;
            miOneCharInSet.Text = StringConsts.OneCharInSetCaption;
            miOneCharNotInSet.Text = StringConsts.OneCharNotInSetCaption;
            miOr.Text = StringConsts.OrCaption;
            miEscape.Text = StringConsts.EscapeCaption;
            miTag.Text = StringConsts.TagCaption;
            miFindWhatText.Text = StringConsts.FindWhatTextCaption;
            miTaggedExpression1.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 1);
            miTaggedExpression2.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 2);
            miTaggedExpression3.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 3);
            miTaggedExpression4.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 4);
            miTaggedExpression5.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 5);
            miTaggedExpression6.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 6);
            miTaggedExpression7.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 7);
            miTaggedExpression8.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 8);
            miTaggedExpression9.Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 9);
        }

        private void btReplacePopup_Click(object sender, System.EventArgs e)
        {
            cmReplace.Show((Control)sender, new Point(0, 0));
        }

        private void miFindWhatText_Click(object sender, System.EventArgs e)
        {
            string s = ((ToolStripMenuItem)sender).Text.Trim();
            int idx = s.IndexOf(Consts.Space);

            if (idx > 0)
                s = s.Substring(0, idx);
            cbReplaceWith.Text = cbReplaceWith.Text + s;
        }

        private void UpdateSearchMode()
        {
            switch (cbLookIn.SelectedIndex)
            {
                case 1:
                case 2:
                    SearchShared = ((search != null) && search.SearchGlobal) ? true : false;
                    break;

                default:
                    SearchShared = false;
                    break;
            }
        }

        private RegexOptions GetRegexOptions(string text, bool searchUp = false)
        {
            return (searchUp ? RegexOptions.RightToLeft : 0) |
                (!chbMatchCase.Checked ? RegexOptions.IgnoreCase : 0) |
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

            if (chbUseRegularExpressions.Checked)
            {
                try
                {
                    string s = cbFindWhat.Text;
                    FixCarriageReturn(ref s);
                    result = new Regex(s, GetRegexOptions(cbFindWhat.Text, searchUp));
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

        private void AddToHistory()
        {
            updateCount++;
            try
            {
                if (cbFindWhat.Text != string.Empty)
                {
                    AddToHistory(cbFindWhat.Items, cbFindWhat.Text);
                    cbFindWhat.SelectedIndex = 0;
                }

                if (isReplace && (cbReplaceWith.Text != string.Empty))
                {
                    AddToHistory(cbReplaceWith.Items, cbReplaceWith.Text);
                    cbReplaceWith.SelectedIndex = 0;
                }
            }
            finally
            {
                updateCount--;
            }
        }

        private void TextFound()
        {
            firstSearch = false;
            saveOptions = Options;
            saveText = cbFindWhat.Text;
        }

        private void btPopup_Click(object sender, System.EventArgs e)
        {
            cmFind.Show((Control)sender, new Point(0, 0));
        }

        private void btFindNext_Click(object sender, System.EventArgs e)
        {
            DoFindNext();
        }

        private void btFindPrevious_Click(object sender, EventArgs e)
        {
            DoFindNext(true);
        }

        private void DoFindNext(bool searchUp = false)
        {
            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
            {
                SearchOptions opt = Options;
                bool res;

                if (!(FirstSearch || firstSearch) && (cbFindWhat.Text == saveText) && (opt == saveOptions))
                {
                    if (searchUp)
                        res = FindPrevious();
                    else
                        res = FindNext();
                }
                else
                    res = Find(cbFindWhat.Text, searchUp ? opt | SearchOptions.BackwardSearch : opt, GetExpression(searchUp));

                if (res)
                    TextFound();
                else
                    ShowNotFound();
            }
        }

        private void btReplace_Click(object sender, System.EventArgs e)
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

                    if (!FirstSearch && (cbFindWhat.Text == saveText) && NeedReplaceCurrent(out match))
                    {
                        res = true;
                    }
                    else
                    {
                        res = Find(cbFindWhat.Text, opt, GetExpression((opt & SearchOptions.BackwardSearch) != 0));
                    }

                    if (res)
                    {
                        if (ReplaceCurrent(cbReplaceWith.Text, opt, match))
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
                        res = Find(cbFindWhat.Text, opt, GetExpression((opt & SearchOptions.BackwardSearch) != 0));

                    if (res)
                        TextFound();
                    else
                        ShowNotFound();
                }
            }
        }

        private void btReplaceAll_Click(object sender, System.EventArgs e)
        {
            if (Search != null)
            {
                int count;

                if (!ReplaceAll(cbFindWhat.Text, cbReplaceWith.Text, Options, GetExpression((Options & SearchOptions.BackwardSearch) != 0), out count))
                    ShowNotFound();
                else
                    MessageBoxHandler.Show(this, string.Format(StringConsts.OccurrencesReplaced, count), Text);
            }

            Close();
        }

        private void btMarkAll_Click(object sender, System.EventArgs e)
        {
            AddToHistory();
            UpdateSearchMode();

            if (Search != null)
                MarkAll(cbFindWhat.Text, Options, GetExpression((Options & SearchOptions.BackwardSearch) != 0), clearBookmarks);
        }

        private void chbUseRegularExpressions_Click(object sender, System.EventArgs e)
        {
            btPopup.Enabled = chbUseRegularExpressions.Checked;
            btReplacePopup.Enabled = chbUseRegularExpressions.Checked;

            HighlightAll();
        }

        private void miBeginWord_Click(object sender, System.EventArgs e)
        {
            string s = ((ToolStripMenuItem)sender).Text.Trim();
            int idx = s.IndexOf(Consts.Space);

            if (idx > 0)
                s = s.Substring(0, idx);
            cbFindWhat.Text = cbFindWhat.Text + s;
        }

        private void btFindOptions_Click(object sender, EventArgs e)
        {
            optionsVisible = !optionsVisible;
            ResizeControls();
        }

        private void ResizeControls()
        {
            float scaleFactor = DisplayScaling.AutoScale((float)Font.Height / DefaultFont.Height);
            bool incremental = search != null ? search.InIncrementalSearch : false;
            btFindOptions.Visible = !incremental;
            tbSearch.Visible = !incremental;
            gbFindOptions.Visible = optionsVisible && !incremental;
            laFindOptions.Visible = !optionsVisible && !incremental;
            pnFindOptions.Visible = !optionsVisible && !incremental;
            cbLookIn.Visible = !incremental;
            laLookIn.Visible = !incremental;
            btPopup.Visible = !incremental;
            btFindOptions.Text = optionsVisible ? "-" : "+";
            btFindPrevious.Text = StringConsts.FindPreviousCaption;
            btFindNext.Text = StringConsts.FindNextCaption;
            laLookIn.Top = (IsReplace ? cbReplaceWith.Bottom : cbFindWhat.Bottom) + (int)(3 * scaleFactor);
            cbLookIn.Top = (IsReplace ? cbReplaceWith.Bottom : cbFindWhat.Bottom) + (int)(19 * scaleFactor);
            btMarkAll.Visible = !incremental;
            FileTypesLabel.Top = cbLookIn.Bottom + (int)(3 * scaleFactor);
            FileTypesComboBox.Top = cbLookIn.Bottom + (int)(19 * scaleFactor);

            gbFindOptions.Top = FileTypesComboBox.Bottom + (int)(14 * scaleFactor);
            laFindOptions.Top = gbFindOptions.Top;

            pnFindOptions.Top = gbFindOptions.Top + (int)(6 * scaleFactor);
            pnFindOptions.Left = laFindOptions.Right + (int)(2 * scaleFactor);
            pnFindOptions.Width = Width - laFindOptions.Right - (int)(20 * scaleFactor);

            btFindOptions.Top = gbFindOptions.Top - (int)(2 * scaleFactor);

            pnButtons.Height = IsReplace ? (int)(DefaultButtonsReplaceHeigth * scaleFactor) :
                (int)(DefaultButtonsHeigth * scaleFactor);

            tbSearch.ItemSize = tbSearch.Visible ? new Size((int)(scaleFactor * DefaultTabControlItemWidth), (int)(scaleFactor * DefaultTabControlItemHeight)) : Size.Empty;
            if (incremental)
                Width -= (int)(scaleFactor * DefaultDialogWidthOffset);
            Height = Height - ClientSize.Height +
            (incremental ? cbFindWhat.Bottom : (gbFindOptions.Visible ? gbFindOptions.Bottom + (int)(DefaultOptionsClosedHeight * scaleFactor) : btFindOptions.Bottom + (int)(DefaultOptionsClosedHeight * scaleFactor))) +
            +pnButtons.Height + (int)(2 * scaleFactor);
        }

        private void DlgSearch_Load(object sender, System.EventArgs e)
        {
            ActiveControl = cbFindWhat;
            LoadFromResource();
            ResizeControls();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            if (Search != null && Search.InIncrementalSearch)
                Search.FinishIncrementalSearch();
            Close();
        }

        private void tbSearch_Selected(object sender, TabControlEventArgs e)
        {
            IsReplace = tbSearch.SelectedIndex == 1;
        }

        private void tbUseReplace_Enter(object sender, EventArgs e)
        {
            if (cbFindWhat.CanFocus)
                cbFindWhat.Focus();
        }

        private void tbSearch_Enter(object sender, EventArgs e)
        {
            if (cbFindWhat.CanFocus)
                cbFindWhat.Focus();
        }

        private void DlgSearch_Shown(object sender, EventArgs e)
        {
            cbFindWhat.SelectAll();
        }

        private void cbFindWhat_TextChanged(object sender, EventArgs e)
        {
            if (updateCount > 0)
                return;

            if ((Search != null) && Search.InIncrementalSearch)
            {
                string str = string.IsNullOrEmpty(cbFindWhat.Text) ? string.Empty : cbFindWhat.Text[cbFindWhat.Text.Length - 1].ToString();
                Search.IncrementalSearch(str, cbFindWhat.Text.Length < lastText.Length);

                lastText = cbFindWhat.Text;
            }
            else
                HighlightAll();
        }

        private void chbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            HighlightAll();
        }

        private void chbMatchWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            HighlightAll();
        }
        #endregion

        private void FindAllButton_Click(object sender, EventArgs e)
        {
            if (SearchManager.SharedSearch != null)
            {
                SearchManager.SharedSearch.FindAll(Search, cbFindWhat.Text, Options & ~SearchOptions.BackwardSearch, GetExpression(), FileTypesComboBox.Text);
            }

            Close();
        }
    }
}
