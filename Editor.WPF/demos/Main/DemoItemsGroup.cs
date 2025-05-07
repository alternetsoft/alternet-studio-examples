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

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public class DemoItemsGroup
    {
        public DemoItemsGroup()
        {
            Items = new List<DemoItem>();
        }

        public string Name { get; set; }

        public List<DemoItem> Items { get; private set; }
    }
}
