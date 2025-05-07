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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using Alternet.Common;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;

namespace CodeOutlining
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string automatic = string.Empty;

        private TextSource csharpSource = new TextSource();
        private TextSource textSource2 = new TextSource();
        private CsParser csParser1 = new CsParser();
        private Parser parser1 = new Parser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool allowOutlining = false;
        private bool drawOnGutter = false;
        private bool drawLines = false;
        private bool drawButtons = false;
        private bool showHints = false;

        private ObservableCollection<string> automatics = new ObservableCollection<string>();

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
            }

            fileInfo = new FileInfo(dir + @"Resources\Editor\Text\customOutlining.txt");
            if (fileInfo.Exists)
            {
                textSource2.LoadFile(fileInfo.FullName);
            }

            csharpSource.Lexer = csParser1;
            textSource2.Lexer = parser1;

            fileInfo = new FileInfo(dir + @"Resources\Editor\Schemes\ini.xml");
            if (fileInfo.Exists)
                parser1.Scheme.LoadFile(fileInfo.FullName);
            automatics.Add("Automatic");
            automatics.Add("Custom");
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            Automatic = "Automatic";
            edit.AllowOutlining = true;
            this.edit = edit;

            edit.Source = csharpSource;
            AllowOutlining = edit.AllowOutlining;
            DrawOnGutter = (edit.Outlining.OutlineOptions & OutlineOptions.DrawOnGutter) != 0;
            DrawLines = (edit.Outlining.OutlineOptions & OutlineOptions.DrawLines) != 0;
            DrawButtons = (edit.Outlining.OutlineOptions & OutlineOptions.DrawButtons) != 0;
            ShowHints = (edit.Outlining.OutlineOptions & OutlineOptions.ShowHints) != 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Automatics
        {
            get { return automatics; }
            set { automatics = value; }
        }

        public bool AllowOutlining
        {
            get
            {
                return allowOutlining;
            }

            set
            {
                if (allowOutlining != value)
                {
                    allowOutlining = value;
                    OnPropertyChanged("AllowOutlining");
                    if (edit != null)
                    {
                        UpdateOutlining(automatic == "Automatic");
                        edit.AllowOutlining = allowOutlining;
                    }
                }
            }
        }

        public bool DrawOnGutter
        {
            get
            {
                return drawOnGutter;
            }

            set
            {
                if (drawOnGutter != value)
                {
                    drawOnGutter = value;
                    OnPropertyChanged("DrawOnGutter");
                    if (edit != null)
                    {
                        edit.Outlining.OutlineOptions = drawOnGutter ? edit.Outlining.OutlineOptions
                        | OutlineOptions.DrawOnGutter : edit.Outlining.OutlineOptions & ~OutlineOptions.DrawOnGutter;
                    }
                }
            }
        }

        public bool DrawLines
        {
            get
            {
                return drawLines;
            }

            set
            {
                if (drawLines != value)
                {
                    drawLines = value;
                    OnPropertyChanged("DrawLines");
                    if (edit != null)
                    {
                        edit.Outlining.OutlineOptions = drawLines ? edit.Outlining.OutlineOptions
                        | OutlineOptions.DrawLines : edit.Outlining.OutlineOptions & ~OutlineOptions.DrawLines;
                    }
                }
            }
        }

        public bool DrawButtons
        {
            get
            {
                return drawButtons;
            }

            set
            {
                if (drawButtons != value)
                {
                    drawButtons = value;
                    OnPropertyChanged("DrawButtons");
                    if (edit != null)
                    {
                        edit.Outlining.OutlineOptions = drawButtons ? edit.Outlining.OutlineOptions
                        | OutlineOptions.DrawButtons : edit.Outlining.OutlineOptions & ~OutlineOptions.DrawButtons;
                    }
                }
            }
        }

        public bool ShowHints
        {
            get
            {
                return showHints;
            }

            set
            {
                if (showHints != value)
                {
                    showHints = value;
                    OnPropertyChanged("ShowHints");
                    if (edit != null)
                    {
                        edit.Outlining.OutlineOptions = showHints ? edit.Outlining.OutlineOptions
                        | OutlineOptions.ShowHints : edit.Outlining.OutlineOptions & ~OutlineOptions.ShowHints;
                    }
                }
            }
        }

        public string Automatic
        {
            get
            {
                return automatic;
            }

            set
            {
                if (automatic != value)
                {
                    automatic = value;
                    OnPropertyChanged("Automatic");
                    UpdateOutlining(automatic == "Automatic");
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

        private void DoCustomOutlining()
        {
            if (edit != null)
            {
                System.Drawing.Point oldPos = edit.Position;
                edit.Source.BeginUpdate();
                try
                {
                    if (edit.Find(@"\[.*\]", SearchOptions.EntireScope | SearchOptions.RegularExpressions, new System.Text.RegularExpressions.Regex(@"\[.*\]", System.Text.RegularExpressions.RegexOptions.Singleline)))
                    {
                        IList<IRange> ranges = new List<IRange>();
                        System.Drawing.Point start = edit.Position;
                        while (edit.FindNext())
                        {
                            ranges.Add(new OutlineRange(start, PrevPosition(edit.Position), 0, "..."));
                            start = edit.Position;
                        }

                        ranges.Add(new OutlineRange(start, new System.Drawing.Point(edit.Lines[edit.Lines.Count - 1].Length, edit.Lines.Count - 1), 0, "..."));
                        edit.Outlining.SetOutlineRanges(ranges, true);
                    }

                    edit.Selection.Clear();
                }
                finally
                {
                    edit.MoveTo(oldPos);
                    edit.Source.EndUpdate();
                }
            }
        }

        private System.Drawing.Point PrevPosition(System.Drawing.Point position)
        {
            System.Drawing.Point pos = position;
            if (pos.Y > 0)
            {
                pos.Y--;
                pos.X = (edit != null) ? Math.Max(0, edit.Strings[pos.Y].Length - 1) : pos.X;
            }
            else
                pos.X--;
            return pos;
        }

        private void UpdateOutlining(bool automatic)
        {
            if (edit != null)
                edit.Source = automatic ? csharpSource : textSource2;
            if (!automatic)
                DoCustomOutlining();
        }
    }
}
