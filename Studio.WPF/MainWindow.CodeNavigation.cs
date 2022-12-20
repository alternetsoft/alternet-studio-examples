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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Threading;
using Alternet.Common;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private CodeNavigationBar codeNavigationBar;

        private void InitializeCodeNavigationBar()
        {
            codeNavigationBar = new CodeNavigationBar(classesComboBox, methodsComboBox, codeExplorer, () => ActiveSyntaxEdit);
        }

        private void UpdateCodeNavigation(bool update = true)
        {
            codeNavigationBar?.Update(update);

            if (update)
                UpdateControls();
        }

        private void ActivateFindResultsTab()
        {
            bottomTabControl.SelectedIndex = FindResultTabIndex;
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

            if (!IsValidLocation(location))
                return;

            navigationHistory.SaveCurrentLocationToHistory(edit.Position, edit.FileName, edit.GetLine(edit.Position.Y));
            edit = OpenFile(location.FileName);
            edit.MakeVisible(new Point(location.Column, location.Line), true);
            navigationHistory.SaveCurrentLocationToHistory(edit.Position, edit.FileName, edit.GetLine(edit.Position.Y));
            navigationHistory.UpdateHistory(backwardMenu.ContextMenu.Items, 0, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
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
                FilterReferences(references);
                if (references.Count > 0)
                {
                    findResultsControl.AddFindResults(references);
                    if (references.Count > 1)
                        ActivateFindResultsTab();
                    else
                        NavigateToRange(references[0] as IFileRange);
                }
            }
        }

        private void NavigateToRange(IFileRange range)
        {
            if (range != null)
            {
                if (!string.IsNullOrEmpty(range.FileName))
                {
                    IScriptEdit edit = OpenFile(range.FileName);
                    if (edit != null)
                    {
                        edit.MakeVisible(new System.Drawing.Point(range.StartPoint.X, range.StartPoint.Y), true);
                        edit.Focus();
                    }
                }
            }
        }

        private void FindImplementationsMenuItem_Click(object sender, EventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit != null)
                FindAllImplementations(edit);
        }

        private void FindAllImplementations(IScriptEdit edit)
        {
            IRangeList references = new RangeList();
            if (edit.FindImplementations(edit.Position, references) > 0)
            {
                FilterReferences(references);
                if (references.Count > 0)
                {
                    findResultsControl.AddFindResults(references);
                    if (references.Count > 1)
                        ActivateFindResultsTab();
                    else
                        NavigateToRange(references[0] as IFileRange);
                }
            }
            else
                GoToDefinition(edit);
        }

        private void FilterReferences(IList<IRange> references)
        {
            for (int i = references.Count - 1; i >= 0; i--)
            {
                if (!IsValidReference(references[i]))
                    references.RemoveAt(i);
            }
        }

        private bool IsValidLocation(string fileName)
        {
            return string.IsNullOrEmpty(fileName) && !System.IO.Path.GetFileName(fileName).StartsWith("Alternet_XamlGeneratedCodeFile_");
        }

        private bool IsValidLocation(SymbolLocation symbol)
        {
            return symbol != null && IsValidLocation(symbol.FileName);
        }

        private bool IsValidReference(IRange range)
        {
            var fileRange = range as IFileRange;
            return fileRange != null && IsValidLocation(fileRange.FileName);
        }

        private bool CanGotoDefinition()
        {
            var result = false;
            var edit = ActiveSyntaxEdit;
            if (edit != null)
            {
                var symbol = edit.FindDeclaration(edit.Position);
                return IsValidLocation(symbol);
            }

            return result;
        }
    }
}