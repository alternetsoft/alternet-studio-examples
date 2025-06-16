#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using AllQuickStarts.Pages;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Parsers.TextMate;

using Alternet.UI;
using Alternet.Maui;
using Alternet.Syntax;
using Elfie.Serialization;
using ExCSS;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class TextMateParsingPage : DemoPage
{
    private static string increasePattern = "^((?!//).)*(\\{([^}\"'`/]*|(\\t|[ ])*//.*)|\\([^)\"'`/]*|\\[[^\\]\"'`/]*)$";
    private static string decreasePattern = "^((?!.*?\\/\\*).*\\*/)?\\s*[\\)\\}\\]].*$";
    private static string unindentPattern = "^(\\t|[ ])*[ ]\\*[^/]*\\*/\\s*$|^(\\t|[ ])*[ ]\\*/\\s*$|^(\\t|[ ])*[ ]\\*([ ]([^\\*]|\\*(?!/))*)?$";

    internal string SampleFileNameNoExt = "embres:AllQuickStarts.Content.TextMate.";

    private TextMateParser? parser = new TextMateParser();
    private IList<string> langList = new List<string>();
    private IList<string> themeList = new List<string>();

    private LanguageInfo[] langItems =
    {
            new LanguageInfo(".adoc", "*.adoc", "sample.adoc", "Ascii doc"),
            new LanguageInfo(".bat", "*.bat", "sample.bat", "Bat"),
            new LanguageInfo(".clj", "*clj.", "sample.clj", "Clojure"),
            new LanguageInfo(".coffee", "*.coffee", "sample.coffee", "Coffeescript"),
            new LanguageInfo(".c", "*.c", "sample.c", "C", true),
            new LanguageInfo(".cpp", "*.cpp", "sample.cpp", "C++", true),
            new LanguageInfo(".cu", "*.cu", "sample.cu", "Cuda cpp"),
            new LanguageInfo(".cs", "*.cs", "sample.cs", "C#", true),
            new LanguageInfo(".cshtml", "*.cshtml", "sample.cshtml", "Razor"),
            new LanguageInfo(".css", "*.css", "sample.css", "Css"),
            new LanguageInfo(".dart", "*.dart", "sample.dart", "Dart"),
            new LanguageInfo(".dockerfile", "*.dockerfile", "sample.dockerfile", "Dockerfile"),
            new LanguageInfo(".fs", "*.fs", "sample.fs", "Fsharp"),
            new LanguageInfo(".gitignore", "*.gitignore", "sample.gitignore", "Ignore"),
            new LanguageInfo(".go", "*.go", "sample.go", "Go"),
            new LanguageInfo(".groovy", "*.groovy", "sample.groovy", "Groovy"),
            new LanguageInfo(".handlebars", "*.handlebars", "sample.handlebars", "Handlebars"),
            new LanguageInfo(".hlsl", "*.hlsl", "sample.hlsl", "Hlsl"),
            new LanguageInfo(".html", "*.html", "sample.html", "Html"),
            new LanguageInfo(".ini", "*.ini", "sample.ini", "Ini"),
            new LanguageInfo(".java", "*.java", "sample.java", "Java", true),
            new LanguageInfo(".jsx", "*.jsx", "sample.jsx", "Java Script React"),
            new LanguageInfo(".js", "*.js", "sample.js", "Java Script"),
            new LanguageInfo(".json", "*.json", "sample.json", "Json"),
            new LanguageInfo(".jsonc", "*.jsonc", "sample.jsonc", "Jsonc"),
            new LanguageInfo(".jl", "*.jl", "sample.jl", "Julia"),
            new LanguageInfo(".less", "*.less", "sample.less", "Less"),
            new LanguageInfo(".lua", "*.lua", "sample.lua", "Lua"),
            new LanguageInfo(".mak", "*.mak", "sample.mak", "Makefile"),
            new LanguageInfo(".md", "*.md", "sample.md", "Markdown"),
            new LanguageInfo(".m", "*.m", "sample.m", "Objective-c", true),
            new LanguageInfo(".mm", "*.mm", "sample.mm", "Objective-cpp", true),
            new LanguageInfo(".pas", "*.pas", "sample.pas", "Pascal"),
            new LanguageInfo(".pl", "*.pl", "sample.pl", "Perl"),
            new LanguageInfo(".p6", "*.p6", "sample.p6", "Perl6"),
            new LanguageInfo(".php", "*.php", "sample.php", "Php"),
            new LanguageInfo(".properties", "*.properties", "sample.properties", "Properties"),
            new LanguageInfo(".ps1", "*.ps1", "sample.ps1", "Power Shell"),
            new LanguageInfo(".pug", "*.pug", "sample.pug", "Jude"),
            new LanguageInfo(".py", "*.py", "sample.py", "Python"),
            new LanguageInfo(".r", "*.r", "sample.r", "R"),
            new LanguageInfo(".rb", "*.rb", "sample.rb", "Ruby"),
            new LanguageInfo(".rs", "*.rs", "sample.rs", "Rust"),
            new LanguageInfo(".scss", "*.scss", "sample.scss", "Scss"),
            new LanguageInfo(".shader", "*.shader", "sample.shader", "Shaderlab"),
            new LanguageInfo(".sh", "*.sh", "sample.sh", "Shell Script"),
            new LanguageInfo(".sql", "*.sql", "sample.sql", "Sql"),
            new LanguageInfo(".swift", "*.swift", "sample.swift", "Swift"),
            new LanguageInfo(".tex", "*.tex", "sample.tex", "LaTex"),
            new LanguageInfo(".ts", "*.ts", "sample.ts", "Type Script", true),
            new LanguageInfo(".vb", "*.vb", "sample.vb", "Vb"),
            new LanguageInfo(".xml", "*.xml", "sample.xml", "Xml"),
            new LanguageInfo(".xsl", "*.xsl", "sample.xsl", "Xsl"),
            new LanguageInfo(".yml", "*.yml", "sample.yml", "Yaml"),
        };

    static TextMateParsingPage()
    {
    }

    public TextMateParsingPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        syntaxEdit1.Lexer = parser;
        InitializeLanguagePicker();
        ThemeNamesPicker.ItemsSource = Enum.GetValues<ThemeName>();
        ThemeNamesPicker.SelectedIndex = (int)ThemeName.DarkPlus;

        InitializeVisualThemePicker();
        int index = FindLangByExt(".cs");
        LanguagesPicker.SelectedIndex = index >= 0 ? index : 0;
        syntaxEdit1.VisualTheme = new TextMateTheme(parser.LanguageDefinition.ThemeColors);

        LanguagesPicker.SelectedIndex = 0;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "TextMate";

    private int FindLangByExt(string ext)
    {
        for (int i = 0; i < langItems.Length; i++)
        {
            if (string.Compare(langItems[i].FileType, ext, true) == 0)
                return i;
        }

        return -1;
    }

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (parser is null)
            return;

        if (LanguagesPicker.SelectedIndex >= 0)
        {
            string fileName = langItems[LanguagesPicker.SelectedIndex].FileName;
            if (fileName != string.Empty)
            {
                parser.FileName = fileName;
                LoadFile(syntaxEdit1.Source, SampleFileNameNoExt + fileName);
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

        if (langItems[LanguagesPicker.SelectedIndex].IndentBraces)
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
            item.State &= ~ItemState.Parsed;
        }

        syntaxEdit1.Source.SetLastParsed(0);
    }

    private void InitializeLanguagePicker()
    {
        foreach (LanguageInfo lang in langItems)
        {
            langList.Add(lang.Description);
        }

        LanguagesPicker.ItemsSource = langList.ToArray();
        LanguagesPicker.SelectedIndex = 7;
    }

    private void InitializeVisualThemePicker()
    {
        foreach (string item in Enum.GetNames(typeof(VisualThemeType)))
        {
            themeList.Add(string.Compare(item, "Custom", true) == 0 ? "TextMate" : item);
        }

        VisualThemePicker.ItemsSource = themeList.ToArray();
        VisualThemePicker.SelectedIndex = (int)VisualThemeType.Custom;
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
        syntaxEdit1.WordWrap = true;
    }

    public static void LoadFile(Alternet.Editor.TextSource.ITextSource? source, string url)
    {
        if (source is null)
            return;

        source.Text = string.Empty;
        source.BookMarks.Clear();
        source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !source.LoadStream(stream))
        {
            source.Text = $"Error loading text: {url}";
            return;
        }
    }

    private void KeyboardButton_Clicked(object? sender, EventArgs e)
    {
        syntaxEdit1.ToggleKeyboard();
    }

    private void ThemeNamesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (parser is null)
            return;
        parser.ThemeName = (ThemeName)ThemeNamesPicker.SelectedIndex;
        if (parser.ThemeName == ThemeName.Custom)
        {
            var url = SampleFileNameNoExt + "custom_theme.json";
            var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

            if (!(stream is null))
            {
                parser.LanguageDefinition.LoadThemeFromStream(stream);
            }
        }

        var theme = syntaxEdit1.VisualTheme as TextMateTheme;
        if (theme != null)
        {
            theme.UpdateTheme(parser.LanguageDefinition.ThemeColors);
            syntaxEdit1.Editor.ApplyTheme(theme);
        }

        ReparseText();
    }

    private void VisualThemePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.VisualThemeType = (VisualThemeType)VisualThemePicker.SelectedIndex;
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref parser);
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
        public LanguageInfo(string fileType, string fileExt, string fileName, string description, bool indentBraces = false)
        {
            this.FileType = fileType;
            this.FileExt = fileExt;
            this.Description = description;
            this.IndentBraces = indentBraces;
            FileName = fileName;
            SchemeName = string.Empty;
        }
    }
}