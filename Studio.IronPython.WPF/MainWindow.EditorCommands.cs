#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System.Windows;

namespace AlternetStudio.IronPython.Wpf.Demo
{
    public partial class MainWindow
    {
        private void InitializeToolbar()
        {
        }

        private void UpdateEditorStatus()
        {
            var edit = ActiveSyntaxEdit;
            var status = edit?.Status;
            positionStatusLabel.Content = status?.Position ?? string.Empty;
            modifiedStatusLabel.Content = status?.Modified ?? string.Empty;
            overwriteStatusLabel.Content = status?.Overwrite ?? string.Empty;
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

            saveProjectMenuItem.IsEnabled = Project.HasProject;
            closeProjectMenuItem.IsEnabled = Project.HasProject;
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
            deleteMenuItem.IsEnabled = canDelete;

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

            undoToolButton.IsEnabled = canUndo;
            redoToolButton.IsEnabled = canRedo;
        }

        private void UndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanUndo)
                edit.Undo();
        }

        private void RedoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanRedo)
                edit.Redo();
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
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanCopy)
                    edit.Copy();
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
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanDelete)
                    edit.Delete();
            }
        }

        private void SelectAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.SelectAll();
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
    }
}