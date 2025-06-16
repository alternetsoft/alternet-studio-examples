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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using Microsoft.Win32;
using NJsonSchema;

namespace AdvancedSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private ComboBox comboBox;
        private string language = string.Empty;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private int updateCount = 0;

        private LanguageInfo[] langItems =
        {
            new LanguageInfo("c#", "*.cs", "C#"),
            new LanguageInfo("vb_net", "*.vb", "Visual Basic"),
            new LanguageInfo("java", "*.java", "Java#"),

            new LanguageInfo("JScript.NET", "*.jscript.NET", "JScript.NET"),
            new LanguageInfo("vbs_script", "*.vbs",  "VB Script"),
            new LanguageInfo("java_script", "*.js", "JavaScript"),
            new LanguageInfo("json", "*.json", "JSON"),

            new LanguageInfo("c", "*.h;*.c", "ANSI-C"),
            new LanguageInfo("sql_oracle", "*.sql", "SQL"),

            new LanguageInfo("html", "*.htm;*.html", "HTML"),
            new LanguageInfo("CSS", "*.css", "Css"),
            new LanguageInfo("xml", "*.xml", "XML"),

            new LanguageInfo("all", "*.*", "All files"),
        };

        #endregion

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
                dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            }

            openFileDialog.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb|JS files (*.java)|*.js|JScript.NET files (*.jscript.NET)|*.jscript.NET|VB Script files (*.vbs)|*.vbs|Java Script files (*.js)|*.js|Ansi-C files (*.h;*.c)|*.h;*.c|SQL files (*.sql)|*.sql|HTML files (*.htm;*.html)|*.htm;*.html|XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"text";
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

            languages.Add("C#");
            languages.Add("Visual Basic");
            languages.Add("Java#");
            languages.Add("JScript.NET");
            languages.Add("VB Script");
            languages.Add("JavaScript");
            languages.Add("JSON");
            languages.Add("Ansi-C");
            languages.Add("SQL");
            languages.Add("HTML");
            languages.Add("Css");
            languages.Add("XML");
            LoadCommand = new RelayCommand(LoadClick);
        }

        public ViewModel(TextEditor edit, ComboBox comboBox)
            : this()
        {
            this.edit = edit;
            this.comboBox = comboBox;
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
                    if (updateCount > 0)
                        return;

                    if (edit != null)
                    {
                        int idx = FindLangByDesc(language);
                        if (idx >= 0)
                        {
                            edit.Source.Lexer = GetLexer(idx);
                            string fileName = langItems[idx].FileName;
                            if (fileName != string.Empty)
                            {
                                edit.Source.LoadFile(fileName);
                                edit.Source.FileName = fileName;
                            }
                        }
                    }
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Private Methods

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
                case "json":
                    result = new JSONParserWithSchema();
                    schemaFileName = @"..\..\..\Schemas\JsonSchema.schema.json";
                    if (File.Exists(schemaFileName))
                        ((JSONParserWithSchema)result).Schema = JsonSchema.FromFileAsync(schemaFileName).Result;
                    break;
                case "vbs_script":
                    result = new VbScriptParser();
                    break;
                case "java_script":
                    result = new JavaScriptParser();
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
                if (string.Compare(langItems[i].FileExt, ext, true) == 0)
                    return i;
            }

            return -1;
        }

        private string RemoveFileExt(string fileName)
        {
            int p = fileName.LastIndexOf(".");
            return (p >= 0) ? fileName.Substring(0, p) : fileName;
        }

        private void LoadClick()
        {
            int idx = FindLangByDesc(language);
            openFileDialog.FilterIndex = idx + 1;
            if (openFileDialog.ShowDialog().Value)
            {
                idx = FindLangByExt("*" + Path.GetExtension(openFileDialog.FileName));
                if (idx >= 0)
                {
                    edit.Lexer = GetLexer(idx);
                    updateCount++;
                    try
                    {
                        comboBox.SelectedIndex = idx;
                    }
                    finally
                    {
                        updateCount--;
                    }
                }

                edit.Source.LoadFile(openFileDialog.FileName);
                edit.Source.FileName = openFileDialog.FileName;
            }
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
