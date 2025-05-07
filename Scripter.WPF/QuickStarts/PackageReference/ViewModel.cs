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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.Wpf;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;

namespace PackageReference
{
    public class ViewModel
    {
        private static readonly string[] ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\" };
        private static readonly string StartupProjectFileSubPath = @"Resources\Debugger\CS\PackageReferenceTest\PackageReferenceTest.csproj";
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private TextEditor edit;

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            OpenProject(FindProjectFile());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunScript { get; set; }

        protected DotNetProject Project { get; private set; } = new DotNetProject();

        public void StartScript()
        {
            if (window != null)
            {
                window.Dispatcher.BeginInvoke((Action)(() =>
                {
                    SaveIfModified();

                    if (!scriptRun.Compiled)
                    {
                        if (!scriptRun.Compile())
                        {
                            MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                            return;
                        }
                    }

                    scriptRun.RunProcess();
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

        private static string FindProjectFile() =>
           ProjectSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, StartupProjectFileSubPath))).FirstOrDefault(File.Exists);

        private void SaveIfModified()
        {
            if (edit.Source.Modified && !string.IsNullOrEmpty(edit.Source.FileName))
                edit.Source.SaveFile(edit.Source.FileName);
        }

        private void LoadFile(TextEditor edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.Source.FileName = fileName;
        }

        private TextEditor CreateEditor(string fileName, Border border)
        {
            TextEditor edit = new TextEditor();

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            IList<string> references = new List<string>();
            foreach (var reference in Project.References)
                references.Add(reference.FullName);

            var parser = new CsParser();
            parser.Repository.RegisterAssemblies(references.ToArray());
            edit.Lexer = parser;

            LoadFile(edit, fileName);

            return edit;
        }

        private void RunScriptClick()
        {
            StartScript();
        }

        private void OpenProject(string projectFilePath)
        {
            Project.Load(projectFilePath);
            scriptRun.ScriptSource.FromScriptProject(Project.ProjectFileName);
            scriptRun.ScriptHost.GenerateModulesOnDisk = true;

            if (Project.Files.Count > 0)
            {
                edit = CreateEditor(Project.Files[0], window.EditBorder);
            }
        }
    }
}