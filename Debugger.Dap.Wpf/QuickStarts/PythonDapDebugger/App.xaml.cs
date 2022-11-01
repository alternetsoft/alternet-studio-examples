#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#if NET5_0
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif

namespace PythonDapDebugger.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
