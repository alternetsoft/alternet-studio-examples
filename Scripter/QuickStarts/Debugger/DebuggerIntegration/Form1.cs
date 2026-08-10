using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.Integration;

namespace DebuggerIntegration
{
    public partial class Form1 : Form
    {
        private readonly DebuggerUIController controller;
        private readonly DebugCodeEditContainer codeEditContainer;

        private IScriptRun scriptRun1;
        private IScriptDebuggerBase debugger;
        private bool useNewDebugger = false;

        public Form1()
        {
            InitializeComponent();
            ActivateDarkTheme();

            var asm = typeof(Form1).Assembly;
            var prefix = "DebuggerIntegration.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;
            controller = new DebuggerUIController(this, codeEditContainer);
            controller.DebuggerPanels = debuggerPanelsTabControl;

            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;
            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;

            if (LoadDefaultProject)
                OpenProject(FindProjectFile());
        }

        public static string[] ProjectSearchDirectories { get; set; } = new[] { ".", @"..\..\..\..\..\..\..\" };

        public static string StartupProjectFileSubPath { get; set; } = @"Resources\Debugger\CS\DebuggerTest\DebuggerTest.csproj";

        public static bool LoadDefaultProject { get; set; } = true;

        public static bool IsDark
        {
            get
            {
                return Alternet.Editor.SyntaxEdit.IsDarkModeEnabled();
            }
        }

        public DebuggerControlToolbar DebuggerControlToolbar => debuggerControlToolbar1;

        public DebugMenu DebugMenu => debugMenu1;

        public DotNetProject Project { get; private set; } = new DotNetProject();

        public DebugCodeEditContainer CodeEditContainer => codeEditContainer;

        public Alternet.Scripter.Integration.DebuggerPanelsTabControl DebuggerPanels => debuggerPanelsTabControl;

        public IScriptDebuggerBase Debugger
        {
            get
            {
                if (debugger == null)
                {
                    InitializeDebugger();
                }

                return debugger;
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Browsable(false)]
        public bool UseNewDebugger
        {
            get
            {
                return useNewDebugger;
            }

            set
            {
                if (useNewDebugger != value)
                {
                    FinalizeDebugger();
                    useNewDebugger = value;
                    InitializeDebugger();
                    UpdateDebugControls();
                }
            }
        }

        public static string FindProjectFile()
        {
            return ProjectSearchDirectories.Select(
                x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);
        }

        public void OpenProject(string projectFilePath)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

            if (Project != null && Project.HasProject)
                CloseProject(Project);

            if (!File.Exists(projectFilePath))
                return;

            Project.Load(projectFilePath);
            scriptRun1.ScriptSource.FromScriptProject(Project.ProjectFileName);
            var extension = Project.ProjectExtension;
            CodeEditExtensions.OpenProject(Project);

            if (Project.Files.Count > 0)
            {
                codeEditContainer.TryActivateEditor(Project.Files[0]);
            }

            debuggerPanelsTabControl.Errors.Clear();
            UpdateDebugControls();
        }

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private void Debugger_DebuggerErrorOccurred(object sender, DebuggerErrorOccurredEventArgs e)
        {
            BeginInvoke(new Action(() => MessageBox.Show(this, e.Exception.ToString(), "Debugger Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }

        private void Debugger_ExecutionStopped(object sender, ExecutionStoppedEventArgs e)
        {
            if (e.StopReason == ExecutionStopReason.Exception || e.StopReason == ExecutionStopReason.UnhandledException)
                DisplayDebuggerException(e);
        }

        private void DisplayDebuggerException(ExecutionStoppedEventArgs e)
        {
            var str = $"{e.Exception.ExceptionType}\n{e.Exception.Message}";
            using (var dlg = new DebuggerException(Debugger, str))
            {
                dlg.Text = e.StopReason == ExecutionStopReason.UnhandledException ? StringConsts.UnhandledException : StringConsts.DebuggerException;
                dlg.ExceptionEvaluationExpression = e.Exception.ExceptionEvaluationExpression;
                dlg.OnEvaluate += Dlg_OnEvaluate;
                DialogResult result = dlg.ShowDialog();
            }
        }

        private void Dlg_OnEvaluate(object sender, EventArgs e)
        {
            EvaluateExpression(true);
        }

        private void EvaluateExpression(bool evaluateCurrentException = false)
        {
            var edit = codeEditContainer.ActiveEditor;
            var symbol = edit != null ? edit.GetSymbolAtCursor() : string.Empty;
            using (var dialog = new EvaluateDialog(
                EvaluateDialog.CodeCompletionOptions.Custom((s, d, tb) => new EditorCodeCompletionController(s, d, tb)))
            {
                Debugger = this.Debugger,
                EvaluateCurrentException = evaluateCurrentException,
                Expression = symbol,
            })
            {
                var watchesControl = debuggerPanelsTabControl.Watches;

                dialog.WatchAdded += (o, e) => watchesControl.AddWatch(e.Expression);
                dialog.WatchAdded += (o, e) => ActivateWatchesTab();
                dialog.ShowDialog();
            }
        }

        private void ActivateWatchesTab()
        {
            debuggerPanelsTabControl.FocusPanel(DebuggerPanelKinds.Watches);
        }

        private void ActivateDarkTheme()
        {
            if (IsDark)
            {
            }
            else
            {
            }
        }

        private void OnDebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
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

        private bool TryResetDebuggerOnProjectChange()
        {
            if (Debugger != null && Debugger.IsStarted)
            {
                MessageBox.Show("Please stop debugging session first");
                return false;
            }

            if (Debugger != null)
                Debugger.Breakpoints.Clear();

            return true;
        }

        private void UpdateDebugControls()
        {
            bool enabled = (Project != null && Project.HasProject) || codeEditContainer.ActiveEditor != null;

            debuggerControlToolbar1.Debugger = enabled ? Debugger : null;
            debugMenu1.Debugger = enabled ? Debugger : null;
        }

        private void CloseProject(DotNetProject project)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

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
            UpdateDebugControls();
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

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.BorderStyle = EditBorderStyle.None;
            var projectName = GetProjectName(e.FileName);
            edit.SetFileNameAndProject(e.FileName, projectName);
            edit.LoadFile(e.FileName);
            e.DebugEdit = edit;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UseUniversalDebuggerMenuItem_Click(object sender, System.EventArgs e)
        {
            if (ChangeDebuggerMode(!useUniversalDebuggerMenuItem.Checked))
                useUniversalDebuggerMenuItem.Checked = UseNewDebugger;
        }

        private bool ChangeDebuggerMode(bool useUnivesal)
        {
            if (!TryResetDebuggerOnProjectChange())
                return false;
            UseNewDebugger = useUnivesal;
            return true;
        }

        private void InitializeDebugger()
        {
            if (UseNewDebugger)
                debugger = new Alternet.Scripter.Debugger.Universal.ScriptDebugger { ScriptRun = scriptRun1 };
            else
                debugger = new Alternet.Scripter.Debugger.ScriptDebugger { ScriptRun = scriptRun1 };

            var myPlatform = Consts.IsNetFramework ? ".NET Framework" : ".NET Core";

            if (UseNewDebugger)
            {
                LogToOutput($"Using universal debugger on {myPlatform} platform");
            }
            else
            {
                LogToOutput($"Using legacy debugger on {myPlatform} platform");
            }

            debugger.DebuggerErrorOccurred += Debugger_DebuggerErrorOccurred;
            debugger.ExecutionStopped += Debugger_ExecutionStopped;

            controller.Debugger = debugger;
            codeEditContainer.Debugger = debugger;

            debuggerPanelsTabControl.Breakpoints.Debugger = debugger;
            debuggerPanelsTabControl.CallStack.Debugger = debugger;
            debuggerPanelsTabControl.Output.Debugger = debugger;
            debuggerPanelsTabControl.Locals.Debugger = debugger;
            debuggerPanelsTabControl.Watches.Debugger = debugger;
            debuggerPanelsTabControl.Errors.Debugger = debugger;
            debuggerPanelsTabControl.Threads.Debugger = debugger;
        }

        private void LogToOutput(string s)
        {
            debuggerPanelsTabControl.Output?.CustomLog(s + Environment.NewLine);
        }

        private void FinalizeDebugger()
        {
            if (debugger == null)
                return;
            debugger.DebuggerErrorOccurred -= Debugger_DebuggerErrorOccurred;
            debugger.ExecutionStopped -= Debugger_ExecutionStopped;
            controller.Debugger = null;
            codeEditContainer.Debugger = null;
            debuggerPanelsTabControl.Breakpoints.Debugger = null;
            debuggerPanelsTabControl.CallStack.Debugger = null;
            debuggerPanelsTabControl.Output.Debugger = null;
            debuggerPanelsTabControl.Locals.Debugger = null;
            debuggerPanelsTabControl.Watches.Debugger = null;
            debuggerPanelsTabControl.Errors.Debugger = null;
            debuggerPanelsTabControl.Threads.Debugger = null;
            debuggerControlToolbar1.Debugger = null;
            debugMenu1.Debugger = null;

            (debugger as IDisposable)?.Dispose();
            debugger = null;
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

                UpdateDebugControls();
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

            UpdateDebugControls();
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, System.EventArgs e)
        {
            closeProjectToolStripMenuItem.Enabled = Project != null && Project.HasProject;
            closeToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
            saveToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
        }
    }
}