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
using System.IO;
using System.Linq;

using Alternet.Common.AlternetUI;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.TypeScript;
using Alternet.Syntax.Parsers.TypeScript;

using Alternet.UI;

namespace ObjectReference.TypeScript
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new ();
        private IDisposable? scriptObject;

        private volatile bool scriptRunning;

        private TypeScriptParser typeScriptParser = new ();
        private JavaScriptParser javaScriptParser = new ();

        private Timer timer = new ();

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.Add("TypeScript");
            cbLanguages.Add("JavaScript");
            cbLanguages.Value = "TypeScript";
            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            syntaxEdit1.Outlining.AllowOutlining = true;

            syntaxEdit1.ContextMenuStrip = syntaxEdit1.DefaultMenu;

            if (DebugUtils.IsDebugDefined)
            {
                syntaxEdit1.ContextMenuStrip.AddSeparator();
                syntaxEdit1.ContextMenuStrip.Add(ControlUtilities.CreateShowAlternetSpecialFoldersMenu());
            }

            ScriptButton.Click += RunScriptButton_Click;
            TestButton.Click += TestButton_Click;
            UpdateButtons();

            lbDescription.WordWrap = true;

            InitDefaultHostAssemblies();
            InitEditor();
        }

        protected override void DisposeManaged()
        {
            StopScript();
            scriptRunning = false;
            SafeDispose(ref scriptObject);

            base.DisposeManaged();
        }

        public void StartScript()
        {
            App.DoInsideBusyCursor(() =>
            {
                Invoke((Action)(() =>
                {
                    scriptRun.ScriptSource.FromScriptCode(
                        syntaxEdit1.Text,
                        syntaxEdit1.Source.FileName);

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

                    scriptObject = scriptRun.Run() as IDisposable;
                    try
                    {
                        scriptRun.RunFunction("RunMe");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Script Execution Error");
                        StopScript();
                    }

                    scriptRunning = true;
                    UpdateButtons();
                }));
            });
        }

        public void StopScript()
        {
            Invoke((Action)(() =>
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
            GetSourceParametersForTypeScript(out string sourceFileSubPath, out ScriptLanguage language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);
            syntaxEdit1.Lexer = typeScriptParser;
            scriptRun.ScriptLanguage = language;
        }

        private static void GetSourceParametersForTypeScript(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReference.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions).AddAssemblies("core", DemoUtils.DefaultScriptAssemblies, options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions)
                .AddObject("RunButton", TestButton)
                .AddObject("timer", timer);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void GetSourceParametersForJavaScript(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReference.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
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

        private void TestButton_Click(object? sender, EventArgs e)
        {
            StopScript();
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForTypeScript(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = typeScriptParser;
                    break;
                default:
                    GetSourceParametersForJavaScript(out sourceFileSubPath, out language);
                    syntaxEdit1.Lexer = javaScriptParser;
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(syntaxEdit1, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
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