using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Roslyn;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Integration;

namespace DebuggerIntegration
{
    public partial class Form1 : Form
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };

        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger\CS\DebuggerTest\DebuggerTest.csproj";

        private ScriptDebugger debugger;

        private DebugCodeEditContainer codeEditContainer;

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

            ScaleControls();
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

        private static string FindProjectFile() =>
           ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private void Debugger_DebuggerErrorOccured(object sender, DebuggerErrorOccuredEventArgs e)
        {
            BeginInvoke(new Action(() => MessageBox.Show(this, e.Exception.ToString(), "Debugger Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
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
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}