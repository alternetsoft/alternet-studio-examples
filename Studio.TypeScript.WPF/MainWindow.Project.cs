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
using System.Windows;
using System.Windows.Controls;
using Alternet.Common.Projects;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Editor.Wpf;

namespace AlternetStudio.TypeScript.Wpf.Demo
{
    public partial class MainWindow
    {
        private bool projectIsClosing;

        private ProjectCreationData projectCreationData = new ProjectCreationData { ProjectType = "ConsoleApp" };

        protected TSProject Project { get; private set; } = new TSProject();

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

        private bool IsProjectFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return string.Compare(ext, ".json", true) == 0;
        }

        private void OpenProject(string fileName)
        {
            if (!TryResetDebuggerOnProjectChange())
                return;

            Project.Load(fileName);
            CodeEditExtensions.OpenProject(fileName, Project);

            UpdateScriptProject(Project);
            LoadBreakpoints(GetBreakpointFile(Project));
            LoadBookmarks(GetBookmarkFile(Project));

            Project.ProjectModified += ProjectModified;

            if (Project.Files.Count > 0)
            {
                string mainFile = Project.Files.Last();
                OpenFile(mainFile);

                var extension = Path.GetExtension(mainFile);

                CodeEditExtensions.RegisterCode(extension, GetSourceFiles(Project.Files));
            }

            UpdateProjectExplorer();
            UpdateCodeNavigation();
            errorsControl.Clear();
        }

        private void SaveProject(TSProject proj)
        {
            if (proj.HasProject && proj.IsModified)
            {
                proj.Save();
                if (proj == Project)
                    UpdateScriptProject(Project);
            }
        }

        private bool FileBelongsToProject(string fileName)
        {
            if (Project.HasProject)
            {
                if (Project.Files.Contains(fileName, StringComparer.OrdinalIgnoreCase) || Project.Resources.Contains(fileName, StringComparer.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void ProjectModified(object sender, EventArgs e)
        {
            UpdateProjectExplorer();
        }

        private void CloseProject(TSProject project)
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

            CodeEditExtensions.CloseProject(extension);
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
            navigationHistory.ClearHistory(backwardMenu.ContextMenu.Items, historyBackwardToolButton, backwardMenu, historyForwardToolButton, Backward_ItemClick);
            return true;
        }

        private void NewProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(projectCreationData.ProjectLocation))
                projectCreationData.ProjectLocation = DefaultProjectSubPath;
            DlgNewProject dlg = new DlgNewProject(projectCreationData, new string[] { "TypeScript", "JavaScript" }, new string[] { "BlankProject", "ConsoleApp", "WindowsApp" });
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

        private void OpenProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.FilterIndex = 3;
            if (openFileDialog.ShowDialog(Window.GetWindow(this)).Value == true)
            {
                if (Project.HasProject)
                    CloseProject();
                OpenProject(openFileDialog.FileName);
            }
        }

        private void SaveProjectMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void CloseProjectMenuItem_Click(object sender, RoutedEventArgs e)
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