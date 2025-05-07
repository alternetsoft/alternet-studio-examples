#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows;

namespace DebugRemoteScript.Wpf
{
    public class ScriptAPI : IScriptAPI
    {
        private MainWindow form;

        public ScriptAPI()
        {
        }

        public ScriptAPI(MainWindow form)
        {
            this.form = form;
        }

        public string GetEditorText()
        {
            return form.textBox1.Text;
        }

        public void ShowMessage(string text)
        {
            MessageBox.Show(text);
        }

        public object Invoke(Delegate method)
        {
            return form.Dispatcher.Invoke(method);
        }
    }
}
