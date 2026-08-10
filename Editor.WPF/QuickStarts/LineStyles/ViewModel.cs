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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace LineStyles
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool lineStyleBeyond = false;
        private System.Windows.Media.Color lineStyleColor;
        private string startText = string.Empty;

        private bool startDebug;
        private bool stepOverEnabled = false;
        private int startLine = 44;
        private int endLine = 0;
        private int index;
        private int breakpointStyleIndex;
        private int customStyleIndex;
        private int traceLineStyleIndex;

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
            StartText = "Start";
            StepOverEnabled = false;

            StartDebug = new RelayCommand(StartDebugClick);
            StepOver = new RelayCommand(StepOverClick);
            SetBreakpoint = new RelayCommand(SetBreakpointClick);
            ToggleCustom = new RelayCommand(ToggleCustomClick);
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            if (edit != null)
            {
                IEditLineStyle traceLineStyle = new EditLineStyle
                {
                    BackColor = Color.Black,
                    ForeColor = Color.FromArgb(255, 241, 129),
                    Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                    ImageIndex = (int)KnownImageIndex.TraceLine,
                };
                edit.LineStyles.Add(traceLineStyle);
                traceLineStyleIndex = edit.LineStyles.Count - 1;
                LineStyleBeyond = (LineStyleOptions.BeyondEol & traceLineStyle.Options) != 0;
                LineStyleColor = System.Windows.Media.Color.FromRgb(traceLineStyle.ForeColor.R, traceLineStyle.ForeColor.G, traceLineStyle.ForeColor.B);

                var breakpointStyle = new EditLineStyle() // breakpoint style
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(171, 97, 107),
                    Options = LineStyleOptions.BeyondEol | LineStyleOptions.InvertColors,
                    ImageIndex = (int)KnownImageIndex.Breakpoint,
                };

                edit.LineStyles.Add(breakpointStyle);
                breakpointStyleIndex = edit.LineStyles.Count - 1;
                var customImageIndex = SetImage("gear16green.png", edit.GutterImages);

                var customLineStyle = new EditLineStyle
                {
                    BackColor = Color.Green,
                    ForeColor = Color.White,
                    Options = LineStyleOptions.BeyondEol,
                    ImageIndex = customImageIndex,
                };

                edit.LineStyles.Add(customLineStyle);
                customStyleIndex = edit.LineStyles.Count - 1;

                this.edit = edit;
                edit.Source = csharpSource;
                if (edit.Find("Main"))
                {
                    edit.Selection.Clear();
                    startLine = edit.Position.Y + 2;
                }

                endLine = edit.Lines.Count - 2;

                edit.GutterClick += Edit_GutterClick;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private enum KnownImageIndex
        {
            Breakpoint = 11,

            TraceLine,
        }

        public string StartText
        {
            get
            {
                return startText;
            }

            set
            {
                if (startText != value)
                {
                    startText = value;
                    OnPropertyChanged("StartText");
                }
            }
        }

        public bool StepOverEnabled
        {
            get
            {
                return stepOverEnabled;
            }

            set
            {
                if (stepOverEnabled != value)
                {
                    stepOverEnabled = value;
                    OnPropertyChanged("StepOverEnabled");
                }
            }
        }

        public bool LineStyleBeyond
        {
            get
            {
                return lineStyleBeyond;
            }

            set
            {
                if (lineStyleBeyond != value)
                {
                    lineStyleBeyond = value;
                    OnPropertyChanged("LineStyleBeyond");
                    if ((edit != null) && (edit.LineStyles.Count > 0))
                    {
                        foreach (IEditLineStyle lineStyle in edit.LineStyles)
                        {
                            lineStyle.Options = lineStyleBeyond ? lineStyle.Options | LineStyleOptions.BeyondEol :
                                lineStyle.Options & ~LineStyleOptions.BeyondEol;
                        }

                        edit.Invalidate();
                    }
                }
            }
        }

        public System.Windows.Media.Color LineStyleColor
        {
            get
            {
                return lineStyleColor;
            }

            set
            {
                if (lineStyleColor != value)
                {
                    lineStyleColor = value;
                    OnPropertyChanged("LineStyleColor");
                    if ((edit != null) && (edit.LineStyles.Count > 0))
                    {
                        IEditLineStyle lineStyle = edit.LineStyles[0];
                        lineStyle.ForeColor = System.Drawing.Color.FromArgb(lineStyleColor.R, lineStyleColor.G, lineStyleColor.B);
                        edit.Invalidate();
                    }
                }
            }
        }

        public ICommand StartDebug { get; set; }

        public ICommand StepOver { get; set; }

        public ICommand SetBreakpoint { get; set; }

        public ICommand ToggleCustom { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Edit_GutterClick(object sender, EventArgs e)
        {
            DoSetBreakpoint();
        }

        private void StartDebugClick()
        {
            Start();
        }

        private void StepOverClick()
        {
            DoStepOver();
        }

        private void SetBreakpointClick()
        {
            DoSetBreakpoint();
        }

        private void ToggleCustomClick()
        {
            DoCustomLineStyle();
        }

        private void Debug()
        {
            index = 0;
            StartText = startDebug ? "Start" : "Stop";
            StepOverEnabled = !startDebug;
        }

        private void Start()
        {
            if (edit != null)
            {
                edit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                edit.MakeVisible(new System.Drawing.Point(0, startLine + index));
            }

            Debug();
            startDebug = !startDebug;
        }

        private void DoStepOver()
        {
            if ((edit != null) && (index < (endLine - startLine)))
            {
                if (edit.Source.LineStyles.GetLineStyle(startLine + index) >= 0)
                    edit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                index++;
                while ((index < (endLine - startLine)) && (edit.Source.Lines[startLine + index].Trim() == string.Empty))
                    index++;
                edit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                edit.MakeVisible(new System.Drawing.Point(0, startLine + index));
            }
            else
            {
                edit.Source.LineStyles.ToggleLineStyle(startLine + index, 1, 0);
                Debug();
                startDebug = !startDebug;
            }
        }

        private void DoSetBreakpoint()
        {
            if (edit != null)
            {
                edit.Source.LineStyles.ToggleLineStyle(edit.Position.Y, 0, 1);
            }
        }

        private void DoCustomLineStyle()
        {
            if (edit != null)
            {
                edit.Source.LineStyles.ToggleLineStyle(edit.Position.Y, 0, customStyleIndex);
            }
        }

        private int SetImage(string name, ImageSourceCollection images)
        {
            var assembly = this.GetType().Assembly;
            using (var stream = assembly.GetManifestResourceStream(string.Format("LineStyles.Resources.{0}", name)))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                images.Add(bitmap);
                return images.Count - 1;
            }
        }
    }
}
