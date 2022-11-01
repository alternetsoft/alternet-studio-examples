#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System.Windows.Controls;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.Wpf
{
    [ExtensionFor(typeof(Label))]
    public class LabelDesignItemInitializer : DesignItemInitializer
    {
        public override void InitializeDesignItem(DesignItem item)
        {
            item.Properties["Content"].SetValue("Text modified by LabelDesignItemInitializer");
        }
    }
}