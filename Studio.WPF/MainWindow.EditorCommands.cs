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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Alternet.FormDesigner.Wpf;

namespace AlternetStudio.Wpf.Demo
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
            overwriteStatusLabel.Content = status?.Overwrite ?? " ";
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

        private void ContextMenu_Opened(object sender, EventArgs e)
        {
            if (sender is ContextMenu)
            {
                ContextMenu popMenu = (ContextMenu)sender;
                bool enabled = CanGotoDefinition();

                for (int i = popMenu.Items.Count - 1; i >= 0; i--)
                {
                    if (popMenu.Items[i] is MenuItem)
                    {
                        MenuItem item = (MenuItem)popMenu.Items[i];
                        switch (item.Name)
                        {
                            case "GotoMenuItemDefinition":
                                item.IsEnabled = enabled;
                                break;
                        }
                    }
                }
            }
        }
    }
}