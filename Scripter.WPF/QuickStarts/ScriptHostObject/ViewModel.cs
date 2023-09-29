#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Wpf;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace ScriptHostObject
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private TextEditor edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private IDefaultAssembliesProvider defaultAssembliesProvider = DefaultAssembliesProviderFactory.CreateDefaultAssembliesProvider();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
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
                    scriptRun.ScriptSource.References.Add(typeof(Globals).Assembly.Location);
                    scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

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
            sourceFileSubPath = "ScriptHostObjectWpf.csx";
            language = ScriptLanguage.CSharpScript;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ScriptHostObjectWpf.vbx";
            language = ScriptLanguage.VisualBasicScript;
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
            LoadFile(edit, sourceFileFullPath, language);

            scriptRun.ScriptLanguage = language;
            InitScripter();
        }

        private void LoadFile(TextEditor edit, string fileName, ScriptLanguage language)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.Source.FileName = fileName;
            RoslynParser parser = null;
            RoslynSolution solution = null;
            switch (language)
            {
                case ScriptLanguage.CSharpScript:
                    solution = new CsSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script, typeof(Globals));
                    parser = new CsParser(solution);
                    parser.Repository.RegisterAssemblies(defaultAssembliesProvider.GetDefaultAssemblies(Alternet.Common.TechnologyEnvironment.Wpf));
                    break;
                case ScriptLanguage.VisualBasicScript:
                    //solution = new VbSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script, typeof(Globals));
                    //parser = new VbParser(solution);
                    break;
            }

            edit.Lexer = parser;
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForCSharp(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private TextEditor CreateEditor(string fileName, Border border)
        {
            TextEditor edit;
            edit = new TextEditor();
            edit.Source.FileName = fileName;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName, ScriptLanguage.CSharpScript);

            return edit;
        }

        private void InitScripter()
        {
            var global = new Globals();
            global.LabelExpression = window.ExpressionLabel;
            scriptRun.ScriptHost.HostGlobalObject = global;
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}