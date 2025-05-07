#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows.Controls;

using Alternet.Common;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.Wpf
{
    public partial class CustomEditContextMenu
    {
        private DesignItem designItem;

        public CustomEditContextMenu(DesignItem designItem)
        {
            this.designItem = designItem;
            InitializeComponent();
            LoadFromResource();
        }

        private void Click_DisplayCustomEditor(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = (Button)designItem.Component;
            var dialog = new EditButtonTextWindow { ButtonText = button.Content as string };
            dialog.ShowDialog();

            button.Content = dialog.ButtonText;
        }

        private void LoadFromResource()
        {
            CustomHeader.Header = StringConsts.MenuDisplayCustomEditorCaption;
        }
    }
}
