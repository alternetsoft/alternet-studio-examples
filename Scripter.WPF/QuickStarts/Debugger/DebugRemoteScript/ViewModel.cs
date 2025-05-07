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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Integration.Wpf;

namespace DebugRemoteScript.Wpf
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private ScriptDebugger scriptDebugger;
        private MainWindow window;
        private IScriptEdit edit;
        private IScriptAPI scriptAPI;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ExecutionPosition executionPosition;
        private string ipcPortName = null;
        private string ipcObjectUri = null;
        private string lang = string.Empty;
        private bool useDirectAPI = true;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
            StartDebug = new RelayCommand(StartDebugClick);
            StopDebug = new RelayCommand(StopDebugClick);
            StepOver = new RelayCommand(StepOverClick);
            Continue = new RelayCommand(ContinueClick);
        }

        public ViewModel(MainWindow window, string[] args)
            : this()
        {
            this.window = window;
            ParseCommandLineArgs(args);
            scriptAPI = new ScriptAPI(this.window);

            CreateEditorAndDebugger();

            InitializeDebugger();
            UpdateDebuggerControls(DebuggerState.Off);

            languages.Add("C#");
            languages.Add("Visual Basic");
            Language = "C#";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return lang;
            }

            set
            {
                if (lang != value)
                {
                    lang = value;
                    OnPropertyChanged("Language");
                    UpdateSource();
                }
            }
        }

        public bool UseDirectAPI
        {
            get
            {
                return useDirectAPI;
            }

            set
            {
                if (useDirectAPI != value)
                {
                    useDirectAPI = value;
                    OnPropertyChanged("UseDirectAPI");
                }
            }
        }

        public ICommand RunScript { get; set; }

        public ICommand StartDebug { get; set; }

        public ICommand StopDebug { get; set; }

        public ICommand StepOver { get; set; }

        public ICommand Continue { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void InitializeDebugger()
        {
            this.window.debuggerControlToolbar.Debugger = scriptDebugger;
            this.window.debuggerControlToolbar.DebuggerPreStartup += DebuggerPreStartup;
            this.window.debugMenu.DebuggerPreStartup += DebuggerPreStartup;
            this.window.debugMenu.Debugger = scriptDebugger;
        }

        private void DebuggerPreStartup(object sender, EventArgs e)
        {
            scriptRun.AssemblyKind = ScriptAssemblyKind.WindowsApplication;
            RemoteAPI.UseRemoteAPI = true;

            CompileScript();
        }

        private string GetSourceParametersForCSharp()
        {
            return "DebugRemoteScriptWpf.cs";
        }

        private string GetSourceParametersForVisualBasic()
        {
            return "DebugRemoteScriptWpf.vb";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource()
        {
            ScriptLanguage language;
            switch (lang)
            {
                case "C#":
                    language = ScriptLanguage.CSharp;
                    break;
                default:
                    language = ScriptLanguage.VisualBasic;
                    break;
            }

            var sourceFileFullPath = GetScriptFilePath();
            LoadFile(edit, sourceFileFullPath);
            CodeEditExtensions.RegisterAssembly(edit, System.Reflection.Assembly.GetEntryAssembly().Location);
            scriptRun.ScriptSource.FromScriptCode(edit.Text, sourceFileFullPath);
            scriptRun.ScriptLanguage = language;
        }

        private void UpdateUseDirectAPI()
        {
            RemoteAPI.UseRemoteAPI = !useDirectAPI;
            scriptRun.AssemblyKind = RemoteAPI.UseRemoteAPI ? ScriptAssemblyKind.WindowsApplication : ScriptAssemblyKind.DynamicLibrary;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
            edit.RegisterAssemblies(new string[] { "WindowsBase", "PresentationCore", "PresentationFramework" });
        }

        private void CreateEditorAndDebugger()
        {
            edit = CreateEditorAndDebugger(GetScriptFilePath(), window.EditBorder);
        }

        private IScriptEdit CreateEditorAndDebugger(string fileName, Border border)
        {
            var thisAssemblyLocation = GetType().Assembly.Location;

            RemoteAPI.ScriptAPI = this.scriptAPI;

            RemoteAPI.StartServer(ipcPortName, ipcObjectUri);

            var scriptSource = scriptRun.ScriptSource;

            scriptSource.WithDefaultReferences();
            scriptSource.References.Add("System.Runtime.Remoting");

            scriptSource.References.Add(thisAssemblyLocation);
            scriptRun.AssemblyKind = ScriptAssemblyKind.WindowsApplication;

            string generatedModulesPath = Path.GetDirectoryName(thisAssemblyLocation);

            scriptDebugger = new ScriptDebugger
            {
                GeneratedModulesPath = generatedModulesPath,
                EventsSyncAction = action => window.Dispatcher.BeginInvoke(action),
                ScriptRun = scriptRun,
            };

            scriptDebugger.ExecutionStopped += ScriptDebugger_ExecutionStopped;
            scriptDebugger.DebuggerErrorOccured += ScriptDebugger_DebuggerErrorOccured;
            scriptDebugger.StateChanged += ScriptDebugger_StateChanged;

            scriptRun.ScriptHost.ModulesDirectoryPath = generatedModulesPath;
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;

            IScriptEdit edit;
            edit = new DebugCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;
            ((IDebugEdit)edit).Debugger = scriptDebugger;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);

            return edit;
        }

        private string ExtractCommandLineArg(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        private void ParseCommandLineArgs(string[] args)
        {
            foreach (string arg in args)
            {
                string larg = arg.ToLower();
                if (larg.StartsWith("-ipcportname="))
                    ipcPortName = ExtractCommandLineArg(arg);
                else
                if (larg.StartsWith("-ipcobjecturi="))
                    ipcObjectUri = ExtractCommandLineArg(arg);
            }
        }

        private void RunScriptClick()
        {
            UpdateUseDirectAPI();

            if (CompileScript())
                StartScript();
        }

        private void StartDebugClick()
        {
            if (CompileScript())
            {
                scriptDebugger.StartDebugging(
                    new StartDebuggingOptions
                    {
                        BreakOnStart = true,
                        Arguments = new string[] { string.Format("-ipcPortName={0}", ipcPortName ?? string.Empty), string.Format("-ipcObjectUri={0}", ipcObjectUri ?? string.Empty) },
                    });
            }
        }

        private async void StopDebugClick()
        {
            await scriptDebugger.StopDebuggingAsync();
        }

        private void StepOverClick()
        {
            scriptDebugger.StepOver();
        }

        private void ContinueClick()
        {
            scriptDebugger.Continue();
        }

        private IScriptEdit CreateEditorAndDebugger(string fileName, string generatedModulesPath)
        {
            scriptDebugger = new ScriptDebugger
            {
                GeneratedModulesPath = generatedModulesPath,
                EventsSyncAction = action => window.Dispatcher.BeginInvoke(action),
                ScriptRun = scriptRun,
            };

            scriptDebugger.ExecutionStopped += ScriptDebugger_ExecutionStopped;
            scriptDebugger.DebuggerErrorOccured += ScriptDebugger_DebuggerErrorOccured;
            scriptDebugger.StateChanged += ScriptDebugger_StateChanged;

            scriptRun.ScriptHost.ModulesDirectoryPath = generatedModulesPath;

            IScriptEdit edit;
            edit = new DebugCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;
            ((IDebugEdit)edit).Debugger = scriptDebugger;
            edit.RegisterAssembly("WindowsBase");
            edit.RegisterAssembly("PresentationCore");

            if ((edit is UIElement) && (window != null))
            {
                window.EditBorder.Child = (UIElement)edit;
            }

            FileInfo fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                edit.LoadFile(fileInfo.FullName);
            }

            return edit;
        }

        private void ScriptDebugger_DebuggerErrorOccured(object sender, DebuggerErrorOccuredEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }

        private void ScriptDebugger_StateChanged(object sender, DebuggerStateChangedEventArgs e)
        {
            UpdateDebuggerControls(e.NewState);
        }

        private void UpdateDebuggerControls(DebuggerState state)
        {
            switch (state)
            {
                case DebuggerState.Off:
                    ClearExecutionPosition();
                    break;
                case DebuggerState.Running:
                    ClearExecutionPosition();
                    break;
                default:
                    break;
            }
        }

        private void ClearExecutionPosition()
        {
            var debugEdit = edit as IDebugEdit;
            if (debugEdit == null)
                return;

            if (executionPosition != null)
            {
                debugEdit.ClearDebugStyles(executionPosition);
                executionPosition = null;
            }
        }

        private void ScriptDebugger_ExecutionStopped(object sender, ExecutionStoppedEventArgs e)
        {
            var debugEdit = edit as IDebugEdit;
            if (debugEdit == null)
                return;

            ClearExecutionPosition();

            executionPosition = e.Position;

            if (e.Position != null && !string.IsNullOrEmpty(e.Position.File))
            {
                if (Path.GetFullPath(e.Position.File) == Path.GetFullPath(GetScriptFilePath()))
                    debugEdit.ExecutionStopped(e.Position);
                else
                    scriptDebugger.Continue();
            }

            if (e.StopReason == ExecutionStopReason.Exception || e.StopReason == ExecutionStopReason.UnhandledException)
            {
                MessageBox.Show(e.Exception.ToString());
            }
        }

        private string GetScriptFilePath()
        {
            switch (lang)
            {
                case "C#":
                    return GetSourceFileFullPath(GetSourceParametersForCSharp());

                default:
                    return GetSourceFileFullPath(GetSourceParametersForVisualBasic());
            }
        }

        private bool CompileScript()
        {
            if (edit.Modified)
                scriptRun.ScriptSource.FromScriptCode(edit.Text, edit.FileName);

            if (!scriptRun.Compiled)
            {
                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N") + (scriptRun.AssemblyKind == ScriptAssemblyKind.WindowsApplication ? ".exe" : ".dll");

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
            if (!RemoteAPI.UseRemoteAPI)
                scriptRun.Run(new string[] { });
            else
                new Task(StartScriptProcess).Start();
        }

        private void StartScriptProcess()
        {
            scriptRun.RunProcess(new string[] { string.Format("-ipcPortName={0}", ipcPortName ?? string.Empty), string.Format("-ipcObjectUri={0}", ipcObjectUri ?? string.Empty) });
        }
    }
}
