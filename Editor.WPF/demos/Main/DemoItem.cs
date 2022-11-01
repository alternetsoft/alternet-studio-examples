#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public delegate void InitializeEditorDelegate(TextEditor editor);

    public class DemoItem
    {
        public string Name { get; set; }

        public InitializeEditorDelegate OnInitializeEditor { get; set; }

        public InitializeEditorDelegate OnDeinitializeEditor { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
