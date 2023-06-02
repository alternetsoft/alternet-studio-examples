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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Editor.Common;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace ObjectReference
{
    public partial class MainForm : Form
    {
        private bool scriptRunning;
        private IDisposable scriptObject;
        private IScriptEdit edit;
        private Timer timer = new Timer();

        public MainForm()
        {
            SetupPython();

            InitializeComponent();

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.WindowsForms;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Forms");
            AddScriptItem();
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled())
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
                    await embeddedPythonInstaller.SetupPython();
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private IScriptEdit CreateEditor(string fileName, Control parent)
        {
            var edit = new ScriptCodeEdit();
            edit.FileName = fileName;
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;

            LoadFile(edit, fileName);
            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
            edit.Lexer = pythonParser1;
            return edit;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Control parent)
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, parent);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateEditor(pnEdit);
        }

        private void AddScriptItem()
        {
            ScriptGlobalItem item = new ScriptGlobalItem("RunButton", btNETFromScript);
            ScriptGlobalItem item1 = new ScriptGlobalItem("timer", timer);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            scriptRun.GlobalItems.Add(item1);
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

            scriptObject = scriptRun.Run() as IDisposable;

            try
            {
                scriptRun.RunFunction("Main", new object[] { "Catch me if you can" });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Script Execution Error");
                StopScript();
            }

            scriptRunning = true;
            btRun.Text = "Stop Script";
        }

        private void StopScript()
        {
            if (scriptObject != null)
                scriptObject.Dispose();
            timer.Stop();
            scriptObject = null;
            scriptRunning = false;
            btRun.Text = "Run Script";
            btNETFromScript.Text = "Test Button";
        }

        private void NETFromScriptButton_Click(object sender, EventArgs e)
        {
            StopScript();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (!scriptRunning)
                StartScript();
            else
                StopScript();
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "ObjectReference.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.Python";
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