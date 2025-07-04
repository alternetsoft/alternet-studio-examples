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

namespace SearchReplace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel model;

        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel(this);
            this.DataContext = model;
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}
