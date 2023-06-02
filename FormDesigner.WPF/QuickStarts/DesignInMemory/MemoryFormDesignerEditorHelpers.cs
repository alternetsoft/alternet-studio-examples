#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Softwares

using System.IO;

using Alternet.FormDesigner.Wpf;

namespace FormDesigner.InMemory.Wpf
{
    public enum ContentType
    {
        Xaml,
        UserCode,
    }

    public static class MemoryFormDesignerEditorHelpers
    {
        public static void SetEditorSource(Alternet.Editor.Wpf.TextEditor edit, ContentType contentType, MemoryFormDesignerDataSource source)
        {
            var syntaxEdit = edit;

            if (contentType == ContentType.UserCode)
            {
                syntaxEdit.Source = (Alternet.Editor.Wpf.ITextSource)source.UserCodeTextSource;
                syntaxEdit.Lexer = new Alternet.Syntax.Parsers.Roslyn.CsParser();
            }
            else if (contentType == ContentType.Xaml)
            {
                syntaxEdit.Source = (Alternet.Editor.Wpf.ITextSource)source.XamlTextSource;
                syntaxEdit.Lexer = new Alternet.Syntax.XmlParser();
            }
            else
                throw new System.Exception();
        }
    }
}
