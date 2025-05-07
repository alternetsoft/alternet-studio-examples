#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.Collections.Generic;
using System.IO;

using Alternet.Syntax;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.TextMate;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public class ProgrammingLanguagesDemoInitializer
    {
#pragma warning disable SA1401 // Fields should be private
        public static string SampleDir;
#pragma warning restore SA1401 // Fields should be private
        private static IDictionary<string, IParser> parsers = new Dictionary<string, IParser>();

        public static void InitializeLanguagesWithParsersDemoGroup(DemoItemsGroup group)
        {
            var csharpItem = new DemoItem { Name = "C#", OnInitializeEditor = CSharpItemInit };
            group.Items.Add(csharpItem);

            var vbItem = new DemoItem { Name = "Visual Basic .NET", OnInitializeEditor = VisualBasicItemInit };
            group.Items.Add(vbItem);

            var vbScriptItem = new DemoItem { Name = "VBScript", OnInitializeEditor = VBScriptItemInit };
            group.Items.Add(vbScriptItem);

            var javaItem = new DemoItem { Name = "Java", OnInitializeEditor = JavaItemInit };
            group.Items.Add(javaItem);

            var citem = new DemoItem { Name = "C", OnInitializeEditor = CItemInit };
            group.Items.Add(citem);

            var xmlItem = new DemoItem { Name = "XML", OnInitializeEditor = XmlItemInit };
            group.Items.Add(xmlItem);

            var htmlItem = new DemoItem { Name = "HTML", OnInitializeEditor = HtmlItemInit };
            group.Items.Add(htmlItem);

            var javaScriptItem = new DemoItem { Name = "JavaScript", OnInitializeEditor = JavaScriptItemInit };
            group.Items.Add(javaScriptItem);
        }

        public static void InitializeProgrammingLanguagesDemoGroup(DemoItemsGroup group)
        {
            var asmItem = new DemoItem { Name = "x86 Assembler", OnInitializeEditor = AssemblerItemInit };
            group.Items.Add(asmItem);

            var pythonItem = new DemoItem { Name = "Python", OnInitializeEditor = PythonItemInit };
            group.Items.Add(pythonItem);

            var powerShellItem = new DemoItem { Name = "PowerShell", OnInitializeEditor = PowerShellItemInit };
            group.Items.Add(powerShellItem);

            var rubyItem = new DemoItem { Name = "Ruby", OnInitializeEditor = RubyItemInit };
            group.Items.Add(rubyItem);

            var tcltkItem = new DemoItem { Name = "TCL/TK", OnInitializeEditor = TcltkItemInit };
            group.Items.Add(tcltkItem);

            var unixShellItem = new DemoItem { Name = "Unix Shell", OnInitializeEditor = UnixShellItemInit };
            group.Items.Add(unixShellItem);

            var htmlWithScriptsItem = new DemoItem { Name = "HTML With Scripts", OnInitializeEditor = HtmlWithScriptsItemInit };
            group.Items.Add(htmlWithScriptsItem);

            var cssItem = new DemoItem { Name = "CSS", OnInitializeEditor = CssItemInit };
            group.Items.Add(cssItem);

            var phpItem = new DemoItem { Name = "PHP", OnInitializeEditor = PhpItemInit };
            group.Items.Add(phpItem);

            var batItem = new DemoItem { Name = "Windows Batch Files", OnInitializeEditor = BatchItemInit };
            group.Items.Add(batItem);

            var iniItem = new DemoItem { Name = "Windows INI Files", OnInitializeEditor = IniItemInit };
            group.Items.Add(iniItem);

            var mssqlItem = new DemoItem { Name = "MS SQL", OnInitializeEditor = MsSqlItemInit };
            group.Items.Add(mssqlItem);

            var oracleSqlItem = new DemoItem { Name = "Oracle SQL", OnInitializeEditor = OracleSqlItemInit };
            group.Items.Add(oracleSqlItem);

            var delphiItem = new DemoItem { Name = "Delphi", OnInitializeEditor = DelphiItemInit };
            group.Items.Add(delphiItem);

            var luaItem = new DemoItem { Name = "Lua", OnInitializeEditor = LuaItemInit };
            group.Items.Add(luaItem);

            var perlItem = new DemoItem { Name = "Perl", OnInitializeEditor = PerlItemInit };
            group.Items.Add(perlItem);
        }

        public static void CSharpItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "c#.cs"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("c#", out parser))
            {
                parser = new Alternet.Syntax.Parsers.Roslyn.CsParser();
                parsers["c#"] = parser;
            }

            editor.Lexer = parser;
        }

        private static IParser LoadParser(string scheme)
        {
            IParser parser;
            if (!parsers.TryGetValue(scheme, out parser))
            {
                parser = new Parser();
                parsers[scheme] = parser;

                var fileInfo = new FileInfo(Path.Combine(SampleDir, "schemes", scheme));
                if (fileInfo.Exists)
                    parser.Scheme.LoadFile(fileInfo.FullName);
            }

            return parser;
        }

        private static void AssemblerItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "assembler.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("assembler.xml");
        }

        private static void BatchItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "batch.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("batch.xml");
        }

        private static void CssItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "Css.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("css.xml");
        }

        private static void DelphiItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "delphi.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("delphi.xml");
        }

        private static void HtmlWithScriptsItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "htmlscripts.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("htmlscripts.xml");
        }

        private static void IniItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "ini.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("ini.xml");
        }

        private static void LuaItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "Lua.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("lua.xml");
        }

        private static void MsSqlItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "Ms_Sql.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("ms_sql.xml");
        }

        private static void OracleSqlItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "sql_oracle.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("sql_oracle.xml");
        }

        private static void PerlItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "perl.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("perl.xml");
        }

        private static void PhpItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "php.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("php.xml");
        }

        private static void PythonItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "python.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("python.xml");
        }

        private static void PowerShellItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "powershell.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("powershell.xml");
        }

        private static void RubyItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "Ruby.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("Ruby.xml");
        }

        private static void TcltkItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "tcltk.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("tcltk.xml");
        }

        private static void UnixShellItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "unix_shell.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            editor.Lexer = LoadParser("unix_shell.xml");
        }

        private static void VisualBasicItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "vb_net.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("vb_net", out parser))
            {
                parser = new Alternet.Syntax.Parsers.Roslyn.VbParser();
                parsers["vb_net"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void HtmlItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "html.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("html_script", out parser))
            {
                parser = new HtmlScriptParser();
                parsers["html_script"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void CItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "c.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("c", out parser))
            {
                parser = new CParser();
                parsers["c"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void VBScriptItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "vbs_script.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("vbs", out parser))
            {
                parser = new VbScriptParser();
                parsers["vbs"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void XmlItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "xml.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("xml", out parser))
            {
                parser = new XmlParser();
                parsers["xml"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void JavaItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "java.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("java", out parser))
            {
                parser = new JsParser();
                parsers["java"] = parser;
            }

            editor.Lexer = parser;
        }

        private static void JavaScriptItemInit(TextEditor editor)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(SampleDir, "text", "java_script.txt"));
            if (fileInfo.Exists)
                editor.Source.LoadFile(fileInfo.FullName);

            IParser parser;
            if (!parsers.TryGetValue("javascript", out parser))
            {
                parser = new JavaScriptParser();
                parsers["javascript"] = parser;
            }

            editor.Lexer = parser;
        }
    }
}
