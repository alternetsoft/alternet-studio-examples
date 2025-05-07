using Alternet.UI;
using System;

namespace UndoRedo
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var window = new Form1();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}