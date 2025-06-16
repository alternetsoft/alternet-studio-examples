#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using AllQuickStarts.Pages;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Alternet.UI;
using Alternet.Maui;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class VisualThemePage : DemoPage
{
    private Alternet.Editor.TextSource.TextSource? csharpSource = new();
    private CsParser? csParser1 = new(new CsSolution());
    private IList<string> themeList = new List<string>();

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.newfile";

    static VisualThemePage()
    {
    }

    public VisualThemePage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        csharpSource.Lexer = csParser1;
        LoadFile(csharpSource, NewFileNameNoExt + ".cs");
        csharpSource.HighlightReferences = true;

        syntaxEdit1.Editor.Source = csharpSource;
        InitializeVisualThemePicker();
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Visual Theme";

    private void InitializeVisualThemePicker()
    {
        foreach (string item in Enum.GetNames(typeof(VisualThemeType)))
        {
            themeList.Add(string.Compare(item, "Custom", true) == 0 ? "TextMate" : item);
        }

        VisualThemePicker.ItemsSource = themeList.ToArray();
        VisualThemePicker.SelectedIndex = (int)VisualThemeType.Auto;
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options
            = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
    }

    public static void LoadFile(Alternet.Editor.TextSource.TextSource? source, string url)
    {
        if (source is null)
            return;

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

    private void KeyboardButton_Clicked(object? sender, EventArgs e)
    {
        syntaxEdit1.ToggleKeyboard();
    }

    private void VisualThemePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.VisualThemeType = (VisualThemeType)VisualThemePicker.SelectedIndex;
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref csharpSource);
        SafeDispose(ref csParser1);
    }
}