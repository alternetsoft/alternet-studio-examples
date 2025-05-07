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

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class WordWrapSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public WordWrapSettingsUserControl()
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

        private void UpdateDataFromEditor()
        {
            WordWrapCheckBox.IsChecked = Editor.WordWrap;
        }

        private void WordWrapCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.WordWrap = WordWrapCheckBox.IsChecked.Value;
        }
    }
}
