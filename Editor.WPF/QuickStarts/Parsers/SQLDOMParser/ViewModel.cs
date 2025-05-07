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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Microsoft.Win32;

namespace SQLDOMParser
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private TextEditor edit;
        private ISyntaxParser parser = new SQLWrapper();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";

        #endregion

        public ViewModel()
        {
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";

            string path = Path.GetFullPath(Path.Combine(dir, @"Resources\Editor\QuickStarts\Parsers\SQLDOM\databaseObjects.xml"));
            if (File.Exists(path))
                ((SqlWrapRepository)parser.CompletionRepository).LoadDataFromXml(path);

            this.edit = edit;
            this.edit.Lexer = parser;

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\SQLDOM.txt");
            if (fileInfo.Exists)
                this.edit.Source.LoadFile(fileInfo.FullName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
