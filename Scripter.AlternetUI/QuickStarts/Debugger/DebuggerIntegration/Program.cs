using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace DebuggerIntegration
{
    internal class Program
    {
        internal static bool SetSystemAppearance = true;

        [STAThread]
        public static void Main()
        {
            if (SetSystemAppearance)
            {
                AssemblyUtils.InvokeMethodWithResult(typeof(AppUtils), "SetSystemAppearanceIfDebug");
            }

            var application = new Application();
            var window = new Form1();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}