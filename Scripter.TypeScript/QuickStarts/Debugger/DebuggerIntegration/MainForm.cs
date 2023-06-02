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
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;
using Alternet.Scripter.Debugger.TypeScript;
using Alternet.Scripter.Integration;

namespace DebuggerIntegration.TypeScript
{
    public partial class MainForm : Form
    {
        protected TSProject Project { get; private set; } = new TSProject();

        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.TypeScript\TS\MultipleFiles\project.json";
        private static readonly string StartupFileSubPath = @"Resources\Debugger.TypeScript\TS\DebugMyScript\DebugMyScript.ts";

        private IDebugEdit edit;
        private ScriptDebugger debugger;
        private DebugCodeEditContainer codeEditContainer;
        private string initDirectory = string.Empty;

        public MainForm(string[] args)
        {
            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenFile(FindFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            debuggerControlToolbar.Debugger = debugger;
            debuggerControlToolbar.DebuggerPreStartup += DebuggerPreStartup;

            debugMenu1.Debugger = debugger;
            debugMenu1.DebuggerPreStartup += DebuggerPreStartup;

            debuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
            debuggerPanelsTabControl.Debugger = debugger;
            var controller = new DebuggerUIController(this, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            ScaleControls();
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, StartupFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

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
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            CodeEditExtensions.OpenProject(Project.ProjectFileName, Project);

            if (Project.Files.Count > 0)
            {
                codeEditContainer.TryActivateEditor(Project.Files[Project.Files.Count - 1]);
            }

            debuggerPanelsTabControl.Errors.Clear();
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void CloseProject(TSProject project)
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
            CodeEditExtensions.CloseProject(extension);

            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.InitSyntax();
            LoadFile(edit, e.FileName);
            e.DebugEdit = edit;
            this.edit = edit;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitDefaultHostAssemblies();
        }

        private void DebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
        }

        private bool CompileScript()
        {
            if (edit.Modified)
                edit.SaveFile(edit.FileName);

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return false;
                }
            }

            return true;
        }

        private void StartScript()
        {
            if (CompileScript())
                CatchAndDisplayExceptions(() => scriptRun.Run());
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            StartScript();
        }

        private void CatchAndDisplayExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
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
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void OpenProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Project files *.json|*.json|All files (*.*)|*.*",
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

        private void OpenFile(string fileName)
        {
            codeEditContainer.TryActivateEditor(fileName);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Typescript files (*.ts)|*.ts|Javascript files (*.js)|*.js|Any files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = initDirectory,
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null && !string.IsNullOrEmpty(codeEditContainer.ActiveEditor.FileName))
            {
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.CloseFile(codeEditContainer.ActiveEditor.FileName);

            if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun.ScriptSource?.Reset();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, System.EventArgs e)
        {
            closeProjectToolStripMenuItem.Enabled = Project != null && Project.HasProject;
            closeToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
            saveToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
        }
    }
}
