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
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Alternet.Common.Python;
using Alternet.Common.Wpf;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace ExpressionEvaluation
{
    public class ViewModel
    {
        private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + len(tbExpression.Text)";
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";

        public ViewModel()
        {
            SetupPython();
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

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"AlterNETStudio\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython();
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}