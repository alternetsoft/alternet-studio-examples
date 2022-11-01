#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using Alternet.FormDesigner.Wpf;
using Microsoft.Win32;

namespace LoadAndSave
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private const string XamlExtension = ".xaml";

        private MainWindow window;
        private FormDesignerDataSource source = null;
        private string fileName = string.Empty;
        private bool modified = false;

        #endregion

        public ViewModel()
        {
            LoadCommand = new RelayCommand(LoadClick);
            SaveCommand = new RelayCommand(SaveClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            string folder = GetTestFilesDirectoryPath();
            if (Directory.Exists(folder))
            {
                var xamlFiles = Directory.GetFiles(folder, "*" + XamlExtension, SearchOption.AllDirectories);

                var result = new List<FormDesignerDataSource>();

                foreach (var xamlFile in xamlFiles)
                {
                    OpenAllFormFiles(xamlFile);
                    UpdateFormTitle();
                    break;
                }
            }

            window.formDesignerControl.SelectionChanged += FormDesignerControl_SelectionChanged;
            window.formDesignerControl.DesignedContentChanged += OnDesignedContentChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Private Methods

        private void OnDesignedContentChanged(object sender, EventArgs e)
        {
            modified = true;
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            string title = modified ? string.Format("Form Designer - {0} *", fileName) : string.Format("Form Designer - {0}", fileName);
            window.Title = title;
        }

        private void LoadClick()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XAML Files|*.xaml|All Files|*.*",
                Multiselect = false,
                InitialDirectory = GetTestFilesDirectoryPath(),
            };

            if (!dialog.ShowDialog().Value)
                return;

            var xamlFileName = dialog.FileName;

            if (!xamlFileName.EndsWith(XamlExtension, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The file must have an extension .xaml");
                return;
            }

            OpenAllFormFiles(xamlFileName);
        }

        private void SaveClick()
        {
            SaveDesignerFiles(window.formDesignerControl);
        }

        private void OpenAllFormFiles(string xamlFileName)
        {
            source = GetDesignerSource(xamlFileName);

            OpenDesigner(xamlFileName);
        }

        private void OpenDesigner(string fileName)
        {
            this.fileName = Path.GetFileName(fileName);
            window.formDesignerControl.Source = source;
        }

        private void FormDesignerControl_SelectionChanged(object sender, EventArgs e)
        {
            var designer = (FormDesignerControl)sender;

            window.propertyGrid.SelectedItems = designer.SelectedItems.ToArray();
        }

        private FormDesignerDataSource GetDesignerSource(string xamlFileName)
        {
            FormDesignerDataSource ds;
            var language = FormFilesUtility.FindUserCodeFile(xamlFileName);

            ds = new FormDesignerDataSource(
                    xamlFileName,
                    language);

            return ds;
        }

        private string GetTestFilesDirectoryPath()
        {
            const string Subdirectory = @"Resources\Designer\Wpf";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var directory = Path.Combine(baseDirectory, Subdirectory);

            if (!Directory.Exists(directory))
                directory = Path.Combine(Path.GetFullPath(baseDirectory.TrimEnd('\\') + @"\..\..\..\..\..\..\"), Subdirectory);

            return directory;
        }

        private void SaveDesignerFiles(IFormDesignerControl designer)
        {
            designer.Save();
            modified = false;
            UpdateFormTitle();
        }

        #endregion
    }
}