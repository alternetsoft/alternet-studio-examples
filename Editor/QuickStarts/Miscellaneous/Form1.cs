#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn;
using WeCantSpell.Hunspell;

namespace Miscellaneous
{
    public partial class Form1 : Form
    {
        private const string CheckSpellingDesc = "Perform spelling check of the text content in the editor";
        private const string SpellColorDesc = "Color of wavy underlines under mispelled words";
        private const string TransparentDesc = "Draw edit control background";
        private const string BackgroundStyleDesc = "Background style for the Edit control";
        private const string BorderStyleDesc = "Border style for the Edit control";
        private const string GradientBeginColorDesc = "Gradient background start color";
        private const string GradientEndColorDesc = "Gradient background end color";
        private const string HighlightBracesDesc = "Highlight matching braces in the text";
        private const string HighlightBoundsDesc = "Highlight matching braces only if caret is positioned on the brace";
        private const string TempHighlightBracesDesc = "Remove highlighting of the matched brace after small delay";
        private const string UseRoundRectDesc = "Draw rectanguar frame around matching braces";
        private const string FontStyleDesc = "Font style for matching braces";
        private const string BracesColorDesc = "Background color for matching braces";
        private const string WhiteSpaceVisibleDesc = "Display white-space symbols such as spaces, tabs, end-of line or end-of-file markers";
        private const string SeparateLinesDesc = "Draw horizontal lines to visualy separate lines in Edit control";
        private const string SymbolColorDesc = "Color used to paint special symbols";
        private SpellChecker spellChecker = new SpellChecker();
        private CsParser csParser1 = new CsParser();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\spell.txt");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            syntaxEdit1.Lexer = csParser1;

// background
            cbBackgroundStyle.SelectedIndex = 0;

            string[] s = Enum.GetNames(typeof(EditBorderStyle));
            cbBorderStyle.Items.AddRange(s);
            cbBorderStyle.SelectedItem = syntaxEdit1.BorderStyle.ToString();

// braces
            syntaxEdit1.Braces.BracesOptions = BracesOptions.Highlight;
            chbHighlightBraces.Checked = (BracesOptions.Highlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbUseRoundRect.Checked = syntaxEdit1.Braces.UseRoundRect;
            chbTempHighlightBraces.Checked = (BracesOptions.TempHighlight & syntaxEdit1.Braces.BracesOptions) != 0;
            chbHighlightBounds.Checked = (BracesOptions.HighlightBounds & syntaxEdit1.Braces.BracesOptions) != 0;
            cbBracesColor.SelectedColor = syntaxEdit1.Braces.BackColor;
            cbFontStyle.Items.Add(FontStyle.Regular);
            cbFontStyle.Items.Add(FontStyle.Bold);
            cbFontStyle.SelectedIndex = cbFontStyle.Items.IndexOf(syntaxEdit1.Braces.FontStyle);

// spelling
            chbCheckSpelling.Checked = syntaxEdit1.Spelling.CheckSpelling;
            cbSpellColor.SelectedColor = syntaxEdit1.Spelling.SpellColor;
            spellChecker.CheckSpelling(syntaxEdit1, chbCheckSpelling.Checked);

// whitespace
            chbWhiteSpaceVisible.Checked = syntaxEdit1.WhiteSpace.Visible;
            cbSymbolColor.SelectedColor = syntaxEdit1.WhiteSpace.SymbolColor;

            chbSeparateLines.Checked = (SeparatorOptions.SeparateLines & syntaxEdit1.LineSeparator.Options) != 0;
        }

        private void CheckSpellingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            spellChecker.CheckSpelling(syntaxEdit1, chbCheckSpelling.Checked);
        }

        private void SpellColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Spelling.SpellColor = cbSpellColor.SelectedColor;
        }

        private void CheckSpellingCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbCheckSpelling);
            if (str != CheckSpellingDesc)
                toolTip1.SetToolTip(chbCheckSpelling, CheckSpellingDesc);
        }

        private void SpellColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSpellColor);
            if (str != SpellColorDesc)
                toolTip1.SetToolTip(cbSpellColor, SpellColorDesc);
        }

        private void TransparentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Transparent = chbTransparent.Checked;
        }

        private void BackgroundStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Invalidate();
        }

        private void BorderStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = Enum.Parse(typeof(EditBorderStyle), cbBorderStyle.Text);
            if ((obj != null) && (obj is EditBorderStyle))
                syntaxEdit1.BorderStyle = (EditBorderStyle)obj;
        }

        private void SyntaxEdit1_PaintBackground(object sender, PaintEventArgs e)
        {
            switch (cbBackgroundStyle.SelectedIndex)
            {
                case 0:
                    {
                        // do nothing, painting background image specified by BackgroundImage property
                        break;
                    }

                case 1:
                    {
                        // painting gradient using linear gradient brush
                        Rectangle r = syntaxEdit1.ClientRect;
                        e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(r.Location, new Point(r.Right, r.Bottom), cbGradientBeginColor.SelectedColor, cbGradientEndColor.SelectedColor), r);
                        break;
                    }

                case 2:
                    {
                        // painthing theme background
                        IPainter painter = new GdiPainter();
                        painter.BeginPaint(e.Graphics);
                        try
                        {
                            Rectangle r = syntaxEdit1.ClientRect;
                            XPThemes.DrawBackground(painter, r, cbGradientBeginColor.SelectedColor, cbGradientEndColor.SelectedColor);
                        }
                        finally
                        {
                            painter.EndPaint();
                        }

                        break;
                    }
            }
        }

        private void HighlighracesCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Braces.BracesOptions = chbHighlightBraces.Checked ? syntaxEdit1.Braces.BracesOptions
                | BracesOptions.Highlight : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.Highlight;
        }

        private void UseRoundRectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Braces.UseRoundRect = chbUseRoundRect.Checked;
            syntaxEdit1.Braces.ForeColor = chbUseRoundRect.Checked ? Color.Gray : Color.Black;
        }

        private void HighlighoundsCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Braces.BracesOptions = chbHighlightBounds.Checked ? syntaxEdit1.Braces.BracesOptions
                | BracesOptions.HighlightBounds : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.HighlightBounds;
        }

        private void TempHighlighracesCheckBoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Braces.BracesOptions = chbTempHighlightBraces.Checked ? syntaxEdit1.Braces.BracesOptions
                | BracesOptions.TempHighlight : syntaxEdit1.Braces.BracesOptions & ~BracesOptions.TempHighlight;
        }

        private void FontStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = Enum.Parse(typeof(FontStyle), cbFontStyle.Text);
            if ((obj != null) && (obj is FontStyle))
                syntaxEdit1.Braces.FontStyle = (FontStyle)obj;
        }

        private void BracesColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.Braces.BackColor = cbBracesColor.SelectedColor;
        }

        private void WhiteSpaceVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.WhiteSpace.Visible = chbWhiteSpaceVisible.Checked;
        }

        private void SymbolColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.WhiteSpace.SymbolColor = cbSymbolColor.SelectedColor;
        }

        private void SeparateLinesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.LineSeparator.Options = chbSeparateLines.Checked ? syntaxEdit1.LineSeparator.Options
                | SeparatorOptions.SeparateLines : syntaxEdit1.LineSeparator.Options & ~SeparatorOptions.SeparateLines;
        }

        private void TransparentCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbTransparent);
            if (str != TransparentDesc)
                toolTip1.SetToolTip(chbTransparent, TransparentDesc);
        }

        private void BackgroundStyleComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbBackgroundStyle);
            if (str != BackgroundStyleDesc)
                toolTip1.SetToolTip(cbBackgroundStyle, BackgroundStyleDesc);
        }

        private void BorderStyleComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbBorderStyle);
            if (str != BorderStyleDesc)
                toolTip1.SetToolTip(cbBorderStyle, BorderStyleDesc);
        }

        private void GradieneginColorComboBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbGradientBeginColor);
            if (str != GradientBeginColorDesc)
                toolTip1.SetToolTip(cbGradientBeginColor, GradientBeginColorDesc);
        }

        private void GradientEndColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbGradientEndColor);
            if (str != GradientEndColorDesc)
                toolTip1.SetToolTip(cbGradientEndColor, GradientEndColorDesc);
        }

        private void HighlighracesCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightBraces);
            if (str != HighlightBracesDesc)
                toolTip1.SetToolTip(chbHighlightBraces, HighlightBracesDesc);
        }

        private void HighlighoundsCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightBounds);
            if (str != HighlightBoundsDesc)
                toolTip1.SetToolTip(chbHighlightBounds, HighlightBoundsDesc);
        }

        private void TempHighlighracesCheckBoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbTempHighlightBraces);
            if (str != TempHighlightBracesDesc)
                toolTip1.SetToolTip(chbTempHighlightBraces, TempHighlightBracesDesc);
        }

        private void UseRoundRectCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbUseRoundRect);
            if (str != UseRoundRectDesc)
                toolTip1.SetToolTip(chbUseRoundRect, UseRoundRectDesc);
        }

        private void FontStyleComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbFontStyle);
            if (str != FontStyleDesc)
                toolTip1.SetToolTip(cbFontStyle, FontStyleDesc);
        }

        private void BracesColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbBracesColor);
            if (str != BracesColorDesc)
                toolTip1.SetToolTip(cbBracesColor, BracesColorDesc);
        }

        private void WhiteSpaceVisibleCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbWhiteSpaceVisible);
            if (str != WhiteSpaceVisibleDesc)
                toolTip1.SetToolTip(chbWhiteSpaceVisible, WhiteSpaceVisibleDesc);
        }

        private void SeparateLinesCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbSeparateLines);
            if (str != SeparateLinesDesc)
                toolTip1.SetToolTip(chbSeparateLines, SeparateLinesDesc);
        }

        private void SymbolColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbSymbolColor);
            if (str != SymbolColorDesc)
                toolTip1.SetToolTip(cbSymbolColor, SymbolColorDesc);
        }

        public class SpellChecker
        {
            private WordList wordList = null;

            public SpellChecker()
            {
                try
                {
                    string dir = Application.StartupPath;
                    if (!File.Exists(Path.GetFullPath(Path.Combine(dir, "en_us.aff"))))
                        dir = Application.StartupPath + @"\..\..\..\";
                    wordList = WordList.CreateFromFiles(Path.GetFullPath(Path.Combine(dir, "en_us.dic")), Path.GetFullPath(Path.Combine(dir, "en_us.aff")));
                }
                catch
                {
                }
            }

            public void CheckSpelling(ISyntaxEdit edit, bool spell)
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
                bool correct = (wordList != null) ? wordList.Check(e.Text) : true;
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
}
