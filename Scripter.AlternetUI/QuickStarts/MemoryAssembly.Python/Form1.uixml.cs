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
using System.Linq;
using System.Reflection;

using Alternet.Common.DotNet;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Python;
using Alternet.UI;

namespace MemoryAssembly.Python
{
    public partial class Form1 : Window
    {
        internal static bool HighlightAssemblyScriptEditor = true;

        internal static bool GenerateModulesOnDisk = false;

        internal string dir = Application.StartupPath + @"\";

        private readonly ScriptRun scriptRun = new();
        private readonly PythonNETParser pythonParser1 = new();

        private readonly CsParser csParser1 = new();

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

            AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            {
                if (ea.Name.Contains("ExternalAssembly"))
                {
                    var stream = this.GetType().Assembly.GetManifestResourceStream("MemoryAssembly.Python.ExternalAssembly.dll");
                    if (stream is null)
                        return null;
                    return Assembly.Load(ReadFully(stream));
                }

                return null;
            };

            var assemblies = DemoUtils.DefaultScriptAssemblies;
            scriptRun.ScriptSource.SetReferences(assemblies);

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;

            scriptRun.ScriptSource.References.Add("ExternalAssembly");
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("ExternalAssembly");
            ScriptButton.Click += RunScriptButton_Click;

            lbDescription.WordWrap = true;
            pnSettings.MinSizeGrowMode = WindowSizeToContentMode.Height;
            ActiveControl = edit;

            GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath);
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
            edit.Lexer = pythonParser1;

            editExt.Lexer = csParser1;
            editExt.ReadOnly = true;

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
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using MemoryStream ms = new ();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);

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

                try
                {
                    scriptRun.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                    throw;
                }
            }));
        }

        protected virtual void GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath)
        {
            sourceFileSubPath = "CustomAssemblyTest.py";
            sourceExternalSubPath = "CustomClass.cs";
        }

        protected virtual string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
            return path;
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

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            StartScript();
        }
    }
}