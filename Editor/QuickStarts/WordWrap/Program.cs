using System;
using System.Windows.Forms;

using Alternet.Editor;

namespace WordWrap
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Supress for Visual Studio-generated code")]
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();

            SyntaxEditUtils.EnableDarkModeIfArgs();

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
