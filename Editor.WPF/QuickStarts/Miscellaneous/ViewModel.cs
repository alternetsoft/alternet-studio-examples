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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Roslyn;

using WeCantSpell.Hunspell;

namespace Miscellaneous
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private SpellChecker spellChecker = new SpellChecker();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;

        private bool transparent = false;
        private bool checkSpelling = false;
        private bool highlightBraces = false;
        private bool highlightBounds = false;
        private bool tempHighlight = false;
        private bool roundRect = false;
        private bool whitespaceVisible = false;
        private bool separateLines = false;

        private System.Windows.Media.Color bracesColor;
        private System.Windows.Media.Color spellColor;
        private System.Windows.Media.Color symbolColor;
        private System.Windows.Media.Color backgroundColor;
        private System.Windows.Media.Color gradientBeginColor;
        private System.Windows.Media.Color gradientEndColor;
        private BitmapImage backgroundImage;

        private ObservableCollection<string> fontStyles = new ObservableCollection<string>();
        private string fontStyle;

        private ObservableCollection<string> backgroundStyles = new ObservableCollection<string>();
        private string backgroundStyle;

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\spell.txt");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;
            csharpSource.CheckSpelling = true;
            string[] s = Enum.GetNames(typeof(System.Drawing.FontStyle));
            foreach (string str in s)
                FontStyles.Add(str);
            backgroundStyles.Add("Image Background");
            backgroundStyles.Add("Gradient");
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            if (edit != null)
            {
                this.edit = edit;
                edit.Source = csharpSource;
                edit.Braces.FontStyle = System.Drawing.FontStyle.Bold;

                // background
                Transparent = true;
                BackgroundStyle = "Image Background";
                BackgroundColor = (edit.Background is SolidColorBrush) ? ((SolidColorBrush)edit.Background).Color : System.Windows.Media.Colors.White;
                GradientBeginColor = System.Windows.Media.Colors.SeaGreen;
                GradientEndColor = System.Windows.Media.Colors.DarkGray;

                // braces
                edit.Braces.BracesOptions = BracesOptions.Highlight;
                HighlightBraces = (BracesOptions.Highlight & edit.Braces.BracesOptions) != 0;
                RoundRect = edit.Braces.UseRoundRect;
                TempHighlight = (BracesOptions.TempHighlight & edit.Braces.BracesOptions) != 0;
                HighlightBounds = (BracesOptions.HighlightBounds & edit.Braces.BracesOptions) != 0;
                BracesColor = System.Windows.Media.Color.FromRgb(edit.Braces.ForeColor.R, edit.Braces.ForeColor.G, edit.Braces.ForeColor.B);
                FontStyle = edit.Braces.FontStyle.ToString();

                // spelling
                CheckSpelling = edit.Spelling.CheckSpelling;
                SpellColor = System.Windows.Media.Color.FromRgb(edit.Spelling.SpellColor.R, edit.Spelling.SpellColor.G, edit.Spelling.SpellColor.B);
                spellChecker.CheckSpelling(edit, CheckSpelling);

                // whitespace
                WhitespaceVisible = edit.WhitespaceVisible;
                SymbolColor = System.Windows.Media.Color.FromRgb(edit.Whitespace.SymbolColor.R, edit.Whitespace.SymbolColor.G, edit.Whitespace.SymbolColor.B);
                SeparateLines = (SeparatorOptions.SeparateLines & edit.LineSeparator.Options) != 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> FontStyles
        {
            get { return fontStyles; }
            set { fontStyles = value; }
        }

        public ObservableCollection<string> BackgroundStyles
        {
            get { return backgroundStyles; }
            set { backgroundStyles = value; }
        }

        public string FontStyle
        {
            get
            {
                return fontStyle;
            }

            set
            {
                if (fontStyle != value)
                {
                    fontStyle = value;
                    OnPropertyChanged("FontStyle");
                    if (edit != null)
                    {
                        switch (fontStyle)
                        {
                            case "Bold":
                                edit.Braces.FontStyle = System.Drawing.FontStyle.Bold;
                                break;
                            case "Italic":
                                edit.Braces.FontStyle = System.Drawing.FontStyle.Italic;
                                break;
                            case "Underline":
                                edit.Braces.FontStyle = System.Drawing.FontStyle.Underline;
                                break;
                            case "Regular":
                                edit.Braces.FontStyle = System.Drawing.FontStyle.Regular;
                                break;
                            default:
                                edit.Braces.FontStyle = System.Drawing.FontStyle.Regular;
                                break;
                        }
                    }
                }
            }
        }

        public string BackgroundStyle
        {
            get
            {
                return backgroundStyle;
            }

            set
            {
                if (backgroundStyle != value)
                {
                    backgroundStyle = value;
                    OnPropertyChanged("BackgroundStyle");
                    UpdateEditor();
                }
            }
        }

        public BitmapImage BackgroundImage
        {
            get
            {
                if (backgroundImage != null)
                    return backgroundImage;

                byte[] bytes = Properties.Resources.draft;
                MemoryStream stream = new MemoryStream(bytes);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                return backgroundImage = image;
            }
        }

        public bool Transparent
        {
            get
            {
                return transparent;
            }

            set
            {
                if (transparent != value)
                {
                    transparent = value;
                    OnPropertyChanged("Transparent");
                    UpdateEditor();
                }
            }
        }

        public bool CheckSpelling
        {
            get
            {
                return checkSpelling;
            }

            set
            {
                if (checkSpelling != value)
                {
                    checkSpelling = value;
                    OnPropertyChanged("CheckSpelling");
                    if (edit != null)
                        spellChecker.CheckSpelling(edit, checkSpelling);
                    if (edit != null)
                        spellChecker.CheckSpelling(edit, checkSpelling);
                    edit.Spelling.CheckSpelling = checkSpelling;
                }
            }
        }

        public bool HighlightBraces
        {
            get
            {
                return highlightBraces;
            }

            set
            {
                if (highlightBraces != value)
                {
                    highlightBraces = value;
                    OnPropertyChanged("HighlightBraces");
                    if (edit != null)
                    {
                        edit.Braces.BracesOptions = highlightBraces ? edit.Braces.BracesOptions
                            | BracesOptions.Highlight : edit.Braces.BracesOptions & ~BracesOptions.Highlight;
                    }
                }
            }
        }

        public bool HighlightBounds
        {
            get
            {
                return highlightBounds;
            }

            set
            {
                if (highlightBounds != value)
                {
                    highlightBounds = value;
                    OnPropertyChanged("HighlightBounds");
                    if (edit != null)
                    {
                        edit.Braces.BracesOptions = highlightBounds ? edit.Braces.BracesOptions
                            | BracesOptions.HighlightBounds : edit.Braces.BracesOptions & ~BracesOptions.HighlightBounds;
                    }
                }
            }
        }

        public bool TempHighlight
        {
            get
            {
                return tempHighlight;
            }

            set
            {
                if (tempHighlight != value)
                {
                    tempHighlight = value;
                    OnPropertyChanged("TempHighlight");
                    if (edit != null)
                    {
                        edit.Braces.BracesOptions = tempHighlight ? edit.Braces.BracesOptions
                            | BracesOptions.TempHighlight : edit.Braces.BracesOptions & ~BracesOptions.TempHighlight;
                    }
                }
            }
        }

        public bool RoundRect
        {
            get
            {
                return roundRect;
            }

            set
            {
                if (roundRect != value)
                {
                    roundRect = value;
                    OnPropertyChanged("RoundRect");
                    if (edit != null)
                    {
                        edit.Braces.UseRoundRect = roundRect;
                        edit.Braces.ForeColor = roundRect ? System.Drawing.Color.Gray : System.Drawing.Color.Black;
                    }
                }
            }
        }

        public bool WhitespaceVisible
        {
            get
            {
                return whitespaceVisible;
            }

            set
            {
                if (whitespaceVisible != value)
                {
                    whitespaceVisible = value;
                    OnPropertyChanged("WhitespaceVisible");
                    if (edit != null)
                    {
                        edit.WhitespaceVisible = whitespaceVisible;
                    }
                }
            }
        }

        public bool SeparateLines
        {
            get
            {
                return separateLines;
            }

            set
            {
                if (separateLines != value)
                {
                    separateLines = value;
                    OnPropertyChanged("SeparateLines");
                    if (edit != null)
                    {
                        edit.LineSeparator.Options = separateLines ? edit.LineSeparator.Options
                            | SeparatorOptions.SeparateLines : edit.LineSeparator.Options & ~SeparatorOptions.SeparateLines;
                    }
                }
            }
        }

        public System.Windows.Media.Color BracesColor
        {
            get
            {
                return bracesColor;
            }

            set
            {
                if (bracesColor != value)
                {
                    bracesColor = value;
                    OnPropertyChanged("BracesColor");
                    if (edit != null)
                        edit.Braces.ForeColor = System.Drawing.Color.FromArgb(bracesColor.R, bracesColor.G, bracesColor.B);
                }
            }
        }

        public System.Windows.Media.Color SymbolColor
        {
            get
            {
                return symbolColor;
            }

            set
            {
                if (symbolColor != value)
                {
                    symbolColor = value;
                    OnPropertyChanged("SymbolColor");
                    if (edit != null)
                        edit.Whitespace.SymbolColor = System.Drawing.Color.FromArgb(symbolColor.R, symbolColor.G, symbolColor.B);
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
                    UpdateEditor();
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
                    UpdateEditor();
                }
            }
        }

        public System.Windows.Media.Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    OnPropertyChanged("BackgroundColor");
                    UpdateEditor();
                }
            }
        }

        public System.Windows.Media.Color SpellColor
        {
            get
            {
                return spellColor;
            }

            set
            {
                if (spellColor != value)
                {
                    spellColor = value;
                    OnPropertyChanged("SpellColor");
                    if (edit != null)
                    {
                        edit.Spelling.SpellColor = System.Drawing.Color.FromArgb(spellColor.R, spellColor.G, spellColor.B);
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

        private void UpdateEditor()
        {
            if (edit != null)
            {
                if (!Transparent)
                    edit.Background = new SolidColorBrush(BackgroundColor);
                else
                {
                    switch (backgroundStyle)
                    {
                        case "Image Background":
                            edit.Background = new ImageBrush(BackgroundImage);
                            break;
                        case "Gradient":
                            edit.Background = new LinearGradientBrush(gradientBeginColor, gradientEndColor, 0);
                            break;
                    }
                }
            }
        }

        public class SpellChecker
        {
            private WordList wordList = null;

            public SpellChecker()
            {
                try
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    if (!File.Exists(Path.GetFullPath(Path.Combine(dir, "en_us.aff"))))
                        dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\";
                    wordList = WordList.CreateFromFiles(Path.GetFullPath(Path.Combine(dir, "en_us.dic")), Path.GetFullPath(Path.Combine(dir, "en_us.aff")));
                }
                catch
                {
                }
            }

            public void CheckSpelling(TextEditor edit, bool spell)
            {
                edit.Spelling.CheckSpelling = spell;
                if (spell)
                    edit.Spelling.WordSpell += new WordSpellEvent(WordSpell);
                else
                    edit.Spelling.WordSpell -= new WordSpellEvent(WordSpell);
            }

            private void WordSpell(object sender, WordSpellEventArgs e)
            {
                ITextSource source = (ITextSource)sender;
                bool correct = (wordList != null) ? wordList.Check(e.Text) : true;
                if (source.Lexer != null)
                {
                    LexToken tok = (LexToken)(e.ColorStyle.Data - 1);
                    if ((tok == LexToken.String) || (tok == LexToken.Comment) || (tok == LexToken.XmlComment))
                        e.Correct = (e.Text.Length <= 1) || correct;
                }
                else
                    e.Correct = (e.Text.Length <= 1) || correct;
            }
        }
    }
}
