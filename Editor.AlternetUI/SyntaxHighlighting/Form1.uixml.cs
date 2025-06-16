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

using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Generic;
using Alternet.Syntax.Lexer;

namespace SyntaxHighlighting
{
    public partial class Form1 : Window
    {
        private readonly OpenFileDialog openFileDialog1 = new();
        private readonly SaveFileDialog saveFileDialog1 = new();
        private readonly Parser powerShellParser = new();
        private readonly LanguageInfo customScheme
            = new LanguageInfo("custom", "*.*", "Custom Scheme", () => new Parser());

        private LanguageInfo[] langItems;

        public Form1()
        {
            langItems = new LanguageInfo[]
            {
                new LanguageInfo("c#", "*.cs", "C#", ()=> InitParser(new Cs_SchemeParser())),
                
                new LanguageInfo("assembler", "*.assembler", "Assembler",
                ()=> InitParser(new AssemblerParser())),

                new LanguageInfo("batch", "*.batch", "Command Prompt",
                ()=> InitParser(new BatchParser())),
                
                new LanguageInfo("cobol", "*.CBL", "COBOL",
                ()=> InitParser(new CobolParser())),
                
                new LanguageInfo("c", "*.c", "ANSI C",
                ()=> InitParser(new C_SchemeParser())),
                
                new LanguageInfo("c++builder", "*.cbuilder", "C++ Builder",
                ()=> InitParser(new CBuilderParser())),
                
                new LanguageInfo("delphi", "*.delphi", "Delphi",
                ()=> InitParser(new DelphiParser())),
                
                new LanguageInfo("dfm", "*.dfm", "Dfm",
                ()=> InitParser(new DfmParser())),
                
                new LanguageInfo("html", "*.html", "Html",
                ()=> InitParser(new Html_SchemeParser())),
                
                new LanguageInfo(
                    "htmlscripts",
                    "*.htmlscripts",
                    "HTML with scripts",
                    ()=> InitParser(new HtmlScriptsParser())),
                
                new LanguageInfo("il", "*.il", "MSIL",
                ()=> InitParser(new IlParser())),
                
                new LanguageInfo("ini", "*.ini", "Ini files",
                ()=> InitParser(new IniParser())),
                
                new LanguageInfo("java", "*.js", "Java",
                ()=> InitParser(new Js_SchemeParser())),
                
                new LanguageInfo("javascript", "*.jscript", "Java Script",
                ()=> InitParser(new JScriptParser())),
                
                new LanguageInfo("perl", "*.perl", "Perl",
                ()=> InitParser(new PerlParser())),
                
                new LanguageInfo("php", "*.php", "PHP",
                ()=> InitParser(new PHPParser())),
                
                new LanguageInfo("python", "*.python", "Python",
                ()=> InitParser(new Python_SchemeParser())),
                
                new LanguageInfo("powershell", "*.ps1", "PowerShell",
                ()=> InitParser(powerShellParser)),
                
                new LanguageInfo("SQL_Oracle", "*.sql", "SQL",
                ()=> InitParser(new Sql_SchemeParser())),
                
                new LanguageInfo("tcltk", "*.tcltk", "TclTk",
                ()=> InitParser(new TclTkParser())),
                
                new LanguageInfo("unix_shell", "*.unixshell", "Unix Shell",
                ()=> InitParser(new UnixShellParser())),
                
                new LanguageInfo("vb_net", "*.vbnet", "Visual Basic NET",
                ()=> InitParser(new VbNet_SchemeParser())),
                
                new LanguageInfo("vbs_script", "*.vbscript", "VB Script",
                ()=> InitParser(new VbScript_SchemeParser())),
                
                new LanguageInfo(
                    "vbs_scripts",
                    "*.vbscripts",
                    "VB Script in HTML",
                    ()=> InitParser(new VbScripts_SchemeParser())),
                
                new LanguageInfo("xml", "*.xml", "XML",
                ()=> InitParser(new Xml_SchemeParser())),
                
                new LanguageInfo(
                    "xml_scripts",
                    "*.xmlscripts",
                    "XML with scripts",
                    ()=> InitParser(new XmlScripts_SchemeParser())),
                
                customScheme,
            };

            InitializeComponent();

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;
            }

            syntaxEdit1.Outlining.AllowOutlining = true;
            Idle += Form1_Idle;

            Form1_Load(this, EventArgs.Empty);

            LogUtils.RegisterLogAction("Check Demo Files", CheckFiles);

            Form1_Idle(this, EventArgs.Empty);
            ActiveControl = syntaxEdit1;
        }

        public Lexer InitParser(Lexer lexer)
        {
            lexer.Scheme.MakeForeColorLighterLighterIfDark(syntaxEdit1);
            return lexer;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Idle(object? sender, EventArgs e)
        {
            lbDescription.WrapToParent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Schemes/");

            openFileDialog1.InitialDirectory = dirInfo.FullName;
            saveFileDialog1.InitialDirectory = dirInfo.FullName;

            foreach (LanguageInfo info in langItems)
                LanguagesListBox.Items.Add(info);

            string powerShellPath = Path.Combine(dirInfo.FullName, "powershell.xml");
            if (File.Exists(powerShellPath))
            {
                powerShellParser.Scheme.LoadFile(powerShellPath);
                InitParser(powerShellParser);
            }

            LanguagesListBox.SelectedIndex = 0;
            LanguagesListBox.SelectionChanged += LanguagesListBox_SelectedIndexChanged;
            LanguagesListBox_SelectedIndexChanged(LanguagesListBox, EventArgs.Empty);
            btLoadScheme.Click += LoadSchemeButton_Click;
        }

        private void LanguagesListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateLanguage(LanguagesListBox.SelectedItem as LanguageInfo);
        }

        private void UpdateLanguage(LanguageInfo? info)
        {
            App.DoInsideBusyCursor(() =>
            {
                syntaxEdit1.Lexer = info?.Lexer;
                string s = string.Empty;
                bool customEnabled = info == customScheme;
                if (FindFile(info, out s))
                    syntaxEdit1.LoadFile(s);
            });
        }

        private void CheckFiles()
        {
            if (!App.IsWindowsOS)
                return;
            foreach (LanguageInfo info in langItems)
                CheckFile(info);

            void CheckFile(LanguageInfo? info)
            {
                FindFile(info, out var fileName);

                if (FileUtils.RealFileHasSameCase(fileName))
                    return;

                App.Log($"Bad case: {fileName}");
            }
        }

        private bool FindFile(LanguageInfo? info, out string fileName)
        {
            fileName = string.Empty;
            DirectoryInfo dirInfo = new(DemoUtils.ResourcesFolder + @"Editor/Text");

            if (dirInfo.Exists)
            {
                if (info is not null)
                    fileName = dirInfo.FullName + @"/" + info.FileType + ".txt";
            }

            return (fileName != string.Empty) && new FileInfo(fileName).Exists;
        }

        private void LoadSchemeButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.ShowAsync(() =>
            {
                customScheme.Lexer.Scheme.LoadFile(this.openFileDialog1.FileName);
                InitParser(customScheme.Lexer);
                LanguagesListBox.SelectedItem = customScheme;
            });
        }

        private class LanguageInfo
        {
            private Func<Alternet.Syntax.Lexer.Lexer> CreateFunc;
            private Alternet.Syntax.Lexer.Lexer? lexer;

            public string FileType;
            public string FileExt;
            public string Description;

            public LanguageInfo(
                string fileType,
                string fileExt,
                string description,
                Func<Alternet.Syntax.Lexer.Lexer> createFunc)
            {
                FileType = fileType;
                FileExt = fileExt;
                Description = description;
                CreateFunc = createFunc;
            }

            public Lexer Lexer
            {
                get
                {
                    lexer ??= CreateFunc();
                    return lexer;
                }
            }

            public override string ToString()
            {
                return Description;
            }
        }
    }
}
