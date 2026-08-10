using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.Wpf;
using Alternet.Scripter.Integration.Wpf;
using Microsoft.Win32;

namespace DebuggerNamedPipes.Wpf
{
    public partial class MainWindow : Window
    {
        private Task? serverTask;
        private CancellationTokenSource cts = new();

        private IScriptDebuggerBase? debugger;

        private DebugCodeEditContainer codeEditContainer;
        private DebuggerUIController controller;
        private IScriptRun scriptRun;
        private bool useNewDebugger = false;

        public MainWindow()
        {
            InitializeComponent();

            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;
            controller = new DebuggerUIController(Dispatcher, codeEditContainer);
            controller.DebuggerPanels = DebuggerPanelsTabControl;

            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.InstallKeyboardShortcuts(CommandBindings);
            FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;
            UpdateDebugControls();

            OpenProject(FindProjectFile());

            RestartTask();
        }

        public static string[] ProjectSearchDirectories { get; set; } = new[] { ".", @"..\..\..\..\..\..\..\" };

        public static string StartupProjectFileSubPath { get; set; } = @"Resources\Debugger\CS\Other\DbgNamedPipes\DbgNamedPipes.csproj";

        public DebugCodeEditContainer CodeEditContainer => codeEditContainer;

        public DotNetProject Project { get; private set; } = new DotNetProject();

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

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Browsable(false)]
        public IScriptDebuggerBase? Debugger
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

        internal Task? ServerTask => serverTask;

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private static async Task<string> LogViaPipes(string s, string color = "Black")
        {
            return await DbgNamedPipes.SendRequest("Log", new string[] { s, color });
        }

        private static string? FindProjectFile()
        {
            return ProjectSearchDirectories.Select(
                x => Path.GetFullPath(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);
        }

        private static string? FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x,
                    Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

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

        private void Debugger_ExecutionStopped(object? sender, ExecutionStoppedEventArgs e)
        {
            if (e.StopReason == ExecutionStopReason.Exception || e.StopReason == ExecutionStopReason.UnhandledException)
                DisplayDebuggerException(e);
        }

        private void DisplayDebuggerException(ExecutionStoppedEventArgs e)
        {
            var str = $"{e.Exception?.ExceptionType}\n{e.Exception?.Message}";
            using (var dlg = new DebuggerException(debugger, str))
            {
                dlg.Title = e.StopReason == ExecutionStopReason.UnhandledException ? StringConsts.UnhandledException : StringConsts.DebuggerException;
                dlg.ExceptionEvaluationExpression = e.Exception?.ExceptionEvaluationExpression;
                dlg.OnEvaluate += Dlg_OnEvaluate;
                dlg.ShowDialog();
            }
        }

        private void Dlg_OnEvaluate(object? sender, EventArgs e)
        {
            EvaluateExpression(true);
        }

        private void EvaluateExpression(bool evaluateCurrentException = false)
        {
            var edit = codeEditContainer.ActiveEditor;
            var symbol = edit != null ? edit.GetSymbolAtCursor() : string.Empty;
            var dialog = new EvaluateDialog(
                EvaluateDialog.CodeCompletionOptions.Custom((s, d, tb) => new EditorCodeCompletionController(s, d, tb)))
            {
                Debugger = debugger,
                EvaluateCurrentException = evaluateCurrentException,
                Expression = symbol,
            };
            {
                var watchesControl = DebuggerPanelsTabControl.Watches;

                dialog.WatchAdded += (o, e) => watchesControl.AddWatch(e.Expression);
                dialog.WatchAdded += (o, e) => ActivateWatchesTab();
                dialog.ShowDialog();
            }
        }

        private void ActivateWatchesTab()
        {
            DebuggerPanelsTabControl.FocusPanel(DebuggerPanelKinds.Watches);
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

        private void OpenProject(string? projectFilePath)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

            if (Project != null && Project.HasProject)
                CloseProject(Project);

            if (Project is null || projectFilePath is null)
                return;

            Project.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            var extension = Project.ProjectExtension;
            CodeEditExtensions.OpenProject(extension, Project.ProjectName, Project.ProjectFileName);

            var codeFiles = Project.Files.Where(x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".vb").ToList();

            if (codeFiles.Count > 0)
            {
                foreach (var file in codeFiles.ToArray())
                {
                    string formId;
                    if (FormFilesUtility.IsXamlCodeBehindFile(file, out formId))
                    {
                        codeFiles.Add(
                            XamlGeneratedCodeFileService.GetGeneratedCodeFile(Project, new FormDesignerDataSource(formId, FormFilesUtility.DetectLanguageFromFileName(file))));
                    }
                }

                CodeEditExtensions.RegisterCode(extension, codeFiles.ToArray(), Project.ProjectName);
                codeEditContainer.TryActivateEditor(GetFirstFile(codeFiles, Project.DefaultExtension));
            }

            var references = Project.References.Concat(Project.AutoReferences).Select(x => x.FullName).Concat(
                Project.FrameworkReferences.SelectMany(x => x.Assemblies).Select(x => x.HintPath)).ToArray();

            if (references is not null)
            {
                CodeEditExtensions.RegisterAssemblies(
                   extension,
                   Project.TryResolveAbsolutePaths(references!).ToArray(),
                   projectName: Project.ProjectName,
                   targetFramework: Project.TargetFramework);
            }

            DebuggerPanelsTabControl.Errors.Clear();
            UpdateDebugControls();
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

            DebuggerControlToolbar.Debugger = enabled ? debugger : null;
            DebugMenu.Debugger = enabled ? debugger : null;
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
            scriptRun.ScriptSource?.Reset();
            UpdateDebugControls();
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private string? GetProjectName(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return Project.ProjectName;
            }

            return null;
        }

        private void EditorContainer_EditorRequested(object? sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            var projectName = GetProjectName(e.FileName);
            edit.SetFileNameAndProject(e.FileName, projectName);
            edit.LoadFile(e.FileName);
            e.DebugEdit = edit;
        }

        private void UseUniversalDebuggerMenuItem_Click(object? sender, System.EventArgs e)
        {
            if (ChangeDebuggerMode(!useUniversalDebuggerMenuItem.IsChecked))
                useUniversalDebuggerMenuItem.IsChecked = UseNewDebugger;
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
                debugger = new Alternet.Scripter.Debugger.Universal.ScriptDebugger { ScriptRun = scriptRun };
            else
                debugger = new Alternet.Scripter.Debugger.ScriptDebugger { ScriptRun = scriptRun };

            var myPlatform = Consts.IsNetFramework ? ".NET Framework" : ".NET Core";

            if (UseNewDebugger)
            {
                LogToOutput($"Using universal debugger on {myPlatform} platform");
            }
            else
            {
                LogToOutput($"Using legacy debugger on {myPlatform} platform");
            }

            debugger.ExecutionStopped += Debugger_ExecutionStopped;

            controller.Debugger = debugger;
            codeEditContainer.Debugger = debugger;

            DebuggerPanelsTabControl.Breakpoints.Debugger = debugger;
            DebuggerPanelsTabControl.CallStack.Debugger = debugger;
            DebuggerPanelsTabControl.Output.Debugger = debugger;
            DebuggerPanelsTabControl.Locals.Debugger = debugger;
            DebuggerPanelsTabControl.Watches.Debugger = debugger;
            DebuggerPanelsTabControl.Errors.Debugger = debugger;
            DebuggerPanelsTabControl.Threads.Debugger = debugger;
        }

        private void LogToOutput(string s)
        {
            DebuggerPanelsTabControl.Output?.CustomLog(s + Environment.NewLine);
        }

        private void FinalizeDebugger()
        {
            if (debugger == null)
                return;
            debugger.ExecutionStopped -= Debugger_ExecutionStopped;
            controller.Debugger = null;
            codeEditContainer.Debugger = null;
            DebuggerPanelsTabControl.Breakpoints.Debugger = null;
            DebuggerPanelsTabControl.CallStack.Debugger = null;
            DebuggerPanelsTabControl.Output.Debugger = null;
            DebuggerPanelsTabControl.Locals.Debugger = null;
            DebuggerPanelsTabControl.Watches.Debugger = null;
            DebuggerPanelsTabControl.Errors.Debugger = null;
            DebuggerPanelsTabControl.Threads.Debugger = null;
            DebuggerControlToolbar.Debugger = null;
            DebugMenu.Debugger = null;

            (debugger as IDisposable)?.Dispose();
            debugger = null;
        }

        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Project files (*.csproj; *.vbproj)|*.csproj;*.vbproj|All files (*.*)|*.*",
                InitialDirectory = FindDefaultProjectDirectory(),
            };

            if (dialog.ShowDialog(this) != true)
                return;

            OpenProject(dialog.FileName);
        }

        private void CloseProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseProject(Project);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindProjectFile());
            if (dialog.ShowDialog() == true)
            {
                codeEditContainer.TryActivateEditor(dialog.FileName);

                UpdateDebugControls();
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
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
                scriptRun.ScriptSource?.Reset();
            }

            UpdateDebugControls();
        }

        private void ExitMenuItem_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FileMenu_SubmenuOpened(object? sender, RoutedEventArgs e)
        {
            CloseProjectMenuItem.IsEnabled = Project != null && Project.HasProject;
            CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }

        private void CancelTask()
        {
            if (serverTask != null)
            {
                cts.Cancel();
                serverTask.Wait();
                serverTask = null;
            }
        }

        private void RestartTask()
        {
            CancelTask();

            serverTask = Task.Run(() => DbgNamedPipes.InitServer(HandleRequest, cts.Token), cts.Token);
        }

        private void Form1_Closing(object? sender, CancelEventArgs e)
        {
            cts.Cancel();
        }

        private DbgNamedPipes.Response HandleRequest(DbgNamedPipes.Request request)
        {
            DbgNamedPipes.Response response = new();

            if (request is null)
            {
                return response;
            }

            if (request.Method == "Log")
            {
                var message = request?.Args?[0] ?? string.Empty;
                var color = request?.Args?.Length > 1 ? request.Args[1] : "Black";
                Log(message, color);
                response.StringResult = "Logged: " + message;
            }
            else
            {
                response.StringResult = "Unknown method: " + request.Method;
            }

            return response;
        }

        private void Log(string text, string color)
        {
            Log(text, System.Drawing.Color.FromName(color));
        }

        private void Log(string text, System.Drawing.Color color)
        {
            Dispatcher.Invoke(() =>
            {
                DebuggerPanelsTabControl.Output.CustomLog($"{text} ({color})" + Environment.NewLine);
            });
        }

        private void Debugger_StateChanged(object? sender, Alternet.Scripter.Debugger.DebuggerStateChangedEventArgs e)
        {
            var state = e.NewState;

            if (state == Alternet.Scripter.Debugger.DebuggerState.Startup)
            {
            }
        }
    }
}