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
using System.Windows.Forms;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;
using Alternet.FormDesigner.WinForms;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private ToolStripButton newFile = new ToolStripButton("File...");
        private ToolStripButton newProject = new ToolStripButton("Project...");
        private ToolStripButton newForm = new ToolStripButton("Form...");
        private IList<string> recentFiles = new List<string>();
        private IList<string> recentProjects = new List<string>();

        private void InitializeToolbar()
        {
            filesMenuStrip.Items.Add(newFile);
            filesMenuStrip.Items.Add(newForm);
            filesMenuStrip.Items.Add(newProject);
            fileMenuItem.DropDownOpening += FileMenuItem_DropDownOpening;
            LoadRecentFiles();

            newFile.Click += new System.EventHandler(this.NewMenuItem_Click);
            newFile.ImageScaling = ToolStripItemImageScaling.None;
            newFile.DisplayStyle = ToolStripItemDisplayStyle.Text;
            newForm.Click += new System.EventHandler(this.NewFormMenuItem_Click);
            newForm.ImageScaling = ToolStripItemImageScaling.None;
            newForm.DisplayStyle = ToolStripItemDisplayStyle.Text;
            newProject.Click += new System.EventHandler(this.NewProjectMenuItem_Click);
            newProject.ImageScaling = ToolStripItemImageScaling.None;
            newProject.DisplayStyle = ToolStripItemDisplayStyle.Text;

            openToolButton.Tag = openMenuItem;
            saveToolButton.Tag = saveMenuItem;
            cutToolButton.Tag = cutMenuItem;
            copyToolButton.Tag = copyMenuItem;
            pasteToolButton.Tag = pasteMenuItem;
            undoToolButton.Tag = undoMenuItem;
            redoToolButton.Tag = redoMenuItem;
            findToolButton.Tag = findMenuItem;
            replaceToolButton.Tag = replaceMenuItem;
            gotoToolButton.Tag = gotoMenuItem;
            printPreviewToolButton.Tag = printPreviewMenuItem;
            printToolButton.Tag = printMenuItem;

            ProjectFrameworksComboBox.SelectedIndexChanged += ProjectComboBox_SelectedIndexChanged;
        }

        private void FileMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentFilesMenuItem.Enabled = recentFiles.Count > 0;
            recentProjectsMenuItem.Enabled = recentProjects.Count > 0;
            UpdateRecentFiles();
        }

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentFramework = CodeUtils.GetSelectedFramework(ProjectFrameworksComboBox);
        }

        private void UpdateEditorStatus()
        {
            var edit = ActiveSyntaxEdit;
            var status = edit?.Status;
            positionStatusLabel.Text = status?.Position ?? string.Empty;
            modifiedStatusLabel.Text = status?.Modified ?? string.Empty;
            overwriteStatusLabel.Text = status?.Overwrite ?? " ";
        }

        private void StandardToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag is ToolStripItem)
                ((ToolStripItem)e.ClickedItem.Tag).PerformClick();
        }

        private void UpdateBookmarkButtons()
        {
            var edit = ActiveSyntaxEdit;
            bool enabled = edit != null;

            bool HasBookmarks()
            {
                return BookMarkManager.SharedBookMarks.GetBookMarkCount() > 0;
            }

            bool hasBookmarks = HasBookmarks();
            toggleBookmarkToolButton.Enabled = enabled;
            prevBookmarkToolButton.Enabled = hasBookmarks;
            nextBookmarkToolButton.Enabled = hasBookmarks;
            clearAllBookmarksToolButton.Enabled = hasBookmarks;
        }

        private void UpdateEditorButtons()
        {
            var edit = ActiveSyntaxEdit;

            var enabled = edit != null;
            var canCut = enabled && edit.CanCut;
            var canCopy = enabled && edit.CanCopy;
            var canUndo = enabled && edit.CanUndo;
            var canRedo = enabled && edit.CanRedo;
            var canDelete = enabled && edit.CanDelete;
            var canPaste = enabled && edit.CanPaste;

            closeProjectMenuItem.Enabled = HasProject();
            saveProjectMenuItem.Enabled = HasProject();
            bool hasFrameworks = HasProject() && Project.TargetFrameworks?.Count > 0;
            ProjectFrameworksComboBox.Visible = hasFrameworks;

            saveMenuItem.Enabled = enabled;

            saveAsMenuItem.Enabled = enabled && !edit.FileName.Contains("Designer");
            findMenuItem.Enabled = enabled;
            replaceMenuItem.Enabled = enabled;
            gotoMenuItem.Enabled = enabled;
            selectAllMenuItem.Enabled = enabled;
            printMenuItem.Enabled = enabled;
            printPreviewMenuItem.Enabled = enabled;
            closeFileMenuItem.Enabled = enabled;

            cutMenuItem.Enabled = canCut;
            copyMenuItem.Enabled = canCopy;
            pasteMenuItem.Enabled = canPaste;
            deleteMenuItem.Enabled = canDelete;

            undoMenuItem.Enabled = canUndo;
            redoMenuItem.Enabled = canRedo;

            viewDesignerMenuItem.Enabled = false;
            if ((edit != null) && (edit.FileName != string.Empty) && !edit.FileName.EndsWith(".resx") && !edit.FileName.EndsWith(".xaml"))
                viewDesignerMenuItem.Enabled = enabled && FormFilesUtility.CheckIfFormFilesExist(edit.FileName);

            viewCodeMenuItem.Enabled = false;

            saveToolButton.Enabled = enabled;
            findToolButton.Enabled = enabled;
            replaceToolButton.Enabled = enabled;
            gotoToolButton.Enabled = enabled;
            printToolButton.Enabled = enabled;
            printPreviewToolButton.Enabled = enabled;

            cutToolButton.Enabled = canCut;
            copyToolButton.Enabled = canCopy;
            pasteToolButton.Enabled = canPaste;

            undoToolButton.Enabled = canUndo;
            redoToolButton.Enabled = canRedo;

            UpdateBookmarkButtons();

            if (edit == null && ActiveFormDesigner != null)
                UpdateDesignerButtons();
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanUndo)
                edit.Undo();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Undo();
            }
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanRedo)
                edit.Redo();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Redo();
            }
        }

        private void FindMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Search();
        }

        private void ReplaceMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Replace();
        }

        private void GotoMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.GotoLine();
        }

        private void CutMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanCut)
                edit.Cut();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Cut();
            }
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
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
                if (designer != null)
                    designer.DesignerCommands.Copy();
            }
        }

        private void PasteMenuItem_Click(object sender, EventArgs e)
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
                if (designer != null)
                    designer.DesignerCommands.Paste();
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanDelete)
                edit.Delete();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Delete();
            }
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.SelectAll();
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.SelectAll();
            }
        }

        private void PrintMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.Print();
        }

        private void PrintPreviewMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.PrintPreview();
        }

        private void ClearAllBookmarksToolButton_Click(object sender, System.EventArgs e)
        {
            BookMarkManager.SharedBookMarks.Clear();
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                edit.Invalidate();
            }

            UpdateBookmarkButtons();
        }

        private void NextBookmarkToolButton_Click(object sender, System.EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoNextBookMark();
        }

        private void PrevBookmarkToolButton_Click(object sender, System.EventArgs e)
        {
            BookMarkManager.SharedBookMarks.GotoPrevBookMark();
        }

        private void ToggleBookmarkToolButton_Click(object sender, System.EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                BookMarkManager.SharedBookMarks.ToggleBookMark(edit.Position, 0, int.MaxValue, -1, string.Empty, string.Empty, string.Empty, null, edit.FileName);
            }
        }

        private void EditorContextMenu_Opened(object sender, EventArgs e)
        {
            if (sender is ContextMenuStrip)
                UpdateEditorContextMenu((ContextMenuStrip)sender);
        }

        private void UpdateEditorContextMenu(ContextMenuStrip menu)
        {
            var gotoDefinitionItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(item => item.Name == "cmiGotoDefinition");
            if (gotoDefinitionItem != null)
                gotoDefinitionItem.Enabled = CanGotoDefinition();
        }
    }
}