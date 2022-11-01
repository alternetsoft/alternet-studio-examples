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
using System.Linq;
using System.Windows;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI.Wpf;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private IScriptRun scriptRun;

        protected IScriptRun ScriptRun => scriptRun;

        protected virtual void RunScriptCore()
        {
            scriptRun.RunProcess(Project.HasProject ? Project.UserSettings.CommandLineArgs : null);
            if (scriptRun.ScriptHost.CompileFailed)
                ActivateErrorsTab();
        }

        protected void ReportScriptCompilationErrors(ScriptCompilationDiagnostic[] errors)
        {
            Dispatcher.Invoke((Action)(() =>
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

            if (ActiveSyntaxEdit != null)
            {
                string fileName = ActiveSyntaxEdit.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    scriptRun.ScriptSource.WithDefaultReferences();
                    scriptRun.ScriptSource.WpfResources.AddRange(GetAllImageFilesInFormFolder(fileName));

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
            bottomTabControl.SelectedIndex = ErrorsTabIndex;
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
                    edit.MakeVisible(new System.Drawing.Point(error.Column, error.Line), true);
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
                    scriptRun.ScriptSource.References.Add(reference.ProjectName);
                scriptRun.ScriptSource.SearchPaths.Add(Path.GetDirectoryName(scriptRun.ScriptHost.ExecutableModulePath));
            }
        }

        private string[] GetAllImageFilesInFormFolder(string formFilePath)
        {
            var path = Path.GetDirectoryName(formFilePath);
            var extensions = new[] { "png", "gif", "jpg", "jpeg" };

            return extensions.SelectMany(x => Directory.GetFiles(path, "*." + x)).ToArray();
        }

        private void RunScript()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();
                RunScriptCore();
            }
        }

        private void RunParametersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowRunParametersDialog();
        }

        private void Compile()
        {
            if (SaveAllModifiedFiles() && SetScriptSource())
            {
                errorsControl.Clear();

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
                else
                {
                    designedComponentAssemblyManager.CopyOutput();
                }
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