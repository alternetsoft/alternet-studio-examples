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
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.TextMate;

namespace TextMateParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private static string increasePattern = "^((?!//).)*(\\{([^}\"'`/]*|(\\t|[ ])*//.*)|\\([^)\"'`/]*|\\[[^\\]\"'`/]*)$";
        private static string decreasePattern = "^((?!.*?\\/\\*).*\\*/)?\\s*[\\)\\}\\]].*$";
        private static string unindentPattern = "^(\\t|[ ])*[ ]\\*[^/]*\\*/\\s*$|^(\\t|[ ])*[ ]\\*/\\s*$|^(\\t|[ ])*[ ]\\*([ ]([^\\*]|\\*(?!/))*)?$";

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string resourcePath;
        private TextEditor edit;
        private string language = string.Empty;
        private string theme = string.Empty;
        private string visualTheme = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private ObservableCollection<string> themeNames = new ObservableCollection<string>();
        private ObservableCollection<string> visualThemes = new ObservableCollection<string>();

        private TextMateParser parser = new TextMateParser();

        private LanguageInfo[] langItems =
        {
            new LanguageInfo(".adoc", "*.adoc", "Ascii doc"),
            new LanguageInfo(".bat", "*.bat", "Bat"),
            new LanguageInfo(".clj", "*clj.", "Clojure"),
            new LanguageInfo(".coffee", "*.coffee", "Coffeescript"),
            new LanguageInfo(".c", "*.c", "C", true),
            new LanguageInfo(".cpp", "*.cpp", "C++", true),
            new LanguageInfo(".cu", "*.cu", "Cuda cpp"),
            new LanguageInfo(".cs", "*.cs", "C#", true),
            new LanguageInfo(".cshtml", "*.cshtml", "Razor"),
            new LanguageInfo(".css", "*.css", "Css"),
            new LanguageInfo(".dart", "*.dart", "Dart"),
            new LanguageInfo(".dockerfile", "*.dockerfile", "Dockerfile"),
            new LanguageInfo(".fs", "*.fs", "Fsharp"),
            new LanguageInfo(".gitignore", "*.gitignore", "Ignore"),
            new LanguageInfo(".go", "*.go", "Go"),
            new LanguageInfo(".groovy", "*.groovy", "Groovy"),
            new LanguageInfo(".handlebars", "*.handlebars", "Handlebars"),
            new LanguageInfo(".hlsl", "*.hlsl", "Hlsl"),
            new LanguageInfo(".html", "*.html", "Html"),
            new LanguageInfo(".ini", "*.ini", "Ini"),
            new LanguageInfo(".java", "*.java", "Java", true),
            new LanguageInfo(".jsx", "*.jsx", "Java Script React"),
            new LanguageInfo(".js", "*.js", "Java Script"),
            new LanguageInfo(".json", "*.json", "Json"),
            new LanguageInfo(".jsonc", "*.jsonc", "Jsonc"),
            new LanguageInfo(".jl", "*.jl", "Julia"),
            new LanguageInfo(".less", "*.less", "Less"),
            new LanguageInfo(".lua", "*.lua", "Lua"),
            new LanguageInfo(".mak", "*.mak", "Makefile"),
            new LanguageInfo(".md", "*.md", "Markdown"),
            new LanguageInfo(".m", "*.m", "Objective-c", true),
            new LanguageInfo(".mm", "*.mm", "Objective-cpp", true),
            new LanguageInfo(".pas", "*.pas", "Pascal"),
            new LanguageInfo(".pl", "*.pl", "Perl"),
            new LanguageInfo(".p6", "*.p6", "Perl6"),
            new LanguageInfo(".php", "*.php", "Php"),
            new LanguageInfo(".properties", "*.properties", "Properties"),
            new LanguageInfo(".ps1", "*.ps1", "Power Shell"),
            new LanguageInfo(".pug", "*.pug", "Jude"),
            new LanguageInfo(".py", "*.py", "Python"),
            new LanguageInfo(".r", "*.r", "R"),
            new LanguageInfo(".rb", "*.rb", "Ruby"),
            new LanguageInfo(".rs", "*.rs", "Rust"),
            new LanguageInfo(".scss", "*.scss", "Scss"),
            new LanguageInfo(".shader", "*.shader", "Shaderlab"),
            new LanguageInfo(".sh", "*.sh", "Shell Script"),
            new LanguageInfo(".sql", "*.sql", "Sql"),
            new LanguageInfo(".swift", "*.swift", "Swift"),
            new LanguageInfo(".tex", "*.tex", "LaTex"),
            new LanguageInfo(".ts", "*.ts", "Type Script", true),
            new LanguageInfo(".vb", "*.vb", "Vb"),
            new LanguageInfo(".xml", "*.xml", "Xml"),
            new LanguageInfo(".xsl", "*.xsl", "Xsl"),
            new LanguageInfo(".yml", "*.yml", "Yaml"),
        };

        #endregion

        public ViewModel()
        {
            foreach (LanguageInfo lang in langItems)
            {
                languages.Add(lang.Description);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\TextMate");
            if (!dirInfo.Exists)
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";

            resourcePath = Path.GetFullPath(dir) + @"Resources\Editor\TextMate";
            dirInfo = new DirectoryInfo(resourcePath);
            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles();
                for (int j = 0; j < files.Length; j++)
                {
                    int idx = FindLangByExt(Path.GetExtension(files[j].Name));
                    if (idx >= 0)
                        langItems[idx].FileName = files[j].FullName;
                }
            }
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            edit.Lexer = parser;
            InitializeThemes();
            Language = "C#";
            Theme = ThemeName.DarkPlus.ToString();
            VisualTheme = "TextMate";
            edit.VisualTheme = new TextMateTheme(parser.LanguageDefinition.ThemeColors);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public ObservableCollection<string> ThemeNames
        {
            get { return themeNames; }
            set { themeNames = value; }
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
                        if (string.Compare(visualTheme, "TextMate") == 0)
                            edit.VisualThemeType = VisualThemeType.Custom;
                        else
                            edit.VisualThemeType = (VisualThemeType)Enum.Parse(typeof(VisualThemeType), visualTheme);
                    }
                }
            }
        }

        public string Theme
        {
            get
            {
                return theme;
            }

            set
            {
                if (theme != value)
                {
                    theme = value;
                    OnPropertyChanged("Theme");
                    if (edit != null)
                    {
                        ThemeName name = (ThemeName)Enum.Parse(typeof(ThemeName), theme);
                        parser.ThemeName = name;
                        if (parser.ThemeName == ThemeName.Custom)
                            parser.LanguageDefinition.LoadThemeFromPath(Path.Combine(resourcePath, "custom_theme.json"));

                        var visualTheme = edit.VisualTheme as TextMateTheme;
                        if (visualTheme != null)
                        {
                            visualTheme.UpdateTheme(parser.LanguageDefinition.ThemeColors);
                            edit.ApplyTheme(visualTheme);
                        }

                        ReparseText();
                    }
                }
            }
        }

        public string Language
        {
            get
            {
                return language;
            }

            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged("Language");
                    int idx = FindLangByDesc(language);
                    if (edit != null)
                    {
                        if (idx >= 0)
                        {
                            string fileName = langItems[idx].FileName;
                            if (fileName != string.Empty)
                            {
                                parser.FileName = fileName;
                                edit.Source.LoadFile(fileName);
                                edit.Source.FileName = fileName;
                                ReparseText();
                            }
                        }

                        var brackets = parser.LanguageDefinition?.Brackets;
                        if (brackets != null)
                        {
                            edit.Source.OpenBraces = brackets.ToOpenBrackets();
                            edit.Source.ClosingBraces = brackets.ToCloseBrackets();
                        }
                        else
                        {
                            edit.Source.OpenBraces = new char[] { };
                            edit.Source.ClosingBraces = new char[] { };
                        }
                    }

                    if (langItems[idx].IndentBraces)
                    {
                        parser.InitIndentationRules(increasePattern, decreasePattern, unindentPattern);
                        parser.SmartFormatChars = new char[] { '}' };
                    }
                    else
                    {
                        parser.SmartFormatChars = new char[] { };
                    }
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Private Methods

        private void ReparseText()
        {
            UpdateParsed(0, int.MaxValue);
            edit.Source.ParseToString(int.MaxValue);
            edit.Invalidate();
        }

        private void UpdateParsed(int fromIndex, int toIndex)
        {
            IStringItem item;

            for (int i = fromIndex; i <= Math.Min(toIndex, edit.Source.Lines.Count - 1); i++)
            {
                item = edit.Source.Lines.GetItem(i);
                item.State = item.State & ~ItemState.Parsed;
            }

            edit.Source.SetLastParsed(0);
        }

        private void InitializeThemes()
        {
            foreach (string themeName in Enum.GetNames(typeof(ThemeName)))
            {
                themeNames.Add(themeName);
            }

            foreach (string item in Enum.GetNames(typeof(VisualThemeType)))
            {
                visualThemes.Add(string.Compare(item, "Custom", true) == 0 ? "TextMate" : item);
            }
        }

        private int FindLangByDesc(string desc)
        {
            for (int i = 0; i < langItems.Length; i++)
            {
                if (string.Compare(langItems[i].Description, desc, true) == 0)
                    return i;
            }

            return -1;
        }

        private int FindLangByExt(string ext)
        {
            for (int i = 0; i < langItems.Length; i++)
            {
                if (string.Compare(langItems[i].FileType, ext, true) == 0)
                    return i;
            }

            return -1;
        }

        #endregion

        private struct LanguageInfo
        {
            public string FileType;
            public string FileExt;
            public string Description;
            public string SchemeName;
            public string FileName;
            public bool IndentBraces;

            public LanguageInfo(string fileType, string fileExt, string description, bool indentBraces = false)
            {
                FileType = fileType;
                FileExt = fileExt;
                Description = description;
                IndentBraces = indentBraces;
                FileName = string.Empty;
                SchemeName = string.Empty;
            }
        }
    }
}
