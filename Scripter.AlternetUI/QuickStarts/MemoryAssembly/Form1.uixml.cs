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

using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.UI;

namespace MemoryAssembly
{
    public partial class Form1 : Window
    {
        internal static bool HighlightAssemblyScriptEditor = true;

        internal static bool GenerateModulesOnDisk = false;

        internal string dir = Application.StartupPath + @"\";

        private readonly ScriptRun scriptRun = new();

        private readonly CsSolution csSolution = new ();
        private readonly VbSolution vbSolution = new();

        private readonly CsParser csParser;
        private readonly VbParser vbParser;
        private readonly CsParser csParser2;
        private readonly VbParser vbParser2;

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

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            edit.Dock = DockStyle.Fill;
            editExt.Dock = DockStyle.Fill;

            edit.Outlining.AllowOutlining = true;
            editExt.Outlining.AllowOutlining = true;

            this.scriptRun.ReferenceResolve += ScriptRun_ReferenceResolve;

            AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            {
                if (ea.Name.Contains("ExternalAssembly"))
                {
                    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MemoryAssembly.ExternalAssembly.dll");
                    if (stream == null)
                        return null;
                    return Assembly.Load(ReadFully(stream));
                }

                return null;
            };

            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in assemblies)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            scriptRun.ScriptSource.References.Add("ExternalAssembly");

            if (File.Exists(Path.Combine(dir, "ExternalAssembly.dll")))
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            else
            {
                dir += @"..\..\..\";
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            }

            csParser.Repository.RegisterAssemblies(assemblies);
            vbParser.Repository.RegisterAssemblies(assemblies);

            scriptRun.ScriptHost.GenerateModulesOnDisk = GenerateModulesOnDisk;

            cbLanguages.Value = "C#";
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
                scriptRun.ScriptSource.WithDefaultReferences();
                scriptRun.ScriptSource.References.Add("ExternalAssembly");
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

                    if(HighlightAssemblyScriptEditor)
                        editExt.Lexer = csParser2;
                    break;
                default:
                    GetSourceParametersForVisualBasic(
                        out sourceFileSubPath,
                        out sourceExternalSubPath,
                        out language);
                    edit.Lexer = vbParser;

                    if(HighlightAssemblyScriptEditor)
                        editExt.Lexer = vbParser2;
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            editExt.ReadOnly = true;
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));

            scriptRun.ScriptLanguage = language;

            Reparse();

            void Reparse()
            {
                foreach (var editor in new SyntaxEdit[] { edit, editExt })
                {
                    var l = editor.Lexer;
                    editor.Lexer = null;
                    editor.Lexer = l;

                    (editor.Lexer as CsParser)?.ReparseText();
                    (editor.Lexer as VbParser)?.ReparseText();
                }
            };

           RunWhenIdle(() =>
           {
               Reparse();
           });
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

        private void ScriptRun_ReferenceResolve(object? sender, Alternet.Scripter.ResolveReferenceEventArgs e)
        {
            if (e.Reference == "ExternalAssembly")
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MemoryAssembly.ExternalAssembly.dll");

                if (stream == null)
                    return;

                byte[] bytes = ReadFully(stream);

                e.AssemblyImage = bytes;
                e.Handled = true;
            }
        }
    }
}