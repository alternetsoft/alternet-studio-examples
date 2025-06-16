#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Lsp.Python.Embedded.Pyright;
using Microsoft.Win32;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace PythonSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private PythonParserEmbedded pythonParser = new PythonParserEmbedded();
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

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\python.txt");
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileInfo.FullName;
                edit.Source.LoadFile(fileInfo.FullName);
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";

            edit.Lexer = pythonParser;
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
            if (PythonParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "LSP Python Parser Demo",
                Message = "Please wait until the Python LSP server files are extracted...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    PythonParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void LoadClick()
        {
            openFileDialog.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog().Value)
            {
                edit.Source.LoadFile(openFileDialog.FileName);
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