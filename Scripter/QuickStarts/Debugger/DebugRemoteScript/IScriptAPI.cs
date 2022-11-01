#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Windows.Forms;

namespace DebugRemoteScript
{
    public interface IScriptAPI
    {
        string GetEditorText();

        void ShowMessage(string text);

        object Invoke(Delegate method);
    }
}