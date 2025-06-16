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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Editor;

using Alternet.Editor.Dialogs;
using Alternet.Editor.TextSource;

namespace SearchReplace
{
    public class SearchInFilesDialog
    {
        private IFormDlgSearch searchDlg = null;

        public virtual IDlgSearch SearchDlg
        {
            get
            {
                return searchDlg;
            }
        }

        public DialogResult Execute(ISearch search, bool isModal, bool isReplace, IWin32Window owner)
        {
            SearchManager.SharedSearch.SearchChanged -= SharedSearch_SearchChanged;
            SearchManager.SharedSearch.SearchChanged += SharedSearch_SearchChanged;
            string text = string.Empty;
            bool selection = search.CanSearchSelection(out text);
            SearchOptions options = SearchOptions.CurrentProject;

            if (searchDlg == null)
            {
                searchDlg = CreateSearchDlg();
                searchDlg.Closed += new EventHandler(DoClose);
                searchDlg.Disposed += new EventHandler(DoDisposed);
            }

            if (text == string.Empty)
            {
                text = search.InIncrementalSearch ? string.Empty : search.GetTextToSearchAtCursor().Trim();

                if (text != string.Empty)
                    searchDlg.UpdateFindText(text);
            }

            searchDlg.Search = search;
            searchDlg.IsReplace = isReplace;
            searchDlg.SelectionEnabled = selection;

            searchDlg.Init();
            searchDlg.Options = options;

            if (isModal)
            {
                DialogResult res = (owner != null) ? searchDlg.ShowDialog(owner) : searchDlg.ShowDialog();
                return res;
            }
            else
            {
                searchDlg.TopMost = true;
                searchDlg.Owner = null;
                searchDlg.Show();
                return DialogResult.None;
            }
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

        private void DoClose(object sender, EventArgs e)
        {
            if (searchDlg != null)
            {
                if (searchDlg != null && searchDlg.Search != null)
                {
                    searchDlg.Search.UnhighlightAll();
                }

                searchDlg.Dispose();
            }

            searchDlg = null;
        }

        private void DoDisposed(object sender, EventArgs e)
        {
            searchDlg = null;
        }

        private IFormDlgSearch CreateSearchDlg()
        {
            return new DlgSearchInFiles();
        }
    }
}
