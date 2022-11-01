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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for AppearanceSettingsUserControl.xaml
    /// </summary>
    public partial class FontSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public FontSettingsUserControl()
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

                if (editor != null)
                    UpdateDataFromEditor();
            }
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.FontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
        }

        private void UpdateDataFromEditor()
        {
            FontFamilyComboBox.SelectedItem = Editor.FontFamily;

            var g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            var points = Editor.FontSize * 72 / g.DpiX;
            g.Dispose();

            FontSizeComboBox.SelectedValue = (int)Math.Ceiling(points) + "pt";
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedValue == null)
                return;

            Editor.FontSize = (double)new FontSizeConverter().ConvertFromString((string)FontSizeComboBox.SelectedValue);
        }
    }
}
