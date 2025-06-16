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
using System.Diagnostics;
using System.IO;
using System.Linq;

using Alternet.Drawing;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;

using Alternet.UI;

namespace CallMethod.Python
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new ();
        private Timer? updateTimer;
        private Stopwatch? updateDeltaStopwatch = new();
        private volatile bool scriptRunning;
        private PythonNETParser pythonParser1 = new ();
        private double currentAngle = 0;

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }


            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
            scriptRun.ScriptSource.Imports.Add("Alternet.UI");

            InitEditor();

            updateTimer = new Timer
            {
                Interval = 50,
            };

            updateTimer.Tick += UpdateTimer_Tick;

            ScriptButton.Click += RunScriptButton_Click;
            displayPanel.Paint += DisplayPanel_Paint;
            UpdateButtons();

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
        }

        protected override void DisposeManaged()
        {
            scriptRunning = false;
            SafeDispose(ref updateTimer);

            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in assemblies)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text, "CallMethod.py");
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        var errors = scriptRun.ScriptHost.CompilerErrors;
                        var msg = string.Join("\r\n", errors.Select(x => x.ToString()).ToArray());
                        MessageBox.Show(msg);
                        return;
                    }
                }

                scriptRunning = true;
                UpdateButtons();
                displayPanel.Background = Brushes.Transparent;
                displayPanel.Refresh();
                updateTimer?.Start();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer?.Stop();
                displayPanel.Background = Brushes.Gray;
                displayPanel.Refresh();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
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

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "CallMethod.py";
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

        private void DisplayPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            scriptRun.RunFunction(
                "OnPaint",
                new object[] { e.Graphics, displayPanel.ClientRectangle, currentAngle });
            updateDeltaStopwatch?.Restart();
        }

        private double ConstrainAngle(double x)
        {
            x %= 360;
            if (x < 0)
                x += 360;

            return x;
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!scriptRunning || updateDeltaStopwatch is null)
                return;

            currentAngle += (int)updateDeltaStopwatch.ElapsedMilliseconds * 0.1;
            currentAngle = ConstrainAngle(currentAngle);

            updateDeltaStopwatch.Restart();
            displayPanel.Refresh();
        }
    }
}