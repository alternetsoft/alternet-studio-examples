#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml;

using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Common.Projects.DotNet;
using Alternet.Maui;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.MAUI;
using Alternet.Scripter.Integration.MAUI;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.Maui.Layouts;

namespace DebuggerIntegration
{
    public partial class MainPage
        : Alternet.UI.DisposableContentPage, Alternet.UI.IProcessRunnerNotification,
        Alternet.UI.IRaiseSystemColorsChanged
    {
        private static readonly bool ExceptionsLogger = true;

        private static readonly bool DefaultWordWrap;

        private static readonly string ProjectFolder
            = "embres:DebuggerIntegration.Content.DebuggerTest";

        private static readonly string[] VBExtensions = [".vb"];
        private static readonly string[] CSExtensions = [".cs"];
        private static readonly string[] VBProjectExtensions = [".vbproj"];
        private static readonly string[] CSProjectExtensions = [".csproj"];

        private readonly DebuggerPanelsTabControlView debuggerPanelsTabControl = new();
        private readonly SimpleTabControlView editorsTabControl = new();
        private readonly DebugMenu debugMenu = new();
        private readonly DebuggerControlToolBarView debuggerControlToolBar = new();

        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;

        private DotNetProject project = new();
        private IScriptDebuggerBase debugger;

        static MainPage()
        {
            Alternet.UI.MauiUtils.SuppressMenuBarFocus();

            DefaultWordWrap = !Consts.IsWindows;

            CoreClrLauncher.RunProcessFunc = Alternet.UI.ProcessRunnerWithNotification.RunProcess;
            CoreClrLauncher.NetCoreAppConfigOnWindows = true;

            if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                    if (e is System.Runtime.InteropServices.COMException)
                        return;
                    if (e is XmlException)
                        return;
                    if (e is OperationCanceledException)
                        return;
                    Nop();
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
                        codeEditContainer.CloseFile(edit.FileName);
                        edit.FileName = string.Empty;
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
                  var edit = codeEditContainer.ActiveEditor;
                  edit?.SaveFile(edit.FileName);
              },
              canExecute: () =>
              {
                  return codeEditContainer?.ActiveEditor != null;
              });

            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

            var fileMenu = new MenuBarItem
            {
                Text = "File",
            };

            // Creating menu items
            var openProjectMenuItem = new MenuFlyoutItem
            {
                Text = "Open Project...",
                AutomationId = "OpenProjectMenuItem",
            };

            var closeProjectMenuItem = new MenuFlyoutItem
            {
                Text = "Close Project",
                AutomationId = "CloseProjectMenuItem",
            };

            var openMenuItem = new MenuFlyoutItem
            {
                Text = "Open...",
                AutomationId = "OpenMenuItem",
            };

            var saveMenuItem = new MenuFlyoutItem
            {
                Text = "Save",
                AutomationId = "SaveMenuItem",
            };

            var closeMenuItem = new MenuFlyoutItem
            {
                Text = "Close File",
                AutomationId = "CloseMenuItem",
            };

            var exitMenuItem = new MenuFlyoutItem
            {
                Text = "Exit",
                AutomationId = "ExitMenuItem",
            };

            // Adding items to the menu
            fileMenu.Add(openProjectMenuItem);
            fileMenu.Add(closeProjectMenuItem);
            fileMenu.Add(openMenuItem);
            fileMenu.Add(saveMenuItem);
            fileMenu.Add(closeMenuItem);
            fileMenu.Add(exitMenuItem);

            MenuBarItems.Add(fileMenu);

            if (Consts.IsMacOs)
                fileMenu.Remove(exitMenuItem);

            AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

            MainGrid.Insert(0, debuggerControlToolBar);
            debuggerControlToolBar.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsTopBorderVisible = true;

            SimpleToolBarView.StickyButtonStyle tabStyle;

            tabStyle = SimpleToolBarView.StickyButtonStyle.Border;

            debuggerPanelsTabControl.Header.StickyStyle = tabStyle;
            editorsTabControl.Header.StickyStyle = tabStyle;

            panel.Add(editorsTabControl);

            editorsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsBottomBorderVisible = true;

            panel.Add(debuggerPanelsTabControl, 0, 1);
            debuggerPanelsTabControl.MinimumHeightRequest = 200;
            debuggerPanelsTabControl.MaximumHeightRequest = 200;

            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".cs"] = typeof(CsParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".vb"] = typeof(VbParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.CreateParserFunc = DoCreateParser;
            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            string[] projectFiles =
                [
                    "DebuggerTest.cs",
                    "DebuggerTest.csproj",
                    "DebuggerTest_Second.cs",
                ];

            var destFolder = PathUtilities.GetTempPathUniquePerApp();

            var extractionResult = Alternet.UI.ResourceLoader.ExtractResourcesSafe(
                ProjectFolder,
                projectFiles,
                destFolder);

            OpenProject(destFolder, "DebuggerTest.csproj");
            var useOldDebugger = true;

            if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-NewDebugger"))
                useOldDebugger = false;

            debugger = Alternet.Scripter.Debugger.UI.AlternetUI.DebuggerUtils
                .CreateDebugger(useOldDebugger)!;
            debugger.EventsSyncAction = (action) => Alternet.UI.App.Invoke(action);
            Alternet.UI.AssemblyUtils.TrySetMemberValue(debugger, "ScriptRun", scriptRun);
            Alternet.Scripter.Roslyn.RoslynScriptProvider.PortablePdb
                = !useOldDebugger || !Alternet.UI.App.IsWindowsOS;

            debuggerControlToolBar.Debugger = debugger;
            debuggerControlToolBar.DebuggerPreStartup += OnDebuggerPreStartup;

            debugMenu.Debugger = debugger;
            debugMenu.DebuggerPreStartup += OnDebuggerPreStartup;
            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new Alternet.Scripter.Integration.AlternetUI
                .DebuggerUIController(this, codeEditContainer)
            {
                Debugger = debugger,
                DebuggerPanels = debuggerPanelsTabControl,
            };

            codeEditContainer.Debugger = debugger;

            MenuBarItems.Add(debugMenu);

            if (Alternet.UI.App.IsWindowsOS)
            {
                openMenuItem.Command = OpenCommand;
                openProjectMenuItem.Command = OpenProjectCommand;
            }

            closeProjectMenuItem.Command = CloseProjectCommand;
            closeMenuItem.Command = CloseCommand;
            saveMenuItem.Command = SaveCommand;
            exitMenuItem.Command = ExitCommand;

            UpdateToolbar();
            UpdateCommands();

            Alternet.UI.App.LogMessage += (s, e) =>
            {
            };

            Alternet.UI.ProcessRunnerWithNotification.Bind(this);

            var window = Application.Current?.Windows.FirstOrDefault();
            if (window is not null)
            {
                window.Stopped += (s, e) =>
                {
                    debuggerControlToolBar.Debugger = null;
                    StopDebugger();
                };

                window.Destroying += (s, e) =>
                {
                    debuggerControlToolBar.Debugger = null;
                    StopDebugger();
                };
            }

            editorsTabControl.SelectedTabChanged += (s, e) =>
            {
                codeEditContainer.ActiveEditor?.SetFocusIfPossible();
            };

            RaiseSystemColorsChanged();
        }

        public ICommand OpenProjectCommand { get; set; }

        public ICommand CloseProjectCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        Alternet.UI.ObjectUniqueId Alternet.UI.IProcessRunnerNotification.UniqueId { get; } = new();

        protected DotNetProject Project
        {
            get => project;
            private set => project = value;
        }

        public static void Nop()
        {
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

        public static string GetFirstFile(IList<string> files, string langExt)
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

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessLog(
            Process process,
            string data,
            Alternet.UI.LogItemKind kind)
        {
            debuggerPanelsTabControl.Output?.WriteLineAndWait(data);
        }

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessStarted(Process process)
        {
        }

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessDisposed(Process process)
        {
        }

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessExited(Process process)
        {
        }

        public void RaiseSystemColorsChanged()
        {
            Color? headerColor;

            if (Alternet.UI.SystemSettings.AppearanceIsDark)
            {
                headerColor = Color.FromRgb(37, 37, 38);
            }
            else
            {
                headerColor = Color.FromRgb(245, 245, 245);
            }

            editorsTabControl.Header.BackgroundColor = headerColor;
            debuggerPanelsTabControl.Header.BackgroundColor = headerColor;
        }

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var editor = codeEditContainer.FirstOrDefault();

            if (editor is null)
                return;

            await editor.TrySetFocusWithTimeout();
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Window" || propertyName == "Parent")
            {
            }
        }

        protected override void DisposeResources()
        {
            Alternet.UI.ProcessRunnerWithNotification.Unbind(this);
            base.DisposeResources();
        }

        private void OnDebuggerPreStartup(object? sender, System.EventArgs e)
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
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void EditorContainer_EditorRequested(
            object? sender,
            Alternet.Scripter.Integration.AlternetUI.DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEditView();
            edit.WordWrap = DefaultWordWrap;
            edit.SetBorderWidth(0, 1, 0, 1);

            LoadFile(edit.Editor.Source, e.FileName);
            edit.Editor.FileName = e.FileName;
            e.DebugEdit = edit.Editor;
            UpdateCommands();
        }

        private void OpenProject(string? projectPathUrl, string projectName)
        {
            if (projectPathUrl is null || Project is null)
                return;

            var projectFileUrl = Path.Combine(projectPathUrl, projectName);
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            if (Project is null)
                return;

            Project.Load(projectFileUrl);

            scriptRun.ScriptSource.FromScriptProject(projectFileUrl);
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

            debuggerPanelsTabControl.Errors?.Clear();
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
                RegisterCode(
                    extension,
                    project.Files.Where(x => x.EndsWith(project.ProjectExtension)).ToArray(),
                    project.ProjectName);
            }

            var references = project.References
                .Concat(project.AutoReferences)
                .Select(x => x.FullName ?? string.Empty)
                .Concat(project.FrameworkReferences
                    .SelectMany(x => x.Assemblies).Select(x => (x.HintPath ?? string.Empty)))
                .Distinct().ToArray();

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

            solution.WithDefaultAssemblies(
                technology.Value,
                keepExisting,
                projectId,
                targetFramework)
                .RegisterAssemblies(references, projectId, targetFramework);
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

            if (result is not null)
            {
            }

            return result;
        }

        private void StopDebugger()
        {
            if (debugger != null && debugger.IsStarted)
            {
                if (debugger.IsStarted)
                    debugger.StopDebuggingAsync();
            }
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
            debugMenu.IsEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
            debuggerControlToolBar.IsEnabled = (Project != null && Project.HasProject)
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
            var result = Activator.CreateInstance(type) as ILexer;

            if (result is SyntaxParser parser)
            {
                parser.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                    | SyntaxOptions.SyntaxErrors;
            }

            return result;
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
                                    { DevicePlatform.WinUI, CSProjectExtensions },
                    });

            var customProjectTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, VBProjectExtensions },
                    });

            PickOptions options = new()
            {
                PickerTitle = "Select a C# project file",
                FileTypes = customProjectTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var dirPath = Path.GetDirectoryName(files.FullPath);
            string projectName = Path.GetFileName(files.FullPath);
            OpenProject(dirPath, projectName);
        }

        private async void OpenFileDialog()
        {
            var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, CSExtensions },
                });

            var customFileTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, VBExtensions },
                    });

            PickOptions options = new()
            {
                PickerTitle = "Select a C# file",
                FileTypes = customFileTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var edit = codeEditContainer.ActiveEditor;

            if (edit is not null)
            {
                edit.WordWrap = DefaultWordWrap;
            }

            LoadFile(edit?.Source, files.FullPath);
            UpdateToolbar();
        }
    }
}
