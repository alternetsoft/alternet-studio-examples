#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Common;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Python;
using Alternet.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CustomAssembly.Python
{
    public partial class Form1 : Window
    {
        internal static bool HighlightAssemblyScriptEditor = true;

        internal static bool GenerateModulesOnDisk = false;

        private ScriptRun? scriptRun = new();

        private PythonNETParser? pythonParser1 = new();
        private CsParser? csParser1 = new();

        private string dir = Application.StartupPath + @"\";

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                edit.VisualThemeType = VisualThemeType.Dark;
                editExt.VisualThemeType = VisualThemeType.Dark;
            }

            edit.Dock = DockStyle.Fill;
            editExt.Dock = DockStyle.Fill;

            edit.Outlining.AllowOutlining = true;
            editExt.Outlining.AllowOutlining = true;

            scriptRun.ScriptSource.SetReferences(DemoUtils.DefaultScriptAssemblies);

            if (File.Exists(Path.Combine(dir, "ExternalAssembly.dll")))
                scriptRun.ScriptSource.ReferencesSearchPaths.Add(dir);
            else
            {
                dir = dir + @"..\..\..\";
                scriptRun.ScriptSource.ReferencesSearchPaths.Add(dir);
            }

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;
            scriptRun.ScriptSource.References.Add("ExternalAssembly.dll");
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("ExternalAssembly");

            ScriptButton.Click += RunScriptButton_Click;

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

            FormUtils.BindShown(this, () => LoadData());
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref scriptRun);
            edit.Lexer = null;
            editExt.Lexer = null;
            SafeDispose(ref pythonParser1);
            SafeDispose(ref csParser1);

            base.DisposeManaged();
        }

        private void LoadData()
        {
            if (File.Exists(Path.Combine(dir, "ExternalAssembly.dll")))
                scriptRun?.ScriptSource.ReferencesSearchPaths.Add(dir);
            else
            {
                dir += dir + @"..\..\..\";
                scriptRun?.ScriptSource.ReferencesSearchPaths.Add(dir);
            }

            GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath);
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
            edit.Lexer = pythonParser1;
            editExt.Lexer = csParser1;

            editExt.ReadOnly = true;
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun?.ScriptSource.FromScriptCode(edit.Text);

                if (!scriptRun?.Compiled ?? false)
                {
                    if (!scriptRun?.Compile() ?? false)
                    {
                        var errors = scriptRun?.ScriptHost.CompilerErrors
                        .Select(x => x.ToString()).ToArray() ?? Array.Empty<string>();
                        MessageBox.Show(string.Join("\r\n", errors));
                        return;
                    }
                }

                scriptRun?.RunFunction("Main", new object[] { });
            }));
        }

        private static void GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath)
        {
            sourceFileSubPath = "CustomAssemblyTest.py";
            sourceExternalSubPath = "CustomClass.cs";
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
                edit.Lines.Add($"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;
        }

        private static string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
            return path;
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            StartScript();
        }
    }
}