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
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Roslyn.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;

namespace MemoryAssembly
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

            CreateEditor(window.EditBorder, window.ExternalBorder);

            this.scriptRun.ReferenceResolve += ScriptRun_ReferenceResolve;

            AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            {
                if (ea.Name.Contains("ExternalAssembly"))
                    return Assembly.Load(ReadFully(Assembly.GetExecutingAssembly().GetManifestResourceStream("MemoryAssembly.ExternalAssemblyWpf.dll")));

                return null;
            };

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

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        public void StartScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);
                scriptRun.ScriptSource.WithDefaultReferences();
                scriptRun.ScriptSource.References.Add("ExternalAssemblyWpf");
                scriptRun.ScriptSource.References.Add("PresentationFramework");
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ScriptRun_ReferenceResolve(object sender, Alternet.Scripter.ResolveReferenceEventArgs e)
        {
            if (e.Reference == "ExternalAssemblyWpf")
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MemoryAssembly.ExternalAssemblyWpf.dll");
                byte[] bytes = ReadFully(stream);

                e.AssemblyImage = bytes;
                e.Handled = true;
            }
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTestWpf.cs";
            sourceExternalSubPath = "CustomClassWpf.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out string sourceExternalSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CustomAssemblyTestWpf.vb";
            sourceExternalSubPath = "CustomClassWpf.vb";
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
            string sourceExternalSubPath;
            ScriptLanguage language;
            switch (lang)
            {
                case "C#":
                    GetSourceParametersForCSharp(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out sourceExternalSubPath, out language);
                    break;
            }

            scriptRun.ScriptLanguage = language;
            LoadFile(editExt, GetSourceFileFullPath(sourceExternalSubPath));
            LoadFile(edit, GetSourceFileFullPath(sourceFileSubPath));
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            var parser = (edit as ScriptCodeEdit).Lexer as RoslynParser;
            if (parser != null)
            {
                parser.Repository.RegisterDefaultAssemblies(Alternet.Common.TechnologyEnvironment.Wpf);
                parser.ReparseText();
            }
        }

        private void CreateEditor(Border border, Border extBorder)
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;
            ScriptLanguage language;

            GetSourceParametersForCSharp(out sourceFileSubPath, out sourceExternalSubPath, out language);

            editExt = CreateEditor(GetSourceFileFullPath(sourceExternalSubPath), extBorder, true);
            edit = CreateEditor(GetSourceFileFullPath(sourceFileSubPath), border, false);
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