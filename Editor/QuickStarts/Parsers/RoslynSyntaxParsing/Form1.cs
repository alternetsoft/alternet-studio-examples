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

using Alternet.Syntax.Parsers.Roslyn;

namespace RoslynSyntaxParsing
{
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string LoadDesc = "Load code file";
        private CsParser csParser1 = new CsParser();
        private VbParser vbParser1 = new VbParser();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            cbLanguages.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            fileInfo = new FileInfo(dir + @"Resources\Editor\text\vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);
            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\text\";
            csharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;
            csharpSource.HighlightReferences = true;
            vbSource.HighlightReferences = true;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbLanguages.SelectedIndex)
            {
                case 0:
                    syntaxEdit1.Source = csharpSource;
                    break;
                case 1:
                    syntaxEdit1.Source = vbSource;
                    break;
                default:
                    syntaxEdit1.Source = csharpSource;
                    break;
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = Math.Max(1, cbLanguages.SelectedIndex + 1);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            }
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }
    }
}
