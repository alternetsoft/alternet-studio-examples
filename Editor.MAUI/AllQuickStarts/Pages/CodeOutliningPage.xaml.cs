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
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;

using Alternet.UI;
using Alternet.Maui;

using System.Text.RegularExpressions;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts.Pages;

public partial class CodeOutliningPage : DemoPage
{
    private TextSource? csharpSource = new();
    private TextSource? textSource2 = new();
    private CsParser? csParser1 = new();
    private Parser? parser1 = new();
    private readonly IDispatcherTimer timer;
    
    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    public CodeOutliningPage()
	{
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(500);
        timer.Tick += (s, e) => DoTimer();

        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        csharpSource.OptimizedForMemory = false;
        textSource2.OptimizedForMemory = false;
        syntaxEdit1.HyperText.HighlightHyperText = true;// !!

        csharpSource.Lexer = csParser1;
        LoadFile(csharpSource, NewFileNameNoExt + "newfile.cs");
        csharpSource.HighlightReferences = true;

        textSource2.Lexer = parser1;
        LoadFile(textSource2, NewFileNameNoExt + "customOutlining.txt");
        csharpSource.HighlightReferences = true;


        syntaxEdit1.Editor.Source = csharpSource;
        syntaxEdit1.Outlining.AllowOutlining = true;

        OutliningModePicker.SelectedIndex = 0;

        chbAllowOutlining.IsChecked = syntaxEdit1.Outlining.AllowOutlining;
        chbDrawOnGutter.IsChecked
            = (OutlineOptions.DrawOnGutter & syntaxEdit1.Outlining.OutlineOptions) != 0;
        chbDrawLines.IsChecked
            = (OutlineOptions.DrawLines & syntaxEdit1.Outlining.OutlineOptions) != 0;
        chbDrawButtons.IsChecked
            = (OutlineOptions.DrawButtons & syntaxEdit1.Outlining.OutlineOptions) != 0;
        chbShowHints.IsChecked = (OutlineOptions.ShowHints & syntaxEdit1.Outlining.OutlineOptions)
            != 0;

        chbAllowOutlining.CheckedChanged += AllowOutliningCheckBox_CheckedChanged;
        chbDrawOnGutter.CheckedChanged += DrawOnGutterCheckBox_CheckedChanged;
        chbDrawLines.CheckedChanged += DrawLinesCheckBox_CheckedChanged;
        chbDrawButtons.CheckedChanged += DrawButtonsCheckBox_CheckedChanged;
        chbShowHints.CheckedChanged += ShowHintsCheckBox_CheckedChanged;
        syntaxEdit1.SourceStateChanged += SyntaxEdit1_SourceStateChanged;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Code Outlining";

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref csharpSource);
        SafeDispose(ref textSource2);
        SafeDispose(ref csParser1);
        SafeDispose(ref parser1);
    }

    private void OutliningModePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateOutlining(OutliningModePicker.SelectedIndex == 0);
    }

    public void LoadFile(TextSource source, string url)
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

    private void SyntaxEdit1_SourceStateChanged(object? sender, NotifyEventArgs e)
    {
        object source1 = (object)syntaxEdit1.Source;
        if (source1.Equals(textSource2))
        {
            if ((e.State & NotifyState.Edit) != 0)
                timer.Start();
        }
    }

    private void ShowHintsCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Outlining.OutlineOptions = chbShowHints.IsChecked
            ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.ShowHints
            : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.ShowHints;
    }

    private void DrawButtonsCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Outlining.OutlineOptions = chbDrawButtons.IsChecked
            ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawButtons
            : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawButtons;
    }

    private void DrawLinesCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Outlining.OutlineOptions = chbDrawLines.IsChecked
            ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawLines
            : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawLines;
    }

    private void DrawOnGutterCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Outlining.OutlineOptions = chbDrawOnGutter.IsChecked
            ? syntaxEdit1.Outlining.OutlineOptions | OutlineOptions.DrawOnGutter
        : syntaxEdit1.Outlining.OutlineOptions & ~OutlineOptions.DrawOnGutter;
    }

    private void AllowOutliningCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Outlining.AllowOutlining = chbAllowOutlining.IsChecked;
        if (chbAllowOutlining.IsChecked && (OutliningModePicker.SelectedIndex != 0))
            DoCustomOutlining();
    }

    private void DoTimer()
    {
        if(!IsDisposed)
            DoCustomOutlining();
        timer.Stop();
    }

    private System.Drawing.Point PrevPosition(System.Drawing.Point position)
    {
        System.Drawing.Point pos = position;
        if (pos.Y > 0)
        {
            pos.Y--;
            pos.X = Math.Max(0, syntaxEdit1.Strings[pos.Y].Length - 1);
        }
        else
            pos.X--;
        return pos;
    }

    [GeneratedRegex(@"\[.*\]", RegexOptions.Singleline)]
    private static partial Regex GeneratedRegex();

    private void DoCustomOutlining()
    {
        System.Drawing.Point oldPos = syntaxEdit1.Position;
        syntaxEdit1.Source.BeginUpdate();
        try
        {
            /*Regex regexp = new(
                @"\[.*\]",
                System.Text.RegularExpressions.RegexOptions.Singleline);*/

            if (syntaxEdit1.Editor.Find(
                @"\[.*\]",
                SearchOptions.EntireScope | SearchOptions.RegularExpressions, GeneratedRegex()))
            {
                IList<Alternet.Common.IRange> ranges = new List<Alternet.Common.IRange>();
                System.Drawing.Point start = syntaxEdit1.Position;
                while (syntaxEdit1.Editor.FindNext())
                {
                    ranges.Add(new OutlineRange(start, PrevPosition(syntaxEdit1.Position), 0, "..."));
                    start = syntaxEdit1.Position;
                }

                var pt = new System.Drawing.Point(
                    syntaxEdit1.Lines[syntaxEdit1.Lines.Count - 1].Length,
                    syntaxEdit1.Lines.Count - 1);

                ranges.Add(new OutlineRange(start, pt, 0, "..."));
                syntaxEdit1.Outlining.SetOutlineRanges(ranges, true);
            }

            syntaxEdit1.Selection.Clear();
        }
        finally
        {
            syntaxEdit1.Editor.MoveTo(oldPos);
            syntaxEdit1.Source.EndUpdate();
        }
    }

    private void UpdateOutlining(bool automatic)
    {
        syntaxEdit1.Source = (automatic ? csharpSource : textSource2) ?? new TextSource();
        if (!automatic)
            DoCustomOutlining();
    }
}