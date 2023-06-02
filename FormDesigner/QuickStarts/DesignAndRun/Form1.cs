#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.Editor.TextSource;
using Alternet.FormDesigner.Integration;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter;

namespace DesignAndRun
{
    public partial class Form1 : Form
    {
        private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();
        private HashSet<string> editedCodeFiles = new HashSet<string>();
        private Dictionary<Tuple<string, string>, TabPage> codeTabPages = new Dictionary<Tuple<string, string>, TabPage>();
        private IScriptRun scriptRun;

        public Form1()
        {
            InitializeComponent();
        }

        private IScriptEdit ActiveEditor
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Tag as IScriptEdit;
            }
        }

        private FormDesignerControl ActiveFormDesigner
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Tag as FormDesignerControl;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = PromptToSaveUnsavedDesigns();

            base.OnClosing(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string folder = GetTestFilesDirectoryPath();
            if (Directory.Exists(folder))
            {
                var designerFiles = Directory.GetFiles(folder, "*.Designer.*", SearchOption.AllDirectories);

                foreach (var designerFile in designerFiles)
                {
                    var extension = Path.GetExtension(designerFile);

                    var userCodeFile = designerFile.Replace(".Designer" + extension, extension);
                    OpenAllFormFiles(userCodeFile);
                    break;
                }
            }

            OnSelectedContentTabChanged();
        }

        private bool PromptToSaveUnsavedDesigns()
        {
            var designers = contentTabControl.TabPages.Cast<TabPage>().Select(x => x.Tag).OfType<FormDesignerControl>();
            foreach (var designer in designers)
            {
                if (designer.Source.IsModified)
                {
                    DialogResult result = PromptToSaveFile("designer", designer.Source.DesignerFileName);

                    if (result == DialogResult.Yes)
                        SaveDesignerFiles(designer);
                    else if (result == DialogResult.No)
                        continue;
                    else if (result == DialogResult.Cancel)
                        return true;
                    else
                        throw new Exception();
                }
            }

            var editors = contentTabControl.TabPages.Cast<TabPage>().Select(x => x.Tag).OfType<IScriptEdit>();
            foreach (var editor in editors)
            {
                if (editor.Modified)
                {
                    DialogResult result = PromptToSaveFile("code", (string)((Control)editor).Tag);

                    if (result == DialogResult.Yes)
                        SaveEditorFile(editor);
                    else if (result == DialogResult.No)
                        continue;
                    else if (result == DialogResult.Cancel)
                        return true;
                    else
                        throw new Exception();
                }
            }

            return false;
        }

        private DialogResult PromptToSaveFile(string fileType, string fileName)
        {
            var message = string.Format(
                "Do you want to save changes to the {0} file: {1}",
                fileType,
                fileName);

            var result = MessageBox.Show(
                this,
                message,
                "Designer Demo",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            return result;
        }

        private TabPage FindPage(string formId, string fileName)
        {
            var key = new Tuple<string, string>(formId, fileName);

            TabPage page;
            if (codeTabPages.TryGetValue(key, out page))
                return page;
            return null;
        }

        private TabPage OpenCode(string formId, string fileName)
        {
            var page = new TabPage("Code: " + Path.GetFileName(fileName));
            contentTabControl.TabPages.Add(page);

            var editor = new ScriptCodeEdit
            {
                Dock = DockStyle.Fill,
                Tag = fileName,
            };

            editor.TextChanged += (o, e) =>
            {
                if (contentTabControl.SelectedTab == page)
                    editedCodeFiles.Add(fileName);
            };

            var source = GetDesignerSource(formId);
            editor.InitSyntax();

            FormDesignerEditorHelpers.SetEditorSource(editor, fileName, source);
            editor.FileName = fileName;
            page.Controls.Add(editor);
            page.Tag = editor;
            contentTabControl.SelectTab(page);
            AddDesignFileForParsing(formId, fileName);
            return page;
        }

        private void AddDesignFileForParsing(string formId, string fileName)
        {
            var designFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".Designer" + Path.GetExtension(fileName));

            if (new FileInfo(designFile).Exists && FindPage(formId, designFile) == null)
                CodeEditExtensions.RegisterCode(Path.GetExtension(fileName), new string[] { designFile });
        }

        private void OpenOrActivateCode(string formId, string fileName)
        {
            TabPage page = FindPage(formId, fileName);
            if (page == null)
            {
                var key = new Tuple<string, string>(formId, fileName);
                page = OpenCode(formId, fileName);
                codeTabPages.Add(key, page);
            }
            else
                contentTabControl.SelectTab(page);
        }

        private void OpenAllFormFiles(string userCodeFile)
        {
            var source = GetDesignerSource(userCodeFile);

            OpenOrActivateCode(source.UserCodeFileName, source.UserCodeFileName);
            OpenOrActivateCode(source.UserCodeFileName, source.DesignerFileName);
            OpenDesigner(source.UserCodeFileName);
        }

        private void OpenDesigner(string fileName)
        {
            FormDesignerControl designer;

            try
            {
                designer = new FormDesignerControl
                {
                    Dock = DockStyle.Fill,
                    ReferencedAssemblies = GetReferencedAssemblies(fileName),
                    ImportedNamespaces = GetImportedNamespaces(fileName),
                    Source = GetDesignerSource(fileName),
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var page = new TabPage("Design: " + Path.GetFileNameWithoutExtension(fileName));
            contentTabControl.TabPages.Add(page);

            designer.NavigateToUserMethodRequested += FormDesignerControl_NavigateToUserMethodRequested;

            designer.IsSmartDiffCodeSerializationRequired = d => ((ITextSource)((EditorFormDesignerDataSource)d.Source).DesignerTextSource).Edits.Any();
            page.Controls.Add(designer);
            page.Tag = designer;
            contentTabControl.SelectTab(page);
        }

        private void SetCaretToMethod(string userCodeFileName, string methodName)
        {
            string toFind;
            var cs = Path.GetExtension(userCodeFileName) == ".cs";
            if (cs)
                toFind = "void " + methodName;
            else
                toFind = "Sub " + methodName;

            var editor = ActiveEditor;

            Point oldPosition = editor.Position;

            editor.Position = new System.Drawing.Point();
            if (editor.Find(toFind))
            {
                editor.MoveToLine(ActiveEditor.Position.Y + (cs ? 2 : 1));
                editor.MoveLineEnd();
            }
            else
                editor.Position = oldPosition;
        }

        private void FormDesignerControl_NavigateToUserMethodRequested(object sender, NavigateToUserMethodRequestedEventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            if (designer != ActiveFormDesigner)
                return;

            var source = (EditorFormDesignerDataSource)designer.Source;

            OpenOrActivateCode(source.UserCodeFileName, source.UserCodeFileName);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);
        }

        private EditorFormDesignerDataSource GetDesignerSource(string formId)
        {
            EditorFormDesignerDataSource ds;
            if (!sourcesByFormId.TryGetValue(formId, out ds))
            {
                ds = new EditorFormDesignerDataSource(
                    formId,
                    fileName =>
                    {
                        var source = new FormDesignerTextSource();
                        source.LoadFile(fileName);
                        return source;
                    });

                sourcesByFormId.Add(formId, ds);
            }

            return ds;
        }

        private DesignerReferencedAssemblies GetReferencedAssemblies(string fileName)
        {
#if NETCOREAPP
            var defaultReferences =
                new DesignerReferencedAssemblies(
                    MinimalDotNetCoreDependenciesService.GetReferences(Alternet.Common.TechnologyEnvironment.WindowsForms, useRuntimeAssemblies: false, needFullPaths: true));
#else
            var defaultReferences = Path.GetExtension(fileName).ToLower().Equals(".vb") ?
                DesignerReferencedAssemblies.DefaultForVisualBasic :
                DesignerReferencedAssemblies.DefaultForCSharp;
#endif

            return defaultReferences;
        }

        private DesignerImportedNamespaces GetImportedNamespaces(string fileName)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(".vb"))
                return null;

            return DesignerImportedNamespaces.DefaultForVisualBasic;
        }

        private string GetTestFilesDirectoryPath()
        {
            const string Subdirectory = @"Resources\Designer\WinForms";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, Subdirectory);

            if (!Directory.Exists(directory))
                directory = Path.Combine(Path.GetFullPath(baseDirectory.TrimEnd('\\') + @"\..\..\..\..\..\..\"), Subdirectory);

            return directory;
        }

        private void OnSelectedContentTabChanged()
        {
            var designer = ActiveFormDesigner;
            var editor = ActiveEditor;

            toolboxControl1.FormDesignerControl = designer;
            propertyGridControl1.FormDesignerControl = designer;

            bool haveActiveDesigner = designer != null;

            if (designer != null)
                ReloadDesigner(designer);
        }

        private void ReloadDesigner(IFormDesignerControl designer)
        {
            try
            {
                var designerFileName = designer.Source.DesignerFileName;
                if (editedCodeFiles.Contains(designerFileName))
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

        private void SaveDesignerFiles(IFormDesignerControl designer)
        {
            var source = (EditorFormDesignerDataSource)designer.Source;
            source.Save();
        }

        private void SaveEditorFile(IScriptEdit editor)
        {
            editor.SaveFile((string)((Control)editor).Tag);
        }

        private bool SaveAllModifiedFiles()
        {
            foreach (TabPage tabPage in contentTabControl.TabPages)
            {
                IScriptEdit edit = tabPage.Tag as IScriptEdit;
                if (edit != null && edit.Modified)
                {
                    edit.SaveFile(edit.FileName);
                }

                var designer = tabPage.Tag as IFormDesignerControl;
                if (designer != null)
                {
                    SaveDesignerFiles(designer);
                }
            }

            return true;
        }

        private EditorFormDesignerDataSource GetActiveDesignerSource()
        {
            var designer = ActiveFormDesigner;
            if (designer != null)
                return (EditorFormDesignerDataSource)designer.Source;

            var editor = ActiveEditor;
            if (editor != null)
            {
                var fileName = editor.FileName;
                var trimmed = Path.ChangeExtension(fileName, string.Empty).TrimEnd('.');
                var extension = Path.GetExtension(fileName).TrimStart('.');
                if (trimmed.EndsWith(".Designer", StringComparison.OrdinalIgnoreCase))
                    return this.GetDesignerSource(Path.ChangeExtension(trimmed, extension));

                if (extension.Equals("cs", StringComparison.OrdinalIgnoreCase) || extension.Equals("vb", StringComparison.OrdinalIgnoreCase))
                    return GetDesignerSource(fileName);
            }

            return null;
        }

        private void InitializeScripter()
        {
            if (scriptRun == null)
                scriptRun = new ScriptRun() { ScriptLanguage = ScriptLanguage.CSharp };
        }

        private bool SetScriptSource(EditorFormDesignerDataSource source, DesignerReferencedAssemblies referencedAssemblies)
        {
            string fileName = source.UserCodeFileName;
            if (new FileInfo(fileName).Exists)
            {
                scriptRun.ScriptSource.FromScriptFile(fileName);
                scriptRun.ScriptSource.WithDefaultReferences();
                var language = FormFilesUtility.DetectLanguageFromFileName(source.UserCodeFileName);

                foreach (string reference in referencedAssemblies.AssemblyNames)
                {
                    if (!scriptRun.ScriptSource.References.Contains(reference))
                        scriptRun.ScriptSource.References.Add(reference);
                }

                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N");
                scriptRun.ScriptHost.GenerateModulesOnDisk = true;

                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(
                            scriptRun.ScriptHost.CompilerErrors.Where(
                                x => x.Kind == ScriptCompilationDiagnosticKind.Error).First().ToString());
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void RunScript()
        {
            var source = GetActiveDesignerSource();
            if (source == null)
                return;

            var designer = ActiveFormDesigner;
            var referencedAssemblies = designer != null ? designer.ReferencedAssemblies : GetReferencedAssemblies(source.UserCodeFileName);

            if (SaveAllModifiedFiles())
            {
                InitializeScripter();
                if (SetScriptSource(source, referencedAssemblies))
                    scriptRun.RunProcess();
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            RunScript();
        }

        private void ContentTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedContentTabChanged();
        }
    }
}
