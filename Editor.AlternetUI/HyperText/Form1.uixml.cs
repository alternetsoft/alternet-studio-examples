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
using Alternet.Editor.TextSource;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace HyperText
{
    public partial class Form1 : Window
    {
        private readonly CsParser csParser1 = new(new CsSolution());

        private int clickCounter = 1;
        private bool isCustomHyperText;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.HyperText.HighlightHyperText = true;
            syntaxEdit1.Outlining.AllowOutlining = true;

            var textSource = new TextSource();
            syntaxEdit1.Source = textSource;

            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/");

            if (textSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                textSource.Lexer = csParser1;
            }

            chbHighlightUrls.IsChecked = syntaxEdit1.HyperText.HighlightHyperText;
            cbUrlColor.Value = syntaxEdit1.HyperText.UrlColor;
            cbFontStyle.AddEnumValues<FontStyle>(syntaxEdit1.HyperText.UrlStyle);
            chbHighlightUrls.CheckedChanged += HighlightUrlsCheckBox_CheckedChanged;
            chbCustomHypertext.CheckedChanged += CustomHypertextCheckBox_CheckedChanged;
            cbUrlColor.SelectedIndexChanged += UrlColorComboBox_SelectedIndexChanged;
            syntaxEdit1.JumpToUrl += SyntaxEdit1_JumpToUrl;
            syntaxEdit1.CheckHyperText += SyntaxEdit1_CheckHyperText;
            cbFontStyle.SelectedIndexChanged += CbFontStyle_SelectedIndexChanged;

            chbHighlightUrls.IsChecked = true;

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;

            syntaxEdit1.UrlDisplayText += SyntaxEdit1_UrlDisplayText;
        }

        private bool IsCustomHyperText
        {
            get => isCustomHyperText;
            
            set
            {
                if (isCustomHyperText == value)
                    return;
                isCustomHyperText = value;
                UpdateUrlTable();
            }
        }

        private void SyntaxEdit1_UrlDisplayText(object sender, UrlDisplayTextEventArgs e)
        {
            if(e.Text.Contains("<"))
            {
                e.Handled = true;
                e.DisplayText = "Custom url";
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void CbFontStyle_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if(cbFontStyle.SelectedItem is FontStyle fontStyle)
                syntaxEdit1.HyperText.UrlStyle = fontStyle;
        }

        private void HighlightUrlsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.HyperText.HighlightHyperText = chbHighlightUrls.IsChecked;
            UpdateUrlTable();
        }

        private void CustomHypertextCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            IsCustomHyperText = chbCustomHypertext.IsChecked;
        }

        private void UpdateUrlTable()
        {
            var hs = ((TextSource)syntaxEdit1.Source).UrlTable;
            if (hs != null)
            {
                if (IsCustomHyperText)
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
        }

        private void UrlColorComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.HyperText.UrlColor = cbUrlColor.Value;
        }

        private void SyntaxEdit1_JumpToUrl(object? sender, UrlJumpEventArgs e)
        {
            if (IsCustomHyperText || true)
            {
                e.Handled = true;
                RunWhenIdle(() =>
                {
                    statusBar.Text = $"Url '{e.Text}' clicked {clickCounter++} times";
                });
            }
        }

        private void SyntaxEdit1_CheckHyperText(object? sender, HyperTextEventArgs e)
        {
            if (IsCustomHyperText)
                e.IsHyperText = e.Text.Contains("<");
        }
    }
}