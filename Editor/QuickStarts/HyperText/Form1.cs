#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn;

namespace HyperText
{
    public partial class Form1 : Form
    {
        private const string HighlightURLDesc = "Highlight hyperlinks in the text";
        private const string CustomHypertextDesc = "Highlight custom hypertext";
        private const string UrlColorDesc = "Color of highlighted urls";
        private const string FontStyleDesc = "Font style of highlighted urls";
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

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
            syntaxEdit1.Lexer = csParser1;

            chbHighlightUrls.Checked = syntaxEdit1.HyperText.HighlightHyperText;
            cbUrlColor.SelectedColor = syntaxEdit1.HyperText.UrlColor;
            string[] s = Enum.GetNames(typeof(FontStyle));
            cbFontStyle.Items.AddRange(s);
            cbFontStyle.SelectedItem = syntaxEdit1.HyperText.UrlStyle.ToString();
        }

        private void HighlightUrlsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit1.HyperText.HighlightHyperText = chbHighlightUrls.Checked;
            syntaxEdit1.Invalidate();
        }

        private void CustomHypertextCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Hashtable hs = ((TextSource)syntaxEdit1.Source).UrlTable;
            if (chbCustomHypertext.Checked)
            {
                hs.Add('<', true);
                hs.Add('>', false);
            }
            else
            {
                hs.Remove('<');
                hs.Remove('>');
            }

            syntaxEdit1.Source.Notification(syntaxEdit1.Lexer, EventArgs.Empty);
        }

        private void UrlColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.HyperText.UrlColor = cbUrlColor.SelectedColor;
        }

        private void FontStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object obj = Enum.Parse(typeof(FontStyle), cbFontStyle.Text);
            if ((obj != null) && (obj is FontStyle))
                syntaxEdit1.HyperText.UrlStyle = (FontStyle)obj;
        }

        private void SyntaxEdit1_JumpToUrl(object sender, UrlJumpEventArgs e)
        {
            if (chbCustomHypertext.Checked)
            {
                MessageBox.Show(e.Text);
                e.Handled = true;
            }
        }

        private void SyntaxEdit1_CheckHyperText(object sender, HyperTextEventArgs e)
        {
            if (chbCustomHypertext.Checked)
                e.IsHyperText = e.Text.IndexOf("<") >= 0;
        }

        private void HighlightUrlsCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbHighlightUrls);
            if (str != HighlightURLDesc)
                toolTip1.SetToolTip(chbHighlightUrls, HighlightURLDesc);
        }

        private void CustomHypertextCheckBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(chbCustomHypertext);
            if (str != CustomHypertextDesc)
                toolTip1.SetToolTip(chbCustomHypertext, CustomHypertextDesc);
        }

        private void UrlColorComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbUrlColor);
            if (str != UrlColorDesc)
                toolTip1.SetToolTip(cbUrlColor, UrlColorDesc);
        }

        private void FontStyleComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbFontStyle);
            if (str != FontStyleDesc)
                toolTip1.SetToolTip(cbFontStyle, FontStyleDesc);
        }
    }
}
