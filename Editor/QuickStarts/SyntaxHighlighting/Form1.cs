#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Generic;

namespace SyntaxHighlighting
{
    public partial class Form1 : Form
    {
        #region Private members

        private const string LoadSchemeDesc = "Load parser scheme from file";
        private const string LangListDesc = "Select available programming language";
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
        private Sql_SchemeParser sqlParser = new Sql_SchemeParser();
        private TclTkParser tclTkParser = new TclTkParser();
        private UnixShellParser unixShellParser = new UnixShellParser();
        private VbNet_SchemeParser vbNetParser = new VbNet_SchemeParser();
        private VbScript_SchemeParser vbScriptParser = new VbScript_SchemeParser();
        private VbScripts_SchemeParser vbScriptsParser = new VbScripts_SchemeParser();
        private Xml_SchemeParser xmlParser = new Xml_SchemeParser();
        private XmlScripts_SchemeParser xmlScriptsParser = new XmlScripts_SchemeParser();
        private Parser parser1 = new Parser();
        private int customSchemeIndex = 24;

        private string dir = Application.StartupPath + @"\";

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

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\schemes");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\";
                if (!Directory.Exists(dir))
                    dir = Application.StartupPath + @"\..\..\..\..\..\";
            }

            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\schemes";
            saveFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\schemes";
            IList<string> langs = new List<string>();
            foreach (LanguageInfo info in langItems)
                lbLanguages.Items.Add(info.Description);
            lbLanguages.SelectedIndex = 0;
        }

        private void LanguagesLisoxTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLanguage(lbLanguages.SelectedIndex);
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
                    return sqlParser;
                case 18:
                    return tclTkParser;
                case 19:
                    return unixShellParser;
                case 20:
                    return vbNetParser;
                case 21:
                    return vbScriptParser;
                case 22:
                    return vbScriptsParser;
                case 23:
                    return xmlParser;
                case 24:
                    return xmlScriptsParser;
                case 25:
                    return parser1;
                default:
                    return parser1;
            }
        }

        private void UpdateLanguage(int index)
        {
            Cursor.Current = Cursors.WaitCursor;
            syntaxEdit1.Lexer = GetLexer(index);
            string s = string.Empty;
            bool customEnabled = index == customSchemeIndex;
            if (FindFile(index, out s))
                syntaxEdit1.LoadFile(s);
        }

        private bool FindFile(int index, out string fileName)
        {
            fileName = string.Empty;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\";
                dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            }

            if (dirInfo.Exists)
            {
                if (index >= 0)
                    fileName = dirInfo.FullName + @"\" + langItems[index].FileType + ".txt";
            }

            return (fileName != string.Empty) && new FileInfo(fileName).Exists;
        }

        private void LoadSchemeButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                parser1.Scheme.LoadFile(this.openFileDialog1.FileName);
                lbLanguages.SelectedIndex = customSchemeIndex;
            }
        }

        private void LanguagesLisoxTextBox_MouseMove(object sender, MouseEventArgs e)
        {
             string str = toolTip1.GetToolTip(lbLanguages);
             if (str != LangListDesc)
                toolTip1.SetToolTip(lbLanguages, LangListDesc);
        }

        private void LoadSchemeButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoadScheme);
            if (str != LoadSchemeDesc)
                toolTip1.SetToolTip(btLoadScheme, LoadSchemeDesc);
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
