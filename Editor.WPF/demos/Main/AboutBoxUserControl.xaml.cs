#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBoxUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public AboutBoxUserControl()
        {
            InitializeComponent();
        }

        public TextEditor Editor
        {
            get
            {
                return editor;
            }

            set
            {
                if (editor == value)
                    return;

                editor = value;
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
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
    }
}
