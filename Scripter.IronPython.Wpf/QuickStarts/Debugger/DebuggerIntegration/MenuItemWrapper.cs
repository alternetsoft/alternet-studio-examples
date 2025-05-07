using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DebuggerIntegration.IronPython.Wpf
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
                if (!owner.Dispatcher.CheckAccess())
                {
                    bool value = false;
                    owner.Dispatcher.Invoke(new Action(() => { value = item.IsEnabled; }));
                    return value;
                }
                else
                    return item.IsEnabled;
            }

            set
            {
                if (!owner.Dispatcher.CheckAccess())
                    owner.Dispatcher.Invoke(new Action(() => { item.IsEnabled = value; }));
                else
                    item.IsEnabled = value;
            }
        }

        public string Text
        {
            get
            {
                if (!owner.Dispatcher.CheckAccess())
                {
                    string value = string.Empty;
                    owner.Dispatcher.Invoke(new Action(() => { value = item.Header.ToString(); }));
                    return value;
                }
                else
                    return item.Header.ToString();
            }

            set
            {
                if (!owner.Dispatcher.CheckAccess())
                    owner.Dispatcher.Invoke(new Action(() => { item.Header = value; }));
                else
                    item.Header = value;
            }
        }
    }
}
