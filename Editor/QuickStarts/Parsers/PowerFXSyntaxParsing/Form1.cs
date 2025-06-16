#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.PowerFX;

namespace PowerFXSyntaxParsing
{
    public partial class Form1 : Form
    {
        private const string LoadDesc = "Load code file";
        private readonly JSONParser jsonParser = new JSONParser();
        private readonly PowerFxParser fxParser = new PowerFxParser();
        private TextSource source = new TextSource();
        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "PowerFXSyntaxParsing.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Padding = new Padding(4, 0, 0, 8);
            label2.Padding = new Padding(4, 0, 0, 8);
            syntaxEdit1.Source = this.source;
            source.Lexer = fxParser;
            syntaxEdit2.Source.FileName = "test.js";
            syntaxEdit2.Lexer = jsonParser;
            syntaxEdit1.Spelling.SpellColor = Color.Navy;
            syntaxEdit1.Outlining.AllowOutlining = true;
            fxParser.EvaluateQuickInfo = false;
            EvaluateQuickInfoCheckBox.CheckedChanged += EvaluateQuickInfoCheckBox_CheckedChanged;

            openFileDialog1.Filter = "PowerFx files (*.fx)|*.fx|All files (*.*)|*.*";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\Record.fx");
            if (fileInfo.Exists)
                source.LoadFile(fileInfo.FullName);
            fileInfo = new FileInfo(dir + @"Resources\Editor\text\Record.fx.json");

            if (fileInfo.Exists && syntaxEdit2.LoadFile(fileInfo.FullName))
            {
                ApplyGlobalContext(syntaxEdit2.Text);
            }
            else
            {
                syntaxEdit2.Text = $"Error loading: {fileInfo.Name}";
            }

            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\text\";
        }

        private void EvaluateQuickInfoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (fxParser != null)
                fxParser.EvaluateQuickInfo = EvaluateQuickInfoCheckBox.Checked;
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

        private void EvaluateButton_Click(object sender, EventArgs e)
        {
            var r = fxParser.Evaluator.Eval(syntaxEdit1.Text);
            var s = PowerFXEvaluator.EvalResultToString(r);
            MessageBox.Show(s, "Evaluation Result");
        }
    }
}
