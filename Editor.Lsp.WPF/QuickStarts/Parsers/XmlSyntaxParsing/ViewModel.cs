#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Alternet.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Xml.Embedded;
using Microsoft.Win32;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace XmlSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private XmlParserEmbedded xmlParser = new XmlParserEmbedded();
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

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\QuickStarts\Parsers\XML\SimpleSchema\books.xml");
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileInfo.FullName;
                edit.Source.LoadFile(fileInfo.FullName);
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";

            edit.Lexer = xmlParser;
            edit.HighlightReferences = true;
            edit.Spelling.SpellColor = Color.Navy;
            edit.KeyDown += SyntaxEdit_KeyDown;

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
            if (XmlParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "LSP Xml Parser Demo",
                Message = "Please wait until the XML LSP server files are extracted...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    XmlParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void LoadClick()
        {
            openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog().Value)
            {
                edit.Source.LoadFile(openFileDialog.FileName);
            }
        }

        private void SyntaxEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
                GoToDefinition();
        }

        private void GoToDefinition()
        {
            var declaration = xmlParser.FindDeclaration(edit.Position);
            if (declaration == null)
                return;

            if (edit.Source.FileName.Equals(declaration.FileName, StringComparison.OrdinalIgnoreCase))
                edit.MakeVisible(new System.Drawing.Point(declaration.Column, declaration.Line), true);
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