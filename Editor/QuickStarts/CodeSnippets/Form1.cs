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
using System.IO;
using System.Windows.Forms;

using Alternet.Syntax.Parsers.Roslyn;

namespace CodeSnippets
{
    public partial class Form1 : Form
    {
        private const string CShaprSnippetsDesc = "Code snippets for C# language";
        private const string VBSnippetsDesc = "Code snippets for VB language";
        private CsParser csParser1 = new CsParser();
        private VbParser vbParser1 = new VbParser();
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
                dir = Application.StartupPath + @"\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);

            fileInfo = new FileInfo(dir + @"Resources\Editor\text\vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;

            UpdateSnippets();
        }

        private void UpdateSnippets()
        {
            if (rbCSharpSnippets.Checked)
            {
                syntaxEdit1.Source = csharpSource;
            }
            else
            {
                syntaxEdit1.Source = vbSource;
            }
        }

        private void CSharpSnippetsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSnippets();
        }

        private void CSharpSnippetsRadioButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(rbCSharpSnippets);
            if (str != CShaprSnippetsDesc)
                toolTip1.SetToolTip(rbCSharpSnippets, CShaprSnippetsDesc);
        }

        private void VBSnippetsRadioButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(rbVBSnippets);
            if (str != VBSnippetsDesc)
                toolTip1.SetToolTip(rbVBSnippets, VBSnippetsDesc);
        }
    }
}
