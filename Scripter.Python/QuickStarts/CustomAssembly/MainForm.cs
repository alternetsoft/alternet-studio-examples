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
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Editor.Common;
using Alternet.Scripter.Python.Embedded;

namespace CustomAssembly
{
    public partial class MainForm : Form
    {
        private IScriptEdit edit;
        private IScriptEdit editExt;

        public MainForm()
        {
            SetupPython();

            InitializeComponent();

            scriptRun.ScriptSource.ReferencesSearchPaths.Add(Path.GetDirectoryName(GetType().Assembly.Location));
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.WindowsForms;
            scriptRun.ScriptSource.References.Add("ExternalAssembly.dll");
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Forms");
            scriptRun.ScriptSource.Imports.Add("ExternalAssembly");

            CreateEditor();
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled(true))
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
                ProgressBarStyle = ProgressBarStyle.Marquee,
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython(true);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void CreateEditor()
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;

            GetSourceParametersForPython(out sourceFileSubPath, out sourceExternalSubPath);

            edit = CreateEdit(GetSourceFileFullPath(sourceFileSubPath), tpEditor, false);
            editExt = CreateEdit(GetSourceFileFullPath(sourceExternalSubPath), tpExternal, true);
        }

        private IScriptEdit CreateEdit(string fileName, Control parent, bool readOnly)
        {
            var edit = new ScriptCodeEdit();
            edit.FileName = fileName;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            FileInfo fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                edit.LoadFile(fileInfo.FullName);
            }

            edit.ReadOnly = readOnly;
            if (readOnly)
                edit.Lexer = csParser1;
            else
                edit.Lexer = pythonParser1;

            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
            return edit;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

            scriptRun.RunFunction("Main", new object[] { });
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath)
        {
            sourceFileSubPath = "CustomAssemblyTest.py";
            sourceExternalSubPath = "CustomClass.cs";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.Python\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }
    }
}
