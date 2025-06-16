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
using System.Windows.Forms;

using Alternet.Common.Projects.DotNet;

namespace AlternetStudio.Demo
{
    internal enum ReferenceType
    {
        All,
        References,
        Projects,
        Frameworks,
        Packages,
    }

    public partial class AddReferenceDialog : Form
    {
        private const string SearchPlaceholder = "Search...";
        private IDictionary<string, AssemblyData> assemblies = new Dictionary<string, AssemblyData>();
        private IDictionary<string, AssemblyData> packages = new Dictionary<string, AssemblyData>();
        private IDictionary<string, AssemblyData> frameworkAssemblies = new Dictionary<string, AssemblyData>();

        private IDictionary<string, DotNetProject> projects = new Dictionary<string, DotNetProject>();

        private IList<string> references = new List<string>();
        private IList<string> frameworkReferences = new List<string>();
        private List<DotNetProject.ProjectReference> projectReferences = new List<DotNetProject.ProjectReference>();

        private TreeNode assembliesNode = new TreeNode("All References");
        private TreeNode referencesNode = new TreeNode("Assemblies");
        private TreeNode projectsNode = new TreeNode("Projects");
        private TreeNode frameworkNode = new TreeNode("Framework");
        private TreeNode packagesNode = new TreeNode("Packages");

        private string initPath;
        private DotNetProject project;
        private DotNetSolution solution;
        private bool isSdk = false;
        private string searchFilter = string.Empty;
        private ReferenceType referenceType = ReferenceType.All;
        private int updateCount = 0;

        public AddReferenceDialog(DotNetProject project, DotNetSolution solution)
        {
            InitializeComponent();
            if (!AssemblyHelper.GACLoaded)
                AssemblyHelper.LoadAssembliesFromGAC();
            this.FilterAssembliesTextBox.Text = SearchPlaceholder;

            FilterAssembliesTextBox.GotFocus += FilterAssembliesTextBox_GotFocus;
            FilterAssembliesTextBox.LostFocus += FilterAssembliesTextBox_LostFocus;

            this.project = project;
            this.solution = solution;
            isSdk = project != null ? project.IsSdkStyle : false;
            this.initPath = Path.GetDirectoryName(project.ProjectFileName);
            ReferencesTreeView.Nodes.Add(assembliesNode);
            assembliesNode.Nodes.Add(referencesNode);
            if (!isSdk)
            {
                assembliesNode.Nodes.Add(frameworkNode);
            }

            assembliesNode.Nodes.Add(packagesNode);

            assembliesNode.Nodes.Add(projectsNode);

            assembliesNode.ExpandAll();

            ReferencesTreeView.SelectedNode = assembliesNode;
            ReferencesTreeView.Focus();
            ControlsFromAssemblies();
            ReferencesTreeView.AfterSelect += ReferencesTreeView_AfterSelect;
        }

        public IList<string> References
        {
            get
            {
                return references;
            }
        }

        public IList<string> FrameworkReferences
        {
            get
            {
                return frameworkReferences;
            }
        }

        public IList<DotNetProject.ProjectReference> ProjectReferences
        {
            get
            {
                return projectReferences;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchFilter
        {
            get
            {
                return string.Compare(searchFilter, SearchPlaceholder, true) == 0 ? string.Empty : searchFilter;
            }

            set
            {
                if (searchFilter == value)
                    return;

                searchFilter = value;
                ApplySearchFilter();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal ReferenceType ReferenceType
        {
            get
            {
                return referenceType;
            }

            set
            {
                if (referenceType != value)
                {
                    referenceType = value;
                    OnReferenceTypeChanged();
                }
            }
        }

        protected virtual void OnReferenceTypeChanged()
        {
            PopulateListBox();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AssembliesListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (updateCount > 0)
                return;

            var assemblyData = e.Item.Tag as AssemblyData;
            if (assemblyData != null)
            {
                assemblyData.Checked = e.Item.Checked;
                string name = Path.IsPathRooted(assemblyData.Name) ? Path.GetFileNameWithoutExtension(assemblyData.Name) : Path.GetFileName(assemblyData.Name);
                if (assemblies.ContainsKey(name))
                {
                    assemblies[assemblyData.Name] = assemblyData;
                }

                if (packages.ContainsKey(name))
                {
                    packages[assemblyData.Name] = assemblyData;
                }

                if (frameworkAssemblies.ContainsKey(name))
                {
                    frameworkAssemblies[name] = assemblyData;
                }

                references = assemblies.Where(y => y.Value.Checked).Select(x => x.Value.Name).ToList();
                references = references.Concat(packages.Where(y => y.Value.Checked).Select(x => AssemblyHelper.GetNugetName(x.Value))).ToList();
                frameworkReferences = frameworkAssemblies.Where(y => y.Value.Checked).Select(x => x.Value.Name).ToList();
            }

            var projectData = e.Item.Tag as DotNetProject;
            if (projectData != null)
            {
                if (e.Item.Checked)
                {
                    DotNetProject.ProjectReference refer = new DotNetProject.ProjectReference(projectData.ProjectName, projectData.ProjectFileName, projectData.ProjectGuid);
                    ProjectReferences.Add(refer);
                }
                else
                    projectReferences.RemoveAll(x => string.Compare(x.ProjectName, projectData.ProjectName, true) == 0);
            }
        }

        private void FilterAssembliesTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterAssembliesTextBox.Text))
                FilterAssembliesTextBox.Text = SearchPlaceholder;
        }

        private void FilterAssembliesTextBox_GotFocus(object sender, EventArgs e)
        {
            if (string.Compare(FilterAssembliesTextBox.Text, SearchPlaceholder, true) == 0)
            {
                FilterAssembliesTextBox.Text = string.Empty;
            }
        }

        private void ReferencesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ReferenceType = ReferencesTreeView.SelectedNode == assembliesNode ? ReferenceType.All :
                (ReferencesTreeView.SelectedNode == referencesNode ? ReferenceType.References :
                (ReferencesTreeView.SelectedNode == projectsNode ? ReferenceType.Projects :
                (ReferencesTreeView.SelectedNode == packagesNode ? ReferenceType.Packages :
                (ReferencesTreeView.SelectedNode == frameworkNode ? ReferenceType.Frameworks : ReferenceType.All))));
        }

        private void ApplySearchFilter()
        {
            PopulateListBox();
        }

        private void PopulateListBox()
        {
            void AddAsm(AssemblyData data, bool onlyChecked = false)
            {
                if (onlyChecked && !data.Checked)
                    return;

                string fileName = Path.IsPathRooted(data.Name) ? Path.GetFileNameWithoutExtension(data.Name) : Path.GetFileName(data.Name);
                string[] items =
                    {
                string.Format("{0}", fileName),
                string.Format("{0}", data.Version),
                    };

                ListViewItem item = new ListViewItem(items);
                item.Checked = data.Checked;
                item.Tag = data;
                AssembliesListView.Items.Add(item);
            }

            void AddProject(DotNetProject data, bool onlyChecked = false)
            {
                if (data == project)
                    return;

                string[] items =
                    {
                string.Format("{0}", data.ProjectName),
                string.Format("{0}", data.ProjectFileName),
                    };

                ListViewItem item = new ListViewItem(items);
                bool isChecked = projectReferences.Any(x => string.Compare(x.ProjectName, data.ProjectName) == 0);
                if (onlyChecked && !isChecked)
                    return;
                item.Checked = isChecked;
                item.Tag = data;
                AssembliesListView.Items.Add(item);
            }

            void AddProjects()
            {
                var filteredProjects = projects;
                if (!string.IsNullOrEmpty(SearchFilter))
                {
                    filteredProjects = projects.Where(i => Path.GetFileName(i.Key).StartsWith(SearchFilter, true, System.Globalization.CultureInfo.InvariantCulture))
         .ToDictionary(i => i.Key, i => i.Value);
                }

                foreach (string item in filteredProjects.Keys)
                {
                    DotNetProject data = filteredProjects[item];
                    AddProject(data, referenceType == ReferenceType.All);
                }
            }

            void AddReferences()
            {
                var filteredAssemblies = assemblies;
                if (!string.IsNullOrEmpty(SearchFilter))
                {
                    filteredAssemblies = assemblies.Where(i => Path.GetFileName(i.Key).StartsWith(SearchFilter, true, System.Globalization.CultureInfo.InvariantCulture))
         .ToDictionary(i => i.Key, i => i.Value);
                }

                foreach (string item in filteredAssemblies.Keys)
                {
                    AssemblyData data = filteredAssemblies[item];
                    AddAsm(data);
                }
            }

            void AddPackages()
            {
                var filteredPackages = packages;
                if (!string.IsNullOrEmpty(SearchFilter))
                {
                    filteredPackages = packages.Where(i => Path.GetFileName(i.Key).StartsWith(SearchFilter, true, System.Globalization.CultureInfo.InvariantCulture))
         .ToDictionary(i => i.Key, i => i.Value);
                }

                foreach (string item in filteredPackages.Keys)
                {
                    AssemblyData data = filteredPackages[item];
                    AddAsm(data);
                }
            }

            void AddFrameworkReferences()
            {
                var filteredAssemblies = frameworkAssemblies;
                if (!string.IsNullOrEmpty(SearchFilter))
                {
                    filteredAssemblies = frameworkAssemblies.Where(i => Path.GetFileName(i.Key).StartsWith(SearchFilter, true, System.Globalization.CultureInfo.InvariantCulture))
         .ToDictionary(i => i.Key, i => i.Value);
                }

                foreach (string item in filteredAssemblies.Keys)
                {
                    AssemblyData data = filteredAssemblies[item];
                    AddAsm(data, referenceType == ReferenceType.All);
                }
            }

            VersionHeader.Text = referenceType == ReferenceType.Projects ? "Path" : "Version";
            AssembliesListView.BeginUpdate();
            updateCount++;
            try
            {
                AssembliesListView.Items.Clear();
                switch (referenceType)
                {
                    case ReferenceType.All:
                        AddReferences();
                        AddFrameworkReferences();
                        AddProjects();
                        AddPackages();
                        break;

                    case ReferenceType.References:
                        AddReferences();
                        break;

                    case ReferenceType.Packages:
                        AddPackages();
                        break;

                    case ReferenceType.Frameworks:
                        AddFrameworkReferences();
                        break;

                    case ReferenceType.Projects:
                        AddProjects();
                        break;
                }
            }
            finally
            {
                updateCount--;
                AssembliesListView.EndUpdate();
            }
        }

        private void AddAssembly(AssemblyData data, bool nuget)
        {
            string name = Path.IsPathRooted(data.Name) ? Path.GetFileNameWithoutExtension(data.Name) : Path.GetFileName(data.Name);
            if (nuget)
            {
                if (!packages.ContainsKey(name))
                    packages.Add(name, data);
            }
            else
            if (!assemblies.ContainsKey(name))
                assemblies.Add(name, data);
        }

        private void AddFrameworkAssembly(AssemblyData data)
        {
            string name = Path.IsPathRooted(data.Name) ? Path.GetFileNameWithoutExtension(data.Name) : Path.GetFileName(data.Name);
            if (!frameworkAssemblies.ContainsKey(name))
                frameworkAssemblies.Add(name, data);
        }

        private void ControlsFromAssemblies()
        {
            if (!isSdk)
            {
                foreach (var data in AssemblyHelper.GACAssemblies)
                {
                    bool isChecked = project.References.Any(x => string.Compare(Path.GetFileName(x.Name), Path.GetFileNameWithoutExtension(data.Name), true) == 0);
                    data.Checked = isChecked;
                    AddFrameworkAssembly(data);
                }
            }

            foreach (var item in project.References)
            {
                if (isSdk || !AssemblyHelper.GACAssemblies.Any(x => string.Compare(Path.GetFileNameWithoutExtension(x.Name), Path.GetFileName(item.Name), true) == 0))
                {
                    bool nuget = false;
                    AssemblyData data = AssemblyHelper.FromFile(item.FullName, ref nuget);
                    data.Checked = true;
                    AddAssembly(data, nuget);
                }
            }

            if (!solution.IsEmpty)
            {
                foreach (var proj in solution.Projects)
                {
                    if (!projects.ContainsKey(proj.ProjectName))
                        projects.Add(proj.ProjectName, proj);
                }
            }

            references = assemblies.Select(x => x.Value.Name).ToList();
            references = references.Concat(packages.Select(x => AssemblyHelper.GetNugetName(x.Value))).ToList();
            frameworkReferences = frameworkAssemblies.Where(y => y.Value.Checked).Select(x => x.Value.Name).ToList();
            projectReferences = project.ProjectReferences.ToList();
            PopulateListBox();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = initPath;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var reference = dialog.FileName;
            bool nuget = false;
            AssemblyData data = AssemblyHelper.FromFile(reference, ref nuget);
            data.Checked = true;
            AddAssembly(data, nuget);
            ControlsFromAssemblies();
        }

        private void FilterAssembliesTextBox_TextChanged(object sender, EventArgs e)
        {
            SearchFilter = FilterAssembliesTextBox.Text;
        }

        private void AddReferenceDialog_Load(object sender, EventArgs e)
        {
            AssembliesListView.ItemChecked += AssembliesListView_ItemChecked;
        }
    }
}
