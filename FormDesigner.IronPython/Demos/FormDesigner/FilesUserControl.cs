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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.FormDesigner.WinForms;

namespace Alternet.FormDesigner.Demo
{
    public partial class FilesUserControl : UserControl
    {
        public const string ResourceFileExtension = ".resx";
        private bool doubleClick;
        private Action<string, string, FormOpenMode> fileOpenRequestedFunc;
        private string formsFolderName = ".";

        public FilesUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler NewFormButtonClick;

        public void Initialize(string formsFolderName, Action<string, string, FormOpenMode> fileOpenRequestedFunc)
        {
            this.formsFolderName = formsFolderName;
            this.fileOpenRequestedFunc = fileOpenRequestedFunc;
        }

        public string[] RefreshFiles()
        {
            var forms = GetFormsInFolder(formsFolderName);

            filesTreeView.Nodes.Clear();

            foreach (var form in forms)
            {
                var formNode = new FormTreeNode(
                    form.UserCodeFileName,
                    form.UserCodeFileName,
                    FormOpenMode.Design,
                    form.DesignedClassFullName);

                filesTreeView.Nodes.Add(formNode);

                Action<string> addNode = (string fileName) =>
                {
                    var node = new FormTreeNode(
                        form.UserCodeFileName,
                        fileName,
                        FormOpenMode.Code,
                        Path.GetFileName(fileName));

                    formNode.Nodes.Add(node);
                };

                addNode(form.UserCodeFileName);
                addNode(form.DesignerFileName);

                AddResourceNodes(form, addNode);

                formNode.Expand();
            }

            return forms.Select(x => x.UserCodeFileName).ToArray();
        }

        private static void AddResourceNodes(FormDesignerDataSource form, Action<string> addNode)
        {
            addNode(form.DefaultResourceFileName);

            var addedFiles = new HashSet<string>();

            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                var fileName = form.GetResourceFileName(culture);
                if (File.Exists(fileName) && !addedFiles.Contains(fileName))
                {
                    addedFiles.Add(fileName);
                    addNode(fileName);
                }
            }
        }

        private void FilesTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (doubleClick && e.Action == TreeViewAction.Collapse)
            {
                doubleClick = false;
                e.Cancel = true;
            }
        }

        private void FilesTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (doubleClick && e.Action == TreeViewAction.Expand)
            {
                doubleClick = false;
                e.Cancel = true;
            }
        }

        private void FilesTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
                doubleClick = true;
            else
                doubleClick = false;
        }

        private void FilesTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = (FormTreeNode)e.Node;

            if (fileOpenRequestedFunc != null)
                fileOpenRequestedFunc(node.FormId, node.FileName, node.OpenMode);
        }

        private FormDesignerDataSource[] GetFormsInFolder(string folder)
        {
            var designerFiles = Directory.GetFiles(folder, "*_Designer.*", SearchOption.AllDirectories);

            var result = new List<FormDesignerDataSource>();

            foreach (var designerFile in designerFiles)
            {
                var extension = Path.GetExtension(designerFile);

                var userCodeFile = designerFile.Replace("_Designer" + extension, extension);
                var dataSource = new FormDesignerDataSource(userCodeFile, designerFileNameSuffix: "_Designer");

                if (File.Exists(dataSource.UserCodeFileName))
                    result.Add(dataSource);
            }

            return result.ToArray();
        }

        private void NewFormToolStripButton_Click(object sender, EventArgs e)
        {
            if (NewFormButtonClick != null)
                NewFormButtonClick(sender, e);
        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshFiles();
        }

        private class FormTreeNode : TreeNode
        {
            public FormTreeNode(string formId, string fileName, FormOpenMode openMode, string displayText)
            {
                FormId = formId;
                FileName = fileName;
                OpenMode = openMode;
                Text = displayText;
            }

            public string FileName { get; private set; }

            public string FormId { get; private set; }

            public FormOpenMode OpenMode { get; private set; }
        }
    }
}