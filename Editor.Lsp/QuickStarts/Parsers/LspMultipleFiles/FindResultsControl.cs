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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;

namespace LspMultipleFiles
{
    public partial class FindResultsControl : UserControl
    {
        private IRangeList results;

        public FindResultsControl()
        {
            InitializeComponent();

            ScaleControls();
        }

        public event EventHandler<FileRangeEventArgs> NavigateToResultRequested;

        public IRangeList FindResults
        {
            get
            {
                return results;
            }

            set
            {
                results = value;
                findResultsListView.BeginUpdate();
                try
                {
                    findResultsListView.Items.Clear();
                    if (results != null)
                    {
                        foreach (var range in results.OfType<IFileRange>())
                            AddFindResult(range);

                        fileColumnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        codeColumnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }
                finally
                {
                    findResultsListView.EndUpdate();
                }
            }
        }

        private IFileRange SelectedItem => findResultsListView.SelectedItems.Cast<ListViewItem>().SingleOrDefault()?.Tag as IFileRange;

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            foreach (var column in findResultsListView.Columns.Cast<ColumnHeader>())
                column.Width = DisplayScaling.AutoScale(column.Width);
        }

        private void AddFindResult(IFileRange range)
        {
            var items = new[]
            {
                Path.GetFileName(range.FileName),
                (range.StartPoint.Y + 1).ToString(),
                range.SourceText.Trim(),
            };

            findResultsListView.Items.Add(new ListViewItem(items) { Tag = range });
        }

        private void FindResultsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RequestNavigateToResult();
        }

        private void RequestNavigateToResult()
        {
            var selectedItem = SelectedItem;
            if (selectedItem == null)
                return;

            NavigateToResultRequested?.Invoke(this, new FileRangeEventArgs(selectedItem));
        }

        private void FindResultsListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RequestNavigateToResult();
            }
        }

        public class FileRangeEventArgs : EventArgs
        {
            public FileRangeEventArgs(IFileRange range)
            {
                Range = range;
            }

            public IFileRange Range { get; }
        }
    }
}