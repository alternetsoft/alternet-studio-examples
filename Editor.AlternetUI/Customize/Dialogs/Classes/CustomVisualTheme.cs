#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Drawing;

using Alternet.Editor;

namespace Customize.Dialogs
{
    public class CustomVisualTheme : StandardVisualTheme
    {
        public CustomVisualTheme()
            : base("Custom")
        {
        }

        protected override VisualThemeColors GetColors()
        {
            var colors = DarkVisualTheme.Instance.Colors.Clone();
            colors.Reswords = Color.Red;
            colors.WindowBackground = Color.FromArgb(40, 40, 40);
            return colors;
        }
    }
}
