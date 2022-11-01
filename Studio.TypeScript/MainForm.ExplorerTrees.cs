#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private TSProjectExplorer projectExplorer = new TSProjectExplorer();
        private bool projectExplorerTreeDoubleClicked;

        private CodeExplorer codeExplorer = new CodeExplorer();

        private static bool IsFileNode(TreeNode node)
        {
            var fileName = ProjectExplorer.GetFileNameFromNode(node);
            return !string.IsNullOrEmpty(fileName) && !IsReferenceNode(node);
        }

        private static bool IsReferenceNode(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (string.Compare(parent.Text, "references", true) == 0)
                    return true;
                parent = parent.Parent;
            }

            return false;
        }

        private void InitializeExplorerTrees()
        {
            projectExplorer.ExplorerTree = projectExplorerTreeView;

            codeExplorer.ExplorerTree = codeExplorerTreeView;
            codeExplorer.NavigateToNodeRequested += CodeExplorer_NavigateToNodeRequested;
        }

        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            if (projectExplorerTreeView.SelectedNode != null)
            {
                string fileName = GetFileNameFromNode(projectExplorerTreeView.SelectedNode);
                Project.MoveFileUp(fileName);
                UpdateProjectExplorer();
            }
        }

        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            if (projectExplorerTreeView.SelectedNode != null)
            {
                string fileName = GetFileNameFromNode(projectExplorerTreeView.SelectedNode);
                Project.MoveFileDown(fileName);
                UpdateProjectExplorer();
            }
        }

        private void RemoveFileMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            TreeNode node = GetNodeToRemove(projectExplorerTreeView.SelectedNode);

            IList<string> removedFiles = new List<string>();

            var fileName = GetFileNameFromNode(node);

            if (!string.IsNullOrEmpty(fileName))
            {
                removedFiles.Add(fileName);
            }

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                fileName = GetFileNameFromNode(node.Nodes[i]);

                if (!string.IsNullOrEmpty(fileName))
                {
                    removedFiles.Add(fileName);
                }
            }

            if (removedFiles.Count > 0)
            {
                if (!ConfirmSaveBeforeClosing(GetModifiedFiles(removedFiles), true))
                    return;

                foreach (string file in removedFiles)
                {
                    Project.RemoveFile(file);
#if USEFORMDESIGNER
                    RemoveDesigner(FindDesigner(file));
#endif
                    CloseFile(file);
                }

                CodeEditExtensions.UnregisterCode(Project.ProjectExtension, GetSourceFiles(removedFiles));

                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddFileMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            switch (Project.DefaultExtension)
            {
                case "ts":
                    openFileDialog.FilterIndex = 1;
                    break;
                case "js":
                    openFileDialog.FilterIndex = 2;
                    break;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                addedFiles.Add(fileName);

                if (addedFiles.Count > 0)
                {
                    foreach (var file in addedFiles)
                    {
                        Project.AddFile(file);
                        OpenFile(file);
                    }

                    CodeEditExtensions.RegisterCode(Project.ProjectExtension, GetSourceFiles(addedFiles));
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
        }

        private void CodeExplorer_NavigateToNodeRequested(object sender, NavigateToNodeRequestedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit == null)
                return;

            System.Drawing.Point p = new System.Drawing.Point(-1, -1);
            var parser = CodeEditExtensions.DefaultParser;
            if (!CodeUtils.GetPosition(e.Node, edit.FileName, out p, parser))
                return;

            edit.Position = p;
            edit.Focus();
        }

        private void UpdateProjectExplorer()
        {
            if (projectIsClosing)
                return;
            else if (Project.HasProject)
                projectExplorer.UpdateExplorer(Project);
            else
                projectExplorer.UpdateExplorer(null);
        }

        private void ProjectExplorerTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (projectExplorerTreeDoubleClicked && e.Action == TreeViewAction.Collapse)
            {
                projectExplorerTreeDoubleClicked = false;
                e.Cancel = true;
            }

            if (ProjectExplorer.IsFolderNode(e.Node))
            {
                e.Node.ImageIndex = GetFolderIcon(e.Node.Name, false);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void ProjectExplorerTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (projectExplorerTreeDoubleClicked && e.Action == TreeViewAction.Expand)
            {
                projectExplorerTreeDoubleClicked = false;
                e.Cancel = true;
            }

            if (ProjectExplorer.IsFolderNode(e.Node))
            {
                e.Node.ImageIndex = GetFolderIcon(e.Node.Name, true);
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void ProjectExplorerTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                projectExplorerTreeView.SelectedNode = projectExplorerTreeView.GetNodeAt(e.X, e.Y);

            projectExplorerTreeDoubleClicked = e.Clicks > 1;
        }

        private void ProjectExplorerTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var node = e.Node;
            if (node == null)
                return;

            var tag = node.Tag;
            var formNodeData = tag as FormNodeData;

            string codeFileName;
            if (formNodeData != null)
            {
#if USEFORMDESIGNER
                if (formNodeData.OpenMode == FormOpenMode.Design)
                {
                    OpenDesigner(formNodeData.FileName);
                    return;
                }
#endif
                codeFileName = formNodeData.FileName;
            }
            else
                codeFileName = tag as string;

            if (!IsReferenceNode(node) && !string.IsNullOrEmpty(codeFileName) && new FileInfo(codeFileName).Exists)
                OpenFile(codeFileName);
        }

        private int GetFolderIcon(string folderName, bool expand)
        {
            if (folderName.Equals("Properties"))
                return ProjectExplorerImageIndices.Properties;
            else
                return expand ? ProjectExplorerImageIndices.FolderOpened : ProjectExplorerImageIndices.FolderClosed;
        }

        private void ReferencesContextMenu_Opening(object sender, CancelEventArgs e)
        {
            addFileMenuItem.Enabled = Project.HasProject;
            addReferenceMenuItem.Enabled = Project.HasProject;
            moveUpMenuItem.Enabled = false;
            moveDownMenuItem.Enabled = false;
            removeFileMenuItem.Enabled = false;
            removeReferenceMenuItem.Enabled = false;

            if (Project != null && Project.HasProject && projectExplorerTreeView.SelectedNode != null)
            {
                bool isFile = IsFileNode(projectExplorerTreeView.SelectedNode);
                removeReferenceMenuItem.Enabled = IsReferenceNode(projectExplorerTreeView.SelectedNode);
                removeFileMenuItem.Enabled = isFile;
                moveUpMenuItem.Enabled = isFile && CanBeMoved(projectExplorerTreeView.SelectedNode, true);
                moveDownMenuItem.Enabled = isFile && CanBeMoved(projectExplorerTreeView.SelectedNode, false);
            }
        }

        private void AddReferenceMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            var dialog = new OpenFileDialog();
            dialog.Filter = ".Assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            var projectDirectory = Path.GetDirectoryName(Project.ProjectFileName);
            dialog.InitialDirectory = projectDirectory;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            if (Project.AddHostAssembly(dialog.FileName))
            {
                Project.UpdateHostConfiguration(
                    CodeEditExtensions.DefaultProject,
                    scriptRun.ScriptHost.HostItemsConfiguration);

                UpdateSyntax();
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveReferenceMenuItem_Click(object sender, EventArgs e)
        {
            if (!Project.HasProject)
                return;

            if (IsReferenceNode(projectExplorerTreeView.SelectedNode))
            {
                string name = projectExplorerTreeView.SelectedNode.Tag as string;
                if (Project.RemoveHostAssembly(name))
                {
                    Project.UpdateHostConfiguration(
                        CodeEditExtensions.DefaultProject,
                        scriptRun.ScriptHost.HostItemsConfiguration);

                    UpdateSyntax();
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
        }

        private static class ProjectExplorerImageIndices
        {
            public const int Properties = 2;
            public const int FolderClosed = 7;
            public const int FolderOpened = 8;
        }
    }
}