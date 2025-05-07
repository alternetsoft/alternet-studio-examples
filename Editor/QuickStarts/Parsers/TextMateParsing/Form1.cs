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
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.TextMate;

namespace TextMateParsing
{
    public partial class Form1 : Form
    {
        private static string increasePattern = "^((?!//).)*(\\{([^}\"'`/]*|(\\t|[ ])*//.*)|\\([^)\"'`/]*|\\[[^\\]\"'`/]*)$";
        private static string decreasePattern = "^((?!.*?\\/\\*).*\\*/)?\\s*[\\)\\}\\]].*$";
        private static string unindentPattern = "^(\\t|[ ])*[ ]\\*[^/]*\\*/\\s*$|^(\\t|[ ])*[ ]\\*/\\s*$|^(\\t|[ ])*[ ]\\*([ ]([^\\*]|\\*(?!/))*)?$";

        private string dir = Application.StartupPath + @"\";
        private string resourcePath;
        private TextMateParser parser = new TextMateParser();
        private IList<string> themeList = new List<string>();

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

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "TextMateParsing.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            syntaxEdit1 = new SyntaxEdit();
            syntaxEdit1.Parent = MainPanel;
            syntaxEdit1.Dock = DockStyle.Fill;
            syntaxEdit1.Outlining.AllowOutlining = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syntaxEdit1.Lexer = parser;
            foreach (LanguageInfo lang in langItems)
            {
                LanguagesCombobox.Items.Add(lang.Description);
            }

            ThemeNamesCombobox.DataSource = Enum.GetValues(typeof(ThemeName));
            ThemeNamesCombobox.SelectedIndex = (int)ThemeName.DarkPlus;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\TextMate");
            if (!dirInfo.Exists)
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";

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
            int index = FindLangByExt(".cs");
            LanguagesCombobox.SelectedIndex = index >= 0 ? index : 0;
            syntaxEdit1.VisualTheme = new TextMateTheme(parser.LanguageDefinition.ThemeColors);
            InitializeVisualThemeComboBox();
        }

        private void LanguagesCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LanguagesCombobox.SelectedIndex >= 0)
            {
                string fileName = langItems[LanguagesCombobox.SelectedIndex].FileName;
                if (fileName != string.Empty)
                {
                    parser.FileName = fileName;
                    syntaxEdit1.Source.LoadFile(fileName);
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
            }

            if (langItems[LanguagesCombobox.SelectedIndex].IndentBraces)
            {
                parser.InitIndentationRules(increasePattern, decreasePattern, unindentPattern);
                parser.SmartFormatChars = new char[] { '}' };
            }
            else
            {
                parser.SmartFormatChars = new char[] { };
            }
        }

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

        private void InitializeVisualThemeComboBox()
        {
            foreach (string item in Enum.GetNames(typeof(VisualThemeType)))
            {
                themeList.Add(string.Compare(item, "Custom", true) == 0 ? "TextMate" : item);
            }

            visualThemeComboBox.DataSource = themeList;
            visualThemeComboBox.SelectedIndex = (int)VisualThemeType.Custom;
        }

        private void ThemesCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            parser.ThemeName = (ThemeName)ThemeNamesCombobox.SelectedIndex;
            if (parser.ThemeName == ThemeName.Custom)
                parser.LanguageDefinition.LoadThemeFromPath(Path.Combine(resourcePath, "custom_theme.json"));

            var theme = syntaxEdit1.VisualTheme as TextMateTheme;
            if (theme != null)
            {
                theme.UpdateTheme(parser.LanguageDefinition.ThemeColors);
                syntaxEdit1.ApplyTheme(theme);
            }

            ReparseText();
        }

        private void VisualThemesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit1.VisualThemeType = (VisualThemeType)visualThemeComboBox.SelectedIndex;
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
                this.FileType = fileType;
                this.FileExt = fileExt;
                this.Description = description;
                this.IndentBraces = indentBraces;
                FileName = string.Empty;
                SchemeName = string.Empty;
            }
        }
    }
}
