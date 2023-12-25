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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IScriptRun scriptRun;

        protected IScriptRun ScriptRun => scriptRun;

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

            debugger.RunScriptAsync(Project.HasProject ? Project.UserSettings.CommandLineArgs : null);
        }

        protected virtual bool CompileCore()
        {
            bool result = true;
            bool generateModulesOnDisk = scriptRun.ScriptHost.GenerateModulesOnDisk;
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            try
            {
                result = scriptRun.Compile();
            }
            finally
            {
                scriptRun.ScriptHost.GenerateModulesOnDisk = generateModulesOnDisk;
            }

            if (scriptRun.ScriptHost.CompileFailed)
            {
                ActivateErrorsTab();
            }
            else
            {
                designedComponentAssemblyManager.CopyOutput();
                foreach (var source in sourcesByFormId.Values)
                    source.DesignedComponentAssembly.NotifyAssemblyChanged();
            }

            return result;
        }

        protected void ReportScriptCompilationErrors(ScriptCompilationDiagnostic[] errors)
        {
            Invoke((Action)(() =>
            {
                errorsControl.Clear();
                errorsControl.AddCompilerErrors(errors);
                ActivateErrorsTab();
            }));
        }

        protected virtual bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            string fileName = (ActiveSyntaxEdit != null) ? ActiveSyntaxEdit.FileName : string.Empty;
            var designer = ActiveFormDesigner;
            if (designer != null)
                fileName = designer.Source.UserCodeFileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    scriptRun.ScriptSource.WithDefaultReferences();
                    scriptRun.ScriptSource.Imports.Clear();
                    var imports = GetDesignerImportedNamespaces(fileName);
                    if (imports != null)
                    {
                        foreach (string nspace in imports.Namespaces)
                        {
                            scriptRun.ScriptSource.Imports.Add(nspace);
                        }
                    }

                    if (designer != null)
                    {
                        var referencedAssemblies = designer.ReferencedAssemblies;
                        foreach (string reference in referencedAssemblies.AssemblyNames)
                        {
                            if (!scriptRun.ScriptSource.References.Contains(reference))
                                scriptRun.ScriptSource.References.Add(reference);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private void InitializeScripter()
        {
            scriptRun = new ScriptRun() { ScriptLanguage = ScriptLanguage.CSharp, Platform = ScriptPlatform.Auto, ScriptMode = ScriptMode.Debug };
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

        private void UpdateScriptProject(DotNetProject project)
        {
            scriptRun.ScriptSource.FromScriptProject(project.ProjectFileName);
            scriptRun.ScriptSource.SearchPaths.Clear();

            if (project.ProjectReferences.Count > 0)
            {
                foreach (var reference in project.ProjectReferences)
                {
                    if (!string.IsNullOrEmpty(reference.ProjectName))
                        scriptRun.ScriptSource.References.Add(reference.ProjectName);
                    else
                    if (!string.IsNullOrEmpty(reference.ProjectPath))
                    {
                        // ideally we need to load assemblyname from the project
                        scriptRun.ScriptSource.References.Add(Path.GetFileNameWithoutExtension(reference.ProjectPath));
                    }
                }

                scriptRun.ScriptSource.SearchPaths.Add(Path.GetDirectoryName(scriptRun.ScriptHost.ExecutableModulePath));
            }
        }

        private void RunScript()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();
                RunScriptCore();
            }
        }

        private void RunParametersMenuItem_Click(object sender, EventArgs e)
        {
            ShowRunParametersDialog();
        }

        private bool Compile()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                if (errorsControl.UpdateCount == 0)
                    errorsControl.Clear();
                return CompileCore();
            }

            return false;
        }

        private bool CompileAll()
        {
            bool result = true;
            errorsControl.Clear();
            errorsControl.BeginUpdate();
            try
            {
                if (!solution.IsEmpty)
                {
                    IList<DotNetProject> projects = new List<DotNetProject>();

                    foreach (var prj in solution.Projects)
                    {
                        if (prj.ProjectReferences.Count == 0)
                            projects.Add(prj);
                    }

                    foreach (var prj in solution.Projects)
                    {
                        if (prj.ProjectReferences.Count > 0)
                            projects.Add(prj);
                    }

                    foreach (var prj in projects)
                    {
                        UpdateScriptProject(prj);
                        if (!Compile())
                            result = false;
                    }

                    if (Project.HasProject)
                        UpdateScriptProject(Project);
                }
                else
                {
                    if (!Compile())
                        result = false;
                }
            }
            finally
            {
                errorsControl.EndUpdate();
            }

            return result;
        }
    }
}