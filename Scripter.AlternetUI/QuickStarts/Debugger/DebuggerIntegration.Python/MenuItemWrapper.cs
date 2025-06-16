#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace DebuggerIntegration.Python
{
    /// <summary>
    /// While debugging the script, the script debugger executes script code in a separate process.
    /// The application objects have to be accessed in a thread-safe way from the script.
    /// </summary>
    public class MenuItemWrapper
    {
        private Window owner;
        private MenuItem item;

        public MenuItemWrapper(Window owner, MenuItem item)
        {
            this.owner = owner;
            this.item = item;
        }

        public bool Enabled
        {
            get
            {
                if (Window.InvokeRequired)
                {
                    bool value = false;
                    Window.Invoke(new Action(() => { value = item.Enabled; }));
                    return value;
                }
                else
                    return item.Enabled;
            }

            set
            {
                if (Window.InvokeRequired)
                    Window.Invoke(new Action(() => { item.Enabled = value; }));
                else
                    item.Enabled = value;
            }
        }

        public string Text
        {
            get
            {
                if (Window.InvokeRequired)
                {
                    string value = string.Empty;
                    Window.Invoke(new Action(() => { value = item.Text; }));
                    return value;
                }
                else
                    return item.Text;
            }

            set
            {
                if (Window.InvokeRequired)
                    Window.Invoke(new Action(() => { item.Text = value; }));
                else
                    item.Text = value;
            }
        }
    }
}
