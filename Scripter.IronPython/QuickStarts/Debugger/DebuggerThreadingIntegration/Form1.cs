using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Editor.IronPython;
using Alternet.Scripter.Debugger.IronPython;
using Alternet.Scripter.Integration;
using Alternet.Scripter.IronPython;
using Alternet.Syntax.Parsers.Python;

namespace DebuggerThreadingIntegration.IronPython
{
    public partial class Form1 : Form
    {
        protected IronPythonProject Project { get; private set; } = new IronPythonProject();

        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.IronPython\DebuggerThreadingIntegration\Project.pyproj";
        private static readonly string[] SourceSearchDirectories = new[] { "..", @"..\..\..\..\..\..\..\" };
        private static readonly string SourceFileSubPath = @"Resources\Debugger.Dap\Python\MultiThreadedTest.py";

        private DebugCodeEditContainer editorTabContainer;

        public Form1()
        {
            InitializeComponent();

            editorTabContainer = new DebugCodeEditContainer(editorsTabControl);

            editorTabContainer.EditorRequested += EditorContainer_EditorRequested;

            scriptRun1.ScriptSource.FromScriptFile(Path.GetFullPath(FindSourceFile()));
            editorTabContainer.TryActivateEditor(scriptRun1.ScriptSource.ScriptFile);

            //OpenProject(FindProjectFile());
            //scriptRun1.GlobalItems.Add(new ScriptGlobalItem("TestMenuItem", testMenuItemToolStripMenuItem));

            var debugger = new ScriptDebugger();
            debugger.ScriptRun = scriptRun1;
            debuggerControlToolbar1.Debugger = debugger;
            debugMenu1.Debugger = debugger;

            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;

            debuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, editorTabContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;

            editorTabContainer.Debugger = debugger;
        }

        private static string FindSourceFile()
        {
            return SourceSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, SourceFileSubPath))).
                FirstOrDefault(File.Exists);
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
          ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

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

        private void CloseProject(IronPythonProject project)
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
            if (!File.Exists(e.FileName))
                return;

            var parser = new IronPythonParser();
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

        private void ScriptRun1_FileLoad(object sender, Alternet.Scripter.FileLoadEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                if (editorTabContainer.ActiveEditor != null && string.Compare(editorTabContainer.ActiveEditor.FileName, e.FileName, true) == 0)
                {
                    e.Text = editorTabContainer.ActiveEditor.Text;
                    e.Handled = true;
                }
            }));
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