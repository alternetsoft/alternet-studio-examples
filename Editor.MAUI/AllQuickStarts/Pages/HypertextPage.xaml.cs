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

public partial class HypertextPage : DemoPage
{
    private CsParser? csParser1 = new();

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.newfile";

    public HypertextPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        syntaxEdit1.HyperText.HighlightHyperText = true;
        syntaxEdit1.Outlining.AllowOutlining = true;

        syntaxEdit1.Source.Lexer = csParser1;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + ".cs");
        syntaxEdit1.Source.HighlightReferences = true;

        chbHighlightUrls.IsChecked = syntaxEdit1.HyperText.HighlightHyperText;

        UrlColorPicker.EnsureAddedAndSelect(syntaxEdit1.HyperText.UrlColor);

        List<Alternet.Drawing.FontStyle> fontStyles =
        [
            Alternet.Drawing.FontStyle.Regular,
            Alternet.Drawing.FontStyle.Bold,
            Alternet.Drawing.FontStyle.Italic,
        ];

        syntaxEdit1.HyperText.UrlStyle = Alternet.Drawing.FontStyle.Bold;

        FontStylePicker.ItemsSource = fontStyles;
        FontStylePicker.SelectedItem = Alternet.Drawing.FontStyle.Bold;

        chbHighlightUrls.CheckedChanged += HighlightUrlsCheckBox_CheckedChanged;
        chbCustomHypertext.CheckedChanged += CustomHypertextCheckBox_CheckedChanged;
        syntaxEdit1.JumpToUrl += SyntaxEdit1_JumpToUrl;
        syntaxEdit1.CheckHyperText += SyntaxEdit1_CheckHyperText;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Hypertext";

    public void LoadFile(Alternet.Editor.TextSource.ITextSource source, string url)
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

    private void SyntaxEdit1_JumpToUrl(object? sender, UrlJumpEventArgs e)
    {
        if (chbCustomHypertext.IsChecked)
        {
            //statusBar.Text = $"Url '{e.Text}' clicked {clickCounter++} times";
            e.Handled = true;
        }
    }

    private void SyntaxEdit1_CheckHyperText(object? sender, HyperTextEventArgs e)
    {
        if (chbCustomHypertext.IsChecked)
            e.IsHyperText = e.Text.Contains('<', StringComparison.CurrentCulture);
    }

    private void HighlightUrlsCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.HyperText.HighlightHyperText = chbHighlightUrls.IsChecked;
        UpdateUrlTable();
    }

    private void CustomHypertextCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateUrlTable();
    }

    private void UpdateUrlTable()
    {
        var hs = ((TextSource)syntaxEdit1.Source).UrlTable;
        if (hs != null)
        {
            if (chbCustomHypertext.IsChecked)
            {
                hs['<'] = true;
                hs['>'] = false;
            }
            else
            {
                hs.Remove('<');
                hs.Remove('>');
            }
        }

        syntaxEdit1.Source.Notification(syntaxEdit1.Lexer, EventArgs.Empty);
        syntaxEdit1.Invalidate();
    }

    private void UrlColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.HyperText.UrlColor = UrlColorPicker.SelectedColor;
    }

    private void FontStylePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!(FontStylePicker.SelectedItem is null) && Enum.TryParse(FontStylePicker.SelectedItem.ToString(), out Alternet.Drawing.FontStyle fontStyle))
        {
            syntaxEdit1.HyperText.UrlStyle = fontStyle;
        }
    }
}