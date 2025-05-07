#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows;

using AlternetStudio.Wpf.Demo;
using AlternetStudio.Wpf.Demo.RemoteControl;

namespace AlternetStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mw;
            RemoteControlParameters remoteControlParameters;
            if (CommandLineParser.TryParseCommandLine(e.Args, out remoteControlParameters))
                mw = new RemoteControlMainWindow(remoteControlParameters);
            else
                mw = new MainWindow();
            mw.Show();
        }
    }
}
