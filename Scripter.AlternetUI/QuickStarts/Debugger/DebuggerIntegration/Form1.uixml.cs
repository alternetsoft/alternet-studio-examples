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

using Alternet.UI;
using Alternet.Common;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Integration.AlternetUI;
using Alternet.Scripter;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DebuggerIntegration
{
    public partial class Form1 : Window
    {
        private static bool ExceptionsLogger;

        private static readonly string StartupProjectFileSubPath
            = @"Debugger/CS/DebuggerTest/DebuggerTest.csproj";

        private readonly DebuggerIntegrationPanelCSharp debuggerPanel;

        static Form1()
        {
            ExceptionsLogger = DebugUtils.IsDebugDefinedAndAttached;

            if (CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                DebugUtils.ExceptionsLoggerDebugWriteLine = true;

                DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
                });
                ExceptionsLogger = false;
            }

            CoreClrLauncher.ProcessStarted += (process) =>
            {
                App.Log("Process started");
            };

            Process.GetCurrentProcess().Exited += (s, e) =>
            {
                Debug.WriteLine("GetCurrentProcess().Exited");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Debug.WriteLine($"Unobserved exception: {e.Exception}");
                e.SetObserved(); // Prevents application crash
            };
        }

        public Form1()
        {
            PathUtilities.UseAppSubFolderAsTempPath = true;

            DebuggerIntegrationPanelCSharp.UseOldDebugger
                = !CommandLineArgs.ParseAndHasArgument("-NewDebugger");

            debuggerPanel = new();

            State = WindowState.Maximized;

            InitializeComponent();

            debuggerPanel.Parent = this;

            ActiveControl = debuggerPanel.FirstEditor;

            var mainMenu = new MainMenu();
            mainMenu.Items.Add(debuggerPanel.FileMenu);
            mainMenu.Items.Add(debuggerPanel.DebugMenu);

            Menu = mainMenu;

            debuggerPanel.UpdateToolbar();

            var projectFile = Path.Combine(DemoUtils.ResourcesFolder, StartupProjectFileSubPath);
            debuggerPanel.OpenProject(projectFile);

            DebugUtils.DebugCall(() =>
            {
                debuggerPanel.DebuggerPanelsTabControl.AddDeveloperToolsToContextMenu();
                debuggerPanel.DebuggerPanelsTabControl.AddOutputContextMenuAction(
                    "Show Temp Folder",
                    () =>
                    {
                        PathUtilities.OpenFolderInFileExplorer(PathUtilities.GetTempPath());
                    });
            });

            DebugUtils.DebugCallIf(false, ()=> { PathUtilities.DeleteAllContentsInTempFolder(); });
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            debuggerPanel.StopDebugger();
            base.OnClosing(e);
        }
    }
}
