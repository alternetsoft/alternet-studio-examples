using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.Integration;
using Microsoft.CodeAnalysis.Differencing;

namespace DebuggerUIThread
{
    public partial class Form1 : Form
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };

        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger\CS\DebuggerUIThread\DebuggerTest.csproj";

        private ScriptDebugger debugger;

        private DebugCodeEditContainer codeEditContainer;
        private DisplayForm displayForm;

        public Form1()
        {
            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenProject(FindProjectFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun1,
            };

            debugger.DebuggerErrorOccured += Debugger_DebuggerErrorOccured;

            debuggerControlToolbar1.Debugger = debugger;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;

            debugMenu1.Debugger = debugger;
            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;

            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;
            debugMenu1.CommandsListener = new DebuggerUICommands(this, debugger);
            debuggerControlToolbar1.CommandsListener = new DebuggerUICommands(this, debugger);

            ScaleControls();

            StartDisplayFormThread(DisplayForm.Command.None);
            FormClosing += Form1_FormClosing;
        }

        protected DotNetProject Project { get; private set; } = new DotNetProject();

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
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

        private static string FindProjectFile() =>
           ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (displayForm != null)
                displayForm.InvokeCloseDisplayForm();
        }

        private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            displayForm = null;
        }

        private void Debugger_DebuggerErrorOccured(object sender, DebuggerErrorOccuredEventArgs e)
        {
            BeginInvoke(new Action(() => MessageBox.Show(this, e.Exception.ToString(), "Debugger Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }

        private void OnDebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
            if (displayForm == null)
                StartDisplayFormThread(DisplayForm.Command.Debug);
        }

        private bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            if (codeEditContainer.ActiveEditor != null)
            {
                string fileName = codeEditContainer.ActiveEditor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun1.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            splitContainer.SplitterDistance = DisplayScaling.AutoScale(splitContainer.SplitterDistance);
        }

        private void OpenProject(string projectFilePath)
        {
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            Project.Load(projectFilePath);
            scriptRun1.ScriptSource.FromScriptProject(Project.ProjectFileName);
            var extension = Project.ProjectExtension;
            CodeEditExtensions.OpenProject(extension, Project);

            if (Project.Files.Count > 0)
            {
                codeEditContainer.TryActivateEditor(Project.Files[0]);
            }

            debuggerPanelsTabControl.Errors.Clear();
        }

        private void CloseProject(DotNetProject project)
        {
            foreach (string fileName in project.Files)
            {
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

            var extension = string.Format(".{0}", project.DefaultExtension);

            CodeEditExtensions.CloseProject(extension, project.ProjectName);
            Project?.Reset();
            scriptRun1.ScriptSource?.Reset();
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private string GetProjectName(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return Project.ProjectName;
            }

            return null;
        }

        private ISyntaxEdit TryActivateEditor(string fileName)
        {
            return codeEditContainer.TryActivateEditor(fileName) as ISyntaxEdit;
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            var projectName = GetProjectName(e.FileName);
            edit.SetFileNameAndProject(e.FileName, projectName);
            edit.LoadFile(e.FileName);
            e.DebugEdit = edit;
            scriptRun1.ScriptSource.WithDefaultReferences();
            var globalItems = scriptRun1.GlobalItems;
            globalItems.Clear();
            globalItems.Add(new ScriptGlobalItem("resultLabel", typeof(System.Windows.Forms.Label), null));
            globalItems.Add(new ScriptGlobalItem("progressBar", typeof(System.Windows.Forms.ProgressBar), null));
            RegisterScriptCodeForEditor(edit);
        }

        private void AddScriptItems()
        {
        }

        private void RegisterScriptCodeForEditor(IScriptEdit edit)
        {
            edit.RegisterCode("global.cs", scriptRun1.ScriptHost.GlobalCode);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
                Filter = "Project files (*.csproj; *.vbproj)|*.csproj;*.vbproj|All files (*.*)|*.*",
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
                Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|Any files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Path.GetDirectoryName(FindProjectFile()),
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
            {
                codeEditContainer.CloseFile(edit.FileName);
                edit.FileName = string.Empty;
            }

            if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun1.ScriptSource?.Reset();
            }
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, System.EventArgs e)
        {
            closeProjectToolStripMenuItem.Enabled = Project != null && Project.HasProject;
            closeToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
            saveToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
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