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
            scriptRun.RunProcess(Project.HasProject ? Project.UserSettings.CommandLineArgs : null);
            if (scriptRun.ScriptHost.CompileFailed)
                ActivateErrorsTab();
        }

        protected virtual void CompileCore()
        {
            bool generateModulesOnDisk = scriptRun.ScriptHost.GenerateModulesOnDisk;
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            try
            {
                scriptRun.Compile();
            }
            finally
            {
                scriptRun.ScriptHost.GenerateModulesOnDisk = generateModulesOnDisk;
            }

            if (scriptRun.ScriptHost.CompileFailed)
                ActivateErrorsTab();
#if USEFORMDESIGNER
            else
            {
                designedComponentAssemblyManager.CopyOutput();
                foreach (var source in sourcesByFormId.Values)
                    source.DesignedComponentAssembly.NotifyAssemblyChanged();
            }
#endif
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
#if USEFORMDESIGNER
            var designer = ActiveFormDesigner;
            if (designer != null)
                fileName = designer.Source.UserCodeFileName;
#endif
            if (!string.IsNullOrEmpty(fileName))
            {
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    scriptRun.ScriptSource.WithDefaultReferences();
                    scriptRun.ScriptSource.Imports.Clear();
#if USEFORMDESIGNER
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
#endif

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
                    scriptRun.ScriptSource.References.Add(reference.ProjectName);
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

        private void RunParamentersMenuItem_Click(object sender, EventArgs e)
        {
            ShowRunParametersDialog();
        }

        private void Compile()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();
                CompileCore();
            }
        }

        private void CompileAll()
        {
            if (!solution.IsEmpty)
            {
                foreach (var prj in solution.Projects)
                {
                    if (prj.ProjectReferences.Count == 0)
                    {
                        UpdateScriptProject(prj);
                        Compile();
                    }
                }

                foreach (var prj in solution.Projects)
                {
                    if (prj.ProjectReferences.Count > 0)
                    {
                        UpdateScriptProject(prj);
                        Compile();
                    }
                }

                if (Project.HasProject)
                    UpdateScriptProject(Project);
            }
            else
                Compile();
        }
    }
}