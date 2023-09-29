#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Editor;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace ScriptHostObject
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private ISyntaxEdit edit;

        public MainForm()
        {
            InitializeComponent();
        }

        private ISyntaxEdit CreateEditor(string fileName, Control parent)
        {
            ISyntaxEdit edit;
            edit = new SyntaxEdit();
            edit.Source.FileName = fileName;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName, ScriptLanguage.CSharpScript);

            return edit;
        }

        private void LoadFile(ISyntaxEdit edit, string fileName, ScriptLanguage language)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.Source.FileName = fileName;
            RoslynParser parser = null;
            switch (language)
            {
                case ScriptLanguage.CSharpScript:
                    var solution = new CsSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script, typeof(Globals));
                    parser = new CsParser(solution);
                    break;
            }

            edit.Lexer = parser;
        }

        private void CreateEditor(Control parent)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, parent);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateEditor(pnEdit);

            cbLanguages.SelectedIndex = 0;
        }

        private void InitScripter()
        {
            var global = new Globals();
            global.LabelExpression = this.ExpressionLabel;
            scriptRun.ScriptHost.HostGlobalObject = global;
        }

        private void StartScript()
        {
            scriptRun.ScriptSource.FromScriptCode(edit.Source.Text);
            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.ScriptSource.References.Add(typeof(Globals).Assembly.Location);
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }
            }

            scriptRun.Run();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            StartScript();
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ScriptHostObject.csx";
            language = ScriptLanguage.CSharpScript;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ScriptHostObject.vbx";
            language = ScriptLanguage.VisualBasicScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath, language);

            scriptRun.ScriptLanguage = language;
            InitScripter();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }
    }
}
