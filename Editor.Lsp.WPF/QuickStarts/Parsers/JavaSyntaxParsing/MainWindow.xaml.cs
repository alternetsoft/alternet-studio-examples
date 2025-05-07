#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.Windows;

namespace JavaSyntaxParsing
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
            model = new ViewModel(this, this.syntaxEdit1);
            this.DataContext = model;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
