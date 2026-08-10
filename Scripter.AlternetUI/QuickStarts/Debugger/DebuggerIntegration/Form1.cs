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
using Alternet.Drawing;

namespace DebuggerIntegration
{
    public partial class Form1 : Window
    {
        internal static bool logFont = false;

        private static readonly bool ExceptionsLogger;

        private static readonly string StartupProjectFileSubPath
            = @"Debugger/CS/DebuggerTest/DebuggerTest.csproj";

        private readonly DebuggerIntegrationPanelCSharp debuggerPanel;

        static Form1()
        {
            Alternet.Editor.AlternetUI.GlobalEditorInitializer.Bind();

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
            Title = "DebuggerIntegration on Alternet.UI";
            StartLocation = WindowStartLocation.CenterScreen;
            MinimumSize = (600, 400);
            Icon = (Alternet.Drawing.IconSet?)"embres:DebuggerIntegration.Sample.ico";

            PathUtilities.UseAppSubFolderAsTempPath = true;

            DebuggerIntegrationPanelCSharp.UseOldDebugger
                = !CommandLineArgs.ParseAndHasArgument("-NewDebugger");

            debuggerPanel = new();

            State = WindowState.Maximized;
            Size = (900, 700);

            InitColors();
            debuggerPanel.Parent = this;

            var mainMenu = new MainMenu();
            mainMenu.Items.Add(debuggerPanel.FileMenu);
            mainMenu.Items.Add(debuggerPanel.DebugMenu);

            Menu = mainMenu;

            debuggerPanel.UpdateToolbar();

            var projectFile = DemoUtils.GetResourceFileFullPath(StartupProjectFileSubPath);

            DebugUtils.DebugCall(() =>
            {
                debuggerPanel.DebuggerPanelsTabControl.AddDebugToolsToContextMenu();
            });

            DebugUtils.DebugCallIf(false, ()=> { PathUtilities.DeleteAllContentsInTempFolder(); });

            FormUtils.BindShown(this, () =>
            {
                debuggerPanel.OpenProject(projectFile);

                ActiveControl = debuggerPanel.FirstEditor;

                if (ActiveControl is Alternet.Editor.AlternetUI.SyntaxEdit syntaxEdit)
                {
                    if (logFont)
                    {
                        var font = syntaxEdit.RealFont;
                        App.LogIf($"ActiveControl.RealFont: {font}, Monospaced: {font.IsFixedWidth}", true);
                    }
                }
            });

            var showTestActions = false && Alternet.UI.DebugUtils.IsDebugOnWindows;

            if (showTestActions)
            {
                var panel = debuggerPanel.DebuggerPanelsTabControl.FindResults
                    as Alternet.Scripter.Debugger.UI.AlternetUI.FindResultsPanel;
                
                if(panel is not null)
                {
                    var menu = panel.MainContextMenu;

                    menu?.Add("Add Test Items", () =>
                    {
                        Alternet.Scripter.Debugger.UI.AlternetUI.ScriptDebuggerUtils.FindResultsPanelAddBadItems(panel);
                        Alternet.Scripter.Integration.AlternetUI.DebugCodeEditUtils.FindResultsPanelAddItems(
                            panel,
                            debuggerPanel.CodeEditContainer);
                    });
                }
            }
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            debuggerPanel.StopDebugger();
            base.OnClosing(e);
        }

        protected virtual void InitColors()
        {
            Color toolBarBackColorDark = (38, 38, 38);
            Color toolBarBackColorLight = (247, 247, 247);

            var toolBarBackColor = SystemSettings.AppearanceIsDark ? toolBarBackColorDark : toolBarBackColorLight;

            if (SystemSettings.AppearanceIsDark)
            {
                BackColor = (28, 28, 28);
            }
            else
            {
                BackColor = (238, 238, 238);
            }

            debuggerPanel.DebuggerPanelsTabControl?.SetToolBarBackColor(toolBarBackColor);
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            InitColors();
        }
    }
}
