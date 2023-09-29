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
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.TypeScript;
using Alternet.Scripter.Debugger.UI.Wpf;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Scripter.TypeScript;
using Microsoft.Win32;

namespace DebuggerUIThread.TypeScript.Wpf
{
    public partial class MainWindow : Window
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupFileSubPath = @"Resources\Debugger.TypeScript\TS\DebuggerUIThread\MainWPF.ts";
        private ScriptDebugger debugger;
        private DisplayForm displayForm;

        private DebugCodeEditContainer codeEditContainer;

        private ScriptRun scriptRun;

        public MainWindow()
        {
            InitializeComponent();
            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenFile(FindFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.CommandsListener = new DebuggerUICommands(this, debugger);
            DebuggerControlToolbar.CommandsListener = new DebuggerUICommands(this, debugger);

            DebuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
            DebuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(Dispatcher, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            DebugMenu.InstallKeyboardShortcuts(CommandBindings);
            FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;
            this.Closing += MainWindow_Closing;
            this.Loaded += MainWindow_Loaded;
            InitDefaultHostAssemblies();
            AddBreakpoint();
        }

        protected TSProject Project { get; private set; } = new TSProject();

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        protected void StartDisplayFormThread(DisplayForm.Command startCommand, Point pos)
        {
            var thread = new Thread(() =>
            {
                displayForm = new DisplayForm(DebuggerControlToolbar.Debugger, startCommand, pos);
                displayForm.Closed += (sender1, e1) => displayForm.Dispatcher.InvokeShutdown();
                displayForm.Show();
                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private static string FindFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupFileSubPath))).FirstOrDefault(File.Exists);

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartDisplayFormThread(DisplayForm.Command.None, new Point(this.Left + this.Width, this.Top));
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnDebuggerPreStartup(object sender, System.EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
            if (displayForm == null)
                StartDisplayFormThread(DisplayForm.Command.Debug, new Point(this.Left + this.Width, this.Top));
        }

        private void AddBreakpoint()
        {
            int firstLine = 4;
            var edit = codeEditContainer.ActiveEditor as DebugCodeEdit;
            if (edit != null && edit.Lines.Count > firstLine)
            {
                debugger.Breakpoints.AddBreakpoint(edit.Source.FileName, firstLine);
                edit.UpdateBreakpoints();
            }
        }

        private void OpenFile(string fileName)
        {
            scriptRun.ScriptSource.FromScriptFile(fileName);
            codeEditContainer.TryActivateEditor(fileName);
        }

        private void OpenProject(string projectFilePath)
        {
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            Project.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);

            if (Project.Files.Count > 0)
            {
                codeEditContainer.TryActivateEditor(Project.Files[0]);
            }

            DebuggerPanelsTabControl.Errors.Clear();
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(technology: TechnologyEnvironment.Wpf, options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
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

        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Project files (*.pyproj)|*.pyproj|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(@"..\..\..\"),
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

            dialog.Filter = "TypeScript files (*.py)|*.py|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindFile());
            if (dialog.ShowDialog().Value)
            {
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.CloseFile(codeEditContainer.ActiveEditor.FileName);
            if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun.ScriptSource?.Reset();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            CloseProjectMenuItem.IsEnabled = Project != null && Project.HasProject;
            CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
            SaveMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }

        private void RunMenu_Click(object sender, RoutedEventArgs e)
        {
            SaveAllModifiedFiles();
            if (displayForm == null)
                StartDisplayFormThread(DisplayForm.Command.Run, new Point(this.Left + this.Width, this.Top));
            else
                displayForm.InvokeRunScript();
        }

        public class DebuggerUICommands : DefaultDebuggerUICommands
        {
            private MainWindow owner;

            public DebuggerUICommands(MainWindow owner, IScriptDebuggerBase debugger)
                : base(debugger)
            {
                this.owner = owner;
            }

            public override bool Start()
            {
                if (Debugger.IsStarted)
                    return base.Start();

                return true;
            }

            public override bool StepInto()
            {
                if (Debugger.IsStarted)
                    return base.StepInto();
                return true;
            }

            public override bool StepOver()
            {
                if (Debugger.IsStarted)
                    return base.StepOver();

                return true;
            }

            public override bool PreStartup()
            {
                if (owner.displayForm != null)
                    owner.displayForm.InvokeStartDebug();

                return true;
            }
        }
    }
}