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
using Alternet.Scripter.IronPython;

namespace ExpressionEvaluation
{
    public class ViewModel
    {
        private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", window.Expression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            window.Expression.Text = ExpressionPython;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}