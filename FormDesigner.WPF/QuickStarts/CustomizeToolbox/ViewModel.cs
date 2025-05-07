#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Alternet.FormDesigner.Wpf;
using Alternet.FormDesigner.Wpf.Toolbox;

using Microsoft.Win32;

namespace CustomizeToolbox
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private const string XamlExtension = ".xaml";

        private MainWindow window;
        private string fileName = string.Empty;
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };
        private SaveFileDialog saveFileDialog = new SaveFileDialog { };

        #endregion

        public ViewModel()
        {
            AddLibraryCommand = new RelayCommand(AddLibraryClick);
            CustomizeCommand = new RelayCommand(CustomizeClick);
            SaveCommand = new RelayCommand(SaveToolboxClick);
            LoadCommand = new RelayCommand(LoadToolboxClick);
            openFileDialog.Filter = "ToolBox content files (*.xml) |*.xml";
            openFileDialog.FileName = "toolBoxContent";
            openFileDialog.InitialDirectory = Path.GetFullPath(baseDirectory);
            saveFileDialog.Filter = "ToolBox content files (*.xml) |*.xml";
            saveFileDialog.FileName = "toolBoxContent";
            saveFileDialog.InitialDirectory = Path.GetFullPath(baseDirectory);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            window.toolboxControl.FormDesigner = window.formDesignerControl;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddLibraryCommand { get; set; }

        public ICommand CustomizeCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Private Methods

        private void AddLibraryClick()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = ".NET assembly files (*.dll)|*.dll|All files (*.*)|*.*";
            dialog.InitialDirectory = baseDirectory;
            if (!dialog.ShowDialog().Value)
                return;
            AddAssembly(dialog.FileName, "Custom Controls");
        }

        private void CustomizeClick()
        {
            window.toolboxControl.BeginUpdate();
            try
            {
                RemoveCategory("Common WPF Controls");
                RemoveCategory("All WPF Controls");
                RemoveCategory("Text WPF Controls");
                RemoveCategory("Advanced WPF Controls");

                FillWithCustomItems(true);
            }
            finally
            {
                window.toolboxControl.EndUpdate();
            }
        }

        private void SaveToolboxClick()
        {
            if (saveFileDialog.ShowDialog().Value)
            {
                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                try
                {
                    window.toolboxControl.Save(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void LoadToolboxClick()
        {
            if (openFileDialog.ShowDialog().Value)
            {
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                try
                {
                    window.toolboxControl.Load(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void RemoveCategory(string categoryName)
        {
            if (window.toolboxControl.DoesCategoryExist(categoryName))
                window.toolboxControl.RemoveCategory(categoryName);
        }

        private void FillWithCustomItems(bool useMetroIcons)
        {
            window.toolboxControl.BeginUpdate();
            try
            {
                foreach (var pair in ToolboxCustomItems.TypesByCategoryName)
                {
                    foreach (var type in pair.Value)
                    {
                        ImageSource customImage = null;
                        customImage = TryLoadImageForStandardItem(type.Name);

                        window.toolboxControl.AddItemForType(pair.Key, type, customImage);
                    }
                }
            }
            finally
            {
                window.toolboxControl.EndUpdate();
            }
        }

        private ImageSource TryLoadImageForStandardItem(string typeName)
        {
            var stream = typeof(ToolboxControl).Assembly.GetManifestResourceStream(
                string.Format(
                    "Alternet.FormDesigner.Wpf.Images.Toolbox.StandardItems.{0}.png",
                    typeName));

            if (stream == null)
                return null;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void AddAssembly(string asmName, string tabName)
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(asmName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load assembly: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                window.toolboxControl.AddItemsFromAssembly(tabName, assembly);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add toolbox items: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #endregion

        private static class ToolboxCustomItems
        {
            public static readonly Dictionary<string, List<Type>> TypesByCategoryName = new Dictionary<string, List<Type>>
            {
                {
                    "Common WPF Controls",
                    new List<Type>(new[]
                    {
                        typeof(Button),
                        typeof(CheckBox),
                        typeof(RadioButton),
                    })
                },
                {
                    "Text WPF Controls",
                    new List<Type>(new[]
                    {
                        typeof(Label),
                        typeof(TextBlock),
                        typeof(TextBox),
                    })
                },
                {
                    "Advanced WPF Controls",
                    new List<Type>(new[]
                    {
                        typeof(Border),
                        typeof(Calendar),
                        typeof(Canvas),
                        typeof(ComboBox),
                        typeof(ContentControl),
                        typeof(DataGrid),
                        typeof(DatePicker),
                        typeof(DockPanel),
                        typeof(DocumentViewer),
                        typeof(Expander),
                        typeof(Frame),
                        typeof(Grid),
                        typeof(GridSplitter),
                        typeof(GroupBox),
                        typeof(Image),
                        typeof(ListBox),
                        typeof(ListView),
                        typeof(MediaElement),
                        typeof(Menu),
                        typeof(PasswordBox),
                        typeof(ProgressBar),
                        typeof(RichTextBox),
                        typeof(ScrollBar),
                        typeof(ScrollViewer),
                        typeof(Separator),
                        typeof(Slider),
                        typeof(StackPanel),
                        typeof(StatusBar),
                        typeof(TabControl),
                        typeof(ToolBar),
                        typeof(ToolBarPanel),
                        typeof(ToolBarTray),
                        typeof(TreeView),
                        typeof(Viewbox),
                        typeof(WebBrowser),
                        typeof(WrapPanel),
                    })
                },
            };
        }
    }
}