#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Windows.Forms;

namespace DebugRemoteScript
{
    public class ScriptAPI : IScriptAPI
    {
        private MainForm form;

        public ScriptAPI()
        {
        }

        public ScriptAPI(MainForm form)
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
            return form.Invoke(method);
        }
    }
}
