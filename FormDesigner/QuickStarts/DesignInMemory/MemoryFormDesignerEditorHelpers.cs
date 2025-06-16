#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Softwares

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DesignInMemory
{
    public enum ContentType
    {
        Design,
        UserCode,
        Designer,
    }

    public static class MemoryFormDesignerEditorHelpers
    {
        public static void SetEditorSource(Alternet.Editor.SyntaxEdit edit, ContentType contentType, MemoryFormDesignerDataSource source)
        {
            var syntaxEdit = edit;

            if (contentType == ContentType.UserCode)
            {
                syntaxEdit.Source = (Alternet.Editor.TextSource.ITextSource)source.UserCodeTextSource;
                syntaxEdit.Lexer = new Alternet.Syntax.Parsers.Roslyn.CsParser();
            }
            else if (contentType == ContentType.Designer)
            {
                syntaxEdit.Source = (Alternet.Editor.TextSource.ITextSource)source.DesignerTextSource;
                syntaxEdit.Lexer = new Alternet.Syntax.XmlParser();
            }
            else
                throw new System.Exception();
        }
    }
}
