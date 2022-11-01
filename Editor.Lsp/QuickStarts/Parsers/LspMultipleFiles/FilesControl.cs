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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LspMultipleFiles
{
    internal class FilesControl : TreeView
    {
        private string rootDirectory;

        public FilesControl()
        {
            ShowRootLines = true;
        }

        public event EventHandler<OpenFileRequestEventArgs> OpenFileRequested;

        public string[] SearchPatterns { get; set; } = new[] { "*.*" };

        public string RootDirectory
        {
            get
            {
                return rootDirectory;
            }

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
            Nodes.Clear();

            if (rootDirectory == null)
                return;
            if (!Directory.Exists(rootDirectory))
                return;

            var workspaceNode = Nodes.Add("Workspace");

            foreach (var file in SearchPatterns.SelectMany(x => Directory.GetFiles(rootDirectory, x, SearchOption.AllDirectories)))
                workspaceNode.Nodes.Add(new TreeNode(Path.GetFileName(file)) { Tag = file });

            workspaceNode.Expand();
        }

        public IEnumerable<string> GetFiles() => Nodes[0].Nodes.Cast<TreeNode>().Select(x => (string)x.Tag);

        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
        {
            var fileName = e.Node.Tag as string;
            if (fileName == null)
                return;

            OpenFileRequested?.Invoke(this, new OpenFileRequestEventArgs(fileName));
        }

        public class OpenFileRequestEventArgs : EventArgs
        {
            public OpenFileRequestEventArgs(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }
    }
}