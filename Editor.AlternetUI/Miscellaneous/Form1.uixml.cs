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
using Alternet.Syntax.Parsers.Roslyn;
using WeCantSpell.Hunspell;
using Alternet.Editor.TextSource;
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

        private Color gradientBeginColor = Color.Blue;
        private Color gradientEndColor = Color.White;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            Form1_Load(this, EventArgs.Empty);
            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"spell.txt"))
            {
                textSource.Lexer = csParser1;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;

            // background
            cbBackgroundStyle.Items.Add(BackgroundStyleSolid);
            cbBackgroundStyle.Items.Add(BackgroundStyleGradient);
            cbBackgroundStyle.SelectedIndex = 0;
            cbBackgroundColor.Value = syntaxEdit1.BackColor;
            cbGradientBeginColor.Value = gradientBeginColor;
            cbGradientEndColor.Value = gradientEndColor;

            //string[] s = Enum.GetNames(typeof(EditBorderStyle));
            //cbBorderStyle.Items.AddRange(s);
            //cbBorderStyle.SelectedItem = syntaxEdit1.BorderStyle.ToString();

            // braces
            syntaxEdit1.Braces.BracesOptions = BracesOptions.Highlight;
            chbHighlightBraces.IsChecked
                = (BracesOptions.Highlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbUseRoundRect.IsChecked = syntaxEdit1.Braces.UseRoundRect;
            chbTempHighlightBraces.IsChecked
                = (BracesOptions.TempHighlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbHighlightBounds.IsChecked
                = (BracesOptions.HighlightBounds & syntaxEdit1.Braces.BracesOptions) != 0;
            cbBracesColor.Value = syntaxEdit1.Braces.BackColor;
            cbFontStyle.Items.Add(FontStyle.Regular);
            cbFontStyle.Items.Add(FontStyle.Bold);
            cbFontStyle.SelectedIndex = cbFontStyle.Items.IndexOf(syntaxEdit1.Braces.FontStyle);

            // spelling
            chbCheckSpelling.IsChecked = syntaxEdit1.Spelling.CheckSpelling;
            cbSpellColor.Value = syntaxEdit1.Spelling.SpellColor;
            spellChecker.CheckSpelling(syntaxEdit1, chbCheckSpelling.IsChecked);

            // whitespace
            chbWhiteSpaceVisible.IsChecked = syntaxEdit1.WhiteSpace.Visible;
            cbSymbolColor.Value = syntaxEdit1.WhiteSpace.SymbolColor;

            cbGradientBeginColor.SelectedIndexChanged += GradientBeginColor_SelectedIndexChanged;
            cbGradientEndColor.SelectedIndexChanged += GradientEndColor_SelectedIndexChanged;
            chbSeparateLines.IsChecked
                = (SeparatorOptions.SeparateLines & syntaxEdit1.LineSeparator.Options) != 0;

            chbCheckSpelling.CheckedChanged += CheckSpellingCheckBox_CheckedChanged;
            cbSpellColor.SelectedIndexChanged += SpellColorComboBox_SelectedIndexChanged;
            chbTransparent.CheckedChanged += TransparentCheckBox_CheckedChanged;
            cbBackgroundStyle.SelectedIndexChanged += BackgroundStyleComboBox_SelectedIndexChanged;
            chbHighlightBraces.CheckedChanged += HighlightBracesCheckBoxTextBox_CheckedChanged;
            chbUseRoundRect.CheckedChanged += UseRoundRectCheckBox_CheckedChanged;
            chbHighlightBounds.CheckedChanged += HighlightBoundsCheckBoxTextBox_CheckedChanged;
            chbTempHighlightBraces.CheckedChanged += TempHighlightBracesCheckBoxTextBox_CheckedChanged;
            cbFontStyle.SelectedIndexChanged += FontStyleComboBox_SelectedIndexChanged;
            cbBracesColor.SelectedIndexChanged += BracesColorComboBox_SelectedIndexChanged;
            chbWhiteSpaceVisible.CheckedChanged += WhiteSpaceVisibleCheckBox_CheckedChanged;
            cbSymbolColor.SelectedIndexChanged += SymbolColorComboBox_SelectedIndexChanged;
            chbSeparateLines.CheckedChanged += SeparateLinesCheckBox_CheckedChanged;
            cbBackgroundColor.SelectedIndexChanged += CbBackgroundColor_SelectedIndexChanged;

            ActiveControl = syntaxEdit1;
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

            switch (cbBackgroundStyle.Text)
            {
                case BackgroundStyleSolid:
                    syntaxEdit1.Background = null;
                    break;
                case BackgroundStyleGradient:

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

        //private void BorderStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    object obj = Enum.Parse(typeof(EditBorderStyle), cbBorderStyle.Text);
        //    if ((obj != null) && (obj is EditBorderStyle))
        //        syntaxEdit1.BorderStyle = (EditBorderStyle)obj;
        //}

        //private void SyntaxEdit1_PaintBackground(object sender, PaintEventArgs e)
        //{
        //    switch (cbBackgroundStyle.SelectedIndex)
        //    {
        //        case 0:
        //            {
        //                // do nothing, painting background image specified by BackgroundImage property
        //                break;
        //            }

        //        case 1:
        //            {
        //                // painting gradient using linear gradient brush
        //                Rectangle r = syntaxEdit1.ClientRect;
        //                e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(r.Location, new Point(r.Right, r.Bottom), cbGradientBeginColor.SelectedColor, cbGradientEndColor.SelectedColor), r);
        //                break;
        //            }

        //        case 2:
        //            {
        //                // painthing theme background
        //                IPainter painter = new GdiPainter();
        //                painter.BeginPaint(e.Graphics);
        //                try
        //                {
        //                    Rectangle r = syntaxEdit1.ClientRect;
        //                    XPThemes.DrawBackground(painter, r, cbGradientBeginColor.SelectedColor, cbGradientEndColor.SelectedColor);
        //                }
        //                finally
        //                {
        //                    painter.EndPaint();
        //                }

        //                break;
        //            }
        //    }
        //}

        private void HighlightBracesCheckBoxTextBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Braces.BracesOptions
                = chbHighlightBraces.IsChecked ? syntaxEdit1.Braces.BracesOptions
                | BracesOptions.Highlight : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.Highlight;
        }

        private void UseRoundRectCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Braces.UseRoundRect = chbUseRoundRect.IsChecked;
            syntaxEdit1.Braces.ForeColor = chbUseRoundRect.IsChecked ? Color.Gray : Color.Black;
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
            object obj = Enum.Parse(typeof(FontStyle), cbFontStyle.Text);
            if ((obj != null) && (obj is FontStyle))
                syntaxEdit1.Braces.FontStyle = (FontStyle)obj;
        }

        private void BracesColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Braces.BackColor = cbBracesColor.Value;
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