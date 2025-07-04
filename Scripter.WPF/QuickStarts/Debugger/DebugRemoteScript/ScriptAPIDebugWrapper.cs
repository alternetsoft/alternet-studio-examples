﻿#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows.Controls;

namespace DebugRemoteScript.Wpf
{
    public class ScriptAPIDebugWrapper : MarshalByRefObject, IScriptAPI
    {
        public static IScriptAPI ScriptAPI { get; set; }

        public string GetEditorText()
        {
            string result = null;
            Invoke((Action)(() => result = ScriptAPI.GetEditorText()));
            return result;
        }

        public void ShowMessage(string text)
        {
            Invoke((Action)(() => ScriptAPI.ShowMessage(text)));
        }

        public object Invoke(Delegate method)
        {
            return ScriptAPI.Invoke(method);
        }
    }
}
