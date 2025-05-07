#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.FormDesigner.WinForms;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private const string NewFolder = "NewFolder";
        private const string NewSolutionFolder = "NewSolutionFolder";

        private DotNetProjectExplorer projectExplorer = new DotNetProjectExplorer();

        private SolutionExplorer solutionExplorer = new SolutionExplorer();

        private bool projectExplorerTreeDoubleClicked;

        private CodeExplorer codeExplorer = new CodeExplorer();

        private bool isProjectFolder = true;

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

        private static bool IsProjectNode(TreeNode node)
        {
            return (node.Tag != null) && (node.Tag is DotNetProject);
        }

        private static bool IsProjectFolderNode(TreeNode node)
        {
            return ProjectExplorer.IsProjectFolder(node);
        }

        private void InitializeFileProperties()
        {
            propertyGrid.HelpVisible = false;
            propertyGrid.ToolbarVisible = false;
        }

        private void InitializeExplorerTrees()
        {
            projectExplorer.ExplorerTree = projectExplorerTreeView;
            solutionExplorer.ExplorerTree = projectExplorerTreeView;

            codeExplorer.ExplorerTree = codeExplorerTreeView;
            codeExplorer.NavigateToNodeRequested += CodeExplorer_NavigateToNodeRequested;
        }

        private void UpdateFileProperty()
        {
            propertyGrid.SelectedObject = null;
            labelProperties.Text = "Properties";
            if (projectExplorerTreeView.SelectedNode == null)
            {
                return;
            }

            string fileName = ProjectExplorer.GetFileNameFromNode(projectExplorerTreeView.SelectedNode);

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string file = Path.GetFileName(fileName);
            labelProperties.Text = string.Compare(file, Alternet.Common.StringConsts.FolderNode) != 0 ? string.Format("Properties - {0}", file) : "Properties";
            var fileAttr = Project.GetFileAttribute(fileName);

            if (fileAttr == null)
                return;

            fileAttr.Changed -= FileAttr_Changed;
            fileAttr.Changed += FileAttr_Changed;
            propertyGrid.SelectedObject = fileAttr;
        }

        private void FileAttr_Changed(object sender, EventArgs e)
        {
            Project.IsModified = true;
        }

        private void FilePropertiesMenuItem_Click(object sender, System.EventArgs e)
        {
            bool propVisible = filePropertiesMenuItem.Checked;
            splitterProperties.Visible = propVisible;
            panelProperties.Visible = propVisible;
        }

        private void AddFileMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !(project.HasProject || project.IsFolder))
                return;

            switch (project.DefaultExtension)
            {
                case "cs":
                    openFileDialog.FilterIndex = 1;
                    break;

                case "vb":
                    openFileDialog.FilterIndex = 2;
                    break;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                if (FormFilesUtility.IsSupportedLanguage(fileName) && !project.IsFolder)
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
                        if (project.IsFolder)
                        {
                            project.AddSolutionFile(file, Path.GetDirectoryName(solution.SolutionFileName) + "/");
                            solution.IsModified = true;
                        }
                        else
                            project.AddFile(file);
                        OpenFile(file);
                    }

                    CodeEditExtensions.RegisterCode(project.ProjectExtension, GetSourceFiles(addedFiles), project.ProjectName);
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
        }

        private string FindUniqueSolutionFolderName(IList<DotNetProject> projects)
        {
            int count = 0;
            string newName = NewSolutionFolder;
            string result = newName;
            while (!IsSolutionFolderNameUnique(projects, result))
            {
                count++;
                result = newName + count.ToString();
            }

            return result;
        }

        private bool IsSolutionFolderNameUnique(IList<DotNetProject> projects, string name)
        {
            if (projects == null)
                return true;

            foreach (var project in projects)
            {
                if (project.IsFolder && string.Compare(project.ProjectName, name, true) == 0)
                    return false;
            }

            return true;
        }

        private string FindUniqueFolderName(string path)
        {
            int count = 0;
            string newName = NewFolder;
            string result = newName;
            while (!IsFolderNameUnique(path, result))
            {
                count++;
                result = newName + count.ToString();
            }

            return result;
        }

        private bool IsFolderNameUnique(string path, string name)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    if (string.Compare(Path.GetFileName(dir), name, true) == 0)
                        return false;
                }
            }

            return true;
        }

        private void AddProjectFolderMenuItem_Click(object sender, EventArgs e)
        {
            var node = projectExplorerTreeView.SelectedNode;
            var project = GetProject(node);
            if (project != null)
            {
                var newNode = new TreeNode();
                newNode.ImageIndex = GetFolderIcon(string.Empty, false);
                newNode.SelectedImageIndex = GetFolderIcon(string.Empty, false);
                string newName = FindUniqueFolderName(Path.GetDirectoryName(project.ProjectFileName));
                newNode.Text = newName;
                newNode.Tag = StringConsts.FolderNode;
                node.Nodes.Add(newNode);

                project.BeginUpdate();
                project.AddProjectFolder(newNode.Text);
                projectExplorerTreeView.LabelEdit = true;
                isProjectFolder = true;

                newNode.BeginEdit();
            }
        }

        private void CodeExplorer_NavigateToNodeRequested(object sender, NavigateToNodeRequestedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit == null)
                return;

            System.Drawing.Point p;
            if (!CodeUtils.GetPosition(e.Node, edit.Document(), out p))
                return;

            edit.Position = p;
            edit.Focus();
        }

        private void UpdateProjectExplorer()
        {
            if (projectIsClosing)
                return;

            if (solution.HasProjects)
                solutionExplorer.UpdateExplorer(solution);
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

        private void ProjectExplorerTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            UpdateFileProperty();
        }

        private void ProjectExplorerTreeView_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            var project = GetProject(e.Node);
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (isProjectFolder)
                    {
                        string errorMessage = string.Empty;
                        if ((bool)!project?.ReplaceProjectFolder(e.Node.Text, e.Label, ref errorMessage) && !string.IsNullOrEmpty(errorMessage))
                        {
                            e.CancelEdit = true;
                            MessageBox.Show(errorMessage, "AlterNET Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Node.BeginEdit();
                            return;
                        }
                    }
                    else
                    {
                        project.ProjectName = e.Label;
                        solution.IsModified = true;
                    }

                    UpdateCodeNavigation();
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show(
                        "Invalid tree node label.\nThe label cannot be blank",
                        "Node Label Edit");
                    e.Node.BeginEdit();
                    return;
                }
            }
            else
            {
                e.CancelEdit = true;
                e.Node?.Parent?.Nodes.Remove(e.Node);
            }

            BeginInvoke((Action)(() =>
                    {
                        if (isProjectFolder)
                        {
                            if (project != null && project.IsModified)
                                project.EndUpdate();
                        }
                        else
                        {
                            if (solution.IsModified)
                                UpdateProjectExplorer();
                        }
                    }));
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
            var sol = GetSolution(projectExplorerTreeView.SelectedNode);
            var selectedProject = GetProject(projectExplorerTreeView.SelectedNode, false);

            addProjectMenuItem.Enabled = solution != null && solution.HasProjects;

            addSolutionFolderMenuItem.Visible = sol != null;
            renameSolutionFolderMenuItem.Visible = (selectedProject != null && selectedProject.IsFolder) || IsProjectFolderNode(projectExplorerTreeView.SelectedNode);

            addFileMenuItem.Visible = project != null && (project.HasProject || project.IsFolder);
            addProjectFolderMenuItem.Visible = project != null && project.HasProject && (projectExplorerTreeView.SelectedNode == null || !IsFileNode(projectExplorerTreeView.SelectedNode));
            addReferenceMenuItem.Visible = project != null && project.HasProject;

            filePropertiesMenuItem.Enabled = false;

            setDefaultProjectMenuItem.Visible = !solution.IsEmpty && project != null && project.HasProject && project != Project;

            removeProjectItemMenuItem.Visible = project != null && projectExplorerTreeView.SelectedNode != null && IsValidNodeToRemove(project, projectExplorerTreeView.SelectedNode);

            if (project != null && (project.HasProject || project.IsFolder) && projectExplorerTreeView.SelectedNode != null)
            {
                filePropertiesMenuItem.Enabled = IsFileNode(projectExplorerTreeView.SelectedNode);
            }
        }

        private bool IsValidNodeToRemove(DotNetProject project, TreeNode node)
        {
            return IsFileNode(node) || IsReferenceNode(node) || IsProjectNode(node) || IsProjectFolderNode(node) || (project.IsFolder && solution != null);
        }

        private void AddProjectMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = 4;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var project = new DotNetProject();
                project.Load(openFileDialog.FileName);
                solution.AddProject(project, GetSolutionFolder(projectExplorerTreeView.SelectedNode));
                OpenProject(project);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddSolutionFolderMenuItem_Click(object sender, EventArgs e)
        {
            var node = projectExplorerTreeView.SelectedNode;
            var newNode = new TreeNode();
            if (solution != null)
            {
                projectExplorerTreeView.BeginUpdate();
                try
                {
                    newNode.ImageIndex = GetFolderIcon(string.Empty, false);
                    newNode.SelectedImageIndex = GetFolderIcon(string.Empty, false);

                    string newName = FindUniqueSolutionFolderName(solution?.Projects);
                    newNode.Text = newName;
                    node.Nodes.Add(newNode);
                    var project = new DotNetProject();

                    project.IsFolder = true;
                    project.ProjectGuid = "{" + Guid.NewGuid().ToString() + "}";
                    project.ProjectName = newName;
                    project.IsFolder = true;
                    newNode.Tag = project;

                    solution.AddProject(project, GetSolutionFolder(projectExplorerTreeView.SelectedNode));
                    projectExplorerTreeView.SelectedNode = newNode;
                }
                finally
                {
                    projectExplorerTreeView.Sort();
                    projectExplorerTreeView.EndUpdate();
                }

                projectExplorerTreeView.LabelEdit = true;
                isProjectFolder = false;

                newNode.BeginEdit();
            }
        }

        private void RenameSolutionFolderMenuItem_Click(object sender, EventArgs e)
        {
            var node = projectExplorerTreeView.SelectedNode;
            if (node != null)
            {
                projectExplorerTreeView.LabelEdit = true;
                isProjectFolder = IsProjectFolderNode(node);
                if (isProjectFolder)
                {
                    var project = GetProject(node);
                    project?.BeginUpdate();
                }

                node.BeginEdit();
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
                CodeEditExtensions.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(new string[] { reference }).ToArray(), keepExisting: true, projectName: project.ProjectName);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveReference(DotNetProject project)
        {
            var reference = projectExplorerTreeView.SelectedNode.Tag as DotNetProject.AssemblyReference;
            if (reference != null)
            {
                if (project.RemoveReference(reference.FullName))
                {
                    var references = project.References.Select(x => x.FullName).ToArray();
                    CodeEditExtensions.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(references).ToArray(), projectName: project.ProjectName);
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
            else
            {
                var projectReference = projectExplorerTreeView.SelectedNode.Tag as DotNetProject.ProjectReference;
                if (projectReference != null)
                {
                    if (project.RemoveProjectReference(projectReference))
                    {
                        var references = project.ProjectReferences.Select(x => string.IsNullOrEmpty(x.ProjectName) && !string.IsNullOrEmpty(x.ProjectPath) ? Path.GetFileNameWithoutExtension(x.ProjectPath) : x.ProjectName).ToArray();
                        var extension = string.Format(".{0}", project.DefaultExtension);
                        CodeEditExtensions.RegisterProjectReferences(extension, references, project.ProjectName);
                        UpdateProjectExplorer();
                        UpdateCodeNavigation();
                    }
                }
            }
        }

        private void RemoveProjectFolder(DotNetProject project)
        {
            var node = GetNodeToRemove(projectExplorerTreeView.SelectedNode);

            if (MessageBox.Show(string.Format(StringConsts.RemoveFolderConfirmation, node.Name), "Studio", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                project.RemoveProjectFolder(node.Name);
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
        }

        private void RemoveFile(DotNetProject project)
        {
            var node = GetNodeToRemove(projectExplorerTreeView.SelectedNode);

            IList<string> removedFiles = new List<string>();

            var fileName = ProjectExplorer.GetFileNameFromNode(node);

            if (!string.IsNullOrEmpty(fileName))
                removedFiles.Add(fileName);

            for (var i = 0; i < node.Nodes.Count; i++)
            {
                fileName = ProjectExplorer.GetFileNameFromNode(node.Nodes[i]);

                if (!string.IsNullOrEmpty(fileName))
                    removedFiles.Add(fileName);
            }

            if (removedFiles.Count > 0)
            {
                if (!ConfirmSaveBeforeClosing(GetModifiedFiles(removedFiles), true))
                    return;

                foreach (var file in removedFiles)
                {
                    if (project.IsFolder)
                    {
                        project.RemoveSolutionFile(file, Path.GetDirectoryName(solution.SolutionFileName) + "/");
                        solution.IsModified = true;
                    }
                    else
                    {
                        project.RemoveFile(file);
                        RemoveDesigner(FindDesigner(file));
                    }

                    CloseFile(file);
                }

                CodeEditExtensions.UnregisterCode(project.ProjectExtension, GetSourceFiles(removedFiles), project.ProjectName);

                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveSolutionFolder(DotNetProject project)
        {
            if (MessageBox.Show(string.Format("'{0}' will be removed", project.ProjectName), "Alternet Studio", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                solution.RemoveProject(project);
                CloseProject(project);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveProject(DotNetProject project)
        {
            var list = GetModifiedFiles(project.AllFiles(true));
            if (!ConfirmSaveBeforeClosing(list, true))
                return;

            solution.RemoveProject(project);
            CloseProject(project);
            UpdateProjectExplorer();
            UpdateCodeNavigation();
        }

        private void RemoveProjectItemMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project == null || !(project.HasProject || project.IsFolder))
                return;

            if (IsProjectFolderNode(projectExplorerTreeView.SelectedNode))
                RemoveProjectFolder(project);
            else
            if (IsReferenceNode(projectExplorerTreeView.SelectedNode))
                RemoveReference(project);
            else
                if (IsFileNode(projectExplorerTreeView.SelectedNode))
                RemoveFile(project);
            else
                if (project.IsFolder && solution != null)
                RemoveSolutionFolder(project);
            else
                RemoveProject(project);
        }

        private void SetDefaultProjectMenuItem_Click(object sender, EventArgs e)
        {
            var project = GetProject(projectExplorerTreeView.SelectedNode);
            if (project != null)
            {
                solution.DefaultProject = project;
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