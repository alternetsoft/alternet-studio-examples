#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml;

using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Common.Projects.DotNet;

using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Maui;
using Alternet.Maui.Extensions;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.MAUI;
using Alternet.Scripter.Integration.MAUI;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Storage;

#if WINDOWS
#endif

namespace DebuggerIntegration
{
    public partial class MainPage
        : Alternet.UI.DisposableContentPage, Alternet.UI.IProcessRunnerNotification,
        Alternet.UI.IRaiseSystemColorsChanged
    {
        public const string LoremIpsumSmall =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        public static double DefaultDebuggerPanelHeight { get; set; } = 200;

        public static bool? IgnoreHoveredState { get; set; }

        // If set to true, the code editor will be initialized in a way suitable
        // for mobile devices when the debugger is attached.
        // This can be useful for testing the mobile experience on desktop during development.
        public static bool InitForMobileIfDebug { get; set; } = false;

        private static readonly bool ExceptionsLogger = true;

        private static readonly bool DefaultWordWrap;

        private static readonly string ProjectFolder
            = "embres:DebuggerIntegration.Content.DebuggerTest";

        private static readonly string[] VBExtensions = [".vb"];
        private static readonly string[] CSExtensions = [".cs"];
        private static readonly string[] VBProjectExtensions = [".vbproj"];
        private static readonly string[] CSProjectExtensions = [".csproj"];

        private readonly DebuggerPanelsTabControlView debuggerPanelsTabControl = new();
        private readonly SimpleTabControlView editorsTabControl = new();
        private readonly Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu debugMenu;

        private readonly Alternet.Scripter.Debugger.UI.AlternetUI.DebuggerController
            debugController = new();

        private readonly DebuggerControlToolBarView debuggerControlToolBar = new();
        private readonly Alternet.UI.ContextMenu mainMenu = new();

        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;

        private readonly IScriptDebuggerBase debugger;
        private readonly MenuBarItem? fileMenuBar;
        private readonly MenuBarItem? debugMenuBar;

        private DotNetProject project = new();

        static MainPage()
        {
            Alternet.Editor.Maui.SyntaxEditView.UseDefaultSearchDialog = false;

            TestComplexToolTips();

#if MACCATALYST
            Alternet.Editor.AlternetUI.EditConsts.DefaultFontSizeIncrement = 1;
#endif

            if (IgnoreHoveredState.HasValue)
                Alternet.UI.PlessMouse.IgnoreHoveredState = IgnoreHoveredState.Value;

            Alternet.UI.MauiUtils.SuppressMenuBarFocus();

            DefaultWordWrap = !Consts.IsWindows;

            CoreClrLauncher.RunProcessFunc = Alternet.UI.ProcessRunnerWithNotification.RunProcess;
            CoreClrLauncher.NetCoreAppConfigOnWindows = true;

            if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
                ExceptionsLogger = true;

            if (ExceptionsLogger)
            {
                Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
                {
#if ANDROID
                    if (e is Java.Lang.IllegalArgumentException)
                    {
                        return;
                    }
#endif

                    if (e is XmlException)
                    {
                        return;
                    }

                    if (e is FileNotFoundException)
                    {
                        return;
                    }

                    if (e is ReflectionTypeLoadException)
                    {
                        return;
                    }

                    if (e is TargetInvocationException)
                    {
                        if (e.InnerException is ReflectionTypeLoadException)
                        {
                            return;
                        }
                    }

                    if (e is OperationCanceledException)
                    {
                        return;
                    }

                    if (e is System.Runtime.InteropServices.COMException)
                        return;

                    Nop();

                    Debug.WriteLine($"Exception: {e}");
                });
                ExceptionsLogger = false;
            }
        }

        public MainPage()
        {
            Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu.ImageSize
                = (int)(Alternet.UI.Display.MaxScaleFactor * 16);

            debugMenu = new Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu();

            var canOpen = Alternet.UI.App.IsWindowsOS;

            OpenProjectCommand = new Alternet.UI.Command(
                execute: () =>
                {
                    OpenProjectDialog();
                },
                canExecute: () =>
                {
                    return canOpen;
                });

            OpenCommand = new Alternet.UI.Command(
                execute: () =>
                {
                    OpenFileDialog();
                },
                canExecute: () =>
                {
                    return canOpen;
                });

            CloseProjectCommand = new Alternet.UI.Command(
                execute: () =>
                {
                    CloseProject(Project);
                    GetEditorOrEmptyPanel();
                },
                canExecute: () =>
                {
                    var canExecute = Project != null && Project.HasProject && canOpen;
                    return canExecute;
                });

            CloseCommand = new Alternet.UI.Command(
                execute: () =>
                {
                    if (codeEditContainer is null || scriptRun is null)
                        return;
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
                    GetEditorOrEmptyPanel();
                },
                canExecute: () =>
                {
                    return codeEditContainer?.ActiveEditor != null && canOpen;
                });

            SaveCommand = new Alternet.UI.Command(
              execute: () =>
              {
                  if (codeEditContainer is null)
                      return;
                  var edit = codeEditContainer.ActiveEditor;
                  edit?.SaveFile(edit.FileName);
              },
              canExecute: () =>
              {
                  return codeEditContainer?.ActiveEditor != null;
              });

            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

            Loaded += (_, __) =>
            {
#if WINDOWS
                var xamlWindow = App.Current?.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window;
#endif
            };


#if ANDROID || IOS || MACCATALYST
            Shell.SetNavBarIsVisible(this, false);
#endif

            var tabFontSize = Alternet.Editor.AlternetUI.EditConsts.DefaultFont.Size * 1.333;
            editorsTabControl.SetTabFont(null, tabFontSize);
            debuggerPanelsTabControl.SetTabFont(null, tabFontSize);

            var fileMenu = new Alternet.UI.MenuItem
            {
                Text = "File",
            };

            // Creating menu items
            var openProjectMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Open Project...",
                AutomationId = "OpenProjectMenuItem",
            };

            var closeProjectMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Close Project",
                AutomationId = "CloseProjectMenuItem",
            };

            var openMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Open...",
                AutomationId = "OpenMenuItem",
            };

            var saveMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Save",
                AutomationId = "SaveMenuItem",
            };

            var closeMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Close File",
                AutomationId = "CloseMenuItem",
            };

            var exitMenuItem = new Alternet.UI.MenuItem
            {
                Text = "Exit",
                AutomationId = "ExitMenuItem",
            };

            // Adding items to the menu
            fileMenu.Add(openProjectMenuItem);
            fileMenu.Add(closeProjectMenuItem);
            fileMenu.AddSeparator();
            fileMenu.Add(openMenuItem);
            fileMenu.Add(saveMenuItem);
            fileMenu.Add(closeMenuItem);
            fileMenu.AddSeparator();
            fileMenu.Add(exitMenuItem);

            bool addSampleItems = Alternet.UI.DebugUtils.IsDebugOnWindows && false;

            if (addSampleItems)
            {
                var sampleSubMenu = fileMenu.Add("Sample SubMenu");
                sampleSubMenu.Add("Item 1");
                sampleSubMenu.Add("Item 2");
                sampleSubMenu.Add("Item 3");

                var sampleSubMenu2 = fileMenu.Add("Sample SubMenu 2");
                sampleSubMenu2.Add("Item 1 1");
                sampleSubMenu2.Add("Item 2 1");
                sampleSubMenu2.Add("Item 3 1");
                sampleSubMenu2.Add("Item 3 4").Add("AAAAAAAAAAAAAAA").Add("BBB");
            }

            mainMenu.ItemsTitle = "Application Menu";
            fileMenu.ItemsTitle = "File";

            mainMenu.Add(fileMenu);

            Alternet.UI.MauiUtils.FillAbsoluteLayout(MainGrid);

            MainGrid.Insert(0, debuggerControlToolBar);
            debuggerControlToolBar.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsTopBorderVisible = true;

            SimpleToolBarView.StickyButtonStyle tabStyle;

            tabStyle = SimpleToolBarView.StickyButtonStyle.Border;

            debuggerPanelsTabControl.Header.StickyStyle = tabStyle;
            editorsTabControl.Header.StickyStyle = tabStyle;

            SimpleToolBarView.AddNextAndPreviousTabButtonsFlags options =
                SimpleToolBarView.AddNextAndPreviousTabButtonsFlags.MakeSticky;

            debuggerPanelsTabControl.Header.AddNextAndPreviousTabButtons(options);
            editorsTabControl.Header.AddNextAndPreviousTabButtons(options);
            debuggerPanelsTabControl.MakeSelectedTabFirst = true;
            editorsTabControl.MakeSelectedTabFirst = true;

            if (Alternet.UI.App.IsTabletOrPhoneDevice)
            {
            }

            panel.Add(editorsTabControl);

            editorsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsBottomBorderVisible = true;

            panel.Add(debuggerPanelsTabControl, 0, 1);
            debuggerPanelsTabControl.MinimumHeightRequest = DefaultDebuggerPanelHeight;
            debuggerPanelsTabControl.MaximumHeightRequest = DefaultDebuggerPanelHeight;

            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".cs"] = typeof(CsParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".vb"] = typeof(VbParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.CreateParserFunc = DoCreateParser;
            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += OnEditorRequested;

            string[] projectFiles =
                [
                    "DebuggerTest.cs",
                    "DebuggerTest.csproj",
                    "DebuggerTest_Second.cs",
                ];

            var destFolder = PathUtilities.GetTempPathUniquePerApp();

            var extractionResult = Alternet.UI.ResourceLoader.ExtractResourcesSafe(
                ProjectFolder,
                projectFiles,
                destFolder);

            bool openDefaultProject = true;

            if (openDefaultProject)
            {
                OpenProject(destFolder, "DebuggerTest.csproj");
            }

            var d = new Alternet.Scripter.Debugger.Universal.ScriptDebugger();
            debugger = d;
            debugger.EventsSyncAction = (action) => Alternet.UI.App.Invoke((Action)action);
            d.ScriptRun = scriptRun;

            debuggerControlToolBar.Debugger = debugger;
            debuggerControlToolBar.DebuggerPreStartup += OnDebuggerPreStartup;

            debugController.Debugger = debugger;
            debugController.DebuggerPreStartup += OnDebuggerPreStartup;
            debugController.DebuggerStateChanged += OnDebuggerStateChanged;
            debugMenu.Controller = debugController;
            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new Alternet.Scripter.Integration.AlternetUI
                .DebuggerUIController(this, codeEditContainer)
            {
                Debugger = debugger,
                DebuggerPanels = debuggerPanelsTabControl,
            };

            codeEditContainer.Debugger = debugger;

            debugMenu.ItemsTitle = "Debug";
            mainMenu.Add(debugMenu);

            if (Alternet.UI.App.IsWindowsOS)
            {
                openMenuItem.Command = OpenCommand;
                openProjectMenuItem.Command = OpenProjectCommand;
            }

            closeProjectMenuItem.Command = CloseProjectCommand;
            closeMenuItem.Command = CloseCommand;
            saveMenuItem.Command = SaveCommand;
            exitMenuItem.Command = ExitCommand;

            UpdateToolbar();
            UpdateCommands();

            Alternet.UI.App.LogMessage += (s, e) =>
            {
            };

            Alternet.UI.ProcessRunnerWithNotification.Bind(this);

            var window = Alternet.UI.MauiUtils.FirstWindow;
            if (window is not null)
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
                Alternet.UI.MauiUtils.HideContextMenus(editorsTabControl);
                codeEditContainer.ActiveEditor?.SetFocusIfPossible();
            };

            RaiseSystemColorsChanged();

            void CreateMenuButton()
            {
                var button = debuggerControlToolBar.InsertButton(
                    0,
                    text: null,
                    toolTip: "Show Application Menu",
                    image: Alternet.UI.KnownSvgImages.ImgBars);
                button.ClickedAction = () =>
                {
                    var uiControl = GetEditorOrEmptyPanelView();
                    Alternet.UI.HVDropDownAlignment alignment;
                    alignment = new((float)0, (float)-editorsTabControl.Header.Height);

                    Alternet.UI.MauiUtils.ContextMenuDisplayOptions options = new()
                    {
                    };

                    Alternet.UI.MauiUtils.ShowContextMenu(mainMenu, uiControl, alignment, options);
                };
            }

            if (Alternet.UI.App.IsWindowsOS)
            {
                fileMenuBar = fileMenu.ToMenuBarItem();
                debugMenuBar = debugMenu.ToMenuBarItem();

                MenuBarItems.Add(fileMenuBar);
                MenuBarItems.Add(debugMenuBar);

                if (Alternet.UI.App.IsDebuggerAttached)
                {
                    CreateMenuButton();
                }
            }
            else
            {
                CreateMenuButton();
            }

            debuggerPanelsTabControl.SelectedTabClickedAgain += (s, e) =>
            {
                debuggerPanelsTabControl.ToggleContentVisibility(DefaultDebuggerPanelHeight);
            };

            var showTestActions = true && Alternet.UI.DebugUtils.IsDebugOnWindows;

            if (showTestActions)
            {
                var menu = debuggerPanelsTabControl.FindResultsView.MainContextMenu;

                menu?.Add("Add Test Items", () =>
                {
                    var panel = debuggerPanelsTabControl.FindResultsView.MainPanel;
                    Alternet.Scripter.Debugger.UI.AlternetUI.ScriptDebuggerUtils.FindResultsPanelAddBadItems(panel);
                    Alternet.Scripter.Integration.AlternetUI.DebugCodeEditUtils
                        .FindResultsPanelAddItems(panel, codeEditContainer);
                });
            }

            var notification = new Alternet.UI.ControlSubscriber();

            notification.BeforeControlKeyDown += (s, e) =>
            {
                if (e.Key == Alternet.UI.Key.Escape && !e.HasModifiers)
                {
                    var uiControl = GetEditorOrEmptyPanelView();
                    var popupWasClosed = Alternet.UI.MauiUtils.HideContextMenus(uiControl);
                    e.Handled = popupWasClosed;
                }
            };

            Alternet.UI.AbstractControl.AddGlobalNotification(notification);
        }

        private void OnDebuggerStateChanged(object? sender, DebuggerStateChangedEventArgs e)
        {
            if (e.NewState == DebuggerState.Stopped)
            {
                Alternet.UI.BaseObject.Post(() =>
                {
                    Alternet.UI.MauiUtils.BrintToFront(this.Window);
                });
            }
        }

        public Alternet.UI.ICommand OpenProjectCommand { get; set; }

        public Alternet.UI.ICommand CloseProjectCommand { get; set; }

        public Alternet.UI.ICommand OpenCommand { get; set; }

        public Alternet.UI.ICommand CloseCommand { get; set; }

        public Alternet.UI.ICommand SaveCommand { get; set; }

        public Alternet.UI.ICommand ExitCommand { get; set; }

        Alternet.UI.ObjectUniqueId Alternet.UI.IProcessRunnerNotification.UniqueId { get; } = new();

        protected DotNetProject Project
        {
            get => project;
            private set => project = value;
        }

        public static void Nop()
        {
        }

        public static string GetFirstFile(IList<string> files, string langExt)
        {
            string result = files.Count > 0 ? files[0] : string.Empty;

            foreach (string file in files)
            {
                if (file.Contains("program.cs", StringComparison.CurrentCultureIgnoreCase))
                    return file;
                if (file.Contains("main", StringComparison.CurrentCultureIgnoreCase) && file.EndsWith(langExt))
                    return file;
            }

            return result;
        }

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessLog(
            Process process,
            string data,
            Alternet.UI.LogItemKind kind)
        {
            debuggerPanelsTabControl.Output?.WriteLineAndWait(data);
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

        public void RaiseSystemColorsChanged()
        {
            var headerColor = SimpleTabControlView.AltHeaderBackColor;
            editorsTabControl.Header.BackgroundColor = headerColor;
            debuggerPanelsTabControl.Header.BackgroundColor = headerColor;
        }

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        protected async Task TrySetFocusToEditor()
        {
            var editor = codeEditContainer.FirstOrDefault();
            if (editor is null)
                return;
            await editor.TrySetFocusWithTimeout();
        }

        protected override void OnAppearingOnce()
        {
            try
            {
                Alternet.UI.App.Log("Application is ready...");
            }
            finally
            {
                editorsTabControl.IsVisible = true;
            }

            var visibilityService = Alternet.UI.MauiUtils.BindToKeyboardVisibility((e) =>
            {
                debuggerPanelsTabControl.SetContentVisibility(!e.IsVisible, DefaultDebuggerPanelHeight);

                if (e.IsVisible)
                {
                }
                else
                {
                }
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await TrySetFocusToEditor();
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Window" || propertyName == "Parent")
            {
            }
        }

        protected View? GetEditorOrEmptyPanelView()
        {
            View? uiControl = codeEditContainer.ActiveEditorView;

            uiControl ??= editorsTabControl.EmptyPanel;

            return uiControl;
        }

        protected override void DisposeResources()
        {
            Alternet.UI.ProcessRunnerWithNotification.Unbind(this);
            base.DisposeResources();
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

        private void OnEditorRequested(
            object? sender,
            Alternet.Scripter.Integration.AlternetUI.DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEditView
            {
                WordWrap = DefaultWordWrap
            };

            edit.Editor.ContextMenuStrip = edit.Editor.DefaultMenu;

            edit.Editor.CodeCompletionBoxCreated += (s, e) =>
            {
                if (edit.Editor.CodeCompletionBox
                is not Alternet.Editor.AlternetUI.ICategorizedCodeCompletionBox codeCompletion)
                    return;
                var menu = new Alternet.UI.ContextMenuStrip();
                codeCompletion.CategoriesContextMenu = menu;
                codeCompletion.ItemsContextMenu = menu;
                menu.Add("Clear Categories Filter", () =>
                {
                    codeCompletion.CategoriesIncludedInFilter = [];
                });
                var itemOptions = menu.Add("Additional Options");

                itemOptions.Add("Set Option 1", () =>
                {
                    Alternet.UI.App.Log("Set Option 1 Clicked");
                });

                itemOptions.Add("Set Option 2", () =>
                {
                    Alternet.UI.App.Log("Set Option 2 Clicked");
                });

                var itemActions = menu.Add("Additional Actions");

                itemActions.Add("Additional Action 1", () =>
                {
                    Alternet.UI.App.Log("Additional Action 1 Clicked");
                });

                itemActions.Add("Additional Action 2", () =>
                {
                    Alternet.UI.App.Log("Additional Action 2 Clicked");
                });

                menu.AddSeparator();
                menu.Add("Close", () =>
                {
                    edit.Editor.CodeCompletionBox.Close(false);
                });
            };

            edit.SetBorderWidth(0, 1, 0, 1);

            if (Alternet.UI.DebugUtils.IsDebugDefinedAndAttached && InitForMobileIfDebug)
            {
                Alternet.Editor.AlternetUI.EditorOnMobileHelper.IsMobileDeviceOverride = () => true;
            }

            Alternet.Editor.AlternetUI.EditorOnMobileHelper.InitializeOnMobile(edit.Editor);

            var debugCodeEdit = edit.Editor as Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit;

            var projectName = GetProjectName(e.FileName);
            SetFileNameAndProject(debugCodeEdit, e.FileName, projectName);

            edit.Editor.LoadFileOrShowErrorInText(e.FileName);
            e.DebugEdit = edit.Editor;
            UpdateCommands();
        }

        public virtual string? GetProjectName(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return Project.ProjectName;
            }

            return null;
        }

        /// <summary>
        /// Updates name of the file and project and reparses editor's text.
        /// </summary>
        /// <param name="edit">IScriptEdit that contains source code to process.</param>
        /// <param name="fileName">New file name.</param>
        /// <param name="projectName">Optional project name.</param>
        public static void SetFileNameAndProject(
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit edit,
            string fileName,
            string? projectName)
        {
            edit.FileName = fileName;
            var parser = edit.Lexer as RoslynParser;

            if (parser != null)
            {
                parser.ProjectName = projectName;

                var solution = parser.Repository.Solution;
                if (solution != null)
                {
                    switch (Path.GetExtension(fileName).ToLower())
                    {
                        case ".csx":
                            solution.ScriptSearchPaths = new string[] { Path.GetDirectoryName(fileName) ?? string.Empty };
                            break;
                        case ".vbx":
                            solution.ScriptSearchPaths = new string[] { Path.GetDirectoryName(fileName) ?? string.Empty };
                            break;
                    }
                }

                parser.ReparseText();
            }
        }

        private void OpenProject(string? projectPathUrl, string projectName)
        {
            if (projectPathUrl is null || Project is null)
                return;

            var projectFileUrl = Path.Combine(projectPathUrl, projectName);
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            if (Project is null)
                return;

            Project.Load(projectFileUrl);

            scriptRun.ScriptSource.FromScriptProject(projectFileUrl);
            var extension = Project.ProjectExtension;
            OpenProject(extension, Project);

            var codeFiles = Project.Files.Where(
                x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".vb").ToList();

            var firstFile = GetFirstFile(codeFiles, Project.DefaultExtension);

            if (codeFiles.Count > 0)
            {
                while (codeFiles.Count > 5)
                    codeFiles.RemoveAt(codeFiles.Count - 1);

                foreach (var codeFile in codeFiles)
                {
                    codeEditContainer.TryActivateEditor(codeFile);
                }

                codeEditContainer.TryActivateEditor(firstFile);
            }

            var references = Project.References.Concat(Project.AutoReferences).Select(
                x => x.FullName).Concat(
                Project.FrameworkReferences.SelectMany(x => x.Assemblies).Select(
                    x => x.HintPath)).ToArray();

            debuggerPanelsTabControl.Errors?.Clear();
            UpdateToolbar();
            UpdateCommands();
        }

        private void OpenProject(string extension, DotNetProject project)
        {
            var projectName = project.ProjectName;
            var projectPath = project.ProjectFileName;
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            if (solution.GetProject(projectName) == null)
                solution.AddProject(projectName, projectPath);

            if (project.Files.Count > 0)
            {
                RegisterCode(
                    extension,
                    [.. project.Files.Where(x => x.EndsWith(project.ProjectExtension))],
                    project.ProjectName);
            }

            var references = project.References
                .Concat(project.AutoReferences)
                .Select(x => x.FullName ?? string.Empty)
                .Concat(project.FrameworkReferences
                    .SelectMany(x => x.Assemblies).Select(x => (x.HintPath ?? string.Empty)))
                .Distinct().ToArray();

            if (references is null)
                return;

            RegisterAssemblies(
                extension,
                [.. project.TryResolveAbsolutePaths(references)],
                projectName: project.ProjectName,
                targetFramework: project.TargetFramework);
        }

        private void RegisterAssemblies(
                    string extension,
                    string[] references,
                    TechnologyEnvironment? technology = null,
                    bool keepExisting = false,
                    string? projectName = null,
                    TargetFramework? targetFramework = null)
        {
            if (references == null)
                return;

            var solution = GetSolution(extension);
            if (solution == null)
                return;
            var project = !string.IsNullOrEmpty(projectName) ? solution.GetProject(projectName) : null;
            var projectId = project?.Id;

            technology ??= DetectTechnologyEnvironmentFromReferences(references);
            targetFramework ??= DetectTargetFrameworkFromReferences(references);

            solution.WithDefaultAssemblies(
                technology.Value,
                keepExisting,
                projectId,
                targetFramework)
                .RegisterAssemblies(references, projectId, targetFramework);
        }

        private static TargetFramework? DetectTargetFrameworkFromReferences(string[] references)
        {
            foreach (var reference in references)
            {
                if (DotNetCoreReferencesDetector.IsDotNetCoreReference(reference, out var version))
                    return new TargetFramework(version, isDotNetCore: true);
            }

            return null;
        }

#pragma warning disable
        private static TechnologyEnvironment DetectTechnologyEnvironmentFromReferences(string[] references)
#pragma warning restore
        {
            return TechnologyEnvironment.System;
        }

        public virtual IRoslynSolution? GetSolution(string extension)
        {
            switch (extension.ToLower())
            {
                case ".cs":
                    return CsSolution.DefaultSolution;
                case ".vb":
                    return VbSolution.DefaultSolution;
                case ".csx":
                    return CsSolution.DefaultScriptSolution;
                case ".vbx":
                    return VbSolution.DefaultScriptSolution;
                default:
                    return null;
            }
        }

        private void RegisterCode(string extension, string[] files, string? projectName = null)
        {
            var solution = GetSolution(extension);
            if (solution == null)
                return;
            var project = !string.IsNullOrEmpty(projectName) ? solution.GetProject(projectName) : null;
            solution.RegisterCodeFiles(files, project?.Id);
        }

        private void StopDebugger()
        {
            if (debugger != null && debugger.IsStarted)
            {
                if (debugger.IsStarted)
                    debugger.StopDebuggingAsync();
            }
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

            var solution = GetSolution(extension);
            if (solution is not null)
            {
                var myproj = solution.GetProject(project.ProjectName);
                if (myproj != null)
                {
                    solution.RemoveProject(myproj.Id);
                }
            }

            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
            UpdateToolbar();
            UpdateCommands();
        }

        private void UpdateToolbar()
        {
            var isToolEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;

            debugMenu.IsEnabled = isToolEnabled;
            debuggerControlToolBar.DebugButtonsEnabled = isToolEnabled;
        }

        private void UpdateCommands()
        {
            (CloseProjectCommand as Alternet.UI.Command)?.ChangeCanExecute();
            (CloseCommand as Alternet.UI.Command)?.ChangeCanExecute();
            (SaveCommand as Alternet.UI.Command)?.ChangeCanExecute();
            fileMenuBar?.UpdateCanExecute();
            debugMenuBar?.UpdateCanExecute();
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
            var customProjectTypeCs = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, CSProjectExtensions },
                    });

#pragma warning disable
            var customProjectTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, VBProjectExtensions },
                    });
#pragma warning restore

            PickOptions options = new()
            {
                PickerTitle = "Select a project file",
                FileTypes = customProjectTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var dirPath = Path.GetDirectoryName(files.FullPath);
            string projectName = Path.GetFileName(files.FullPath);
            OpenProject(dirPath, projectName);
        }

        private Alternet.UI.AbstractControl? GetEditorOrEmptyPanel()
        {
            Alternet.UI.AbstractControl? uiControl = codeEditContainer.ActiveEditor;
            uiControl ??= editorsTabControl.EmptyPanel.Control;
            return uiControl;
        }

        private async void OpenFileDialog()
        {
            var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, CSExtensions },
                });

#pragma warning disable
            var customFileTypeVb = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                    { DevicePlatform.WinUI, VBExtensions },
                    });
#pragma warning restore

            PickOptions options = new()
            {
                PickerTitle = "Select a file",
                FileTypes = customFileTypeCs,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            codeEditContainer.TryActivateEditor(files.FullPath);

            UpdateToolbar();
        }

        [Conditional("DEBUG")]
        private static void TestComplexToolTips()
        {
            var testComplexToolTips = false;

            // This will register custom tooltip for on-screen toolbar, which is shown on mobile devices.
            if (testComplexToolTips)
            {
                InitForMobileIfDebug = true;

                var commandId = Alternet.Editor.AlternetUI.EditorCommands.KnownCommand.ToggleOnScreenKeyboard;
                var existingFunc = Alternet.Editor.AlternetUI.EditorCommands.GetInfoFunc(commandId);

                Alternet.Editor.AlternetUI.EditorCommands.SetInfo(commandId, () =>
                {
                    var result = existingFunc?.Invoke();

                    Alternet.UI.RichToolTipParams prm = new()
                    {
                        Title = "Tooltip title",
                        Text = LoremIpsumSmall,
                        Icon = Alternet.UI.MessageBoxIcon.Information
                    };

                    result?.SetToolTip(prm);
                    return result;
                });
            }
        }
    }
}