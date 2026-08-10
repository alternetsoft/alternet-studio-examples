#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.UI;
using System;
using System.IO;
using System.Linq;

namespace ScriptHostObject
{
    public partial class Form1 : Window
    {
        private readonly ScriptRun scriptRun = new ();

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;
            InitEditor();

            ScriptButton.Click += RunScriptButton_Click;

            lbDescription.WordWrap = true;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected virtual string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(Path.Combine(ResourcesFolderName, sourceFileSubPath));
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        protected virtual void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ScriptHostObject.csx";
            language = ScriptLanguage.CSharpScript;
        }

        private void InitEditor()
        {
            GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);

            syntaxEdit1.Source.FileName = sourceFileFullPath;

            LoadFile(syntaxEdit1, sourceFileFullPath, ScriptLanguage.CSharpScript);
            scriptRun.ScriptLanguage = language;
            InitScripter();
        }

        private void LoadFile(SyntaxEdit edit, string fileName, ScriptLanguage language)
        {
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                edit.LoadFile(fileName);
                ScriptButton.Enabled = true;
            }
            else
            {
                syntaxEdit1.Lines.Add($"File not found: {fileInfo.FullName}");
            }

            edit.Source.FileName = fileName;

            RoslynParser? parser;
            RoslynSolution? solution;

            switch (language)
            {
                case ScriptLanguage.CSharpScript:
                case ScriptLanguage.CSharp:
                    solution = new CsSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script, typeof(Globals));
                    parser = new CsParser(solution);
                    break;
                case ScriptLanguage.VisualBasicScript:
                case ScriptLanguage.VisualBasic:
                    solution = new VbSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script);
                    parser = new VbParser(solution);
                    break;
                default:
                    throw new NotSupportedException("Language not supported: " + language);
            }

            edit.Lexer = parser;
            edit.Source.FileName = fileName;

            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            parser?.Repository.RegisterAssemblies(assemblies);
        }

        private void InitScripter()
        {
            var global = new Globals
            {
                LabelExpression = ExpressionLabel,
            };

            scriptRun.ScriptHost.HostGlobalObject = global;
            scriptRun.ScriptSource.AddReferences(DemoUtils.DefaultScriptAssemblies);
        }

        private void RunScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);
                scriptRun.ScriptSource.References.Add(typeof(Globals).Assembly.Location);
                scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

                if (!scriptRun.CompileIfNotCompiled())
                {
                    MessageBox.Show(scriptRun.ScriptHost.CompilerErrorsAsString());
                    return;
                }

                scriptRun.Run();
            }));
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            RunScript();
        }
    }
}