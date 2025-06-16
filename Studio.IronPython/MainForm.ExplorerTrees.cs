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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Editor.Common;
using Alternet.Editor.IronPython;
using Alternet.FormDesigner.WinForms;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IronPythonProjectExplorer projectExplorer = new IronPythonProjectExplorer();

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

        private void RemoveFileMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !project.HasProject)
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
                    project.RemoveFile(file);
                    RemoveDesigner(FindDesigner(file));
                    CloseFile(file);
                }

                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddFileMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !project.HasProject)
                return;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                if (FormFilesUtility.IsSupportedLanguage(fileName))
                {
                    string designerFileName;
                    string resourceFileName;
                    FormFilesUtility.TryDetectFormSourceFiles(fileName, out designerFileName, out resourceFileName);
                    addedFiles.Add(fileName);

                    if (!string.IsNullOrEmpty(designerFileName) && File.Exists(designerFileName))
                        addedFiles.Add(designerFileName);

                    if (!string.IsNullOrEmpty(resourceFileName) && File.Exists(resourceFileName))
                        addedFiles.Add(resourceFileName);
                }
                else
                {
                    addedFiles.Add(fileName);
                }

                if (addedFiles.Count > 0)
                {
                    foreach (var file in addedFiles)
                    {
                        project.AddFile(file);
                        OpenFile(file);
                    }

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

            Point p = new Point(-1, -1);
            Point endP = new Point(-1, -1);
            string name = string.Empty;
            if (!CodeUtils.GetPosition(e.Node, out p))
                return;

            edit.Position = p;
            edit.Focus();
        }

        private void UpdateProjectExplorer()
        {
            if (projectIsClosing)
                return;

            if (Project.HasProject)
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
                if (formNodeData.OpenMode == FormOpenMode.Design)
                {
                    OpenDesigner(formNodeData.FileName);
                    return;
                }

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
            var project = GetProject(projectExplorerTreeView.SelectedNode);

            addFileMenuItem.Enabled = project != null && project.HasProject;
            addReferenceMenuItem.Enabled = project != null && project.HasProject;

            removeFileMenuItem.Enabled = false;
            removeReferenceMenuItem.Enabled = false;

            setDefaultProjectMenuItem.Enabled = project != null && project.HasProject;

            if (project != null && project.HasProject && projectExplorerTreeView.SelectedNode != null)
            {
                removeReferenceMenuItem.Enabled = IsReferenceNode(projectExplorerTreeView.SelectedNode);
                removeFileMenuItem.Enabled = IsFileNode(projectExplorerTreeView.SelectedNode);
            }
        }

        private void AddReferenceMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !project.HasProject)
                return;

            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = Path.GetDirectoryName(project.ProjectFileName);

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var reference = dialog.FileName;
            if (project.AddReference(Path.GetFileName(reference), reference))
            {
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveReferenceMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !project.HasProject)
                return;

            if (IsReferenceNode(projectExplorerTreeView.SelectedNode))
            {
                var reference = projectExplorerTreeView.SelectedNode.Tag as IronPythonProject.AssemblyReference;
                if (project.RemoveReference(reference.FullName))
                {
                    var references = project.References.Select(x => x.FullName).ToArray();
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
        }

        private void SetDefaultProjectMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project != null)
            {
                Project = project;
                UpdateScriptProject(project);
                UpdateProjectExplorer();
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