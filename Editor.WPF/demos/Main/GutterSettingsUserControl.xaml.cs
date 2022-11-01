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
    public partial class GutterSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public GutterSettingsUserControl()
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

        private void GutterVisibleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.GutterVisible = GutterVisibleCheckBox.IsChecked.Value;
        }

        private void LineModificatorsVisibleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.LineModificatorsVisible = LineModificatorsVisibleCheckBox.IsChecked.Value;
        }

        private void GutterColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.GutterBrush = new SolidColorBrush(e.NewValue);
        }

        private void GutterWidthNumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            Editor.GutterWidth = Convert.ToDouble(GutterWidthNumericUpDown.Value);
        }

        private void LineNumbersVisibleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.LineNumbersVisible = LineNumbersVisibleCheckBox.IsChecked.Value;
        }

        private void UpdateDataFromEditor()
        {
            GutterVisibleCheckBox.IsChecked = Editor.GutterVisible;
            LineModificatorsVisibleCheckBox.IsChecked = Editor.LineModificatorsVisible;
            GutterColorComboBox.SelectedColor = ((SolidColorBrush)Editor.GutterBrush).Color;
            GutterWidthNumericUpDown.Value = Convert.ToDecimal(Editor.GutterWidth);

            LineNumbersVisibleCheckBox.IsChecked = Editor.LineNumbersVisible;
            LineNumbersColorComboBox.SelectedColor = ((SolidColorBrush)Editor.LineNumbersBrush).Color;
        }

        private void LineNumbersColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.LineNumbersBrush = new SolidColorBrush(e.NewValue);
        }
    }
}
