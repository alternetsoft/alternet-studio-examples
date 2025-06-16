#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Scripter.TypeScript;

namespace CustomAssembly.TypeScript
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private IScriptEdit edit;
        private IScriptEdit editExt;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            InitDefaultHostAssemblies();

            CreateEditor(window.EditBorder, window.ExternalBorder);

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return lang;
            }

            set
            {
                if (lang != value)
                {
                    lang = value;
                    OnPropertyChanged("Language");
                    UpdateSource();
                }
            }
        }

        public ICommand RunScript { get; set; }

        public void StartScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);

                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                scriptRun.Run();
            }));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\CustomAssemblyTestWpf.ts";
            sourceExternalSubPath = @"TS\CustomClassWpf.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\CustomAssemblyTestWpf.js";
            sourceExternalSubPath = @"JS\CustomClassWpf.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private string GetSourceDirectoryFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript\";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!Directory.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!Directory.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(TechnologyEnvironment.Wpf, options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            var externalDll = @"lib\ExternalAssembly.dll";
            AddAssembly(GetSourceFileFullPath(externalDll));

            var project = Alternet.Common.TypeScript.TypeScriptProject.DefaultProject;
            project.TypeDefinitionsSearchPaths = scriptRun.TypeDefinitionsSearchPaths = new[] { GetSourceDirectoryFullPath("lib") };
            project.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void AddAssembly(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string alias = "_" + name;

            foreach (HostItem item in scriptRun.ScriptHost.HostItemsConfiguration.Items)
            {
                if (item is AssemblyCollectionHostItem)
                {
                    if (string.Compare(item.Name, alias) == 0)
                        return;
                }
            }

            scriptRun.ScriptHost.HostItemsConfiguration.AddAssembly(
                alias,
                path,
                HostItemOptions.GlobalMembers,
                new[] { GetSourceDirectoryFullPath("lib") });
        }

        private void UpdateSource()
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;
            switch (lang)
            {
                case "TypeScript":
                    GetSourceParametersForTS(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
            }

            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));

            scriptRun.ScriptLanguage = language;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Border border, Border extBorder)
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;

            GetSourceParametersForTS(out sourceFileSubPath, out sourceExternalSubPath, out language);

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(GetSourceFileFullPath(sourceFileSubPath), border, false);
            editExt = CreateEditor(GetSourceFileFullPath(sourceExternalSubPath), extBorder, true);
        }

        private IScriptEdit CreateEditor(string fileName, Border border, bool readOnly)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);
            edit.ReadOnly = readOnly;

            return edit;
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}