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

using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Editor.Common.AlternetUI;

namespace CodeSnippets
{
    public partial class Form1 : Window
    {
        private readonly TextSource csharpSource = new();
        private readonly TextSource vbSource = new();
        private readonly CsParser csParser1 = new(new CsSolution());
        private readonly VbParser vbParser1 = new(new VbSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Source = csharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            csharpSource.OptimizedForMemory = false;
            vbSource.OptimizedForMemory = false;

            Form1_Load(this, EventArgs.Empty);

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.GetResourceFolderFullPath(@"Editor/Text/"));

            if(csharpSource.LoadOrAddNotFound(dirInfo.FullName + @"c#.cs"))
            {
                csharpSource.HighlightReferences = true;
                csharpSource.Lexer = csParser1;
            }

            if (vbSource.LoadOrAddNotFound(dirInfo.FullName + @"vb_net.txt"))
            {
                vbSource.HighlightReferences = true;
                vbSource.Lexer = vbParser1;
            }

            rbCSharpSnippets.CheckedChanged += CSharpSnippetsRadioButton_CheckedChanged;
            UpdateSnippets();
        }

        private void UpdateSnippets()
        {
            if (rbCSharpSnippets.IsChecked)
            {
                syntaxEdit1.Source = csharpSource;
            }
            else
            {
                syntaxEdit1.Source = vbSource;
            }
        }

        private void CSharpSnippetsRadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateSnippets();
        }
    }
}
