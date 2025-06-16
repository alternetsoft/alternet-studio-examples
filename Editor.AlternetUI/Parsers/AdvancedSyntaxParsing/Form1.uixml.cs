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
using Alternet.Editor.Common.AlternetUI;
using NJsonSchema;

namespace AdvancedSyntaxParsing
{
    public partial class Form1 : Window
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

            cbLanguages.Items.AddRange(new object[] {
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
            "XML"});

            cbLanguages.SelectedIndexChanged += LanguagesComboBox_SelectedIndexChanged;
            btLoad.Click += LoadButton_Click;

            syntaxEdit1.Outlining.AllowOutlining = true;

            Form1_Load(this, EventArgs.Empty);

            cbLanguages.SelectedIndex = 0;

            Idle += Form1_Idle;
            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        protected override void DisposeManaged()
        {
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

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private ILexer GetLexer(int index)
        {
            LanguageInfo info = (index >= 0) && (index < langItems.Length)
                ? langItems[index] : new LanguageInfo(string.Empty, string.Empty, string.Empty);
            ILexer result;
            string schemaFileName;

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
                        DemoUtils.ResourcesFolder + @"Editor/QuickStarts/Parsers/SQL/databaseObjects.xml");
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

            if (result is ISyntaxParser sp)
            {
                sp.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                    | SyntaxOptions.SyntaxErrors;
            }

            return result;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb|JS files (*.java)|*.js|JScript.NET files (*.jscript.NET)|*.jscript.NET|VB Script files (*.vbs)|*.vbs|Java Script files (*.js)|*.js|Ansi-C files (*.h;*.c)|*.h;*.c|SQL files (*.sql)|*.sql|HTML files (*.htm;*.html)|*.htm;*.html|XML files (*.xml)|*.xml|All files (*.*)|*.*";

            var dirInfo = new DirectoryInfo(DemoUtils.ResourcesFolder + @"Editor/Text");
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
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbLanguages.SelectedIndex >= 0)
            {
                syntaxEdit1.Source.Lexer = GetLexer(cbLanguages.SelectedIndexAsInt);
                string fileName = langItems[cbLanguages.SelectedIndexAsInt].FileName;
                if (fileName != string.Empty)
                {
                    syntaxEdit1.Source.LoadFile(fileName);
                    syntaxEdit1.Source.FileName = fileName;
                }
            }
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = cbLanguages.SelectedIndexAsInt;

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
