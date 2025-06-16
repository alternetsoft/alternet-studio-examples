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
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace CallMethod
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new ScriptRun();
        private Timer? updateTimer;
        private Stopwatch? updateDeltaStopwatch = new();
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

            if(Consts.IsWindows)
                cbLanguages.Items.Add("Visual Basic");

            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            updateTimer = new Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;

            cbLanguages.SelectedIndex = 0;
            ScriptButton.Click += RunScriptButton_Click;
            displayPanel.Paint += DisplayPanel_Paint;
            UpdateButtons();

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
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
            var asms = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in asms)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }
            csParser.Repository.RegisterAssemblies(asms);
            vbParser.Repository.RegisterAssemblies(asms);
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
            sourceFileSubPath = "CallMethod.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.vb";
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

        private void DisplayPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            scriptRun.RunMethod("OnPaint", null, new object[] { e.Graphics, displayPanel.ClientRectangle });
            updateDeltaStopwatch?.Restart();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!scriptRunning || updateDeltaStopwatch is null)
                return;

            scriptRun.RunMethod("OnUpdate", null, new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });
            updateDeltaStopwatch.Restart();
            displayPanel.Refresh();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbLanguages.SelectedIndex is null)
                return;
            StopScript();
            UpdateSource(cbLanguages.SelectedIndex.Value);
        }
    }
}