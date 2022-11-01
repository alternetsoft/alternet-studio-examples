#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System.Windows;
using System.Windows.Controls;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class BookmarksSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;

        public BookmarksSettingsUserControl()
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

        private void ToggleBookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Source.BookMarks.ToggleBookMark();
        }

        private void GotoNextBookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Source.BookMarks.GotoNextBookMark();
            Editor.Focus();
        }
    }
}
