using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.AlternetUI;
using Alternet.Scripter.Roslyn;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.UI;

namespace Alternet.Scripter.Integration.AlternetUI
{
    public class DebuggerIntegrationPanelCSharp : BaseDebuggerIntegrationPanel
    {
        private MenuItem? fileMenu;
        private MenuItem? openProjectMenuItem;
        private MenuItem? closeProjectMenuItem;
        private MenuItem? openMenuItem;
        private MenuItem? saveMenuItem;
        private MenuItem? closeMenuItem;
        private MenuItem? exitMenuItem;

        static DebuggerIntegrationPanelCSharp()
        {
            DebugCodeEdit.Parsers[".cs"] = typeof(CsParser);
            DebugCodeEdit.Parsers[".vb"] = typeof(VbParser);
        }

        public DebuggerIntegrationPanelCSharp()
        {
            Controller!.DebuggerPreStartup += OnDebuggerPreStartup;
            CodeEditContainer.EditorRequested += OnEditorEditorRequested;

            OpenDialog.Filter
                 = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|Any files (*.*)|*.*";
            OpenDialog.FilterIndex = 0;

            OpenProjectDialog.Filter
                = "Project files (*.csproj; *.vbproj)|*.csproj;*.vbproj|All files (*.*)|*.*";
        }

        public static bool UseOldDebugger { get; set; }

        public OpenFileDialog OpenDialog { get; } = new OpenFileDialog();

        public OpenFileDialog OpenProjectDialog { get; } = new OpenFileDialog();

        public virtual MenuItem FileMenu
        {
            get
            {
                if (fileMenu is null)
                {
                    fileMenu = new("_File");

                    fileMenu.Add(OpenProjectMenuItem);
                    fileMenu.Add(CloseProjectMenuItem);
                    fileMenu.Add("-");
                    fileMenu.Add(OpenMenuItem);
                    fileMenu.Add(SaveMenuItem);
                    fileMenu.Add(CloseMenuItem);
                    fileMenu.Add("-");
                    fileMenu.Add(ExitMenuItem);

                    fileMenu.Opened += OnFileMenuOpened;
                }

                return fileMenu;
            }
        }

        public virtual MenuItem OpenProjectMenuItem
        {
            get
            {
                if (openProjectMenuItem is null)
                {
                    openProjectMenuItem = new("_Open Project...");
                    openProjectMenuItem.Click += OnOpenProjectMenuItemClick;
                }

                return openProjectMenuItem;
            }
        }

        public virtual MenuItem CloseProjectMenuItem
        {
            get
            {
                if (closeProjectMenuItem is null)
                {
                    closeProjectMenuItem = new("_Close Project");
                    closeProjectMenuItem.Click += OnCloseProjectMenuItemClick;
                }

                return closeProjectMenuItem;
            }
        }

        public virtual MenuItem OpenMenuItem
        {
            get
            {
                if (openMenuItem is null)
                {
                    openMenuItem = new("_Open...");
                    openMenuItem.Click += OnOpenMenuItemClick;
                }

                return openMenuItem;
            }
        }

        public virtual MenuItem SaveMenuItem
        {
            get
            {
                if (saveMenuItem is null)
                {
                    saveMenuItem = new("_Save");
                    saveMenuItem.Click += OnSaveMenuItemClick;
                }

                return saveMenuItem;
            }
        }

        public virtual MenuItem CloseMenuItem
        {
            get
            {
                if (closeMenuItem is null)
                {
                    closeMenuItem = new("_Close");
                    closeMenuItem.Click += OnCloseMenuItemClick;
                }

                return closeMenuItem;
            }
        }

        public virtual MenuItem ExitMenuItem
        {
            get
            {
                if (exitMenuItem is null)
                {
                    exitMenuItem = new("E_xit");
                    exitMenuItem.Click += OnExitMenuItemClick;
                }

                return exitMenuItem;
            }
        }

        public override IScriptDebuggerBase CreateDebugger()
        {
            RoslynScriptProvider.PortablePdb = !UseOldDebugger || !App.IsWindowsOS;

            var result = DebuggerUtils.CreateDebugger(UseOldDebugger)!;
            result.EventsSyncAction = (action) => Alternet.UI.App.Invoke(action);

            /*
            var success = AssemblyUtils.TrySetMemberValue(
                result,
                "GeneratedModulesPath",
                DebuggerUtils.GenModulesDirectoryPath());
            */
            return result;
        }

        public override IScriptRunBase CreateScriptRun()
        {
            var result = new ScriptRun();
            result.ScriptHost.GenerateModulesOnDisk = true;
            /*
            result.ScriptHost.ModulesDirectoryPath = DebuggerUtils.GenModulesDirectoryPath();
            */
            return result;            
        }

        public virtual bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            if (CodeEditContainer.ActiveEditor != null)
            {
                string fileName = CodeEditContainer.ActiveEditor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    (ScriptRun?.ScriptSource as IScriptSource)?.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        public virtual void RegisterAssemblies(
            string extension,
            string[] references,
            TechnologyEnvironment? technology = null,
            bool keepExisting = false,
            string? projectName = null,
            TargetFramework? targetFramework = null)
        {
            if (references == null)
                return;

            var solution = GetSolution(extension);
            if (solution == null)
                return;
            var project = !string.IsNullOrEmpty(projectName) ? solution.GetProject(projectName) : null;
            var projectId = project != null ? project.Id : null;

            technology ??= DetectTechnologyEnvironmentFromReferences(references);
            targetFramework ??= DetectTargetFrameworkFromReferences(references);

            solution.WithDefaultAssemblies(technology.Value, keepExisting, projectId, targetFramework)
                .RegisterAssemblies(references, projectId, targetFramework);
        }

        public virtual void RegisterCode(string extension, string[] files, string? projectName = null)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            var project = !string.IsNullOrEmpty(projectName) ? solution.GetProject(projectName) : null;
            solution.RegisterCodeFiles(files, project?.Id);
        }

        public virtual string GetFirstFile(IList<string> files, string langExt)
        {
            string result = files.Count > 0 ? files[0] : string.Empty;

            foreach (string file in files)
            {
                if (file.ToLower().Contains("program.cs"))
                    return file;
                if (file.ToLower().Contains("main") && file.EndsWith(langExt))
                    return file;
            }

            return result;
        }

        public virtual void OpenProject(string? projectFilePath)
        {
            var safeProject = Project;

            if (projectFilePath is null)
                return;

            if (safeProject != null && safeProject.HasProject)
                CloseProject(safeProject);

            if (safeProject is null)
                return;

            if (!File.Exists(projectFilePath))
            {
                return;
            }

            var projectFolder = Path.GetDirectoryName(projectFilePath);
            OpenDialog.InitialDirectory = projectFolder;
            OpenProjectDialog.InitialDirectory = projectFolder;

            safeProject.Load(projectFilePath);
            (ScriptRun?.ScriptSource as IScriptSource)?.FromScriptProject(safeProject.ProjectFileName);
            var extension = safeProject.ProjectExtension;
            OpenProject(extension, safeProject);

            var codeFiles = safeProject.Files.Where(
                x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".vb").ToList();

            if (codeFiles.Count > 0)
            {
                CodeEditContainer.TryActivateEditor(
                    GetFirstFile(codeFiles, safeProject.DefaultExtension));
            }

            var references = safeProject.References.Concat(safeProject.AutoReferences).Select(
                x => x.FullName).Concat(
                safeProject.FrameworkReferences.SelectMany(x => x.Assemblies).Select(
                    x => x.HintPath)).ToArray();

            DebuggerPanelsTabControl.Errors?.Clear();
            UpdateToolbar();
        }

        public virtual void OpenProject(string extension, DotNetProject project)
        {
            var projectName = project.ProjectName;
            var projectPath = project.ProjectFileName;
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            if (solution.GetProject(projectName) == null)
                solution.AddProject(projectName, projectPath);

            if (project.Files.Count > 0)
            {
                RegisterCode(extension, project.Files.Where(x =>
                x.EndsWith(project.ProjectExtension)).ToArray(), project.ProjectName);
            }

            var references = project.References
                .Concat(project.AutoReferences)
                .Select(x => x.FullName ?? string.Empty)
                .Concat(project.FrameworkReferences
                    .SelectMany(x => x.Assemblies).Select(x => x.HintPath ?? string.Empty))
                .Distinct().ToArray();

            RegisterAssemblies(
                extension,
                project.TryResolveAbsolutePaths(references).ToArray(),
                projectName: project.ProjectName,
                targetFramework: project.TargetFramework);
        }

        public virtual IRoslynSolution? GetSolution(string extension)
        {
            switch (extension.ToLower())
            {
                case ".cs":
                    return CsSolution.DefaultSolution;
                case ".vb":
                    return VbSolution.DefaultSolution;
                case ".csx":
                    return CsSolution.DefaultScriptSolution;
                case ".vbx":
                    return VbSolution.DefaultScriptSolution;
                default:
                    return null;
            }
        }

        public virtual void OnOpenProjectMenuItemClick(object? sender, EventArgs e)
        {
            OpenProjectDialog.ShowAsync(() =>
            {
                if (OpenProjectDialog.FileName == null)
                    return;
                OpenProject(OpenProjectDialog.FileName);
            });
        }

        public virtual void OnCloseProjectMenuItemClick(object? sender, EventArgs e)
        {
            CloseProject(Project);
        }

        public virtual void OnOpenMenuItemClick(object? sender, EventArgs e)
        {
            OpenDialog.ShowAsync(() =>
            {
                if (OpenDialog.FileName == null)
                    return;

                CodeEditContainer.TryActivateEditor(OpenDialog.FileName);

                UpdateToolbar();
            });
        }

        public void OnCloseMenuItemClick(object? sender, EventArgs e)
        {
            StopDebugger();
            var edit = CodeEditContainer.ActiveEditor;
            if (edit != null)
            {
                CodeEditContainer.CloseFile(edit.FileName);
                edit.FileName = string.Empty;
            }

            if (!Project.HasProject && CodeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                (ScriptRun?.ScriptSource as IScriptSource)?.Reset();
            }

            UpdateToolbar();
        }

        protected virtual void OnFileMenuOpened(object? sender, EventArgs e)
        {
            CloseProjectMenuItem.Enabled = CanCloseProject();
            CloseMenuItem.Enabled = CanCloseFile();
            SaveMenuItem.Enabled = CanSaveFile();
        }

        protected virtual void OnEditorEditorRequested(object? sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            /*var projectName = GetProjectName(e.FileName);*/
            edit.LoadFile(e.FileName);
            e.DebugEdit = edit;
        }

        protected virtual void OnDebuggerPreStartup(object? sender, System.EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();

            /*var s = (Debugger?.ScriptRun as IScriptRun)?.ScriptHost.ExecutableModulePath;*/
        }
    }
}