#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using WeCantSpell.Hunspell;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace Miscellaneous
{
    public partial class Form1 : Window
    {
        public const string BackgroundStyleSolid = "Solid Color";
        public const string BackgroundStyleImage = "Image";
        public const string BackgroundStyleGradient = "Gradient";

        private readonly CsParser csParser1 = new(new CsSolution());
        private readonly SpellChecker spellChecker = new();

        private Color gradientBeginColor;
        private Color gradientEndColor;

        public Form1()
        {
            if (IsDarkBackground)
            {
                gradientBeginColor = Color.RebeccaPurple;
                gradientEndColor = DefaultColors.ControlBackColor;
            }
            else
            {
                gradientBeginColor = Color.PaleTurquoise;
                gradientEndColor = DefaultColors.ControlBackColor;
            }

            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            FileInfo fileInfo = new(DemoUtils.GetResourceFileFullPath(@"Editor/Text/spell.txt"));

            syntaxEdit1.Outlining.AllowOutlining = true;

            // background
            cbBackgroundStyle.EnumType = typeof(DemoBackgroundStyle);
            cbBackgroundStyle.Value = DemoBackgroundStyle.Solid;

            cbBackgroundColor.Select(syntaxEdit1.BackColor);
            cbGradientBeginColor.Select(gradientBeginColor);
            cbGradientEndColor.Select(gradientEndColor);

            // braces
            syntaxEdit1.Braces.BracesOptions = BracesOptions.Highlight;
            chbHighlightBraces.IsChecked
                = (BracesOptions.Highlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbUseRoundRect.IsChecked = syntaxEdit1.Braces.UseRoundRect;
            chbTempHighlightBraces.IsChecked
                = (BracesOptions.TempHighlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbHighlightBounds.IsChecked
                = (BracesOptions.HighlightBounds & syntaxEdit1.Braces.BracesOptions) != 0;
            cbBracesColor.Select(syntaxEdit1.Braces.BackColor);

            syntaxEdit1.Braces.UseThemeBracesColors = false;
            syntaxEdit1.Braces.UseThemeBracesOptions = false;

            cbFontStyle.ExcludeValues = new[]
            {
                FontStyle.Italic,
                FontStyle.Underline,
                FontStyle.Strikeout
            };

            cbFontStyle.EnumType = typeof(FontStyle);
            cbFontStyle.Value = syntaxEdit1.Braces.FontStyle;

            // spelling
            chbCheckSpelling.IsChecked = syntaxEdit1.Spelling.CheckSpelling;
            cbSpellColor.Select(syntaxEdit1.Spelling.SpellColor);
            spellChecker.CheckSpelling(syntaxEdit1, chbCheckSpelling.IsChecked);

            // whitespace
            chbWhiteSpaceVisible.IsChecked = syntaxEdit1.WhiteSpace.Visible;
            cbSymbolColor.Select(syntaxEdit1.WhiteSpace.SymbolColor);

            cbGradientBeginColor.ValueChanged += GradientBeginColor_SelectedIndexChanged;
            cbGradientEndColor.ValueChanged += GradientEndColor_SelectedIndexChanged;
            chbSeparateLines.IsChecked
                = (SeparatorOptions.SeparateLines & syntaxEdit1.LineSeparator.Options) != 0;

            chbCheckSpelling.CheckedChanged += CheckSpellingCheckBox_CheckedChanged;
            cbSpellColor.ValueChanged += SpellColorComboBox_SelectedIndexChanged;
            chbTransparent.CheckedChanged += TransparentCheckBox_CheckedChanged;
            cbBackgroundStyle.ValueChanged += BackgroundStyleComboBox_SelectedIndexChanged;

            chbUseRoundRect.CheckedChanged += UseRoundRectCheckBox_CheckedChanged;
            cbFontStyle.ValueChanged += FontStyleComboBox_SelectedIndexChanged;
            chbWhiteSpaceVisible.CheckedChanged += WhiteSpaceVisibleCheckBox_CheckedChanged;
            cbSymbolColor.ValueChanged += SymbolColorComboBox_SelectedIndexChanged;
            chbSeparateLines.CheckedChanged += SeparateLinesCheckBox_CheckedChanged;
            cbBackgroundColor.ValueChanged += CbBackgroundColor_SelectedIndexChanged;

            chbHighlightBraces.CheckedChanged += HighlightBracesCheckBoxTextBox_CheckedChanged;
            chbHighlightBounds.CheckedChanged += HighlightBoundsCheckBoxTextBox_CheckedChanged;
            chbTempHighlightBraces.CheckedChanged += TempHighlightBracesCheckBoxTextBox_CheckedChanged;
            cbBracesColor.ValueChanged += BracesColorComboBox_SelectedIndexChanged;

            ActiveControl = syntaxEdit1;
            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;
            lbDescription.WordWrap = true;

            syntaxEdit1.Text = "Text loading...";

            FormUtils.BindShown(this, () =>
            {
                if (textSource.LoadOrAddNotFound(fileInfo.FullName))
                {
                    textSource.Lexer = csParser1;
                }
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        public enum DemoBackgroundStyle
        { 
            Solid,
            Gradient,
        }

        private void CbBackgroundColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateEditor();
            syntaxEdit1.Invalidate();
        }

        private void GradientEndColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            gradientEndColor = cbGradientEndColor?.Value ?? Color.Blue;
            UpdateEditor();
            syntaxEdit1.Invalidate();
        }

        private void GradientBeginColor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            gradientBeginColor = cbGradientBeginColor?.Value ?? Color.White;
            UpdateEditor();
            syntaxEdit1.Invalidate();
        }

        private void CheckSpellingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            spellChecker.CheckSpelling(syntaxEdit1, chbCheckSpelling.IsChecked);
        }

        private void SpellColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Spelling.SpellColor = cbSpellColor.Value;
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

            syntaxEdit1.BackgroundColor = cbBackgroundColor.Value;

            switch ((DemoBackgroundStyle?)cbBackgroundStyle.Value ?? DemoBackgroundStyle.Solid)
            {
                case DemoBackgroundStyle.Solid:
                default:
                    syntaxEdit1.Background = null;
                    break;
                case DemoBackgroundStyle.Gradient:

                    GradientStop[] gradientStops =
                    {
                        new(gradientBeginColor, 0),
                        new(gradientEndColor, 1),
                    };

                    var gradientBrush =
                        new LinearGradientBrush(
                            new PointD(0, 0),
                            new PointD(0, syntaxEdit1.ClientHeight),
                            gradientStops);

                    syntaxEdit1.Background = gradientBrush;
                    break;
            }
        }

        private void UpdateBraces()
        {
            var bracesOptions = syntaxEdit1.Braces.BracesOptions;

            bracesOptions = chbHighlightBraces.IsChecked ? bracesOptions
                | BracesOptions.Highlight : bracesOptions & ~BracesOptions.Highlight;

            bracesOptions = chbHighlightBounds.IsChecked
                ? bracesOptions | BracesOptions.HighlightBounds
                : bracesOptions & ~BracesOptions.HighlightBounds;

            bracesOptions = chbTempHighlightBraces.IsChecked
                ? bracesOptions | BracesOptions.TempHighlight
                : bracesOptions & ~BracesOptions.TempHighlight;

            syntaxEdit1.Braces.BracesOptions = bracesOptions;
            syntaxEdit1.Braces.UseRoundRect = chbUseRoundRect.IsChecked;
            syntaxEdit1.Braces.ForeColor = chbUseRoundRect.IsChecked ? Color.Gray : Color.Black;
            syntaxEdit1.Braces.BackColor = cbBracesColor.Value;

            syntaxEdit1.Invalidate();
        }

        private void HighlightBracesCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateBraces();
        }

        private void UseRoundRectCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateBraces();
        }

        private void HighlightBoundsCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateBraces();
        }

        private void TempHighlightBracesCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateBraces();
        }

        private void FontStyleComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var fontStyle = cbFontStyle.ValueAs<FontStyle>();
            syntaxEdit1.Braces.FontStyle = fontStyle;
        }

        private void BracesColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateBraces();
        }

        private void WhiteSpaceVisibleCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.WhiteSpace.Visible = chbWhiteSpaceVisible.IsChecked;
        }

        private void SymbolColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.WhiteSpace.SymbolColor = cbSymbolColor.Value;
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
                    if ((tok == LexToken.String) || (tok == LexToken.Comment)
                        || (tok == LexToken.XmlComment))
                        e.Correct = (e.Text.Length <= 1) || correct;
                }
                else
                    e.Correct = (e.Text.Length <= 1) || correct;
            }
        }
    }
}