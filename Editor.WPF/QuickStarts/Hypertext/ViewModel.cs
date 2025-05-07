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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace HyperText
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool highlightUrls;
        private bool customHypertext;
        private System.Windows.Media.Color urlColor;
        private ObservableCollection<string> urlFonts = new ObservableCollection<string>();
        private System.Drawing.FontStyle urlFont = System.Drawing.FontStyle.Regular;
        private string styleName;

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
            string[] s = Enum.GetNames(typeof(System.Drawing.FontStyle));
            foreach (string str in s)
                URLFonts.Add(str);
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            csharpSource.HyperText += CsharpSource_HyperText;
            edit.Source = csharpSource;
            this.edit = edit;
            edit.HyperText.HighlightHyperText = true;
            HighlightURLs = edit.HyperText.HighlightHyperText;
            URLColor = System.Windows.Media.Color.FromRgb(edit.HyperText.UrlColor.R, edit.HyperText.UrlColor.G, edit.HyperText.UrlColor.B);
            StyleName = edit.HyperText.UrlStyle.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> URLFonts
        {
            get { return urlFonts; }
            set { urlFonts = value; }
        }

        public System.Drawing.FontStyle URLFont
        {
            get
            {
                return urlFont;
            }

            set
            {
                if (urlFont != value)
                {
                    urlFont = value;
                    OnPropertyChanged("URLFont");
                    if (edit != null)
                    {
                        edit.HyperText.UrlStyle = urlFont;
                        edit.Invalidate();
                    }
                }
            }
        }

        public string StyleName
        {
            get
            {
                return styleName;
            }

            set
            {
                if (styleName != value)
                {
                    styleName = value;
                    OnPropertyChanged("StyleName");
                    if (edit != null)
                    {
                        switch (styleName)
                        {
                            case "Bold":
                                URLFont = System.Drawing.FontStyle.Bold;
                                break;
                            case "Italic":
                                URLFont = System.Drawing.FontStyle.Italic;
                                break;
                            case "Underline":
                                URLFont = System.Drawing.FontStyle.Underline;
                                break;
                            case "Regular":
                                URLFont = System.Drawing.FontStyle.Regular;
                                break;
                            default:
                                URLFont = System.Drawing.FontStyle.Regular;
                                break;
                        }
                    }
                }
            }
        }

        public bool HighlightURLs
        {
            get
            {
                return highlightUrls;
            }

            set
            {
                if (highlightUrls != value)
                {
                    highlightUrls = value;
                    OnPropertyChanged("HighlightURLs");
                    if (edit != null)
                    {
                        edit.HyperText.HighlightHyperText = highlightUrls;
                        UpdateUrlTable();
                    }
                }
            }
        }

        public bool CustomHypertext
        {
            get
            {
                return customHypertext;
            }

            set
            {
                if (customHypertext != value)
                {
                    customHypertext = value;
                    OnPropertyChanged("CustomHypertext");
                    UpdateUrlTable();
                }
            }
        }

        public System.Windows.Media.Color URLColor
        {
            get
            {
                return urlColor;
            }

            set
            {
                if (urlColor != value)
                {
                    urlColor = value;
                    OnPropertyChanged("URLColor");
                    if (edit != null)
                    {
                        edit.HyperText.UrlColor = Color.FromArgb(urlColor.R, urlColor.G, urlColor.B);
                        edit.Invalidate();
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

        private void CsharpSource_HyperText(object sender, HyperTextEventArgs e)
        {
            if (customHypertext)
                e.IsHyperText = e.Text.IndexOf("<") >= 0;
        }

        private void UpdateUrlTable()
        {
            if (edit != null)
            {
                Hashtable hs = ((TextSource)edit.Source).UrlTable;
                if (hs != null)
                {
                    if (customHypertext)
                    {
                        hs.Add('<', true);
                        hs.Add('>', false);
                    }
                    else
                    {
                        hs.Remove('<');
                        hs.Remove('>');
                    }
                }

                edit.Source.Notification(edit.Lexer, EventArgs.Empty);
                edit.Invalidate();
            }
        }
    }
}
