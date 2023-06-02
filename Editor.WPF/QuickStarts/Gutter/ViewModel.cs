#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace Gutter
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool showGutter = false;
        private bool renderCustomGutterItems = true;
        private bool lineNumbersVisible = false;
        private bool linesOnGutter = false;
        private bool useGradient = false;

        private System.Windows.Media.Color gutterColor;
        private System.Windows.Media.Color penColor;
        private System.Windows.Media.Color gradientBeginColor;
        private System.Windows.Media.Color gradientEndColor;
        private System.Windows.Media.Color foreColor;
        private System.Windows.Media.Color backColor;
        private double gutterWidth;
        private double leftIndent;
        private double rightIndent;
        private double numberStart;

        private ObservableCollection<string> alignments = new ObservableCollection<string>();
        private string alignment;

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
            edit.LineNumbersVisible = true;
            this.edit = edit;
            edit.Source = csharpSource;

            SetupCustomGutterItems();

            NumberStart = edit.LineNumbersStart;
            ShowGutter = edit.GutterVisible;
            LineNumbersVisible = edit.LineNumbersVisible;
            LinesOnGutter = edit.LinesOnGutter;
            if (edit.GutterBrush is System.Windows.Media.SolidColorBrush)
                GutterColor = ((System.Windows.Media.SolidColorBrush)edit.GutterBrush).Color;
            GutterWidth = edit.GutterWidth;
            LeftIndent = edit.LineNumbersLeftMargin;
            RightIndent = edit.LineNumbersRightMargin;
            string[] aligns = Enum.GetNames(typeof(TextAlignment));
            foreach (string s in aligns)
                alignments.Add(s);
            Alignment = edit.LineNumbersHorizontalAlignment.ToString();
            GradientBeginColor = edit.GutterGradientStartColor;
            GradientEndColor = edit.GutterGradientEndColor;
            UseGradient = edit.GradientGutter;
            if (edit.LineNumbersBrush is System.Windows.Media.SolidColorBrush)
                ForeColor = ((System.Windows.Media.SolidColorBrush)edit.LineNumbersBrush).Color;
            if (edit.LineNumbersBackBrush is System.Windows.Media.SolidColorBrush)
                BackColor = ((System.Windows.Media.SolidColorBrush)edit.LineNumbersBackBrush).Color;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Alignments
        {
            get { return alignments; }
            set { alignments = value; }
        }

        public string Alignment
        {
            get
            {
                return alignment;
            }

            set
            {
                if (alignment != value)
                {
                    alignment = value;
                    OnPropertyChanged("Alignment");
                    if (edit != null)
                    {
                        switch (alignment)
                        {
                            case "Center":
                                edit.LineNumbersHorizontalAlignment = TextAlignment.Center;
                                break;
                            case "Left":
                                edit.LineNumbersHorizontalAlignment = TextAlignment.Left;
                                break;
                            case "Right":
                                edit.LineNumbersHorizontalAlignment = TextAlignment.Right;
                                break;
                            case "Justify":
                                edit.LineNumbersHorizontalAlignment = TextAlignment.Justify;
                                break;
                            default:
                                edit.LineNumbersHorizontalAlignment = TextAlignment.Left;
                                break;
                        }
                    }
                }
            }
        }

        public System.Windows.Media.Color GutterColor
        {
            get
            {
                return gutterColor;
            }

            set
            {
                if (gutterColor != value)
                {
                    gutterColor = value;
                    OnPropertyChanged("GutterColor");
                    if (edit != null)
                        edit.GutterBrush = new System.Windows.Media.SolidColorBrush(gutterColor);
                }
            }
        }

        public System.Windows.Media.Color PenColor
        {
            get
            {
                return penColor;
            }

            set
            {
                if (penColor != value)
                {
                    penColor = value;
                    OnPropertyChanged("PenColor");
                }
            }
        }

        public System.Windows.Media.Color GradientBeginColor
        {
            get
            {
                return gradientBeginColor;
            }

            set
            {
                if (gradientBeginColor != value)
                {
                    gradientBeginColor = value;
                    OnPropertyChanged("GradientBeginColor");
                    if (edit != null)
                    {
                        edit.GutterGradientStartColor = gradientBeginColor;
                    }
                }
            }
        }

        public System.Windows.Media.Color GradientEndColor
        {
            get
            {
                return gradientEndColor;
            }

            set
            {
                if (gradientEndColor != value)
                {
                    gradientEndColor = value;
                    OnPropertyChanged("GradientEndColor");
                    if (edit != null)
                    {
                        edit.GutterGradientEndColor = gradientEndColor;
                    }
                }
            }
        }

        public System.Windows.Media.Color ForeColor
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
                    OnPropertyChanged("ForeColor");
                    if (edit != null)
                        edit.LineNumbersBrush = new System.Windows.Media.SolidColorBrush(foreColor);
                }
            }
        }

        public System.Windows.Media.Color BackColor
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
                    OnPropertyChanged("BackColor");
                    if (edit != null)
                        edit.LineNumbersBackBrush = new System.Windows.Media.SolidColorBrush(backColor);
                }
            }
        }

        public bool ShowGutter
        {
            get
            {
                return showGutter;
            }

            set
            {
                if (showGutter != value)
                {
                    showGutter = value;
                    OnPropertyChanged("ShowGutter");
                    if (edit != null)
                        edit.GutterVisible = showGutter;
                }
            }
        }

        public bool RenderCustomGutterItems
        {
            get
            {
                return renderCustomGutterItems;
            }

            set
            {
                if (renderCustomGutterItems != value)
                {
                    renderCustomGutterItems = value;
                    OnPropertyChanged(nameof(RenderCustomGutterItems));
                    if (edit != null)
                        edit.CustomGutterItemsVisible = value;
                }
            }
        }

        public bool UseGradient
        {
            get
            {
                return useGradient;
            }

            set
            {
                if (useGradient != value)
                {
                    useGradient = value;
                    OnPropertyChanged("UseGradient");
                    if (edit != null)
                    {
                        edit.GradientGutter = useGradient;
                    }
                }
            }
        }

        public double GutterWidth
        {
            get
            {
                return gutterWidth;
            }

            set
            {
                if (gutterWidth != value)
                {
                    gutterWidth = value;
                    OnPropertyChanged("GutterWidth");
                    if (edit != null)
                        edit.GutterWidth = gutterWidth;
                }
            }
        }

        public double LeftIndent
        {
            get
            {
                return leftIndent;
            }

            set
            {
                if (leftIndent != value)
                {
                    leftIndent = value;
                    OnPropertyChanged("LeftIndent");
                    if (edit != null)
                        edit.LineNumbersLeftMargin = leftIndent;
                }
            }
        }

        public double RightIndent
        {
            get
            {
                return rightIndent;
            }

            set
            {
                if (rightIndent != value)
                {
                    rightIndent = value;
                    OnPropertyChanged("RightIndent");
                    if (edit != null)
                        edit.LineNumbersRightMargin = rightIndent;
                }
            }
        }

        public double NumberStart
        {
            get
            {
                return numberStart;
            }

            set
            {
                if (numberStart != value)
                {
                    numberStart = value;
                    OnPropertyChanged("NumberStart");
                    edit.LineNumbersStart = (int)numberStart;
                }
            }
        }

        public bool LineNumbersVisible
        {
            get
            {
                return lineNumbersVisible;
            }

            set
            {
                if (lineNumbersVisible != value)
                {
                    lineNumbersVisible = value;
                    OnPropertyChanged("LineNumbersVisible");
                    if (edit != null)
                        edit.LineNumbersVisible = lineNumbersVisible;
                }
            }
        }

        public bool LinesOnGutter
        {
            get
            {
                return linesOnGutter;
            }

            set
            {
                if (linesOnGutter != value)
                {
                    linesOnGutter = value;
                    OnPropertyChanged("LinesOnGutter");
                    if (edit != null)
                        edit.LinesOnGutter = LinesOnGutter;
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

        private void SetupCustomGutterItems()
        {
            edit.QueryCustomGutterItem += (o, e) =>
            {
                var editor = (TextEditor)o;
                e.ItemRequired = IsCustomGutterItemRequired(e.LineIndex);
            };
            edit.RenderCustomGutterItem += (o, e) =>
            {
                var width = 5;
                e.DrawingContext.DrawRectangle(
                    Brushes.Green,
                    null,
                    new Rect((e.ItemBounds.Width - width) / 2, (e.ItemBounds.Height - width) / 2, width, width));
            };
            edit.GutterItemMouseDown += (o, e) =>
            {
                if (IsCustomGutterItemRequired(e.LineIndex))
                    MessageBox.Show("Custom gutter item clicked.");
            };
        }

        private bool IsCustomGutterItemRequired(int lineIndex)
        {
            return edit.Strings[lineIndex].TrimStart().StartsWith("//");
        }
    }
}
