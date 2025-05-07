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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;
using Alternet.Scripter;
using Alternet.Scripter.TypeScript;

namespace ObjectReference.TypeScript
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private bool scriptRunning;
        private IScriptEdit edit;
        private string dir = Application.StartupPath + @"\";
        private dynamic catcher;

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "ObjectReference.TypeScript.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitDefaultHostAssemblies();

            CreateEditor();
            cbLanguages.SelectedIndex = 0;
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
            ScriptLanguage language;
            GetSourceParametersForTS(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, pnEdit);
        }

        private IScriptEdit CreateEditor(string fileName, Control parent)
        {
            IScriptEdit edit;
            edit = new Alternet.Editor.Common.ScriptCodeEdit();
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;
            edit.InitSyntax();

            LoadFile(edit, fileName);
            return edit;
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions)
                 .AddObject("RunButton", btNETFromScript);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void StartScript()
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

            catcher = scriptRun.RunFunction("RunMe");

            scriptRunning = true;
            btRun.Text = "Stop Script";
        }

        private void StopScript()
        {
            if (catcher != null)
                catcher.Dispose();
            catcher = null;
            scriptRunning = false;
            btRun.Text = "Run Script";
            btNETFromScript.Text = "Test Button";
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForTS(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\ObjectReference.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\ObjectReference.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript";
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

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (!scriptRunning)
                StartScript();
            else
                StopScript();
        }

        private void NETFromScriptButton_Click(object sender, EventArgs e)
        {
            StopScript();
        }
    }
}
