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
using Alternet.Syntax.Parsers.Roslyn;

using Alternet.UI;
using Alternet.Maui;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts.Pages;

public partial class GutterPage : DemoPage
{
    private CsParser? csParser1 = new();

    private System.Drawing.Color gradientBeginColor = System.Drawing.Color.Blue;
    private System.Drawing.Color gradientEndColor = System.Drawing.Color.White;
    
    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.newfile";

    public GutterPage()
	{
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Lexer = csParser1;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + ".cs");
        syntaxEdit1.HighlightReferences = true;

        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers;

        chbDisplayGutter.IsChecked = syntaxEdit1.Gutter.Visible;
        GutterWidthStepper.Maximum = (int)syntaxEdit1.Width;
        GutterWidthStepper.Value = (int)syntaxEdit1.Gutter.Width;
        GutterWidthValue.Text = syntaxEdit1.Gutter.Width.ToString();
        
        GutterColorPicker.EnsureAddedAndSelect(syntaxEdit1.Gutter.BrushColor);
        LineColorPicker.EnsureAddedAndSelect(syntaxEdit1.Gutter.PenColor);
        StartColorPicker.EnsureAddedAndSelect(gradientBeginColor);
        EndColorPicker.EnsureAddedAndSelect(gradientEndColor);

        chbDisplayLineNumbers.IsChecked
            = (GutterOptions.PaintLineNumbers & syntaxEdit1.Gutter.Options) != 0;
        chbLinesOnGutter.IsChecked
            = (GutterOptions.PaintLinesOnGutter & syntaxEdit1.Gutter.Options) != 0;
        StartStepper.Maximum = 10000;
        StartStepper.Value = syntaxEdit1.Gutter.LineNumbersStart;
        AlignPicker.SelectedIndex = (int)syntaxEdit1.Gutter.LineNumbersAlignment;
        LeftIndentStepper.Maximum = 10000;
        LeftIndentStepper.Value = (int)syntaxEdit1.Gutter.LineNumbersLeftIndent;
        LeftIndentValue.Text = syntaxEdit1.Gutter.LineNumbersLeftIndent.ToString();
        RightIndentStepper.Maximum = 10000;
        RightIndentStepper.Value = (int)syntaxEdit1.Gutter.LineNumbersRightIndent;
        RightIndentValue.Text = syntaxEdit1.Gutter.LineNumbersRightIndent.ToString();
        StartStepper.Maximum = 10000;
        StartStepper.Value = (int)syntaxEdit1.Gutter.LineNumbersStart;
        StartValue.Text = syntaxEdit1.Gutter.LineNumbersStart.ToString();
        
        ForeColorPicker.EnsureAddedAndSelect(syntaxEdit1.Gutter.LineNumbersForeColor);
        BackColorPicker.EnsureAddedAndSelect(syntaxEdit1.Gutter.LineNumbersBackColor);

        chbDisplayGutter.CheckedChanged += ShowGutterCheckBox_CheckedChanged;
        chbUseGradient.CheckedChanged += GradientGutterCheckBox_CheckedChanged;

        chbDisplayLineNumbers.CheckedChanged += DisplayLineNumbers_CheckedChanged;
        chbLinesOnGutter.CheckedChanged += LinesOnGutter_CheckedChanged;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Gutter";

#pragma warning disable
    public void LoadFile(Alternet.Editor.TextSource.ITextSource source, string url)
#pragma warning restore
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

    private void DisplayLineNumbers_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.Options = chbDisplayLineNumbers.IsChecked
            ? syntaxEdit1.Gutter.Options | GutterOptions.PaintLineNumbers
            : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLineNumbers;
    }

    private void GradientGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Transparent = chbUseGradient.IsChecked;
        syntaxEdit1.Invalidate();
    }

    private void LinesOnGutter_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.Options = chbLinesOnGutter.IsChecked
            ? syntaxEdit1.Gutter.Options | GutterOptions.PaintLinesOnGutter
            : syntaxEdit1.Gutter.Options & ~GutterOptions.PaintLinesOnGutter;
    }

    private void ShowGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.Visible = chbDisplayGutter.IsChecked;
    }

    private void GutterColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
	{
        syntaxEdit1.Gutter.BrushColor = GutterColorPicker.SelectedColor;
    }

    private void LineColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.PenColor = LineColorPicker.SelectedColor;
    }

    private void StartColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (StartColorPicker.SelectedColor is null)
            return;
        gradientBeginColor = StartColorPicker.SelectedColor;
        if (chbUseGradient.IsChecked)
            syntaxEdit1.Invalidate();
    }

    private void EndColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (EndColorPicker.SelectedColor is null)
            return;
        gradientEndColor = EndColorPicker.SelectedColor;
        if (chbUseGradient.IsChecked)
            syntaxEdit1.Invalidate();
    }

    private void ForeColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.LineNumbersForeColor = ForeColorPicker.SelectedColor;
    }

    private void BackColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Gutter.LineNumbersBackColor = BackColorPicker.SelectedColor;
    }

    private void GutterWidthValueChanged(object sender, ValueChangedEventArgs e)
    {
        syntaxEdit1.Gutter.Width = GutterWidthStepper.Value;
        GutterWidthValue.Text = syntaxEdit1.Gutter.Width.ToString();
    }

    private void LeftIndentValueChanged(object sender, ValueChangedEventArgs e)
    {
        syntaxEdit1.Gutter.LineNumbersLeftIndent = LeftIndentStepper.Value;
        LeftIndentValue.Text = syntaxEdit1.Gutter.LineNumbersLeftIndent.ToString();
    }

    private void RightIndentValueChanged(object sender, ValueChangedEventArgs e)
    {
        syntaxEdit1.Gutter.LineNumbersRightIndent = RightIndentStepper.Value;
        RightIndentValue.Text = syntaxEdit1.Gutter.LineNumbersRightIndent.ToString();

    }

    private void StartValueChanged(object sender, ValueChangedEventArgs e)
    {
        syntaxEdit1.Gutter.LineNumbersStart = (int)StartStepper.Value;
        StartValue.Text = syntaxEdit1.Gutter.LineNumbersStart.ToString();
    }

    private void AlignPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (AlignPicker.SelectedItem is null)
            return;
        syntaxEdit1.Gutter.LineNumbersAlignment
            = (Alternet.Drawing.StringAlignment)AlignPicker.SelectedIndex;
    }
}