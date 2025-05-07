#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace Margin
{
    public partial class Form1 : Form
    {
        private const string UserMarginDesc = "Draw margin area with user-defined content.";
        private const string UserMarginTextDesc = "Text of the user margin";
        private const string UserMarginForeColorDesc = "Foreground color for the user margin";
        private const string UserMarginBkColorDesc = "Background color for the user margin";
        private const string ShowMarginDesc = "Draw vertical line at Margin column";
        private const string ColumnsVisibleDesc = "Draw vertical lines at the given text columns";
        private const string MarginColorDesc = "Color of the margin line";
        private const string ColumnsPenColorDesc = "Color of the column margin line";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            syntaxEdit1.Lexer = csParser1;

            chbPaintUserMargin.Checked = (syntaxEdit1.Gutter.Options & GutterOptions.PaintUserMargin) != 0;
            nudUserMarginWidth.Maximum = syntaxEdit1.Width;
            nudUserMarginWidth.Value = syntaxEdit1.Gutter.UserMarginWidth;
            tbUserMarginText.Text = syntaxEdit1.Gutter.UserMarginText;
            cbUserMarginForeColor.SelectedColor = syntaxEdit1.Gutter.UserMarginForeColor;
            cbUserMarginBkColor.SelectedColor = syntaxEdit1.Gutter.UserMarginBackColor;

            chbShowMargin.Checked = syntaxEdit1.EditMargin.Visible;
            nudMarginPos.Maximum = 1000;
            nudMarginPos.Value = syntaxEdit1.EditMargin.Position;
            cbMarginColor.SelectedColor = syntaxEdit1.EditMargin.PenColor;
            chbColumnsVisible.Checked = syntaxEdit1.EditMargin.ColumnsVisible;
            cbColumnsPenColor.SelectedColor = syntaxEdit1.EditMargin.ColumnPenColor;
        }

        private void PaintUserMarginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbPaintUserMargin.Checked ? syntaxEdit1.Gutter.Options
                | GutterOptions.PaintUserMargin : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintUserMargin;
        }

        private void UserMarginWidthNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginWidth = (int)nudUserMarginWidth.Value;
        }

        private void UserMarginTextTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                syntaxEdit1.Gutter.UserMarginText = tbUserMarginText.Text;
        }

        private void UserMarginTextTextBox_Leave(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginText = tbUserMarginText.Text;
        }

        private void UserMarginForeColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginForeColor = cbUserMarginForeColor.SelectedColor;
        }

        private void UserMarginBkColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginBackColor = cbUserMarginBkColor.SelectedColor;
        }

        private void ShowMarginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.Visible = chbShowMargin.Checked;
        }

        private void ColumnsVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.ColumnsVisible = chbColumnsVisible.Checked;
        }

        private void MarginPosNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.Position = (int)nudMarginPos.Value;
        }

        private void MarginColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.PenColor = cbMarginColor.SelectedColor;
            syntaxEdit1.Invalidate();
        }

        private void ColumnsPenColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.ColumnPenColor = cbColumnsPenColor.SelectedColor;
            syntaxEdit1.Invalidate();
        }

        private void PaintUserMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbPaintUserMargin);
            if (str != UserMarginDesc)
                toolTip1.SetToolTip(chbPaintUserMargin, UserMarginDesc);
        }

        private void UserMarginTextTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(tbUserMarginText);
            if (str != UserMarginTextDesc)
                toolTip1.SetToolTip(tbUserMarginText, UserMarginTextDesc);
        }

        private void UserMarginForeColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbUserMarginForeColor);
            if (str != UserMarginForeColorDesc)
                toolTip1.SetToolTip(cbUserMarginForeColor, UserMarginForeColorDesc);
        }

        private void UserMarginBkColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbUserMarginBkColor);
            if (str != UserMarginBkColorDesc)
                toolTip1.SetToolTip(cbUserMarginBkColor, UserMarginBkColorDesc);
        }

        private void ShowMarginCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowMargin);
            if (str != ShowMarginDesc)
                toolTip1.SetToolTip(chbShowMargin, ShowMarginDesc);
        }

        private void ColumnsVisibleCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbColumnsVisible);
            if (str != ColumnsVisibleDesc)
                toolTip1.SetToolTip(chbColumnsVisible, ColumnsVisibleDesc);
        }

        private void MarginColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbMarginColor);
            if (str != MarginColorDesc)
                toolTip1.SetToolTip(cbMarginColor, MarginColorDesc);
        }

        private void ColumnsPenColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbColumnsPenColor);
            if (str != ColumnsPenColorDesc)
                toolTip1.SetToolTip(cbColumnsPenColor, ColumnsPenColorDesc);
        }
    }
}
