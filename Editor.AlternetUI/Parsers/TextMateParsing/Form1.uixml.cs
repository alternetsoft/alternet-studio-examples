#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Alternet.UI;

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.TextMate;
using Alternet.Common;

namespace TextMateParsing
{
    public partial class Form1 : Window
    {
        private static string increasePattern
            = "^((?!//).)*(\\{([^}\"'`/]*|(\\t|[ ])*//.*)|\\([^)\"'`/]*|\\[[^\\]\"'`/]*)$";
        private static string decreasePattern = "^((?!.*?\\/\\*).*\\*/)?\\s*[\\)\\}\\]].*$";
        private static string unindentPattern
            = "^(\\t|[ ])*[ ]\\*[^/]*\\*/\\s*$|^(\\t|[ ])*[ ]\\*/\\s*$|^(\\t|[ ])*[ ]\\*([ ]([^\\*]|\\*(?!/))*)?$";

        private readonly TextMateParser parser = new TextMateParser();
        private readonly ObservableCollection<string> languages = new();
        private readonly ObservableCollection<string> themeNames = new();
        private readonly ObservableCollection<string> visualThemes = new();

        private static LanguageInfo pythonLanguageInfo = new LanguageInfo(".py", "*.py", "Python");

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
            pythonLanguageInfo,
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

        public Form1()
        {
            InitializeComponent();

            if (!IsNet471OrGreater)
            {
                syntaxEdit1.Lines.Add("This demo requires .Net 4.71 or greater");
                return;
            }

            bool isDark = IsDarkBackground;

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                isDark = true;
            }

            SuspendLayout();
            try
            {
                foreach (LanguageInfo lang in langItems)
                {
                    languages.Add(lang.Description);
                }

                cbLanguages.Items.AddRange(languages.ToArray());

                cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;

                foreach (var themeName in Enum.GetValues(typeof(ThemeName)))
                {
                    var s = themeName.ToString();
                    if (s is not null)
                        themeNames.Add(s);
                }

                cbColors.Items.AddRange(themeNames);

                DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/TextMate");

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

                syntaxEdit1.Outlining.AllowOutlining = true;
                syntaxEdit1.Lexer = parser;
                InitializeThemes();
                int index = FindLangByExt(".py");
                cbLanguages.SelectedIndex = index >= 0 ? index : 0;
                syntaxEdit1.VisualTheme = new TextMateTheme(parser.LanguageDefinition.ThemeColors);

                cbColors.SelectedIndexChanged += ThemesComboBox_SelectedIndexChanged;

                cbVisualThemes.IsEnabled = false;

                if (isDark)
                {
                    syntaxEdit1.VisualThemeType = VisualThemeType.Custom;
                    cbVisualThemes.SelectedIndex = (int)VisualThemeType.Custom;
                    cbColors.SelectedIndex = (int)ThemeName.DarkPlus;
                }
                else
                {
                    syntaxEdit1.VisualThemeType = VisualThemeType.Custom;
                    cbVisualThemes.SelectedIndex = (int)VisualThemeType.Custom;
                    cbColors.SelectedIndex = (int)ThemeName.LightPlus;
                }

                Idle += Form1_Idle;
                Form1_Idle(this, EventArgs.Empty);
                cbVisualThemes.SelectedIndexChanged += VisualThemesComboBox_SelectedIndexChanged;

                App.AddIdleTask(() =>
                {
                    syntaxEdit1.SetFocusIfPossible();
                });
            }
            finally
            {
                ResumeLayout();
            }

            SetSizeToContent();
        }

        public bool IsNet471OrGreater
        {
            get
            {
#if NET471_OR_GREATER
                return true;
#else
                return false;
#endif
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void SetCurrentItem(LanguageInfo info)
        {
            string fileName = info.FileName;
            if (fileName != string.Empty)
            {
                parser.FileName = fileName;
                if (!syntaxEdit1.Source.LoadFile(fileName))
                {
                    syntaxEdit1.Text = $"Error loading file: {fileName}";
                }
                syntaxEdit1.Source.FileName = fileName;
                ReparseText();
            }

            var brackets = parser.LanguageDefinition?.Brackets;
            if (brackets != null)
            {
                syntaxEdit1.Source.OpenBraces = brackets.ToOpenBrackets();
                syntaxEdit1.Source.ClosingBraces = brackets.ToCloseBrackets();
            }
            else
            {
                syntaxEdit1.Source.OpenBraces = new char[] { };
                syntaxEdit1.Source.ClosingBraces = new char[] { };
            }

            if (info.IndentBraces)
            {
                parser.InitIndentationRules(increasePattern, decreasePattern, unindentPattern);
                parser.SmartFormatChars = new char[] { '}' };
            }
            else
            {
                parser.SmartFormatChars = new char[] { };
            }
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbLanguages.SelectedIndex >= 0)
            {
                SetCurrentItem(langItems[cbLanguages.SelectedIndexAsInt]);
            }
        }

        #region Private Methods

        private void ReparseText()
        {
            UpdateParsed(0, int.MaxValue);
            syntaxEdit1.Source.ParseToString(int.MaxValue);
            syntaxEdit1.Invalidate();
        }

        private void UpdateParsed(int fromIndex, int toIndex)
        {
            IStringItem item;

            for (int i = fromIndex; i <= Math.Min(toIndex, syntaxEdit1.Source.Lines.Count - 1); i++)
            {
                item = syntaxEdit1.Source.Lines.GetItem(i);
                item.State = item.State & ~ItemState.Parsed;
            }

            syntaxEdit1.Source.SetLastParsed(0);
        }

        private void InitializeThemes()
        {
            foreach (string item in Enum.GetNames(typeof(VisualThemeType)))
            {
                visualThemes.Add(string.Compare(item, "Custom", true) == 0 ? "TextMate" : item);
            }

            cbVisualThemes.Items.AddRange(visualThemes);
        }

        private void ThemesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            parser.ThemeName = (ThemeName?)cbColors.SelectedIndex ?? ThemeName.Custom;
            if (parser.ThemeName == ThemeName.Custom)
            {
                parser.LanguageDefinition.LoadThemeFromPath(
                    Path.Combine(DemoUtils.ResourcesFolder, @"Editor/TextMate/custom_theme.json"));
            }

            var theme = syntaxEdit1.VisualTheme as TextMateTheme;
            if (theme != null)
            {
                theme.UpdateTheme(parser.LanguageDefinition.ThemeColors);
                syntaxEdit1.ApplyTheme(theme);
            }

            ReparseText();
        }

        private void VisualThemesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbVisualThemes.SelectedIndex is null)
                return;
            syntaxEdit1.VisualThemeType = (VisualThemeType)cbVisualThemes.SelectedIndex;
        }

        internal int FindLangByDesc(string desc)
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

            public LanguageInfo(
                string fileType,
                string fileExt,
                string description,
                bool indentBraces = false)
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
