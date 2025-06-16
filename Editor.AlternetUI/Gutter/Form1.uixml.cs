#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Gutter
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        private Color gradientBeginColor = Color.Blue;
        private Color gradientEndColor = Color.White;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            Group(
                lbBackColor,
                cbBackColor,
                chbUseGradient,
                cbGradientBeginColor,
                cbGradientEndColor,
                lbGradientBegin,
                lbGradientEnd)
                .Enabled(false);

            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);

            pnSettings.MinSizeGrowMode = WindowSizeToContentMode.Height;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            if(textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;

            chbDisplayGutter.IsChecked = syntaxEdit1.Gutter.Visible;
            nudGutterWidth.Maximum = (int)syntaxEdit1.Width;
            nudGutterWidth.Value = (int)syntaxEdit1.Gutter.Width;
            cbGutterColor.SelectedItem = cbGutterColor.FindOrAdd(syntaxEdit1.Gutter.BrushColor);
            cbGradientBeginColor.Value = gradientBeginColor;
            cbGradientEndColor.Value = gradientEndColor;

            chbDisplayLineNumbers.IsChecked
                = (GutterOptions.PaintLineNumbers & syntaxEdit1.Gutter.Options) != 0;
            chbLinesOnGutter.IsChecked
                = (GutterOptions.PaintLinesOnGutter & syntaxEdit1.Gutter.Options) != 0;
            nudStart.Maximum = 10000;
            nudStart.Value = syntaxEdit1.Gutter.LineNumbersStart;
            string[] s = Enum.GetNames(typeof(Alternet.Drawing.StringAlignment));
            cbAlign.Items.AddRange(s);
            cbAlign.SelectedIndex = (int)syntaxEdit1.Gutter.LineNumbersAlignment;
            nudLeftIndent.Maximum = 10000;
            nudLeftIndent.Value = (int)syntaxEdit1.Gutter.LineNumbersLeftIndent;
            nudRightIndent.Maximum = 10000;
            nudRightIndent.Value = (int)syntaxEdit1.Gutter.LineNumbersRightIndent;
            cbForeColor.SelectedItem = cbForeColor.FindOrAdd(syntaxEdit1.Gutter.LineNumbersForeColor);
            cbBackColor.SelectedItem = cbBackColor.FindOrAdd(syntaxEdit1.Gutter.LineNumbersBackColor);

            chbDisplayGutter.CheckedChanged += ShowGutterCheckBox_CheckedChanged;
            cbGutterColor.SelectedIndexChanged += GutterColorComboBox_SelectedIndexChanged;
            nudGutterWidth.ValueChanged += GutterWidth_ValueChanged;
            chbUseGradient.CheckedChanged += GradientGutterCheckBox_CheckedChanged;
            cbGradientBeginColor.SelectedIndexChanged += GradientBeginColor_SelectedIndexChanged;
            cbGradientEndColor.SelectedIndexChanged += GradientEndColor_SelectedIndexChanged;

            chbDisplayLineNumbers.CheckedChanged += DisplayLineNumbers_CheckedChanged;
            chbLinesOnGutter.CheckedChanged += LinesOnGutter_CheckedChanged;
            cbForeColor.SelectedIndexChanged += ForeColor_SelectedIndexChanged;
            cbBackColor.SelectedIndexChanged += BackColor_SelectedIndexChanged;
            nudLeftIndent.ValueChanged += LeftIndent_ValueChanged;
            nudRightIndent.ValueChanged += RightIndent_ValueChanged;
            nudStart.ValueChanged += Start_ValueChanged;
            cbAlign.SelectedIndexChanged += Align_SelectedIndexChanged;
        }

        private void Align_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbAlign.SelectedIndex is null)
                return;
            syntaxEdit1.Gutter.LineNumbersAlignment
                = (Alternet.Drawing.StringAlignment)cbAlign.SelectedIndex;
        }

        private void Start_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersStart = nudStart.Value;
        }

        private void RightIndent_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersRightIndent = nudRightIndent.Value;
        }

        private void LeftIndent_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersLeftIndent = nudLeftIndent.Value;
        }

        private void BackColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersBackColor = cbBackColor.Value;
        }

        private void ForeColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.LineNumbersForeColor = cbForeColor.Value;
        }

        private void LinesOnGutter_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbLinesOnGutter.IsChecked
                ? syntaxEdit1.Gutter.Options | GutterOptions.PaintLinesOnGutter
                : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLinesOnGutter;
        }

        private void DisplayLineNumbers_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbDisplayLineNumbers.IsChecked
                ? syntaxEdit1.Gutter.Options | GutterOptions.PaintLineNumbers
                : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineNumbers;
        }

        private void GradientEndColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbGradientEndColor.Value is null)
                return;
            gradientEndColor = cbGradientEndColor.Value;
            if (chbUseGradient.IsChecked)
                syntaxEdit1.Invalidate();
        }

        private void GradientBeginColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbGradientBeginColor.Value is null)
                return;
            gradientBeginColor = cbGradientBeginColor.Value;
            if (chbUseGradient.IsChecked)
                syntaxEdit1.Invalidate();
        }

        private void GradientGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Transparent = chbUseGradient.IsChecked;
            syntaxEdit1.Invalidate();
        }

        private void GutterWidth_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Width = nudGutterWidth.Value;
        }

        private void GutterColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.BrushColor = cbGutterColor.Value;
        }

        private void ShowGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Visible = chbDisplayGutter.IsChecked;
        }
    }
}
