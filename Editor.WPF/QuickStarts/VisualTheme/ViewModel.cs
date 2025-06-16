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
using System.Drawing;
using System.IO;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace VisualTheme
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string visualTheme = string.Empty;
        private ObservableCollection<string> visualThemes = new ObservableCollection<string>();

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
                csharpSource.LoadFile(fileInfo.FullName);

            csharpSource.Lexer = csParser1;
            csharpSource.HighlightReferences = true;

            foreach (VisualThemeType value in Enum.GetValues(typeof(VisualThemeType)))
                visualThemes.Add(value.ToString());
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            this.edit.VisualTheme = new CustomVisualTheme();
            VisualTheme = this.edit.VisualThemeType.ToString();
            edit.Source = csharpSource;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TextSource Source
        {
            get
            {
                return csharpSource;
            }
        }

        public ObservableCollection<string> VisualThemes
        {
            get { return visualThemes; }
            set { visualThemes = value; }
        }

        public string VisualTheme
        {
            get
            {
                return visualTheme;
            }

            set
            {
                if (visualTheme != value)
                {
                    visualTheme = value;
                    OnPropertyChanged("VisualTheme");
                    if (edit != null)
                    {
                        VisualThemeType theme;
                        if (Enum.TryParse(visualTheme, out theme))
                            edit.VisualThemeType = theme;
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

#pragma warning disable SA1402 // File may only contain a single type
    public class CustomVisualTheme : StandardVisualTheme
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CustomVisualTheme()
            : base("MyCustomTheme")
        {
        }

        protected override VisualThemeColors GetColors()
        {
            var colors = DarkVisualTheme.Instance.Colors.Clone();
            colors.Reswords = System.Drawing.Color.Red;
            colors.WindowBackground = System.Drawing.Color.FromArgb(40, 40, 40);
            return colors;
        }
    }
}
