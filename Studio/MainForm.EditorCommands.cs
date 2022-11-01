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
using System.Windows.Forms;
#if USEFORMDESIGNER
using Alternet.FormDesigner.WinForms;
#endif

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private ToolStripButton newFile = new ToolStripButton("File...");
        private ToolStripButton newProject = new ToolStripButton("Project...");
        private ToolStripButton newForm = new ToolStripButton("Form...");

        private void InitializeToolbar()
        {
            filesMenuStrip.Items.Add(newFile);
#if USEFORMDESIGNER
            filesMenuStrip.Items.Add(newForm);
#endif
            filesMenuStrip.Items.Add(newProject);

            newFile.Click += new System.EventHandler(this.NewMenuItem_Click);
            newFile.ImageScaling = ToolStripItemImageScaling.None;
            newFile.DisplayStyle = ToolStripItemDisplayStyle.Text;
#if USEFORMDESIGNER
            newForm.Click += new System.EventHandler(this.NewFormMenuItem_Click);
            newForm.ImageScaling = ToolStripItemImageScaling.None;
            newForm.DisplayStyle = ToolStripItemDisplayStyle.Text;
#endif
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

#if USEFORMDESIGNER
            viewDesignerMenuItem.Enabled = false;
            if ((edit != null) && (edit.FileName != string.Empty) && !edit.FileName.EndsWith(".resx") && !edit.FileName.EndsWith(".xaml"))
                viewDesignerMenuItem.Enabled = enabled && FormFilesUtility.CheckIfFormFilesExist(edit.FileName);
#endif

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

#if USEFORMDESIGNER
            if (edit == null && ActiveFormDesigner != null)
                UpdateDesignerButtons();
#endif
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanUndo)
                edit.Undo();
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Undo();
            }
#endif
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanRedo)
                edit.Redo();
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Redo();
            }
#endif
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
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Cut();
            }
#endif
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanCopy)
                    edit.Copy();
            }
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Copy();
            }
#endif
        }

        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                if (edit.CanPaste)
                    edit.Paste();
            }
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Paste();
            }
#endif
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null && edit.CanDelete)
                edit.Delete();
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.Delete();
            }
#endif
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                edit.SelectAll();
#if USEFORMDESIGNER
            else
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                    designer.DesignerCommands.SelectAll();
            }
#endif
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

        private void EditorContextMenu_Opened(object sender, EventArgs e)
        {
            if (sender is ContextMenuStrip)
                UpdateEditorContextMenu((ContextMenuStrip)sender);
        }

        private void UpdateEditorContextMenu(ContextMenuStrip menu)
        {
            var gotoDefinitionItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(item => item.Name == "gotoDefinitionMenuItem");
            if (gotoDefinitionItem != null)
                gotoDefinitionItem.Enabled = CanGotoDefinition();
        }
    }
}