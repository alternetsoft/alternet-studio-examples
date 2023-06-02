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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxEditor_Wpf
{
    public struct LanguageInfo
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

    public static class LangInfos
    {
#pragma warning disable SA1401 // Fields should be private
        public static LanguageInfo[] LangItems =
#pragma warning restore SA1401 // Fields should be private
        {
            new LanguageInfo("c#", "*.cs", "C#"),
            new LanguageInfo("vb_net", "*.vb", "Visual Basic NET"),
            new LanguageInfo("java", "*.java", "Java"),
            new LanguageInfo("c", "*.h;*.c", "ANSI C"),
            new LanguageInfo("vbs_script", "*.vbs",  "VB Script"),
            new LanguageInfo("t4", "*.tt", "T4"),

            new LanguageInfo("javascript", "*.js", "Java Script"),
            new LanguageInfo("xml", "*.xml", "XML"),
            new LanguageInfo("html", "*.htm;*.html", "HTML"),
            new LanguageInfo("-", string.Empty, string.Empty),

            new LanguageInfo("perl", "*.pl", "Perl"),
            new LanguageInfo("php", "*.php", "PHP"),

            new LanguageInfo("assembler", "*.asm", "Assembler"),
            new LanguageInfo("delphi", "*.pas;*.bpg;*.dpr;*.dpk", "Delphi"),

            new LanguageInfo("ms_sql", "*.sql", "MS SQL"),
            new LanguageInfo("sql_oracle", "*.sql", "SQL"),
            new LanguageInfo("tcltk", "*.tcl", "TclTk"),

            new LanguageInfo("-", string.Empty, string.Empty),

            new LanguageInfo("batch", "*.bat", "Command Prompt"),
            new LanguageInfo("ini", "*.ini", "Ini files"),
            new LanguageInfo("lua", "*.lua", "Lua files"),
            new LanguageInfo("unix_shell", "*.sh;.csh", "Unix Shell"),
            new LanguageInfo("text", "*.txt", "Text files"),
            new LanguageInfo("-", string.Empty, string.Empty),

            new LanguageInfo("htmlscripts", "*.htm;*.html", "HTML with scripts"),
            new LanguageInfo("Css", "*.css", "Css"),
            new LanguageInfo("Ruby", "*.ruby", "Ruby files"),
            new LanguageInfo("all", "*.*", "All files"),
        };
    }
}
