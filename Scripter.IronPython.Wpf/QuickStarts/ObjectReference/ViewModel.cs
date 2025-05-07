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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Alternet.Common.DotNet;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Editor.Common.Wpf;
using Alternet.Scripter.IronPython;
using Alternet.Syntax.Parsers.Python;

namespace ObjectReference
{
    public class ViewModel
    {
        private bool scriptRunning;
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private IDisposable scriptObject;
        private IDefaultAssembliesProvider defaultAssembliesProvider = DefaultAssembliesProviderFactory.CreateDefaultAssembliesProvider();
        private IronPythonParser pythonParser1 = new IronPythonParser();
        private DispatcherTimer timer = new DispatcherTimer();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
            StopScript = new RelayCommand(StopScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            CreateEditor(window.EditBorder);

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.Wpf;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Windows");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunScript { get; set; }

        public ICommand StopScript { get; set; }

        public void StartScript()
        {
            if (window != null)
            {
                window.Dispatcher.BeginInvoke((Action)(() =>
                {
                    scriptRun.ScriptSource.FromScriptCode(edit.Text);

                    if (!scriptRun.Compiled)
                    {
                        if (!scriptRun.Compile())
                        {
                            MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                            return;
                        }
                    }

                    scriptObject = scriptRun.Run() as IDisposable;
                    scriptRun.RunFunction("Main", new object[] { "Catch me if you can" });
                    scriptRunning = true;
                    window.ScriptButton.Content = "Stop Script";
                }));
            }
        }

        public void DoStopScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                if (window != null)
                {
                    if (scriptObject != null)
                        scriptObject.Dispose();
                    scriptObject = null;
                    window.ScriptButton.Content = "Run Script";
                    window.TestButton.Content = "Test Button";
                }

                scriptRunning = false;
            }));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "ObjectReferenceWpf.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.IronPython";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            var edit = new ScriptCodeEdit();
            edit.FileName = fileName;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);
            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
            edit.Lexer = pythonParser1;

            AddScriptItem();

            return edit;
        }

        private void AddScriptItem()
        {
            if (window != null)
            {
                ScriptGlobalItem item = new ScriptGlobalItem("RunButton", window.TestButton);
                ScriptGlobalItem item1 = new ScriptGlobalItem("timer", timer);
                scriptRun.GlobalItems.Clear();
                scriptRun.GlobalItems.Add(item);
                scriptRun.GlobalItems.Add(item1);
            }
        }

        private void RunScriptClick()
        {
            if (!scriptRunning)
                StartScript();
            else
                DoStopScript();
        }

        private void StopScriptClick()
        {
            DoStopScript();
        }
    }
}