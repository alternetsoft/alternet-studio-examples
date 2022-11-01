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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;
using Alternet.Scripter.TypeScript;

namespace CustomAssembly.TypeScript
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
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor()
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;

            GetSourceParametersForTS(out sourceFileSubPath, out sourceExternalSubPath, out language);

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEdit(GetSourceFileFullPath(sourceFileSubPath), tpEditor, false);
            editExt = CreateEdit(GetSourceFileFullPath(sourceExternalSubPath), tpExternal, true);
        }

        private IScriptEdit CreateEdit(string fileName, Control parent, bool readOnly)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName);

            edit.ReadOnly = readOnly;
            return edit;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitDefaultHostAssemblies();
            CreateEditor();
            cbLanguages.SelectedIndex = 0;
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            var externalDll = @"lib\ExternalAssembly.dll";
            AddAssembly(GetSourceFileFullPath(externalDll));

            var project = Alternet.Common.TypeScript.TypeScriptProject.DefaultProject;
            project.TypeDefinitionsSearchPaths = scriptRun.TypeDefinitionsSearchPaths = new[] { GetSourceDirectoryFullPath("lib") };
            project.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForTS(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
            }

            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));

            scriptRun.ScriptLanguage = language;
        }

        private void AddAssembly(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string alias = "_" + name;

            foreach (HostItem item in scriptRun.ScriptHost.HostItemsConfiguration.Items)
            {
                if (item is AssemblyCollectionHostItem)
                {
                    if (string.Compare(item.Name, alias) == 0)
                        return;
                }
            }

            scriptRun.ScriptHost.HostItemsConfiguration.AddAssembly(
                alias,
                path,
                HostItemOptions.GlobalMembers,
                new[] { GetSourceDirectoryFullPath("lib") });
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\CustomAssemblyTest.ts";
            sourceExternalSubPath = @"TS\CustomClass.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\CustomAssemblyTest.js";
            sourceExternalSubPath = @"JS\CustomClass.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private string GetSourceDirectoryFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!Directory.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!Directory.Exists(path))
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

        private void RunButton_Click(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.FromScriptCode(edit.Text);

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
    }
}
