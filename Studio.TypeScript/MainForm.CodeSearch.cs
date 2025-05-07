#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System.IO;

using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Editor.TextSource;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private bool findInProject = false;

        private void InitializeCodeSearch()
        {
            var search = SearchManager.SharedSearch;
            search.InitSearch += Search_InitSearch;
            search.GetSearch += Search_GetSearch;
            search.TextFound += Search_TextFound;

            findResultsControl.FindResultClick += FindResultsControl_FindResultClick;
        }

        private void FinalizeCodeSearch()
        {
            var search = SearchManager.SharedSearch;
            search.InitSearch -= Search_InitSearch;
            search.GetSearch -= Search_GetSearch;
            search.TextFound -= Search_TextFound;
        }

        private void UpdateSearch()
        {
            var edit = ActiveSyntaxEdit;
            var search = edit as ISearch;
            if (search != null)
                search.SearchGlobal = true;
        }

        private void UpdateBookmarks()
        {
            var edit = ActiveSyntaxEdit as SyntaxEdit;
            if (edit != null)
            {
                edit.Source.BookMarks.Shared = true;
                BookMarkManager.Register(edit.Source);
            }
        }

        private void Search_TextFound(object sender, TextFoundEventArgs e)
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

        private void Search_GetSearch(object sender, GetSearchEventArgs e)
        {
            if (findInProject && Project.HasProject)
            {
                foreach (var file in Project.Files)
                {
                    if (string.Compare(file, e.FileName, true) == 0)
                    {
                        var edit = FindFile(e.FileName);
                        if (edit != null)
                        {
                            SetActiveEdit(edit);
                            UpdateSearch();
                            e.Search = (ISearch)edit;
                        }

                        break;
                    }
                }
            }
            else
            {
                foreach (var page in editors.Keys)
                {
                    var edit = GetEditor(page);
                    if (edit != null && string.Compare(edit.FileName, e.FileName, true) == 0)
                    {
                        editorsTabControl.SelectedTab = page;
                        UpdateSearch();
                        e.Search = (ISearch)edit;
                        break;
                    }
                }
            }
        }

        private void Search_InitSearch(object sender, InitSearchEventArgs e)
        {
            e.Search = ActiveSyntaxEdit as ISearch;
            findInProject = (e.Options & SearchOptions.CurrentProject) != 0;
            if (findInProject && Project.HasProject)
            {
                foreach (var file in Project.Files)
                {
                    if (File.Exists(file))
                        e.SearchList.Add(file);
                }
            }
            else
            {
                foreach (var edit in editors.Values)
                {
                    var search = edit as ISearch;
                    if (search != null)
                        search.SearchGlobal = true;
                    e.SearchList.Add(edit.FileName);
                }
            }
        }

        private void FindResultsControl_FindResultClick(object sender, FindResultClickEventArgs e)
        {
            NavigateToRange(e.FileRange);
        }

        private void DoActivate(object sender, ActivateEventArgs e)
        {
            var edit = OpenFile(e.FileName) as SyntaxEdit;
            if (edit != null)
            {
                e.Source = edit.Source;
            }
        }

        private void SharedBookMarks_BookMarkRemoved(object sender, BookMarkEventArgs e)
        {
            UpdateBookmarkButtons();
        }

        private void SharedBookMarks_BookMarkAdded(object sender, BookMarkEventArgs e)
        {
            UpdateBookmarkButtons();
        }
    }
}