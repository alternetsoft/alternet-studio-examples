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
using System.Drawing;
using System.IO;
using System.Linq;
using Alternet.Editor.Common;
using Alternet.Editor.IronPython;
using Alternet.Scripter;
using Alternet.Scripter.Debugger.UI;
using Alternet.Scripter.IronPython;
using Alternet.Syntax;

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

            debugger.RunScriptAsync();
        }

        private void InitializeScripter()
        {
            scriptRun = new ScriptRun();
        }

        private void ActivateErrorsTab()
        {
            bottomTabControl.SelectedTab = errorsTabPage;
        }

        private ScriptCompilationDiagnosticKind GetErrorSeverity(SyntaxErrorType errorType)
        {
            switch (errorType)
            {
                case SyntaxErrorType.Hidden:
                case SyntaxErrorType.Info:
                    return ScriptCompilationDiagnosticKind.Info;

                case SyntaxErrorType.Warning:
                    return ScriptCompilationDiagnosticKind.Warning;

                case SyntaxErrorType.Error:
                    return ScriptCompilationDiagnosticKind.Error;

                default:
                    throw new Exception();
            }
        }

        private ScriptCompilationDiagnostic[] GetErrors(string fileName, IList<ISyntaxError> errors)
        {
            if (errors == null)
                return null;

            return errors.Where(t => t.ErrorType != SyntaxErrorType.Hidden).Select(
                error =>
                {
                    return new ScriptCompilationDiagnostic
                    {
                        Kind = GetErrorSeverity(error.ErrorType),
                        FileName = fileName,
                        Line = error.Position.Y,
                        Column = error.Position.X,
                        Message = error.Description,
                        Code = error.ErrorCode,
                        InSource = !string.IsNullOrEmpty(fileName),
                    };
                }).ToArray();
        }

        private void UpdateErrors(ScriptCodeEdit edit)
        {
            ISyntaxParser parser = edit.Lexer as ISyntaxParser;
            if (parser == null)
                return;

            IList<ISyntaxError> list = new List<ISyntaxError>();
            parser.GetSyntaxErrors(list);
            errorsControl.Clear();
            errorsControl.AddCompilerErrors(GetErrors(parser.FileName, list));
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