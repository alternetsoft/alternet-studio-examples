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
using System.Windows;
using System.Windows.Controls;

using Alternet.Editor.Common.Wpf;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.IronPython;
using Alternet.Scripter.Debugger.UI.Wpf;
using Alternet.Scripter.Integration.Wpf;

namespace AlternetStudio.IronPython.Wpf.Demo
{
    public partial class MainWindow : IDebuggerUICommands
    {
#pragma warning disable SA1401 // Fields should be private
        public static bool DisplayExceptions = false;
#pragma warning restore SA1401 // Fields should be private

        private IScriptDebugger debugger;
        private ExecutionPosition executionPosition;
        private StackFrame stackFrame;
        private bool switchToTopUserStackFrameNeeded;

        /// <summary>
        /// Gets or sets <see cref="StartDebuggingOptions"/> to use when
        /// the debugging starts while executing commands such as <see cref="Start"/> or <see cref="StepOver"/>.
        /// </summary>
        public StartDebuggingOptions StartDebuggingOptions { get; set; } = new StartDebuggingOptions();

        private IScriptDebugger Debugger
        {
            get
            {
                if (debugger == null)
                {
                    debugger = new Alternet.Scripter.Debugger.IronPython.ScriptDebugger { ScriptRun = scriptRun };
                    debugger.DebuggingStarted += Debugger_DebuggingStarted;
                    debugger.DebuggingStopped += Debugger_DebuggingStopped;
                    debugger.DebuggerErrorOccured += Debugger_DebuggerErrorOccured;
                    debugger.ExecutionResumed += Debugger_ExecutionResumed;
                    debugger.ExecutionStopped += Debugger_ExecutionStopped;
                    debugger.StackFrameSwitched += Debugger_StackFrameSwitched;
                    debugger.ActiveThreadChanged += Debugger_ActiveThreadChanged;

                    breakpointsControl.Debugger = debugger;
                    callStackControl.Debugger = debugger;
                    outputControl.Debugger = debugger;
                    localsControl.Debugger = debugger;
                    watchesControl.Debugger = debugger;
                    errorsControl.Debugger = debugger;

                    callStackControl.StackFramesChanged += Callstack_StackFramesRetrieved;
                    callStackControl.CallStackClick += CallStackControl_CallStackClick;

                    localsControl.AddToWatchClick += Locals_AddToWatchClick;

                    debugger.EventsSyncAction = action => Dispatcher.BeginInvoke(action);
                }

                return debugger;
            }
        }

        bool IDebuggerUICommands.PreStartup() => false;

        bool IDebuggerUICommands.Start()
        {
            StartDebugging(false);
            return true;
        }

        bool IDebuggerUICommands.StartWithoutDebug()
        {
            RunScript();
            return true;
        }

        bool IDebuggerUICommands.Continue()
        {
            if (Debugger.IsStarted)
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.Continue();
            }

            return true;
        }

        bool IDebuggerUICommands.Compile()
        {
            Compile();
            return true;
        }

        bool IDebuggerUICommands.Stop()
        {
            StopDebug();
            return true;
        }

        bool IDebuggerUICommands.Break()
        {
            Debugger.Break();
            return true;
        }

        bool IDebuggerUICommands.StepOut()
        {
            CancelLocalsAndWatchesEvaluation();
            Debugger.StepOut();

            return true;
        }

        bool IDebuggerUICommands.StepInto()
        {
            if (!Debugger.IsStarted)
                StartDebugging(true);
            else
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.StepInto();
            }

            return true;
        }

        bool IDebuggerUICommands.StepOver()
        {
            if (!Debugger.IsStarted)
                StartDebugging(true);
            else
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.StepOver();
            }

            return true;
        }

        bool IDebuggerUICommands.EvaluateExpression()
        {
            EvaluateExpression();

            return true;
        }

        bool IDebuggerUICommands.EvaluateCurrentException()
        {
            EvaluateExpression(evaluateCurrentException: true);
            return true;
        }

        protected virtual void StartDebugCore(bool breakOnStart)
        {
            StartDebuggingOptions.BreakOnStart = breakOnStart;
            Debugger.StartDebugging(StartDebuggingOptions);
        }

        protected void StopDebug()
        {
            CancelLocalsAndWatchesEvaluation();
            StopDebugCore();
        }

        protected virtual void StopDebugCore()
        {
            Debugger.StopDebuggingAsync();
        }

        private bool TryResetDebuggerOnProjectChange()
        {
            if (debugger != null && debugger.IsStarted)
            {
                MessageBox.Show("Please stop debugging session first");
                return false;
            }

            if (debugger != null)
                debugger.Breakpoints.Clear();

            return true;
        }

        private void InitDebugEdit(IDebugEdit edit)
        {
            edit.Debugger = Debugger;
            edit.BreakpointToggle += DebugEdit_BreakpointToggle;
            edit.SetNextStatement += DebugEdit_SetNextStatement;
            edit.AddToWatchClick += DebugEdit_AddToWatchClick;

            if (Debugger.IsStarted)
                edit.ReadOnly = true;
        }

        private void InitializeDebugger()
        {
            debuggerControlToolbar.Debugger = Debugger;

            debugMenu.Debugger = Debugger;
            debugMenu.CommandsListener = debuggerControlToolbar.CommandsListener = this;
        }

        private void ActivateWatchesTab()
        {
            bottomTabControl.SelectedIndex = WatchesTabIndex;
        }

        private void Locals_AddToWatchClick(object sender, AddToWatchClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Expression))
            {
                watchesControl.AddWatch(e.Expression, true, true);
                ActivateWatchesTab();
            }
        }

        private void StartDebugging(bool breakOnStart)
        {
            if (Debugger.IsStarted)
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.Continue();
            }
            else
            {
                if (Debugger.State == DebuggerState.Startup)
                    return;

                if (SaveAllModifiedFiles() && SetScriptSource())
                {
                    errorsControl.Clear();

                    StartDebugCore(breakOnStart);

                    var errors = scriptRun.ScriptHost.CompilerErrors;
                    if (errors != null && errors.Length > 0)
                        errorsControl.AddCompilerErrors(errors);

                    if (scriptRun.ScriptHost.CompileFailed)
                        ActivateErrorsTab();
                }
            }
        }

        private void EvaluateExpression(bool evaluateCurrentException = false)
        {
            string symbol = ActiveSyntaxEdit != null ? ActiveSyntaxEdit.GetSymbolAtCursor() : string.Empty;
            var dialog = new EvaluateDialog()
            {
                Debugger = Debugger,
                EvaluateCurrentException = evaluateCurrentException,
                Expression = symbol,
                Owner = this,
            };

            dialog.WatchAdded += (o, e) => watchesControl.AddWatch(e.Expression);
            dialog.WatchAdded += (o, e) => ActivateWatchesTab();
            dialog.ShowDialog();
        }

        private void UpdateDebugButtons()
        {
            var state = Debugger.State;
            bool hasProject = Project.HasProject;
            var isEmpty = !(hasProject | ActiveSyntaxEdit != null);
            var isDebuggingStarted = Debugger.IsStarted;
            var isDebugging = isDebuggingStarted || state == DebuggerState.Startup;
            runToCursorMenuItem.IsEnabled = !isEmpty && state != DebuggerState.Running && ActiveSyntaxEdit != null;
        }

        private async void UpdateDebugPanels()
        {
            try
            {
                switch (bottomTabControl.SelectedIndex)
                {
                    case CallStackTabIndex:
                        if (Debugger.IsStarted)
                            callStackControl.SetFrames((await Debugger.GetStackFramesAsync()).Frames);
                        break;
                    case VariablesInScopeTabIndex:
                        if (Debugger.IsStarted)
                            localsControl.EvaluateVariables((await Debugger.GetVariablesInScopeAsync()).VariableNames);
                        break;
                    case WatchesTabIndex:
                        watchesControl.EvaluateWatchExpressions();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while updating debugger windows: " + e);
            }
        }

        private void Callstack_StackFramesRetrieved(object sender, EventArgs e)
        {
            if (switchToTopUserStackFrameNeeded)
            {
                switchToTopUserStackFrameNeeded = false;
                callStackControl.TrySwitchToTopUserStackFrame();
            }
        }

        private void SetAllEditorsReadOnlyValue(bool readOnly)
        {
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                var edit = GetEditor(tabPage);
                if (edit != null)
                    edit.ReadOnly = readOnly;
            }
        }

        private void Debugger_DebuggingStarted(object sender, DebuggingStartedEventArgs e)
        {
            executionPosition = null;
            outputControl.Clear();
            UpdateDebugButtons();
            SetAllEditorsReadOnlyValue(true);
        }

        private void Debugger_DebuggerErrorOccured(object sender, DebuggerErrorOccuredEventArgs e)
        {
            DisplayDebuggerUnhandledException(e.Exception.Message);
        }

        private void Debugger_DebuggingStopped(object sender, DebuggingStoppedEventArgs e)
        {
            UpdateDebugButtons();
            ClearExecutionPosition();
            ClearStackFrame();
            SetAllEditorsReadOnlyValue(false);
        }

        private void Debugger_ExecutionStopped(object sender, ExecutionStoppedEventArgs e)
        {
            UpdateDebugButtons();

            Activate();
            UpdateDebugPanels();

            ClearExecutionPosition();

            executionPosition = e.Position;

            if (e.Position != null && !string.IsNullOrEmpty(e.Position.File))
            {
                var edit = OpenFile(e.Position.File);
                if (edit != null)
                    ((IDebugEdit)edit).ExecutionStopped(e.Position);
            }

            if (e.StopReason == ExecutionStopReason.Exception)
            {
                if (DisplayExceptions)
                    DisplayDebuggerUnhandledException(e);
                else
                    ((IDebuggerUICommands)this).Continue();
            }
        }

        private void DisplayDebuggerUnhandledException(ExecutionStoppedEventArgs e)
        {
#pragma warning disable SA1012 // Opening braces should be spaced correctly
            DisplayDebuggerUnhandledException($"{ e.Exception.ExceptionType}\n{ e.Exception.Message}");
#pragma warning restore SA1012 // Opening braces should be spaced correctly
        }

        private void DisplayDebuggerUnhandledException(string exception)
        {
            var str = $"{exception}\nDo you want to evaluate the exception object?";
            if (MessageBox.Show(this, str, "Unhandled exception", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                EvaluateExpression(true);
        }

        private void Debugger_StackFrameSwitched(object sender, StackFrameSwitchedEventArgs e)
        {
            if (e.Frame.Line == null)
                return;

            if (string.IsNullOrEmpty(e.ErrorMessage))
            {
                ClearStackFrame();
                stackFrame = e.Frame;

                var edit = OpenFile(e.Frame.File);
                if (edit != null)
                {
                    (edit as IDebugEdit).SwitchStackFrame(e.Frame, executionPosition);
                    edit.Focus();
                }
            }
            else
                MessageBox.Show(e.ErrorMessage);
        }

        private async void Debugger_ActiveThreadChanged(object sender, ActiveThreadChangedEventArgs e)
        {
            if (e.ActivatedThread == null)
                return;

            if (string.IsNullOrEmpty(e.ErrorMessage))
            {
                switchToTopUserStackFrameNeeded = true;
                ActivateCallStackTab();
                try
                {
                    callStackControl.SetFrames((await Debugger.GetStackFramesAsync()).Frames);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while retrieving stack frames: " + ex);
                }
            }
            else
                MessageBox.Show(e.ErrorMessage);
        }

        private void ActivateCallStackTab()
        {
            bottomTabControl.SelectedIndex = CallStackTabIndex;
        }

        private void Debugger_ExecutionResumed(object sender, ExecutionResumedEventArgs e)
        {
            UpdateDebugButtons();
            ClearExecutionPosition();
            ClearStackFrame();
        }

        private void ClearExecutionPosition()
        {
            if (executionPosition != null)
            {
                var edit = FindFile(executionPosition.File);
                if (edit != null)
                    (edit as IDebugEdit).ClearDebugStyles(executionPosition);
                executionPosition = null;
            }
        }

        private void ClearStackFrame()
        {
            if (stackFrame != null)
            {
                var edit = FindFile(stackFrame.File);
                if (edit != null)
                    (edit as IDebugEdit).ClearStackFrame(stackFrame);
                stackFrame = null;
            }
        }

        private async void CancelLocalsAndWatchesEvaluation()
        {
            switch (bottomTabControl.SelectedIndex)
            {
                case VariablesInScopeTabIndex:
                    if (Debugger.IsStarted)
                        await localsControl.CancelEvaluationAsync();
                    break;
                case WatchesTabIndex:
                    await watchesControl.CancelEvaluationAsync();
                    break;
            }
        }

        private void RunToCursorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = ActiveSyntaxEdit;
            if (edit == null)
                return;
            Debugger.SetRunToPositionBreakpoint(new RunToPositionBreakpoint(edit.FileName, edit.CurrentLine + 1));
            StartDebugging(false);
        }

        private void StartDebugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartDebugging(false);
        }

        private void StopDebugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StopDebug();
        }

        private void BottomTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDebugPanels();
        }

        private void StepIntoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsStarted)
                StartDebugging(true);
            else
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.StepInto();
            }
        }

        private void StepOverMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!Debugger.IsStarted)
                StartDebugging(true);
            else
            {
                CancelLocalsAndWatchesEvaluation();
                Debugger.StepOver();
            }
        }

        private void EvaluateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EvaluateExpression();
        }

        private void CallStackControl_CallStackClick(object sender, CallStackClickEventArgs e)
        {
            if (e.StackFrame != null)
            {
                IScriptEdit edit = OpenFile(e.StackFrame.File);
                if (edit != null)
                {
                    edit.MakeVisible(new System.Drawing.Point(0, e.StackFrame.Line.Value - 1), true);
                    edit.Focus();
                }
            }
        }

        private void BreakpointsControl_BreakpointClick(object sender, BreakpointClickEventArgs e)
        {
            if (e.Breakpoint != null)
            {
                IScriptEdit edit = OpenFile(e.Breakpoint.FilePath);
                if (edit != null)
                {
                    edit.MakeVisible(new System.Drawing.Point(0, e.Breakpoint.LineNumber - 1), true);
                    edit.Focus();
                }
            }
        }

        private void BreakpointsControl_BreakpointStateChanged(object sender, BreakpointChangedEventArgs e)
        {
            var breakpoint = e.Breakpoint;
            if (breakpoint != null)
            {
                var edit = FindFile(breakpoint.FilePath) as IDebugEdit;
                if (edit != null)
                    edit.BreakpointStateChanged(breakpoint);
            }
        }

        private void BreakpointsControl_BreakpointDeleted(object sender, BreakpointChangedEventArgs e)
        {
            Action<Breakpoint> delete = (Breakpoint breakpoint) =>
            {
                if (breakpoint != null)
                {
                    var edit = FindFile(breakpoint.FilePath) as IDebugEdit;
                    if (edit != null)
                        edit.BreakpointDeleted(breakpoint);
                }
            };

            if (e.Breakpoints != null)
            {
                foreach (var breakpoint in e.Breakpoints)
                    delete(breakpoint);
            }
            else
                delete(e.Breakpoint);
        }

        private void DebugEdit_BreakpointToggle(object sender, EventArgs e)
        {
            breakpointsControl.UpdateList();
        }

        private async void DebugEdit_SetNextStatement(object sender, EventArgs e)
        {
            var line = ActiveSyntaxEdit.CurrentLine + 1;
            var result = await debugger.TrySetNextStatementAsync(line);
            if (result.IsFailure)
                MessageBox.Show("Cannot set next statement to line: " + line + ". " + result.ErrorMessage);
        }

        private void DebugEdit_AddToWatchClick(object sender, AddToWatchClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Expression))
            {
                watchesControl.AddWatch(e.Expression, true, true);
                ActivateWatchesTab();
            }
        }
    }
}
