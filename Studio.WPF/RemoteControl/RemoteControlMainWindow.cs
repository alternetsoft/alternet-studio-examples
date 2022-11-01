#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Studio

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
using System.Text;
using System.Threading.Tasks;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Scripter.Debugger;

namespace AlternetStudio.Wpf.Demo.RemoteControl
{
    [System.ComponentModel.DesignerCategory("Code")]
    internal class RemoteControlMainWindow : MainWindow
    {
        private const string GlobalCodeFileName = "GlobalCode.cs";

        private RemoteControlParameters remoteControlParameters;
        private RemoteControlService remoteControlService;

        public RemoteControlMainWindow(RemoteControlParameters remoteControlParameters)
        {
            this.remoteControlParameters = remoteControlParameters;
            remoteControlService = new RemoteControlService(remoteControlParameters);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (Debugger.IsStarted)
                Task.Run(async () => await Debugger.StopDebuggingAsync()).Wait();

            base.OnClosed(e);

            Debugger.ClearTemporaryGeneratedModules();
        }

        protected override void RunScriptCore()
        {
            remoteControlService.CompileScript(result =>
            {
                if (result.IsSuccessful)
                    Dispatcher.Invoke((Action)(() => remoteControlService.StartScript(() => { })));
                else if (result.Errors.Length > 0)
                    ReportScriptCompilationErrors(result.Errors);
            });
        }

        protected override void StartDebugCore(bool breakOnStart)
        {
            remoteControlService.CompileScript(result =>
            {
                if (result.IsSuccessful)
                {
                    var targetAssembly = new[] { result.TargetAssemblyName };
                    AttachToControlledProcess(remoteControlParameters.MyCodeModules?.Concat(targetAssembly).ToArray() ?? targetAssembly);
                    Dispatcher.Invoke((Action)StartControlledScript);
                }

                if (result.Errors.Length > 0)
                    ReportScriptCompilationErrors(result.Errors);
            });
        }

        protected override void StopDebugCore()
        {
            Dispatcher.Invoke((Action)StopControlledScript);
        }

        protected override void LoadStartupFile()
        {
            LoadControlledScript();
        }

        protected override IScriptEdit NewFile(string fileName)
        {
            var scriptEdit = base.NewFile(fileName);

            var globalCode = remoteControlParameters.GlobalCode;
            if (globalCode != null)
            {
                scriptEdit.RegisterCode(GlobalCodeFileName, globalCode);
                scriptEdit.Rescan();
            }

            return scriptEdit;
        }

        protected override bool SetScriptSource()
        {
            if (!base.SetScriptSource())
                return false;

            if (Project.HasProject)
                return true;

            if (ScriptRun.ScriptSource.HasScriptFile && !string.IsNullOrEmpty(remoteControlParameters.GlobalCode))
                ScriptRun.ScriptSource.Files.Add(GlobalCodeFileName);

            return true;
        }

        private void StopControlledScript()
        {
            if (remoteControlService != null)
            {
                if (Debugger.State != DebuggerState.Off)
                {
                    Task.Run(() => Debugger.StopDebuggingAsync()).Wait();
                    remoteControlService.StopScript();
                }
            }
        }

        private void LoadControlledScript()
        {
            if (remoteControlService == null)
                return;

            string extension;
            string projectName = null;

            var controlledScriptFile = remoteControlParameters.MainScriptFile;

            if (IsProjectFile(controlledScriptFile))
            {
                extension = string.Compare(Path.GetExtension(controlledScriptFile), ".csproj", true) == 0 ? ".cs" : ".vb";
                projectName = controlledScriptFile;
                OpenProject(projectName);
            }
            else
            {
                extension = Path.GetExtension(controlledScriptFile);
                OpenFile(controlledScriptFile);
            }

            var controlledScriptCodeFiles = remoteControlParameters.CodeFiles;
            if (controlledScriptCodeFiles != null)
                CodeEditExtensions.RegisterCode(extension, controlledScriptCodeFiles, projectName);

            var references = remoteControlParameters.References;
            if (references != null)
            {
                CodeEditExtensions.RegisterAssemblies(
                    extension,
                    Project.HasProject ? Project.TryResolveAbsolutePaths(references).ToArray() : references.ToArray(),
                    keepExisting: true,
                    projectName: projectName);
            }

            ScriptRun.FileLoad += (o, e) =>
            {
                if (!string.IsNullOrEmpty(remoteControlParameters.GlobalCode) && Path.GetFileName(e.FileName) == GlobalCodeFileName)
                {
                    e.Text = remoteControlParameters.GlobalCode;
                    e.Handled = true;
                }
            };
        }

        private void AttachToControlledProcess(string[] myCodeModules)
        {
            Task.Run(() =>
                Debugger.AttachToProcessAsync(
                    remoteControlParameters.ProcessId,
                    new StartDebuggingOptions
                    {
                        MyCodeModules = myCodeModules,
                    })).Wait();
        }

        private void StartControlledScript()
        {
            if (remoteControlService != null)
            {
                remoteControlService.StartScript(() =>
                {
                    Dispatcher.Invoke((Action)StopDebug);
                });
            }
        }
    }
}