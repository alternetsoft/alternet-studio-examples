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
using System.ComponentModel;
using System.IO;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace CodeSnippets
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private TextSource vbSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private VbParser vbParser1 = new VbParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool csharpSnippets;
        private bool vbSnippets;

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            CSharpSnippets = true;
        }

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
            fileInfo = new FileInfo(dir + @"Resources\Editor\Text\vb_net.txt");
            if (fileInfo.Exists)
                vbSource.LoadFile(fileInfo.FullName);
            vbSource.Lexer = vbParser1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CSharpSnippets
        {
            get
            {
                return csharpSnippets;
            }

            set
            {
                if (csharpSnippets != value)
                {
                    csharpSnippets = value;
                    OnPropertyChanged("CSharpSnippets");
                    UpdateSnippets();
                }
            }
        }

        public bool VBSnippets
        {
            get
            {
                return vbSnippets;
            }

            set
            {
                if (vbSnippets != value)
                {
                    vbSnippets = value;
                    OnPropertyChanged("VBSnippets");
                    UpdateSnippets();
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

        private void UpdateSnippets()
        {
            if (csharpSnippets)
                edit.Source = csharpSource;
            else
                edit.Source = vbSource;
        }
    }
}
