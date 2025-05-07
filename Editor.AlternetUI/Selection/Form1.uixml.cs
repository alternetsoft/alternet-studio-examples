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

namespace Selection
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

            AddNoneColor(cbSelectionForeColor);
            AddNoneColor(cbSelectionBackColor);
            AddNoneColor(cbSelectionBorderColor);

            Form1_Load(this, EventArgs.Empty);

            static void AddNoneColor(ColorComboBox comboBox)
            {
                comboBox.AddColor(Color.Transparent, "Transparent");
                comboBox.AddColor(Color.Empty, "Empty");
            }

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;
            ActiveControl = syntaxEdit1;
            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
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
            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit1.Selection.Options |= SelectionOptions.DrawBorder;
/*
We can't assign selection colors like that as we don't know editor theme (dark or light)
            syntaxEdit1.Selection.ForeColor = Color.White;
            syntaxEdit1.Selection.BackColor = Color.SkyBlue;
            syntaxEdit1.Selection.BorderColor = Color.CadetBlue;
*/
            chbDisableSelection.IsChecked
                = (SelectionOptions.DisableSelection & syntaxEdit1.Selection.Options) != 0;
            chbDisableDragging.IsChecked
                = (SelectionOptions.DisableDragging & syntaxEdit1.Selection.Options) != 0;
            chbSelectBeyondEol.IsChecked
                = (SelectionOptions.SelectBeyondEol & syntaxEdit1.Selection.Options) != 0;
            chbUseColors.IsChecked = (SelectionOptions.UseColors & syntaxEdit1.Selection.Options) != 0;
            chbHideSelection.IsChecked
                = (SelectionOptions.HideSelection & syntaxEdit1.Selection.Options) != 0;
            chbSelectLineOnDblClick.IsChecked
                = (SelectionOptions.SelectLineOnDblClick & syntaxEdit1.Selection.Options) != 0;
            chbHighlightSelectedWords.IsChecked
                = (SelectionOptions.HighlightSelectedWords & syntaxEdit1.Selection.Options) != 0;
            chbPersistentBlocks.IsChecked
                = (SelectionOptions.PersistentBlocks & syntaxEdit1.Selection.Options) != 0;
            chbOverwriteBlocks.IsChecked
                = (SelectionOptions.OverwriteBlocks & syntaxEdit1.Selection.Options) != 0;
            cbSelectionForeColor.Value = syntaxEdit1.Selection.ForeColor;
            cbSelectionBackColor.Value = syntaxEdit1.Selection.BackColor;
            cbSelectionBorderColor.Value = syntaxEdit1.Selection.BorderColor;

            chbDisableSelection.CheckedChanged += DisableSelectionCheckBox_CheckedChanged;
            chbDisableDragging.CheckedChanged += DisableDraggingCheckBox_CheckedChanged;
            chbSelectBeyondEol.CheckedChanged += SelectByondEolCheckBoxTextBox_CheckedChanged;
            chbUseColors.CheckedChanged += UseColorsCheckBox_CheckedChanged;
            chbHideSelection.CheckedChanged += HideSelectionCheckBox_CheckedChanged;
            chbSelectLineOnDblClick.CheckedChanged += SelectLineOnDblClickCheckBox_CheckedChanged;
            chbHighlightSelectedWords.CheckedChanged += HighlightSelectedWordsCheckBox_CheckedChanged;
            chbPersistentBlocks.CheckedChanged += PersistentBlocksCheckBoxTextBox_CheckedChanged;
            chbOverwriteBlocks.CheckedChanged += OverwriteBlocksCheckBox_CheckedChanged;
            cbSelectionForeColor.SelectedIndexChanged += SelectionForeColorComboBox_SelectedIndexChanged;
            cbSelectionBackColor.SelectedIndexChanged += SelectionBackColorComboBox_SelectedIndexChanged;
            cbSelectionBorderColor.SelectedIndexChanged
                += SelectionBorderColorComboBox_SelectedIndexChanged;
        }

        private void DisableSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbDisableSelection.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.DisableSelection
                : syntaxEdit1.Selection.Options & ~SelectionOptions.DisableSelection;
        }

        private void DisableDraggingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbDisableDragging.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.DisableDragging
                : syntaxEdit1.Selection.Options & ~SelectionOptions.DisableDragging;
        }

        private void SelectByondEolCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbSelectBeyondEol.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol
                : syntaxEdit1.Selection.Options & ~SelectionOptions.SelectBeyondEol;
        }

        private void UseColorsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbUseColors.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.UseColors
                : syntaxEdit1.Selection.Options & ~SelectionOptions.UseColors;
        }

        private void HideSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbHideSelection.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.HideSelection
                : syntaxEdit1.Selection.Options & ~SelectionOptions.HideSelection;
        }

        private void SelectLineOnDblClickCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbSelectLineOnDblClick.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.SelectLineOnDblClick
                : syntaxEdit1.Selection.Options & ~SelectionOptions.SelectLineOnDblClick;
        }

        private void HighlightSelectedWordsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbHighlightSelectedWords.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.HighlightSelectedWords
                : syntaxEdit1.Selection.Options & ~SelectionOptions.HighlightSelectedWords;
        }

        private void PersistentBlocksCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbPersistentBlocks.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.PersistentBlocks
                : syntaxEdit1.Selection.Options & ~SelectionOptions.PersistentBlocks;
        }

        private void OverwriteBlocksCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.Options = chbOverwriteBlocks.IsChecked
                ? syntaxEdit1.Selection.Options | SelectionOptions.OverwriteBlocks
                : syntaxEdit1.Selection.Options & ~SelectionOptions.OverwriteBlocks;
        }

        private void SelectionForeColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.ForeColor = cbSelectionForeColor.Value;
            syntaxEdit1.Selection.Clear();
        }

        private void SelectionBackColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.BackColor = cbSelectionBackColor.Value;
            syntaxEdit1.Selection.Clear();
        }

        private void SelectionBorderColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Selection.BorderColor = cbSelectionBorderColor.Value;
        }
    }
}
