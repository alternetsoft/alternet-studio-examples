#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Alternet.Common;

namespace LspMultipleFiles
{
    /// <summary>
    /// Interaction logic for FindResults.xaml
    /// </summary>
    public partial class FindResultsControl : UserControl
    {
        private ObservableCollection<ListViewItemData> source = new ObservableCollection<ListViewItemData>();
        private IRangeList results;

        public FindResultsControl()
        {
            InitializeComponent();
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
                AddFindResults(results);
            }
        }

        private IFileRange SelectedItem => findResultsListView.SelectedItems.Cast<ListViewItemData>().SingleOrDefault()?.Tag as IFileRange;

        public void AddFindResults(IRangeList references)
        {
            source.Clear();

            if (references == null)
                return;

            foreach (var range in references)
            {
                var fileRange = range as IFileRange;
                if (fileRange != null)
                    AddFindResult(fileRange);
            }

            findResultsListView.ItemsSource = source;
        }

        public void AddFindResult(IFileRange range)
        {
            if (source != null)
            {
                ListViewItemData data = new ListViewItemData();
                data.File = System.IO.Path.GetFileName(range.FileName);
                data.Line = (range.StartPoint.Y + 1).ToString();
                data.Code = range.SourceText.Trim();
                data.Tag = range;
                source.Add(data);
            }
        }

        private void FindResultsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        public class FileRangeEventArgs : EventArgs
        {
            public FileRangeEventArgs(IFileRange range)
            {
                Range = range;
            }

            public IFileRange Range { get; }
        }

        private class ListViewItemData
        {
            public string File { get; set; }

            public string Line { get; set; }

            public object Code { get; set; }

            public object Tag { get; set; }
        }
    }
}
