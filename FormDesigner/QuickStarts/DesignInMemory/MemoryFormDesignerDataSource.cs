#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Common;
using Alternet.FormDesigner.WinForms;
using static System.Net.Mime.MediaTypeNames;

namespace DesignInMemory
{
    public class MemoryFormDesignerDataSource
        : IFormDesignerDataSource
    {
        public MemoryFormDesignerDataSource(
            string name,
            string designerText,
            string userCodeText,
            string resourceText,
            string designedClassName,
            Language userCodeLanguage,
            Func<string, IFormDesignerTextSource> openTextSourceFunc)
        {
            Name = name;
            UserCodeTextSource = openTextSourceFunc(userCodeText);
            DesignerTextSource = openTextSourceFunc(designerText);
            ResourceTextSource = openTextSourceFunc(resourceText);
            UserCodeLanguage = userCodeLanguage;
            DesignedClassFullName = designedClassName;
            DesignedComponentAssembly = new DesignedComponentAssemblyFileProvider();
        }

        public event EventHandler<ResourceCultureAddedEventArgs> ResourceCultureAdded;

        public string Name { get; private set; }

        public Language UserCodeLanguage { get; private set; }

        public IFormDesignerTextSource UserCodeTextSource { get; private set; }

        public IFormDesignerTextSource DesignerTextSource { get; private set; }

        public IFormDesignerTextSource ResourceTextSource { get; private set; }

        public string DesignerFileName { get; set; }

        public string UserCodeFileName { get; set; }

        public string Path { get; set; }

        public bool IsModified { get; private set; }

        public IDesignedComponentAssemblyProvider DesignedComponentAssembly { get; }

        public string DesignedClassFullName { get; private set; }

        public IEnumerable<DesignerAssemblyResources> AssemblyResources { get; }

        public bool SupportsCultureResources => false;

        public string GetResourceFileName(CultureInfo cultureInfo)
        {
            return null;
        }

        public void InsertTextIntoUserCode(FormDesignerTextSourceOperations.TextInsertionOperation[] operations)
        {
            Check.NonNull(operations, "operations");

            UserCodeTextSource.InsertText(operations);

            IsModified = true;
        }

        public Stream OpenDesignerStream(StreamAccessMode mode)
        {
            if (mode == StreamAccessMode.Write)
                IsModified = true;

            return OpenStream(DesignerTextSource, mode);
        }

        public Stream OpenResourceStream(StreamAccessMode mode, CultureInfo cultureInfo)
        {
            if (mode == StreamAccessMode.Write)
                IsModified = true;

            return OpenStream(ResourceTextSource, mode);
        }

        public Stream OpenUserCodeStreamForReading()
        {
            return OpenStream(UserCodeTextSource, StreamAccessMode.Read);
        }

        public void ReplaceTextInDesignerCode(FormDesignerTextSourceOperations.TextReplacementOperation[] operations)
        {
            Check.NonNull(operations, "operations");

            DesignerTextSource.ReplaceText(operations);
            IsModified = true;
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
