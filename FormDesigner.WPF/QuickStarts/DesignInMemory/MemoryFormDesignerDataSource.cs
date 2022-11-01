#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.IO;
using System.Text;
using Alternet.Common;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.InMemory.Wpf
{
    public class MemoryFormDesignerDataSource : IFormDesignerBaseDataSource
    {
        public MemoryFormDesignerDataSource(
            string name,
            string xamlText,
            string userCodeText,
            string designedClassName,
            Language userCodeLanguage,
            Func<string, IFormDesignerTextSource> openTextSourceFunc)
        {
            Name = name;
            UserCodeTextSource = openTextSourceFunc(userCodeText);
            XamlTextSource = openTextSourceFunc(xamlText);
            UserCodeLanguage = userCodeLanguage;
            DesignedClassFullName = designedClassName;
        }

        public string Name { get; private set; }

        public string DesignedClassFullName { get; private set; }

        public IFormDesignerTextSource XamlTextSource { get; private set; }

        public IFormDesignerTextSource UserCodeTextSource { get; private set; }

        public Language UserCodeLanguage { get; private set; }

        public bool IsModified
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void InsertTextIntoUserCode(FormDesignerTextSourceOperations.TextInsertionOperation[] operations)
        {
            Check.NonNull(operations, "operations");

            UserCodeTextSource.InsertText(operations);
        }

        public Stream OpenXamlStream(StreamAccessMode mode)
        {
            return OpenStream(XamlTextSource, mode);
        }

        public Stream OpenUserCodeStreamForReading()
        {
            return OpenStream(UserCodeTextSource, StreamAccessMode.Read);
        }

        public void ReplaceTextInXamlCode(FormDesignerTextSourceOperations.TextReplacementOperation[] operations)
        {
            Check.NonNull(operations, "operations");

            XamlTextSource.ReplaceText(operations);
        }

        private Stream OpenStream(IFormDesignerTextSource source, StreamAccessMode mode)
        {
            switch (mode)
            {
                case StreamAccessMode.Read:
                    return new MemoryStream(Encoding.UTF8.GetBytes(source.Text));

                case StreamAccessMode.Write:
                    return new TextSourceStream(source);
            }

            throw Check.GetEnumValueNotSupportedException(mode);
        }
    }
}