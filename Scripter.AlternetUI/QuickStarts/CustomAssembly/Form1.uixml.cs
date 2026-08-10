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

namespace CustomAssembly
{
    public partial class Form1 : Window
    {
        internal static bool HighlightAssemblyScriptEditor = true;

        internal static bool GenerateModulesOnDisk = false;

        private ScriptRun scriptRun = new();

        private readonly CsSolution csSolution = new ();
        private readonly VbSolution vbSolution = new();

        private readonly CsParser csParser;
        private readonly VbParser vbParser;
        private readonly CsParser csParser2;
        private readonly VbParser vbParser2;

        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            csParser = new(csSolution);
            vbParser = new(vbSolution);
            csParser2 = new(csSolution);
            vbParser2 = new(vbSolution);

            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                edit.VisualThemeType = VisualThemeType.Dark;
                editExt.VisualThemeType = VisualThemeType.Dark;
            }

            cbLanguages.Add("C#");
            cbLanguages.Add("Visual Basic");

            edit.Dock = DockStyle.Fill;
            editExt.Dock = DockStyle.Fill;

            edit.Outlining.AllowOutlining = true;
            editExt.Outlining.AllowOutlining = true;

            var assemblies = DemoUtils.DefaultScriptAssemblies;
            scriptRun.ScriptSource.SetReferences(assemblies);

            scriptRun.ScriptSource.References.Add("ExternalAssembly");

            if (File.Exists(Path.Combine(dir, "ExternalAssembly.dll")))
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            else
            {
                dir = dir + @"..\..\..\";
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            }

            csParser.Repository.RegisterAssemblies(assemblies);
            vbParser.Repository.RegisterAssemblies(assemblies);

            scriptRun.ScriptHost.GenerateModulesOnDisk = GenerateModulesOnDisk;

            cbLanguages.Value = "C#";

            lbDescription.WordWrap = true;
            pnSettings.MinSizeGrowMode = WindowSizeToContentMode.Height;
            ActiveControl = edit;

            // This will be useful in case when scriptRun.ScriptHost.GenerateModulesOnDisk = false;
            AppDomain.CurrentDomain.AssemblyResolve += (s, args) =>
            {
                var assemblyName = new System.Reflection.AssemblyName(args.Name);
                if (assemblyName.Name == "ExternalAssembly")
                {
                    var assemblyPath = Path.Combine(dir, "ExternalAssembly.dll");
                    if (File.Exists(assemblyPath))
                        return System.Reflection.Assembly.LoadFrom(assemblyPath);
                }
                return null;
            };

            edit.Text = "Loading Text...";

            FormUtils.BindShown(this, () =>
            {
                UpdateSource(0);
                ScriptButton.Click += RunScriptButton_Click;
                cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);
                scriptRun.ScriptHost.GenerateModulesOnDisk = GenerateModulesOnDisk;
                scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
                scriptRun.Platform = ScriptPlatform.AnyCpu;

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

                scriptRun.Run();
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
                edit.Text = string.Empty;
                edit.Lines.Add($"File not found: {fileInfo.FullName}");
                edit.Lexer = null;
            }

            edit.Source.FileName = fileName;
        }

        protected virtual void GetSourceParametersForCSharp(
            out string sourceFileSubPath,
            out string sourceExternalSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTest.cs";
            sourceExternalSubPath = "CustomClass.cs";
            language = ScriptLanguage.CSharp;
        }

        protected virtual void GetSourceParametersForVisualBasic(
            out string sourceFileSubPath,
            out string sourceExternalSubPath,
            out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTest.vb";
            sourceExternalSubPath = "CustomClass.vb";
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
            string sourceExternalSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(
                        out sourceFileSubPath,
                        out sourceExternalSubPath,
                        out language);
                    edit.Lexer = csParser;

                    if (HighlightAssemblyScriptEditor)
                        editExt.Lexer = csParser2;
                    break;
                default:
                    GetSourceParametersForVisualBasic(
                        out sourceFileSubPath,
                        out sourceExternalSubPath,
                        out language);
                    edit.Lexer = vbParser;

                    if (HighlightAssemblyScriptEditor)
                        editExt.Lexer = vbParser2;
                    break;
            }

            editExt.ReadOnly = true;

            scriptRun.ScriptLanguage = language;

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, sourceFileFullPath);
            EditUtilities.Reparse(edit, editExt);
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            StartScript();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = cbLanguages.IndexOfValue;
            if (index is null)
                return;
            Invoke(()=>
            {
                App.DoInsideBusyCursor(() =>
                {
                    UpdateSource(index.Value);
                });
            });
        }
    }
}