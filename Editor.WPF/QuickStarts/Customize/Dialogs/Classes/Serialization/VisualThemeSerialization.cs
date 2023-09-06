#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using Alternet.Common;

namespace Alternet.Editor.Wpf
{
    public static class VisualThemeSerialization
    {
        public static ISerializationInfo GetSerializationInfo(this IVisualTheme theme)
        {
            return new XmlVisualThemeInfo(theme);
        }

        public static void SetSerializationInfo(this IVisualTheme theme, ISerializationInfo info)
        {
            info.FixupReferences(theme);
        }
    }
}
