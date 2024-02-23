#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Clangd.Embedded;
using Microsoft.Win32;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace CLangSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private CPlusPlusParserEmbedded cppParser = new CPlusPlusParserEmbedded();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\";
        private TextEditor edit;
        private Window window;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
        }

        public ViewModel(Window window, TextEditor edit)
        {
            DeployServer();
            this.edit = edit;
            this.window = window;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\QuickStarts\Parsers\Clangd");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\QuickStarts\Parsers\Clangd\src\dynamicvoronoi.cpp");
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileInfo.FullName;
                edit.Source.LoadFile(fileInfo.FullName);

                cppParser.Workspace.Project = new CProject();
                cppParser.Workspace.Project.SourceFiles.Add(fileInfo.FullName);
                cppParser.Workspace.Project.IncludeDirectories.Add(
                    Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fileInfo.FullName), "../include")));

                var dirs = new List<string>();
                GetHeaderPaths(dirs);
                foreach (var dir in dirs)
                    cppParser.Workspace.Project.IncludeDirectories.Add(dir);
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\QuickStarts\Parsers\Clangd\";

            edit.Lexer = cppParser;
            edit.HighlightReferences = true;
            edit.Spelling.SpellColor = Color.Navy;
            InitEditor();

            LoadCommand = new RelayCommand(LoadClick);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void DeployServer()
        {
            if (CPlusPlusParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "LSP C++ Parser Demo",
                Message = "Please wait until the C++ LSP server files are extracted...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    CPlusPlusParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void GetHeaderPaths(IList<string> directories)
        {
            string programFilesX86 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86);
            DirectoryInfo kitsDirectory = new DirectoryInfo(Path.Combine(programFilesX86, "Windows Kits"));
            if (kitsDirectory.Exists)
            {
                var includePath = kitsDirectory
                    .GetDirectories("Include", SearchOption.AllDirectories)
                    .SelectMany(ucrtDir => ucrtDir.GetDirectories("ucrt", SearchOption.AllDirectories))
                    .OrderByDescending(msBuild => msBuild.LastWriteTimeUtc)
                    .FirstOrDefault()?.FullName;

                if (!string.IsNullOrEmpty(includePath))
                    directories.Add(includePath);
            }

            string programFiles = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            DirectoryInfo vsDirectory = new DirectoryInfo(Path.Combine(programFiles, "Microsoft Visual Studio"));
            if (vsDirectory.Exists)
            {
                var includePath = vsDirectory
                    .GetDirectories("VC", SearchOption.AllDirectories)
                    .SelectMany(toolsDir => toolsDir.GetDirectories("Tools", SearchOption.AllDirectories))
                    .SelectMany(msvcDir => msvcDir.GetDirectories("MSVC", SearchOption.TopDirectoryOnly))
                    .SelectMany(includeDir => includeDir.GetDirectories("include", SearchOption.AllDirectories))
                    .OrderByDescending(msBuild => msBuild.LastWriteTimeUtc)
                    .FirstOrDefault()?.FullName;

                if (!string.IsNullOrEmpty(includePath))
                {
                    directories.Add(includePath);
                }
            }
        }

        private void LoadClick()
        {
            openFileDialog.Filter = "C/C++ files (*.c;*.cpp;*.h)|*.c;*.cpp;*.h|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog().Value)
            {
                edit.Source.LoadFile(openFileDialog.FileName);
                edit.Source.FileName = openFileDialog.FileName;
            }
        }

        private void InitEditor()
        {
            var gotoDefinitionMenuItem = new MenuItem();
            gotoDefinitionMenuItem.Header = "Go to Definition";
            gotoDefinitionMenuItem.Command = new RelayCommand(GotoDefinitionMenuItem_Click);
            gotoDefinitionMenuItem.Name = "cmiGotoDefinition";
            gotoDefinitionMenuItem.InputGestureText = "F12";

            edit.InputBindings.Add(new KeyBinding(gotoDefinitionMenuItem.Command, new KeyGesture(Key.F12)));
            edit.DefaultMenu.Items.Add(gotoDefinitionMenuItem);
            edit.KeyDown += Edit_KeyDown;
            edit.Spelling.SpellColor = Color.Navy;
        }

        private void Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
                GoToDefinition();
        }

        private void GotoDefinitionMenuItem_Click()
        {
            GoToDefinition();
        }

        private async void GoToDefinition()
        {
            var parser = edit.Lexer as ISyntaxParser;
            var declaration = await parser.FindDeclarationAsync(edit.Position);
            if (declaration == null)
                return;

            if (edit.Source.FileName.Equals(declaration.FileName, StringComparison.OrdinalIgnoreCase))
            {
                edit.MoveTo(new System.Drawing.Point(declaration.Column, declaration.Line));
                edit.MakeVisible(new System.Drawing.Point(declaration.Column, declaration.Line), true);
            }
            else
            {
                MessageBox.Show(
                    window,
                    $"Definition: {declaration.FileName} ({declaration.Line}:{declaration.Column})",
                    "Go To Definition",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates