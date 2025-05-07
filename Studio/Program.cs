#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows.Forms;

using Alternet.Editor;

namespace AlternetStudio.Demo
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

            Form mainForm;
            RemoteControl.RemoteControlParameters remoteControlParameters;
            if (RemoteControl.CommandLineParser.TryParseCommandLine(args, out remoteControlParameters))
                mainForm = new RemoteControl.RemoteControlMainForm(remoteControlParameters);
            else
                mainForm = new MainForm();

            Application.Run(mainForm);
        }
    }
}
