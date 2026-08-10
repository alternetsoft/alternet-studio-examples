#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.IO;

using Alternet.UI;

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
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
        private readonly Stopwatch? updateDeltaStopwatch = new();
        private readonly CsSolution csSolution = new();
        private readonly VbSolution vbSolution = new();
        private readonly CsParser csParser;
        private readonly VbParser vbParser;
        private readonly ScriptRun scriptRun = new ();

        private Timer? updateTimer;
        private volatile bool scriptRunning;

        public Form1()
        {
            var assemblies = DemoUtils.DefaultScriptAssemblies;

            csParser = new(csSolution);
            vbParser = new(vbSolution);

            csParser.Repository.RegisterAssemblies(assemblies);
            vbParser.Repository.RegisterAssemblies(assemblies);

            InitializeComponent();

            cbLanguages.Add("C#");
            cbLanguages.Add("Visual Basic");
            cbLanguages.Value = "C#";

            scriptRun.ScriptSource.SetReferences(assemblies);

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            updateTimer = new Timer
            {
                Interval = 50,
            };

            updateTimer.Tick += UpdateTimer_Tick;

            ScriptButton.Click += RunScriptButton_Click;
            displayPanel.Paint += DisplayPanel_Paint;
            UpdateButtons();

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;

            syntaxEdit1.Text = "Loading text...";

            FormUtils.BindShown(this, () =>
            {
                UpdateSource(0);
            });
        }

        protected override void DisposeManaged()
        {
            scriptRunning = false;
            SafeDispose(ref updateTimer);

            base.DisposeManaged();
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
                        var errors = scriptRun.ScriptHost.CompilerErrors
                        .Select(x => x.ToString()).ToArray();
                        MessageBox.Show(string.Join("\r\n", errors));
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
                syntaxEdit1.Text = string.Empty;
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
                syntaxEdit1.Lexer = null;
            }

            edit.Source.FileName = fileName;
        }

        protected virtual void GetSourceParametersForCSharp(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.cs";
            language = ScriptLanguage.CSharp;
        }

        protected virtual void GetSourceParametersForVisualBasic(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.vb";
            language = ScriptLanguage.VisualBasic;
        }

        protected virtual string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
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

            EditUtilities.Reparse(syntaxEdit1);

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

            try
            {
                scriptRun.RunMethod(
                    "OnPaint",
                    null,
                    new object[] { e.Graphics, displayPanel.ClientRectangle });

                updateDeltaStopwatch?.Restart();
            }
            catch (Exception ex)
            {
                StopScript();

                App.AddIdleTask(() =>
                {
                    App.AlertException(ex);
                });
            }
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!scriptRunning || updateDeltaStopwatch is null)
                return;

            try
            {
                scriptRun.RunMethod(
                    "OnUpdate",
                    null,
                    new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });
                updateDeltaStopwatch.Restart();
                displayPanel.Refresh();
            }
            catch (Exception ex)
            {
                StopScript();

                App.AddIdleTask(() =>
                {
                    App.AlertException(ex);
                });
            }
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = cbLanguages.IndexOfValue;
            if (index is null)
                return;
            StopScript();
            UpdateSource(index.Value);
        }
    }
}