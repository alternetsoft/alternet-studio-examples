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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Alternet.Common.Projects.DotNet;
using Alternet.Maui;

using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.MAUI;
using Alternet.Scripter.Integration.MAUI;
using Alternet.Syntax.Lexer;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Python;
using Alternet.Common.Python;
using Alternet.Maui.Extensions;
using Microsoft.Maui.Layouts;
using Alternet.Common;
using System.Diagnostics;
using Alternet.Syntax;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;

using Alternet.Editor.TextSource.AlternetUI;
using System.Reflection;
using System.Xml;
using System.Runtime.InteropServices;

namespace DebuggerIntegration
{
    public partial class MainPage
        : Alternet.UI.DisposableContentPage, Alternet.UI.IProcessRunnerNotification, Alternet.UI.IRaiseSystemColorsChanged
    {
        private static bool testKeyboardVisible;

        public static double DefaultDebuggerPanelHeight { get; set; } = 200;

        public static bool InitForMobileIfDebug { get; set; } = true;

        public static bool? IgnoreHoveredState { get; set; } = null;

        private static bool ExceptionsLogger { get; set; } = true;

        internal string ProjectFolderInResource = "embres:DebuggerIntegration.Content";
        private static readonly string[] pyExtensions = [".py"];
        private static readonly string[] pyProjectExtensions = [".pyproj"];

        private readonly DebugCodeEditContainer codeEditContainer;
        private readonly ScriptRun scriptRun;
        private readonly Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu debugMenu;
        private readonly DebuggerControlToolBarView debuggerControlToolBar = new();
        private readonly DebuggerPanelsTabControlView debuggerPanelsTabControl = new();
        private readonly SimpleTabControlView editorsTabControl = new();
        private readonly MenuBarItem? fileMenuBar;
        private readonly MenuBarItem? debugMenuBar;
        private readonly Alternet.UI.ContextMenu mainMenu = new();
        private readonly Alternet.Scripter.Debugger.UI.AlternetUI.DebuggerController debugController = new();
        private readonly Alternet.Scripter.Debugger.Python.ScriptDebugger debugger;

        private PythonProject project = new();

        public Alternet.UI.ICommand OpenProjectCommand { set; get; }
        public Alternet.UI.ICommand CloseProjectCommand { set; get; }
        public Alternet.UI.ICommand OpenCommand { set; get; }
        public Alternet.UI.ICommand CloseCommand { set; get; }
        public Alternet.UI.ICommand SaveCommand { set; get; }
        public Alternet.UI.ICommand ExitCommand { set; get; }
        Alternet.UI.ObjectUniqueId Alternet.UI.IProcessRunnerNotification.UniqueId { get; } = new();

        static MainPage()
        {
#if MACCATALYST
            Alternet.Editor.AlternetUI.EditConsts.DefaultFontSizeIncrement = 1;
#endif

            BaseLogView.CreateLogView = () =>
            {
                return new Alternet.Editor.Maui.SyntaxEditLogView();
            };

            if (IgnoreHoveredState.HasValue)
                Alternet.UI.PlessMouse.IgnoreHoveredState = IgnoreHoveredState.Value;

            Alternet.UI.MauiUtils.SuppressMenuBarFocus();

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

                    if (e is COMException)
                    {
                        return;
                    }

                    Nop();
                });
                ExceptionsLogger = false;
            }

            Alternet.Scripter.Python.ScriptEngine.RunProcessFunc
                = Alternet.UI.ProcessRunnerWithNotification.RunProcess;

            Alternet.UI.App.LogMessage += (s, e) =>
            {
            };
        }

        public MainPage()
        {
            Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu.ImageSize
                = (int)(Alternet.UI.Display.MaxScaleFactor * 16);

            debugMenu = new Alternet.Scripter.Debugger.UI.AlternetUI.DebugMenu();

            PathUtilities.UseAppSubFolderAsTempPath = true;

            debugger = new Alternet.Scripter.Debugger.Python.ScriptDebugger
            {
                EventsSyncAction = (action) =>
                {
                    Alternet.UI.App.Invoke((Action)action);
                }
            };

            scriptRun = new ScriptRun();
            debugger.ScriptRun = scriptRun;

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
                },
                canExecute: () =>
                {
                    return Project != null && Project.HasProject && canOpen;
                });

            CloseCommand = new Alternet.UI.Command(
                execute: () =>
                {
                    StopDebugger();
                    var edit = codeEditContainer?.ActiveEditor;
                    if (edit != null)
                    {
                        codeEditContainer?.CloseFile(edit.FileName);
                        edit.FileName = string.Empty;
                    }

                    if (!Project.HasProject && codeEditContainer?.Editors.Count == 0)
                    {
                        Project?.Reset();
                        scriptRun?.ScriptSource?.Reset();
                    }

                    UpdateToolbar();
                },
                canExecute: () =>
                {
                    return codeEditContainer?.ActiveEditor != null && canOpen;
                });

            SaveCommand = new Alternet.UI.Command(
              execute: () =>
              {
                  var edit = codeEditContainer?.ActiveEditor;
                  edit?.SaveFile(edit.FileName);
              },
             canExecute: () =>
             {
                 return codeEditContainer?.ActiveEditor != null;
             });

            ExitCommand = MauiCommands.ExitCommand;

            InitializeComponent();

#if ANDROID || IOS || MACCATALYST
            Shell.SetNavBarIsVisible(this, false);
#endif

            var tabFontSize = Alternet.Editor.AlternetUI.EditConsts.DefaultFont.Size * 1.3333;
            editorsTabControl.SetTabFont(null, tabFontSize);
            debuggerPanelsTabControl.SetTabFont(null, tabFontSize);

            debuggerPanelsTabControl.ContextMenuContainerFunc = GetEditorOrEmptyPanelView;
            debuggerPanelsTabControl.ContextMenuAlignment = Alternet.UI.HVDropDownAlignment.BottomRight;

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

            var title = mainMenu.AddDisabledText("Application Menu");

            mainMenu.Add(fileMenu);
            mainMenu.Add(debugMenu);

            Alternet.UI.MauiUtils.FillAbsoluteLayout(MainGrid);

            MainGrid.Insert(0, debuggerControlToolBar);

            debuggerControlToolBar.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsTopBorderVisible = true;

            editorsTabControl.IsVisible = false;

            panel.Add(editorsTabControl);
            panel.Add(debuggerPanelsTabControl, 0, 1);

            debuggerPanelsTabControl.MinimumHeightRequest = DefaultDebuggerPanelHeight;

            editorsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.IsBottomBorderVisible = true;
            debuggerPanelsTabControl.Header.StickyStyle = SimpleToolBarView.StickyButtonStyle.Border;
            editorsTabControl.Header.StickyStyle = SimpleToolBarView.StickyButtonStyle.Border;

            if (Alternet.UI.App.IsTabletOrPhoneDevice || Alternet.UI.DebugUtils.IsDebugDefinedAndAttached)
            {
                SimpleToolBarView.AddNextAndPreviousTabButtonsFlags options =
                    SimpleToolBarView.AddNextAndPreviousTabButtonsFlags.MakeSticky;

                debuggerPanelsTabControl.Header.AddNextAndPreviousTabButtons(options);
                editorsTabControl.Header.AddNextAndPreviousTabButtons(options);
                debuggerPanelsTabControl.MakeSelectedTabFirst = true;
                editorsTabControl.MakeSelectedTabFirst = true;
            }

            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.Parsers[".py"] = typeof(PythonNETParser);
            Alternet.Scripter.Integration.AlternetUI.DebugCodeEdit.CreateParserFunc = DoCreateParser;

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += OnEditorRequested;

            debuggerControlToolBar.Debugger = debugger;
            debuggerControlToolBar.DebuggerPreStartup += OnDebuggerPreStartup;

            debugController.Debugger = debugger;
            debugController.DebuggerPreStartup += OnDebuggerPreStartup;
            debugMenu.Controller = debugController;

            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new Alternet.Scripter.Integration.AlternetUI.DebuggerUIController(this, codeEditContainer)
            {
                Debugger = debugger,
                DebuggerPanels = debuggerPanelsTabControl
            };

            codeEditContainer.Debugger = debugger;

            if (Alternet.UI.App.IsWindowsOS)
            {
                openMenuItem.Command = OpenCommand;
                openProjectMenuItem.Command = OpenProjectCommand;
            }

            closeProjectMenuItem.Command = CloseProjectCommand;
            closeMenuItem.Command = CloseCommand;
            saveMenuItem.Command = SaveCommand;
            exitMenuItem.Command = ExitCommand;

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

            void CreateMenuButton()
            {
                debuggerControlToolBar.InsertButton(
                    0,
                    text: null,
                    toolTip: "Show Application Menu",
                    image: Alternet.UI.KnownSvgImages.ImgBars,
                    onClick: () =>
                    {
                        var uiControl = GetEditorOrEmptyPanelView();
                        Alternet.UI.MauiUtils.ShowContextMenu(mainMenu, uiControl, Alternet.UI.HVDropDownAlignment.Center);
                    });
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

            UpdateToolbar();
            UpdateCommands();
            RaiseSystemColorsChanged();

            debuggerPanelsTabControl.SelectedTabClickedAgain+= (s, e) =>
            {
                debuggerPanelsTabControl.ToggleContentVisibility(DefaultDebuggerPanelHeight);
            };
        }

        public static void Nop()
        {
        }

        public void RaiseSystemColorsChanged()
        {
            var headerColor = SimpleTabControlView.AltHeaderBackColor;
            editorsTabControl.Header.BackgroundColor = headerColor;
            debuggerPanelsTabControl.Header.BackgroundColor = headerColor;
        }

        protected View? GetEditorOrEmptyPanelView()
        {
            View? uiControl = codeEditContainer.ActiveEditorView;

            uiControl ??= editorsTabControl.EmptyPanel;

            return uiControl;
        }

        protected Alternet.UI.AbstractControl? GetEditorOrEmptyPanel()
        {
            Alternet.UI.AbstractControl? uiControl = codeEditContainer.ActiveEditor;

            uiControl ??= editorsTabControl.EmptyPanel.Control;

            return uiControl;
        }

        protected override void OnAppearingOnce()
        {
            base.OnAppearingOnce();

            try
            {
                var projectFolder = PathUtilities.GetTempPathUniquePerApp();

                string[] projectFiles =
                [
                        "MyModule.py",
                        "Project.pyproj",
                        "ScriptSimple.py",
                    ];

                var extractionResult = Alternet.UI.ResourceLoader.ExtractResourcesSafe(
                    ProjectFolderInResource,
                    projectFiles,
                    projectFolder);

                OpenProject(projectFolder, "Project.pyproj");

                SendToLog("Application is ready...");
                SendToLog(Alternet.Editor.AlternetUI.EditUtilities.LogViewCommandGoToBegin);
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

            if (visibilityService != null)
            {
                visibilityService.KeyboardVisibleChanged += (s, e) =>
                {
                };
            }

            if (Alternet.UI.DebugUtils.IsDebugDefinedAndAttached && true && Alternet.UI.App.IsWindowsOS)
            {
                Alternet.Editor.AlternetUI.EditorCommands.CanToggleOnScreenKeyboardOverride = (e) =>
                {
                    return true;
                };

                Alternet.Editor.AlternetUI.EditorCommands.ToggleOnScreenKeyboardOverride = (e) =>
                {
                    Alternet.Editor.Maui.SyntaxEditView.DefaultToggleOnScreenKeyboard(e);

                    testKeyboardVisible = !testKeyboardVisible;

                    visibilityService?.RaiseKeyboardVisibleChanged(new Alternet.UI.KeyboardVisibleChangedEventArgs
                    {
                        IsVisible = testKeyboardVisible,
                        Height = 300,
                    });
                };

                codeEditContainer.UpdateCommandState();
            }
        }

        protected virtual void SendToLog(string s)
        {
            debuggerPanelsTabControl.Output?.WriteLine(s);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await TrySetFocusToEditor();
        }

        protected async Task TrySetFocusToEditor()
        {
            var editor = codeEditContainer.FirstOrDefault();
            if (editor is null)
                return;
            await editor.TrySetFocusWithTimeout();
        }

        protected override void DisposeResources()
        {
            StopDebugger();
            Alternet.UI.ProcessRunnerWithNotification.Unbind(this);
            base.DisposeResources();
        }

        protected PythonProject Project
        {
            get => project;
            private set => project = value;
        }

        private static string GetFirstFile(List<string> files, string langExt)
        {
            string result = files.Count > 0 ? files[0] : string.Empty;

            foreach (string file in files)
            {
                if (file.Contains("program.py", StringComparison.CurrentCultureIgnoreCase))
                    return file;
                if (file.Contains("main", StringComparison.CurrentCultureIgnoreCase) && file.EndsWith(langExt))
                    return file;
            }

            return result;
        }

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
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
            var parser = new PythonNETParser
            {
                CodeEnvironment = scriptRun.CodeEnvironment,
            };

            var edit = new DebugCodeEditView();
            var editor = edit.Editor;
            editor.ContextMenuStrip = editor.DefaultMenu;
            edit.SetBorderWidth(0, 1, 0, 1);

            if (Alternet.UI.DebugUtils.IsDebugDefinedAndAttached && InitForMobileIfDebug)
            {
                Alternet.Editor.AlternetUI.EditorOnMobileHelper.IsMobileDeviceOverride = () => true;
            }

            var editorOnMobileResult = Alternet.Editor.AlternetUI.EditorOnMobileHelper.InitializeOnMobile(edit.Editor);

            edit.Editor.LoadFileOrShowErrorInText(e.FileName);
            editor.FileName = e.FileName;
            edit.Lexer = parser;
            e.DebugEdit = editor;
            UpdateCommands();

            var logMouseEvents = false;

            if (logMouseEvents)
            {
                editor.MouseDown += (s, ev) =>
                {
                    Alternet.UI.App.Log($"MouseDown: Button={ev.Button}, Clicks={ev.Clicks}, X={ev.X}, Y={ev.Y}");
                };
                editor.MouseUp += (s, ev) =>
                {
                    Alternet.UI.App.Log($"MouseUp: Button={ev.Button}, Clicks={ev.Clicks}, X={ev.X}, Y={ev.Y}");
                };
                editor.LongTap += (s, ev) =>
                {
                    Alternet.UI.App.Log($"LongTap gesture");
                };
            }
        }

        private void OpenProject(string? projectPathUrl, string projectName)
        {
            if (projectPathUrl is null)
                return;

            if (Project is null)
                return;

            if (Project.HasProject)
                CloseProject(Project);

            var projectFileUrl = Path.Combine(projectPathUrl, projectName);

            Project.Load(projectFileUrl);

            scriptRun.ScriptSource.FromScriptProject(projectFileUrl);

            var codeFiles = Project.Files.Where(
                x => Path.GetExtension(x) == ".py").ToList().ToList();

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

            SendToLog($"Project is loaded: {projectName}");
        }

        private async void StopDebugger()
        {
            if (debugger != null && debugger.IsStarted)
                await debugger.StopDebuggingAsync();
        }

        private void CloseProject(DotNetProject project)
        {
            StopDebugger();

            var hasOpenFiles = project.Files.Count > 0 || project.Resources.Count > 0;

            foreach (string fileName in project.Files)
            {
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

#pragma warning disable
            var extension = string.Format(".{0}", project.DefaultExtension);
#pragma warning restore

            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
            UpdateToolbar();
            UpdateCommands();

            if (hasOpenFiles)
                Alternet.UI.App.Log($"Project is closed.");
        }

        private void UpdateToolbar()
        {
            debugMenu.IsEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
            debuggerControlToolBar.IsEnabled = (Project != null && Project.HasProject)
                || codeEditContainer.ActiveEditor != null;
        }

        private void UpdateCommands()
        {
            (CloseProjectCommand as Command)?.ChangeCanExecute();
            (CloseCommand as Command)?.ChangeCanExecute();
            (SaveCommand as Command)?.ChangeCanExecute();
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
            var customProjectTypePy = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                                { DevicePlatform.WinUI, pyProjectExtensions },
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a Py proj file",
                FileTypes = customProjectTypePy,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            var dirPath = Path.GetDirectoryName(files.FullPath);
            string projectName = Path.GetFileName(files.FullPath);
            OpenProject(dirPath, projectName);
        }

        private async void OpenFileDialog()
        {
            var customFileTypePy = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, pyExtensions },
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a Py file",
                FileTypes = customFileTypePy,
            };

            var files = await FilePicker.Default.PickAsync(options);

            if (files == null)
                return;

            codeEditContainer.TryActivateEditor(files.FullPath);
            UpdateToolbar();
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

        void Alternet.UI.IProcessRunnerNotification.OnRunningProcessLog(
            Process process,
            string data,
            Alternet.UI.LogItemKind kind)
        {
            debuggerPanelsTabControl.Output?.WriteLineAndWait(data);
        }
    }
}
