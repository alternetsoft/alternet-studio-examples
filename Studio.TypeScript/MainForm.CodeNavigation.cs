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
using System.Drawing;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private CodeNavigationBar codeNavigationBar;

        private void InitializeCodeNavigationBar()
        {
            codeNavigationBar = new CodeNavigationBar(classesComboBox, methodsComboBox, codeExplorer, () => ActiveSyntaxEdit, imageList1);
        }

        private void CodeNavigationBarPanel_Resize(object sender, EventArgs e)
        {
            const int SizeGap = 6;
            int newWidth = (codeNavigationBarPanel.ClientSize.Width - SizeGap) / 2;
            classesComboBox.Width = newWidth;
            methodsComboBox.Width = newWidth;
        }

        private void UpdateCodeNavigation(bool update = true)
        {
            codeNavigationBar?.Update(update);

            if (update)
                UpdateControls();
        }

        private void ActivateFindResultsTab()
        {
            bottomTabControl.SelectedTab = findResultsTabPage;
        }

        private void GotoDefinitionMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                GoToDefinition(edit);
        }

        private void GoToDefinition(IScriptEdit edit)
        {
            var location = edit.FindDeclaration(edit.Position);
            if (location == null || string.IsNullOrEmpty(location.FileName))
                return;

            navigationHistory.SaveCurrentLocationToHistory(edit.Position, edit.FileName, edit.GetLine(edit.Position.Y));
            edit = OpenFile(location.FileName);
            edit.MakeVisible(new Point(location.Column, location.Line), true);

            navigationHistory.SaveCurrentLocationToHistory(edit.Position, edit.FileName, edit.GetLine(edit.Position.Y));
            navigationHistory.UpdateHistory(historyBackwardContextMenu.Items, 0, historyBackwardToolSplitButton, historyForwardToolButton);
        }

        private void FindReferencesMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                FindAllReferences(edit);
        }

        private void FindAllReferences(IScriptEdit edit)
        {
            IRangeList references = new RangeList();
            if (edit.FindReferences(edit.Position, references) > 0)
            {
                findResultsControl.AddFindResults(references);
                if (references.Count > 1)
                    ActivateFindResultsTab();
                else
                    NavigateToRange(references[0] as IFileRange);
            }
        }

        private void NavigateToRange(IFileRange range)
        {
            if (range != null)
            {
                if (!string.IsNullOrEmpty(range.FileName))
                {
                    var edit = OpenFile(range.FileName);
                    if (edit != null)
                    {
                        edit.MakeVisible(new Point(range.StartPoint.X, range.StartPoint.Y), true);
                        edit.Focus();
                    }
                }
            }
        }

        private bool CanGotoDefinition()
        {
            var result = false;
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                return edit.CanFindDeclaration(edit.Position);

            return result;
        }
    }
}
