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
using Alternet.Syntax.Parsers.Advanced;

using NJsonSchema;
using System.Xml.Schema;
using Alternet.Editor.Common.AlternetUI;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts;

public partial class AdvancedSyntaxParsingPage : DemoPage
{
    private const string typeCSharp = "c#";
    private const string typeVBNet = "vb_net";
    private const string typeJava = "java";
    private const string typeJScriptNet = "JScript.NET";
    private const string typeVB = "vbs_script";
    private const string typeJScript = "java_script";
    private const string typeJson = "json";
    private const string typeC = "c";
    private const string typeSql = "sql_oracle";
    private const string typeHtml = "html";
    private const string typeCss = "CSS";
    private const string typeXml = "xml";

    private static LanguageInfo infoCSharp
        = new LanguageInfo("csharp.txt", typeCSharp, "*.cs", "C#");

    private static LanguageInfo infoVBNet
        = new LanguageInfo("vb_net.txt", typeVBNet, "*.vb", "Visual Basic NET");

    private static LanguageInfo infoJava
        = new LanguageInfo("java.txt", typeJava, "*.java", "J#");

    private static LanguageInfo infoJScriptNet
        = new LanguageInfo("JScript.NET.txt", typeJScriptNet, "*.jscript.NET", "JScript.NET");

    private static LanguageInfo infoVB
        = new LanguageInfo("vbs_script.txt", typeVB, "*.vbs", "VB Script");

    private static LanguageInfo infoJScript
        = new LanguageInfo("java_script.txt", typeJScript, "*.js", "Java Script");

    private static LanguageInfo infoJson
        = new LanguageInfo("json.txt", typeJson, "*.json", "JSON");

    private static LanguageInfo infoC
        = new LanguageInfo("c.txt", typeC, "*.h;*.c", "ANSI C");

    private static LanguageInfo infoSql
        = new LanguageInfo("SQL_Oracle.txt", typeSql, "*.sql", "SQL");

    private static LanguageInfo infoHtml
        = new LanguageInfo("html.txt", typeHtml, "*.htm;*.html", "HTML");

    private static LanguageInfo infoCss
        = new LanguageInfo("Css.txt", typeCss, "*.css", "CSS files");

    private static LanguageInfo infoXml
        = new LanguageInfo("xml.txt", typeXml, "*.xml", "XML");

    private LanguageInfo[] langItems =
    {
            infoCSharp,
            infoVBNet,
            infoJava,
            infoJScriptNet,
            infoVB,
            infoJScript,
            infoJson,
            infoC,
            infoSql,
            infoHtml,
            infoCss,
            infoXml,
        };

    private static readonly string[] vbExtensions = [".vb"];
    private static readonly string[] csExtensions = [".cs"];

    private static readonly string[] javaSharpExtensions = [".java"];
    private static readonly string[] jScriptNetExtensions = [".jscript.NET"];
    private static readonly string[] vbScriptExtensions = [".vbs"];
    private static readonly string[] javaScriptExtensions = [".js"];
    private static readonly string[] jsonExtensions = [".json"];
    private static readonly string[] cExtensions = [".c", ".h"];
    private static readonly string[] sqlExtensions = [".sql"];
    private static readonly string[] htmlExtensions = [".htm", ".html"];
    private static readonly string[] cssExtensions = [".css"];
    private static readonly string[] xmlExtensions = [".xml"];

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.";

    static AdvancedSyntaxParsingPage()
    {
    }

    public AdvancedSyntaxParsingPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        syntaxEdit1.Outlining.AllowOutlining = true;

        LanguagesPicker.SelectedIndex = 0;

        if (!Alternet.UI.App.IsWindowsOS)
        {
            // Currently loadButton only supports loading on Windows
            // For other platforms, see 
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-8.0&tabs=macios
            loadButton.IsVisible = false;
        }

        if(HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);

    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Advanced Syntax";

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var selIndex = LanguagesPicker.SelectedIndex;

        if (selIndex >= 0)
        {
            syntaxEdit1.Source.Lexer = GetLexer(selIndex);

            string fileName = langItems[LanguagesPicker.SelectedIndex].FileName;
            if (fileName != string.Empty)
            {
                LoadFile(syntaxEdit1.Source, NewFileNameNoExt + fileName);
                syntaxEdit1.Source.FileName = fileName;
            }
        }
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options
            = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
    }

    private ILexer GetLexer(int index)
    {
        LanguageInfo info = (index >= 0) && (index < langItems.Length)
            ? langItems[index]
            : new LanguageInfo(string.Empty, string.Empty, string.Empty, string.Empty);
        ILexer result;

        switch (info.FileType)
        {
            default:
            case typeCSharp:
                result = LexerDemoUtils.CreateSyntaxParserAdvancedCs();
                break;
            case typeVBNet:
                result = LexerDemoUtils.CreateSyntaxParserAdvancedVb();
                break;
            case typeJava:
                result = new JsParser();
                break;
            case typeJScriptNet:
                result = new JScriptNETParser();
                break;
            case typeVB:
                result = new VbScriptParser();
                break;
            case typeJScript:
                result = new JavaScriptParser();
                break;
            case typeJson:
                result = new JSONParserWithSchema();
                var str = Alternet.UI.ResourceLoader.StringFromUrlOrNull(
                    "embres:AllQuickStarts.Schemas.JsonSchema.schema.json");
                if(str is not null)
                {
                    ((JSONParserWithSchema)result).Schema
                        = JsonSchema.FromJsonAsync(str).Result;
                }

                break;
            case typeC:
                result = new CParser();
                break;
            case typeSql:
                result = new SqlParser();
                var dbObjects = Alternet.UI.ResourceLoader.StringFromUrlOrNull(
                    "embres:AllQuickStarts.Schemas.databaseObjects.xml");
                if (dbObjects is not null)
                {
                    ((SqlRepositoryBase)((SqlParser)result)
                        .CompletionRepository)?.LoadDataFromXmlString(dbObjects);
                    ((SqlParser)result).FormatCase = FormatCase.Upper;
                }

                break;
            case typeHtml:
                result = new HtmlScriptParser();
                break;
            case typeCss:
                result = new CssParser();
                break;
            case typeXml:
                var xmlParserWithSchema = new XmlParserWithSchema();
                result = xmlParserWithSchema;
                xmlParserWithSchema.LoadSchema += (s, e) =>
                {
                    if (s is not XmlParserWithSchema xmlParser)
                        return;

                    var schemaName = e.SchemaName;
                    var schemaUrl = e.SchemaURL;

                    if (schemaUrl != "XmlSchema.xsd")
                    {
                        return;
                    }

                    var schemaStream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(
                        "embres:AllQuickStarts.Schemas.XmlSchema.xsd");
                    if(schemaStream is not null)
                    {
                        using var reader = new StreamReader(schemaStream);
                        var schema = XmlSchema.Read(reader, (s, e) =>
                        {
                            // process schema read errors here.
                        });
                        
                        xmlParser.Schema = schema;
                        e.Handled = true;
                    }
                };

                break;
        }

        if (result is ISyntaxParser sp)
        {
            sp.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                | SyntaxOptions.SyntaxErrors;
        }

        return result;
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

    private async void LoadButton_Clicked(object? sender, EventArgs e)
    {
        var customFileTypeCs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, csExtensions },
                });

        var customFileTypeVb = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, vbExtensions },
                });

        var customFileTypeJavaSharp = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, javaSharpExtensions },
                });

        var customFileTypeJScriptNet = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, jScriptNetExtensions },
                });

        var customFileTypeVbScript = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, vbScriptExtensions },
                });

        var customFileTypeJavaScript = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, javaScriptExtensions },
                });

        var customFileTypeJson = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, jsonExtensions },
                });

        var customFileTypeC = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, cExtensions },
                });

        var customFileTypeSql = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, sqlExtensions },
                });

        var customFileTypeHtml = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, htmlExtensions },
                });

        var customFileTypeCss = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
                    { DevicePlatform.WinUI, cssExtensions },
        });

        var customFileTypeXml = new FilePickerFileType(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
                    { DevicePlatform.WinUI, xmlExtensions },
        });

        PickOptions options = new()
        {
        };

        options.FileTypes = LanguagesPicker.SelectedIndex switch
        {
            0 => customFileTypeCs,
            1 => customFileTypeVb,
            2 => customFileTypeJavaSharp,
            3 => customFileTypeJScriptNet,
            4 => customFileTypeVbScript,
            5 => customFileTypeJavaScript,
            6 => customFileTypeJson,
            7 => customFileTypeC,
            8 => customFileTypeSql,
            9 => customFileTypeHtml,
            10 => customFileTypeCss,
            11 => customFileTypeXml,
            _ => customFileTypeCs,
        }; 
        
        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        LoadFile(syntaxEdit1.Source, files.FullPath);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
    }

    private struct LanguageInfo
    {
        public string FileType;
        public string FileExt;
        public string Description;
        public string SchemeName;
        public string FileName;

        public LanguageInfo(string fileName, string fileType, string fileExt, string description)
        {
            this.FileName = fileName;
            this.FileType = fileType;
            this.FileExt = fileExt;
            this.Description = description;
            SchemeName = string.Empty;
        }
    }
}