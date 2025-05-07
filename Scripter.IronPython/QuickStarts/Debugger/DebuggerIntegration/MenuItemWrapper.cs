using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebuggerIntegration.IronPython
{
    /// <summary>
    /// While debugging the script, the script debugger executes script code in a separate process.
    /// The application objects have to be accessed in a thread-safe way from the script.
    /// </summary>
    public class MenuItemWrapper
    {
        private Form owner;
        private ToolStripMenuItem item;

        public MenuItemWrapper(Form owner, ToolStripMenuItem item)
        {
            this.owner = owner;
            this.item = item;
        }

        public bool Enabled
        {
            get
            {
                if (owner.InvokeRequired)
                {
                    bool value = false;
                    owner.Invoke(new Action(() => { value = item.Enabled; }));
                    return value;
                }
                else
                    return item.Enabled;
            }

            set
            {
                if (owner.InvokeRequired)
                    owner.Invoke(new Action(() => { item.Enabled = value; }));
                else
                    item.Enabled = value;
            }
        }

        public string Text
        {
            get
            {
                if (owner.InvokeRequired)
                {
                    string value = string.Empty;
                    owner.Invoke(new Action(() => { value = item.Text; }));
                    return value;
                }
                else
                    return item.Text;
            }

            set
            {
                if (owner.InvokeRequired)
                    owner.Invoke(new Action(() => { item.Text = value; }));
                else
                    item.Text = value;
            }
        }
    }
}
