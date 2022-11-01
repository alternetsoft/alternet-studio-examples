#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;
using Alternet.Syntax;

namespace CustomAssembly
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private IScriptEdit edit;
        private IScriptEdit editExt;
        private string dir = Application.StartupPath + @"\";

        public MainForm()
        {
            InitializeComponent();

            CreateEditor();
            cbLanguages.SelectedIndex = 0;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            var parser = (edit as ScriptCodeEdit).Lexer as ISyntaxParser;
            if (parser != null)
                parser.ReparseText();
        }

        private void CreateEditor()
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;

            GetSourceParametersForCSharp(out sourceFileSubPath, out sourceExternalSubPath, out language);

            editExt = CreateEdit(GetSourceFileFullPath(sourceExternalSubPath), tpExternal, true);
            edit = CreateEdit(GetSourceFileFullPath(sourceFileSubPath), tpEditor, false);
        }

        private IScriptEdit CreateEdit(string fileName, Control parent, bool readOnly)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;
            edit.InitSyntax();
            LoadFile(edit, fileName);

            edit.ReadOnly = readOnly;
            return edit;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(dir, "ExternalAssembly.dll")))
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            else
            {
                dir = dir + @"..\..\..\";
                scriptRun.ScriptSource.SearchPaths.Add(dir);
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.FromScriptCode(edit.Text);
            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.ScriptSource.References.Add("ExternalAssembly");
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

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForCSharp(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
            }

            scriptRun.ScriptLanguage = language;
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTest.cs";
            sourceExternalSubPath = "CustomClass.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTest.vb";
            sourceExternalSubPath = "CustomClass.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
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
