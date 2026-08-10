#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System.IO;
using System.Reflection;
using System.Windows;
using System.Xaml;

using Alternet.Editor.Wpf;

namespace LineStyles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ScrollBarStyleKind? scrollBarStyle = ScrollBarStyleKind.WpfDefault;

        private static bool loadCustomScrollBarStyle = false;

        private ViewModel model;

        static MainWindow()
        {
            if (scrollBarStyle != null)
            {
                TextEditor.ScrollBarStyle = scrollBarStyle.Value;

                if (!loadCustomScrollBarStyle)
                    return;

                var assembly = Assembly.GetExecutingAssembly();
                using (var stream
                    = assembly.GetManifestResourceStream("LineStyles.Themes.ScrollBar.xaml"))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var xaml = reader.ReadToEnd();
                            var resourceDict
                                = (ResourceDictionary)System.Windows.Markup.XamlReader.Parse(xaml);
                            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                        }
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            model = new ViewModel(syntaxEdit1);
            this.DataContext = model;
        }
    }
}
