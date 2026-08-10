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
using System.Reflection;

using Alternet.Drawing;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.TypeScript;
using Alternet.Syntax.Parsers.TypeScript;

using Alternet.UI;
using Alternet.Common.AlternetUI;
using Alternet.Common;
using Alternet.Common.TypeScript.Compiler;
using Alternet.Syntax.Parsers.TextMate;
using Alternet.Syntax;

namespace Mini.TypeScript
{
    public partial class Form1 : Window
    {
        private readonly int typeScriptSimpleIndex;
        private readonly int javaScriptSimpleIndex;

        private readonly TextMateParser textmateParser = new ();
        private readonly ScriptRun scriptRun;
        private readonly bool isDark;
        private readonly GlobalObject external = new();

        private volatile bool scriptRunning;
        private Alternet.Syntax.Parsers.Generic.JScriptParser? javaScriptParser;

        static Form1()
        {
        }

        public Form1()
        {
            scriptRun = new();
            scriptRun.LoadUserTypeDefinitions = true;
            scriptRun.AutoGenerateTypeDefinitions = true;

            javaScriptParser = new();

            InitializeComponent();

            isDark = this.BackColor.IsDark();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                isDark = true;
            }

            var typeScriptSimpleItem = "Simple TypeScript";
            var javaScriptSimpleItem = "Simple JavaScript";

            typeScriptSimpleIndex = cbLanguages.AddAndReturnIndex(typeScriptSimpleItem);
            javaScriptSimpleIndex = cbLanguages.AddAndReturnIndex(javaScriptSimpleItem);

            cbLanguages.Value = javaScriptSimpleItem;
            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            ScriptButton.Click += RunScriptButton_Click;
            UpdateButtons();

            lbDescription.WordWrap = true;

            FormUtils.BindShown(this, () =>
            {
                UpdateSource(cbLanguages.IndexOfValue);
            });
        }

        protected override void DisposeManaged()
        {
            StopScript();
            scriptRunning = false;

            base.DisposeManaged();
        }

        public void StartScript()
        {
            App.DoInsideBusyCursor(() =>
            {
                Invoke(() =>
                {
                    if (scriptRun.ScriptHost is not ClearScriptScriptHost host)
                    {
                        MessageBox.Show("ClearScriptScriptHost is not available.");
                        return;
                    }

                    var isTypeScript = scriptRun.ScriptLanguage == ScriptLanguage.TypeScript;
                    host.SetUniqueModulesDirectoryPath();

                    Debug.WriteLineIf(false, $"Unique modules directory path: {host.ModulesDirectoryPath}");

                    host.NeedSystemSupportCode = false;
                    host.HostItemsConfiguration.Clear();
                    host.HostItemsConfiguration.ResetToMinimal();

                    host.HostItemsConfiguration.AddObject("External", external);

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

                    scriptRun.RunWithoutCompile();

                    var s = string.Empty;

                    var engine = host?.Engine;

                    if (engine is not null)
                    {
                        var sum = engine.Evaluate("Calculator.add(5, 7)");
                        s += $"\r\nEvaluated via engine: Calculator.add(5, 7) = {sum}";

                        dynamic calc = engine.Script.Calculator;
                        var sub = calc.sub(3, 1);
                        s += $"\r\nDynamic call: Calculator.sub([3, 1]) = {sub}";
                    }
                    else
                    {
                        s = "Script engine is not available.";
                    }

                    App.Alert(s);
                });
            });
        }

        public void StopScript()
        {
            Invoke(() =>
            {
                scriptRunning = false;
                UpdateButtons();
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
            }
            else
            {
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;
        }

        protected virtual void GetSourceParametersForTypeScript(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodCalculator.ts";
            language = ScriptLanguage.TypeScript;
        }

        protected virtual void GetSourceParametersForJavaScript(
            out string sourceFileSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodCalculator.js";
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

        private void UpdateSource(int? index)
        {
            if (index is null)
                return;
            StopScript();

            string sourceFileSubPath;
            ScriptLanguage language;

            if (index < 0)
                return;

            if (index == typeScriptSimpleIndex)
            {
                GetSourceParametersForTypeScript(out sourceFileSubPath, out language);

                textmateParser.ThemeName = isDark ? ThemeName.DarkPlus : ThemeName.LightPlus;

                syntaxEdit1.Lexer = textmateParser;

                syntaxEdit1.VisualTheme = new TextMateTheme(textmateParser.LanguageDefinition.ThemeColors);
                syntaxEdit1.VisualThemeType = VisualThemeType.Custom;
            }
            else
            {
                GetSourceParametersForJavaScript(out sourceFileSubPath, out language);
                syntaxEdit1.Lexer = javaScriptParser;

                syntaxEdit1.VisualThemeType = isDark ? VisualThemeType.Dark : VisualThemeType.Light;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);

            LoadFile(syntaxEdit1, sourceFileFullPath);

            textmateParser.FileName = sourceFileFullPath;

            scriptRun.ScriptLanguage = language;

            syntaxEdit1.ReparseAllText();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateSource(cbLanguages.IndexOfValue);
        }
    }
}