#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

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
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Common;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Scripter.TypeScript;

namespace ExpressionEvaluation.TypeScript
{
    public class ViewModel
    {
        private const string ExpressionTS = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.length";
        private const string ExpressionJS = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.length";
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            if (window != null)
            {
                scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(TechnologyEnvironment.Wpf)
                   .AddObject("tbExpression", window.Expression);
            }

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return lang;
            }

            set
            {
                if (lang != value)
                {
                    lang = value;
                    OnPropertyChanged("Language");
                    UpdateSource();
                }
            }
        }

        public ICommand RunScript { get; set; }

        public void StartScript()
        {
            if (window != null)
            {
                window.Dispatcher.BeginInvoke((Action)(() =>
                {
                    object obj = scriptRun.EvaluateExpression(window.Expression.Text);
                    if (obj != null)
                        MessageBox.Show(obj.ToString());
                }));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateSource()
        {
            switch (lang)
            {
                case "TypeScript":
                    window.Expression.Text = ExpressionTS;
                    scriptRun.ScriptLanguage = ScriptLanguage.TypeScript;
                    break;
                default:
                    window.Expression.Text = ExpressionJS;
                    scriptRun.ScriptLanguage = ScriptLanguage.JavaScript;
                    break;
            }
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}