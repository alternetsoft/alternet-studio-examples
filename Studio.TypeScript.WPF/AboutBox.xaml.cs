#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AlternetStudio.TypeScript.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AdressLabel_Click(object sender, MouseButtonEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed) && (e.ClickCount == 1))
            {
                laAdress.Foreground = Brushes.Purple;
                try
                {
                    System.Diagnostics.Process.Start(laAdress.Content.ToString());
                }
                catch
                {
                }
            }
        }

        private void MailToLabel_Click(object sender, MouseButtonEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed) && (e.ClickCount == 1))
            {
                laMailTo.Foreground = Brushes.Purple;
                System.Diagnostics.Process.Start("mailto:contact@alternetsoft.com");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }
    }
}
