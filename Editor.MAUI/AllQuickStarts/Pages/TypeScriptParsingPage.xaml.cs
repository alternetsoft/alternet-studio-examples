#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using Alternet.Common.TypeScript.Types;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Maui;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.TypeScript;
using Microsoft.Maui.Layouts;
using System.ComponentModel;

namespace AllQuickStarts;
public enum TypeScriptLexToken
{
    TypeName = 12,
    Warning,
    XmlParams,
    MethodName,
}


public partial class TypeScriptParsingPage : DemoPage
{
    private TypeScriptParserWithSemantic? typeScriptParser = new TypeScriptParserWithSemantic();
    private JavaScriptParserWithSemantic? javaScriptParser = new JavaScriptParserWithSemantic();

    internal string NewFileNameNoExt = "embres:AllQuickStarts.Content.newfile";

    static TypeScriptParsingPage()
    {
    }

    public TypeScriptParsingPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        LanguagesPicker.SelectedIndex = 0;
        chbSemanticHighlighting.CheckedChanged += SemanticHighlightingCheckBox_CheckedChanged;

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

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "TypeScript";

    private static readonly string[] jsExtensions = [".js"];
    private static readonly string[] tsExtensions = [".ts"];

    private bool IsJavaScriptSelected => syntaxEdit1.Lexer == javaScriptParser;

    private void SemanticHighlightingCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if(typeScriptParser is not null)
            typeScriptParser.SemanticHighlighting = chbSemanticHighlighting.IsChecked;

        if (javaScriptParser is not null)
            javaScriptParser.SemanticHighlighting = chbSemanticHighlighting.IsChecked;

        var parser = syntaxEdit1.Lexer as TypeScriptParser;
        if (parser != null)
            parser.ReparseText();
    }


    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var selIndex = LanguagesPicker.SelectedIndex;
        string fileName = string.Empty;
        switch (selIndex)
        {
            default:
            case 0:
                syntaxEdit1.Source.Lexer = typeScriptParser;
                fileName = NewFileNameNoExt + ".ts";
                break;
            case 1:
                syntaxEdit1.Source.Lexer = javaScriptParser;
                fileName = NewFileNameNoExt + ".js";
                break;
        }

        if (fileName != string.Empty)
        {
            LoadFile(syntaxEdit1.Source, fileName);
            syntaxEdit1.Source.FileName = fileName;
        }
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

    public static void LoadFile(ITextSource source, string url)
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
        var customFileTypeTs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, tsExtensions },
                });

        var customFileTypeJs = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, jsExtensions },
                });

        PickOptions options = new()
        {
            PickerTitle = IsJavaScriptSelected
                ? "Please select a Ts file" : "Please select a Js file",
            FileTypes = IsJavaScriptSelected ? customFileTypeJs : customFileTypeTs,
        };

        var files = await FilePicker.Default.PickAsync(options);

        if (files == null)
            return;

        ILexer? lexer = IsJavaScriptSelected ? javaScriptParser : typeScriptParser;
        syntaxEdit1.Lexer = lexer!;

        LoadFile(syntaxEdit1.Source, files.FullPath);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref typeScriptParser);
        SafeDispose(ref javaScriptParser);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
public class TypeScriptParserWithSemantic : TypeScriptParser
{
    private bool semanticHighlighting;

    [DefaultValue(false)]
    public bool SemanticHighlighting
    {
        get
        {
            return semanticHighlighting;
        }

        set
        {
            if (semanticHighlighting != value)
            {
                semanticHighlighting = value;
                OnSemanticHighlightingPropertyChanged();
            }
        }
    }

    protected virtual void OnSemanticHighlightingPropertyChanged()
    {
        var tokenizer = Tokenizer as TypeScriptTokenizerWithSemantic;
        if (tokenizer != null)
            tokenizer.SemanticHighlighting = semanticHighlighting;
    }

    protected override TypeScriptTokenizer CreateTokenizer()
    {
        return new TypeScriptTokenizerWithSemantic();
    }

    protected override void InitDefaultStyles()
    {
        base.InitDefaultStyles();
        AddStyle(TypeScriptConsts.MethodInternalName, TypeScriptConsts.DefaultMethodForeColor);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
public class JavaScriptParserWithSemantic : JavaScriptParser
{
    private bool semanticHighlighting;

    [DefaultValue(false)]
    public bool SemanticHighlighting
    {
        get
        {
            return semanticHighlighting;
        }

        set
        {
            if (semanticHighlighting != value)
            {
                semanticHighlighting = value;
                OnSemanticHighlightingChanged();
            }
        }
    }

    protected virtual void OnSemanticHighlightingChanged()
    {
        var tokenizer = Tokenizer as TypeScriptTokenizerWithSemantic;
        if (tokenizer != null)
            tokenizer.SemanticHighlighting = semanticHighlighting;
    }

    protected override TypeScriptTokenizer CreateTokenizer()
    {
        return new TypeScriptTokenizerWithSemantic();
    }

    protected override void InitDefaultStyles()
    {
        base.InitDefaultStyles();
        AddStyle(TypeScriptConsts.MethodInternalName, TypeScriptConsts.DefaultMethodForeColor);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
public class TypeScriptTokenizerWithSemantic : TypeScriptTokenizer
{
    public bool SemanticHighlighting { get; set; }

    protected override int GetSyntaxToken(ClassifiedSpan span)
    {
        if (SemanticHighlighting)
        {
            switch (span.ClassificationType.Value)
            {
                case "identifier":
                    var symbol = Repository.Parser.GetQuickInfoAtPosition(Repository.FileName, span.Span.Start);
                    if (symbol != null)
                    {
                        switch (symbol.Kind.Value)
                        {
                            case "function":
                            case "method":
                                return (int)TypeScriptLexToken.MethodName;
                            case "class":
                                return (int)TypeScriptLexToken.TypeName;
                        }
                    }

                    return (int)LexToken.Identifier;
            }
        }

        return base.GetSyntaxToken(span);
    }
}