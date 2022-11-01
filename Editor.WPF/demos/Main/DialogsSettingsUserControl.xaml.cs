#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using Alternet.Common;
using Microsoft.Win32;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class DialogsSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public DialogsSettingsUserControl()
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

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Multiselect = false };

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                Editor.Source.Lexer = null;
                Editor.Source.LoadFile(dialog.FileName);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                Editor.Source.SaveFile(dialog.FileName);
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.DisplaySearchDialog(Window.GetWindow(this));
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.DisplayReplaceDialog(Window.GetWindow(this));
        }

        private void GoToLineButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.DisplayGotoLineDialog(Window.GetWindow(this));
        }

        private void DialogLanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var previousCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentUICulture =
                new CultureInfo((string)((ComboBoxItem)DialogLanguageComboBox.SelectedItem).Tag);

            try
            {
                StringConsts.Localize();
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = previousCulture;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
