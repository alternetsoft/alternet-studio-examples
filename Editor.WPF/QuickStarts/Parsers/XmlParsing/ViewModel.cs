#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Advanced;

namespace XmlParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private const string SubDir = @"Resources\Editor\QuickStarts\Parsers\XML\SimpleSchema";
        private const string BooksXml = "books.xml";
        private const string OrderXml = "order.xml";
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private ComboBox comboBox;
        private string xml = string.Empty;
        private ObservableCollection<string> xmls = new ObservableCollection<string>();
        private int updateCount = 0;
        private XmlParserWithSchema xmlParser = new XmlParserWithSchema();

        #endregion

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + SubDir);
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
                if (!Directory.Exists(Path.GetFullPath(dir) + SubDir))
                    dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            xmls.Add("books");
            xmls.Add("order");
        }

        public ViewModel(TextEditor edit, ComboBox comboBox)
            : this()
        {
            this.edit = edit;
            this.comboBox = comboBox;
            this.edit.Lexer = xmlParser;
            Xml = "books";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Xmls
        {
            get { return xmls; }
            set { xmls = value; }
        }

        public string Xml
        {
            get
            {
                return xml;
            }

            set
            {
                if (xml != value)
                {
                    xml = value;
                    OnPropertyChanged("Xml");
                    if (updateCount > 0)
                        return;

                    if (edit != null)
                    {
                        var fileName = string.Empty;
                        switch (xml)
                        {
                            case "books":
                                fileName = Path.GetFullPath(Path.Combine(dir, SubDir, BooksXml));
                                break;

                            case "order":
                                fileName = Path.GetFullPath(Path.Combine(dir, SubDir, OrderXml));
                                break;
                        }

                        if ((fileName != string.Empty) && File.Exists(fileName))
                        {
                            edit.Source.LoadFile(fileName);
                            edit.Source.FileName = fileName;
                        }
                   }
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
