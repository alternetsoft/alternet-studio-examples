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

using Alternet.Common.Projects.DotNet;
using Alternet.Maui;

using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.MAUI;
using Alternet.Scripter.Integration.MAUI;
using Alternet.Syntax.Lexer;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Python;
using Alternet.Common.Python;
using Microsoft.Maui.Layouts;
using Alternet.Common;


namespace DebuggerIntegration
{
    public partial class MainPage : ContentPage
    {
        private static bool ExceptionsLogger = false;

        internal string ProjectFolder = "embres:DebuggerIntegration.Content";
        private static readonly string[] pyExtensions = [".py"];
        private static readonly string[] pyProjExtensions = [".pyproj"];

        private DebuggerPanelsTabControlView DebuggerPanelsTabControl = new();
        private SimpleTabControlView EditorsTabControl = new();

        private Alternet.Scripter.Debugger.Python.ScriptDebugger debugger;
        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;

        private PythonProject project = new();
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

            Alternet.Scripter.Python.ScriptEngine.RunProcessFunc
                = Alternet.UI.ProcessRunnerWithNotification.RunProcess;
        }

        public MainPage()
        {
            debugger = new Alternet.Scripter.Debugger.Python.ScriptDebugger();
            scriptRun = new ScriptRun();
            debugger.ScriptRun = scriptRun;

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
                    StopDebugger();
                    var edit = codeEditContainer?.ActiveEditor;
                    if (edit != null)
                    {
                        codeEditContainer?.CloseFile(edit.Editor.FileName);
                        edit.Editor.FileName = string.Empty;
                    }

                    if (!Project.HasProject && codeEditContainer?.Editors.Count == 0)
                    {
                        Project?.Reset();
                        scriptRun?.ScriptSource?.Reset();
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
                  var edit = codeEditContainer?.ActiveEditor?.Editor;
                  if (edit != null)
                      edit.SaveFile(edit.FileName);
              },
             canExecute: () =>
             {
                 return codeEditContainer?.ActiveEditor != null;
             });

            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

            if(Consts.IsMacOs)
                FileMenu.Remove(ExitMenuItem);

            AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

            MainGrid.Insert(0, DebuggerControlToolbar);
            panel.Add(EditorsTabControl);
            panel.Add(DebuggerPanelsTabControl, 0, 1);
            DebuggerPanelsTabControl.MinimumHeightRequest = 200;

            DebugCodeEdit.Parsers[".py"] = typeof(PythonNETParser);
            DebugCodeEdit.CreateParserFunc = DoCreateParser;

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            string[] projectFiles =
                [
                    "MyModule.py",
                    "Project.pyproj",
                    "ScriptSimple.py",
                ];

            var destFolder = PathUtilities.GetTempPathUniquePerApp();

            var extractionResult = Alternet.UI.ResourceLoader.ExtractResourcesSafe(
                ProjectFolder,
                projectFiles,
                destFolder);

            OpenProject(destFolder, "Project.pyproj");

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

            var editor = codeEditContainer.Editors.FirstOrDefault();

            if (editor is not null)
            {
            }
        }

        protected PythonProject Project
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

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.Editor.SaveFile(edit.Editor.FileName);
            }
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
                string fileName = codeEditContainer.ActiveEditor.Editor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void EditorContainer_EditorRequested(object? sender, DebugEditRequestedEventArgs e)
        {
            var parser = new PythonNETParser
            {
                CodeEnvironment = scriptRun.CodeEnvironment,
            };

            var edit = new DebugCodeEditView();
            LoadFile(edit.Editor.Source, e.FileName);
            edit.Editor.FileName = e.FileName;
            edit.Lexer = parser;
            e.DebugEdit = edit;
            UpdateCommands();
        }

        private void OpenProject(string? projectPathUrl, string projectName)
        {
            if (projectPathUrl is null)
                return;

            if (Project is null)
                return;

            if (Project.HasProject)
                CloseProject(Project);

            var projectFileUrl = Path.Combine(projectPathUrl, projectName);
            var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(projectFileUrl);

            if (stream is null)
                return;

            Project.Load(projectFileUrl);

            scriptRun.ScriptSource.FromScriptProject(projectFileUrl);

            var codeFiles = Project.Files.Where(
                x => Path.GetExtension(x) == ".py").ToList();

            if (codeFiles.Count > 0)
            {
                codeEditContainer.TryActivateEditor(GetFirstFile(codeFiles, Project.DefaultExtension));
            }

            var references = Project.References.Concat(Project.AutoReferences).Select(
                x => x.FullName).Concat(
                Project.FrameworkReferences.SelectMany(x => x.Assemblies).Select(
                    x => x.HintPath)).ToArray();

            DebuggerPanelsTabControl.Errors.Clear();
            UpdateToolbar();
            UpdateCommands();
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
            var customProjectTypePy = new FilePickerFileType(
    new Dictionary<DevicePlatform, IEnumerable<string>>
    {
                    { DevicePlatform.WinUI, pyProjExtensions },
    });

            PickOptions options = new()
            {
                PickerTitle = "Please select a Py proj file",
                FileTypes = customProjectTypePy,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            string dirPath = Path.GetDirectoryName(files.FullPath);
            string projName = Path.GetFileName(files.FullPath);
            OpenProject(dirPath, projName);
        }

        private async void OpenFileDialog()
        {
            var customFileTypePy = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, pyExtensions },
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a Py file",
                FileTypes = customFileTypePy,
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
