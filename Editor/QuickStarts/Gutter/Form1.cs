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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;

namespace Gutter
{
    public partial class Form1 : Form
    {
        private const string ShowGutterDesc = "Display gutter area";
        private const string GutterColorDesc = "Choose gutter background color";
        private const string PenColorDesc = "Choose gutter border color";
        private const string GradientGutterDesc = "Customize gutter backgound";
        private const string GradientBeginColorDesc = "Gutter gradient background start color";
        private const string GradientEndColorDesc = "Gutter gradient background end color";
        private const string LinesOnGutterDesc = "Draw numbers of lines on gutter area";
        private const string LineNumbersBackColorDesc = "Background color of the line numbers";
        private const string LineNumbersAlignDesc = "Choose line numbers alignment";
        private const string LineNumbersForeColorDesc = "Foreground color of the line numbers";
        private const string LineNumbersDesc = "Draw line numbers";
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";
        private Color gradientBeginColor = Color.Blue;
        private Color gradientEndColor = Color.White;

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

            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;
            syntaxEdit1.Gutter.PenColor = Color.LightGray;

            chbShowGutter.Checked = syntaxEdit1.Gutter.Visible;
            nudGutterWidth.Maximum = syntaxEdit1.Width;
            nudGutterWidth.Value = syntaxEdit1.Gutter.Width;
            cbGutterColor.SelectedColor = syntaxEdit1.Gutter.BrushColor;
            cbPenColor.SelectedColor = syntaxEdit1.Gutter.PenColor;
            cbGradientBeginColor.SelectedColor = gradientBeginColor;
            cbGradientEndColor.SelectedColor = gradientEndColor;

            chbLineNumbers.Checked = (GutterOptions.PaintLineNumbers & syntaxEdit1.Gutter.Options) != 0;
            chbLinesOnGutter.Checked = (GutterOptions.PaintLinesOnGutter & syntaxEdit1.Gutter.Options) != 0;
            nudLineNumbersStart.Maximum = 10000;
            nudLineNumbersStart.Value = syntaxEdit1.Gutter.LineNumbersStart;
            string[] s = Enum.GetNames(typeof(StringAlignment));
            cbLineNumbersAlign.Items.AddRange(s);
            cbLineNumbersAlign.SelectedIndex = (int)syntaxEdit1.Gutter.LineNumbersAlignment;
            nudLineNumbersLeftIndent.Maximum = 10000;
            nudLineNumbersLeftIndent.Value = syntaxEdit1.Gutter.LineNumbersLeftIndent;
            nudLineNumbersRightIndent.Maximum = 10000;
            nudLineNumbersRightIndent.Value = syntaxEdit1.Gutter.LineNumbersRightIndent;
            cbLineNumbersForeColor.SelectedColor = syntaxEdit1.Gutter.LineNumbersForeColor;
            cbLineNumbersBackColor.SelectedColor = syntaxEdit1.Gutter.LineNumbersBackColor;
        }

        private void ShowGutterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Visible = chbShowGutter.Checked;
        }

        private void GradientGutterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Transparent = chbGradientGutter.Checked;
            syntaxEdit1.Invalidate();
        }

        private void GutterWidthNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Width = (int)nudGutterWidth.Value;
        }

        private void GutterColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.BrushColor = cbGutterColor.SelectedColor;
        }

        private void PenColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.PenColor = cbPenColor.SelectedColor;
        }

        private void GradieneginColorComboBoxTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            gradientBeginColor = cbGradientBeginColor.SelectedColor;
            if (chbGradientGutter.Checked)
                syntaxEdit1.Invalidate();
        }

        private void GradientEndColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            gradientEndColor = cbGradientEndColor.SelectedColor;
            if (chbGradientGutter.Checked)
                syntaxEdit1.Invalidate();
        }

        private void SyntaxEdit1_PaintBackground(object sender, PaintEventArgs e)
        {
            if (syntaxEdit1.Transparent && syntaxEdit1.Gutter.Visible)
            {
                Rectangle r = syntaxEdit1.Gutter.Rect;
                e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(r.Location, new Point(r.Right, r.Bottom), gradientBeginColor, gradientEndColor), r);
            }
        }

        private void LineNumbersLeftIndentNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersLeftIndent = (int)nudLineNumbersLeftIndent.Value;
        }

        private void LineNumbersRightIndentNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersRightIndent = (int)nudLineNumbersRightIndent.Value;
        }

        private void LineNumbersStartNumeric_ValueChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersStart = (int)nudLineNumbersStart.Value;
        }

        private void LineNumbersAlignComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersAlignment = (StringAlignment)cbLineNumbersAlign.SelectedIndex;
        }

        private void LinesOnGutterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLinesOnGutter.Checked ? syntaxEdit1.Gutter.Options
                | GutterOptions.PaintLinesOnGutter : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLinesOnGutter;
        }

        private void LineNumbersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLineNumbers.Checked ? syntaxEdit1.Gutter.Options
                | GutterOptions.PaintLineNumbers : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineNumbers;
        }

        private void LineNumbersForeColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersForeColor = cbLineNumbersForeColor.SelectedColor;
        }

        private void LineNumbersBackColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersBackColor = cbLineNumbersBackColor.SelectedColor;
        }

        private void ShowGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbShowGutter);
            if (str != ShowGutterDesc)
                toolTip1.SetToolTip(chbShowGutter, ShowGutterDesc);
        }

        private void GutterColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbGutterColor);
            if (str != GutterColorDesc)
                toolTip1.SetToolTip(cbGutterColor, GutterColorDesc);
        }

        private void PenColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbPenColor);
            if (str != PenColorDesc)
                toolTip1.SetToolTip(cbPenColor, PenColorDesc);
        }

        private void GradientGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbGradientGutter);
            if (str != GradientGutterDesc)
                toolTip1.SetToolTip(chbGradientGutter, GradientGutterDesc);
        }

        private void GradieneginColorComboBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbGradientBeginColor);
            if (str != GradientBeginColorDesc)
                toolTip1.SetToolTip(cbGradientBeginColor, GradientBeginColorDesc);
        }

        private void GradientEndColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbGradientEndColor);
            if (str != GradientEndColorDesc)
                toolTip1.SetToolTip(cbGradientEndColor, GradientEndColorDesc);
        }

        private void LinesOnGutterCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLinesOnGutter);
            if (str != LinesOnGutterDesc)
                toolTip1.SetToolTip(chbLinesOnGutter, LinesOnGutterDesc);
        }

        private void LineNumbersForeColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLineNumbersForeColor);
            if (str != LineNumbersForeColorDesc)
                toolTip1.SetToolTip(cbLineNumbersForeColor, LineNumbersForeColorDesc);
        }

        private void LineNumbersBackColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLineNumbersBackColor);
            if (str != LineNumbersBackColorDesc)
                toolTip1.SetToolTip(cbLineNumbersBackColor, LineNumbersBackColorDesc);
        }

        private void LineNumbersAlignComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLineNumbersAlign);
            if (str != LineNumbersAlignDesc)
                toolTip1.SetToolTip(cbLineNumbersAlign, LineNumbersAlignDesc);
        }

        private void LineNumbersCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbLineNumbers);
            if (str != LineNumbersDesc)
                toolTip1.SetToolTip(chbLineNumbers, LineNumbersDesc);
        }
    }
}
