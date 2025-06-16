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
using System.Windows;
using System.Windows.Forms.Integration;
using Alternet.Editor.Wpf;

namespace SearchReplace
{
    public class SearchInFilesDialog
    {
        private static OwnerWindow owner;
        private IDlgSeparateWindow searchDialogWindow = null;

        public static OwnerWindow Owner
        {
            get
            {
                return owner;
            }

            set
            {
                if (owner != value)
                    owner = value;
            }
        }

        public virtual IDlgSeparateWindow SearchDlg
        {
            get
            {
                return searchDialogWindow;
            }
        }

        public virtual bool Execute(ISearch search, bool isModal, bool isReplace, OwnerWindow owner)
        {
            SearchManager.SharedSearch.SearchChanged -= SharedSearch_SearchChanged;
            SearchManager.SharedSearch.SearchChanged += SharedSearch_SearchChanged;
            string text = string.Empty;
            bool selection = search.CanSearchSelection(out text);
            SearchOptions options = SearchOptions.CurrentProject;

            if (searchDialogWindow == null)
            {
                searchDialogWindow = CreateSearchDialogWindow();
                searchDialogWindow.Closed += new EventHandler(DoClose);
            }

            if (text == string.Empty)
            {
                text = search.InIncrementalSearch ? string.Empty : search.GetTextToSearchAtCursor().Trim();

                if (text != string.Empty)
                    searchDialogWindow.UpdateFindText(text);
            }

            searchDialogWindow.Search = search;
            searchDialogWindow.IsReplace = isReplace;
            searchDialogWindow.SelectionEnabled = selection;

            searchDialogWindow.Init();
            searchDialogWindow.Options = options;

            var effectiveOwner = owner == null ? Owner : owner;
            if (effectiveOwner == null)
            {
                searchDialogWindow.Topmost = true;
                OwnerWindow.ClearOwnerFor(searchDialogWindow as Window);
            }
            else
            {
                searchDialogWindow.Topmost = false;
                effectiveOwner.SetAsOwnerFor(searchDialogWindow as Window);
            }

            search.TextChanged += Search_TextChanged;

            ElementHost.EnableModelessKeyboardInterop(searchDialogWindow as Window);
            searchDialogWindow.Show();
            return false;
        }

        protected virtual IDlgSeparateWindow CreateSearchDialogWindow()
        {
            var window = new SearchInFilesDialogWindow();
            return window;
        }

        private void SharedSearch_SearchChanged(object sender, SearchChangedEventArgs e)
        {
            e.OldSearch?.UnhighlightAll();
            var newSearch = e.NewSearch as ISearch;
            if (newSearch != null)
            {
                var dlg = this.SearchDlg;
                if (dlg != null)
                {
                    dlg.UpdateSearch(newSearch, false);
                    dlg.HighlightAll();
                }
            }
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            if (searchDialogWindow != null)
                searchDialogWindow.HighlightAll();
        }

        private void DoClose(object sender, EventArgs e)
        {
            if (searchDialogWindow != null)
            {
                if (searchDialogWindow != null && searchDialogWindow.Search != null)
                {
                    searchDialogWindow.Search.TextChanged -= Search_TextChanged;
                    searchDialogWindow.Search.UnhighlightAll();
                }
            }

            searchDialogWindow = null;
        }
    }
}
