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
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace LspMultipleFiles
{
    internal class FilesControl1 : TreeView
    {
        private string rootDirectory;

        public FilesControl1()
        {
            //ShowRootLines = true;
        }

        public event EventHandler<OpenFileRequestEventArgs> OpenFileRequested;

        public string[] SearchPatterns { get; set; } = new[] { "*.*" };

        public string RootDirectory
        {
            get => rootDirectory;
            set
            {
                if (rootDirectory == value)
                    return;

                rootDirectory = value;

                Reload();
            }
        }

        public void Reload()
        {
            Items.Clear();

            if (rootDirectory == null)
                return;
            if (!Directory.Exists(rootDirectory))
                return;

            TreeViewItem workspaceNode = new TreeViewItem();
            workspaceNode.Name = "Workspace";
            workspaceNode.Header = "Workspace";
            Items.Add(workspaceNode);
            //var workspaceNode = Items.Add("Workspace");

            foreach (var file in SearchPatterns.SelectMany(x => Directory.GetFiles(rootDirectory, x, SearchOption.AllDirectories)))
            {
                TreeViewItem node = new TreeViewItem() { Name = Path.GetFileName(file), Header = Path.GetFileName(file), Tag = file };
                workspaceNode.Items.Add(node);
            }

            workspaceNode.IsExpanded = true;
        }

        public IEnumerable<string> GetFiles() => (Items[0] as TreeViewItem).Items.Cast<TreeViewItem>().Select(x => (string)x.Tag);

        //protected override void OnNodeMouseDoubleClick()//TreeNodeMouseClickEventArgs e)
        //{
        //    //base.MouseDoubleClick
        //    var fileName = e.Node.Tag as string;
        //    if (fileName == null)
        //        return;

        //    OpenFileRequested?.Invoke(this, new OpenFileRequestEventArgs(fileName));
        //}

        public class OpenFileRequestEventArgs : EventArgs
        {
            public OpenFileRequestEventArgs(string fileName) => FileName = fileName;

            public string FileName { get; }
        }
    }
}