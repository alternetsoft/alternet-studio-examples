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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Alternet.Common.Projects.DotNet;
using Microsoft.Win32;

namespace AlternetStudio.Wpf.Demo
{
    internal enum ReferenceType
    {
        All,
        References,
        Projects,
        Frameworks,
        Packages,
    }

    /// <summary>
    /// Interaction logic for AddReferenceDialog.xaml
    /// </summary>
    public partial class AddReferenceDialog : Window
    {
        private const string SearchPlaceholder = "Search...";
        private IDictionary<string, AssemblyData> assemblies = new Dictionary<string, AssemblyData>();
        private IDictionary<string, AssemblyData> packages = new Dictionary<string, AssemblyData>();
        private IDictionary<string, AssemblyData> frameworkAssemblies = new Dictionary<string, AssemblyData>();

        private IDictionary<string, DotNetProject> projects = new Dictionary<string, DotNetProject>();

        private IList<string> references = new List<string>();
        private IList<string> frameworkReferences = new List<string>();
        private List<DotNetProject.ProjectReference> projectReferences = new List<DotNetProject.ProjectReference>();

        private TreeViewItem assembliesNode = new TreeViewItem() { Header = "All References" };
        private TreeViewItem referencesNode = new TreeViewItem() { Header = "Assemblies" };
        private TreeViewItem projectsNode = new TreeViewItem() { Header = "Projects" };
        private TreeViewItem frameworkNode = new TreeViewItem() { Header = "Framework" };
        private TreeViewItem packagesNode = new TreeViewItem() { Header = "Packages" };
        private ObservableCollection<ListViewItemData> source = new ObservableCollection<ListViewItemData>();

        private string initPath;
        private DotNetProject project;
        private DotNetSolution solution;
        private bool isSdk = false;
        private string searchFilter = string.Empty;
        private ReferenceType referenceType = ReferenceType.All;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

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
            ReferencesTreeView.Items.Add(assembliesNode);
            assembliesNode.Items.Add(referencesNode);
            if (!isSdk)
            {
                assembliesNode.Items.Add(frameworkNode);
            }

            assembliesNode.Items.Add(packagesNode);

            assembliesNode.Items.Add(projectsNode);

            assembliesNode.IsSelected = true;
            ReferencesTreeView.Focus();
            ControlsFromAssemblies();
            ReferencesTreeView.SelectedItemChanged += ReferencesTreeView_SelectedItemChanged;
            BrowseButton.Click += BrowseButton_Click;
            FilterAssembliesTextBox.TextChanged += FilterAssembliesTextBox_TextChanged;
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
                ListViewItemData listData = new ListViewItemData();
                listData.Name = fileName;
                listData.Version = data.Version;
                listData.Tag = data;
                listData.IsChecked = data.Checked;
                source.Add(listData);
            }

            void AddProject(DotNetProject data, bool onlyChecked = false)
            {
                if (data == project)
                    return;

                bool isChecked = projectReferences.Any(x => string.Compare(x.ProjectName, data.ProjectName) == 0);
                if (onlyChecked && !isChecked)
                    return;

                ListViewItemData listData = new ListViewItemData();
                listData.Name = data.ProjectName;
                listData.Version = data.ProjectFileName;
                listData.Tag = data;
                listData.IsChecked = isChecked;
                source.Add(listData);
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

            VersionColumn.Header = referenceType == ReferenceType.Projects ? "Path" : "Version";
            source.Clear();
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

            AssembliesListView.ItemsSource = source;
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

        private void ReferencesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ReferenceType = ReferencesTreeView.SelectedItem == assembliesNode ? ReferenceType.All :
                (ReferencesTreeView.SelectedItem == referencesNode ? ReferenceType.References :
                (ReferencesTreeView.SelectedItem == projectsNode ? ReferenceType.Projects :
                (ReferencesTreeView.SelectedItem == packagesNode ? ReferenceType.Packages :
                (ReferencesTreeView.SelectedItem == frameworkNode ? ReferenceType.Frameworks : ReferenceType.All))));
        }

        private void FilterAssembliesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchFilter = FilterAssembliesTextBox.Text;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = initPath;

            if (openFileDialog.ShowDialog().Value)
            {
                var reference = openFileDialog.FileName;
                bool nuget = false;
                AssemblyData data = AssemblyHelper.FromFile(reference, ref nuget);
                data.Checked = true;
                AddAssembly(data, nuget);
                ControlsFromAssemblies();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateData(sender as CheckBox);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateData(sender as CheckBox);
        }

        private void UpdateData(CheckBox checkBox)
        {
            if (checkBox == null)
                return;

            ListViewItemData data = checkBox.DataContext as ListViewItemData;
            if (data == null)
                return;

            var assemblyData = data.Tag as AssemblyData;
            if (assemblyData != null)
            {
                assemblyData.Checked = checkBox.IsChecked.Value;
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

            var projectData = data.Tag as DotNetProject;
            if (projectData != null)
            {
                if (checkBox.IsChecked.Value)
                {
                    DotNetProject.ProjectReference refer = new DotNetProject.ProjectReference(projectData.ProjectName, projectData.ProjectFileName, projectData.ProjectGuid);
                    ProjectReferences.Add(refer);
                }
                else
                    projectReferences.RemoveAll(x => string.Compare(x.ProjectName, projectData.ProjectName, true) == 0);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        protected class ListViewItemData
        {
            public string Name { get; set; }

            public string Version { get; set; }

            public bool IsChecked { get; set; }

            public object Tag { get; set; }
        }
    }
}
