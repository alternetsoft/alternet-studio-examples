using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace DebuggerIntegration
{
    internal class Program
    {
        private static bool RegisterExcdeptionsLogger = false;

        [STAThread]
        public static void Main()
        {
            static void Nop()
            {                
            }

            if(RegisterExcdeptionsLogger)
            {
                DebugUtils.RegisterExceptionsLoggerIfDebug((e)=>{
                    Nop();
                });
            }

            var application = new Application();
            var window = new Form1();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}