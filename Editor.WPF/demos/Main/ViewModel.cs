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

using Alternet.Syntax;
using Alternet.Syntax.Parsers.TextMate;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private string language = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        private TextMateParser parser = new TextMateParser();

        private LanguageInfo[] langItems =
        {
            new LanguageInfo(".adoc", "*.adoc", "Ascii doc"),
            new LanguageInfo(".bat", "*.bat", "Bat"),
            new LanguageInfo(".clj", "*clj.", "Clojure"),
            new LanguageInfo(".coffee", "*.coffee", "Coffeescript"),
            new LanguageInfo(".c", "*.c", "C"),
            new LanguageInfo(".cpp", "*.cpp", "C++"),
            new LanguageInfo(".cu", "*.cu", "Cuda cpp"),
            new LanguageInfo(".cs", "*.cs", "C#"),
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
            new LanguageInfo(".java", "*.java", "Java"),
            new LanguageInfo(".jsx", "*.jsx", "Java Script React"),
            new LanguageInfo(".js", "*.js", "Java Script"),
            new LanguageInfo(".json", "*.json", "Json"),
            new LanguageInfo(".jsonc", "*.jsonc", "Jsonc"),
            new LanguageInfo(".jl", "*.jl", "Julia"),
            new LanguageInfo(".less", "*.less", "Less"),
            new LanguageInfo(".lua", "*.lua", "Lua"),
            new LanguageInfo(".mak", "*.mak", "Makefile"),
            new LanguageInfo(".md", "*.md", "Markdown"),
            new LanguageInfo(".m", "*.m", "Objective-c"),
            new LanguageInfo(".mm", "*.mm", "Objective-cpp"),
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
            new LanguageInfo(".ts", "*.ts", "Type Script"),
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
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";

            dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\TextMate");
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
            parser.ThemeName = ThemeName.Light;
            Language = "C#";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
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
                    RestoreEditor(false);
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        public void RestoreEditor(bool updateParser = true)
        {
            if (updateParser)
                edit.Lexer = parser;
            if (edit != null)
            {
                int idx = FindLangByDesc(language);
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
        }

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

            public LanguageInfo(string fileType, string fileExt, string description)
            {
                FileType = fileType;
                FileExt = fileExt;
                Description = description;
                FileName = string.Empty;
                SchemeName = string.Empty;
            }
        }
    }
}
