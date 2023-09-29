using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Editor.Python;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.Integration;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;

namespace DebuggerIntegration.Python
{
    public partial class Form1 : Form
    {
        protected PythonProject Project { get; private set; } = new PythonProject();

        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.Python\DebuggerUIThread\Project.pyproj";
        private static readonly string StartupFileSubPath = @"Resources\Debugger.Python\DebuggerUIThread\CallMethod.py";
        private DebugCodeEditContainer editorTabContainer;
        private ScriptDebugger debugger;
        private DisplayForm displayForm;

        public Form1()
        {
            SetupPython();

            InitializeComponent();

            editorTabContainer = new DebugCodeEditContainer(editorsTabControl);

            editorTabContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenFile(FindPythonFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun1,
            };

            debuggerControlToolbar1.Debugger = debugger;
            debugMenu1.Debugger = debugger;

            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;
            debuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, editorTabContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;

            debugMenu1.CommandsListener = new DebuggerUICommands(this, debugger);
            debuggerControlToolbar1.CommandsListener = new DebuggerUICommands(this, debugger);

            editorTabContainer.Debugger = debugger;

            StartDisplayFormThread(DisplayForm.Command.None);
            FormClosing += Form1_FormClosing;
        }

        protected void StartDisplayFormThread(DisplayForm.Command startCommand)
        {
            var thread = new Thread(() =>
            {
                displayForm = new DisplayForm(debuggerControlToolbar1.Debugger, startCommand);
                displayForm.Location = Bounds.RightTop();
                displayForm.StartPosition = FormStartPosition.Manual;
                displayForm.FormClosing += DisplayForm_FormClosing;
                Application.Run(displayForm);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        protected async override void OnClosing(CancelEventArgs e)
        {
            if (debugger.IsStarted)
                await debugger.StopDebuggingAsync();

            base.OnClosing(e);
        }

        private static string FindPythonFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
                Text = "Debugger Integration Python Demo",
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

        private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            displayForm = null;
        }

        private void OnDebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
            if (displayForm == null)
                StartDisplayFormThread(DisplayForm.Command.Debug);
        }

        private void OpenFile(string fileName)
        {
            if (Project != null && Project.HasProject)
                CloseProject(Project);
            scriptRun1.ScriptSource.FromScriptFile(fileName);

            editorTabContainer.TryActivateEditor(fileName);
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
            edit.AllowedActions &= ~AllowedActions.SetNextStatement;

            e.DebugEdit = edit;
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
            if (displayForm == null)
                StartDisplayFormThread(DisplayForm.Command.Run);
            else
                displayForm.InvokeRunScript();
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

        public class DebuggerUICommands : DefaultDebuggerUICommands
        {
            private Form1 owner;

            public DebuggerUICommands(Form1 owner, IScriptDebuggerBase debugger)
                : base(debugger)
            {
                this.owner = owner;
            }

            public override bool Start()
            {
                if (Debugger.IsStarted)
                    return base.Start();

                return true;
            }

            public override bool StepInto()
            {
                if (Debugger.IsStarted)
                    return base.StepInto();
                return true;
            }

            public override bool StepOver()
            {
                if (Debugger.IsStarted)
                    return base.StepOver();

                return true;
            }

            public override bool PreStartup()
            {
                if (owner.displayForm != null)
                    owner.displayForm.InvokeStartDebug();

                return true;
            }
        }
    }
}