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

namespace DebuggerIntegration
{
    public partial class Form1 : Window
    {
        private static bool ExceptionsLogger = false;

        private static readonly string StartupProjectFileSubPath
            = @"Debugger/CS/DebuggerTest/DebuggerTest.csproj";

        private readonly DebuggerIntegrationPanelCSharp debuggerPanel;

        static Form1()
        {
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
            PathUtilities.UseAppSubFolderAsTempPath = true;
            DebuggerIntegrationPanelCSharp.UseOldDebugger = !CommandLineArgs.ParseAndHasArgument("-NewDebugger");
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
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            debuggerPanel.StopDebugger();
            base.OnClosing(e);
        }
    }
}
