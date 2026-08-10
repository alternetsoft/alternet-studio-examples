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
using Alternet.Common.Projects.DotNet;
using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.UI;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PackageReference
{
    public partial class Form1 : Window
    {
        protected DotNetProject Project { get; private set; } = new DotNetProject();

        private static readonly string[] ProjectSearchDirectories = new[] { "", @"..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Scripter.AlternetUI\PackageReferenceTest\PackageReferenceTest.csproj";

        private readonly Alternet.Syntax.Parsers.Roslyn.CodeCompletion.CsSolution solution = new();
        private readonly ScriptRun scriptRun = new ScriptRun();
        private Microsoft.CodeAnalysis.ProjectId? projectId;

        private static string? FindProjectFile()
        {
            return ProjectSearchDirectories.Select(
                x => DemoUtils.GetResourceFileFullPath(Path.Combine(x, StartupProjectFileSubPath)))
                .FirstOrDefault(File.Exists);
        }

        public Form1()
        {
            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;

            syntaxEdit1.Text = "Loading...";

            ScriptButton.Enabled = false;
            ScriptButton.Click += RunScriptButton_Click;
            lbDescription.WordWrap = true;

            FormUtils.BindShown(this, () =>
            {
                OpenProject(FindProjectFile());
                ScriptButton.Enabled = true;
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void OpenProject(string? projectFilePath)
        {
            if (string.IsNullOrEmpty(projectFilePath))
                return;
            Project.Load(projectFilePath);

            var projName = Project.ProjectName;
            var projPath = Project.ProjectFileName;

            projectId = solution.AddProject(projName, projPath);

            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;

            if (Project.Files.Count > 0)
            {
                InitEditor(Project.Files[0]);
            }
        }

        private void InitEditor(string fileName)
        {
            List<string> references = [];
            foreach (var reference in Project.References)
            {
                if (string.IsNullOrEmpty(reference.FullName))
                    continue;
                references.Add(reference.FullName);
            }

            var parser = new CsParser(solution)
            {
                ProjectName = Project.ProjectName,
                FileName = fileName,
            };

            parser.Repository.RegisterDefaultAssemblies(TechnologyEnvironment.System);
            parser.Repository.RegisterAssemblies(references.ToArray());
            syntaxEdit1.Lexer = parser;

            LoadFile(syntaxEdit1, fileName);
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

        private void SaveIfModified()
        {
            if (syntaxEdit1.Modified && !string.IsNullOrEmpty(syntaxEdit1.Source.FileName))
                syntaxEdit1.SaveFile(syntaxEdit1.Source.FileName);
        }

        private void RunScript()
        {
            SaveIfModified();

            BeginInvoke((Action)(() =>
            {
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                scriptRun.RunProcess();
            }));
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            RunScript();
        }
    }
}