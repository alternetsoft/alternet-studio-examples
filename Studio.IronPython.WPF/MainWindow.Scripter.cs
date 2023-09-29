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
using System.IO;
using System.Windows;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.IronPython.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI.Wpf;
using Alternet.Scripter.IronPython;

namespace AlternetStudio.IronPython.Wpf.Demo
{
    public partial class MainWindow
    {
        private IScriptRun scriptRun;

        protected virtual void RunScriptCore()
        {
            if (!scriptRun.ScriptHost.Compiled)
            {
                scriptRun.ScriptHost.Compile();
                if (scriptRun.ScriptHost.CompileFailed)
                {
                    ActivateErrorsTab();
                    return;
                }
            }

            debugger.RunScriptAsync();
        }

        private void InitializeScripter()
        {
            scriptRun = new ScriptRun();
        }

        private void ActivateErrorsTab()
        {
            bottomTabControl.SelectedIndex = ErrorsTabIndex;
        }

        private void ErrorsControl_ErrorClick(object sender, ErrorClickEventArgs e)
        {
            NavigateToCompilationError(e.Error);
        }

        private void StartWithoutDebugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RunScript();
        }

        private void NavigateToCompilationError(ScriptCompilationDiagnostic error)
        {
            if (!string.IsNullOrEmpty(error.FileName))
            {
                IScriptEdit edit = this.OpenFile(error.FileName);
                if (edit != null)
                {
                    edit.MakeVisible(new System.Drawing.Point(error.Column, error.Line), true);
                    edit.Focus();
                }
            }
        }

        private void UpdateScriptProject(IronPythonProject project)
        {
            scriptRun.ScriptSource.FromScriptProject(project.ProjectFileName);
        }

        private bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            if (ActiveSyntaxEdit != null)
            {
                string fileName = ActiveSyntaxEdit.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void RunScript()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();
                outputControl.Clear();
                RunScriptCore();
            }
        }

        private void Compile()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();
                scriptRun.Compile();

                if (scriptRun.ScriptHost.CompileFailed)
                    ActivateErrorsTab();
            }
        }
    }
}