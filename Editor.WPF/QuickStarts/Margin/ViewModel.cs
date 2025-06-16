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
using System.Text.RegularExpressions;
using System.Windows.Media;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace Margin
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private decimal userMarginWidth = 0;
        private int marginPos = 0;

        private bool paintUserMargin = false;
        private bool showMargin = false;
        private bool showColumns = false;
        private string userMaginText;
        private Regex userMarginRegex;

        private System.Windows.Media.Color foreColor;
        private System.Windows.Media.Color backColor;
        private System.Windows.Media.Color marginColor;
        private System.Windows.Media.Color columnColor;

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
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            edit.AllowOutlining = true;
            edit.EditMargin.Visible = true;
            edit.UserMarginWidth = 90;
            edit.UserMarginText = @"\[chars] chars";
            edit.GetUserMarginText += Edit_GetUserMarginText;
            this.edit = edit;
            edit.Source = csharpSource;
            UserMarginWidth = (decimal)edit.UserMarginWidth;
            edit.EditMargin.Position = 60;
            MarginPos = edit.EditMargin.Position;
            PaintUserMargin = edit.UserMarginVisible;
            ShowMargin = edit.EditMargin.Visible;
            ShowColumns = edit.EditMargin.ColumnsVisible;
            UserMaginText = edit.UserMarginText;
            if (edit.UserMarginBackgroundBrush is SolidColorBrush)
                UserMarginBackColor = ((SolidColorBrush)edit.UserMarginBackgroundBrush).Color;
            if (edit.UserMarginTextBrush is SolidColorBrush)
                UserMarginForeColor = ((SolidColorBrush)edit.UserMarginTextBrush).Color;

            if (edit.MarginPen.Brush is SolidColorBrush)
                MarginColor = ((SolidColorBrush)edit.MarginPen.Brush).Color;
            if (edit.ColumnPen.Brush is SolidColorBrush)
                ColumnColor = ((SolidColorBrush)edit.MarginPen.Brush).Color;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Media.Color UserMarginForeColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                if (foreColor != value)
                {
                    foreColor = value;
                    OnPropertyChanged("UserMarginForeColor");
                    if (edit != null)
                        edit.UserMarginTextBrush = new SolidColorBrush(foreColor);
                }
            }
        }

        public System.Windows.Media.Color UserMarginBackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                if (backColor != value)
                {
                    backColor = value;
                    OnPropertyChanged("UserMarginBackColor");
                    if (edit != null)
                        edit.UserMarginBackgroundBrush = new SolidColorBrush(backColor);
                }
            }
        }

        public System.Windows.Media.Color MarginColor
        {
            get
            {
                return marginColor;
            }

            set
            {
                if (marginColor != value)
                {
                    marginColor = value;
                    OnPropertyChanged("MarginColor");
                    if (edit != null)
                        edit.MarginPen = new System.Windows.Media.Pen(new SolidColorBrush(marginColor), 1);
                }
            }
        }

        public System.Windows.Media.Color ColumnColor
        {
            get
            {
                return columnColor;
            }

            set
            {
                if (columnColor != value)
                {
                    columnColor = value;
                    OnPropertyChanged("ColumnColor");
                    if (edit != null)
                        edit.ColumnPen = new System.Windows.Media.Pen(new SolidColorBrush(columnColor), 1);
                }
            }
        }

        public decimal UserMarginWidth
        {
            get
            {
                return userMarginWidth;
            }

            set
            {
                if (userMarginWidth != value)
                {
                    userMarginWidth = value;
                    OnPropertyChanged("UserMarginWidth");
                    if (edit != null)
                        edit.UserMarginWidth = (double)userMarginWidth;
                }
            }
        }

        public int MarginPos
        {
            get
            {
                return marginPos;
            }

            set
            {
                if (marginPos != value)
                {
                    marginPos = value;
                    OnPropertyChanged("MarginPos");
                    if (edit != null)
                        edit.EditMargin.Position = marginPos;
                }
            }
        }

        public bool PaintUserMargin
        {
            get
            {
                return paintUserMargin;
            }

            set
            {
                if (paintUserMargin != value)
                {
                    paintUserMargin = value;
                    OnPropertyChanged("PaintUserMargin");
                    if (edit != null)
                        edit.UserMarginVisible = paintUserMargin;
                }
            }
        }

        public bool ShowMargin
        {
            get
            {
                return showMargin;
            }

            set
            {
                if (showMargin != value)
                {
                    showMargin = value;
                    OnPropertyChanged("ShowMargin");
                    if (edit != null)
                        edit.EditMargin.Visible = showMargin;
                }
            }
        }

        public bool ShowColumns
        {
            get
            {
                return showColumns;
            }

            set
            {
                if (showColumns != value)
                {
                    showColumns = value;
                    OnPropertyChanged("ShowColumns");
                    if (edit != null)
                        edit.EditMargin.ColumnsVisible = showColumns;
                }
            }
        }

        public string UserMaginText
        {
            get
            {
                return userMaginText;
            }

            set
            {
                if (userMaginText != value)
                {
                    userMaginText = value;
                    OnPropertyChanged("UserMaginText");
                    if (edit != null)
                        edit.UserMarginText = userMaginText;
                }
            }
        }

        private Regex UserMarginRegex
        {
            get
            {
                if (userMarginRegex == null)
                    userMarginRegex = new Regex(@"\\\[[a-zA-Z_0-9]+\]", RegexOptions.Singleline);
                return userMarginRegex;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string GetUserMarginText(string text, int line, int chars)
        {
            MatchCollection matches = UserMarginRegex.Matches(text);
            Match match;
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                match = matches[i];
                if (match.Success)
                {
                    string tag = text.Substring(match.Index, match.Length);
                    if (string.Compare(tag, EditConsts.LineTag) == 0)
                        tag = (line + 1).ToString();
                    if (string.Compare(tag, EditConsts.CharsTag) == 0)
                        tag = chars.ToString();
                    text = text.Remove(match.Index, match.Length);
                    text = text.Insert(match.Index, tag);
                }
            }

            return text;
        }

        private void Edit_GetUserMarginText(object sender, UserMarginTextEventArgs e)
        {
            e.Text = GetUserMarginText(UserMaginText, e.Line, edit.Lines.GetLength(e.Line));
            e.Handled = true;
        }
    }
}
