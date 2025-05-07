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

using Alternet.UI;
using Alternet.Maui;

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Generic;

using NJsonSchema;
using System.Runtime;
using ExCSS;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class SyntaxHighlightingPage : DemoPage
{
    private readonly Parser powerShellParser = new();

    private LanguageInfo[] langItems;

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    static SyntaxHighlightingPage()
    {
    }

    public SyntaxHighlightingPage()
    {
        langItems = new LanguageInfo[]
        {
                new LanguageInfo("csharp.txt", "c#", "*.cs", "C#", ()=> InitParser(new Cs_SchemeParser())),

                new LanguageInfo("assembler.txt", "assembler", "*.assembler", "Assembler",
                ()=> InitParser(new AssemblerParser())),

                new LanguageInfo("batch.txt", "batch", "*.batch", "Command Prompt",
                ()=> InitParser(new BatchParser())),

                new LanguageInfo("cobol.txt", "cobol", "*.CBL", "COBOL",
                ()=> InitParser(new CobolParser())),

                new LanguageInfo("c.txt", "c", "*.c", "ANSI C",
                ()=> InitParser(new C_SchemeParser())),

                new LanguageInfo("cbuilder.txt", "c++builder", "*.cbuilder", "C++ Builder",
                ()=> InitParser(new CBuilderParser())),

                new LanguageInfo("delphi.txt", "delphi", "*.delphi", "Delphi",
                ()=> InitParser(new DelphiParser())),

                new LanguageInfo("dfm.txt", "dfm", "*.dfm", "Dfm",
                ()=> InitParser(new DfmParser())),

                new LanguageInfo("html.txt", "html", "*.html", "Html",
                ()=> InitParser(new Html_SchemeParser())),

                new LanguageInfo(
                    "htmlscripts.txt",
                    "htmlscripts",
                    "*.htmlscripts",
                    "HTML with scripts",
                    ()=> InitParser(new HtmlScriptsParser())),

                new LanguageInfo("il.txt", "il", "*.il", "MSIL",
                ()=> InitParser(new IlParser())),

                new LanguageInfo("ini.txt", "ini", "*.ini", "Ini files",
                ()=> InitParser(new IniParser())),

                new LanguageInfo("java.txt", "java", "*.js", "Java",
                ()=> InitParser(new Js_SchemeParser())),

                new LanguageInfo("java_script.txt", "javascript", "*.jscript", "Java Script",
                ()=> InitParser(new JScriptParser())),

                new LanguageInfo("perl.txt", "perl", "*.perl", "Perl",
                ()=> InitParser(new PerlParser())),

                new LanguageInfo("php.txt", "php", "*.php", "PHP",
                ()=> InitParser(new PHPParser())),

                new LanguageInfo("python.txt", "python", "*.python", "Python",
                ()=> InitParser(new Python_SchemeParser())),

                new LanguageInfo("powershell.txt", "powershell", "*.ps1", "PowerShell",
                ()=> InitParser(powerShellParser)),

                new LanguageInfo("sql_oracle.txt", "SQL_Oracle", "*.sql", "SQL",
                ()=> InitParser(new Sql_SchemeParser())),

                new LanguageInfo("tcltk.txt", "tcltk", "*.tcltk", "TclTk",
                ()=> InitParser(new TclTkParser())),

                new LanguageInfo("unix_shell.txt", "unix_shell", "*.unixshell", "Unix Shell",
                ()=> InitParser(new UnixShellParser())),

                new LanguageInfo("vb_net.txt", "vb_net", "*.vbnet", "Visual Basic NET",
                ()=> InitParser(new VbNet_SchemeParser())),

                new LanguageInfo("vbs_script.txt", "vbs_script", "*.vbscript", "VB Script",
                ()=> InitParser(new VbScript_SchemeParser())),

                new LanguageInfo(
                    "vbs_scripts.txt",
                    "vbs_scripts",
                    "*.vbscripts",
                    "VB Script in HTML",
                    ()=> InitParser(new VbScripts_SchemeParser())),

                new LanguageInfo("xml.txt", "xml", "*.xml", "XML",
                ()=> InitParser(new Xml_SchemeParser())),

                new LanguageInfo(
                    "xml_scripts.txt",
                    "xml_scripts",
                    "*.xmlscripts",
                    "XML with scripts",
                    ()=> InitParser(new XmlScripts_SchemeParser())),

                //customScheme,
        };

        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        LanguagesListView.ItemsSource = Languages;

        InitEdit();

        syntaxEdit1.Outlining.AllowOutlining = true;

        LoadScheme(powerShellParser, NewFileNameNoExt + "Schemas.powershell.xml");

        LanguagesListView.SelectedItem = langItems[0];

        if (!Alternet.UI.App.IsWindowsOS)
        {
            // Currently loadButton only supports loading on Windows
            // For other platforms, see 
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=macios
            loadButton.IsVisible = false;
        }

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);

    }

    public List<LanguageInfo> Languages
    {
        get
        {
            return langItems.ToList();
        }
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Syntax Highlighting";

    public ILexer InitParser(ILexer lexer)
    {
        lexer.Scheme.MakeForeColorLighterLighterIfDark(syntaxEdit1.Editor);
        return lexer;
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
    }

    public void LoadScheme(ILexer parser, string url)
    {
        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);
        if (stream != null)
        {
            parser?.Scheme.LoadStream(stream);
            if(parser is not null)
                InitParser(parser);
        }
    }

    public static void LoadFile(Alternet.Editor.TextSource.ITextSource source, string url)
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

    private static readonly string[] xmlExtensions = [".xml"];

    private async void LoadButton_Clicked(object? sender, EventArgs e)
    {
        var customFileTypeXml = new FilePickerFileType(
new Dictionary<DevicePlatform, IEnumerable<string>>
{
                    { DevicePlatform.WinUI, xmlExtensions },
});

        PickOptions options = new()
        {
        };

        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        LoadScheme(syntaxEdit1.Lexer, files.FullPath);

    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
    }

    private void LanguagesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        UpdateLanguage(LanguagesListView.SelectedItem as LanguageInfo);
    }

    private void UpdateLanguage(LanguageInfo? info)
    {
        syntaxEdit1.Lexer = info?.Lexer!;
        var fileName = info?.FileName;
        LoadFile(syntaxEdit1.Source, NewFileNameNoExt + "SyntaxHighlighting." + fileName);
    }


    public class LanguageInfo
    {
        private Func<Alternet.Syntax.Lexer.ILexer>? CreateFunc;
        private Alternet.Syntax.Lexer.ILexer? lexer;

        public string FileType { get; set; }
        public string FileExt;
        public string Description { get; set; }
        public string FileName;

        public LanguageInfo(string fileName, string fileType, string fileExt, string description, Func<Alternet.Syntax.Lexer.ILexer>? createFunc)
        {
            this.FileName = fileName;
            this.FileType = fileType;
            this.FileExt = fileExt;
            this.Description = description;
            CreateFunc = createFunc;
        }
        public ILexer? Lexer
        {
            get
            {
                lexer ??= CreateFunc?.Invoke();
                return lexer;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}