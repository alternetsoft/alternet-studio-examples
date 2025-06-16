#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

using Alternet.UI;
using Alternet.Maui;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts.Pages;

public partial class MarginPage : DemoPage
{
    private CsParser? csParser1 = new();
    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    public MarginPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Source.Lexer = csParser1;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + "newfile.cs");
        syntaxEdit1.Source.HighlightReferences = true;

        syntaxEdit1.Lexer = csParser1;
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.EditMargin.Visible = true;
        syntaxEdit1.Gutter.UserMarginWidth = 90;
        syntaxEdit1.Gutter.UserMarginText = @"\[chars] chars";

        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;
        syntaxEdit1.Gutter.PenColor = Alternet.Drawing.Color.LightGray;

        chbDisplayUserMargin.IsChecked
            = (syntaxEdit1.Gutter.Options & GutterOptions.PaintUserMargin) != 0;
        UserMarginWidthStepper.Maximum = (int)syntaxEdit1.Width;
        UserMarginWidthStepper.Value = syntaxEdit1.Gutter.UserMarginWidth;
        tbUserMarginText.Text = syntaxEdit1.Gutter.UserMarginText;
        UserMarginForeColorPicker.SelectedColor = Alternet.Drawing.Color.Black;
        UserMarginBkColorPicker.SelectedColor = Alternet.Drawing.Color.White;
        chbDisplayMargin.IsChecked = syntaxEdit1.EditMargin.Visible;
        MarginPositionStepper.Maximum = 1000;
        MarginPositionStepper.Value = syntaxEdit1.EditMargin.Position;
        MarginColorPicker.EnsureAddedAndSelect(syntaxEdit1.EditMargin.PenColor);
        chbDisplayColumns.IsChecked = syntaxEdit1.EditMargin.ColumnsVisible;
        ColumnColorPicker.EnsureAddedAndSelect(syntaxEdit1.EditMargin.ColumnPenColor);

        chbDisplayUserMargin.CheckedChanged += PaintUserMarginCheckBox_CheckedChanged;
        UserMarginWidthValue.Text = UserMarginWidthStepper.Value.ToString();
        UserMarginWidthStepper.ValueChanged += UserMarginWidthNumeric_ValueChanged;

        tbUserMarginText.TextChanged += UserMarginTextTextBox_TextChanged;
        UserMarginForeColorPicker.SelectedIndexChanged
            += UserMarginForeColorComboBox_SelectedIndexChanged;
        UserMarginBkColorPicker.SelectedIndexChanged += UserMarginBkColorComboBox_SelectedIndexChanged;
        chbDisplayMargin.CheckedChanged += ShowMarginCheckBox_CheckedChanged;
        chbDisplayColumns.CheckedChanged += ColumnsVisibleCheckBox_CheckedChanged;
        MarginPositionValue.Text = MarginPositionStepper.Value.ToString();
        MarginPositionStepper.ValueChanged += MarginPosNumeric_ValueChanged;
        MarginColorPicker.SelectedIndexChanged += MarginColorComboBox_SelectedIndexChanged;
        ColumnColorPicker.SelectedIndexChanged += ColumnsPenColorComboBox_SelectedIndexChanged;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Margin";

#pragma warning disable
    public void LoadFile(ITextSource source, string url)
#pragma warning enable
    {
        source.Text = string.Empty;
        source.BookMarks.Clear();
        source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !source.LoadStream(stream))
        {
            source.Text = $"Error loading text: {url}";
            return;
        }
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref csParser1);
    }

    private void PaintUserMarginCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.Options = chbDisplayUserMargin.IsChecked
            ? syntaxEdit1.Gutter.Options | GutterOptions.PaintUserMargin
            : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintUserMargin;
    }

    private void UserMarginWidthNumeric_ValueChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.UserMarginWidth = (int)UserMarginWidthStepper.Value;
        UserMarginWidthValue.Text = UserMarginWidthStepper.Value.ToString();
    }

    private void UserMarginTextTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        syntaxEdit1.Gutter.UserMarginText = tbUserMarginText.Text;
    }

    private void UserMarginForeColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.UserMarginForeColor = UserMarginForeColorPicker.SelectedColor;
    }

    private void UserMarginBkColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.UserMarginBackColor = UserMarginBkColorPicker.SelectedColor;
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
        syntaxEdit1.EditMargin.Position = (int)MarginPositionStepper.Value;
        MarginPositionValue.Text = MarginPositionStepper.Value.ToString();
    }

    private void MarginColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.EditMargin.PenColor = MarginColorPicker.SelectedColor;
        syntaxEdit1.Invalidate();
    }

    private void ColumnsPenColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.EditMargin.ColumnPenColor = ColumnColorPicker.SelectedColor;
        syntaxEdit1.Invalidate();
    }
}