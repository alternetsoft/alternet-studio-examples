#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for BackgroundAndBorderSettingsUserControl.xaml
    /// </summary>
    public partial class BackgroundAndBorderSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private BitmapImage backgroundImage;

        public BackgroundAndBorderSettingsUserControl()
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

        public BitmapImage BackgroundImage
        {
            get
            {
                if (backgroundImage != null)
                    return backgroundImage;

                byte[] bytes = Properties.Resources.sky;
                MemoryStream stream = new MemoryStream(bytes);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                return backgroundImage = image;
            }
        }

        private void UpdateEditor()
        {
            if (UseImageBackgroundCheckBox.IsChecked.Value)
                Editor.Background = new ImageBrush(BackgroundImage);
            else
                Editor.Background = new SolidColorBrush(BackgroundColorComboBox.SelectedColor);

            Editor.BorderBrush = new SolidColorBrush(BorderColorComboBox.SelectedColor);
            Editor.BorderThickness = new Thickness(Convert.ToDouble(BorderThicknessNumericUpDown.Value));
        }

        private void UpdateDataFromEditor()
        {
            BorderColorComboBox.SelectedColor = Editor.BorderBrush == null ? SystemColors.ControlDarkColor : ((SolidColorBrush)Editor.BorderBrush).Color;
            BorderThicknessNumericUpDown.Value = Convert.ToDecimal(Editor.BorderThickness.Left);
        }

        private void UseImageBackgroundCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateEditor();
        }

        private void BackgroundColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.Background = new SolidColorBrush(e.NewValue);
        }

        private void BorderColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.BorderBrush = new SolidColorBrush(e.NewValue);
        }

        private void BorderThicknessNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            Editor.BorderThickness = new Thickness(Convert.ToDouble(e.NewValue));
        }
    }
}
