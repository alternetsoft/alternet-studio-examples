#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

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
using System.Diagnostics;
using Alternet.Syntax;


namespace DebuggerIntegration
{
    public partial class MainPage
        : Alternet.UI.DisposableContentPage, Alternet.UI.IProcessRunnerNotification
    {
        private static bool ExceptionsLogger = false;

        internal string ProjectFolder = "embres:DebuggerIntegration.Content";
        private static readonly string[] pyExtensions = [".py"];
        private static readonly string[] pyProjectExtensions = [".pyproj"];

        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;
        private readonly DebugMenu debugMenu = new();
        private readonly DebuggerControlToolBarView debuggerControlToolBar = new();
        private readonly DebuggerPanelsTabControlView debuggerPanelsTabControl = new();
        private readonly SimpleTabControlView editorsTabControl = new();

        private PythonProject project = new();
        private Alternet.Scripter.Debugger.Python.ScriptDebugger debugger;

        public ICommand OpenProjectCommand { set; get; }
        public ICommand CloseProjectCommand { set; get; }
        public ICommand OpenCommand { set; get; }
        public ICommand CloseCommand { set; get; }
        public ICommand SaveCommand { set; get; }
        public ICommand ExitCommand { set; get; }
        Alternet.UI.ObjectUniqueId Alternet.UI.IProcessRunnerNotification.UniqueId { get; } = new();

        static MainPage()
        {
            Alternet.UI.MauiUtils.SuppressMenuBarFocus();

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

            Alternet.UI.App.LogMessage += (s, e) =>
            {

            };
        }

        public MainPage()
        {
            PathUtilities.UseAppSubFolderAsTempPath = true;
            debugger = new Alternet.Scripter.Debugger.Python.ScriptDebugger();
            debugger.EventsSyncAction = (action) => Alternet.UI.App.Invoke(action);
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
                        codeEditContainer?.CloseFile(edit.FileName);
                        edit.FileName = string.Empty;
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
                  var edit = codeEditContainer?.ActiveEditor;
                  if (edit != null)
                      edit.SaveFile(edit.FileName);
              },
             canExecute: () =>
             {
                 return codeEditContainer?.ActiveEditor != null;
             });

            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

            OpenProjectMenuItem.Command = OpenProjectCommand;
            CloseProjectMenuItem.Command = CloseProjectCommand;
            OpenMenuItem.Command = OpenCommand;
            SaveMenuItem.Command = SaveCommand;
            CloseMenuItem.Command = CloseCommand;
            ExitMenuItem.Command = ExitCommand;

            if (Consts.IsMacOs)
                FileMenu.Remove(ExitMenuItem);

            AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

            MainGrid.Insert(0, debuggerControlToolBar);
            debuggerControlToolBar.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsTopBorderVisible = true;
            panel.Add(editorsTabControl);
            panel.Add(debuggerPanelsTabControl, 0, 1);
            debuggerPanelsTabControl.MinimumHeightRequest = 200;
            debuggerPanelsTabControl.MaximumHeightRequest = 200;

            editorsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.StickyStyle = SimpleToolBarView.StickyButtonStyle.Border;
            editorsTabControl.Header.StickyStyle = SimpleToolBarView.StickyButtonStyle.Border;

            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".py"]
                = typeof(PythonNETParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.CreateParserFunc = DoCreateParser;

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
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

            debuggerControlToolBar.Debugger = debugger;
            debuggerControlToolBar.DebuggerPreStartup += OnDebuggerPreStartup;

            debugMenu.Debugger = debugger;
            debugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new Alternet.Scripter.Integration.AlternetUI.DebuggerUIController(
                this, codeEditContainer)
            {
                Debugger = debugger,
                DebuggerPanels = debuggerPanelsTabControl
            };

            codeEditContainer.Debugger = debugger;

            MenuBarItems.Add(debugMenu);

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

            Alternet.UI.ProcessRunnerWithNotification.Bind(this);

            var window = Application.Current?.Windows.FirstOrDefault();
            if(window is not null)
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
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var editor = codeEditContainer.FirstOrDefault();

            if (editor is null)
                return;

            await editor.TrySetFocusWithTimeout();
        }

        protected override void DisposeResources()
        {
            StopDebugger();
            Alternet.UI.ProcessRunnerWithNotification.Unbind(this);
            base.DisposeResources();
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
                    edit.SaveFile(edit.FileName);
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
            var parser = new PythonNETParser
            {
                CodeEnvironment = scriptRun.CodeEnvironment,
            };

            var edit = new DebugCodeEditView();
            edit.SetBorderWidth(0, 1, 0, 1);

            LoadFile(edit.Editor.Source, e.FileName);
            edit.Editor.FileName = e.FileName;
            edit.Lexer = parser;
            e.DebugEdit = edit.Editor;
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

            debuggerPanelsTabControl.Errors?.Clear();
            UpdateToolbar();
            UpdateCommands();
        }

        private async void StopDebugger()
        {
            if (debugger != null && debugger.IsStarted)
                await debugger.StopDebuggingAsync();
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
            var customProjectTypePy = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                                { DevicePlatform.WinUI, pyProjectExtensions },
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a Py proj file",
                FileTypes = customProjectTypePy,
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
            LoadFile(edit?.Source, files.FullPath);
            UpdateToolbar();
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

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessLog(
            Process process,
            string data,
            Alternet.UI.LogItemKind kind)
        {
            debuggerPanelsTabControl.Output?.WriteLineAndWait(data);
        }
    }
}
