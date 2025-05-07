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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.XAML;
using Microsoft.Win32;

namespace XAMLParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private XAMLParser parser = new XAMLParser();
        #endregion

        public ViewModel()
        {
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            this.edit.Lexer = parser;
            this.edit.AllowOutlining = true;
            this.edit.WordWrap = true;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
                dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            }

            string path = Path.Combine(dirInfo.FullName, "xaml.xaml");
            if (File.Exists(path))
            {
                this.edit.Source.FileName = path;
                this.edit.Source.LoadFile(path);
            }
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
