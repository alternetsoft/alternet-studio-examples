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
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Alternet.Syntax.Lexer;

using WeCantSpell.Hunspell;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class OtherSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private Dictionary<string, string> spellingDictionary = new Dictionary<string, string>();
        private SpellChecker spellChecker = new SpellChecker();

        public OtherSettingsUserControl()
        {
            InitializeComponent();
        }

        public TextEditor Editor
        {
            get
            {
                return editor;
            }

            set
            {
                if (editor == value)
                    return;

                editor = value;

                if (editor != null)
                    UpdateDataFromEditor();
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void WhitespaceVisibleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.WhitespaceVisible = WhitespaceVisibleCheckBox.IsChecked.Value;
        }

        private void UserMarginVisibleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            AttachOrDetachUserMarginTextHandlers();
            Editor.UserMarginVisible = UserMarginVisibleCheckBox.IsChecked.Value;
        }

        private void HighlightUrlsCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.Source.HighlightHyperText = HighlightUrlsCheckBox.IsChecked.Value;
        }

        private void CheckSpellingCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            spellChecker.CheckSpelling(editor, CheckSpellingCheckBox.IsChecked.Value);
        }

        private void DisplayContentDividersCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.ContentDividersVisible = DisplayContentDividersCheckBox.IsChecked.Value;
        }

        private void AttachOrDetachUserMarginTextHandlers()
        {
            if (UserMarginVisibleCheckBox.IsChecked.Value)
            {
                Editor.GetUserMarginText += Editor_GetUserMarginText;
            }
            else
            {
                Editor.GetUserMarginText -= Editor_GetUserMarginText;
            }
        }

        private void Editor_GetUserMarginText(object sender, UserMarginTextEventArgs e)
        {
            e.Text = string.Format("{0} chars", ((TextEditor)sender).Lines[e.Line].Length);
            e.Handled = true;
        }

        private void SeparateLinesCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.SeparatorOptions = SeparateLinesCheckBox.IsChecked.Value ? Editor.SeparatorOptions | SeparatorOptions.SeparateLines : Editor.SeparatorOptions & ~SeparatorOptions.SeparateLines;
        }

        private void HighlightCurrentLineCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.SeparatorOptions = HighlightCurrentLineCheckBox.IsChecked.Value ? Editor.SeparatorOptions | SeparatorOptions.HighlightCurrentLine : Editor.SeparatorOptions & ~SeparatorOptions.HighlightCurrentLine;
        }

        private void HighlightReferencesCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.HighlightReferences = HighlightReferencesCheckBox.IsChecked.Value;
        }

        private void UpdateDataFromEditor()
        {
            WhitespaceVisibleCheckBox.IsChecked = Editor.WhitespaceVisible;
            HighlightUrlsCheckBox.IsChecked = Editor.Source.HighlightHyperText;
            CheckSpellingCheckBox.IsChecked = Editor.Spelling.CheckSpelling;
            spellChecker.CheckSpelling(Editor, CheckSpellingCheckBox.IsChecked.Value);
            DisplayContentDividersCheckBox.IsChecked = Editor.ContentDividersVisible;
            UserMarginVisibleCheckBox.IsChecked = Editor.UserMarginVisible;
            SeparateLinesCheckBox.IsChecked = (Editor.SeparatorOptions & SeparatorOptions.SeparateLines) != 0;
            HighlightCurrentLineCheckBox.IsChecked = (Editor.SeparatorOptions & SeparatorOptions.HighlightCurrentLine) != 0;
            HighlightBracesCheckBox.IsChecked = (Editor.Braces.BracesOptions & BracesOptions.Highlight) != 0;
        }

        private void HighlightBracesCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.Braces.BracesOptions = HighlightBracesCheckBox.IsChecked.Value ? Editor.Braces.BracesOptions | BracesOptions.Highlight : Editor.Braces.BracesOptions & ~BracesOptions.Highlight;
        }

        public class SpellChecker
        {
            private WordList wordList = null;

            public SpellChecker()
            {
                try
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    if (!File.Exists(Path.GetFullPath(Path.Combine(dir, "en_us.aff"))))
                        dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\";
                    wordList = WordList.CreateFromFiles(Path.GetFullPath(Path.Combine(dir, "en_us.dic")), Path.GetFullPath(Path.Combine(dir, "en_us.aff")));
                }
                catch
                {
                }
            }

            public void CheckSpelling(TextEditor edit, bool spell)
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
