using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Editor.Python;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Integration;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;

namespace DebuggerIntegration.Python
{
    public partial class Form1 : Form
    {
        protected PythonProject Project { get; private set; } = new PythonProject();

        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.Python\DebuggerIntegration\Project.pyproj";
        private DebugCodeEditContainer editorTabContainer;
        private ScriptDebugger debugger;

        public Form1()
        {
            SetupPython();

            InitializeComponent();

            editorTabContainer = new DebugCodeEditContainer(editorsTabControl);

            editorTabContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenProject(FindProjectFile());

            scriptRun1.GlobalItems.Add(new ScriptGlobalItem("TestMenuItem", testMenuItemToolStripMenuItem));

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun1,
            };
            debuggerControlToolbar1.Debugger = debugger;
            debugMenu1.Debugger = debugger;

            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;

            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, editorTabContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;

            editorTabContainer.Debugger = debugger;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (debugMenu1.Debugger.IsStarted)
                debugMenu1.Debugger.StopDebuggingAsync();

            base.OnClosing(e);
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private void SetupPython()
        {
            var requiredModules = new[] { "numpy" };

            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"AlterNETStudio\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled() &&
                requiredModules.All(embeddedPythonInstaller.IsWheelInstalled))
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "Debugger Integration Python Demo",
                Message = "Deploying Python and packages...",
                ProgressBarStyle = ProgressBarStyle.Marquee,
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython();

                    var numpyFileName = Environment.Is64BitProcess ? "numpy-1.16.3-cp37-cp37m-win_amd64.whl" : "numpy-1.16.3-cp37-cp37m-win32.whl";

                    await embeddedPythonInstaller.InstallWheel(
                        GetType().Assembly.GetManifestResourceStream("DebuggerIntegration.Python.Resources.numpy." + numpyFileName),
                        numpyFileName);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void OnDebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
        }

        private void OpenProject(string projectFilePath)
        {
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            Project.Load(projectFilePath);

            scriptRun1.ScriptSource.FromScriptProject(Project.ProjectFileName);

            if (Project.Files.Count > 0)
            {
                editorTabContainer.TryActivateEditor(Project.Files[0]);
            }

            debuggerPanelsTabControl.Errors.Clear();
        }

        private void CloseProject(PythonProject project)
        {
            foreach (string fileName in project.Files)
            {
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

            Project?.Reset();
            scriptRun1.ScriptSource?.Reset();
        }

        private void CloseFile(string fileName)
        {
            editorTabContainer.CloseFile(fileName);
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            if (!PathUtilities.IsPathValid(e.FileName) || !File.Exists(e.FileName))
                return;

            var parser = new PythonNETParser();
            parser.CodeEnvironment = scriptRun1.CodeEnvironment;

            var edit = new DebugCodeEdit();
            edit.LoadFile(e.FileName);
            edit.Lexer = parser;

            edit.SetNextStatement += DebugEdit_SetNextStatement;

            e.DebugEdit = edit;
        }

        private async void DebugEdit_SetNextStatement(object sender, EventArgs e)
        {
            if (editorTabContainer.ActiveEditor == null)
            {
                return;
            }

            var line = editorTabContainer.ActiveEditor.CurrentLine + 1;
            var result = await editorTabContainer.Debugger.TrySetNextStatementAsync(line);
            if (result.IsFailure)
                MessageBox.Show("Cannot set next statement to line: " + line + ". " + result.ErrorMessage);
        }

        private void SaveAllModifiedFiles()
        {
            foreach (var edit in editorTabContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            if (editorTabContainer.ActiveEditor != null)
            {
                string fileName = editorTabContainer.ActiveEditor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun1.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void RunMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveAllModifiedFiles();
            scriptRun1.Run();
        }

        private void OpenProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Project files (*.pyproj|*.pyproj|All files (*.*)|*.*",
                InitialDirectory = FindDefaultProjectDirectory(),
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                OpenProject(dialog.FileName);
            }
        }

        private void CloseProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject(Project);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Python files (*.py)|*.py|Any files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Path.GetDirectoryName(FindProjectFile()),
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                editorTabContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (editorTabContainer.ActiveEditor != null)
                editorTabContainer.ActiveEditor.SaveFile(editorTabContainer.ActiveEditor.FileName);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editorTabContainer.ActiveEditor != null)
                editorTabContainer.CloseFile(editorTabContainer.ActiveEditor.FileName);
            if (!Project.HasProject && editorTabContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun1.ScriptSource?.Reset();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, System.EventArgs e)
        {
            closeProjectToolStripMenuItem.Enabled = Project != null && Project.HasProject;
            closeToolStripMenuItem.Enabled = editorTabContainer.ActiveEditor != null;
            saveToolStripMenuItem.Enabled = editorTabContainer.ActiveEditor != null;
        }
    }
}