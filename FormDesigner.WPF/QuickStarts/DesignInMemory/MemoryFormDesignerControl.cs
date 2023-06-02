#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.IO;
using System.Text;
using Alternet.Common;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.InMemory.Wpf
{
    public class MemoryFormDesignerControl : FormDesignerControl
    {
        protected override IResourceResolutionService CreateResourceResolutionService()
        {
            return new MemoryResourceResolutionService(this);
        }
    }
}