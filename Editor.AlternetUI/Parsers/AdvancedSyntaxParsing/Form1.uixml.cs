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
using System.IO;

using Alternet.UI;

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using NJsonSchema;

namespace AdvancedSyntaxParsing
{
    public partial class Form1 : Window
    {
        internal bool UseNullTemporaryParser = false;

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

        private readonly OpenFileDialog openFileDialog1 = new();

        private static LanguageInfo infoCSharp
            = new(typeCSharp, "*.cs", "C#");

        private static LanguageInfo infoVBNet
            = new LanguageInfo(typeVBNet, "*.vb", "Visual Basic NET");

        private static LanguageInfo infoJava
            = new LanguageInfo(typeJava, "*.java", "J#");

        private static LanguageInfo infoJScriptNet
            = new LanguageInfo(typeJScriptNet, "*.jscript.NET", "JScript.NET");
        
        private static LanguageInfo infoVB
            = new LanguageInfo(typeVB, "*.vbs",  "VB Script");

        private static LanguageInfo infoJScript
            = new LanguageInfo(typeJScript, "*.js", "Java Script");

        private static LanguageInfo infoJson
            = new LanguageInfo(typeJson, "*.json", "JSON");

        private static LanguageInfo infoC
            = new LanguageInfo(typeC, "*.h;*.c", "ANSI C");

        private static LanguageInfo infoSql
            = new LanguageInfo(typeSql, "*.sql", "SQL");

        private static LanguageInfo infoHtml
            = new LanguageInfo(typeHtml, "*.htm;*.html", "HTML");

        private static LanguageInfo infoCss
            = new LanguageInfo(typeCss, "*.css", "CSS files");

        private static LanguageInfo infoXml
            = new LanguageInfo(typeXml, "*.xml", "XML");

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
            new LanguageInfo("all", "*.*", "All files"),
        };

        public Form1()
        {
            InitializeComponent();

            DemoUtils.InitCommonEditorProps(syntaxEdit1);

            if (CommandLineArgs.ParseAndGetIsDark())
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;

            cbLanguages.AddRange(new object[] {
                "C#",
                "Visual Basic",
                "Java#",
                "JScript NET",
                "VB Script",
                "JavaScript",
                "JSON",
                "Ansi-C",
                "SQL",
                "HTML",
                "Css",
                "XML",
                "None",
            });

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;
            btLoad.Click += LoadButton_Click;

            syntaxEdit1.Outlining.AllowOutlining = true;

            var dialogFilter = FileMaskUtils.ToFileDialogFilter(
                new("C# Files", "cs"),
                new("VB Files", "vb"),
                new("Java Files", "java"),
                new("JScript.NET Files", "jscript"),
                new("VB Script Files", "vbs"),
                new("Java Script Files", "js"),
                new("Ansi-C Files", ["h","c"]),
                new("SQL Files", "sql"),
                new("HTML Files", ["htm", "html"]),
                new("XML Files", "xml"),
                new(FileDialogFilterItem.Kind.AllFiles)
            );

            openFileDialog1.Filter = dialogFilter;

            var dirInfo = new DirectoryInfo(DemoUtils.GetResourceFolderFullPath(@"Editor/Text"));
            openFileDialog1.InitialDirectory = dirInfo.FullName;
            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles();
                for (int j = 0; j < files.Length; j++)
                {
                    int idx = FindLangByName(RemoveFileExt(files[j].Name));
                    if (idx >= 0)
                        langItems[idx].FileName = files[j].FullName;
                }
            }

            LogUtils.RegisterLogAction("Check Demo Files", CheckFiles);

            cbLanguages.Value = cbLanguages.Items[9].Value;

            lbDescription.WordWrap = true;

            ActiveControl = syntaxEdit1;

            cbTheme.ExcludeValues = new VisualThemeType[] {
                VisualThemeType.Custom,
            };

            cbTheme.EnumType = typeof(VisualThemeType);

            cbTheme.Value = syntaxEdit1.VisualThemeType;
            cbTheme.ValueChanged += (s, e) =>
            {
                syntaxEdit1.VisualThemeType = (VisualThemeType)cbTheme.Value;
            };
        }

        Action<ILexer?>? UpdateLexerAction { get; set; }

        protected override void DisposeManaged()
        {
            UpdateLexerAction = null;
            base.DisposeManaged();
        }

        private void CheckFiles()
        {
            if (!App.IsWindowsOS)
                return;
            foreach (LanguageInfo info in langItems)
                CheckFile(info);

            void CheckFile(LanguageInfo info)
            {
                if (string.IsNullOrEmpty(info.FileName))
                    return;
                if (FileUtils.RealFileHasSameCase(info.FileName))
                    return;

                App.Log($"Bad case: {info.FileName}");
            }
        }

        private void SetLexer(int index, Action<ILexer?>? setAction)
        {
            LanguageInfo info = (index >= 0) && (index < langItems.Length)
                ? langItems[index] : new LanguageInfo(string.Empty, string.Empty, string.Empty);
            ILexer? result;
            string schemaFileName;
            bool codeCompletion = true;

            switch (info.FileType)
            {
                default:
                case typeCSharp:
                    if (UseNullTemporaryParser)
                        result = null;
                    else
                        result = LexerDemoUtils.CreateSyntaxParserAdvancedCs(false);
                    codeCompletion = false;

                    var slowLexer = LexerDemoUtils.CreateSyntaxParserAdvancedCs(true, (lexer) =>
                    {
                        codeCompletion = true;
                        SendResult(lexer);
                    });
                    break;
                case typeVBNet:
                    if (UseNullTemporaryParser)
                        result = null;
                    else
                        result = LexerDemoUtils.CreateSyntaxParserAdvancedVb(false);
                    codeCompletion = false;

                    var slowLexerVB = LexerDemoUtils.CreateSyntaxParserAdvancedVb(
                        true,
                        (lexer) =>
                        {
                            codeCompletion = true;
                            SendResult(lexer);
                            // cbLanguages.Enabled = true;
                        });
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
                    schemaFileName = PathUtils.GetAppSubFolder("Schemas")+ "/JsonSchema.schema.json";
                    if (File.Exists(schemaFileName))
                    {
                        ((JSONParserWithSchema)result).Schema
                            = JsonSchema.FromFileAsync(schemaFileName).Result;
                    }

                    break;
                case typeC:
                    result = new CParser();
                    break;
                case typeSql:
                    result = new SqlParser();
                    FileInfo fileInfo = new(
                        DemoUtils.GetResourceFileFullPath(@"Editor/QuickStarts/Parsers/SQL", "databaseObjects.xml"));
                    if (fileInfo.Exists)
                    {
                        ((SqlRepositoryBase)((SqlParser)result)
                            .CompletionRepository)?.LoadDataFromXml(fileInfo.FullName);
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
                    result = new XmlParserWithSchema();
                    break;
            }

            void SendResult(ILexer? lexer)
            {
                if (DisposingOrDisposed)
                    return;

                if (lexer is ISyntaxParser sp && codeCompletion)
                {
                    sp.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                        | SyntaxOptions.SyntaxErrors;
                }

                setAction?.Invoke(lexer);
            }

            SendResult(result);
        }

        private int FindLangByName(string name)
        {
            for (int i = 0; i < langItems.Length; i++)
            {
                if (string.Compare(langItems[i].FileType, name, true) == 0)
                    return i;
            }

            return -1;
        }

        private static string RemoveFileExt(string fileName)
        {
            int p = fileName.LastIndexOf(".");
            return (p >= 0) ? fileName.Substring(0, p) : fileName;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = cbLanguages.IndexOfValue;
            if (index is null)
                return;

            syntaxEdit1.Source.Lexer = null;

            if(cbLanguages.Value?.ToString() == "None")
            {
                return;
            }

            UpdateLexerAction = (lexer) =>
            {
                if (syntaxEdit1.DisposingOrDisposed)
                    return;
                syntaxEdit1.Source.Lexer = lexer;
            };

            SetLexer(index.Value, UpdateLexerAction);

            string fileName = langItems[index.Value].FileName;
            if (fileName != string.Empty)
            {
                syntaxEdit1.Source.LoadFile(fileName);
                syntaxEdit1.Source.FileName = fileName;
            }
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = 10;

            openFileDialog1.ShowAsync(() =>
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            });
        }

        private struct LanguageInfo
        {
            public string FileType;
            public string FileExt;
            public string Description;
            public string SchemeName;
            public string FileName;

            public LanguageInfo(string fileType, string fileExt, string description)
            {
                this.FileType = fileType;
                this.FileExt = fileExt;
                this.Description = description;
                FileName = string.Empty;
                SchemeName = string.Empty;
            }
        }
    }
}
