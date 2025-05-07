using System;
using Alternet.Scripter.Debugger;
using Alternet.UI;
using Alternet.Scripter;
using Alternet.Scripter.Integration.MAUI;

namespace DebuggerIntegration
{
    public static class ScripterDemoUtils
    {
        public static bool ThrowExceptionIfDebugging { get; set; } = true;

        public static void Initialize()
        {
            CoreClrLauncher.RunProcessFunc = ProcessRunnerWithNotification.RunProcess;

            CoreClrLauncher.NetCoreAppConfigOnWindows = true;
        }

        public static IScriptDebuggerBase? CreateDebugger(bool useOldDebugger)
        {
            const bool logSelectedDebugger = true;

            IScriptDebuggerBase? debugger;

            if (Alternet.UI.App.IsWindowsOS && useOldDebugger)
            {
                debugger = AssemblyUtils.ActivatorCreateInstance<IScriptDebuggerBase>(
                    "Alternet.Scripter.Debugger.v10",
                    "Alternet.Scripter.Debugger.ScriptDebugger");

                if (Alternet.UI.App.IsWindowsOS && DebugUtils.IsDebugDefinedAndAttached)
                    Alternet.UI.App.LogIf("Using old debugger", logSelectedDebugger);
            }
            else
            {
                debugger = AssemblyUtils.ActivatorCreateInstance<IScriptDebuggerBase>(
                    "Alternet.Scripter.DebuggerNew.v10",
                    "Alternet.Scripter.Debugger.ScriptDebuggerNew");
                if (Alternet.UI.App.IsWindowsOS && DebugUtils.IsDebugDefinedAndAttached)
                    Alternet.UI.App.LogIf("Using new debugger", logSelectedDebugger);
            }

            return debugger;
        }
    }

}
