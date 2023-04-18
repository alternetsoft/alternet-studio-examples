#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace Selection
{
    public partial class Form1 : Form
    {
        private const string DisableSelectionDesc = "Disable selecting any text";
        private const string DisableDraggingDesc = "Disable dragging the selected text";
        private const string SelectBeyondEolDesc = "Draw selection beyond end of line.";
        private const string UseColorsDesc = "Preserve colors of the text when drawing selection";
        private const string HideSelectionDesc = "Hide selection when control losts focus";
        private const string SelectLineOnDblClickDesc = "Select whole line instead of single word when user double clicks on the text";
        private const string HighlightSelectedWordsDesc = "Specifies that the Edit control should select all instances of the chosen words.";
        private const string PersistentBlocksDesc = "Retain selection when the cursor is moved, until a new block is selecte.";
        private const string OverwriteBlocksDesc = "Replace selected text with whatever is typed next";
        private const string SelectionForeColorDesc = "Foreground color of the selected text when owner control has input focus";
        private const string SelectionBackColorDesc = "Background color of the selected text when owner control has input focus";
        private const string SelectionBorderColorDesc = "Color of the selection border";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options |= SelectionOptions.DrawBorder;
            chbDisableSelection.Checked = (SelectionOptions.DisableSelection & syntaxEdit1.Selection.Options) != 0;
            chbDisableDragging.Checked = (SelectionOptions.DisableDragging & syntaxEdit1.Selection.Options) != 0;
            chbSelectBeyondEol.Checked = (SelectionOptions.SelectBeyondEol & syntaxEdit1.Selection.Options) != 0;
            chbUseColors.Checked = (SelectionOptions.UseColors & syntaxEdit1.Selection.Options) != 0;
            chbHideSelection.Checked = (SelectionOptions.HideSelection & syntaxEdit1.Selection.Options) != 0;
            chbSelectLineOnDblClick.Checked = (SelectionOptions.SelectLineOnDblClick & syntaxEdit1.Selection.Options) != 0;
            HighlightSelectedWordsCheckBox.Checked = (SelectionOptions.HighlightSelectedWords & syntaxEdit1.Selection.Options) != 0;
            chbPersistentBlocks.Checked = (SelectionOptions.PersistentBlocks & syntaxEdit1.Selection.Options) != 0;
            chbOverwriteBlocks.Checked = (SelectionOptions.OverwriteBlocks & syntaxEdit1.Selection.Options) != 0;
            cbSelectionForeColor.SelectedColor = syntaxEdit1.Selection.ForeColor;
            cbSelectionBackColor.SelectedColor = syntaxEdit1.Selection.BackColor;
            cbSelectionBorderColor.SelectedColor = syntaxEdit1.Selection.BorderColor;

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            syntaxEdit1.Lexer = csParser1;
        }

        private void DisableSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbDisableSelection.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.DisableSelection : syntaxEdit1.Selection.Options & ~SelectionOptions.DisableSelection;
        }

        private void DisableDraggingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbDisableDragging.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.DisableDragging : syntaxEdit1.Selection.Options & ~SelectionOptions.DisableDragging;
        }

        private void SeleceyondEolCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbSelectBeyondEol.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.SelectBeyondEol : syntaxEdit1.Selection.Options & ~SelectionOptions.SelectBeyondEol;
        }

        private void UseColorsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbUseColors.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.UseColors : syntaxEdit1.Selection.Options & ~SelectionOptions.UseColors;
        }

        private void HideSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbHideSelection.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.HideSelection : syntaxEdit1.Selection.Options & ~SelectionOptions.HideSelection;
        }

        private void SelectLineOnDblClickCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbSelectLineOnDblClick.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.SelectLineOnDblClick : syntaxEdit1.Selection.Options & ~SelectionOptions.SelectLineOnDblClick;
        }

        private void HighlightSelectedWordsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = HighlightSelectedWordsCheckBox.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.HighlightSelectedWords : syntaxEdit1.Selection.Options & ~SelectionOptions.HighlightSelectedWords;
        }

        private void PersistenlocksCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbPersistentBlocks.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.PersistentBlocks : syntaxEdit1.Selection.Options & ~SelectionOptions.PersistentBlocks;
        }

        private void OverwriteBlocksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbOverwriteBlocks.Checked ? syntaxEdit1.Selection.Options
                | SelectionOptions.OverwriteBlocks : syntaxEdit1.Selection.Options & ~SelectionOptions.OverwriteBlocks;
        }

        private void DisableSelectionCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDisableSelection);
            if (str != DisableSelectionDesc)
                toolTip1.SetToolTip(chbDisableSelection, DisableSelectionDesc);
        }

        private void DisableDraggingCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbDisableDragging);
            if (str != DisableDraggingDesc)
                toolTip1.SetToolTip(chbDisableDragging, DisableDraggingDesc);
        }

        private void SeleceyondEolCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSelectBeyondEol);
            if (str != SelectBeyondEolDesc)
                toolTip1.SetToolTip(chbSelectBeyondEol, SelectBeyondEolDesc);
        }

        private void UseColorsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUseColors);
            if (str != UseColorsDesc)
                toolTip1.SetToolTip(chbUseColors, UseColorsDesc);
        }

        private void HideSelectionCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHideSelection);
            if (str != HideSelectionDesc)
                toolTip1.SetToolTip(chbHideSelection, HideSelectionDesc);
        }

        private void SelectLineOnDblClickCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSelectLineOnDblClick);
            if (str != SelectLineOnDblClickDesc)
                toolTip1.SetToolTip(chbSelectLineOnDblClick, SelectLineOnDblClickDesc);
        }

        private void HighlightSelectedWordsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(HighlightSelectedWordsCheckBox);
            if (str != HighlightSelectedWordsDesc)
                toolTip1.SetToolTip(HighlightSelectedWordsCheckBox, HighlightSelectedWordsDesc);
        }

        private void PersistenlocksCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbPersistentBlocks);
            if (str != PersistentBlocksDesc)
                toolTip1.SetToolTip(chbPersistentBlocks, PersistentBlocksDesc);
        }

        private void OverwriteBlocksCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbOverwriteBlocks);
            if (str != OverwriteBlocksDesc)
                toolTip1.SetToolTip(chbOverwriteBlocks, OverwriteBlocksDesc);
        }

        private void SelectionForeColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.ForeColor = cbSelectionForeColor.SelectedColor;
        }

        private void SelectionBackColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.BackColor = cbSelectionBackColor.SelectedColor;
        }

        private void SelectionBorderColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Selection.BorderColor = cbSelectionBorderColor.SelectedColor;
        }

        private void SelectionForeColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSelectionForeColor);
            if (str != SelectionForeColorDesc)
                toolTip1.SetToolTip(cbSelectionForeColor, SelectionForeColorDesc);
        }

        private void SelectionBackColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSelectionBackColor);
            if (str != SelectionBackColorDesc)
                toolTip1.SetToolTip(cbSelectionBackColor, SelectionBackColorDesc);
        }

        private void SelectionBorderColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSelectionBorderColor);
            if (str != SelectionBorderColorDesc)
                toolTip1.SetToolTip(cbSelectionBorderColor, SelectionBorderColorDesc);
        }
    }
}
