#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows.Forms;
using Alternet.Common;
using Alternet.FormDesigner.WinForms;
using Alternet.FormDesigner.WinForms.Toolbox;

namespace Alternet.FormDesigner.Demo
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
                ContextMenu.Items.Add(new ToolStripSeparator());
                ContextMenu.Items.Add(new ToolStripMenuItem(StringConsts.MenuResetToolboxCaption, null, (o, e) => ToolboxControl.Reset()));
            }
        }
    }
}
