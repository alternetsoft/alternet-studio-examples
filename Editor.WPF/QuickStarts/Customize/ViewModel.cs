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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace Customize
{
    public class ViewModel : INotifyPropertyChanged
    {
        private TextSource csharpSource = new TextSource();
        private CsParser csParser1 = new CsParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private SyntaxSettings globalSettings = new SyntaxSettings();
        private TextEditor edit;
        private MainWindow window;

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\c#.cs");
            if (fileInfo.Exists)
                csharpSource.LoadFile(fileInfo.FullName);
            csharpSource.Lexer = csParser1;

            CustomizeCommand = new RelayCommand(CustomizeClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            window.syntaxEdit1.Source = csharpSource;
            this.window = window;
            window.Closed += Window_Closed;
            this.edit = window.syntaxEdit1;
            edit.HighlightReferences = true;
            globalSettings.LoadFromEdit(edit);
            string applicationGlobalSettingsFilePath = GetApplicationGlobalSettingsFilePath();
            if (File.Exists(applicationGlobalSettingsFilePath))
            {
                globalSettings.LoadFile(applicationGlobalSettingsFilePath);
                globalSettings.ApplyToEdit(edit);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CustomizeCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void CustomizeClick()
        {
            DlgSyntaxSettings options = new DlgSyntaxSettings();
            options.SyntaxSettings.Assign(globalSettings);
            if (options.ShowDialog().Value)
            {
                globalSettings.Assign(options.SyntaxSettings);
                globalSettings.ApplyToEdit(edit);
            }

            options = null;
        }

        private string GetApplicationGlobalSettingsFilePath()
        {
            const string FolderName = "Alternet.Editor.Wpf.Demo.GlobalSettings";
            const string FileName = "GlobalSettings.xml";

            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string p = Path.Combine(applicationDataPath, FolderName);

            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }

            return Path.Combine(p, FileName);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            globalSettings.SaveFile(GetApplicationGlobalSettingsFilePath());
        }
    }
}
