#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;

namespace AlternetStudio.Wpf.Demo.RemoteControl
{
    internal class RemoteControlService
    {
        private readonly RemoteControlParameters parameters;
        private Timer killRemoteDebuggerTimer;

        public RemoteControlService(RemoteControlParameters parameters)
        {
            this.parameters = parameters;
            RemoteControl = DebuggerCommunication.StartClient(
                parameters.IpcPortName,
                parameters.IpcObjectUri,
                parameters.IpcPortName != null ? parameters.IpcPortName + "ClientChannel" : null);

            killRemoteDebuggerTimer = new Timer();
            killRemoteDebuggerTimer.Interval = 200;
            killRemoteDebuggerTimer.Tick += KillRemoteDebuggerTimer_Tick;
            killRemoteDebuggerTimer.Start();
        }

        public IScriptDebuggerRemoteControl RemoteControl { get; set; }

        public void CompileScript(ScriptCompiledDelegate onScriptCompiled)
        {
            if (RemoteControl != null)
            {
                var sink = new DebuggerCommunication.ScriptDebuggerRemoteControlCompileCallbackSink(onScriptCompiled);
                RemoteControl.CompileScript(sink.OnScriptCompiled);
            }
        }

        public void StartScript(ScriptFinishedDelegate onScriptFinished)
        {
            if (RemoteControl != null)
            {
                var sink = new DebuggerCommunication.ScriptDebuggerRemoteControlStartCallbackSink(onScriptFinished);
                RemoteControl.StartScript(sink.OnScriptFinished);
            }
        }

        public void StopScript()
        {
            if (RemoteControl != null)
                RemoteControl.StopScript();
        }

        public void ReadyToDebug(StartDebugDelegate onStartDebug)
        {
            if (RemoteControl != null)
            {
                var sink = new DebuggerCommunication.ScriptDebuggerRemoteControlStartDebugCallbackSink(onStartDebug);
                RemoteControl.ReadyToDebug(sink.OnStartDebug);
            }
        }

        private void KillRemoteDebuggerTimer_Tick(object sender, EventArgs e)
        {
            if (!Process.GetProcesses().Any(x => x.Id == parameters.ProcessId))
                Application.Exit();
        }
    }
}
