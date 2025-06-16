#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;

namespace PackageReference
{
    public partial class MainForm : Form
    {
        protected DotNetProject Project { get; private set; } = new DotNetProject();

        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger\CS\PackageReferenceTest\PackageReferenceTest.csproj";

        private ISyntaxEdit edit;

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "PackageReference.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private static string FindProjectFile() =>
           ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private ISyntaxEdit CreateEditor(string fileName, Control parent)
        {
            ISyntaxEdit edit = new SyntaxEdit();
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            IList<string> references = new List<string>();
            foreach (var reference in Project.References)
                references.Add(reference.FullName);

            var parser = new CsParser();
            parser.Repository.RegisterAssemblies(references.ToArray());
            edit.Lexer = parser;

            LoadFile(edit, fileName);

            return edit;
        }

        private void LoadFile(ISyntaxEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.Source.FileName = fileName;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenProject(FindProjectFile());
        }

        private void OpenProject(string projectFilePath)
        {
            Project.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;

            if (Project.Files.Count > 0)
            {
                edit = CreateEditor(Project.Files[0], pnEdit);
            }
        }

        private void SaveIfModified()
        {
            if (edit.Modified && !string.IsNullOrEmpty(edit.Source.FileName))
                edit.SaveFile(edit.Source.FileName);
        }

        private void RunScript()
        {
            SaveIfModified();

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }
            }

            scriptRun.RunProcess();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            RunScript();
        }
    }
}
