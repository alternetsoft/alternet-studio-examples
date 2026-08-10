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
            var forceLicenseCheck = false;

            if (forceLicenseCheck)
            {
                Alternet.Common.License.ComponentLicenseProvider.ForceWarningWhenDebuggerIsAttached = true;
            }

            AssemblyUtils.InvokeMethodWithResult(typeof(AppUtils), "SetSystemAppearanceIfDebug");

            var application = new Application();
            var window = new MainWindowSimple();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}