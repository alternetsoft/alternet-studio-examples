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
using System.Drawing;
using System.IO;
using Alternet.Editor.Common;
using Alternet.Editor.IronPython;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.IronPython;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IScriptRun scriptRun;

        protected IScriptRun ScriptRun => scriptRun;

        protected virtual void RunScriptCore()
        {
            scriptRun.RunAsync();
            if (scriptRun.ScriptHost.CompileFailed)
                ActivateErrorsTab();
        }

        private void InitializeScripter()
        {
            scriptRun = new ScriptRun();
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

        private void UpdateScriptProject(IronPythonProject project)
        {
            scriptRun.ScriptSource.FromScriptProject(project.ProjectFileName);
            scriptRun.ScriptSource.ReferencesSearchPaths.Clear();
            if (project.References.Count > 0)
            {
                foreach (var reference in project.References)
                {
                    if (!scriptRun.ScriptSource.References.Contains(reference.FullName))
                        scriptRun.ScriptSource.References.Add(reference.FullName);
                }
            }

            if (project.ImportedNamespaces.Count > 0)
            {
                foreach (var import in project.ImportedNamespaces)
                {
                    if (!scriptRun.ScriptSource.Imports.Contains(import))
                        scriptRun.ScriptSource.Imports.Add(import);
                }
            }

            if (project.ProjectReferences.Count > 0)
            {
                foreach (var reference in project.ProjectReferences)
                    scriptRun.ScriptSource.References.Add(reference.ProjectName);
                scriptRun.ScriptSource.ReferencesSearchPaths.Add(Path.GetDirectoryName(scriptRun.ScriptHost.ModulesDirectoryPath));
            }
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

                    // todo
                    scriptRun.ScriptSource.Imports.Clear();
                    var imports = GetImportedNamespaces(fileName);
                    if (imports != null)
                    {
                        foreach (string nspace in imports)
                        {
                            scriptRun.ScriptSource.Imports.Add(nspace);
                        }
                    }

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
                    foreach (var source in sourcesByFormId.Values)
                        source.DesignedComponentAssembly.NotifyAssemblyChanged();
                }
            }
        }

        private void CompileAll()
        {
            Compile();
        }
    }
}