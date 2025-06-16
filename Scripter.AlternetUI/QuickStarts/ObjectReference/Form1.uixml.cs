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

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Scripter;
using Microsoft.CodeAnalysis.Differencing;
using System.Linq;
using System.Diagnostics;
using Alternet.Drawing;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace ObjectReference
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new ScriptRun();
        private IDisposable? scriptObject;

        private volatile bool scriptRunning;

        private readonly CsParser csParser = new(new CsSolution());
        private readonly VbParser vbParser = new(new VbSolution());

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.Items.Add("C#");

            if (Consts.IsWindows)
                cbLanguages.Items.Add("Visual Basic");

            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            scriptRun.ScriptSource.References.Clear();

            var asms = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in asms)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            csParser.Repository.RegisterAssemblies(asms);
            vbParser.Repository.RegisterAssemblies(asms);

            AddScriptItem();

            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            cbLanguages.SelectedIndex = 0;
            ScriptButton.Click += RunScriptButton_Click;
            TestButton.Click += TestButton_Click;
            UpdateButtons();

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref scriptObject);
            scriptRunning = false;

            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);
                scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                scriptObject = scriptRun.Run() as IDisposable;
                scriptRunning = true;
                UpdateButtons();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                SafeDispose(ref scriptObject);
                scriptRunning = false;
                TestButton.Text = "Test Button";
                UpdateButtons();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private void LoadFile(SyntaxEdit edit, string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var exists = fileInfo.Exists;
            ScriptButton.Enabled = exists;

            if (exists)
            {
                edit.Source.LoadFile(fileName);
            }
            else
            {
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;
        }


        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReference.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReference.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = Path.Combine(DemoUtils.ResourcesFolder, ResourcesFolderName, sourceFileSubPath);
            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = csParser;
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = vbParser;
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void UpdateButtons()
        {
            ScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void TestButton_Click(object? sender, EventArgs e)
        {
            StopScript();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbLanguages.SelectedIndex is null)
                return;

            StopScript();
            UpdateSource(cbLanguages.SelectedIndex.Value);
        }

        private void AddScriptItem()
        {
            ScriptGlobalItem item = new ("RunButton", typeof(Alternet.UI.Button), TestButton);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);

            var s = scriptRun.ScriptHost.GlobalCode;
            scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
            var s2 = scriptRun.ScriptHost.GlobalCode;
            scriptRun.ScriptLanguage = ScriptLanguage.CSharp;

            csParser.Repository.AddDocument("global.cs", s);
            vbParser.Repository.AddDocument("global.vb", s2);
        }
    }
}