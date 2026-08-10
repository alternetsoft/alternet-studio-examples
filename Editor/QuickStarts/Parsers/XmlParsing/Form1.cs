#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;

namespace XmlParsing
{
    public partial class Form1 : Form
    {
        private const string XmlDescription = "Choose Xml";
        private const string BooksXml = "books.xml";
        private const string OrderXml = "order.xml";
        private const string SubDir = @"Resources\Editor\QuickStarts\Parsers\XML\SimpleSchema";

        private string dir = Application.StartupPath + @"\";
        private int updateCount = 0;
        private XmlParserWithSchema xmlParser = new XmlParserWithSchema();

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "XmlParsing.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + SubDir);
            if (!dirInfo.Exists)
            {
                dir = Path.GetFullPath(Application.StartupPath + @"\..\..\..\..\..\..\..\");
                if (!Directory.Exists(Path.GetFullPath(dir) + SubDir))
                    dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            cbXmls.SelectedIndex = 0;
            syntaxEdit1.Lexer = xmlParser;
        }

        private void XmlComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updateCount > 0)
                return;

            var fileName = string.Empty;
            switch (cbXmls.SelectedIndex)
            {
                case 0:
                    fileName = Path.GetFullPath(Path.Combine(dir, SubDir, BooksXml));
                    break;

                case 1:
                    fileName = Path.GetFullPath(Path.Combine(dir, SubDir, OrderXml));
                    break;
            }

            if ((fileName != string.Empty) && File.Exists(fileName))
            {
                syntaxEdit1.Source.LoadFile(fileName);
                syntaxEdit1.Source.FileName = fileName;
            }
        }

        private void XmlComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbXmls);
            if (str != XmlDescription)
                toolTip1.SetToolTip(cbXmls, XmlDescription);
        }
    }
}