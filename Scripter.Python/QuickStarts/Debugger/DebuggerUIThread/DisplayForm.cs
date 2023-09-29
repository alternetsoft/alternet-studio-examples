using System;
using System.ComponentModel;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Python;

namespace DebuggerIntegration.Python
{
    public partial class DisplayForm : Form
    {
        private IScriptDebuggerBase debugger;

        private Command startCommand;

        public DisplayForm(IScriptDebuggerBase debugger, Command startCommand)
        {
            InitializeComponent();
            this.debugger = debugger;
            this.startCommand = startCommand;
            SetScriptSource();
        }

        public enum Command
        {
            None = 0,
            Run = 1,
            Debug = 2,
            Close = 3,
        }

        public void InvokeStartDebug()
        {
            Invoke((Action)(() =>
            {
                var command = Command.Debug;
                OSUtils.PostMessage(Handle, OSUtils.Message_WM_USER + 1, new IntPtr((int)command), IntPtr.Zero);
            }));
        }

        public void InvokeCloseDisplayForm()
        {
            Invoke((Action)(() =>
            {
                var command = Command.Close;
                OSUtils.PostMessage(Handle, OSUtils.Message_WM_USER + 1, new IntPtr((int)command), IntPtr.Zero);
            }));
        }

        public void InvokeRunScript()
        {
            this.Invoke((Action)(() =>
            {
                var command = Command.Run;
                OSUtils.PostMessage(Handle, OSUtils.Message_WM_USER + 1, new IntPtr((int)command), IntPtr.Zero);
            }));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == OSUtils.Message_WM_USER + 1)
            {
                var command = m.WParam.ToInt32();
                switch (command)
                {
                    case (int)Command.Run:
                        RunScript();
                        break;

                    case (int)Command.Debug:
                        StartDebug();
                        break;

                    case (int)Command.Close:
                        Close();
                        break;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            switch (startCommand)
            {
                case Command.Run:
                    RunScript();
                    break;

                case Command.Debug:
                    StartDebug();
                    break;
            }
        }

        protected async override void OnClosing(CancelEventArgs e)
        {
            if (debugger.IsStarted)
                await debugger.StopDebuggingAsync();

            base.OnClosing(e);
        }

        private void RunScript()
        {
            ((ScriptRun)debugger.ScriptRun).RunFunction("CalculatePI");
        }

        private void StartDebug()
        {
            if (debugger.State == DebuggerState.Off)
            {
                debugger?.ScriptRun?.Reset();
                debugger.StartDebugging(new PythonStartDebuggingOptions { MethodName = "CalculatePI", RunSynchronously = true, BreakOnStart = true });
            }
        }

        private void SetScriptSource()
        {
            var globalItems = ((ScriptRun)debugger.ScriptRun).GlobalItems;
            globalItems.Clear();
            globalItems.Add(new ScriptGlobalItem("resultLabel", resultLabel));
            globalItems.Add(new ScriptGlobalItem("progressBar", progressBar));
        }

        private void DebugMeButton_Click(object sender, EventArgs e)
        {
            StartDebug();
        }

        private void RunMeButton_Click(object sender, EventArgs e)
        {
            RunScript();
        }
    }
}
