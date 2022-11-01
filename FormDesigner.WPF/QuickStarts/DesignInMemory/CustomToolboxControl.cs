#region Copyright (c) 2016-2022 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2022 Alternet Software

using System.Windows.Controls;
using Alternet.FormDesigner.Wpf;
using Alternet.FormDesigner.Wpf.Toolbox;

namespace FormDesigner.InMemory.Wpf
{
    public class CustomToolboxControl : ToolboxControl
    {
        protected override ToolboxContextMenu CreateContextMenu()
        {
            return new CustomContextMenu(this);
        }

        public class CustomContextMenu : ToolboxContextMenu
        {
            public CustomContextMenu(IToolboxControl toolboxControl)
                : base(toolboxControl)
            {
            }

            protected override void InitializeContextMenu()
            {
                base.InitializeContextMenu();

                ContextMenu.Items.Add(new Separator());

                var mi = new MenuItem { Header = "Reset Toolbox" };
                mi.Click += (o, e) => ToolboxControl.Reset();
                ContextMenu.Items.Add(mi);
            }
        }
    }
}