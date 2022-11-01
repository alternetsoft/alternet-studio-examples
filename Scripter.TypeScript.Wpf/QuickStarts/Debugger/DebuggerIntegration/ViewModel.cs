#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.TypeScript;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Scripter.TypeScript;

using Microsoft.Win32;

namespace DebuggerIntegration.TypeScript
{
    public class ViewModel
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.TypeScript\TS\MultipleFiles\project.json";
        private static readonly string StartupFileSubPath = @"Resources\Debugger.TypeScript\TS\DebugMyScript\DebugMyScript.ts";
        private ScriptRun scriptRun = new ScriptRun();
        private ScriptDebugger debugger;
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private DebugCodeEditContainer codeEditContainer;

        public ViewModel()
        {
            OpenProjectCommand = new RelayCommand(OpenProjectClick);
            CloseProjectCommand = new RelayCommand(CloseProjectClick);
            OpenCommand = new RelayCommand(OpenClick);
            SaveCommand = new RelayCommand(SaveClick);
            CloseCommand = new RelayCommand(CloseClick);
            ExitCommand = new RelayCommand(ExitClick);

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

            codeEditContainer = new DebugCodeEditContainer(window.EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenFile(FindFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            this.window.DebuggerControlToolbar.Debugger = debugger;
            this.window.DebuggerPanelsTabControl.Debugger = debugger;
            this.window.DebugMenu.Debugger = debugger;
            this.window.DebugMenu.InstallKeyboardShortcuts(this.window.CommandBindings);

            this.window.DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;
            this.window.DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;
            this.window.FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;

            var controller = new DebuggerUIController(this.window.Dispatcher, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = this.window.DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";

            InitDefaultHostAssemblies();
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
                }
            }
        }

        public ICommand OpenProjectCommand { get; set; }

        public ICommand CloseProjectCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public ICommand RunScript { get; set; }

        public ICommand StartDebug { get; set; }

        public ICommand StopDebug { get; set; }

        public ICommand StepOver { get; set; }

        public ICommand Continue { get; set; }

        protected TSProject Project { get; private set; } = new TSProject();

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindFile() =>
          ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupFileSubPath))).FirstOrDefault(File.Exists);

        private void OnDebuggerPreStartup(object sender, System.EventArgs e)
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

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
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

            this.window.DebuggerPanelsTabControl.Errors.Clear();
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

        private void OpenFile(string fileName)
        {
            codeEditContainer.TryActivateEditor(fileName);
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.InitSyntax();
            LoadFile(edit, e.FileName);

            e.DebugEdit = edit;
            this.edit = edit;
        }

        private string ExtractCommandLineArg(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        private void RunScriptClick()
        {
            new Task(StartScript).Start();
        }

        private void StartDebugClick()
        {
            if (CompileScript())
                CatchAndDisplayExceptions(() => debugger.StartDebugging(new StartDebuggingOptions { BreakOnStart = false }));
        }

        private async void StopDebugClick()
        {
            await debugger.StopDebuggingAsync();
        }

        private void StepOverClick()
        {
            debugger.StepOver();
        }

        private void ContinueClick()
        {
            debugger.Continue();
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

        private void CatchAndDisplayExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                window.Dispatcher.BeginInvoke((Action)(() => MessageBox.Show(e.ToString())));
            }
        }

        private void OpenProjectClick()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Project files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(@"..\..\..\"),
            };

            if (dialog.ShowDialog(this.window) != true)
                return;

            OpenProject(dialog.FileName);
        }

        private void CloseProjectClick()
        {
            CloseProject(Project);
        }

        private void OpenClick()
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "Typescript files (*.ts)|*.ts|Javascript files (*.js)|*.js|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindProjectFile());
            if (dialog.ShowDialog().Value)
            {
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveClick()
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseClick()
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.CloseFile(codeEditContainer.ActiveEditor.FileName);

            if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun.ScriptSource?.Reset();
            }
        }

        private void ExitClick()
        {
            window.Close();
        }

        private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            window.CloseProjectMenuItem.IsEnabled = Project != null && Project.HasProject;
            window.CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
            window.SaveMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }

        public class MyHostObject
        {
            private TextBox textBox;

            public MyHostObject(TextBox textBox)
            {
                this.textBox = textBox;
            }

            public string Text
            {
                get
                {
                    var text = string.Empty;
                    textBox.Dispatcher.BeginInvoke((Action)(() => { text = textBox.Text; })).Wait();
                    return text;
                }
            }
        }
    }
}