#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Syntax.Parsers.Roslyn;
using WeCantSpell.Hunspell;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Lexer;

using Alternet.UI;
using Alternet.Maui;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts.Pages;

public partial class MiscellaneousPage : DemoPage
{
    public const string BackgroundStyleSolid = "Solid Color";
    public const string BackgroundStyleGradient = "Gradient";

    private readonly SpellChecker spellChecker = new();

    private CsParser? csParser1 = new();
    private Alternet.Drawing.Color gradientBeginColor = Alternet.Drawing.Color.Blue;
    private Alternet.Drawing.Color gradientEndColor = Alternet.Drawing.Color.White;
    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    public MiscellaneousPage()
	{
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Lexer = csParser1;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + "spell.txt");

        // background
        BackgroundStylePicker.SelectedIndex = 0;
        BackgroundColorPicker.EnsureAddedAndSelect(syntaxEdit1.Editor.BackColor);
        GradientBeginColorPicker.EnsureAddedAndSelect(gradientBeginColor);
        GradientEndColorPicker.EnsureAddedAndSelect(gradientEndColor);

        // braces
        syntaxEdit1.Braces.BracesOptions = BracesOptions.Highlight;
        chbHighlightBraces.IsChecked
            = (BracesOptions.Highlight & syntaxEdit1.Braces.BracesOptions) != 0;
        chbTempHighlightBraces.IsChecked
            = (BracesOptions.TempHighlight & syntaxEdit1.Braces.BracesOptions) != 0;
        chbHighlightBounds.IsChecked
            = (BracesOptions.HighlightBounds & syntaxEdit1.Braces.BracesOptions) != 0;
        BracesColorPicker.EnsureAddedAndSelect(syntaxEdit1.Braces.BackColor);

        List<Alternet.Drawing.FontStyle> fontStyles =
        [
            Alternet.Drawing.FontStyle.Regular,
            Alternet.Drawing.FontStyle.Bold,
            Alternet.Drawing.FontStyle.Italic,
        ];

        FontStylePicker.ItemsSource = fontStyles;
        FontStylePicker.SelectedIndex
            = FontStylePicker.Items.IndexOf(syntaxEdit1.Editor.Braces.FontStyle.ToString());

        // spelling
        chbCheckSpelling.IsChecked = syntaxEdit1.Spelling.CheckSpelling;
        SpellColorPicker.EnsureAddedAndSelect(syntaxEdit1.Spelling.SpellColor);
        spellChecker.CheckSpelling(syntaxEdit1.Editor, chbCheckSpelling.IsChecked);

        // whitespace
        chbWhiteSpaceVisible.IsChecked = syntaxEdit1.WhiteSpace.Visible;
        SymbolColorPicker.EnsureAddedAndSelect(syntaxEdit1.WhiteSpace.SymbolColor);

        GradientBeginColorPicker.SelectedIndexChanged += GradientBeginColor_SelectedIndexChanged;
        GradientEndColorPicker.SelectedIndexChanged += GradientEndColor_SelectedIndexChanged;
        chbSeparateLines.IsChecked
            = (SeparatorOptions.SeparateLines & syntaxEdit1.LineSeparator.Options) != 0;

        chbCheckSpelling.CheckedChanged += CheckSpellingCheckBox_CheckedChanged;
        SpellColorPicker.SelectedIndexChanged += SpellColorComboBox_SelectedIndexChanged;
        chbTransparent.CheckedChanged += TransparentCheckBox_CheckedChanged;
        BackgroundStylePicker.SelectedIndexChanged += BackgroundStyleComboBox_SelectedIndexChanged;
        chbHighlightBraces.CheckedChanged += HighlightBracesCheckBoxTextBox_CheckedChanged;
        chbHighlightBounds.CheckedChanged += HighlightBoundsCheckBoxTextBox_CheckedChanged;
        chbTempHighlightBraces.CheckedChanged += TempHighlightBracesCheckBoxTextBox_CheckedChanged;
        FontStylePicker.SelectedIndexChanged += FontStyleComboBox_SelectedIndexChanged;
        BracesColorPicker.SelectedIndexChanged += BracesColorComboBox_SelectedIndexChanged;
        chbWhiteSpaceVisible.CheckedChanged += WhiteSpaceVisibleCheckBox_CheckedChanged;
        SymbolColorPicker.SelectedIndexChanged += SymbolColorComboBox_SelectedIndexChanged;
        chbSeparateLines.CheckedChanged += SeparateLinesCheckBox_CheckedChanged;
        BackgroundColorPicker.SelectedIndexChanged += BackgroundColor_SelectedIndexChanged;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Miscellaneous";

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

    private void BackgroundColor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateEditor();
        syntaxEdit1.Invalidate();
    }

    private void GradientEndColor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        gradientEndColor = GradientEndColorPicker?.SelectedColor ?? Alternet.Drawing.Color.Blue;
        UpdateEditor();
        syntaxEdit1.Invalidate();
    }

    private void GradientBeginColor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        gradientBeginColor = GradientBeginColorPicker?.SelectedColor ?? Alternet.Drawing.Color.White;
        UpdateEditor();
        syntaxEdit1.Invalidate();
    }

    private void CheckSpellingCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        spellChecker.CheckSpelling(syntaxEdit1.Editor, chbCheckSpelling.IsChecked);
    }

    private void SpellColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Spelling.SpellColor = SpellColorPicker.SelectedColor;
    }

    private void TransparentCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Transparent = chbTransparent.IsChecked;
    }

    private void BackgroundStyleComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateEditor();
        syntaxEdit1.Invalidate();
    }

    private void UpdateEditor()
    {
        if (syntaxEdit1 == null)
            return;

        syntaxEdit1.Editor.BackgroundColor = BackgroundColorPicker.SelectedColor;

        switch (BackgroundStylePicker.SelectedItem.ToString())
        {
            case BackgroundStyleSolid:
                syntaxEdit1.Editor.Background = null;
                break;
            case BackgroundStyleGradient:

                Alternet.Drawing.GradientStop[] gradientStops =
                {
                        new(gradientBeginColor, 0),
                        new(gradientEndColor, 1),
                    };

                var gradientBrush =
                    new Alternet.Drawing.LinearGradientBrush(
                        new PointD(0, 0),
                        new PointD(0, syntaxEdit1.ClientHeight),
                        gradientStops);

                syntaxEdit1.Editor.Background = gradientBrush;
                break;
        }
    }

    private void HighlightBracesCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Braces.BracesOptions
            = chbHighlightBraces.IsChecked ? syntaxEdit1.Braces.BracesOptions
            | BracesOptions.Highlight : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.Highlight;
    }

    private void HighlightBoundsCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Braces.BracesOptions = chbHighlightBounds.IsChecked
            ? syntaxEdit1.Braces.BracesOptions | BracesOptions.HighlightBounds
            : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.HighlightBounds;
    }

    private void TempHighlightBracesCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Braces.BracesOptions = chbTempHighlightBraces.IsChecked
            ? syntaxEdit1.Braces.BracesOptions | BracesOptions.TempHighlight
            : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.TempHighlight;
    }

    private void FontStyleComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        object obj = Enum.Parse<FontStyle>(
            FontStylePicker.SelectedItem?.ToString() ?? FontStyle.Regular.ToString());
        if ((obj != null) && (obj is FontStyle style))
            syntaxEdit1.Braces.FontStyle = style;
    }

    private void BracesColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Braces.BackColor = BracesColorPicker.SelectedColor;
    }

    private void WhiteSpaceVisibleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.WhiteSpace.Visible = chbWhiteSpaceVisible.IsChecked;
    }

    private void SymbolColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.WhiteSpace.SymbolColor = SymbolColorPicker.SelectedColor;
    }

    private void SeparateLinesCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.LineSeparator.Options
            = chbSeparateLines.IsChecked ? syntaxEdit1.LineSeparator.Options
            | SeparatorOptions.SeparateLines
            : syntaxEdit1.LineSeparator.Options & ~SeparatorOptions.SeparateLines;
    }

    public class SpellChecker
    {
        private readonly WordList? wordList = null;

        public SpellChecker()
        {
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                if (!File.Exists(Path.GetFullPath(Path.Combine(dir, "en_US.aff"))))
                    dir = AppDomain.CurrentDomain.BaseDirectory + @"/../../../";
                wordList = WordList.CreateFromFiles(
                    Path.GetFullPath(Path.Combine(dir, "en_US.dic")),
                    Path.GetFullPath(Path.Combine(dir, "en_US.aff")));
            }
            catch
            {
            }
        }

        public void CheckSpelling(SyntaxEdit edit, bool spell)
        {
            edit.Spelling.CheckSpelling = spell;
            if (spell)
                edit.Spelling.WordSpell += new WordSpellEvent(WordSpell);
            else
                edit.Spelling.WordSpell -= new WordSpellEvent(WordSpell);
        }

        private void WordSpell(object sender, WordSpellEventArgs e)
        {
            ITextSource source = (ITextSource)sender;
            bool correct = wordList != null ? wordList.Check(e.Text) : true;
            if (source.Lexer != null)
            {
                LexToken tok = (LexToken)(e.ColorStyle.Data - 1);
                if ((tok == LexToken.String) || (tok == LexToken.Comment) || (tok == LexToken.XmlComment))
                    e.Correct = (e.Text.Length <= 1) || correct;
            }
            else
                e.Correct = (e.Text.Length <= 1) || correct;
        }
    }
}