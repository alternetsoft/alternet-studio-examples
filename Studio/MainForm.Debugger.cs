#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects;
using Alternet.Common.Projects.DotNet;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.Integration;

namespace AlternetStudio.Demo
{
    public partial class MainForm : IDebuggerUICommands
    {
        private IScriptDebugger debugger;
        private ExecutionPosition executionPosition;
        private StackFrame stackFrame;
        private bool switchToTopUserStackFrameNeeded;

        /// <summary>
        /// Gets or sets <see cref="StartDebuggingOptions"/> to use when
        /// the debugging starts while executing commands such as <see cref="Start"/> or <see cref="StepOver"/>.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StartDebuggingOptions StartDebuggingOptions { get; set; } = new DotNetStartDebuggingOptions();

        protected IScriptDebugger Debugger
        {
            get
            {
                if (debugger == null)
                {
                    debugger = new ScriptDebugger { ScriptRun = scriptRun };
                    debugger.DebuggingStarted += Debugger_DebuggingStarted;
                    debugger.DebuggingStopped += Debugger_DebuggingStopped;
                    debugger.DebuggerErrorOccurred += Debugger_DebuggerErrorOccurred;
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
                    threadsControl.Debugger = debugger;

                    callStackControl.FramesChanged += CallStack_StackFramesRetrieved;

                    debugger.EventsSyncAction = action => BeginInvoke(action);
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
            CompileAll();
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

        bool IDebuggerUICommands.StepOut()
        {
            CancelLocalsAndWatchesEvaluation();
            Debugger.StepOut();

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

        bool IDebuggerUICommands.RunToCursor()
        {
            var edit = ActiveSyntaxEdit;
            if (edit == null)
                return false;

            Debugger.SetRunToPositionBreakpoint(new RunToPositionBreakpoint(edit.FileName, edit.CurrentLine + 1));
            StartDebugging(false);

            return true;
        }

        protected virtual void StartDebugCore(bool breakOnStart)
        {
            Debugger.GeneratedModulesPath =
                Project.HasProject && !string.IsNullOrEmpty(Project.UserSettings.StartProgram) && Project.UserSettings.StartExternalProgram ?
                Path.GetDirectoryName(Project.UserSettings.StartProgram) :
                null;

            StartDebuggingOptions.BreakOnStart = breakOnStart;
            StartDebuggingOptions.HostApplication = Project.HasProject && Project.UserSettings.StartExternalProgram ? Project.UserSettings.StartProgram : null;
            StartDebuggingOptions.Arguments = Project.HasProject ? Project.UserSettings.CommandLineArgs : null;
            StartDebuggingOptions.MyCodeModules = GetMyCodeModules();

            Debugger.StartDebugging(StartDebuggingOptions);
        }

        protected void StartDebugging(bool breakOnStart)
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

                bool modified = Project.HasProject && Project.IsModified;
                if (SaveAllModifiedFiles() && SetScriptSource(modified))
                {
                    if (CompileIfNeeded())
                        StartDebugCore(breakOnStart);
                }
            }
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
            debugMenu.AllowedDebuggerCommands |= AllowedDebuggerCommands.RunToCursor;
        }

        private void ActivateWatchesTab()
        {
            bottomTabControl.SelectedTab = watchesTabPage;
        }

        private void LocalsControl_AddToWatchClick(object sender, Alternet.Scripter.Debugger.UI.AddToWatchClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Expression))
            {
                watchesControl.AddWatch(e.Expression, true, true);
                ActivateWatchesTab();
            }
        }

        private string[] GetMyCodeModules()
        {
            if (solution.IsEmpty)
                return null;

            IList<string> result = new List<string>();
            foreach (Project project in solution.Projects)
                result.Add(Path.Combine(Debugger.GeneratedModulesPath, project.ProjectName));

            return result.ToArray();
        }

        private void EvaluateExpression(bool evaluateCurrentException = false)
        {
            var symbol = ActiveSyntaxEdit != null ? ActiveSyntaxEdit.GetSymbolAtCursor() : string.Empty;
            using (var dialog = new EvaluateDialog(
                EvaluateDialog.CodeCompletionOptions.Custom((s, d, tb) => new EditorCodeCompletionController(s, d, tb)))
            {
                Debugger = Debugger,
                EvaluateCurrentException = evaluateCurrentException,
                Expression = symbol,
            })
            {
                dialog.WatchAdded += (o, e) => watchesControl.AddWatch(e.Expression);
                dialog.WatchAdded += (o, e) => ActivateWatchesTab();
                dialog.ShowDialog();
            }
        }

        private void ShowRunParametersDialog()
        {
            if (!Project.HasProject)
                return;

            using (var dialog = new RunParameters() { CommandLineArgs = Project.UserSettings.CommandLineArgs, StartProgram = Project.UserSettings.StartProgram, StartExternalProgram = Project.UserSettings.StartExternalProgram, IsDotnetCore = Project.TargetFramework != null })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Project.UserSettings.CommandLineArgs = dialog.CommandLineArgs;
                    Project.UserSettings.StartProgram = dialog.StartProgram;
                    Project.UserSettings.StartExternalProgram = dialog.StartExternalProgram;
                    Project.UserSettings.Save();
                }
            }
        }

        private void UpdateDebugButtons()
        {
            var state = Debugger.State;
            var hasProject = HasProject();
            var isEmpty = !(hasProject | ActiveSyntaxEdit != null);
            var isDebuggingStarted = Debugger.IsStarted;
            var isDebugging = isDebuggingStarted || state == DebuggerState.Startup;

            attachToProcessMenuItem.Enabled = !isDebugging;
            startWithoutDebugMenuItem.Enabled = !isEmpty && !isDebugging;
            tsbStartWithoutDebug.Enabled = !isEmpty && !isDebugging;
            runParametersMenuItem.Enabled = !isEmpty & !isDebugging && hasProject;

            bool debuggerEnabled = (Project != null && Project.HasProject) || ActiveSyntaxEdit != null;
            debuggerControlToolbar.Debugger = debuggerEnabled ? debugger : null;
            debugMenu.Debugger = debuggerEnabled ? debugger : null;
        }

        private async void UpdateDebugPanels()
        {
            try
            {
                var selectedTab = bottomTabControl.SelectedTab;
                if (selectedTab == callStackTabPage)
                {
                    if (Debugger.IsStarted)
                        callStackControl.SetFrames((await Debugger.GetStackFramesAsync()).Frames);
                }
                else if (selectedTab == localsTabPage)
                {
                    if (Debugger.IsStarted)
                        localsControl.EvaluateVariables((await Debugger.GetVariablesInScopeAsync()).VariableNames);
                }
                else if (selectedTab == watchesTabPage)
                {
                    watchesControl.EvaluateWatchExpressions();
                }
                else if (selectedTab == threadsTabPage)
                {
                    if (Debugger.IsStarted)
                        threadsControl.SetThreads((await Debugger.GetThreadsAsync()).Threads);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while updating debugger panels: " + e);
            }
        }

        private void CallStack_StackFramesRetrieved(object sender, EventArgs e)
        {
            if (switchToTopUserStackFrameNeeded)
            {
                switchToTopUserStackFrameNeeded = false;
                callStackControl.TrySwitchToTopUserStackFrame();
            }
        }

        private void SetAllEditorsReadOnlyValue(bool readOnly)
        {
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                var edit = GetEditor(tabPage);
                if (edit != null)
                    edit.ReadOnly = readOnly;
            }
        }

        private void Debugger_DebuggingStarted(object sender, DebuggingStartedEventArgs e)
        {
            executionPosition = null;
            UpdateDebugButtons();
            SetAllEditorsReadOnlyValue(true);
        }

        private void Debugger_DebuggerErrorOccurred(object sender, DebuggerErrorOccurredEventArgs e)
        {
            MessageBox.Show(this, e.Exception.ToString(), "Debugger Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (e.StopReason == ExecutionStopReason.Exception || e.StopReason == ExecutionStopReason.UnhandledException)
                DisplayDebuggerException(e);
        }

        private void DisplayDebuggerException(ExecutionStoppedEventArgs e)
        {
            var str = $"{e.Exception.ExceptionType}\n{e.Exception.Message}";
            using (var dlg = new DebuggerException(Debugger, str))
            {
                dlg.Text = e.StopReason == ExecutionStopReason.UnhandledException ? StringConsts.UnhandledException : StringConsts.DebuggerException;
                dlg.ExceptionEvaluationExpression = e.Exception.ExceptionEvaluationExpression;
                dlg.OnEvaluate += Dlg_OnEvaluate;
                DialogResult result = dlg.ShowDialog();
            }
        }

        private void Dlg_OnEvaluate(object sender, EventArgs e)
        {
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
            bottomTabControl.SelectedTab = callStackTabPage;
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

        private void CancelLocalsAndWatchesEvaluation()
        {
            Invoke((Action)(async () =>
            {
                var selectedTab = bottomTabControl.SelectedTab;
                if (selectedTab == localsTabPage)
                {
                    if (Debugger.IsStarted)
                        await localsControl.CancelEvaluationAsync();
                }
                else if (selectedTab == watchesTabPage)
                {
                    await watchesControl.CancelEvaluationAsync();
                }
            }));
        }

        private void BottomTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDebugPanels();
        }

        private void CallStackControl_CallStackClick(object sender, Alternet.Scripter.Debugger.UI.CallStackClickEventArgs e)
        {
            if (e.StackFrame != null)
            {
                var edit = OpenFile(e.StackFrame.File);
                if (edit != null)
                {
                    edit.MakeVisible(new Point(0, e.StackFrame.Line.Value - 1), true);
                    edit.Focus();
                }
            }
        }

        private void BreakpointsControl_BreakpointClick(object sender, BreakpointClickEventArgs e)
        {
            if (e.Breakpoint != null)
            {
                var edit = OpenFile(e.Breakpoint.FilePath);
                if (edit != null)
                {
                    edit.MakeVisible(new Point(0, e.Breakpoint.LineNumber - 1), true);
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

        private void AttachToProcessMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new AttachToProcessDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.Cancel)
                    return;

                Task.Run(() =>
                    Debugger.AttachToProcessAsync(
                        dialog.SelectedProcess.Id,
                        new DotNetStartDebuggingOptions
                        {
                            DisableJustMyCode = true,
                        })).Wait();
            }
        }
    }
}