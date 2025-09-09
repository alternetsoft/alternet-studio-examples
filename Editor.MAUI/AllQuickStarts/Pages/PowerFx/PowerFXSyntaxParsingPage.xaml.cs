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
using Alternet.Syntax.Parsers.PowerFx;

using Alternet.UI;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class PowerFxSyntaxParsingPage : DemoPage
{
    private readonly PowerFxParser fxParser = new PowerFxParser();
    private TextSource? source = new TextSource();
    private bool evaluateQuickInfo = true;

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    static PowerFxSyntaxParsingPage()
    {
    }

    public PowerFxSyntaxParsingPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();
        syntaxEdit1.Source = this.source;
        fxParser.EvaluateQuickInfo = this.evaluateQuickInfo;
        LoadFile(source, NewFileNameNoExt + "Record.fx");

        var url = NewFileNameNoExt + "Record.fx.json";
        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);
        if (stream != null)
        {
            TextReader tr = new StreamReader(stream);
            ApplyGlobalContext(tr.ReadToEnd());
        }

        EvaluateQuickInfo.CheckedChanged += EvaluateQuickInfo_CheckedChanged;

        if (!Alternet.UI.App.IsWindowsOS)
        {
            // Currently loadButton only supports loading on Windows
            // For other platforms, see 
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=macios
            loadButton.IsVisible = false;
        }
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "PowerFx";

    private void EvaluateQuickInfo_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (fxParser != null)
            fxParser.EvaluateQuickInfo = EvaluateQuickInfo.IsChecked;
    }

    private static readonly string[] fxExtensions = [".fx"];

    private bool ApplyGlobalContext(string s)
    {
        if (!PowerFxParser.IsValidJson(s))
            return false;

        if (string.IsNullOrEmpty(s))
        {
            fxParser.GlobalContextAsJson = null;
        }
        else
            fxParser.GlobalContextAsJson = s;
        return true;
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;

        if(source is not null)
            source.Lexer = fxParser;
        syntaxEdit1.Spelling.SpellColor = Alternet.Drawing.Color.Navy;
        syntaxEdit1.Outlining.AllowOutlining = true;
    }

    public static void LoadFile(Alternet.Editor.TextSource.ITextSource? source, string url)
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

    private async void EvaluateButton_Clicked(object? sender, EventArgs e)
    {
        var r = fxParser.Evaluator.Eval(syntaxEdit1.Text);
        var s = PowerFxEvaluator.EvalResultToString(r);
        await DisplayAlert("Evaluation Result", s, "OK");
    }

    private async void EditContextButton_Clicked(object? sender, EventArgs e)
    {
        PowerFxContextPage page = new PowerFxContextPage();
        page.OkClicked += Page_OkClicked;
        await Dispatcher.DispatchAsync(() =>
        {
            return Navigation.PushAsync(page);
        });
    }

    private void Page_OkClicked(object? sender, EventArgs e)
    {
        if (sender is PowerFxContextPage page)
        {
            ApplyGlobalContext(page.GlobalContext);
        }
    }

    private async void LoadButton_Clicked(object? sender, EventArgs e)
    {
        var customFileTypeFx = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, fxExtensions },
                });

        PickOptions options = new()
        {
            PickerTitle = "Please select a PowerFx file",
            FileTypes = customFileTypeFx,
        };

        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        LoadFile(source, files.FullPath);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref source);
    }
}