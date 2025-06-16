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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private IList<string> recentFiles = new List<string>();
        private IList<string> recentProjects = new List<string>();

        private void InitializeToolbar()
        {
            ProjectFrameworks.Visibility = Visibility.Collapsed;
            ProjectFrameworks.SelectionChanged += ProjectFrameworks_SelectionChanged;
            fileMenuItem.SubmenuOpened += FileMenuItem_SubmenuOpened;
            LoadRecentFiles();
        }

        private void FileMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            recentFilesMenuItem.IsEnabled = recentFiles.Count > 0;
            recentProjectsMenuItem.IsEnabled = recentProjects.Count > 0;
            UpdateRecentFiles();
        }

        private void ProjectFrameworks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFramework = CodeUtils.GetSelectedFramework(ProjectFrameworks);
        }

        private void UpdateEditorStatus()
        {
            var edit = ActiveSyntaxEdit;
            var status = edit?.Status;
            positionStatusLabel.Content = status?.Position ?? string.Empty;
            modifiedStatusLabel.Content = status?.Modified ?? string.Empty;
            overwriteStatusLabel.Content = status?.Overwrite ?? " ";
        }

        private bool HasBookmarks()
        {
            return BookMarkManager.SharedBookMarks.GetBookMarkCount() > 0;
        }

        private void UpdateBookmarkButtons()
        {
            var edit = ActiveSyntaxEdit;
            bool enabled = edit != null;

            bool hasBookmarks = HasBookmarks();
            toggleBookmarkToolButton.IsEnabled = enabled;
            prevBookmarkToolButton.IsEnabled = hasBookmarks;
            nextBookmarkToolButton.IsEnabled = hasBookmarks;
            clearAllBookmarksToolButton.IsEnabled = hasBookmarks;
        }

        private void UpdateEditorButtons()
        {
            var edit = ActiveSyntaxEdit;

            bool enabled = edit != null;
            bool canCut = enabled && edit.CanCut;
            bool canCopy = enabled && edit.CanCopy;
            bool canUndo = enabled && edit.CanUndo;
            bool canRedo = enabled && edit.CanRedo;
            bool canDelete = enabled && edit.CanDelete;
            bool canPaste = enabled && edit.CanPaste;

            saveProjectMenuItem.IsEnabled = HasProject();
            closeProjectMenuItem.IsEnabled = HasProject();
            bool hasFrameworks = HasProject() && Project.TargetFrameworks?.Count > 0;
            ProjectFrameworks.Visibility = hasFrameworks ? Visibility.Visible : Visibility.Collapsed;

            saveMenuItem.IsEnabled = enabled;
            saveMenuItemAs.IsEnabled = enabled && !FileBelongsToProject(edit.FileName);
            findMenuItem.IsEnabled = enabled;
            replaceMenuItem.IsEnabled = enabled;
            gotoMenuItem.IsEnabled = enabled;
            selectAllMenuItem.IsEnabled = enabled;
            printMenuItem.IsEnabled = enabled;
            printPreviewMenuItem.IsEnabled = enabled;
            closeFileMenuItem.IsEnabled = enabled;

            cutMenuItem.IsEnabled = canCut;
            copyMenuItem.IsEnabled = canCopy;
            pasteMenuItem.IsEnabled = canPaste;

            undoMenuItem.IsEnabled = canUndo;
            redoMenuItem.IsEnabled = canRedo;

            saveToolButton.IsEnabled = enabled;
            findToolButton.IsEnabled = enabled;
            replaceToolButton.IsEnabled = enabled;
            gotoToolButton.IsEnabled = enabled;
            printToolButton.IsEnabled = enabled;
            printPreviewToolButton.IsEnabled = enabled;

            cutToolButton.IsEnabled = canCut;
            copyToolButton.IsEnabled = canCopy;
            pasteToolButton.IsEnabled = canPaste;
            deleteMenuItem.IsEnabled = canDelete;

            viewDesignerMenuItem.IsEnabled = enabled && TryGetXamlFileNameToOpenDesigner(edit.FileName) != null;
            viewCodeMenuItem.IsEnabled = false;

            undoToolButton.IsEnabled = canUndo;
            redoToolButton.IsEnabled = canRedo;

            UpdateBookmarkButtons();

            if (edit == null && ActiveFormDesigner != null)
                UpdateDesignerButtons();
        }

        private void UndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanUndo)
                edit.Undo();
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanUndo)
                    designer.DesignerCommands.Undo();
            }
        }

        private void RedoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanRedo)
                edit.Redo();
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanRedo)
                    designer.DesignerCommands.Redo();
            }
        }

        private void FindMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Search();
        }

        private void ReplaceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Replace();
        }

        private void GotoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.GotoLine();
        }

        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanCut)
                edit.Cut();
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCut)
                    designer.DesignerCommands.Cut();
            }
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanCopy)
                    edit.Copy();
            }
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanCopy)
                    designer.DesignerCommands.Copy();
            }
        }

        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanPaste)
                    edit.Paste();
            }
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanPaste)
                    designer.DesignerCommands.Paste();
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanDelete)
                    edit.Delete();
            }
            else
            {
                var designer = ActiveFormDesigner;
                if ((designer != null) && designer.DesignerCommands.CanDelete)
                    designer.DesignerCommands.Delete();
            }
        }

        private void SelectAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.SelectAll();
            else
            {
                var designer = ActiveFormDesigner;
            }
        }

        private void PrintMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Print();
        }

        private void PrintPreviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.PrintPreview();
        }

        private void ToggleBookmarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 0, int.MaxValue, -1, string.Empty, string.Empty, string.Empty, null, edit.FileName);
            }
        }

        private void PrevBookmarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoPrevBookMark();
        }

        private void NextBookmarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoNextBookMark();
        }

        private void ClearAllBookmarksMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BookMarkManager.SharedBookMarks.Clear();
            var edit = ActiveSyntaxEdit as TextEditor;
            if (edit != null)
            {
                edit.Invalidate();
            }

            UpdateBookmarkButtons();
        }

        private void ContextMenu_Opened(object sender, EventArgs e)
        {
            if (sender is ContextMenu)
                UpdateEditorContextMenu((ContextMenu)sender);
        }

        private void UpdateEditorContextMenu(ContextMenu menu)
        {
            var gotoDefinitionItem = menu.Items.Cast<Control>().FirstOrDefault(item => item.Name == "cmiGotoDefinition");
            if (gotoDefinitionItem != null)
                gotoDefinitionItem.IsEnabled = CanGotoDefinition();
        }
    }
}