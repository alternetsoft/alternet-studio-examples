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
using Alternet.Syntax.Parsers.Advanced;

using Alternet.UI;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class PowerFxContextPage : DisposableContentPage
{
    private readonly JSONParser jsonParser = new JSONParser();

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    static PowerFxContextPage()
    {
    }

    public PowerFxContextPage()
    {
        DemoTitleView titleView = new("PowerFx Context", this);
        NavigationPage.SetTitleView(this, titleView);
        titleView.Label.HorizontalTextAlignment = TextAlignment.Center;

        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        if (Alternet.UI.App.IsDesktopDevice)
        {
        }
        else
        {
        }

        this.Loaded += (s, e) =>
        {
            syntaxEdit2.IsVisible = true;
            this.ForceLayout();
        };

        if (!Alternet.UI.App.IsDesktopDevice || Alternet.UI.DebugUtils.IsDebuggerAttached)
        {
            Alternet.UI.MauiUtils.SetButtonImage(
                keyboardButton,
                Alternet.UI.KnownSvgImages.ImgKeyboard,
                32);
            keyboardButton.Aspect = Aspect.Center;
        }
        else
        {
            keyboardButton.IsVisible = false;
        }

        InitEdit();

        LoadFile(syntaxEdit2.Source, NewFileNameNoExt + "Record.fx.json");

        BindingContext = this;
    }

    public event EventHandler? OkClicked;

    public string GlobalContext  => syntaxEdit2.Text;

    private void KeyboardButton_Clicked(object? sender, EventArgs e)
    {
        syntaxEdit2.ToggleKeyboard();
    }

    private void InitEdit()
    {
        syntaxEdit2.Selection.Options = syntaxEdit2.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit2.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
        syntaxEdit2.Outlining.AllowOutlining = true;
        syntaxEdit2.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit2.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;

        syntaxEdit2.Source.FileName = "test.js";
        syntaxEdit2.Lexer = jsonParser;
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

    private void OkButton_Clicked(object? sender, EventArgs e)
    {
        OkClicked?.Invoke(this, new EventArgs());
        Navigation.RemovePage(this);
    }

    private void CancelButton_Clicked(object? sender, EventArgs e)
    {
        Navigation.RemovePage(this);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
    }
}