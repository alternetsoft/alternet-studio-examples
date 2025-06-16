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
using System.Windows.Forms;
using Alternet.Common.Projects;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Common;
using Alternet.Editor.IronPython;
using Alternet.Editor.TextSource;
using Alternet.FormDesigner.WinForms;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private bool projectIsClosing;

        private ProjectCreationData projectCreationData = new ProjectCreationData { ProjectType = "WindowsApp" };

        protected IronPythonProject Project { get; private set; } = new IronPythonProject();

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
            return string.Compare(ext, ".pyproj", true) == 0;
        }

        protected void OpenProject(string fileName)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

            Project.Load(fileName);

            if (Project == null && !Project.HasProject)
            {
                MessageBox.Show("Cannot load project");
                return;
            }

            UpdateScriptProject(Project);
            LoadBreakpoints(GetBreakpointFile(Project));
            LoadBookmarks(GetBookmarkFile(Project));

            Project.ProjectModified += ProjectModified;

            // todo
            if (Project.Files.Count > 0)
            {
                var firstFile = GetFirstFile(Project.Files, Project.DefaultExtension);
                if (FormFilesUtility.CheckIfFormFilesExist(firstFile, "_Designer"))
                    OpenDesigner(firstFile);
                else
                    OpenFile(firstFile);
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
            errorsControl.Clear();
        }

        private IronPythonProject GetProject(TreeNode node)
        {
            while (node != null)
            {
                if ((node.Tag != null) && (node.Tag is IronPythonProject))
                    return node.Tag as IronPythonProject;
                node = node.Parent;
            }

            return null;
        }

        private bool HasProject()
        {
            return Project.HasProject;
        }

        private DotNetProject GetProject(IFormDesignerDataSource source)
        {
            if (source != null)
                return GetProject(source.DesignerFileName);

            return null;
        }

        private IronPythonProject GetProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (FileBelongsToProject(Project, fileName))
                    return Project;
            }

            return null;
        }

        private void SaveProject(IronPythonProject proj)
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

            return false;
        }

        private Project FindProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (string.Compare(Project.ProjectFileName, fileName, true) == 0)
                    return Project;
            }

            return null;
        }

        private string GetProjectName(string fileName)
        {
            if (Project.HasProject)
            {
                if (FileBelongsToProject(Project, fileName))
                    return Project.ProjectName;
            }

            return null;
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

        private void ProjectModified(object sender, EventArgs e)
        {
            UpdateProjectExplorer();
        }

        private string GetFirstFile(IList<string> files, string langExt)
        {
            string result = files.Count > 0 ? files[0] : string.Empty;

            foreach (string file in files)
            {
                if (file.ToLower().Contains("program.py"))
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

            navigationHistory.ClearHistory(historyBackwardContextMenu.Items, historyBackwardToolSplitButton, historyForwardToolButton);
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
                CloseProject(Project);

                Project.Reset();

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
            navigationHistory.ClearHistory(historyBackwardContextMenu.Items, historyBackwardToolSplitButton, historyForwardToolButton);
            return true;
        }

        private void NewProjectMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projectCreationData.ProjectLocation))
                projectCreationData.ProjectLocation = DefaultProjectSubPath;

            using (var dlg = new DlgNewProject(projectCreationData, new string[] { "IronPython" }, new string[] { "BlankProject", "ConsoleApp", "WindowsApp" }))
            {
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
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
            openFileDialog.FilterIndex = 2;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
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
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseProjectMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private bool ConfirmSaveProjectBeforeClosing()
        {
            IList<string> list = GetModifiedFiles(Project.AllFiles(true));
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