#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for AppearanceSettingsUserControl.xaml
    /// </summary>
    public partial class SelectionSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public SelectionSettingsUserControl()
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

        private void AllowStreamSelectionCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            AllowedSelectionModeChanged();
        }

        private void AllowBlockSelectionCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            AllowedSelectionModeChanged();
        }

        private void AllowedSelectionModeChanged()
        {
            var mode = AllowedSelectionMode.None;

            if (AllowStreamSelectionCheckBox.IsChecked.Value)
                mode |= AllowedSelectionMode.Stream;

            if (AllowBlockSelectionCheckBox.IsChecked.Value)
                mode |= AllowedSelectionMode.Block;

            Editor.AllowedSelectionMode = mode;
        }

        private void SelectionColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.SelectionBrush = new SolidColorBrush(e.NewValue);
        }

        private void InactiveSelectionColorComboBox_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Editor.InactiveSelectionBrush = new SolidColorBrush(e.NewValue);
        }

        private void UpdateDataFromEditor()
        {
            var mode = Editor.AllowedSelectionMode;

            AllowStreamSelectionCheckBox.IsChecked = (mode & AllowedSelectionMode.Stream) != 0;
            AllowBlockSelectionCheckBox.IsChecked = (mode & AllowedSelectionMode.Block) != 0;

            SelectionColorComboBox.SelectedColor = ((SolidColorBrush)Editor.SelectionBrush).Color;
            InactiveSelectionColorComboBox.SelectedColor = ((SolidColorBrush)Editor.InactiveSelectionBrush).Color;

            AllowDragAndDropCheckBox.IsChecked = (Editor.Selection.Options & SelectionOptions.DisableDragging) == 0;
            DisableSelectionCheckBox.IsChecked = (Editor.Selection.Options & SelectionOptions.DisableSelection) != 0;
            DisableMouseSelectionCheckBox.IsChecked = (Editor.Selection.Options & SelectionOptions.DisableSelectionByMouse) != 0;
        }

        private void DisableSelectionCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!DisableSelectionCheckBox.IsChecked.Value)
                Editor.Selection.Options &= ~SelectionOptions.DisableSelection;
            else
                Editor.Selection.Options |= SelectionOptions.DisableSelection;
        }

        private void DisableMouseSelectionCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!DisableMouseSelectionCheckBox.IsChecked.Value)
                Editor.Selection.Options &= ~SelectionOptions.DisableSelectionByMouse;
            else
                Editor.Selection.Options |= SelectionOptions.DisableSelectionByMouse;
        }

        private void AllowDragAndDropCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (AllowDragAndDropCheckBox.IsChecked.Value)
                Editor.Selection.Options &= ~SelectionOptions.DisableDragging;
            else
                Editor.Selection.Options |= SelectionOptions.DisableDragging;
        }
    }
}
