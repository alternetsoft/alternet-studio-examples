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
using Alternet.Syntax.Parsers.Lsp.PowerShell.Embedded;
using Microsoft.Win32;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace PowerShellParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private PowerShellParserEmbedded powerShellParser1 = new PowerShellParserEmbedded();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\";
        private TextEditor edit;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            DeployServer();
            this.edit = edit;
            edit.Lexer = powerShellParser1;
            edit.Outlining.AllowOutlining = true;

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\powershell.txt");
            if (fileInfo.Exists)
            {
                edit.Source.FileName = fileInfo.FullName;
                edit.Source.LoadFile(fileInfo.FullName);
            }

            edit.Source.HighlightReferences = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void DeployServer()
        {
            if (PowerShellParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "LSP PowerShell Parser Demo",
                Message = "Please wait until the PowerShell LSP server files are extracted...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    PowerShellParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }
    }
}
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates