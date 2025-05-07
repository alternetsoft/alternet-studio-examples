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

public partial class LineStylesPage : DemoPage
{
    private readonly int startLine = 44;
    private readonly int endLine = 0;

    private CsParser? csParser1 = new();
    private bool startDebug;
    private int index;
    
    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    private enum KnownImageIndex
    {
        Breakpoint = 11,

        TraceLine,
    }

    public LineStylesPage()
	{
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Source.Lexer = csParser1;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + "newfile.cs");
        syntaxEdit1.Source.HighlightReferences = true;

        var lineStyle = new EditLineStyle
        {
            BackColor = Alternet.Drawing.Color.Black,
            ForeColor = Alternet.Drawing.Color.Yellow,
            Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
            ImageIndex = (int)KnownImageIndex.TraceLine,
        };

        chbLineStyleBeyondEol.IsChecked = (LineStyleOptions.BeyondEol & lineStyle.Options) != 0;
        LineStyleColorPicker.EnsureAddedAndSelect(lineStyle.ForeColor);
        syntaxEdit1.LineStyles.Add(lineStyle);

        // breakpoint style
        syntaxEdit1.LineStyles.Add(new EditLineStyle()
        {
            BackColor = Alternet.Drawing.Color.White,
            ForeColor = Alternet.Drawing.Color.FromArgb(171, 97, 107),
            Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
            ImageIndex = (int)KnownImageIndex.Breakpoint,
        });

        endLine = syntaxEdit1.Lines.Count - 2;

        syntaxEdit1.GutterClick += SyntaxEdit1_GutterClick;
        LineStyleColorPicker.SelectedIndexChanged += LineStyleColorComboBox_SelectedIndexChanged;
        chbLineStyleBeyondEol.CheckedChanged += LineStyleBeyondEolCheckBox_CheckedChanged;
        SetBreakpointButton.Clicked += SetBreakpointTextBoxButton_Click;
        StepOverButton.Clicked += StepOverButton_Click;
        StartButton.Clicked += StartButton_Click;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Line Styles";

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

    private void SyntaxEdit1_GutterClick(object? sender, EventArgs e)
    {
        SetBreakpoint();
    }

    private void Debug()
    {
        index = 0;
        StartButton.Text = startDebug ? "Start" : "Stop";
        StepOverButton.IsEnabled = !startDebug;
    }

    private void Start()
    {
        syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
        syntaxEdit1.Editor.MakeVisible(new System.Drawing.Point(0, startLine + index));
        Debug();
        startDebug = !startDebug;
    }

    private void StepOver()
    {
        if (index < (endLine - startLine))
        {
            if (syntaxEdit1.Source.LineStyles.GetLineStyle(startLine + index) >= 0)
                syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
            index++;
            while ((index < (endLine - startLine))
                && (syntaxEdit1.Source.Lines[startLine + index].Trim() == string.Empty))
                index++;

            syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
            syntaxEdit1.Editor.MakeVisible(new System.Drawing.Point(0, startLine + index));
        }
        else
        {
            syntaxEdit1.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
            Debug();
            startDebug = !startDebug;
        }
    }

    private void SetBreakpoint()
    {
        syntaxEdit1.Source.LineStyles.ToggleLineStyle(syntaxEdit1.Position.Y, 0, 1);
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        Start();
    }

    private void StepOverButton_Click(object? sender, EventArgs e)
    {
        StepOver();
    }

    private void SetBreakpointTextBoxButton_Click(object? sender, EventArgs e)
    {
        SetBreakpoint();
    }

    private void LineStyleBeyondEolCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (syntaxEdit1.LineStyles.Count > 0)
        {
            foreach (IEditLineStyle lineStyle in syntaxEdit1.LineStyles)
            {
                lineStyle.Options = chbLineStyleBeyondEol.IsChecked
                    ? lineStyle.Options | LineStyleOptions.BeyondEol :
                    lineStyle.Options & ~LineStyleOptions.BeyondEol;
            }

            syntaxEdit1.Invalidate();
        }
    }

    private void LineStyleColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (syntaxEdit1.LineStyles.Count > 0)
        {
            IEditLineStyle lineStyle = syntaxEdit1.LineStyles[0];
            lineStyle.ForeColor = LineStyleColorPicker.SelectedColor;
            syntaxEdit1.Invalidate();
        }
    }
}