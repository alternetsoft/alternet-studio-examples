#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Alternet.Common;
#if NETCOREAPP
using Alternet.Common.DotNet.DefaultAssemblies;
#endif

using Alternet.Editor.Common;
using Alternet.Editor.Roslyn;
using Alternet.FormDesigner.Integration;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter;

namespace DesignInMemory
{
    public partial class Form1 : Form
    {
        private HashSet<string> editedCodeFiles = new HashSet<string>();
        private Dictionary<string, TabPage> codeTabPages = new Dictionary<string, TabPage>();
        private Dictionary<string, TabPage> designerTabPages = new Dictionary<string, TabPage>();
        private Dictionary<string, MemoryFormDesignerDataSource> sourcesByNames = new Dictionary<string, MemoryFormDesignerDataSource>();
        private IScriptRun scriptRun;
        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "DesignInMemory.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private IScriptEdit ActiveEditor
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Controls.Count > 0 ? tab.Controls[0] as IScriptEdit : null;
            }
        }

        private FormDesignerControl ActiveFormDesigner
        {
            get
            {
                var tab = contentTabControl.TabPages.Count == 0 ? null : contentTabControl.SelectedTab;
                return tab == null ? null : tab.Controls.Count > 0 ? tab.Controls[0] as FormDesignerControl : null;
            }
        }

        private void SaveForm(MemoryFormDesignerDataSource source)
        {
            var initPath = GetTestFilesDirectoryPath();
            folderBrowserDialog.SelectedPath = initPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var newPath = folderBrowserDialog.SelectedPath;
                source.DesignerTextSource.SaveFile(Path.Combine(newPath, source.DesignerFileName));
                var userCode = Path.Combine(newPath, source.UserCodeFileName);
                source.UserCodeTextSource.SaveFile(userCode);
                var ext = Path.GetExtension(userCode);
                var resFilePath = Path.Combine(newPath, Path.GetFileNameWithoutExtension(userCode) + ".resx");
                source.ResourceTextSource.SaveFile(resFilePath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "C # files (*.cs)|*.cs|All files (*.*)|*.*";

            string folder = GetTestFilesDirectoryPath();
            if (Directory.Exists(folder))
            {
                var designerFiles = Directory.GetFiles(folder, "*.Designer.*", SearchOption.AllDirectories);

                foreach (var designerFile in designerFiles)
                {
                    var extension = Path.GetExtension(designerFile);

                    var userCodeFile = designerFile.Replace(".Designer" + extension, extension);
                    OpenAllFormFiles(userCodeFile, LanguageServices.GetSupportedLanguage(SupportedLanguages.CSharp));
                    break;
                }
            }

            OnSelectedContentTabChanged();
        }

        private void OpenOrActivateCode(string sourceName, string fileName, ContentType contentType, Language language)
        {
            var tabName = string.Empty;
            switch (contentType)
            {
                case ContentType.UserCode:
                    tabName = "Code: " + sourceName;
                    break;
                case ContentType.Designer:
                    tabName = "Designer: " + sourceName;
                    break;
            }

            if (codeTabPages.ContainsKey(tabName))
            {
                contentTabControl.SelectedTab = codeTabPages[tabName];
                return;
            }

            TabPage page = new TabPage(tabName);
            var tabInfo = new TabInfo(contentType, sourceName);
            page.Tag = tabInfo;
            contentTabControl.TabPages.Add(page);

            var editor = new ScriptCodeEdit
            {
                Dock = DockStyle.Fill,
            };

            editor.TextChanged += (o, e) =>
            {
                if (contentTabControl.SelectedTab == page)
                    editedCodeFiles.Add(fileName);
            };

            var source = GetDesignerSource(sourceName);

            editor.InitSyntax();

            MemoryFormDesignerEditorHelpers.SetEditorSource(editor, contentType, source);
            editor.FileName = fileName;

            page.Controls.Add(editor);
            contentTabControl.SelectTab(page);

            codeTabPages.Add(tabName, page);
        }

        private void OpenAllFormFiles(string userCodeFile, Language language)
        {
            string filePath = Path.GetDirectoryName(userCodeFile);
            string name = Path.GetFileName(userCodeFile);
            string nameNoExt = Path.GetFileNameWithoutExtension(name);
            string ext = Path.GetExtension(userCodeFile);
            string dir = Path.GetDirectoryName(userCodeFile);

            if (sourcesByNames.ContainsKey(name))
                return;

            string userCodeText = File.ReadAllText(userCodeFile);
            string designerText = File.ReadAllText(Path.Combine(dir, nameNoExt + ".Designer" + ext));
            string resourceText = File.ReadAllText(Path.Combine(dir, nameNoExt + ".resx"));
            string designedClassName = "MyForm";
            var source = CreateDesignerSource(name, designerText, userCodeText, resourceText, designedClassName, language);

            source.UserCodeFileName = name;
            source.DesignerFileName = nameNoExt + ".Designer" + ext;

            OpenOrActivateCode(name, source.UserCodeFileName, ContentType.UserCode, language);
            OpenOrActivateCode(name, source.DesignerFileName, ContentType.Designer, language);
            OpenDesigner(name);
        }

        private MemoryFormDesignerDataSource FindDesignerSource()
        {
            MemoryFormDesignerDataSource result = null;
            var designer = ActiveFormDesigner;
            if (designer != null)
            {
                return (MemoryFormDesignerDataSource)designer.Source;
            }

            var editor = ActiveEditor;
            if (editor != null)
            {
                string name = editor.FileName;
                if (name.Contains(".Designer"))
                {
                    name = name.Replace(".Designer", string.Empty);
                }

                name = Path.GetFileName(name);

                if (sourcesByNames.ContainsKey(name))
                {
                    return sourcesByNames[name];
                }
            }

            return result;
        }

        private MemoryFormDesignerDataSource GetDesignerSource(string name)
        {
            return sourcesByNames[name];
        }

        private MemoryFormDesignerDataSource CreateDesignerSource(
            string name,
            string designerText,
            string userCodeText,
            string resourceText,
            string designedClassName,
            Language language)
        {
            var ds = new MemoryFormDesignerDataSource(
                name,
                designerText,
                userCodeText,
                resourceText,
                designedClassName,
                language,
                text =>
                {
                    var source = new FormDesignerTextSource();
                    using (var stream = GetTextStream(text))
                        source.LoadStream(stream);
                    return source;
                });

            sourcesByNames.Add(name, ds);

            return ds;
        }

        private Stream GetTextStream(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text), writable: false);
        }

        private void OpenDesigner(string sourceName)
        {
            var tabName = "Design: " + sourceName;
            if (designerTabPages.ContainsKey(tabName))
            {
                contentTabControl.SelectedTab = designerTabPages[tabName];
                return;
            }

            FormDesignerControl designer;

            try
            {
                var src = GetDesignerSource(sourceName);
                designer = new FormDesignerControl
                {
                    Dock = DockStyle.Fill,
                    ReferencedAssemblies = GetReferencedAssemblies(src.UserCodeLanguage),
                    ImportedNamespaces = GetImportedNamespaces(src.UserCodeLanguage),
                    Source = src,
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var page = new TabPage(tabName);
            var tabInfo = new TabInfo(ContentType.Design, sourceName);
            page.Tag = tabInfo;
            contentTabControl.TabPages.Add(page);

            designer.NavigateToUserMethodRequested += FormDesignerControl_NavigateToUserMethodRequested;
            designer.IsSmartDiffCodeSerializationRequired = d => ((FormDesignerTextSource)((MemoryFormDesignerDataSource)d.Source).DesignerTextSource).Edits.Any();

            page.Controls.Add(designer);
            contentTabControl.SelectTab(page);
            designerTabPages.Add(tabName, page);
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

            var source = (MemoryFormDesignerDataSource)designer.Source;

            OpenOrActivateCode(source.Name, source.UserCodeFileName, ContentType.UserCode, source.UserCodeLanguage);
            SetCaretToMethod(source.UserCodeFileName, e.MethodName);
        }

        private DesignerReferencedAssemblies GetReferencedAssemblies(Language language)
        {
            var ext = language.Extension;
#if NETCOREAPP
            var defaultReferences =
                new DesignerReferencedAssemblies(
                    MinimalDotNetCoreDependenciesService.GetReferences(Alternet.Common.TechnologyEnvironment.WindowsForms, useRuntimeAssemblies: false, needFullPaths: true, useDesignReferences: true));
#else
            var defaultReferences = ext.ToLower().Equals(".vb") ?
                DesignerReferencedAssemblies.DefaultForVisualBasic :
                DesignerReferencedAssemblies.DefaultForCSharp;
#endif

            return defaultReferences;
        }

        private DesignerImportedNamespaces GetImportedNamespaces(Language language)
        {
            var ext = language.Extension;
            if (!ext.ToLower().Equals(".vb"))
                return null;

            return DesignerImportedNamespaces.DefaultForVisualBasic;
        }

        private string GetTestFilesDirectoryPath()
        {
            const string Subdirectory = @"Resources\Designer\WinForms\CS";

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

        private void InitializeScripter()
        {
            if (scriptRun == null)
                scriptRun = new ScriptRun() { ScriptLanguage = ScriptLanguage.CSharp };
        }

        private bool SetScriptSource(FormDesignerDataSource source, DesignerReferencedAssemblies referencedAssemblies, string tempDirectoryPath)
        {
            string userCodeFileName = source.UserCodeFileName;

            var activeEditor = ActiveEditor;
            if (userCodeFileName != null && File.Exists(userCodeFileName))
            {
                scriptRun.ScriptSource.FromScriptFile(userCodeFileName);
                scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.WindowsForms);

                foreach (string reference in referencedAssemblies.AssemblyNames)
                {
                    if (!scriptRun.ScriptSource.References.Contains(reference))
                        scriptRun.ScriptSource.References.Add(reference);
                }

                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N") + ".exe";
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
            try
            {
                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tempDirectoryPath);
                var runnableScriptFiles = SaveActiveSourceFilesForRunning(tempDirectoryPath);
                if (runnableScriptFiles != null)
                {
                    RunScriptWithScripter(runnableScriptFiles, tempDirectoryPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error running form: " + e);
            }
        }

        private void RunScriptWithScripter(RunnableScriptFiles runnableScriptFiles, string tempDirectoryPath)
        {
            FormDesignerDataSource tempSource = new FormDesignerDataSource(runnableScriptFiles.CodeFilePath);

            var designer = ActiveFormDesigner;
            if (designer == null)
                return;

            InitializeScripter();
            if (SetScriptSource(tempSource, designer.ReferencedAssemblies, tempDirectoryPath))
                scriptRun.RunProcess();
        }

        private RunnableScriptFiles SaveActiveSourceFilesForRunning(string tempDirectoryPath)
        {
            var selectedItem = contentTabControl.SelectedTab;
            if (selectedItem == null)
                return null;

            var tabInfo = (TabInfo)selectedItem.Tag;
            var sourceName = tabInfo.SourceName;

            var source = sourcesByNames[sourceName];
            return SaveDesignerSourceToDirectory(source, tempDirectoryPath);
        }

        private RunnableScriptFiles SaveDesignerSourceToDirectory(MemoryFormDesignerDataSource source, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var ext = GetExtensionFromLanguage(source.UserCodeLanguage);
            var shortName = Path.GetFileNameWithoutExtension(source.Name);
            var codeFilePath = Path.Combine(directoryPath, shortName + ext);
            var designFilePath = Path.Combine(directoryPath, shortName + ".Designer" + ext);
            var resFilePath = Path.Combine(directoryPath, shortName + ".resx");

            using (var stream = new FileStream(codeFilePath, FileMode.Create))
                source.UserCodeTextSource.SaveStream(stream);
            using (var stream = new FileStream(designFilePath, FileMode.Create))
                source.DesignerTextSource.SaveStream(stream);

            using (var stream = new FileStream(resFilePath, FileMode.Create))
                source.ResourceTextSource.SaveStream(stream);

            return new RunnableScriptFiles(codeFilePath, designFilePath);
        }

        private string GetExtensionFromLanguage(Language language)
        {
            return language.Extension;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            RunScript();
        }

        private void ContentTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedContentTabChanged();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# Files|*.Designer.cs|VB.NET Files|*.Designer.vb|All Files|*.*",
                Multiselect = false,
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            var result = dialog.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            var designerFileName = dialog.FileName;

            if (!designerFileName.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase) &&
                !designerFileName.EndsWith(".Designer.vb", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The file must have an extension .Designer.cs or .Designer.vb");
                return;
            }

            var extension = Path.GetExtension(designerFileName);
            var userCodeFile = designerFileName.Replace(".Designer" + extension, extension);
            Language language = LanguageServices.GetSupportedLanguage(SupportedLanguages.CSharp);
            if (string.Compare(extension, ".vb") == 0)
                language = LanguageServices.GetSupportedLanguage(SupportedLanguages.VisualBasic);
            OpenAllFormFiles(userCodeFile, language);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var source = FindDesignerSource();
            if (source != null)
            {
                SaveForm(source);
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tab = contentTabControl.SelectedTab;
            if (tab == null)
                return;

            if (!codeTabPages.ContainsValue(tab))
            {
                var designer = ActiveFormDesigner;
                if (designer != null)
                {
                    var source = designer.Source;
                    CloseDesigner(designer);
                    if (designerTabPages.ContainsValue(tab))
                        RemoveAllByValue(designerTabPages, tab);
                }
            }

            contentTabControl.TabPages.Remove(tab);
            RemoveAllByValue(codeTabPages, tab);
        }

        private void CloseDesigner(IFormDesignerControl designer)
        {
            designer.NavigateToUserMethodRequested -= FormDesignerControl_NavigateToUserMethodRequested;
            var userCodeFile = designer.Source.UserCodeFileName;
            string name = Path.GetFileName(userCodeFile);
            if (sourcesByNames.ContainsKey(name))
                sourcesByNames.Remove(name);
            ((FormDesignerControl)designer).Dispose();
        }

#pragma warning disable SA1314 // Type parameter names should begin with T
        private void RemoveAllByValue<K, V>(Dictionary<K, V> dictionary, V value)
#pragma warning restore SA1314 // Type parameter names should begin with T
        {
            foreach (var key in dictionary.Where(
                kvp => EqualityComparer<V>.Default.Equals(kvp.Value, value)).Select(x => x.Key).ToArray())
                dictionary.Remove(key);
        }

        internal class RunnableScriptFiles
        {
            public RunnableScriptFiles(string codeFilePath, string designFilePath)
            {
                CodeFilePath = codeFilePath;
                DesignFilePath = designFilePath;
            }

            public string CodeFilePath { get; private set; }

            public string DesignFilePath { get; private set; }
        }

        internal class TabInfo
        {
            public TabInfo(ContentType type, string sourceName)
            {
                Type = type;
                SourceName = sourceName;
            }

            public ContentType Type { get; set; }

            public string SourceName { get; set; }
        }
    }
}