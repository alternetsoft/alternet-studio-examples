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

namespace MultipleView
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private TextEditor splitEdit;
        private MainWindow window;

        private bool horzSplit = false;
        private bool vertSplit = false;
        private bool horzButtons = false;
        private bool vertButtons = false;
        private bool scrollHint = false;
        private bool smoothScroll = false;
        private bool systemScroll = false;
        private bool scrollBarAnnotations = false;

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

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            this.edit = window.syntaxEdit1;
            this.splitEdit = window.syntaxEdit2;
            edit.Source = csharpSource;
            splitEdit.Source = csharpSource;
            HorzSplit = edit.AllowHorizontalEditorSplit;
            VertSplit = edit.AllowVerticalEditorSplit;
            HorzButtons = (ScrollingOptions.HorzButtons & edit.Scrolling.Options) != 0;
            VertButtons = (ScrollingOptions.VertButtons & edit.Scrolling.Options) != 0;
            ScrollHint = (edit.Scrolling.Options & ScrollingOptions.ShowScrollHint) != 0;
            SmoothScroll = (edit.Scrolling.Options & ScrollingOptions.SmoothScroll) != 0;
            SystemScroll = (edit.Scrolling.Options & ScrollingOptions.SystemScrollbars) != 0;
            ScrollBarAnnotations = (edit.Scrolling.Options & ScrollingOptions.VerticalScrollBarAnnotations) != 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HorzSplit
        {
            get
            {
                return horzSplit;
            }

            set
            {
                if (horzSplit != value)
                {
                    horzSplit = value;
                    OnPropertyChanged("HorzSplit");
                    if (edit != null)
                        edit.AllowHorizontalEditorSplit = horzSplit;
                    if (splitEdit != null)
                        splitEdit.AllowHorizontalEditorSplit = horzSplit;
                }
            }
        }

        public bool VertSplit
        {
            get
            {
                return vertSplit;
            }

            set
            {
                if (vertSplit != value)
                {
                    vertSplit = value;
                    OnPropertyChanged("VertSplit");
                    if (edit != null)
                        edit.AllowVerticalEditorSplit = vertSplit;
                    if (splitEdit != null)
                        splitEdit.AllowVerticalEditorSplit = vertSplit;
                }
            }
        }

        public bool ScrollBarAnnotations
        {
            get
            {
                return scrollBarAnnotations;
            }

            set
            {
                if (scrollBarAnnotations != value)
                {
                    scrollBarAnnotations = value;
                    OnPropertyChanged("ScrollBarAnnotations");
                    if (edit != null)
                        edit.Scrolling.Options = scrollBarAnnotations ? edit.Scrolling.Options | ScrollingOptions.VerticalScrollBarAnnotations : edit.Scrolling.Options & ~ScrollingOptions.VerticalScrollBarAnnotations;
                    if (splitEdit != null)
                        splitEdit.Scrolling.Options = scrollBarAnnotations ? edit.Scrolling.Options | ScrollingOptions.VerticalScrollBarAnnotations : edit.Scrolling.Options & ~ScrollingOptions.VerticalScrollBarAnnotations;
                }
            }
        }

        public bool HorzButtons
        {
            get
            {
                return horzButtons;
            }

            set
            {
                if (horzButtons != value)
                {
                    horzButtons = value;
                    OnPropertyChanged("HorzButtons");
                }
            }
        }

        public bool VertButtons
        {
            get
            {
                return vertButtons;
            }

            set
            {
                if (vertButtons != value)
                {
                    vertButtons = value;
                    OnPropertyChanged("VertButtons");
                }
            }
        }

        public bool ScrollHint
        {
            get
            {
                return scrollHint;
            }

            set
            {
                if (scrollHint != value)
                {
                    scrollHint = value;
                    OnPropertyChanged("ScrollHint");
                    if (edit != null)
                        edit.Scrolling.Options = scrollHint ? edit.Scrolling.Options | ScrollingOptions.ShowScrollHint : edit.Scrolling.Options & ~ScrollingOptions.ShowScrollHint;
                    if (splitEdit != null)
                        splitEdit.Scrolling.Options = scrollHint ? edit.Scrolling.Options | ScrollingOptions.ShowScrollHint : edit.Scrolling.Options & ~ScrollingOptions.ShowScrollHint;
                }
            }
        }

        public bool SmoothScroll
        {
            get
            {
                return smoothScroll;
            }

            set
            {
                if (smoothScroll != value)
                {
                    smoothScroll = value;
                    OnPropertyChanged("SmoothScroll");
                    if (edit != null)
                        edit.Scrolling.Options = smoothScroll ? edit.Scrolling.Options | ScrollingOptions.SmoothScroll : edit.Scrolling.Options & ~ScrollingOptions.SmoothScroll;
                    if (splitEdit != null)
                        splitEdit.Scrolling.Options = smoothScroll ? edit.Scrolling.Options | ScrollingOptions.SmoothScroll : edit.Scrolling.Options & ~ScrollingOptions.SmoothScroll;
                    window.chbScrollHint.IsEnabled = smoothScroll;
                }
            }
        }

        public bool SystemScroll
        {
            get
            {
                return systemScroll;
            }

            set
            {
                if (systemScroll != value)
                {
                    systemScroll = value;
                    OnPropertyChanged("SystemScroll");
                    if (edit != null)
                        edit.Scrolling.Options = systemScroll ? edit.Scrolling.Options | ScrollingOptions.SystemScrollbars : edit.Scrolling.Options & ~ScrollingOptions.SystemScrollbars;
                    if (splitEdit != null)
                        splitEdit.Scrolling.Options = systemScroll ? edit.Scrolling.Options | ScrollingOptions.SystemScrollbars : edit.Scrolling.Options & ~ScrollingOptions.SystemScrollbars;
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
