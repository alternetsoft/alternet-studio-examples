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
using System.Drawing;
using System.IO;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IScriptRun scriptRun;

        protected virtual void RunScriptCore()
        {
            if (!scriptRun.Compiled)
                scriptRun.Compile();
            if (scriptRun.ScriptHost.CompileFailed)
                ActivateErrorsTab();

            scriptRun.RunAsync();
        }

        private void InitializeScripter()
        {
            scriptRun = new ScriptRun();
            InitDefaultHostAssemblies();
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            CodeEditExtensions.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void ActivateErrorsTab()
        {
            bottomTabControl.SelectedTab = errorsTabPage;
        }

        private void ErrorsControl_ErrorClick(object sender, ErrorClickEventArgs e)
        {
            NavigateToCompilationError(e.Error);
        }

        private void StartWithoutDebugMenuItem_Click(object sender, EventArgs e)
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
                    edit.MakeVisible(new Point(error.Column, error.Line), true);
                    edit.Focus();
                }
            }
        }

        private void UpdateScriptProject(TSProject project)
        {
            scriptRun.ScriptSource.FromScriptProject(project.ProjectFileName);

            scriptRun.ScriptSource.SearchPaths.Clear();
            scriptRun.ScriptSource.SearchPaths.Add(Path.GetDirectoryName(scriptRun.ScriptHost.ModulesDirectoryPath));
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
                    scriptRun.ScriptLanguage = GetScriptLanguage(fileName);
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
                else
                {
#if USEFORMDESIGNER
                    designedComponentAssemblyManager.CopyOutput();
                    foreach (var source in sourcesByFormId.Values)
                        source.DesignedComponentAssembly.NotifyAssemblyChanged();
#endif
                }
            }
        }

        private void CompileAll()
        {
            Compile();
        }
    }
}