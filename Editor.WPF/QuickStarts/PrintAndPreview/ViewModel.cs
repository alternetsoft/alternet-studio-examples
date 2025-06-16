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
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace PrintAndPreview
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
            {
                csharpSource.LoadFile(fileInfo.FullName);
                csharpSource.FileName = fileInfo.Name;
            }

            csharpSource.Lexer = csParser1;

            PrintCommand = new RelayCommand(PrintClick);
            PrintPreviewCommand = new RelayCommand(PrintPreviewClick);
            PageSetupCommand = new RelayCommand(PageSetupClick);
            PrintOptionsCommand = new RelayCommand(PrintOptionsClick);
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            edit.Source = csharpSource;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PrintCommand { get; set; }

        public ICommand PrintPreviewCommand { get; set; }

        public ICommand PageSetupCommand { get; set; }

        public ICommand PrintOptionsCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void PrintClick()
        {
            if (edit.Printing.ExecutePrintDialog() == true)
                edit.Printing.Print();
        }

        private void PrintPreviewClick()
        {
            edit.Printing.ExecutePrintPreviewDialog(edit.Parent);
        }

        private void PageSetupClick()
        {
        }

        private void PrintOptionsClick()
        {
            edit.Printing.ExecutePrintOptionsDialog();
        }
    }
}
