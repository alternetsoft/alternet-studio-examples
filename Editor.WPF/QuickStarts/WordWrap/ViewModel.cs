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

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace WordWrap
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool wordWrap = false;
        private bool wrapAtMargin = false;

        private ObservableCollection<string> automatics = new ObservableCollection<string>();

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\spell.txt");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            edit.EditMargin.Visible = true;
            this.edit = edit;
            edit.Source = csharpSource;
            WordWrap = edit.WordWrap;
            WrapAtMargin = edit.WrapAtMargin;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool WordWrap
        {
            get
            {
                return wordWrap;
            }

            set
            {
                if (wordWrap != value)
                {
                    wordWrap = value;
                    OnPropertyChanged("WordWrap");
                    if (edit != null)
                        edit.WordWrap = wordWrap;
                }
            }
        }

        public bool WrapAtMargin
        {
            get
            {
                return wrapAtMargin;
            }

            set
            {
                if (wrapAtMargin != value)
                {
                    wrapAtMargin = value;
                    OnPropertyChanged("WrapAtMargin");
                    if (edit != null)
                        edit.WrapAtMargin = wrapAtMargin;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
