#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Studio

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common.Projects;
using Alternet.Editor.Common;
using Alternet.Editor.TextSource;
using Alternet.Editor.TypeScript;
using Alternet.FormDesigner.WinForms;
using Alternet.Scripter.TypeScript;

namespace AlternetStudio.Demo
{
    public partial class MainForm
    {
        private ProjectCreationData projectCreationData = new ProjectCreationData { ProjectType = "ConsoleApp" };

        protected TSProject Project { get; private set; } = new TSProject();

        private string TemplateSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(dir, @"Studio\Projects"));
            }
        }

        private string DefaultProjectSubPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(dir, @"..\Projects"));
            }
        }

        private bool IsProjectFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".json", true) == 0;
        }

        private bool FileBelongsToProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private bool FileBelongsToProject(Project project, string fileName)
        {
            if (project.HasProject)
            {
                if (project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void OpenProject(string fileName)
        {
            if (debugger != null && debugger.IsStarted)
            {
                MessageBox.Show("Please stop debugging session first");
                return;
            }

            if (debugger != null)
                debugger.Breakpoints.Clear();

            scriptRun.ScriptSource.FromScriptProject(fileName);
            Project.Load(fileName);
            CodeEditExtensions.OpenProject(fileName, Project);

            scriptRun.ScriptHost.ModulesDirectoryPath =
                Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "Alternet.Studio.TypeScript Generated Modules", Project.ProjectName + fileName.GetHashCode());
            LoadBreakpoints(GetBreakpointFile(Project));
            LoadBookmarks(GetBookmarkFile(Project));

            Project.ProjectModified += ProjectModified;

            if (Project.Files.Count > 0)
            {
                string mainFile = Project.Files.Last();
                if (FormFilesUtility.CheckIfFormFilesExist(mainFile))
                    OpenDesigner(mainFile);
                else
                OpenFile(mainFile);

                var extension = Path.GetExtension(mainFile);
                CodeEditExtensions.RegisterCode(extension, GetSourceFiles(Project.Files));
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
            errorsControl.Clear();
        }

        private void ProjectModified(object sender, EventArgs e)
        {
            UpdateProjectExplorer();
        }

        private void CloseProject(TSProject project)
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

            CodeEditExtensions.CloseProject(extension);

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

        private void GetProjectParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\MultipleFiles\project.json";
            language = ScriptLanguage.TypeScript;
        }

        private void GetProjectParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"jS\MultipleFiles\project.json";
            language = ScriptLanguage.JavaScript;
        }

        private TSProject GetProject(IFormDesignerDataSource source)
        {
            if (source != null)
                return GetProject(source.DesignerFileName);

            return null;
        }

        private TSProject GetProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (FileBelongsToProject(Project, fileName))
                    return Project;
            }

            return null;
        }

        private bool SaveProject()
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

        private TSProject FindProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (string.Compare(Project.ProjectFileName, fileName, true) == 0)
                    return Project;
            }

            return null;
        }

        private bool AddReferencesToProject(TSProject project, IEnumerable<string> references)
        {
            bool result = false;
            foreach (var reference in references)
            {
                if (reference.StartsWith("mscorlib"))
                    continue;

                if (project.AddHostAssembly(reference))
                    result = true;
            }

            return result;
        }

        private void NewProjectMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projectCreationData.ProjectLocation))
                projectCreationData.ProjectLocation = DefaultProjectSubPath;
            using (var dlg = new DlgNewProject(projectCreationData, new string[] { "TypeScript", "JavaScript" }, new string[] { "BlankProject", "ConsoleApp", "WindowsApp" }))
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
            openFileDialog.FilterIndex = 3;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (Project.HasProject)
                    CloseProject();

                OpenProject(openFileDialog.FileName);
            }
        }

        private void SaveProjectMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveProject();
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

        private void GetDisplayFilesForProject(IList<string> displayFiles, IList<string> files)
        {
            bool projectAdded = false;
            int i = 0;
            while (i < files.Count)
            {
                string file = files[i];
                if (!IsProjectFile(file) && FileBelongsToProject(file))
                {
                    if (!projectAdded)
                    {
                        bool projectModified = files.IndexOf(Project.ProjectFileName) >= 0;
                        displayFiles.Add(Path.GetFileName(Project.ProjectFileName) + (projectModified ? "*" : string.Empty));
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
                int idx = files.IndexOf(Project.ProjectFileName);
                if (idx >= 0)
                    files.RemoveAt(idx);
            }
        }
    }
}