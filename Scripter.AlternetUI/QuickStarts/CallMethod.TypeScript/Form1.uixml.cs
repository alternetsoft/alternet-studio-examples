#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Alternet.Drawing;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.TypeScript;
using Alternet.Syntax.Parsers.TypeScript;

using Alternet.UI;
using System.Reflection;
using Alternet.Common.AlternetUI;
using Alternet.Common;
using Alternet.Common.TypeScript.Compiler;

namespace CallMethod.TypeScript
{
    public partial class Form1 : Window
    {
        private readonly int typeScriptIndex;
        private readonly int javaScriptIndex;

        private ScriptRun scriptRun;
        private Timer? updateTimer;
        private Stopwatch? updateDeltaStopwatch = new();
        private volatile bool scriptRunning;

        private TypeScriptProject scriptProject;
        private TypeScriptParser? typeScriptParser;
        private Alternet.Syntax.Parsers.Generic.JScriptParser? javaScriptParser;

        static Form1()
        {
        }

        public Form1()
        {
            scriptProject = new();
            scriptRun = new();

            typeScriptParser = new(scriptProject);
            
            javaScriptParser = new();

            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            var typeScriptItem = "TypeScript";
            var javaScriptItem = "JavaScript";

            typeScriptIndex = cbLanguages.AddAndReturnIndex(typeScriptItem);
            javaScriptIndex = cbLanguages.AddAndReturnIndex(javaScriptItem);

            cbLanguages.Value = javaScriptItem;
            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            updateTimer = new Timer
            {
                Interval = 50,
            };

            updateTimer.Tick += UpdateTimer_Tick;

            ScriptButton.Click += RunScriptButton_Click;
            displayPanel.Paint += DisplayPanel_Paint;
            UpdateButtons();

            lbDescription.WordWrap = true;

            FormUtils.BindShown(this, () =>
            {
                UpdateSource(cbLanguages.IndexOfValue);
            });

            App.AddIdleTask(() =>
            {
                App.DoInsideBusyCursor(InitDefaultHostAssemblies);
            });
        }

        protected override void DisposeManaged()
        {
            StopScript();
            scriptRunning = false;
            SafeDispose(ref updateTimer);

            base.DisposeManaged();
        }

        public void StartScript()
        {
            App.DoInsideBusyCursor(() =>
            {
                Invoke(() =>
                {
                    var host = scriptRun.ScriptHost as ClearScriptScriptHost;
                    var isTypeScript = scriptRun.ScriptLanguage == ScriptLanguage.TypeScript;

                    scriptRun.ScriptSource.FromScriptCode(
                        syntaxEdit1.Text,
                        syntaxEdit1.Source.FileName);

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
                });
            });
        }

        public void StopScript()
        {
            Invoke(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer?.Stop();
                displayPanel.Background = Brushes.Gray;
                displayPanel.Refresh();
            });
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        protected void LoadFile(SyntaxEdit edit, string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var exists = fileInfo.Exists;
            ScriptButton.Enabled = exists;

            if (exists)
            {
                edit.Source.LoadFile(fileName);
                Debug.WriteLineIf(true, $"Loaded file: {fileName}");
            }
            else
            {
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
                Debug.WriteLineIf(true, $"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;
        }

        protected void InitDefaultHostAssemblies()
        {
            bool generateDescriptions = true;

            var options = HostItemOptions.GlobalMembers;

            if(generateDescriptions)
                options |= HostItemOptions.GenerateDescriptions;

            var assemblies = DemoUtils.LoadDefaultScriptAssemblies();

            scriptRun.ScriptHost.HostItemsConfiguration.AddAssemblies("core", assemblies, options);

            TypeScriptProject.DefaultProject.HostItemsConfiguration
                = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        protected virtual void GetSourceParametersForTypeScript(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.ts";
            language = ScriptLanguage.TypeScript;
        }

        protected virtual void GetSourceParametersForJavaScript(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.js";
            language = ScriptLanguage.JavaScript;
        }

        protected virtual string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
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

            try
            {
                scriptRun.RunFunction(
                    "OnPaint",
                    new object[] { e.Graphics, displayPanel.ClientRectangle});
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
                scriptRun.RunFunction(
                    "OnUpdate",
                    new object[] { updateDeltaStopwatch.ElapsedMilliseconds });

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

        private void UpdateSource(int? index)
        {
            if (index is null)
                return;
            StopScript();

            string sourceFileSubPath;
            ScriptLanguage language;

            if (index < 0)
                return;

            if (index == typeScriptIndex)
            {
                GetSourceParametersForTypeScript(out sourceFileSubPath, out language);
                syntaxEdit1.Lexer = typeScriptParser;
            }
            else
            if (index == javaScriptIndex)
            {
                GetSourceParametersForJavaScript(out sourceFileSubPath, out language);
                syntaxEdit1.Lexer = javaScriptParser;
            }
            else
            {
                GetSourceParametersForJavaScript(out sourceFileSubPath, out language);
                syntaxEdit1.Lexer = javaScriptParser;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateSource(cbLanguages.IndexOfValue);
        }
    }
}