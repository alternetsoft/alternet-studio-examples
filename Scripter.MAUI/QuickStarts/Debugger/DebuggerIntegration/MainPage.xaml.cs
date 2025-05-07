#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System.Windows.Input;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Maui;

using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.MAUI;
using Alternet.Scripter.Integration.MAUI;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Alternet.Common.DotNet.DefaultAssemblies;
using Microsoft.Maui.Layouts;

namespace DebuggerIntegration
{
    public partial class MainPage : ContentPage
    {
        private static bool ExceptionsLogger = false;

        internal string ProjectFolder = "embres:DebuggerIntegration.Content.DebuggerTest";
        private static readonly string[] vbExtensions = [".vb"];
        private static readonly string[] csExtensions = [".cs"];
        private static readonly string[] vbProjExtensions = [".vbproj"];
        private static readonly string[] csProjExtensions = [".csproj"];

        private DebuggerPanelsTabControlView DebuggerPanelsTabControl = new();
        private SimpleTabControlView EditorsTabControl = new();

        private IScriptDebuggerBase debugger;
        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;

        private DotNetProject project = new();
        private DebugMenu DebugMenu = new();
        private DebuggerControlToolbarView DebuggerControlToolbar = new();

        public ICommand OpenProjectCommand { set; get; }
        public ICommand CloseProjectCommand { set; get; }
        public ICommand OpenCommand { set; get; }
        public ICommand CloseCommand { set; get; }
        public ICommand SaveCommand { set; get; }
        public ICommand ExitCommand { set; get; }

        static MainPage()
        {
            if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                });
                ExceptionsLogger = false;
            }
        }

        public MainPage()
        {
            OpenProjectCommand = new Command(
                execute: () =>
                {
                    OpenProjectDialog();
                },
                canExecute: () =>
                {
                    return Alternet.UI.App.IsWindowsOS;
                });
            OpenCommand = new Command(
                execute: () =>
                {
                    OpenFileDialog();
                },
                canExecute: () =>
                {
                    return Alternet.UI.App.IsWindowsOS;
                });
            CloseProjectCommand = new Command(
                execute: () =>
                {
                    CloseProject(Project);
                },
                canExecute: () =>
                {
                    return Project != null && Project.HasProject;
                });
            CloseCommand = new Command(
                execute: () =>
                {
                    if (codeEditContainer is null || scriptRun is null)
                        return;
                    StopDebugger();
                    var edit = codeEditContainer.ActiveEditor;
                    if (edit != null)
                    {
                        codeEditContainer.CloseFile(edit.Editor.FileName);
                        edit.Editor.FileName = string.Empty;
                    }

                    if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
                    {
                        Project?.Reset();
                        scriptRun.ScriptSource?.Reset();
                    }

                    UpdateToolbar();

                },
                canExecute: () =>
                {
                    return codeEditContainer?.ActiveEditor != null;
                });
            SaveCommand = new Command(
              execute: () =>
              {
                  if (codeEditContainer is null)
                      return;
                  var edit = codeEditContainer.ActiveEditor?.Editor;
                  if (edit != null)
                      edit.SaveFile(edit.FileName);
              },
             canExecute: () =>
             {
                 return codeEditContainer?.ActiveEditor != null;
             });
            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

            if (Consts.IsMacOs)
                FileMenu.Remove(ExitMenuItem);

            AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

            MainGrid.Insert(0, DebuggerControlToolbar);
            panel.Add(EditorsTabControl);
            panel.Add(DebuggerPanelsTabControl, 0, 1);
            DebuggerPanelsTabControl.MinimumHeightRequest = 200;

            DebugCodeEdit.Parsers[".cs"] = typeof(CsParser);
            DebugCodeEdit.Parsers[".vb"] = typeof(VbParser);
            DebugCodeEdit.CreateParserFunc = DoCreateParser;
            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenProject(ProjectFolder, "DebuggerTest.csproj");
            var useOldDebugger = true;

            if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-NewDebugger"))
                useOldDebugger = false;

            debugger = ScripterDemoUtils.CreateDebugger(useOldDebugger)!;
            Alternet.UI.AssemblyUtils.TrySetMemberValue(debugger, "ScriptRun", scriptRun);

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;
            DebuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            MenuBarItems.Add(DebugMenu);

            if (Alternet.UI.App.IsWindowsOS)
            {
                OpenMenuItem.Command = OpenCommand;
                OpenProjectMenuItem.Command = OpenProjectCommand;
            }

            CloseProjectMenuItem.Command = CloseProjectCommand;
            CloseMenuItem.Command = CloseCommand;
            SaveMenuItem.Command = SaveCommand;
            ExitMenuItem.Command = ExitCommand;

            UpdateToolbar();
            UpdateCommands();
        }

        protected DotNetProject Project
        {
            get => project;
            private set => project = value;
        }

        public static void LoadFile(Alternet.Editor.TextSource.ITextSource? source, string url)
        {
            if (source is null)
                return;

            source.Text = string.Empty;
            source.BookMarks.Clear();
            source.LineStyles.Clear();

            var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

            if (stream is null || !source.LoadStream(stream))
            {
                source.Text = $"Error loading text: {url}";
                return;
            }
        }

        private static string GetFirstFile(IList<string> files, string langExt)
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

        private void OnDebuggerPreStartup(object? sender, System.EventArgs e)
        {
        }

        private void EditorContainer_EditorRequested(object? sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEditView();
            LoadFile(edit.Editor.Source, string.Format("{0}.{1}", ProjectFolder, e.FileName));
            edit.Editor.FileName = e.FileName;
            e.DebugEdit = edit;
            UpdateCommands();
        }

        private void OpenProject(string? projectPathUrl, string projectName)
        {
            if (projectPathUrl is null || Project is null)
                return;

            var projectFileUrl = string.Format("{0}.{1}", projectPathUrl, projectName);
            var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(projectFileUrl);
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            if (stream is null || Project is null)
                return;

            Project.Load(stream, projectName);
            
            scriptRun.ScriptSource.FromScriptProject(Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(projectFileUrl), Project.ProjectFileName);
            var extension = Project.ProjectExtension;
            OpenProject(extension, Project);

            var codeFiles = Project.Files.Where(
                x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".vb").ToList();

            var firstFile = GetFirstFile(codeFiles, Project.DefaultExtension);

            if (codeFiles.Count > 0)
            {
                while (codeFiles.Count > 5)
                    codeFiles.RemoveAt(codeFiles.Count - 1);

                foreach (var codeFile in codeFiles)
                {
                    codeEditContainer.TryActivateEditor(codeFile);
                }

                codeEditContainer.TryActivateEditor(firstFile);
            }

            var references = Project.References.Concat(Project.AutoReferences).Select(
                x => x.FullName).Concat(
                Project.FrameworkReferences.SelectMany(x => x.Assemblies).Select(
                    x => x.HintPath)).ToArray();

            DebuggerPanelsTabControl.Errors.Clear();
            UpdateToolbar();
            UpdateCommands();
        }

        private void OpenProject(string extension, DotNetProject project)
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
                .Select(x => x.FullName)
                .Concat(project.FrameworkReferences
                    .SelectMany(x => x.Assemblies).Select(x => x.HintPath)).Distinct().ToArray();

            if (references is null)
                return;

            RegisterAssemblies(
                extension,
                project.TryResolveAbsolutePaths(references).ToArray(),
                projectName: project.ProjectName,
                targetFramework: project.TargetFramework);
        }

        private void RegisterAssemblies(
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

            if (technology == null)
                technology = DetectTechnologyEnvironmentFromReferences(references);

            if (targetFramework == null)
                targetFramework = DetectTargetFrameworkFromReferences(references);

            solution.WithDefaultAssemblies(technology.Value, keepExisting, projectId, targetFramework).RegisterAssemblies(references, projectId, targetFramework);
        }

        private TargetFramework? DetectTargetFrameworkFromReferences(string[] references)
        {
            foreach (var reference in references)
            {
                if (DotNetCoreReferencesDetector.IsDotNetCoreReference(reference, out var version))
                    return new TargetFramework(version, isDotNetCore: true);
            }

            return null;
        }

        private TechnologyEnvironment DetectTechnologyEnvironmentFromReferences(string[] references)
        {
            return TechnologyEnvironment.System;
        }

        private void RegisterCode(string extension, string[] files, string? projectName = null)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            var project = !string.IsNullOrEmpty(projectName) ? solution.GetProject(projectName) : null;
            solution.RegisterCodeFiles(files, project != null ? project.Id : null);
        }

        private IRoslynSolution? GetSolution(string extension)
        {
            IRoslynSolution? result = null;
            switch (extension.ToLower())
            {
                case ".cs":
                    result = CsSolution.DefaultSolution;
                    break;
                case ".vb":
                    result = VbSolution.DefaultSolution;
                    break;
                case ".csx":
                    result = CsSolution.DefaultScriptSolution;
                    break;
                case ".vbx":
                    result = VbSolution.DefaultScriptSolution;
                    break;
                default:
                    result = null;
                    break;
            }

            if(result is not null)
            {
            }

            return result;
        }

        private void StopDebugger()
        {
            if (debugger != null && debugger.IsStarted)
                if (debugger.IsStarted)
                    debugger.StopDebuggingAsync();
        }

        private void CloseProject(DotNetProject project)
        {
            StopDebugger();
            foreach (string fileName in project.Files)
            {
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

            var extension = string.Format(".{0}", project.DefaultExtension);

            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
            UpdateToolbar();
            UpdateCommands();
        }

        private void UpdateToolbar()
        {
            DebugMenu.IsEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
            DebuggerControlToolbar.IsEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
        }

        private void UpdateCommands()
        {
            (CloseProjectCommand as Command)?.ChangeCanExecute();
            (CloseCommand as Command)?.ChangeCanExecute();
            (SaveCommand as Command)?.ChangeCanExecute();
        }

        private ILexer? DoCreateParser(Type type)
        {
            return Activator.CreateInstance(type) as ILexer;
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private async void OpenProjectDialog()
        {
            var customProjectTypeCs = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                                    { DevicePlatform.WinUI, csProjExtensions },
                    });

            var customProjectTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, vbProjExtensions },
                    });

            PickOptions options = new()
            {
                PickerTitle = "Please select a C# project file",
                FileTypes = customProjectTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var dirPath = Path.GetDirectoryName(files.FullPath);
            string projName = Path.GetFileName(files.FullPath);
            OpenProject(dirPath, projName);
        }

        private async void OpenFileDialog()
        {
            var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, csExtensions },
                });

            var customFileTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, vbExtensions },
                    });

            PickOptions options = new()
            {
                PickerTitle = "Please select a C# file",
                FileTypes = customFileTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var edit = codeEditContainer.ActiveEditor;
            LoadFile(edit.Editor.Source, files.FullPath);
            UpdateToolbar();
        }
    }
}
