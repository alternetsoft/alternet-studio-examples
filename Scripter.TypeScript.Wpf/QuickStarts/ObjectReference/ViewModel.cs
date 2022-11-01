#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

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

using Alternet.Common;
using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.TypeScript.Wpf;
using Alternet.Scripter;
using Alternet.Scripter.TypeScript;

namespace ObjectReference.TypeScript
{
    public class ViewModel
    {
        private bool scriptRunning;
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private dynamic catcher;
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
            StopScript = new RelayCommand(StopScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            InitDefaultHostAssemblies();
            CreateEditor(window.EditBorder);

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

        public ICommand StopScript { get; set; }

        public void StartScript()
        {
            if (window != null)
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

                    catcher = scriptRun.RunFunction("RunMe");

                    scriptRunning = true;
                    window.ScriptButton.Content = "Stop Script";
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
                    if (catcher != null)
                        catcher.Dispose();
                    catcher = null;

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

        private void InitDefaultHostAssemblies()
        {
            if (window != null)
            {
                scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(TechnologyEnvironment.Wpf, options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions)
                 .AddObject("RunButton", window.TestButton);
            }

            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\ObjectReferenceWpf.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\ObjectReferenceWpf.js";
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

        private void UpdateSource()
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (lang)
            {
                case "TypeScript":
                    GetSourceParametersForTS(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForTS(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.InitSyntax();

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);

            return edit;
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