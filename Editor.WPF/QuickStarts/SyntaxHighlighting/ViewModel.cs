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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Generic;
using Microsoft.Win32;

namespace SyntaxHighlighting
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private string language = string.Empty;
        private bool customEnabled = false;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        private LanguageInfo[] langItems =
        {
            new LanguageInfo("Assembler", "*.assembler", "Assembler"),
            new LanguageInfo("Batch", "*.batch", "Command Prompt"),
            new LanguageInfo("Cobol", "*.CBL", "COBOL"),
            new LanguageInfo("C", "*.c", "ANSI C"),
            new LanguageInfo("C#", "*.cs", "C#"),
            new LanguageInfo("C++builder", "*.cbuilder", "C++ Builder"),
            new LanguageInfo("Delphi", "*.delphi", "Delphi"),
            new LanguageInfo("Dfm", "*.dfm", "Dfm"),
            new LanguageInfo("Html", "*.html", "Html"),
            new LanguageInfo("HtmlScripts", "*.htmlscripts", "HTML with scripts"),
            new LanguageInfo("Il", "*.il", "MSIL"),
            new LanguageInfo("Ini", "*.ini", "Ini files"),
            new LanguageInfo("Java", "*.js", "Java"),
            new LanguageInfo("JavaScript", "*.jscript", "Java Script"),
            new LanguageInfo("Perl", "*.perl", "Perl"),
            new LanguageInfo("Php", "*.php", "PHP"),
            new LanguageInfo("Python", "*.python", "Python"),
            new LanguageInfo("PowerShell", "*.ps1", "PowerShell"),
            new LanguageInfo("sql_oracle", "*.sql", "SQL"),
            new LanguageInfo("Tcltk", "*.tcltk", "TclTk"),
            new LanguageInfo("unix_shell", "*.unixshell", "Unix Shell"),
            new LanguageInfo("vb_net", "*.vbnet", "Visual Basic NET"),
            new LanguageInfo("vbs_script", "*.vbscript", "VB Script"),
            new LanguageInfo("vbs_scripts", "*.vbscripts", "VB Script in HTML"),
            new LanguageInfo("Xml", "*.xml", "XML"),
            new LanguageInfo("xml_scripts", "*.xmlscripts", "XML with scripts"),
            new LanguageInfo("custom", "*.*", "custom scheme"),
        };

        private AssemblerParser asmParser = new AssemblerParser();
        private BatchParser bathParser = new BatchParser();
        private CobolParser cobolParser = new CobolParser();
        private C_SchemeParser cshemeParser = new C_SchemeParser();
        private Cs_SchemeParser csParser = new Cs_SchemeParser();
        private CBuilderParser cbuilderParser = new CBuilderParser();
        private DelphiParser delphiParser = new DelphiParser();
        private DfmParser dfmParser = new DfmParser();
        private Html_SchemeParser htmlParser = new Html_SchemeParser();
        private HtmlScriptsParser htmlScriptParser = new HtmlScriptsParser();
        private IlParser ilparser = new IlParser();
        private IniParser iniParser = new IniParser();
        private Js_SchemeParser jsParser = new Js_SchemeParser();
        private JScriptParser javaScriptParser = new JScriptParser();
        private PerlParser perlParser = new PerlParser();
        private PHPParser phpParser = new PHPParser();
        private Python_SchemeParser pythonParser = new Python_SchemeParser();
        private Parser powerShellParser = new Parser();
        private Sql_SchemeParser sqlParser = new Sql_SchemeParser();
        private TclTkParser tclTkParser = new TclTkParser();
        private UnixShellParser unixShellParser = new UnixShellParser();
        private VbNet_SchemeParser vbNetParser = new VbNet_SchemeParser();
        private VbScript_SchemeParser vbScriptParser = new VbScript_SchemeParser();
        private VbScripts_SchemeParser vbScriptsParser = new VbScripts_SchemeParser();
        private Xml_SchemeParser xmlParser = new Xml_SchemeParser();
        private XmlScripts_SchemeParser xmlScriptsParser = new XmlScripts_SchemeParser();
        private Parser parser1 = new Parser();
        private int customSchemeIndex = 25;
        private SaveFileDialog saveFileDialog = new SaveFileDialog { };
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Schemes");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            openFileDialog.Filter = "Xml files (*.xml)|*.xml";
            saveFileDialog.Filter = "Xml files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Schemes";
            saveFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Schemes";
            foreach (LanguageInfo info in langItems)
                languages.Add(info.FileType);

            string powerShellPath = Path.Combine(Path.GetFullPath(dir) + @"Resources\Editor\schemes", "powershell.xml");
            if (File.Exists(powerShellPath))
            {
                powerShellParser.Scheme.LoadFile(powerShellPath);
            }

            LoadCommand = new RelayCommand(LoadClick);
            SaveCommand = new RelayCommand(SaveClick);
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            Language = "Assembler";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public bool CustomEnabled
        {
            get
            {
                return customEnabled;
            }

            set
            {
                if (customEnabled != value)
                {
                    customEnabled = value;
                    OnPropertyChanged("CustomEnabled");
                }
            }
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
                    if (edit != null)
                    {
                        int idx = FindLangByName(language);
                        if (idx >= 0)
                        {
                            UpdateLanguage(idx);
                        }
                    }
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LoadClick()
        {
            if (openFileDialog.ShowDialog().Value)
            {
                parser1.Scheme.LoadFile(this.openFileDialog.FileName);
                Language = "custom";
            }
        }

        private void SaveClick()
        {
            if (saveFileDialog.ShowDialog().Value)
            {
                parser1.Scheme.SaveFile(this.saveFileDialog.FileName);
            }
        }

        private ILexer GetLexer(int index)
        {
            switch (index)
            {
                case 0:
                    return asmParser;
                case 1:
                    return bathParser;
                case 2:
                    return cobolParser;
                case 3:
                    return cshemeParser;
                case 4:
                    return csParser;
                case 5:
                    return cbuilderParser;
                case 6:
                    return delphiParser;
                case 7:
                    return dfmParser;
                case 8:
                    return htmlParser;
                case 9:
                    return htmlScriptParser;
                case 10:
                    return ilparser;
                case 11:
                    return iniParser;
                case 12:
                    return jsParser;
                case 13:
                    return javaScriptParser;
                case 14:
                    return perlParser;
                case 15:
                    return phpParser;
                case 16:
                    return pythonParser;
                case 17:
                    return powerShellParser;
                case 18:
                    return sqlParser;
                case 19:
                    return tclTkParser;
                case 20:
                    return unixShellParser;
                case 21:
                    return vbNetParser;
                case 22:
                    return vbScriptParser;
                case 23:
                    return vbScriptsParser;
                case 24:
                    return xmlParser;
                case 25:
                    return xmlScriptsParser;
                case 26:
                    return parser1;
                default:
                    return parser1;
            }
        }

        private bool FindFile(int index, out string fileName)
        {
            fileName = string.Empty;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
                dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"\..\..\..\Resources\Editor\Text");

            if (dirInfo.Exists)
            {
                if (index >= 0)
                    fileName = dirInfo.FullName + @"\" + langItems[index].FileType + ".txt";
            }

            return (fileName != string.Empty) && new FileInfo(fileName).Exists;
        }

        private void UpdateLanguage(int index)
        {
            edit.Lexer = GetLexer(index);
            string s = string.Empty;
            CustomEnabled = index == customSchemeIndex;
            if (FindFile(index, out s))
                edit.Source.LoadFile(s);
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

        private string RemoveFileExt(string fileName)
        {
            int p = fileName.LastIndexOf(".");
            return (p >= 0) ? fileName.Substring(0, p) : fileName;
        }

        private struct LanguageInfo
        {
            public string FileType;
            public string FileExt;
            public string Description;

            public LanguageInfo(string fileType, string fileExt, string description)
            {
                FileType = fileType;
                FileExt = fileExt;
                Description = description;
            }
        }
    }
}
