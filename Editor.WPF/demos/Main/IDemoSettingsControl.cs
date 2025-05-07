#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows.Controls;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public interface IDemoSettingsControl
    {
        TextEditor Editor { get; set; }

        UserControl Control { get; }
    }
}
