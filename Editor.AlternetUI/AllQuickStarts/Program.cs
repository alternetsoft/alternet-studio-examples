using System;
using Alternet.UI;
using Alternet.Editor.Common.AlternetUI;

namespace AllDemos
{
    internal class Program
    {
        static Program()
        {
            KnownAssemblies.PreloadReferenced();
            LexerDemoUtils.PreloadAssemblyMetaData();
        }

        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var window = new MainWindowSimple();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}