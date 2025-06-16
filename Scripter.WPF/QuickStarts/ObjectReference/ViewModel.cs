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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Scripter;

namespace ObjectReference
{
    public class ViewModel
    {
        private bool scriptRunning;
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private IDisposable scriptObject;
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private IDefaultAssembliesProvider defaultAssembliesProvider = DefaultAssembliesProviderFactory.CreateDefaultAssembliesProvider();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
            StopScript = new RelayCommand(StopScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            CreateEditor(window.EditBorder);

            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            languages.Add("C#");
            languages.Add("VB");
            Language = "C#";
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

        public ICommand StopScript { get; set; }

        public void StartScript()
        {
            if (window != null)
            {
                window.Dispatcher.BeginInvoke((Action)(() =>
                {
                    scriptRun.ScriptSource.FromScriptCode(edit.Text);
                    scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.Wpf);
                    scriptRun.ScriptSource.References.Add("PresentationCore");
                    scriptRun.ScriptSource.References.Add("PresentationFramework");
                    scriptRun.ScriptSource.References.Add("WindowsBase");
                    scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

                    if (!scriptRun.Compiled)
                    {
                        if (!scriptRun.Compile())
                        {
                            MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                            return;
                        }
                    }

                    scriptRunning = true;
                    window.ScriptButton.Content = "Stop Script";
                    scriptObject = scriptRun.Run() as IDisposable;
                }));
            }
        }

        public void DoStopScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                if (window != null)
                {
                    if (scriptObject != null)
                        scriptObject.Dispose();
                    scriptObject = null;
                    window.ScriptButton.Content = "Run Script";
                    window.TestButton.Content = "Test Button";
                }

                scriptRunning = false;
            }));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReferenceWpf.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ObjectReferenceWpf.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource()
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (lang)
            {
                case "C#":
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
            AddScriptItem();
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
            edit.RegisterAssemblies(defaultAssembliesProvider.GetDefaultAssemblies(Alternet.Common.TechnologyEnvironment.Wpf));
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();
            edit.FileName = fileName;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);

            return edit;
        }

        private void AddScriptItem()
        {
            if (window != null)
            {
                ScriptGlobalItem item = new ScriptGlobalItem("RunButton", typeof(System.Windows.Controls.Button), window.TestButton);
                scriptRun.GlobalItems.Clear();
                scriptRun.GlobalItems.Add(item);
                RegisterScriptCodeForEditor();
            }
        }

        private void RegisterScriptCodeForEditor()
        {
            edit.RegisterCode("global.cs", scriptRun.ScriptHost.GlobalCode);
        }

        private void RunScriptClick()
        {
            if (!scriptRunning)
                StartScript();
            else
                DoStopScript();
        }

        private void StopScriptClick()
        {
            DoStopScript();
        }
    }
}