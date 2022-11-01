#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.Wpf
{
    /// <summary>
    /// Interaction logic for FilesUserControl.xaml
    /// </summary>
    public partial class FilesUserControl : UserControl
    {
        private const string XamlFileExtension = ".xaml";

        private string formsFolderPath;

        public FilesUserControl()
        {
            InitializeComponent();
        }

        public Action<string, string, FormOpenMode> FileOpenRequestedFunc { get; private set; }

        public void Initialize(string folderPath, Action<string, string, FormOpenMode> fileOpenRequestedFunc)
        {
            formsFolderPath = folderPath;
            FileOpenRequestedFunc = fileOpenRequestedFunc;
        }

        public string[] RefreshFiles()
        {
            var forms = GetFormsInFolder(formsFolderPath);

            filesTreeView.Items.Clear();

            foreach (var form in forms)
            {
                var formNode = new FormTreeViewItem(
                    form.XamlFileName,
                    form.XamlFileName,
                    FormOpenMode.Design,
                    form.DesignedClassFullName);

                filesTreeView.Items.Add(formNode);

                Action<string, FormOpenMode> addNode = (string fileName, FormOpenMode mode) =>
                {
                    var node = new FormTreeViewItem(
                        form.XamlFileName,
                        fileName,
                        mode,
                        Path.GetFileName(fileName));

                    formNode.Items.Add(node);
                };

                addNode(form.XamlFileName, FormOpenMode.Xaml);
                addNode(form.UserCodeFileName, FormOpenMode.Code);

                formNode.IsExpanded = true;
            }

            return forms.Select(x => x.XamlFileName).ToArray();
        }

        private void FilesTreeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = TryGetClickedItem(filesTreeView, e);

            if (clickedItem == null)
                return;

            e.Handled = true; // to cancel expanded/collapsed toggle
            var item = (FormTreeViewItem)clickedItem;

            if (FileOpenRequestedFunc != null)
                FileOpenRequestedFunc(item.FormId, item.FileName, item.OpenMode);
        }

        private FormDesignerDataSource[] GetFormsInFolder(string folder)
        {
            var xamlFiles = Directory.GetFiles(folder, "*" + XamlFileExtension, SearchOption.AllDirectories);

            var result = new List<FormDesignerDataSource>();

            foreach (var xamlFile in xamlFiles)
            {
                FormDesignerDataSource dataSource;
                if (FormFilesUtility.TryFindUserCodeFile(xamlFile, out dataSource) != null)
                    result.Add(dataSource);
            }

            return result.ToArray();
        }

        private TreeViewItem TryGetClickedItem(TreeView treeView, MouseButtonEventArgs e)
        {
            var hit = e.OriginalSource as DependencyObject;
            while (hit != null && !(hit is TreeViewItem))
                hit = VisualTreeHelper.GetParent(hit);

            return hit as TreeViewItem;
        }

        private class FormTreeViewItem : TreeViewItem
        {
            public FormTreeViewItem(string formId, string fileName, FormOpenMode openMode, string displayText)
            {
                FormId = formId;
                FileName = fileName;
                OpenMode = openMode;
                Header = displayText;
            }

            public string FileName { get; private set; }

            public string FormId { get; private set; }

            public FormOpenMode OpenMode { get; private set; }
        }
    }
}