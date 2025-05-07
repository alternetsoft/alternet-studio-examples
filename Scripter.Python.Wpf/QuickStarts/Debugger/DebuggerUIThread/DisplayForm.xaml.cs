#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.Python;
using Alternet.Scripter.Python;

namespace DebuggerUIThread.Python.Wpf
{
    /// <summary>
    /// Interaction logic for DisplayForm.xaml
    /// </summary>
    public partial class DisplayForm : Window
    {
        private IScriptDebuggerBase debugger;

        private Command startCommand;

        public DisplayForm()
        {
            InitializeComponent();
        }

        public DisplayForm(IScriptDebuggerBase debugger, Command startCommand, Point pos)
        {
            InitializeComponent();
            this.debugger = debugger;
            this.startCommand = startCommand;
            this.Top = pos.Y;
            this.Left = pos.X;
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
            Dispatcher.BeginInvoke((Action)(() =>
            {
                StartDebug();
            }));
        }

        public void InvokeCloseDisplayForm()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Close();
            }));
        }

        public void InvokeRunScript()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                RunScript();
            }));
        }

        public void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(
                    delegate (object f)
                    {
                        ((DispatcherFrame)f).Continue = false;
                        return null;
                    }), frame);
            Dispatcher.PushFrame(frame);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void StartDebug()
        {
            if (debugger.State == DebuggerState.Off)
            {
                debugger?.ScriptRun?.Reset();
                debugger.StartDebugging(new PythonStartDebuggingOptions { MethodName = "CalculatePI", RunSynchronously = true, BreakOnStart = true });
            }
        }

        private void RunScript()
        {
            ((ScriptRun)debugger.ScriptRun).RunFunction("CalculatePI");
        }

        private void SetScriptSource()
        {
            var globalItems = ((ScriptRun)debugger.ScriptRun).GlobalItems;
            globalItems.Clear();
            globalItems.Add(new ScriptGlobalItem("resultLabel", resultLabel));
            globalItems.Add(new ScriptGlobalItem("progressBar", progressBar));
            globalItems.Add(new ScriptGlobalItem("owner", this));
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (debugger.IsStarted)
                await debugger.StopDebuggingAsync();
        }

        private void DebugMeButton_Click(object sender, RoutedEventArgs e)
        {
            StartDebug();
        }

        private void RunMeButton_Click(object sender, RoutedEventArgs e)
        {
            RunScript();
        }
    }
}
