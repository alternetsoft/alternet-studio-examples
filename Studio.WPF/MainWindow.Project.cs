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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Alternet.Common.Projects;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.FormDesigner.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;
using Alternet.Syntax.Parsers.Roslyn;

namespace AlternetStudio.Wpf.Demo
{
    public partial class MainWindow
    {
        private bool projectIsClosing;

        private DotNetSolution solution = new DotNetSolution();

        private ProjectCreationData projectCreationData = new ProjectCreationData { ProjectType = "WPFApp" };

        protected DotNetProject Project { get; private set; } = new DotNetProject();

        protected TargetFramework CurrentFramework
        {
            get
            {
                return Project != null && Project.HasProject ? Project.TargetFramework : null;
            }

            set
            {
                if (CurrentFramework != value)
                {
                    if (Project != null && Project.HasProject)
                    {
                        Project.TargetFramework = value;
                        RefreshProject(Project);
                    }
                }
            }
        }

        private string TemplateSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(startupDirectory, @"Studio\Projects"));
            }
        }

        private string DefaultProjectSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(startupDirectory, @"..\Projects"));
            }
        }

        protected bool IsProjectFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".csproj", true) == 0 || string.Compare(ext, ".vbproj", true) == 0;
        }

        protected void OpenProject(string fileName)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

            if (Path.GetExtension(fileName).EndsWith("sln"))
            {
                solution.Load(fileName);
                if (!solution.IsEmpty)
                {
                    Project = solution.Projects.Where(p => p.IsFolder == false).FirstOrDefault();
                    fileName = Project?.ProjectFileName;
                }
            }
            else
                Project.Load(fileName);

            if (Project == null || !Project.HasProject)
            {
                System.Windows.MessageBox.Show("Cannot load project");
                return;
            }

            UpdateScriptProject(Project);
            BreakpointsDataService.RootPath = Path.GetDirectoryName(Project.ProjectFileName);
            BookmarksDataService.RootPath = Path.GetDirectoryName(Project.ProjectFileName);
            if (solution != null && !solution.IsEmpty)
            {
                LoadBreakpoints(GetBreakpointFile(solution));
                LoadBookmarks(GetBookmarkFile(solution));
            }
            else
            {
                LoadBreakpoints(GetBreakpointFile(Project));
                LoadBookmarks(GetBookmarkFile(Project));
            }

            Project.ProjectModified += ProjectModified;

            if (!solution.IsEmpty)
            {
                foreach (var prj in solution.Projects)
                    OpenProject(prj);

                foreach (var prj in solution.Projects)
                {
                    var extension = string.Format(".{0}", prj.DefaultExtension);
                    CodeEditExtensions.RegisterProjectReferences(extension, prj.ProjectReferences.Select(x => string.IsNullOrEmpty(x.ProjectName) && !string.IsNullOrEmpty(x.ProjectPath) ? Path.GetFileNameWithoutExtension(x.ProjectPath) : x.ProjectName).ToArray(), prj.ProjectName);
                }
            }
            else
                OpenProject(Project);

            RegisterWinFormsApplicationConfigurationCode();

            if (Project.Files.Count > 0)
            {
                var firstFile = GetFirstFile(Project.Files, Project.DefaultExtension);
                if (FormFilesUtility.CheckIfFormFilesExist(firstFile))
                    OpenDesigner(firstFile);
                else
                    OpenFile(firstFile);
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
            bool hasFrameworks = HasProject() && Project.TargetFrameworks?.Count > 0;
            if (hasFrameworks)
            {
                FillProjectFrameworks();
            }

            errorsControl.Clear();
        }

        private void RegisterWinFormsApplicationConfigurationCode()
        {
            var autoCode = ScriptRun.ScriptHost.AutoGeneratedCode;
            if (!string.IsNullOrEmpty(autoCode))
            {
                CodeEditExtensions.RegisterCode(
                    ".cs",
                    "__appconfiguration.cs",
                    autoCode,
                    Project.ProjectName);
            }
        }

        private DotNetProject GetProject(TreeViewItem node)
        {
            while (node != null)
            {
                if ((node.Tag != null) && (node.Tag is Project))
                    return node.Tag as DotNetProject;
                node = GetParentItem(node);
            }

            return null;
        }

        private DotNetSolution GetSolution(TreeViewItem node)
        {
            if (node == null)
                return null;

            if ((node.Tag != null) && (node.Tag is DotNetSolution))
                return node.Tag as DotNetSolution;

            return null;
        }

        private void FillProjectFrameworks()
        {
            CodeUtils.FillFrameworks(ProjectFrameworks, Project?.TargetFrameworks);
        }

        private bool HasProject()
        {
            return !solution.IsEmpty || Project.HasProject;
        }

        private DotNetProject GetProject(string fileName)
        {
            if (!solution.IsEmpty)
            {
                foreach (var prj in solution.Projects)
                {
                    if (FileBelongsToProject(prj, fileName))
                        return prj;
                }
            }
            else
            if (Project.HasProject)
            {
                if (FileBelongsToProject(Project, fileName))
                    return Project;
            }

            return null;
        }

        private void SaveProject(DotNetProject proj)
        {
            if (proj.HasProject && proj.IsModified)
            {
                proj.Save();
                if (proj == Project)
                    UpdateScriptProject(Project);
            }
        }

        private bool SaveAllProjects()
        {
            // saves current project
            if (Project.HasProject && Project.IsModified)
            {
                Project.Save();
                UpdateScriptProject(Project);
                return true;
            }

            if (!solution.IsEmpty && solution.IsModified)
            {
                solution.Save();
                return true;
            }

            return false;
        }

        private DotNetProject FindProject(string fileName)
        {
            if (!solution.IsEmpty)
            {
                foreach (var proj in solution.Projects)
                {
                    if (string.Compare(proj.ProjectFileName, fileName, true) == 0)
                        return proj;
                }
            }
            else
            if (Project.HasProject)
            {
                if (string.Compare(Project.ProjectFileName, fileName, true) == 0)
                    return Project;
            }

            return null;
        }

        private bool IsSolutionFile(string fileName)
        {
            return Path.GetExtension(fileName).EndsWith("sln");
        }

        private bool AddReferencesToProject(DotNetProject project, IEnumerable<string> references)
        {
            bool result = false;
            foreach (var reference in references)
            {
                if (reference.StartsWith("mscorlib"))
                    continue;

                if (project.AddReference(reference))
                    result = true;
            }

            return result;
        }

        private string GetProjectName(string fileName)
        {
            if (!solution.IsEmpty)
            {
                foreach (var prj in solution.Projects)
                {
                    if (FileBelongsToProject(prj, fileName))
                        return prj.ProjectName;
                }
            }
            else
            if (Project.HasProject)
            {
                if (FileBelongsToProject(Project, fileName))
                    return Project.ProjectName;
            }

            return null;
        }

        private bool FileBelongsToSolution(string fileName)
        {
            if (solution != null && !solution.IsEmpty)
                return solution.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase);

            return false;
        }

        private bool FileBelongsToProject(DotNetProject project, string fileName)
        {
            if (project.HasProject)
            {
                if (project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase) || project.Resources.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private bool FileBelongsToProject(string fileName)
        {
            return !string.IsNullOrEmpty(GetProjectName(fileName));
        }

        private void RefreshProject(DotNetProject project)
        {
            CodeEditExtensions.RefreshProject(project, GetSourceFiles(project, project.Files, project.ProjectExtension, true));
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName, project.TargetFramework);
            UpdateOpenFiles();
            UpdateErrors(project);
        }

        private void OpenProject(DotNetProject project)
        {
            CodeEditExtensions.OpenProject(project, GetSourceFiles(project, project.Files, project.ProjectExtension, true));
        }

        private void UpdateOpenFiles()
        {
            foreach (TabItem item in editorsTabControl.Items)
            {
                var editor = GetEditor(item) as TextEditor;
                var parser = editor?.Lexer as RoslynParser;
                if (parser != null)
                    parser.ReparseText();
            }
        }

        private async void UpdateErrors(DotNetProject project)
        {
            var proj = CodeEditExtensions.GetProject(project);
            if (proj != null)
            {
                var compilation = await proj.GetCompilationAsync();
                errorsControl.Clear(FilterError);
                errorsControl.AddCompilerErrors(GetErrors(compilation.GetDiagnostics()));
            }
        }

        private void ProjectModified(object sender, EventArgs e)
        {
            UpdateProjectExplorer();
        }

        private string GetFirstFile(IList<string> files, string langExt)
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

        private void CloseProject(DotNetProject project)
        {
            foreach (string fileName in project.Files)
            {
                RemoveDesigner(FindDesigner(fileName));
                CloseFile(fileName);
            }

            foreach (string fileName in project.Resources)
            {
                CloseFile(fileName);
            }

            var extension = string.Format(".{0}", project.DefaultExtension);

            CodeEditExtensions.CloseProject(extension, project.ProjectName);
            navigationHistory.ClearHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
        }

        private bool CloseProject()
        {
            if (!ConfirmSaveProjectBeforeClosing())
                return false;

            if (!TryResetDebuggerOnProjectChange())
                return false;

            scriptRun.ScriptSource.Reset();
            projectIsClosing = true;

            try
            {
                if (!solution.IsEmpty)
                {
                    foreach (var prj in solution.Projects)
                        CloseProject(prj);
                }
                else
                    CloseProject(Project);

                Project.Reset();
                solution.Reset();

                UpdateCodeNavigation();
                errorsControl.Clear();
                BookMarkManager.SharedBookMarks.Clear();
                UpdateBookmarkButtons();
            }
            finally
            {
                projectIsClosing = false;
            }

            UpdateProjectExplorer();
            navigationHistory.ClearHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            return true;
        }

        private void NewProjectMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projectCreationData.ProjectLocation))
                projectCreationData.ProjectLocation = DefaultProjectSubPath;

            DlgNewProject dlg = new DlgNewProject(projectCreationData, new string[] { "C#", "Visual Basic" }, ProjectTypes.SupportedProjectTypes);
            {
                if (dlg.ShowDialog().Value)
                {
                    projectCreationData = dlg.ProjectData;
                    if (CloseProject())
                    {
                        string projectFileName = Project.Create(dlg.ProjectData, TemplateSubPath);
                        if (File.Exists(projectFileName))
                            OpenProject(projectFileName);
                    }
                }
            }
        }

        private void OpenProjectMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = 4;
            if (openFileDialog.ShowDialog(Window.GetWindow(this)).Value == true)
            {
                if (HasProject())
                    CloseProject();
                OpenProject(openFileDialog.FileName);
            }
        }

        private void SaveProjectMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveProject(Project);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseProjectMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private bool ConfirmSaveProjectBeforeClosing()
        {
            IList<string> list = GetModifiedFiles(!solution.IsEmpty ? solution.AllFiles(true) : Project.AllFiles(true));
            return ConfirmSaveBeforeClosing(list, true);
        }

        private void GetDisplayFilesForProject(IList<string> displayFiles, DotNetProject project, IList<string> files)
        {
            bool projectAdded = false;
            int i = 0;
            while (i < files.Count)
            {
                string file = files[i];
                if (!IsProjectFile(file) && FileBelongsToProject(project, file))
                {
                    if (!projectAdded)
                    {
                        bool projectModified = files.IndexOf(project.ProjectFileName) >= 0;
                        displayFiles.Add(Path.GetFileName(project.ProjectFileName) + (projectModified ? "*" : string.Empty));
                        projectAdded = true;
                    }

                    displayFiles.Add(string.Format("   {0}*", Path.GetFileName(file)));
                    files.RemoveAt(i);
                }
                else
                    i++;
            }

            if (projectAdded)
            {
                int idx = files.IndexOf(project.ProjectFileName);
                if (idx >= 0)
                    files.RemoveAt(idx);
            }
        }
    }
}