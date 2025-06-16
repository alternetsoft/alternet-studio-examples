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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Microsoft.Win32;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private DotNetProjectExplorer projectExplorer = new DotNetProjectExplorer();

        private SolutionExplorer solutionExplorer = new SolutionExplorer();

        private CodeExplorer codeExplorer = new CodeExplorer();

        private static bool IsReferenceNode(TreeViewItem node)
        {
            TreeViewItem parent = GetParentItem(node);
            while (parent != null)
            {
                if (string.Compare(parent.Header.ToString(), "references", true) == 0)
                    return true;
                parent = GetParentItem(parent);
            }

            return false;
        }

        private static bool IsProjectNode(TreeViewItem node)
        {
            return (node.Tag != null) && (node.Tag is DotNetProject);
        }

        private static bool IsProjectFolderNode(TreeViewItem node)
        {
            return ProjectExplorer.IsProjectFolder(node);
        }

        private static TreeViewItem GetParentItem(TreeViewItem item)
        {
            for (var i = VisualTreeHelper.GetParent(item); i != null; i = VisualTreeHelper.GetParent(i))
            {
                if (i is TreeViewItem)
                    return (TreeViewItem)i;
            }

            return null;
        }

        private bool IsFileNode(TreeViewItem node)
        {
            string fileName = ProjectExplorer.GetFileNameFromNode(node);

            return !string.IsNullOrEmpty(fileName) && !IsReferenceNode(node);
        }

        private bool IsFolderNode(TreeViewItem node)
        {
            string fileName = ProjectExplorer.GetFileNameFromNode(node);

            return !string.IsNullOrEmpty(fileName) && string.Compare(fileName, Alternet.Common.StringConsts.FolderNode) == 0;
        }

        private void InitializeExplorerTrees()
        {
            projectExplorerTreeView.ContextMenu = projectExplorerTreeView.Resources["SolutionContext"] as System.Windows.Controls.ContextMenu;
            projectExplorer.ExplorerTree = projectExplorerTreeView;
            solutionExplorer.ExplorerTree = projectExplorerTreeView;

            codeExplorer.ExplorerTree = codeExplorerTreeView;
            codeExplorer.NavigateToNodeRequested += CodeExplorer_NavigateToNodeRequested;
        }

        private void RemoveFolder(DotNetProject project, TreeViewItem item)
        {
            project.RemoveFolder(item.Header.ToString());
            UpdateProjectExplorer();
        }

        private void RemoveProjectFolder(DotNetProject project)
        {
            var node = GetNodeToRemove((TreeViewItem)projectExplorerTreeView.SelectedItem);

            if (MessageBox.Show(string.Format(StringConsts.RemoveOnlyFolderConfirmation, node.Name), "AlterNET Studio", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                project.RemoveProjectFolder(node.Name, false);
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
        }

        private void RemoveFile(DotNetProject project)
        {
            if (project == null || !project.HasProject)
                return;

            TreeViewItem node = GetNodeToRemove((TreeViewItem)projectExplorerTreeView.SelectedItem);
            IList<string> removedFiles = new List<string>();

            var fileName = ProjectExplorer.GetFileNameFromNode(node);

            if (!string.IsNullOrEmpty(fileName))
            {
                removedFiles.Add(fileName);
            }

            for (int i = 0; i < node.Items.Count; i++)
            {
                fileName = ProjectExplorer.GetFileNameFromNode((TreeViewItem)node.Items[i]);

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

                CodeEditExtensions.UnregisterCode(project.ProjectExtension, GetSourceFiles(project, removedFiles, project.ProjectExtension, false));

                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var project = GetProject((TreeViewItem)projectExplorerTreeView.SelectedItem);
            if (project == null || !project.HasProject)
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

            if (openFileDialog.ShowDialog().Value)
            {
                var userCodeFileName = openFileDialog.FileName;
                IList<string> addedFiles = new List<string>();
                string xamlFileName = GetXamlFileName(userCodeFileName);
                if (!string.IsNullOrEmpty(xamlFileName) && File.Exists(xamlFileName))
                    addedFiles.Add(xamlFileName);
                addedFiles.Add(userCodeFileName);

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

                    CodeEditExtensions.RegisterCode(project.ProjectExtension, GetSourceFiles(project, addedFiles, project.ProjectExtension, true));
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

        private void OnItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                e.Handled = true;
                if ((projectExplorerTreeView.SelectedItem != null) && (projectExplorerTreeView.SelectedItem is TreeViewItem))
                {
                    var tag = ((TreeViewItem)projectExplorerTreeView.SelectedItem).Tag;

                    string codeFileName = null;

                    var formNodeData = tag as FormNodeData;
                    if (formNodeData != null)
                    {
                        if (formNodeData.OpenMode == FormOpenMode.Design)
                        {
                            if (formNodeData.FileName.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase))
                                OpenDesigner(formNodeData.FileName);
                            return;
                        }

                        codeFileName = formNodeData.FileName;
                    }
                    else
                        codeFileName = tag as string;

                    if (!IsReferenceNode((TreeViewItem)projectExplorerTreeView.SelectedItem) && !string.IsNullOrEmpty(codeFileName) && new FileInfo(codeFileName).Exists)
                        OpenFile(codeFileName);
                }
            }
        }

        private void OnCodeItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                e.Handled = true;
                if ((codeExplorerTreeView.SelectedItem != null) && (codeExplorerTreeView.SelectedItem is TreeViewItem))
                {
                    IScriptEdit edit = ActiveSyntaxEdit;
                    if (edit != null)
                    {
                        var tag = ((TreeViewItem)codeExplorerTreeView.SelectedItem).Tag;
                        if (tag != null)
                        {
                            System.Drawing.Point p;
                            if (CodeUtils.GetPosition(tag, edit.Document(), out p))
                            {
                                edit.Position = p;
                                edit.Focus();
                            }
                        }
                    }
                }
            }
        }

        private void ProjectExplorerTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TreeView)
            {
                var item = projectExplorerTreeView.SelectedItem as TreeViewItem;
                var project = GetProject(item);
                var sol = GetSolution(item);

                ContextMenu popMenu = projectExplorerTreeView.ContextMenu;
                bool enabledRef = project != null && project.HasProject && (item != null) && IsReferenceNode(item);
                bool enabledFile = project != null && project.HasProject && (item != null) && IsFileNode(item);

                if (popMenu != null)
                {
                    for (int i = popMenu.Items.Count - 1; i >= 0; i--)
                    {
                        if (popMenu.Items[i] is MenuItem)
                        {
                            MenuItem menuItem = (MenuItem)popMenu.Items[i];
                            switch (menuItem.Name)
                            {
                                case "addProjectContextMenuItem":
                                    menuItem.IsEnabled = solution != null && solution.HasProjects;
                                    break;
                                case "addFileContextMenuItem":
                                    menuItem.Visibility = project != null && (project.HasProject || project.IsFolder) ? Visibility.Visible : Visibility.Collapsed;
                                    break;
                                case "addReferenceContextMenuItem":
                                    menuItem.Visibility = project != null && project.HasProject ? Visibility.Visible : Visibility.Collapsed;
                                    break;
                                case "removeProjectItemContextMenuItem":
                                    menuItem.Visibility = project != null && projectExplorerTreeView.SelectedItem != null && IsValidNodeToRemove(project, (TreeViewItem)projectExplorerTreeView.SelectedItem) ? Visibility.Visible : Visibility.Collapsed;
                                    break;
                                case "setDefaultProjectContextMenuItem":
                                    menuItem.Visibility = !solution.IsEmpty && project != null && project.HasProject && project != Project ? Visibility.Visible : Visibility.Collapsed;
                                    menuItem.IsEnabled = project != null && project.HasProject;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidNodeToRemove(DotNetProject project, TreeViewItem node)
        {
            return IsFileNode(node) || IsReferenceNode(node) || ((IsProjectNode(node) || project.IsFolder) && solution != null && !solution.IsEmpty);
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.FilterIndex = 4;
            if (openFileDialog.ShowDialog().Value)
            {
                var project = new DotNetProject();
                project.Load(openFileDialog.FileName);
                solution.AddProject(project);
                OpenProject(project);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveProject(DotNetProject project)
        {
            if (project != null)
            {
                var list = GetModifiedFiles(project.AllFiles(true));
                if (!ConfirmSaveBeforeClosing(list, true))
                    return;

                solution.RemoveProject(project);
                CloseProject(project);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddReference_Click(object sender, RoutedEventArgs e)
        {
            var project = GetProject((TreeViewItem)projectExplorerTreeView.SelectedItem);
            if (project == null || !project.HasProject)
                return;

            var dlg = new AddReferenceDialog(project, solution);
            if (dlg.ShowDialog().Value)
            {
                project.References.Clear();
                project.ProjectReferences.Clear();
                var refs = dlg.References;
                var frameworkRefs = dlg.FrameworkReferences;
                var projRefs = dlg.ProjectReferences;

                foreach (var item in refs)
                {
                    string name = string.Empty;
                    string version = string.Empty;
                    if (AssemblyHelper.IsNugetReference(item, ref name, ref version))
                    {
                        project.AddNuGetReference(name, item);
                    }
                    else
                    {
                        name = Path.IsPathRooted(item) ? Path.GetFileNameWithoutExtension(item) : Path.GetFileName(item);
                        project.AddReference(name, item);
                    }
                }

                foreach (var item in frameworkRefs)
                {
                    string name = Path.IsPathRooted(item) ? Path.GetFileNameWithoutExtension(item) : Path.GetFileName(item);
                    project.AddReference(name, name);
                }

                foreach (var item in projRefs)
                {
                    project.ProjectReferences.Add(item);
                }

                CodeEditExtensions.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(refs).ToArray(), keepExisting: true, projectName: project.ProjectName);
                CodeEditExtensions.RegisterAssemblies(project.ProjectExtension, project.TryResolveAbsolutePaths(frameworkRefs).ToArray(), keepExisting: true, projectName: project.ProjectName);
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveReference(DotNetProject project)
        {
            if (project == null || !project.HasProject)
                return;

            TreeViewItem item = (TreeViewItem)projectExplorerTreeView.SelectedItem;
            if (IsReferenceNode(item))
            {
                var reference = item.Tag as DotNetProject.AssemblyReference;
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
                    var projectReference = item.Tag as DotNetProject.ProjectReference;
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
        }

        private void RemoveProjectItem_Click(object sender, RoutedEventArgs e)
        {
            var project = GetProject((TreeViewItem)projectExplorerTreeView.SelectedItem);
            if (project == null || !(project.HasProject || project.IsFolder))
                return;

            if (IsProjectFolderNode((TreeViewItem)projectExplorerTreeView.SelectedItem))
                RemoveProjectFolder(project);
            else
            if (IsReferenceNode((TreeViewItem)projectExplorerTreeView.SelectedItem))
                RemoveReference(project);
            else
            if (IsFileNode((TreeViewItem)projectExplorerTreeView.SelectedItem))
                RemoveFile(project);
            else
                RemoveProject(project);
        }

        private void SetDefaultProjectContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)projectExplorerTreeView.SelectedItem;
            var project = GetProject(item);

            if (project != null)
            {
                solution.DefaultProject = project;
                Project = project;
                UpdateScriptProject(project);
                UpdateProjectExplorer();
            }
        }
    }
}