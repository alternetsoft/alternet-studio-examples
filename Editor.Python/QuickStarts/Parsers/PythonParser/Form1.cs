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
using System.IO;
using System.Windows.Forms;
using Alternet.Common.Python;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Python;

namespace PythonParser
{
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string LoadDesc = "Load code file";
        private PythonNETParser pythonParser = new PythonNETParser();

        private string dir = Application.StartupPath + @"\";
        private ICodeEnvironment codeEnvironment;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IList<string> paths = new List<string>();

            syntaxEdit1.Lexer = pythonParser;
            openFileDialog1.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
                if (!Directory.Exists(dir))
                    dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\Script.py");
            if (fileInfo.Exists)
            {
                syntaxEdit1.LoadFile(fileInfo.FullName);
                paths.Add(fileInfo.DirectoryName);
            }

            if (!string.IsNullOrEmpty(CodeEnvironment.PythonLibPath))
            {
                paths.Add(CodeEnvironment.PythonLibPath);
            }

            dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\text";

            var imports = new List<string>();

            imports.Add("System");
            imports.Add("System.Drawing");
            imports.Add("System.Windows.Forms");

            codeEnvironment = new CodeEnvironment(null, paths, imports, null, Alternet.Common.TechnologyEnvironment.WindowsForms);

            pythonParser.CodeEnvironment = codeEnvironment;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            }
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }
   }
}
