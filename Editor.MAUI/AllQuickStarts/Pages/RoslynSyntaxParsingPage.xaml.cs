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

public partial class RoslynSyntaxParsingPage : DemoPage
{
    private Alternet.Editor.TextSource.TextSource? csharpSource = new();
    private Alternet.Editor.TextSource.TextSource? vbSource = new();
    private CsParser? csParser1 = new(new CsSolution());
    private VbParser? vbParser1 = new(new VbSolution());

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.newfile";

    static RoslynSyntaxParsingPage()
    {
    }

    public RoslynSyntaxParsingPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        csharpSource.Lexer = csParser1;
        LoadFile(csharpSource, NewFileNameNoExt + ".cs");
        csharpSource.HighlightReferences = true;

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, NewFileNameNoExt + ".vb");
        vbSource.HighlightReferences = true;
        syntaxEdit1.Editor.Source = csharpSource;

        LanguagesPicker.SelectedIndex = 0;

        if (!Alternet.UI.App.IsWindowsOS)
        {
            // Currently loadButton only supports loading on Windows
            // For other platforms, see 
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=macios
            loadButton.IsVisible = false;
        }

        if(HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Roslyn";

    private static readonly string[] vbExtensions = [".vb"];
    private static readonly string[] csExtensions = [".cs"];

    private bool IsVisualBasicSelected => syntaxEdit1.Source == vbSource;

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Source = LanguagesPicker.SelectedIndex switch
        {
            0 => csharpSource,
            1 => vbSource,
            _ => csharpSource,
        } ?? new TextSource();
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
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

    private async void LoadButton_Clicked(object? sender, EventArgs e)
    {
        var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, csExtensions },
                });

        var customFileTypeVb = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, vbExtensions },
                });

        PickOptions options = new()
        {
            PickerTitle = IsVisualBasicSelected
                ? "Please select a C# file" : "Please select a Visual Basic file",
            FileTypes = IsVisualBasicSelected ? customFileTypeVb : customFileTypeCs,
        };

        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        var source = IsVisualBasicSelected ? vbSource : csharpSource;

        LoadFile(source, files.FullPath);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref csharpSource);
        SafeDispose(ref vbSource);
        SafeDispose(ref csParser1);
        SafeDispose(ref vbParser1);
    }
}