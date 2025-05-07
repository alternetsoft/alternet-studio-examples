using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Common.Projects.DotNet;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI.AlternetUI;
using Alternet.Syntax.Lexer;
using Alternet.UI;

namespace Alternet.Scripter.Integration.AlternetUI
{
    public abstract class BaseDebuggerIntegrationPanel : Panel
    {
        private readonly DebugCodeEditContainer codeEditContainer;

        private DebuggerControlToolbar toolbar = new()
        {
            Margin = (0, 0, 0, Alternet.UI.ToolBar.DefaultDistanceToContent),
            Padding = 1,
        };

        private DebugMenu debugMenu = new()
        {
        };

        private readonly TabControl editorsTabControl = new()
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

        private readonly IScriptRunBase scriptRun;

        private DebuggerController? controller = new();
        private DebuggerUIController uicontroller;
        private DotNetProject? project;

        static BaseDebuggerIntegrationPanel()
        {
            CoreClrLauncher.RunProcessFunc = ProcessRunnerWithNotification.RunProcess;
            CoreClrLauncher.NetCoreAppConfigOnWindows = true;
        }

        public BaseDebuggerIntegrationPanel()
        {
            Layout = LayoutStyle.Vertical;
            DebugCodeEdit.CreateParserFunc = DoCreateParser;

            controller.DebuggerStateChanged += (s, e) =>
            {
                if (e.NewState == DebuggerState.Stopped)
                {
                    if (DisposingOrDisposed)
                        return;
                    if (ParentWindow is not null)
                    {
                        ParentWindow.ActiveControl = codeEditContainer?.Editors.FirstOrDefault();
                        ParentWindow.ShowAndFocus(true);
                    }
                }
            };

            toolbar.Controller = controller;
            debugMenu.Controller = controller;
            toolbar.SetVisibleBorders(false, false, false, true);

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);

            uicontroller = new(this, codeEditContainer);
            uicontroller.DebuggerPanels = debuggerPanelsTabControl;

            toolbar.Parent = this;
            panel.Margin = 10;
            panel.Parent = this;
            panel.BottomPanel.MinHeight = 200;
            panel.VerticalAlignment = VerticalAlignment.Fill;
            editorsTabControl.Parent = panel.FillPanel;
            debuggerPanelsTabControl.Parent = panel.BottomPanel;

            void LogMessage(LogMessageEventArgs e)
            {
                if (e.Message is not null)
                    debuggerPanelsTabControl.Output.WriteLineAndWait(e.Message);
            }

            controller.RunningProcessLog += (s, e) =>
            {
                LogMessage(e);
            };

            App.LogMessage += (s, e) =>
            {
                LogMessage(e);
            };

            controller.BeforeCommand += (s, e) =>
            {
                DebuggerUtils.CheckModulesDirectoryPath();

                if (e.Command == DebuggerControllerCommandKind.Compile)
                {
                    debuggerPanelsTabControl.Errors.Clear();
                }
            };

            controller.AfterCommand += (s, e) =>
            {
                if (e.Command == DebuggerControllerCommandKind.Compile)
                {
                }
            };

            var debugger = CreateDebugger();

            controller.Debugger = debugger;

            scriptRun = CreateScriptRun();

            AssemblyUtils.TrySetMemberValue(debugger, "ScriptRun", scriptRun);

            debuggerPanelsTabControl.Debugger = debugger;
            controller.Debugger = debugger;
            uicontroller.Debugger = debugger;
            codeEditContainer.Debugger = debugger;

            DebugMenu.Opened += (s, e) =>
            {
                DebugMenu.IsDebugEnabled = (Project != null && Project.HasProject)
                    || CodeEditContainer.ActiveEditor != null;
            };
        }

        public DotNetProject Project
        {
            get => project ??= CreateProject();
        }

        public IScriptRunBase ScriptRun => scriptRun;

        public DebugCodeEdit? FirstEditor => CodeEditContainer.Editors.FirstOrDefault();

        public DebuggerControlToolbar ToolBar => toolbar;

        public DebugMenu DebugMenu => debugMenu;

        public DebuggerPanelsTabControl DebuggerPanelsTabControl => debuggerPanelsTabControl;

        public DebugCodeEditContainer CodeEditContainer
        {
            get
            {
                return codeEditContainer;
            }
        }

        public DebuggerController? Controller
        {
            get
            {
                return controller;
            }
        }

        public virtual IScriptDebuggerBase? Debugger
        {
            get
            {
                return controller?.Debugger;
            }
        }

        public abstract IScriptDebuggerBase CreateDebugger();

        public abstract IScriptRunBase CreateScriptRun();

        public virtual DotNetProject CreateProject()
        {
            return new DotNetProject();
        }

        public virtual TargetFramework? DetectTargetFrameworkFromReferences(string[] references)
        {
            foreach (var reference in references)
            {
                if (DotNetCoreReferencesDetector.IsDotNetCoreReference(reference, out var version))
                    return new TargetFramework(version, isDotNetCore: true);
            }

            return null;
        }

        public virtual TechnologyEnvironment DetectTechnologyEnvironmentFromReferences(string[] references)
        {
            bool HaveReference(string reference) =>
                references.Any(x =>
                Path.GetFileName(x).Equals(reference, StringComparison.OrdinalIgnoreCase));

            bool windowsForms = HaveReference("System.Windows.Forms")
                || HaveReference("System.Drawing");
            bool wpf = HaveReference("PresentationCore");

            if (windowsForms && wpf)
                return TechnologyEnvironment.WindowsFormsAndWpf;
            if (windowsForms)
                return TechnologyEnvironment.WindowsForms;
            if (wpf)
                return TechnologyEnvironment.Wpf;

            return TechnologyEnvironment.System;
        }

        public virtual void CloseFile(string fileName)
        {
            CodeEditContainer.CloseFile(fileName);
        }

        public virtual void OnSaveMenuItemClick(object? sender, EventArgs e)
        {
            CodeEditContainer.ActiveEditor?.SaveFile(CodeEditContainer.ActiveEditor.FileName);
        }

        public virtual void OnExitMenuItemClick(object? sender, EventArgs e)
        {
            App.AddIdleTask(() =>
            {
                ParentWindow?.Dispose();
            });
        }

        public virtual ILexer? DoCreateParser(Type type)
        {
            return Activator.CreateInstance(type) as ILexer;
        }

        public virtual void StopDebugger()
        {
            controller?.StopDebugAsync();
        }

        public virtual void UpdateToolbar()
        {
            if (ToolBar is null)
                return;
            ToolBar.IsDebugEnabled = (Project != null && Project.HasProject)
                || CodeEditContainer.ActiveEditor != null;
            ToolBar.UpdateDebugButtonsEnabled();
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

        public virtual void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        public virtual void CloseProject(DotNetProject project)
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

            Project?.Reset();
            (scriptRun?.ScriptSource as IScriptSource)?.Reset();
            UpdateToolbar();
        }

        protected override void DisposeManaged()
        {
            SafeDispose(ref controller);
            base.DisposeManaged();
        }
    }
}
