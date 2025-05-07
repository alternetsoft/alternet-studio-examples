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

namespace PrintAndPreview
{
    public partial class Form1 : Form
    {
        private const string PrintDesc = "Display Print Dialog";
        private const string PrintPreviewDesc = "Display Print Preview Dialog";
        private const string PageSetupDesc = "Display Page Setup Dialog";
        private const string PrintOptionsDesc = "Display Print Options Dialog";
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
            {
                syntaxEdit1.LoadFile(fileInfo.FullName);
                syntaxEdit1.Source.FileName = fileInfo.Name;
            }

            syntaxEdit1.Lexer = csParser1;
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            if (syntaxEdit1.Printing.ExecutePrintDialog() == DialogResult.OK)
                syntaxEdit1.Printing.Print();
        }

        private void PrintPreviewButton_Click(object sender, EventArgs e)
        {
            syntaxEdit1.Printing.ExecutePrintPreviewDialog();
        }

        private void PageSetupButton_Click(object sender, EventArgs e)
        {
            syntaxEdit1.Printing.ExecutePageSetupDialog();
        }

        private void PrintOptionsButton_Click(object sender, EventArgs e)
        {
            if (syntaxEdit1.Printing.ExecutePrintOptionsDialog() == DialogResult.OK)
                syntaxEdit1.Printing.Options = syntaxEdit1.Printing.PrintOptionsDialog.Options;
        }

        private void PrintButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrint);
            if (str != PrintDesc)
                toolTip1.SetToolTip(btPrint, PrintDesc);
        }

        private void PrintPreviewButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrintPreview);
            if (str != PrintPreviewDesc)
                toolTip1.SetToolTip(btPrintPreview, PrintPreviewDesc);
        }

        private void PageSetupButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPageSetup);
            if (str != PageSetupDesc)
                toolTip1.SetToolTip(btPageSetup, PageSetupDesc);
        }

        private void PrintOptionsButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btPrintOptions);
            if (str != PrintOptionsDesc)
                toolTip1.SetToolTip(btPrintOptions, PrintOptionsDesc);
        }
    }
}
