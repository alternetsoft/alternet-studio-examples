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
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Alternet.Common;
using Microsoft.Win32;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class ThemeSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public ThemeSettingsUserControl()
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

        private void VisualThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var theme = (VisualThemeType)((ComboBoxItem)VisualThemeComboBox.SelectedItem).Tag;
            Editor.VisualThemeType = theme;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (VisualThemeType value in Enum.GetValues(typeof(VisualThemeType)))
                VisualThemeComboBox.Items.Add(new ComboBoxItem { Content = value.ToString(), Tag = value });
        }
    }
}
