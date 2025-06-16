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
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Margin
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            pnSettings.MinSizeGrowMode = WindowSizeToContentMode.Height;

            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);
            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
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

        private void Form1_Load(object? sender, EventArgs e)
        {
            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.Lexer = csParser1;
            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit1.EditMargin.Visible = true;
            syntaxEdit1.Gutter.UserMarginWidth = 90;
            syntaxEdit1.Gutter.UserMarginText = @"\[chars] chars";

            syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;
            syntaxEdit1.Gutter.PenColor = Color.LightGray;

            chbDisplayUserMargin.IsChecked
                = (syntaxEdit1.Gutter.Options & GutterOptions.PaintUserMargin) != 0;
            nudUserMarginWidth.Maximum = (int)syntaxEdit1.Width;
            nudUserMarginWidth.Value = syntaxEdit1.Gutter.UserMarginWidth;
            tbUserMarginText.Text = syntaxEdit1.Gutter.UserMarginText;
            cbUserMarginForeColor.Value = Color.Black;
            cbUserMarginBkColor.Value = Color.White;
            chbDisplayMargin.IsChecked = syntaxEdit1.EditMargin.Visible;
            nudMarginPositon.Maximum = 1000;
            nudMarginPositon.Value = syntaxEdit1.EditMargin.Position;
            cbMarginColor.Value = syntaxEdit1.EditMargin.PenColor;
            chbDisplayColumns.IsChecked = syntaxEdit1.EditMargin.ColumnsVisible;
            cbColumnColor.Value = syntaxEdit1.EditMargin.ColumnPenColor;

            chbDisplayUserMargin.CheckedChanged += PaintUserMarginCheckBox_CheckedChanged;
            nudUserMarginWidth.ValueChanged += UserMarginWidthNumeric_ValueChanged;
            tbUserMarginText.KeyDown += UserMarginTextTextBox_KeyDown;
            tbUserMarginText.MouseLeave += UserMarginText_MouseLeave;
            cbUserMarginForeColor.SelectedItemChanged
                += UserMarginForeColorComboBox_SelectedIndexChanged;
            cbUserMarginBkColor.SelectedIndexChanged += UserMarginBkColorComboBox_SelectedIndexChanged;
            chbDisplayMargin.CheckedChanged += ShowMarginCheckBox_CheckedChanged;
            chbDisplayColumns.CheckedChanged += ColumnsVisibleCheckBox_CheckedChanged;
            nudMarginPositon.ValueChanged += MarginPosNumeric_ValueChanged;
            cbMarginColor.SelectedIndexChanged += MarginColorComboBox_SelectedIndexChanged;
            cbColumnColor.SelectedIndexChanged += ColumnsPenColorComboBox_SelectedIndexChanged;
        }

        private void UserMarginText_MouseLeave(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginText = tbUserMarginText.Text;
        }

        private void PaintUserMarginCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.Options = chbDisplayUserMargin.IsChecked
                ? syntaxEdit1.Gutter.Options | GutterOptions.PaintUserMargin
                : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintUserMargin;
        }

        private void UserMarginWidthNumeric_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginWidth = (int)nudUserMarginWidth.Value;
        }

        private void UserMarginTextTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                syntaxEdit1.Gutter.UserMarginText = tbUserMarginText.Text;
        }

        private void UserMarginForeColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginForeColor = cbUserMarginForeColor.Value;
        }

        private void UserMarginBkColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Gutter.UserMarginBackColor = cbUserMarginBkColor.Value;
        }

        private void ShowMarginCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.Visible = chbDisplayMargin.IsChecked;
        }

        private void ColumnsVisibleCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.ColumnsVisible = chbDisplayColumns.IsChecked;
        }

        private void MarginPosNumeric_ValueChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.Position = (int)nudMarginPositon.Value;
        }

        private void MarginColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.PenColor = cbMarginColor.Value;
            syntaxEdit1.Invalidate();
        }

        private void ColumnsPenColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.EditMargin.ColumnPenColor = cbColumnColor.Value;
            syntaxEdit1.Invalidate();
        }
    }
}