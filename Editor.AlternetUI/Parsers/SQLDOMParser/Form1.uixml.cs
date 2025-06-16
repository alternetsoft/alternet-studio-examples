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

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;

namespace SQLDOMParser
{
    public partial class Form1 : Window
    {
        private readonly SQLWrapper parser = new();

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;

            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit1.Lexer = parser;
            syntaxEdit1.WordWrap = true;

            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
        }
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo dataFileInfo
                = new(DemoUtils.ResourcesFolder
                + @"Editor/QuickStarts/Parsers/SQLDOM/databaseObjects.xml");

            if (dataFileInfo.Exists)
            {
                ((SqlWrapRepository)parser.CompletionRepository)
                    .LoadDataFromXml(dataFileInfo.FullName);
            }

            FileInfo fileInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text/SQLDOM.txt");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);
        }
    }
}
