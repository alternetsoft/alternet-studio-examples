#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System.Collections.Generic;
using System.Linq;

namespace Alternet.FormDesigner.Demo
{
    public class FormSettings
    {
        public static readonly FormSettings Default = new FormSettings(Enumerable.Empty<AssemblyReference>());

        public FormSettings(IEnumerable<AssemblyReference> references, IEnumerable<string> searchPaths = null)
        {
            AssemblyReferences = references;
            SearchPaths = searchPaths;
        }

        public IEnumerable<AssemblyReference> AssemblyReferences { get; private set; }

        public IEnumerable<string> SearchPaths { get; private set; }

        public class AssemblyReference
        {
            public AssemblyReference(string assemblyPath)
            {
                AssemblyPath = assemblyPath;
            }

            public string AssemblyPath { get; private set; }
        }
    }
}