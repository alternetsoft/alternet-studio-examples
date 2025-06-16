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
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;
using Microsoft.Win32;

namespace RoslynSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string language = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        private TextSource csharpSource = new TextSource();
        private TextSource vbSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private VbParser vbParser1 = new VbParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            fileInfo = new FileInfo(dir + @"Resources\Editor\Text\vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);

            csharpSource.Lexer = csParser1;
            vbSource.Lexer = vbParser1;
            csharpSource.HighlightReferences = true;
            vbSource.HighlightReferences = true;

            languages.Add("C#");
            languages.Add("VB");
            LoadCommand = new RelayCommand(LoadClick);
            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            Language = "C#";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TextSource Source
        {
            get
            {
                switch (language)
                {
                    case "C#":
                        return csharpSource;
                    case "VB":
                        return vbSource;
                    default:
                        return csharpSource;
                }
            }
        }

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return language;
            }

            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged("Language");
                    if (edit != null)
                        edit.Source = Source;
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LoadClick()
        {
            openFileDialog.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb";
            switch (language)
            {
                case "C#":
                    openFileDialog.FilterIndex = 1;
                    break;
                case "VB":
                    openFileDialog.FilterIndex = 2;
                    break;
                default:
                    openFileDialog.FilterIndex = 1;
                    break;
            }

            if (openFileDialog.ShowDialog().Value)
            {
                edit.Source.LoadFile(openFileDialog.FileName);
            }
        }
    }
}
