using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Editor;

#if NET5_0
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif

namespace DebuggerIntegration
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
#if NET9_0_OR_GREATER
#pragma warning disable
            if (args.Length > 0 && args[0] == "-IsDark=true")
            {
                SyntaxEdit.DefaultVisualThemeType = VisualThemeType.Auto;
                Application.SetColorMode(SystemColorMode.Dark);
            }
#pragma warning restore
#endif
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}