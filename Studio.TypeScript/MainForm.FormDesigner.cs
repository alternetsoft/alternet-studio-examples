﻿#region Copyright (c) 2016-2025 Alternet Software

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
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Common;
using Alternet.Editor.TextSource;
using Alternet.Editor.TypeScript;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter;
using Alternet.Scripter.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private Dictionary<TabPage, IFormDesignerControl> formDesigners = new Dictionary<TabPage, IFormDesignerControl>();
        private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();

        private Alternet.FormDesigner.WinForms.PropertyGridControl propertyGridControl;
        private Alternet.FormDesigner.WinForms.ToolboxControl toolboxControl;

        private IFormDesignerControl ActiveFormDesigner
        {
            get
            {
                if (editorsTabControl.TabCount == 0 || editorsTabControl.SelectedTab == null)
                    return null;

                IFormDesignerControl designer;
                if (!formDesigners.TryGetValue(editorsTabControl.SelectedTab, out designer))
                    return null;

                return designer;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AutoSaveToolbox();
            base.OnClosing(e);
        }

        private static void EnsureDotNetCoreReferencesAdded(IList<string> references)
        {
            string GetAssemblyName(string x) => File.Exists(x) ? Path.GetFileNameWithoutExtension(x) : x;

            var defaultReferences = MinimalDotNetCoreDependenciesService.GetReferences(Alternet.Common.TechnologyEnvironment.WindowsForms, useRuntimeAssemblies: false, needFullPaths: true, useDesignReferences: true);
            foreach (string reference in defaultReferences)
            {
                if (references.Contains(reference))
                    continue;

                var assemblyName = GetAssemblyName(reference);
                var existingIndex = references.IndexOfFirst(x => GetAssemblyName(x).Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
                if (existingIndex != -1)
                    references.RemoveAt(existingIndex);

                references.Add(reference);
            }
        }

        private void InitializeFormDesigner()
        {
            CreateFormDesignerControls();
            UpdateDesignerControls();
            AutoLoadToolbox();
        }

        private void CreateFormDesignerControls()
        {
            propertyGridControl = new PropertyGridControl();
            toolboxControl = new ToolboxControl();
            newFormMenuItem.Visible = true;
            newFormMenuItem.Click += new System.EventHandler(NewFormMenuItem_Click);
            viewCodeMenuItem.Visible = true;
            viewCodeMenuItem.Click += new EventHandler(ViewCodeMenuItem_Click);
            viewDesignerMenuItem.Visible = true;
            viewDesignerMenuItem.Click += new EventHandler(ViewDesignerMenuItem_Click);

            rightTabControl.Controls.Add(propertiesTabPage);
            rightTabControl.Controls.Add(toolboxTabPage);

            // propertyGridControl
            this.propertyGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControl.Location = new System.Drawing.Point(3, 3);
            this.propertyGridControl.Name = "propertyGridControl";
            this.propertyGridControl.Size = new System.Drawing.Size(633, 448);
            this.propertyGridControl.TabIndex = 0;

            // toolboxControl
            this.toolboxControl.AutoScroll = true;
            this.toolboxControl.BackColor = System.Drawing.Color.FromArgb((int)((byte)245), (int)(byte)245, (int)((byte)245));
            this.toolboxControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolboxControl.Location = new System.Drawing.Point(3, 3);
            this.toolboxControl.Name = "toolboxControl";
            this.toolboxControl.Size = new System.Drawing.Size(633, 448);
            this.toolboxControl.TabIndex = 0;

            propertiesTabPage.Controls.Add(propertyGridControl);
            toolboxTabPage.Controls.Add(toolboxControl);
        }

        private void AutoSaveToolbox()
        {
            var toolboxAutoSaveFileName = GetToolboxAutoSaveFileName();
            using (var fs = new FileStream(toolboxAutoSaveFileName, FileMode.Create))
                toolboxControl.Save(fs);
        }

        private string GetToolboxAutoSaveFileName()
        {
            var directory = Path.Combine(Path.GetTempPath(), "Alternet.Studio.TypeScript.Demo", "dotnet-" + Environment.Version.ToString(2));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return Path.Combine(directory, "ToolboxAutoSave.xml");
        }

        private void AutoLoadToolbox()
        {
            var toolboxAutoSaveFileName = GetToolboxAutoSaveFileName();
            if (File.Exists(toolboxAutoSaveFileName))
            {
                using (var fs = new FileStream(toolboxAutoSaveFileName, FileMode.Open))
                {
                    try
                    {
                        toolboxControl.Load(fs);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error autoloading the toolbox - resetting. Exception: " + e);
                        toolboxControl.Reset();
                    }
                }
            }
        }

        private void NewFormMenuItem_Click(object sender, EventArgs e)
        {
            var project = Project;
            bool addToProject = project != null && project.HasProject;

            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = FindUniqueName("Form", ".ts");

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var userCodeFileName = saveFileDialog.FileName;

            var source = new FormDesignerDataSource(userCodeFileName);
            FormFilesUtility.CreateFormFiles(source, FormFilesUtility.CreateFormFilesOptions.TypeScript);
            if (!addToProject)
            {
                OpenFile(source.UserCodeFileName);
                OpenFile(source.DesignerFileName);
                OpenDesigner(source.UserCodeFileName);
            }
            else
            {
                project.BeginUpdate();
                try
                {
                    project.AddFile(source.UserCodeFileName);
                    project.AddFile(source.DesignerFileName);
                    project.AddFile(source.DefaultResourceFileName);
                    OpenFile(source.UserCodeFileName);
                    OpenFile(source.DesignerFileName);
                    OpenDesigner(source.UserCodeFileName);
                }
                finally
                {
                    project.EndUpdate();
                }
            }
        }

        private Tuple<IFormDesignerControl, TabPage> FindDesigner(string fileName)
        {
            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabPage tabPage in editorsTabControl.TabPages)
            {
                IFormDesignerControl designer;

                if (!formDesigners.TryGetValue(tabPage, out designer))
                    continue;

                var path = designer.Source.UserCodeFileName;
                path = new Uri(path).LocalPath;
                if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                {
                    return new Tuple<IFormDesignerControl, TabPage>(designer, tabPage);
                }
            }

            return null;
        }

        private bool IsFormFile(string fileName, out string formId)
        {
            string designFile;
            return IsFormFile(fileName, out designFile, out formId);
        }

        private bool IsFormFile(string fileName, out string designFile, out string formId)
        {
            formId = null;
            designFile = string.Empty;

            var pathWithoutExtension = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));

            const string DesignerSuffix = ".Designer";

            if (pathWithoutExtension.EndsWith(DesignerSuffix, StringComparison.OrdinalIgnoreCase) &&
                 ((!Path.GetFileName(fileName).StartsWith("Resources", StringComparison.OrdinalIgnoreCase)) &&
                  (!Path.GetFileName(fileName).StartsWith("Application", StringComparison.OrdinalIgnoreCase))))
            {
                designFile = fileName;
                pathWithoutExtension = pathWithoutExtension.Substring(0, pathWithoutExtension.Length - DesignerSuffix.Length);
            }
            else
            {
                designFile = pathWithoutExtension + DesignerSuffix + Path.GetExtension(fileName);

                if (!File.Exists(designFile))
                    return false;
            }

            formId = pathWithoutExtension + Path.GetExtension(fileName);
            return true;
        }

        private EditorFormDesignerDataSource GetDesignerSource(string formId)
        {
            return GetDesignerSource(formId, true);
        }

        private EditorFormDesignerDataSource GetDesignerSource(string formId, bool createNew)
        {
            EditorFormDesignerDataSource ds;
            if (!sourcesByFormId.TryGetValue(formId, out ds) && createNew)
            {
                ds = new EditorFormDesignerDataSource(
                    formId,
                    fileName =>
                    {
                        var source = new Alternet.FormDesigner.Integration.FormDesignerTextSource();
                        source.LoadFile(fileName);
                        source.FileName = fileName;
                        return source;
                    },
                    null/*GetAssemblyResources(formId)*/);

                sourcesByFormId.Add(formId, ds);
            }

            return ds;
        }

        private void Designer_DesignedContentChanged(object sender, EventArgs e)
        {
            var designer = sender as FormDesignerControl;

            if (designer != null)
            {
                var source = (EditorFormDesignerDataSource)designer.Source;

                UpdateDesignPage(designer.Parent as TabPage, source.UserCodeFileName, source.IsModified);

                if (FindFile(source.DesignerFileName) == null)
                    CodeEditExtensions.ReplaceCodeFile(Path.GetExtension(source.DesignerFileName), source.DesignerFileName, source.DesignerTextSource.Text);
            }
        }

        private void Designer_CompilerErrorClick(object sender, DesignerCompilerErrorClickEventArgs e)
        {
            NavigateToCompilationError(GetDesignerLoadingCompilerError(e.Error));
        }

        private void Designer_LoadingErrorOccured(object sender, DesignerLoadingErrorEventArgs e)
        {
            errorsControl.AddCompilerErrors(GetDesignerLoadingCompilerErrors(e));
            ActivateErrorsTab();
        }

        private ScriptCompilationDiagnostic[] GetDesignerLoadingCompilerErrors(DesignerLoadingErrorEventArgs e)
        {
            if (e.Errors == null || !e.Errors.Any())
            {
                return new[]
                {
                    new ScriptCompilationDiagnostic
                    {
                        Code = "Form Designer",
                        Message = "Designer loading error: " + e.Message,
                    },
                };
            }

            return e.Errors.Select(x => GetDesignerLoadingCompilerError(x)).ToArray();
        }

        private IFormDesignerControl OpenDesigner(string fileName)
        {
            var designerTab = FindDesigner(fileName);
            if (designerTab != null)
            {
                editorsTabControl.SelectedTab = designerTab.Item2;
                return designerTab.Item1;
            }

            var designer = new FormDesignerControl
            {
                Dock = DockStyle.Fill,
                ReferencedAssemblies = GetDesignerReferencedAssemblies(fileName),
                AutoAddComponentAssemblyReferences = false,
            };

            designer.LoadingErrorOccured += Designer_LoadingErrorOccured;
            designer.CompilerErrorClick += Designer_CompilerErrorClick;
            designer.DesignedContentChanged += Designer_DesignedContentChanged;
            designer.ComponentAdded += Designer_ComponentAdded;
            designer.Source = GetDesignerSource(fileName);

            var page = new TabPage("Design: " + Path.GetFileName(fileName));
            page.ToolTipText = fileName;

            editorsTabControl.TabPages.Add(page);

            designer.NavigateToUserMethodRequested += FormDesignerControl_NavigateToUserMethodRequested;
            designer.CommandStateChanged += Designer_CommandStateChanged;

            designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).DesignerTextSource).Edits.Any();

            page.Controls.Add(designer);
            formDesigners.Add(page, designer);

            editorsTabControl.SelectedTab = page;
            UpdateCodeNavigation();
            UpdateDesignerControls();

            return designer;
        }

        private void UpdateFormFiles(IScriptEdit edit, EditorFormDesignerDataSource ds, string newName)
        {
            if (ds != null)
            {
                var newSource = new FormDesignerDataSource(newName);

                new FileInfo(ds.UserCodeFileName).CopyTo(newSource.UserCodeFileName, true);
                new FileInfo(ds.DesignerFileName).CopyTo(newSource.DesignerFileName, true);
                foreach (var resource in ds.ResourceTextSources)
                    new FileInfo(ds.GetResourceFileName(resource.Item1)).CopyTo(newSource.GetResourceFileName(resource.Item1), true);

                sourcesByFormId.Remove(ds.UserCodeFileName);

                foreach (TabPage tabPage in editorsTabControl.TabPages)
                {
                    IScriptEdit editor = GetEditor(tabPage);
                    if (editor != null)
                    {
                        if (editor.FileName.Equals(ds.DesignerFileName))
                        {
                            editor.FileName = newSource.DesignerFileName;
                            UpdatePage(tabPage, newSource.DesignerFileName, editor.Modified);
                        }

                        if (editor.FileName.Equals(ds.DefaultResourceFileName))
                        {
                            editor.SaveFile(newSource.DefaultResourceFileName);
                            UpdatePage(tabPage, newSource.DefaultResourceFileName, editor.Modified);
                        }
                    }
                }

                var designerTab = FindDesigner(ds.UserCodeFileName);
                if (designerTab != null)
                {
                    UpdateDesigner(designerTab, newSource.UserCodeFileName);
                }

                edit.FileName = newName;
                UpdatePage(editorsTabControl.SelectedTab, newName, edit.Modified);
            }
        }

        private void RemoveDesigner(Tuple<IFormDesignerControl, TabPage> designerTab)
        {
            if (designerTab != null)
            {
                CloseDesigner(designerTab.Item1);
                formDesigners.Remove(designerTab.Item2);
                editorsTabControl.TabPages.Remove(designerTab.Item2);
                if (editors.ContainsKey(designerTab.Item2))
                    editors.Remove(designerTab.Item2);
            }
        }

        private void CloseDesigner(IFormDesignerControl designer)
        {
            designer.NavigateToUserMethodRequested -= FormDesignerControl_NavigateToUserMethodRequested;
            designer.CommandStateChanged -= Designer_CommandStateChanged;
            designer.LoadingErrorOccured -= Designer_LoadingErrorOccured;
            designer.CompilerErrorClick -= Designer_CompilerErrorClick;
            var userFileName = designer.Source.UserCodeFileName;
            if (sourcesByFormId.ContainsKey(userFileName))
                sourcesByFormId.Remove(userFileName);
            ((FormDesignerControl)designer).Dispose();
        }

        private void Designer_CommandStateChanged(object sender, EventArgs e)
        {
            UpdateDesignerButtons();
        }

        private void SaveDesignerFiles(TabPage tabPage, IFormDesignerControl designer)
        {
            var source = (EditorFormDesignerDataSource)designer.Source;
            source.Save();
            UpdateDesignPage(tabPage, designer.Source.UserCodeFileName, designer.Source.IsModified);
        }

        private void UpdateDesignerControls()
        {
            var designer = ActiveFormDesigner;

            toolboxControl.FormDesignerControl = designer;
            propertyGridControl.FormDesignerControl = designer;

            bool haveActiveDesigner = designer != null;

            UpdateDesignerButtons();

            if (haveActiveDesigner)
            {
                ReloadDesignerIfNeeded(designer);
            }
        }

        private void ReloadDesignerIfNeeded(IFormDesignerControl designer)
        {
            try
            {
                var designerFileName = designer.Source.DesignerFileName;
                if (this.editedCodeFiles.Contains(designerFileName, StringComparer.OrdinalIgnoreCase))
                {
                    designer.Reload();
                    editedCodeFiles.Remove(designerFileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DesignerReferencedAssemblies GetDesignerReferencedAssemblies(string fileName)
        {
            var project = GetProject(fileName);
            if (project != null && project.HasProject)
            {
                IList<string> references = project.AssemblyNames;
#if NET6_0_OR_GREATER
                EnsureDotNetCoreReferencesAdded(references);
#endif
                return new DesignerReferencedAssemblies(references.ToArray());
            }

            return DesignerReferencedAssemblies.DefaultForTypeScript;
        }

        private void UpdateDesigner(Tuple<IFormDesignerControl, TabPage> designerTab, string fileName)
        {
            if (designerTab != null)
            {
                CloseDesigner(designerTab.Item1);
                formDesigners.Remove(designerTab.Item2);

                var designer = new FormDesignerControl
                {
                    Dock = DockStyle.Fill,
                    ReferencedAssemblies = GetDesignerReferencedAssemblies(fileName),
                    AutoAddComponentAssemblyReferences = false,
                };

                designer.LoadingErrorOccured += Designer_LoadingErrorOccured;
                designer.CompilerErrorClick += Designer_CompilerErrorClick;
                designer.DesignedContentChanged += Designer_DesignedContentChanged;
                designer.ComponentAdded += Designer_ComponentAdded;
                designer.Source = GetDesignerSource(fileName);

                designerTab.Item2.Text = "Design: " + Path.GetFileName(fileName);
                designerTab.Item2.ToolTipText = fileName;

                designer.NavigateToUserMethodRequested += FormDesignerControl_NavigateToUserMethodRequested;
                designer.CommandStateChanged += Designer_CommandStateChanged;

                designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).DesignerTextSource).Edits.Any();

                designerTab.Item2.Controls.Add(designer);
                formDesigners.Add(designerTab.Item2, designer);
            }
            else
                OpenDesigner(fileName);
        }

        private void Designer_ComponentAdded(object sender, ComponentEventArgs e)
        {
            var designer = sender as FormDesignerControl;

            if (designer.IsBeingLoaded)
                return;

            designer.ReferencedAssemblies = ComponentAssemblyReferenceAdder.TryAddComponentAssemblyReferences(
                e.Component,
                designer.ReferencedAssemblies);

            var project = GetProject(designer.Source);

            if (project != null && project.HasProject)
            {
                if (AddReferencesToProject(project, ComponentAssemblyReferenceAdder.TryAddComponentAssemblyReferences(e.Component, project.AssemblyNames)))
                {
                    project.UpdateHostConfiguration(
                        CodeEditExtensions.DefaultProject,
                        scriptRun.ScriptHost.HostItemsConfiguration);

                    UpdateProjectExplorer();
                }
            }
        }

        private ScriptCompilationDiagnostic GetDesignerLoadingCompilerError(DesignerCompilerError error)
        {
            return new ScriptCompilationDiagnostic
            {
                Code = "Form Designer",
                FileName = error.FilePath,
                Line = error.LineNumber,
                Column = error.CharacterNumber,
                Message = error.Message,
                Kind = ScriptCompilationDiagnosticKind.Error,
                InSource = !string.IsNullOrEmpty(error.FilePath),
            };
        }

        private void UpdateDesignPage(TabPage page, string fileName, bool isModified = false)
        {
            if (page == null)
                return;
            string pageText = "Design: " + Path.GetFileName(fileName);
            pageText = isModified ? pageText + "*" : pageText;
            if (page.Text != pageText)
                page.Text = pageText;

            if (page.ToolTipText != fileName)
                page.ToolTipText = fileName;
        }

        private void AddDesignFileForParsing(string fileName)
        {
            var designFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".Designer" + Path.GetExtension(fileName));

            if (new FileInfo(designFile).Exists && FindFile(designFile) == null)
                CodeEditExtensions.RegisterCode(Path.GetExtension(fileName), new string[] { designFile });
        }

        private void RemoveDesignFileForParsing(string fileName)
        {
            var designFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".Designer" + Path.GetExtension(fileName));

            if (new FileInfo(designFile).Exists && FindFile(designFile) == null)
                CodeEditExtensions.UnregisterCode(Path.GetExtension(fileName), new string[] { designFile });
        }

        private void UpdateDesignerButtons()
        {
            var designer = ActiveFormDesigner;

            if (designer == null)
                return;

            bool enabled = true;

            saveMenuItem.Enabled = enabled;
            selectAllMenuItem.Enabled = enabled;
            closeFileMenuItem.Enabled = enabled;

            cutMenuItem.Enabled = enabled;
            copyMenuItem.Enabled = enabled;
            pasteMenuItem.Enabled = enabled;

            deleteMenuItem.Enabled = enabled;

            undoMenuItem.Enabled = enabled && designer.DesignerCommands.CanUndo;
            redoMenuItem.Enabled = enabled && designer.DesignerCommands.CanRedo;

            viewCodeMenuItem.Enabled = enabled;

            saveToolButton.Enabled = enabled;

            cutToolButton.Enabled = enabled;
            copyToolButton.Enabled = enabled;
            pasteToolButton.Enabled = enabled;

            undoToolButton.Enabled = enabled && designer.DesignerCommands.CanUndo;
            redoToolButton.Enabled = enabled && designer.DesignerCommands.CanRedo;
        }

        private void FormDesignerControl_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            if (designer != ActiveFormDesigner)
                return;

            var source = (EditorFormDesignerDataSource)designer.Source;

            OpenFile(source.UserCodeFileName);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);
        }

        private void ViewCodeMenuItem_Click(object sender, EventArgs e)
        {
            var activeDesigner = ActiveFormDesigner;
            if (activeDesigner == null)
                return;

            OpenFile(activeDesigner.Source.UserCodeFileName);
        }

        private void ViewDesignerMenuItem_Click(object sender, EventArgs e)
        {
            var activeEditor = ActiveSyntaxEdit;
            if (activeEditor == null)
                return;

            var fileName = activeEditor.FileName;
            if (FormFilesUtility.CheckIfFormFilesExist(fileName))
                OpenDesigner(fileName);
        }
    }
}