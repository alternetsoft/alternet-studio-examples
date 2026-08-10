using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;

namespace PrintAndPreview
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            if (args.Length > 0 && args[0] == "-IsDark=true")
            {
                SyntaxEdit.DefaultVisualThemeType = VisualThemeType.Auto;
                PlatformUtils.ApplicationColorMode = SystemColorModeType.Dark;
            }

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
