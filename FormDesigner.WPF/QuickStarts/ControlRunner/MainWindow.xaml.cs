using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

using Alternet.Common;
using Alternet.Common.Wpf;
using Alternet.FormDesigner.Wpf;

using Microsoft.Win32;

namespace ControlRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "XAML files (*.xaml)|*.xaml|All files (*.*)|*.*",
            Title = "Select a XAML file to load",
        };

        public MainWindow()
        {
            InitializeComponent();

            LoadWindowButton.Click += LoadWindowButton_Click;

            LoadUserControlButton.Click += LoadUserControlButton_Click;

            LoadButton.Click += LoadButton_Click;

            DesignButton.Click += DesignButton_Click;
        }

        private void DesignButton_Click(object sender, RoutedEventArgs e)
        {
            var existingWindow = App.Current.Windows.OfType<SimpleFormDesignerWindow>().FirstOrDefault();
            if (existingWindow != null)
            {
                existingWindow.Show();
                return;
            }

            SimpleFormDesignerWindow window = new SimpleFormDesignerWindow();
            var s = GetControlXamlPath();
            window.OpenAllFormFiles(s);
            window.Show();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                LoadXaml(openFileDialog.FileName);
            }
        }

        private string GetControlXamlPath()
        {
            var appPath = PathUtilities.GetAppFolder();
            return System.IO.Path.Combine(appPath, @"Content\UserControlToRun\SampleTestControl.xaml");
        }

        private void LoadUserControlButton_Click(object sender, RoutedEventArgs e)
        {
            LoadXaml(GetControlXamlPath());
        }

        private void LoadWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var appPath = PathUtilities.GetAppFolder();
            var s = System.IO.Path.Combine(appPath, @"Content\WindowToRun\WindowToRun.xaml");
            LoadXaml(s);
        }

        private void SetError(string s)
        {
            var tb = new TextBlock
            {
                Text = s,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#6B7280"),
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            ContentHost.Content = tb;
        }

        private void LoadXaml(string xamlFileName)
        {
            var opt = new XamlUtility.XamlLoadOptions
            {
                Flags = XamlUtility.XamlLoadFlags.StripXClassAndEvents,
            };

            var result = XamlUtility.LoadXaml(xamlFileName, opt);
            if (result.IsSuccess)
            {
                if (result.Value is Window w)
                {
                    SetError($"Loaded window: {System.IO.Path.GetFileName(xamlFileName)}");
                    w.Show();
                }
                else
                {
                    ContentHost.Content = result.Value;
                }
            }
            else
            {
                SetError(result.ErrorMessage);
            }
        }
    }
}
