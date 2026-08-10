#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunScriptMethods
{
    public static class Program
    {
        private const uint ATTACHPARENTPROCESS = 0xFFFFFFFF;

        public static void AllocateConsole()
        {
            // Try attach to parent console (if started from a console), otherwise create a new one
            if (!AttachConsole(ATTACHPARENTPROCESS))
            {
                AllocConsole();
            }

            // Redirect Console.* to the console window so Console.WriteLine will work
            var stdOut = Console.OpenStandardOutput();
            var writer = new StreamWriter(stdOut) { AutoFlush = true };
            Console.SetOut(writer);
            Console.SetError(writer);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            AllocateConsole();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);
    }
}
