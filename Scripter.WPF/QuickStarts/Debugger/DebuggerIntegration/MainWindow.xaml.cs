using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.FormDesigner.Integration.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Integration.Wpf;
using Microsoft.Win32;

namespace DebuggerIntegration.Wpf
{
    public partial class MainWindow : Window
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger\CS\HelloWorld.Wpf\HelloWorld.Wpf.csproj";
        private ScriptDebugger debugger;

        private DebugCodeEditContainer codeEditContainer;

        private ScriptRun scriptRun;

        public MainWindow()
        {
            InitializeComponent();

            scriptRun = new ScriptRun();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            OpenProject(FindProjectFile());

            debugger = new ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(Dispatcher, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            DebugMenu.InstallKeyboardShortcuts(CommandBindings);
            FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;
            UpdateDebugControls();
        }

        protected DotNetProject Project { get; private set; } = new DotNetProject();

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private static string FindProjectFile() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private static string FindDefaultProjectDirectory() =>
            ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, Path.GetDirectoryName(StartupProjectFileSubPath)))).FirstOrDefault(Directory.Exists);

        private static string GetFirstFile(IList<string> files, string langExt)
        {
            string result = files.Count > 0 ? files[0] : string.Empty;

            foreach (string file in files)
            {
                if (file.ToLower().Contains("program.cs"))
                    return file;
                if (file.ToLower().Contains("main") && file.EndsWith(langExt))
                    return file;
            }

            return result;
        }

        private void OnDebuggerPreStartup(object sender, System.EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
        }

        private bool SetScriptSource()
        {
            if (Project.HasProject)
                return true;

            if (codeEditContainer.ActiveEditor != null)
            {
                string fileName = codeEditContainer.ActiveEditor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void OpenProject(string projectFilePath)
        {
            if (Project != null && Project.HasProject)
                CloseProject(Project);

            Project.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            var extension = Project.ProjectExtension;
            CodeEditExtensions.OpenProject(extension, Project.ProjectName, Project.ProjectFileName);

            var codeFiles = Project.Files.Where(x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".vb").ToList();

            if (codeFiles.Count > 0)
            {
                foreach (var file in codeFiles.ToArray())
                {
                    string formId;
                    if (FormFilesUtility.IsXamlCodeBehindFile(file, out formId))
                    {
                        codeFiles.Add(
                            XamlGeneratedCodeFileService.GetGeneratedCodeFile(Project, new FormDesignerDataSource(formId, FormFilesUtility.DetectLanguageFromFileName(file))));
                    }
                }

                CodeEditExtensions.RegisterCode(extension, codeFiles.ToArray(), Project.ProjectName);
                codeEditContainer.TryActivateEditor(GetFirstFile(codeFiles, Project.DefaultExtension));
            }

            var references = Project.References.Concat(Project.AutoReferences).Select(x => x.FullName).Concat(
                Project.FrameworkReferences.SelectMany(x => x.Assemblies).Select(x => x.HintPath)).ToArray();

            CodeEditExtensions.RegisterAssemblies(
               extension,
               Project.TryResolveAbsolutePaths(references).ToArray(),
               projectName: Project.ProjectName,
               targetFramework: Project.TargetFramework);

            DebuggerPanelsTabControl.Errors.Clear();
            UpdateDebugControls();
        }

        private void UpdateDebugControls()
        {
            bool enabled = (Project != null && Project.HasProject) || codeEditContainer.ActiveEditor != null;

            DebuggerControlToolbar.Debugger = enabled ? debugger : null;
            DebugMenu.Debugger = enabled ? debugger : null;
        }

        private void CloseProject(DotNetProject project)
        {
            foreach (string fileName in project.Files)
            {
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

            var extension = string.Format(".{0}", project.DefaultExtension);

            CodeEditExtensions.CloseProject(extension, project.ProjectName);
            Project?.Reset();
            scriptRun.ScriptSource?.Reset();
            UpdateDebugControls();
        }

        private void CloseFile(string fileName)
        {
            codeEditContainer.CloseFile(fileName);
        }

        private string GetProjectName(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return Project.ProjectName;
            }

            return null;
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            var projectName = GetProjectName(e.FileName);
            edit.SetFileNameAndProject(e.FileName, projectName);
            edit.LoadFile(e.FileName);
            e.DebugEdit = edit;
        }

        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Project files (*.csproj; *.vbproj)|*.csproj;*.vbproj|All files (*.*)|*.*",
                InitialDirectory = FindDefaultProjectDirectory(),
            };

            if (dialog.ShowDialog(this) != true)
                return;

            OpenProject(dialog.FileName);
        }

        private void CloseProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseProject(Project);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "C# files (*.cs) |*.cs|Visual Basic files (*.vb) | *.vb|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindProjectFile());
            if (dialog.ShowDialog().Value)
            {
                codeEditContainer.TryActivateEditor(dialog.FileName);

                UpdateDebugControls();
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
            {
                codeEditContainer.CloseFile(edit.FileName);
                edit.FileName = string.Empty;
            }

            if (!Project.HasProject && codeEditContainer.Editors.Count == 0)
            {
                Project?.Reset();
                scriptRun.ScriptSource?.Reset();
            }

            UpdateDebugControls();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            CloseProjectMenuItem.IsEnabled = Project != null && Project.HasProject;
            CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }
    }
}