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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private Dictionary<TabItem, IFormDesignerControl> formDesigners = new Dictionary<TabItem, IFormDesignerControl>();
        private Dictionary<string, EditorFormDesignerDataSource> sourcesByFormId = new Dictionary<string, EditorFormDesignerDataSource>();

        private DesignedComponentAssemblyManager designedComponentAssemblyManager;

        private IFormDesignerControl ActiveFormDesigner
        {
            get
            {
                if (editorsTabControl.Items.Count == 0)
                    return null;

                IFormDesignerControl designer;
                if (editorsTabControl.SelectedItem == null ||
                    !formDesigners.TryGetValue((TabItem)editorsTabControl.SelectedItem, out designer))
                    return null;

                return designer;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AutoSaveToolbox();
            AutoSaveRecentFiles();
            base.OnClosing(e);
        }

        private void InitializeFormDesigner()
        {
            designedComponentAssemblyManager = new DesignedComponentAssemblyManager(scriptRun);

            UpdateDesignerControls();
            AutoLoadToolbox();
        }

        private void AutoSaveToolbox()
        {
            var toolboxAutoSaveFileName = GetToolboxAutoSaveFileName();
            using (var fs = new FileStream(toolboxAutoSaveFileName, FileMode.Create))
                toolboxControl.Save(fs);
        }

        private string GetToolboxAutoSaveFileName()
        {
            var directory = Path.Combine(Path.GetTempPath(), "AlternetStudio.Wpf.Demo", "dotnet-" + Environment.Version.ToString(2));
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

        private void NewFormMenuItem_Click(object sender, RoutedEventArgs e)
        {
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = FindUniqueName("MainWindow.xaml", ".cs");

            if (saveFileDialog.ShowDialog().Value != true)
                return;

            var userCodeFileName = saveFileDialog.FileName;

            var language = FormFilesUtility.TryDetectLanguageFromFileName(userCodeFileName);
            if (language == null)
            {
                MessageBox.Show(
                    "The user code file name must have an extension \".xaml.cs\" or \".xaml.vb\". You provided: "
                        + Path.GetFileName(userCodeFileName));
                return;
            }

            string xamlFileName = GetXamlFileName(userCodeFileName);

            if (!CheckUserCodeLanguageNonAmbiguous(xamlFileName, userCodeFileName))
                return;

            var project = GetProject((TreeViewItem)projectExplorerTreeView.SelectedItem);
            if (project == null)
                project = Project;
            bool addToProject = project != null && project.HasProject;

            var source = new FormDesignerDataSource(xamlFileName, language);
            FormFilesUtility.CreateFormFiles(source, new FormFilesUtility.CreateFormFilesOptions { GenerateMainMethod = !addToProject });

            if (!addToProject)
            {
                OpenFile(source.UserCodeFileName);
                OpenFile(source.XamlFileName);
                OpenDesigner(source.XamlFileName);
            }
            else
            {
                project.BeginUpdate();
                try
                {
                    project.AddFile(source.UserCodeFileName);
                    project.AddFile(source.XamlFileName, BuildAction.Page);
                    OpenFile(source.UserCodeFileName);
                    OpenFile(source.XamlFileName);
                    OpenDesigner(source.XamlFileName);
                }
                finally
                {
                    project.EndUpdate();
                }
            }
        }

        private Tuple<IFormDesignerControl, TabItem> FindDesigner(string fileName)
        {
            if (!Path.IsPathRooted(fileName))
                return null;

            var canonicalPath = new Uri(fileName).LocalPath;
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                IFormDesignerControl designer;

                if (!formDesigners.TryGetValue(tabPage, out designer))
                    continue;

                var path = ((IFormDesignerDataSource)designer.Source).XamlFileName;
                path = new Uri(path).LocalPath;
                if (path.Equals(canonicalPath, StringComparison.OrdinalIgnoreCase))
                {
                    return new Tuple<IFormDesignerControl, TabItem>(designer, tabPage);
                }
            }

            return null;
        }

        private EditorFormDesignerDataSource GetDesignerSource(string xamlFileName)
        {
            return GetDesignerSource(xamlFileName, true);
        }

        private EditorFormDesignerDataSource GetDesignerSource(string xamlFileName, bool createNew)
        {
            EditorFormDesignerDataSource ds;
            if (!sourcesByFormId.TryGetValue(xamlFileName, out ds) && createNew)
            {
                var language = FormFilesUtility.FindUserCodeFile(xamlFileName);

                ds = new EditorFormDesignerDataSource(
                    xamlFileName,
                    language,
                    fileName =>
                    {
                        var source = new FormDesignerTextSource();
                        source.LoadFile(fileName);
                        return source;
                    });

                sourcesByFormId.Add(xamlFileName, ds);
            }

            return ds;
        }

        private void Designer_DesignedContentChanged(object sender, EventArgs e)
        {
            var designer = sender as FormDesignerControl;

            if (designer != null)
            {
                var source = (EditorFormDesignerDataSource)designer.Source;

                UpdateDesignPage(designer.Parent as TabItem, source.XamlFileName, source.IsModified);
            }
        }

        private void Designer_ShowPropertiesRequested(object sender, EventArgs e)
        {
            var designer = ActiveFormDesigner;
            if (propertyGridControl.FormDesigner == designer)
            {
                rightTabControl.SelectedItem = propertiesTabItem;
            }
        }

        private IFormDesignerControl OpenDesigner(string fileName)
        {
            var designerTab = FindDesigner(fileName);
            if (designerTab != null)
            {
                editorsTabControl.SelectedItem = designerTab.Item2;
                return designerTab.Item1;
            }

            var designer = CreateDesignerControl(fileName);
            if (designer == null)
                return null;

            var page = new TabItem();
            page.Header = new TextBlock { Text = "Design: " + Path.GetFileName(fileName), ToolTip = fileName };

            editorsTabControl.Items.Add(page);

            page.Content = designer;
            formDesigners.Add(page, designer);

            editorsTabControl.SelectedItem = page;
            UpdateCodeNavigation();
            UpdateDesignerControls();

            designer.DesignContext.Services.Selection.SelectionChanged += (o, e) => OnDesignerSelectionChanged(designer, e);

            return designer;
        }

        private void UpdateFormFiles(IScriptEdit edit, EditorFormDesignerDataSource ds, string newName)
        {
            if (ds != null)
            {
                var newSource = new FormDesignerDataSource(GetXamlFileName(newName), FormFilesUtility.DetectLanguageFromFileName(newName));

                new FileInfo(ds.UserCodeFileName).CopyTo(newSource.UserCodeFileName, true);
                new FileInfo(ds.XamlFileName).CopyTo(newSource.XamlFileName, true);

                sourcesByFormId.Remove(ds.XamlFileName);

                foreach (TabItem tabPage in editorsTabControl.Items)
                {
                    IScriptEdit editor = GetEditor(tabPage);
                    if (editor != null)
                    {
                        if (editor.FileName.Equals(ds.UserCodeFileName))
                        {
                            editor.FileName = newSource.UserCodeFileName;
                            UpdatePage(tabPage, newSource.UserCodeFileName);
                        }

                        if (editor.FileName.Equals(ds.XamlFileName))
                        {
                            editor.FileName = newSource.XamlFileName;
                            UpdatePage(tabPage, newSource.XamlFileName);
                        }
                    }
                }

                var designerTab = FindDesigner(ds.XamlFileName);
                if (designerTab != null)
                {
                    UpdateDesigner(designerTab, newSource.XamlFileName);
                }

                edit.FileName = newName;
                UpdatePage((TabItem)editorsTabControl.SelectedItem, newName);
            }
        }

        private void RemoveDesigner(Tuple<IFormDesignerControl, TabItem> designerTab)
        {
            if (designerTab != null)
            {
                CloseDesigner(designerTab.Item1);
                formDesigners.Remove(designerTab.Item2);
                editorsTabControl.Items.Remove(designerTab.Item2);
                if (editors.ContainsKey(designerTab.Item2))
                    editors.Remove(designerTab.Item2);
            }
        }

        private void CloseDesigner(IFormDesignerControl designer)
        {
            designer.NavigateToUserMethodRequested -= FormDesignerControl_NavigateToUserMethodRequested;
            var designerFileName = ((IFormDesignerDataSource)designer.Source).XamlFileName;
            if (sourcesByFormId.ContainsKey(designerFileName))
                sourcesByFormId.Remove(designerFileName);
        }

        private void SaveDesignerFiles(TabItem tabPage, IFormDesignerControl designer)
        {
            var source = (EditorFormDesignerDataSource)designer.Source;
            source.Save();
            UpdateDesignPage(tabPage, source.XamlFileName, designer.Source.IsModified);
        }

        private void UpdateDesignerControls()
        {
            var designer = ActiveFormDesigner;

            bool haveActiveDesigner = designer != null;

            UpdateDesignerButtons();

            if (haveActiveDesigner)
            {
                ReloadDesignerIfNeeded(designer);
            }

            propertyGridControl.FormDesigner = designer;
            outline.FormDesigner = designer;
            outline.Toolbox = toolboxControl;
            toolboxControl.FormDesigner = designer;
        }

        private void ReloadDesignerIfNeeded(IFormDesignerControl designer)
        {
            try
            {
                var designerFileName = ((IFormDesignerDataSource)designer.Source).XamlFileName;
                if (this.editedCodeFiles.Contains(designerFileName, StringComparer.OrdinalIgnoreCase))
                {
                    designer.Reload();
                    editedCodeFiles.Remove(designerFileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Designer Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsAssemblyFileName(string file)
        {
            string ext = Path.GetExtension(file);
            return string.Compare(ext, ".dll", true) == 0 || string.Compare(ext, ".exe", true) == 0;
        }

        private DesignerReferencedAssemblies GetDesignerReferencedAssemblies(string fileName)
        {
            DesignerReferencedAssemblies defaultReferences = null;
            if (Project.HasProject)
            {
                string projectPath = Path.GetDirectoryName(Project.ProjectFileName);

                IList<string> references = new List<string>();
                foreach (var reference in Project.References)
                {
                    if (PathUtilities.IsPathFullyQualified(reference.FullName))
                        references.Add(reference.FullName);
                    else
                    if (!IsAssemblyFileName(reference.FullName))
                        references.Add(reference.FullName);
                    else
                    {
                        string fullPath = Path.GetFullPath(Path.Combine(projectPath, reference.FullName));
                        references.Add(File.Exists(fullPath) ? fullPath : reference.FullName);
                    }
                }

                AddReference(references, "mscorlib");
                AddReference(references, "PresentationCore");
                AddReference(references, "PresentationFramework");
                AddReference(references, "WindowsBase");

                if (Path.GetExtension(fileName).ToLower().Equals(".vb"))
                    references.Add("Microsoft.VisualBasic");

                defaultReferences = new DesignerReferencedAssemblies(references.ToArray());
            }
            else
                defaultReferences = new DesignerReferencedAssemblies(new string[] { "mscorlib", "System", "PresentationCore", "PresentationFramework", "System.Drawing", "WindowsBase", "Microsoft.VisualBasic" });

            return defaultReferences;
        }

        private string GetRootNamespce(string fileName)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(".vb"))
                return null;
            var project = GetProject(fileName);

            if (project != null && project.HasProject)
                return project.RootNamespace;

            return null;
        }

        private string[] GetImportedNamespaces(string fileName)
        {
            if (!Path.GetExtension(fileName).ToLower().Equals(".vb"))
                return null;

            if (Project.HasProject)
                return Project.ImportedNamespaces.ToArray();

            return new string[] { "Microsoft.VisualBasic", "System", "System.Collections", "System.Collections.Generic", "System.Data", "System.Drawing", "System.Diagnostics", "System.Windows.Media", "System.Windows.Controls", "System.Linq", "System.Xml.Linq", "System.Threading.Tasks" };
        }

        private void UpdateDesigner(Tuple<IFormDesignerControl, TabItem> designerTab, string fileName)
        {
            if (designerTab != null)
            {
                CloseDesigner(designerTab.Item1);
                formDesigners.Remove(designerTab.Item2);

                var designer = CreateDesignerControl(fileName);
                if (designer == null)
                    return;

                designerTab.Item2.Header = new TextBlock { Text = "Design: " + Path.GetFileName(fileName), ToolTip = fileName };

                designerTab.Item2.Content = designer;
                formDesigners.Add(designerTab.Item2, designer);
            }
            else
                OpenDesigner(fileName);
        }

        private void UpdateDesignPage(TabItem page, string fileName, bool isModified = false)
        {
            if (page == null)
                return;
            string pageText = "Design: " + Path.GetFileName(fileName);
            pageText = isModified ? pageText + "*" : pageText;
            UpdatePageHeader(page, pageText, fileName);
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

            saveMenuItem.IsEnabled = enabled;
            selectAllMenuItem.IsEnabled = enabled;
            closeFileMenuItem.IsEnabled = enabled;

            cutMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanCut;
            copyMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanCopy;
            pasteMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanPaste;

            deleteMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanDelete;

            undoMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanUndo;
            redoMenuItem.IsEnabled = enabled && designer.DesignerCommands.CanRedo;

            viewCodeMenuItem.IsEnabled = enabled;

            saveToolButton.IsEnabled = enabled;

            cutToolButton.IsEnabled = enabled;
            copyToolButton.IsEnabled = enabled;
            pasteToolButton.IsEnabled = enabled;

            undoToolButton.IsEnabled = enabled;
            redoToolButton.IsEnabled = enabled;
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

        private void ViewCodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var activeDesigner = ActiveFormDesigner;
            if (activeDesigner == null)
                return;

            OpenFile(((IFormDesignerDataSource)activeDesigner.Source).UserCodeFileName);
        }

        private void ViewDesignerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var activeEditor = ActiveSyntaxEdit;
            if (activeEditor == null)
                return;

            var xamlFileName = TryGetXamlFileNameToOpenDesigner(activeEditor.FileName);

            if (xamlFileName != null)
                OpenDesigner(xamlFileName);
        }

        private bool IsNameUnique(string baseName)
        {
            foreach (TabItem tabPage in editorsTabControl.Items)
            {
                if (tabPage.Header.ToString().StartsWith(baseName, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private string FindUniqueName(string baseName, string extesion)
        {
            int count = 1;
            string result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            while (!IsNameUnique(result))
            {
                count++;
                result = Path.GetFileNameWithoutExtension(baseName) + count.ToString() + Path.GetExtension(baseName);
            }

            return result + extesion;
        }

        private class DesignedComponentAssemblyManager
        {
            private readonly IScriptRun scriptRun;

            public DesignedComponentAssemblyManager(IScriptRun scriptRun)
            {
                this.scriptRun = scriptRun;
            }

            public string AssemblyPath
            {
                get
                {
                    return Path.Combine(AssemblyDirectoryPath, Path.GetFileName(scriptRun.ScriptHost.ExecutableModulePath));
                }
            }

            private string AssemblyDirectoryPath
            {
                get
                {
                    var path = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\DesignedComponentAssembly");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    return path;
                }
            }

            public void CopyOutput()
            {
                FileSystemUtility.CopyDirectory(Path.GetDirectoryName(scriptRun.ScriptHost.ExecutableModulePath), AssemblyDirectoryPath);
            }
        }
    }
}