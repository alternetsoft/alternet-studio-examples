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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Alternet.Common.Python;
using Alternet.Common.Wpf;
using Alternet.Editor.Python.Wpf;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;
using Microsoft.Win32;

namespace DebuggerIntegration.Python.Wpf
{
    public partial class MainWindow : Window
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger.Python\DebuggerIntegration.Wpf\Project.pyproj";
        private ScriptDebugger debugger;

        private DebugCodeEditContainer codeEditContainer;

        private ScriptRun scriptRun;
        private PythonNETParser pythonParser;

        public MainWindow()
        {
            SetupPython();

            InitializeComponent();
            pythonParser = new PythonNETParser();
            scriptRun = new ScriptRun();

            pythonParser.CodeEnvironment = scriptRun.CodeEnvironment;

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenProject(FindProjectFile());

            scriptRun.GlobalItems.Add(new ScriptGlobalItem("TestMenuItem", TestMenuItem));

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(Dispatcher, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            DebugMenu.InstallKeyboardShortcuts(CommandBindings);
            FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;
        }

        protected PythonProject Project { get; private set; } = new PythonProject();

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"AlterNETStudio\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython();
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void OnDebuggerPreStartup(object sender, System.EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
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

        private void CloseProject(PythonProject project)
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

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.LoadFile(e.FileName);
            edit.Lexer = pythonParser;

            edit.SetNextStatement += DebugEdit_SetNextStatement;

            e.DebugEdit = edit;
        }

        private async void DebugEdit_SetNextStatement(object sender, EventArgs e)
        {
            if (codeEditContainer.ActiveEditor == null)
            {
                return;
            }

            var line = codeEditContainer.ActiveEditor.CurrentLine + 1;
            var result = await codeEditContainer.Debugger.TrySetNextStatementAsync(line);
            if (result.IsFailure)
                MessageBox.Show("Cannot set next statement to line: " + line + ". " + result.ErrorMessage);
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

            dialog.Filter = "Python files (*.py)|*.py|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindProjectFile());
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

        private void RunMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveAllModifiedFiles();
            scriptRun.Run();
        }

        private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            CloseProjectMenuItem.IsEnabled = Project != null && Project.HasProject;
            CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
            SaveMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }
    }
}