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
using Alternet.Common.Python;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Python.Wpf;

using Microsoft.Win32;

namespace AlternetStudio.Python.Wpf.Demo
{
    public partial class MainWindow
    {
        private PythonProjectExplorer projectExplorer = new PythonProjectExplorer();
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

        private void InitializeExplorerTrees()
        {
            projectExplorerTreeView.ContextMenu = projectExplorerTreeView.Resources["SolutionContext"] as System.Windows.Controls.ContextMenu;
            projectExplorer.ExplorerTree = projectExplorerTreeView;

            codeExplorer.ExplorerTree = codeExplorerTreeView;
            codeExplorer.NavigateToNodeRequested += CodeExplorer_NavigateToNodeRequested;
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (!Project.HasProject)
                return;

            TreeViewItem node = GetNodeToRemove((TreeViewItem)projectExplorerTreeView.SelectedItem);
            IList<string> removedFiles = new List<string>();

            var fileName = GetFileNameFromNode(node);

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
                    Project.RemoveFile(file);
                    CloseFile(file);
                }

                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            if (!Project.HasProject)
                return;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog().Value)
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
            System.Drawing.Point endP = new System.Drawing.Point(-1, -1);
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
                            System.Drawing.Point p = new System.Drawing.Point(-1, -1);
                            if (CodeUtils.GetPosition(tag, out p))
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
                ContextMenu popMenu = ((TreeView)sender).ContextMenu;
                bool enabledRef = Project.HasProject && (projectExplorerTreeView.SelectedItem != null) && IsReferenceNode((TreeViewItem)projectExplorerTreeView.SelectedItem);
                bool enabledFile = Project.HasProject && (projectExplorerTreeView.SelectedItem != null) && IsFileNode((TreeViewItem)projectExplorerTreeView.SelectedItem);

                if (popMenu != null)
                {
                    for (int i = popMenu.Items.Count - 1; i >= 0; i--)
                    {
                        if (popMenu.Items[i] is MenuItem)
                        {
                            MenuItem item = (MenuItem)popMenu.Items[i];
                            switch (item.Name)
                            {
                                case "addFileContextMenuItem":
                                    item.IsEnabled = Project.HasProject;
                                    break;
                                case "addReferenceContextMenuItem":
                                    item.IsEnabled = Project.HasProject;
                                    break;
                                case "removeFileContextMenuItem":
                                    item.IsEnabled = enabledFile;
                                    break;
                                case "removeReferenceContextMenuItem":
                                    item.IsEnabled = enabledRef;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void AddReference_Click(object sender, RoutedEventArgs e)
        {
            if (!Project.HasProject)
                return;

            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = Path.GetDirectoryName(Project.ProjectFileName);

            if (!dialog.ShowDialog().Value)
                return;

            var reference = dialog.FileName;
            if (Project.AddReference(Path.GetFileName(reference), reference))
            {
                UpdateProjectExplorer();
                UpdateCodeNavigation();
            }
        }

        private void RemoveReference_Click(object sender, RoutedEventArgs e)
        {
            if (!Project.HasProject)
                return;

            TreeViewItem item = (TreeViewItem)projectExplorerTreeView.SelectedItem;
            if (IsReferenceNode(item))
            {
                var reference = item.Tag as PythonProject.AssemblyReference;
                if (Project.RemoveReference(reference.FullName))
                {
                    var references = Project.References.Select(x => x.FullName).ToArray();
                    UpdateProjectExplorer();
                    UpdateCodeNavigation();
                }
            }
        }
    }
}