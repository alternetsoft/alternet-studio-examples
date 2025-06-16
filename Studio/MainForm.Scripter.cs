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
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private IScriptRun scriptRun;

        protected IScriptRun ScriptRun => scriptRun;

        protected virtual void RunScriptCore()
        {
            if (CompileIfNeeded())
                debugger.RunScriptAsync(Project.HasProject ? Project.UserSettings.CommandLineArgs : null);
        }

        protected virtual bool CompileIfNeeded()
        {
            if (scriptRun.ScriptHost.Compiled)
                return true;
            return CompileCore();
        }

        protected virtual bool CompileCore()
        {
            bool result = true;
            result = scriptRun.ScriptHost.Compile(logger: new OutputWriter(outputControl));

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

        protected virtual bool SetScriptSource(bool modified)
        {
            if (Project.HasProject)
            {
                if (modified)
                    scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName, CurrentFramework);

                return true;
            }

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
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;
        }

        private void ActivateErrorsTab()
        {
            bottomTabControl.SelectedTab = errorsTabPage;
        }

        private ScriptCompilationDiagnosticKind GetErrorSeverity(DiagnosticSeverity severity)
        {
            switch (severity)
            {
                case DiagnosticSeverity.Hidden:
                case DiagnosticSeverity.Info:
                    return ScriptCompilationDiagnosticKind.Info;

                case DiagnosticSeverity.Warning:
                    return ScriptCompilationDiagnosticKind.Warning;

                case DiagnosticSeverity.Error:
                    return ScriptCompilationDiagnosticKind.Error;

                default:
                    throw new Exception();
            }
        }

        private bool IsSuppressedDiagnostic(Diagnostic diagnostic)
        {
            return diagnostic.IsSuppressed || diagnostic.Id.Contains("1701");
        }

        private bool IsCompatiblePlatform(Diagnostic diagnostic)
        {
            return !diagnostic.Location.IsInSource || FileBelongsToProjectFramework(Project, diagnostic.Location.SourceTree.FilePath);
        }

        private bool IsAutoGenerateCode(Diagnostic diagnostic)
        {
            return diagnostic.Location.IsInSource && !FileBelongsToProject(Project, diagnostic.Location.SourceTree.FilePath);
        }

        private ScriptCompilationDiagnostic[] GetErrors(ImmutableArray<Diagnostic> errors)
        {
            if (errors == null)
                return null;
            return errors.Where(t => t.Severity != DiagnosticSeverity.Hidden && !IsSuppressedDiagnostic(t) && !IsAutoGenerateCode(t) && IsCompatiblePlatform(t)).Select(
                error =>
                {
                    bool inSource = error.Location.IsInSource;
                    LinePosition position = inSource ? error.Location.GetLineSpan().StartLinePosition : LinePosition.Zero;

                    return new ScriptCompilationDiagnostic
                    {
                        Kind = GetErrorSeverity(error.Severity),
                        FileName = inSource ? error.Location.SourceTree.FilePath : string.Empty,
                        Line = inSource ? position.Line : -1,
                        Column = inSource ? position.Character : -1,
                        Message = error.GetMessage(),
                        Code = error.Id,
                        InSource = inSource,
                    };
                }).ToArray();
        }

        private bool FilterError(ScriptCompilationDiagnostic error)
        {
            return error.InSource != null ? error.InSource.Value : false;
        }

        private async void UpdateErrors(IScriptEdit edit)
        {
            var document = edit.Document() as Document;
            if (document == null)
                return;
            var compilation = await document.Project.GetCompilationAsync();
            errorsControl.Clear(FilterError);
            errorsControl.AddCompilerErrors(GetErrors(compilation.GetDiagnostics()));
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
            bool modified = Project.HasProject && Project.IsModified;
            if (SaveAllModifiedFiles() && SetScriptSource(modified))
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
            bool modified = Project.HasProject && Project.IsModified;
            if (SaveAllModifiedFiles() && SetScriptSource(modified))
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

    internal class OutputWriter : StringWriter
    {
        private Output output;

        public OutputWriter(Output output)
        {
            this.output = output;
        }

        public override void WriteLine(string line)
        {
            base.WriteLine(line);
            output.CustomLog(line);
        }
    }
}