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
using System.Linq;

using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;

using Alternet.UI;

namespace ObjectReference.Python
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new ScriptRun();
        private IDisposable? scriptObject;

        private volatile bool scriptRunning;

        private PythonNETParser pythonParser1 = new PythonNETParser();
        private Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;

            scriptRun.ScriptSource.References.Clear();

            var asms = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in asms)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("Alternet.UI");
            AddScriptItem();

            InitEditor();

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

                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                scriptObject = scriptRun.Run() as IDisposable;
                try
                {
                    scriptRun.RunFunction("Main", new object[] { "Catch me if you can" });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Script Execution Error");
                    StopScript();
                }

                scriptRunning = true;
                UpdateButtons();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                SafeDispose(ref scriptObject);
                timer.Stop();
                scriptRunning = false;
                TestButton.Text = "Test Button";
                UpdateButtons();
            }));
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

        private void InitEditor()
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);
            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
            syntaxEdit1.Lexer = pythonParser1;
        }
        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "ObjectReference.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = Path.Combine(DemoUtils.ResourcesFolder, ResourcesFolderName, sourceFileSubPath);
            return path;
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

        private void AddScriptItem()
        {
            ScriptGlobalItem item = new ScriptGlobalItem("RunButton", TestButton);
            ScriptGlobalItem item1 = new ScriptGlobalItem("timer", timer);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            scriptRun.GlobalItems.Add(item1);
        }
    }
}