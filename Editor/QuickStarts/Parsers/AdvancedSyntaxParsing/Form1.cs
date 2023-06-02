#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;

using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using NJsonSchema;

namespace AdvancedSyntaxParsing
{
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string LoadDesc = "Load code file";

        private string dir = Application.StartupPath + @"\";

        private LanguageInfo[] langItems =
        {
            new LanguageInfo("c#", "*.cs", "C#"),
            new LanguageInfo("vb_net", "*.vb", "Visual Basic NET"),
            new LanguageInfo("java", "*.java", "J#"),

            new LanguageInfo("JScript.NET", "*.jscript.NET", "JScript.NET"),
            new LanguageInfo("vbs_script", "*.vbs",  "VB Script"),
            new LanguageInfo("java_script", "*.js", "Java Script"),
            new LanguageInfo("json", "*.json", "JSON"),

            new LanguageInfo("c", "*.h;*.c", "ANSI C"),
            new LanguageInfo("sql_oracle", "*.sql", "SQL"),

            new LanguageInfo("html", "*.htm;*.html", "HTML"),
            new LanguageInfo("CSS", "*.css", "CSS files"),
            new LanguageInfo("xml", "*.xml", "XML"),

            new LanguageInfo("all", "*.*", "All files"),
        };

        public Form1()
        {
            InitializeComponent();
        }

        private ILexer GetLexer(int index)
        {
            LanguageInfo info = (index >= 0) && (index < langItems.Length) ? langItems[index] : new LanguageInfo(string.Empty, string.Empty, string.Empty);
            ILexer result = null;
            string schemaFileName;

            switch (info.FileType)
            {
                case "c#":
                    result = new CsParser();
                    break;
                case "vb_net":
                    result = new VbParser();
                    break;
                case "java":
                    result = new JsParser();
                    break;
                case "JScript.NET":
                    result = new JScriptNETParser();
                    break;
                case "vbs_script":
                    result = new VbScriptParser();
                    break;
                case "java_script":
                    result = new JavaScriptParser();
                    break;
                case "json":
                    result = new JSONParserWithSchema();
                    schemaFileName = @"..\..\..\Schemas\JsonSchema.schema.json";
                    if (File.Exists(schemaFileName))
                        ((JSONParserWithSchema)result).Schema = JsonSchema.FromFileAsync(schemaFileName).Result;
                    break;
                case "c":
                    result = new CParser();
                    break;
                case "sql_oracle":
                    result = new SqlParser();
                    FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\QuickStarts\Parsers\SQL\databaseObjects.xml");
                    if (fileInfo.Exists)
                    {
                        ((SqlRepositoryBase)((SqlParser)result).CompletionRepository)?.LoadDataFromXml(fileInfo.FullName);
                        ((SqlParser)result).FormatCase = FormatCase.Upper;
                    }

                    break;
                case "html":
                    result = new HtmlScriptParser();
                    break;
                case "CSS":
                    result = new CssParser();
                    break;
                case "xml":
                    result = new XmlParserWithSchema();
                    break;

                default:
                    result = new CsParser();
                    break;
            }

            if (result is ISyntaxParser)
            {
                ISyntaxParser sp = (ISyntaxParser)result;
                sp.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips;
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

        private string RemoveFileExt(string fileName)
        {
            int p = fileName.LastIndexOf(".");
            return (p >= 0) ? fileName.Substring(0, p) : fileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb|JS files (*.java)|*.js|JScript.NET files (*.jscript.NET)|*.jscript.NET|VB Script files (*.vbs)|*.vbs|Java Script files (*.js)|*.js|Ansi-C files (*.h;*.c)|*.h;*.c|SQL files (*.sql)|*.sql|HTML files (*.htm;*.html)|*.htm;*.html|XML files (*.xml)|*.xml|All files (*.*)|*.*";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
                if (!Directory.Exists(dir))
                    dir = Application.StartupPath + @"\..\..\..\..\..\..\";
            }

            dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\text";
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

            cbLanguages.SelectedIndex = 0;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLanguages.SelectedIndex >= 0)
            {
                syntaxEdit1.Source.Lexer = GetLexer(cbLanguages.SelectedIndex);
                string fileName = langItems[cbLanguages.SelectedIndex].FileName;
                if (fileName != string.Empty)
                {
                    syntaxEdit1.Source.LoadFile(fileName);
                    syntaxEdit1.Source.FileName = fileName;
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = Math.Max(1, cbLanguages.SelectedIndex + 1);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
                syntaxEdit1.Source.FileName = openFileDialog1.FileName;
            }
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
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
