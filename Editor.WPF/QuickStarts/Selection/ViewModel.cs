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

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace Selection
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool disableSelection = false;
        private bool disableDragging = false;
        private bool selectBeyondEol = false;
        private bool useColors = false;
        private bool hideSelection = false;
        private bool selectLineOnDblClick = false;
        private bool highlightSelectedWords = false;
        private bool persistentBlocks = false;
        private bool overwriteBlocks = false;
        private System.Windows.Media.Color selectForeColor;
        private System.Windows.Media.Color selectBackColor;
        private System.Windows.Media.Color selectBorderColor;

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
            if (edit != null)
            {
                this.edit = edit;
                edit.Source = csharpSource;
                edit.Selection.Options |= SelectionOptions.DrawBorder;
                DisableSelection = (SelectionOptions.DisableSelection & edit.Selection.Options) != 0;
                DisableDragging = (SelectionOptions.DisableDragging & edit.Selection.Options) != 0;
                SelectBeyondEol = (SelectionOptions.SelectBeyondEol & edit.Selection.Options) != 0;
                UseColors = (SelectionOptions.UseColors & edit.Selection.Options) != 0;
                HideSelection = (SelectionOptions.HideSelection & edit.Selection.Options) != 0;
                SelectLineOnDblClick = (SelectionOptions.SelectLineOnDblClick & edit.Selection.Options) != 0;
                HighlightSelectedWords = (SelectionOptions.HighlightSelectedWords & edit.Selection.Options) != 0;
                PersistentBlocks = (SelectionOptions.PersistentBlocks & edit.Selection.Options) != 0;
                OverwriteBlocks = (SelectionOptions.OverwriteBlocks & edit.Selection.Options) != 0;
                if (edit.SelectionBrush is System.Windows.Media.SolidColorBrush)
                    SelectBackColor = ((System.Windows.Media.SolidColorBrush)edit.SelectionBrush).Color;
                SelectForeColor = edit.SelectionForeColor;
                SelectBorderColor = edit.SectionBorderColor;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Media.Color SelectForeColor
        {
            get
            {
                return selectForeColor;
            }

            set
            {
                if (selectForeColor != value)
                {
                    selectForeColor = value;
                    OnPropertyChanged("SelectForeColor");
                    if (edit != null)
                    {
                        edit.SelectionForeColor = SelectForeColor;
                        edit.Invalidate();
                    }
                }
            }
        }

        public System.Windows.Media.Color SelectBackColor
        {
            get
            {
                return selectBackColor;
            }

            set
            {
                if (selectBackColor != value)
                {
                    selectBackColor = value;
                    OnPropertyChanged("SelectBackColor");
                    if (edit != null)
                    {
                        edit.SelectionBrush = new System.Windows.Media.SolidColorBrush(selectBackColor);
                        edit.Invalidate();
                    }
                }
            }
        }

        public System.Windows.Media.Color SelectBorderColor
        {
            get
            {
                return selectBorderColor;
            }

            set
            {
                if (selectBorderColor != value)
                {
                    selectBorderColor = value;
                    OnPropertyChanged("SelectBorderColor");
                    if (edit != null)
                    {
                        edit.SectionBorderColor = selectBorderColor;
                        edit.Invalidate();
                    }
                }
            }
        }

        public bool DisableSelection
        {
            get
            {
                return disableSelection;
            }

            set
            {
                if (disableSelection != value)
                {
                    disableSelection = value;
                    OnPropertyChanged("DisableSelection");
                    if (edit != null)
                    {
                        edit.Selection.Options = disableSelection ? edit.Selection.Options
                            | SelectionOptions.DisableSelection : edit.Selection.Options & ~SelectionOptions.DisableSelection;
                    }
                }
            }
        }

        public bool DisableDragging
        {
            get
            {
                return disableDragging;
            }

            set
            {
                if (disableDragging != value)
                {
                    disableDragging = value;
                    OnPropertyChanged("DisableDragging");
                    if (edit != null)
                    {
                        edit.Selection.Options = disableDragging ? edit.Selection.Options
                            | SelectionOptions.DisableDragging : edit.Selection.Options & ~SelectionOptions.DisableDragging;
                    }
                }
            }
        }

        public bool SelectBeyondEol
        {
            get
            {
                return selectBeyondEol;
            }

            set
            {
                if (selectBeyondEol != value)
                {
                    selectBeyondEol = value;
                    OnPropertyChanged("SelectBeyondEol");
                    if (edit != null)
                    {
                        edit.Selection.Options = selectBeyondEol ? edit.Selection.Options
                            | SelectionOptions.SelectBeyondEol : edit.Selection.Options & ~SelectionOptions.SelectBeyondEol;
                    }
                }
            }
        }

        public bool UseColors
        {
            get
            {
                return useColors;
            }

            set
            {
                if (useColors != value)
                {
                    useColors = value;
                    OnPropertyChanged("UseColors");
                    if (edit != null)
                    {
                        edit.Selection.Options = useColors ? edit.Selection.Options
                            | SelectionOptions.UseColors : edit.Selection.Options & ~SelectionOptions.UseColors;
                    }
                }
            }
        }

        public bool HideSelection
        {
            get
            {
                return hideSelection;
            }

            set
            {
                if (hideSelection != value)
                {
                    hideSelection = value;
                    OnPropertyChanged("HideSelection");
                    if (edit != null)
                    {
                        edit.Selection.Options = hideSelection ? edit.Selection.Options
                            | SelectionOptions.HideSelection : edit.Selection.Options & ~SelectionOptions.HideSelection;
                    }
                }
            }
        }

        public bool SelectLineOnDblClick
        {
            get
            {
                return selectLineOnDblClick;
            }

            set
            {
                if (selectLineOnDblClick != value)
                {
                    selectLineOnDblClick = value;
                    OnPropertyChanged("SelectLineOnDblClick");
                    if (edit != null)
                    {
                        edit.Selection.Options = selectLineOnDblClick ? edit.Selection.Options
                            | SelectionOptions.SelectLineOnDblClick : edit.Selection.Options & ~SelectionOptions.SelectLineOnDblClick;
                    }
                }
            }
        }

        public bool HighlightSelectedWords
        {
            get
            {
                return highlightSelectedWords;
            }

            set
            {
                if (highlightSelectedWords != value)
                {
                    highlightSelectedWords = value;
                    OnPropertyChanged("HighlightSelectedWords");
                    if (edit != null)
                    {
                        edit.Selection.Options = highlightSelectedWords ? edit.Selection.Options
                            | SelectionOptions.HighlightSelectedWords : edit.Selection.Options & ~SelectionOptions.HighlightSelectedWords;
                    }
                }
            }
        }

        public bool PersistentBlocks
        {
            get
            {
                return persistentBlocks;
            }

            set
            {
                if (persistentBlocks != value)
                {
                    persistentBlocks = value;
                    OnPropertyChanged("PersistentBlocks");
                    if (edit != null)
                    {
                        edit.Selection.Options = persistentBlocks ? edit.Selection.Options
                            | SelectionOptions.PersistentBlocks : edit.Selection.Options & ~SelectionOptions.PersistentBlocks;
                    }
                }
            }
        }

        public bool OverwriteBlocks
        {
            get
            {
                return overwriteBlocks;
            }

            set
            {
                if (overwriteBlocks != value)
                {
                    overwriteBlocks = value;
                    OnPropertyChanged("OverwriteBlocks");
                    if (edit != null)
                    {
                        edit.Selection.Options = overwriteBlocks ? edit.Selection.Options
                            | SelectionOptions.OverwriteBlocks : edit.Selection.Options & ~SelectionOptions.OverwriteBlocks;
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
