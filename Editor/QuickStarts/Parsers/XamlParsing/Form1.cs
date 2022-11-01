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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Syntax.Parsers.XAML;

namespace XAMLParsing
{
    public partial class Form1 : Form
    {
        private string dir = Application.StartupPath + @"\";
        private XAMLParser parser = new XAMLParser();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syntaxEdit1.Lexer = parser;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            string path = Path.Combine(dirInfo.FullName, "xaml.xaml");
            if (File.Exists(path))
            {
                syntaxEdit1.Source.FileName = path;
                syntaxEdit1.Source.LoadFile(path);
            }
        }
    }
}
