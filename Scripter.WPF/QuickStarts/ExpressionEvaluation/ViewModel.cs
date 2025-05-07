#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

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
using Alternet.Scripter;

namespace ExpressionEvaluation
{
    public class ViewModel
    {
        private const string ExpressionCSharp = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
        private const string ExpressionVisualBasic = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
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

            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            if (window != null)
            {
                ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", typeof(System.Windows.Controls.TextBox), window.Expression);
                scriptRun.GlobalItems.Clear();
                scriptRun.GlobalItems.Add(item);
            }

            languages.Add("C#");
            languages.Add("VB");
            Language = "C#";
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
                    scriptRun.ScriptSource.References.Add("PresentationCore");
                    scriptRun.ScriptSource.References.Add("WindowsBase");
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
                case "C#":
                    window.Expression.Text = ExpressionCSharp;
                    scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
                    break;
                default:
                    window.Expression.Text = ExpressionVisualBasic;
                    scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
                    break;
            }
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}