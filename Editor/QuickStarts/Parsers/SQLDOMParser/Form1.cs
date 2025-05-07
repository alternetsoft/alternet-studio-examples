#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Syntax;

namespace SQLDOMParser
{
    public partial class Form1 : Form
    {
        private ISyntaxParser parser = new SQLWrapper();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "SQLDOMParser.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            syntaxEdit1.Lexer = parser;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";

            FileInfo dataFileInfo = new FileInfo(dir + @"Resources\Editor\QuickStarts\Parsers\SQLDOM\databaseObjects.xml");
            if (dataFileInfo.Exists)
                ((SqlWrapRepository)parser.CompletionRepository).LoadDataFromXml(dataFileInfo.FullName);

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\SQLDOM.txt");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
        }
    }
}
