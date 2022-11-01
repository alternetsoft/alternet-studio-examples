#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Windows;

namespace FormDesigner.Wpf
{
    /// <summary>
    /// Interaction logic for EditButtonTextWindow.xaml
    /// </summary>
    public partial class EditButtonTextWindow : Window
    {
        public EditButtonTextWindow()
        {
            InitializeComponent();
        }

        public string ButtonText
        {
            get
            {
                return ButtonTextTextBox.Text;
            }

            set
            {
                ButtonTextTextBox.Text = value;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
