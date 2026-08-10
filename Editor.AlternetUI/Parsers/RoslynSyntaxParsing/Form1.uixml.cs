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
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace RoslynSyntaxParsing
{
    public partial class Form1 : Window
    {
        private readonly TextSource cSharpSource = new();
        private readonly TextSource vbSource = new();
        private readonly OpenFileDialog openFileDialog1 = new();

        private readonly CsParser csParser1 = new(new CsSolution());
        private readonly VbParser vbParser1 = new(new VbSolution());

        public Form1()
        {
            InitializeComponent();

            DemoUtils.InitCommonEditorProps(syntaxEdit1);

            csParser1.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                | SyntaxOptions.SyntaxErrors;

            vbParser1.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                | SyntaxOptions.SyntaxErrors;

            if (CommandLineArgs.ParseAndGetIsDark())
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;

            cbLanguages.AddRange(new []
            {
                "C#",
                "Visual Basic"
            });

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;
            btLoad.Click += LoadButton_Click;

            syntaxEdit1.Source = cSharpSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            cSharpSource.OptimizedForMemory = false;
            vbSource.OptimizedForMemory = false;

            Form1_Load(this, EventArgs.Empty);

            cbLanguages.Value = "C#";

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "C# files (*.cs)|*.cs|VB files (*.vb)|*.vb";
            DirectoryInfo dirInfo = new(DemoUtils.GetResourceFolderFullPath(@"Editor/Text/"));

            FileInfo fileInfo = new(Path.Combine(dirInfo.FullName, "c#.cs"));
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);

            fileInfo = new FileInfo(Path.Combine(dirInfo.FullName, "vb_net.txt"));
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);

            openFileDialog1.InitialDirectory = dirInfo.FullName;

            cSharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;
            cSharpSource.HighlightReferences = true;
            vbSource.HighlightReferences = true;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Source = cbLanguages.Value switch
            {
                "C#" => cSharpSource,
                "Visual Basic" => vbSource,
                _ => cSharpSource,
            };
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = cbLanguages.Value?.ToString() == "C#" ? 0 : 1;

            openFileDialog1.ShowAsync(() =>
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            });
        }
    }
}
