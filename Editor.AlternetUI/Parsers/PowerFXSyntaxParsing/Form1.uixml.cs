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
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.PowerFx;

namespace PowerFxSyntaxParsing
{
    public partial class Form1 : Window
    {
        private readonly Alternet.Editor.TextSource.TextSource jsonSource = new();
        private readonly Alternet.Editor.TextSource.TextSource fxSource = new();
        private readonly JSONParser jsonParser = new();
        private readonly PowerFxParser fxParser = new();

        private readonly OpenFileDialog openFileDialog1 = new();


        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
                syntaxEdit2.VisualThemeType = VisualThemeType.Dark;
            }

            btLoad.Click += LoadButton_Click;

            syntaxEdit1.Source = fxSource;
            syntaxEdit1.Outlining.AllowOutlining = true;
            syntaxEdit2.Source = jsonSource;
            syntaxEdit2.Outlining.AllowOutlining = true;

            fxSource.OptimizedForMemory = false;
            jsonSource.OptimizedForMemory = false;
            fxSource.HighlightReferences = true;
            fxSource.Lexer = fxParser;
            jsonSource.Lexer = jsonParser;
            fxParser.EvaluateQuickInfo = chbEvaluateQuickInfo.Checked;
            Form1_Load(this, EventArgs.Empty);

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PowerFx files (*.fx)|*.fx|All files (*.*)|*.*";
            DirectoryInfo dirInfo
                = new DirectoryInfo(DemoUtils.ResourcesFolder + @"Editor/Text/");

            FileInfo fileInfo = new FileInfo(dirInfo.FullName + @"Record.fx");
            if (fileInfo.Exists)
                fxSource.LoadFile(fileInfo.FullName);
            fileInfo = new FileInfo(dirInfo.FullName + @"Record.fx.json");

            if (fileInfo.Exists && syntaxEdit2.LoadFile(fileInfo.FullName))
            {
                ApplyGlobalContext(syntaxEdit2.Text);
            }
            else
            {
                syntaxEdit2.Text = $"Error loading: {fileInfo.Name}";
            }

            openFileDialog1.InitialDirectory = dirInfo.FullName;
            chbEvaluateQuickInfo.CheckedChanged += EvaluateQuickInfoCheckBox_CheckedChanged;
            btEaluate.Click += EvaluateButton_Click;
            syntaxEdit2.TextChanged += SyntaxEdit2_TextChanged;
        }

        private void SyntaxEdit2_TextChanged(object? sender, EventArgs e)
        {
            ApplyGlobalContext(syntaxEdit2.Text);
        }

        private void EvaluateQuickInfoCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (fxParser != null)
                fxParser.EvaluateQuickInfo = chbEvaluateQuickInfo.Checked;
        }

        private bool ApplyGlobalContext(string s)
        {
            if (!PowerFxParser.IsValidJson(s))
                return false;

            if (string.IsNullOrEmpty(s))
            {
                fxParser.GlobalContextAsJson = null;
            }
            else
                fxParser.GlobalContextAsJson = s;
            return true;
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowAsync(() =>
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            });
        }

        private void EvaluateButton_Click(object? sender, EventArgs e)
        {
            var r = fxParser.Evaluator.Eval(syntaxEdit1.Text);
            var s = PowerFxEvaluator.EvalResultToString(r);
            MessageBox.Show(s, "Evaluation Result");
        }
    }
}
