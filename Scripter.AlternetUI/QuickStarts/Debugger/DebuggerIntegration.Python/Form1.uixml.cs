#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.IO;

using Alternet.Scripter.Debugger.UI.AlternetUI;
using Alternet.UI;

using Alternet.Scripter.Debugger;
using System.Collections.Generic;
using System.Linq;

using Alternet.Common;
using Alternet.Common.Projects;
using Alternet.Common.Python;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Integration;
using Alternet.Scripter.Python;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Python;
using Alternet.Scripter.Integration.AlternetUI;
using System.Diagnostics;

namespace DebuggerIntegration.Python
{
    public partial class Form1 : Window
    {
        private static bool ExceptionsLogger = false;

        protected PythonProject Project { get; private set; } = new PythonProject();

        private static readonly string StartupProjectFileSubPath
            = @"Debugger.Python/DebuggerIntegration.AlternetUI/Project.pyproj";

        private readonly Alternet.Scripter.Debugger.Python.ScriptDebugger debugger;
        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;
        private readonly Alternet.UI.MenuItem testMenuItemToolStripMenuItem = new();

        private readonly DebuggerControlToolbar? toolbar = new()
        {
            Margin = (0, 0, 0, ToolBar.DefaultDistanceToContent),
            Padding = 1,
        };

        private readonly DebugMenu? debugMenu = new()
        {
        };

        private readonly TabControl EditorsTabControl = new()
        {
            VerticalAlignment = VerticalAlignment.Fill,
        };

        private readonly DebuggerPanelsTabControl debuggerPanelsTabControl = new()
        {
            VerticalAlignment = VerticalAlignment.Fill,
            TabAlignment = TabAlignment.Bottom,
        };

        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            BottomVisible = true,
            LeftVisible = false,
            RightVisible = false,
        };

        private DebuggerController? controller = new();

        static Form1()
        {
            KnownAssemblies.PreloadReferenced();
            Alternet.Scripter.Python.PythonDemoUtils.Initialize();

            if (CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                });
                ExceptionsLogger = false;
            }
        }

        public Form1()
        {
            // PythonDemoUtils.SetVirtualEnvironment(@"e:\py1");

            State = WindowState.Maximized;

            InitializeComponent();

            debugger = new Alternet.Scripter.Debugger.Python.ScriptDebugger();
            scriptRun = new ScriptRun();
            debugger.ScriptRun = scriptRun;

            controller.DebuggerPreStartup += OnDebuggerPreStartup;
            controller.Debugger = debugger;
            
            toolbar.Controller = controller;
            debugMenu.Controller = controller;

            testMenuItemToolStripMenuItem.Text = "Test Menu Item";
            toolbar.SetVisibleBorders(false, false, false, true);

            DebugCodeEdit.Parsers[".py"] = typeof(PythonNETParser);
            DebugCodeEdit.CreateParserFunc = DoCreateParser;

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            try
            {
                OpenProject(FindProjectFile());
            }
            catch(Exception e)
            {
                App.LogError(e);
                throw;        
            }

            scriptRun.GlobalItems.Add(
                new ScriptGlobalItem(
                    "TestMenuItem",
                    new MenuItemWrapper(this, testMenuItemToolStripMenuItem)));

            debuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
            debuggerPanelsTabControl.Debugger = debugger;

            var uiController = new DebuggerUIController(this, codeEditContainer)
            {
                Debugger = debugger,
                DebuggerPanels = debuggerPanelsTabControl,
            };

            controller.BeforeCommand += (s, e) =>
            {
                if (e.Command == DebuggerControllerCommandKind.Compile)
                {
                    debuggerPanelsTabControl.Errors?.Clear();
                }
            };

            controller.AfterCommand += (s, e) =>
            {
                if (e.Command == DebuggerControllerCommandKind.Compile)
                {
                }
            };

            codeEditContainer.Debugger = debugger;

            OpenProjectMenuItem.Click += OpenProjectMenuItem_Click;
            CloseProjectMenuItem.Click += CloseProjectMenuItem_Click;
            OpenMenuItem.Click += OpenMenuItem_Click;
            SaveMenuItem.Click += SaveMenuItem_Click;
            CloseMenuItem.Click += CloseMenuItem_Click;
            ExitMenuItem.Click += ExitMenuItem_Click;
            FileMenu.Opened += FileMenu_Opened;

            toolbar.Parent = this;
            MainMenu.Items.Add(debugMenu);
            panel.Margin = 10;
            panel.Parent = this;
            panel.BottomPanel.MinHeight = 200;
            panel.VerticalAlignment = VerticalAlignment.Fill;
            EditorsTabControl.Parent = panel.FillPanel;
            debuggerPanelsTabControl.Parent = panel.BottomPanel;

            UpdateToolbar();

            debugMenu.Opened += (s, e) =>
            {
                debugMenu.IsDebugEnabled = (Project != null && Project.HasProject)
                    || codeEditContainer.ActiveEditor != null;
            };

            Closing += (s, e) =>
            {
                StopDebugger();
                CloseProject(Project);
            };

            // PythonDemoUtils.InstallEmbeddedPythonToFolder(@"e:\Embedded");

            // PythonPathScheme.LogToFile(@"e:\result.txt");

            debuggerPanelsTabControl.AddDeveloperToolsToContextMenu();

            App.AddIdleTask(() =>
            {
            });
        }

        [Conditional("DEBUG")]
        public static void TestLogPythonRuntimeInfo()
        {
            PythonDemoUtils.LogRuntimeInfo();
        }

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref controller);
            base.DisposeManaged();
        }

        private static string? FindProjectFile()
        {
            return Path.Combine(DemoUtils.ResourcesFolder, StartupProjectFileSubPath);
        }

        private static string? FindDefaultProjectDirectory()
        {
            return Path.GetDirectoryName(FindProjectFile());
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
            if (projectFilePath is null)
                return;

            if (Project != null && Project.HasProject)
                CloseProject(Project);

            Project!.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);

            if (Project.Files.Count > 0)
            {
                codeEditContainer.TryActivateEditor(Project.Files[0]);
            }

            debuggerPanelsTabControl.Errors?.Clear();
            UpdateToolbar();
        }

        private void StopDebugger()
        {
            controller?.StopDebugAsync();
        }

        private void CloseProject(PythonProject project)
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

            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
            UpdateToolbar();
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private void EditorContainer_EditorRequested(object? sender, DebugEditRequestedEventArgs e)
        {
            if (!PathUtilities.IsPathValid(e.FileName) || !File.Exists(e.FileName))
                return;

            var parser = new PythonNETParser
            {
                CodeEnvironment = scriptRun.CodeEnvironment,
            };

            var edit = new DebugCodeEdit();
            edit.LoadFile(e.FileName);
            edit.Lexer = parser;
            edit.AllowedActions &= ~AllowedActions.SetNextStatement;

            e.DebugEdit = edit;
        }

        private void OpenProjectMenuItem_Click(object? sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Project",
                Filter = "Project files (*.pyproj)|*.pyproj|All files (*.*)|*.*",
                InitialDirectory = FindDefaultProjectDirectory(),
            };

            dialog.ShowAsync(() =>
            {
                if (dialog.FileName == null)
                    return;
                OpenProject(dialog.FileName);
            });
        }

        private void UpdateToolbar()
        {
            if (toolbar is null)
                return;

            toolbar.IsDebugEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
            toolbar.UpdateDebugButtonsEnabled();
        }

        private void CloseProjectMenuItem_Click(object? sender, EventArgs e)
        {
            CloseProject(Project);
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Python files (*.py)|*.py|Any files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Path.GetDirectoryName(FindProjectFile()),
            };

            dialog.ShowAsync(() =>
            {
                if (dialog.FileName == null)
                    return;

                codeEditContainer.TryActivateEditor(dialog.FileName);

                UpdateToolbar();
            });
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseMenuItem_Click(object? sender, EventArgs e)
        {
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
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            StopDebugger();
            Close();
        }

        private void FileMenu_Opened(object? sender, EventArgs e)
        {
            CloseProjectMenuItem.Enabled = Project != null && Project.HasProject;
            CloseMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
            SaveMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
        }

        private ILexer? DoCreateParser(Type type)
        {
            return Activator.CreateInstance(type) as ILexer;
        }
    }
}
