﻿#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Alternet.Common.DotNet;
using Alternet.Editor.Common.Wpf;
using Alternet.Scripter.IronPython;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.Python;

namespace CustomAssembly
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private IScriptEdit edit;
        private IScriptEdit editExt;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private IronPythonParser pythonParser1 = new IronPythonParser();
        private CsParser csParser1 = new CsParser();

        public ViewModel()
        {
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;

            CreateEditor(window.EditBorder, window.ExternalBorder);

            scriptRun.ScriptSource.ReferencesSearchPaths.Add(Path.GetDirectoryName(GetType().Assembly.Location));
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.Wpf | Framework.WindowsForms;
            scriptRun.ScriptSource.References.Add("ExternalAssemblyWpf.dll");
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Controls");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Media");
            scriptRun.ScriptSource.Imports.Add("ExternalAssembly");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunScript { get; set; }

        public void StartScript()
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

                scriptRun.RunFunction("Main", new object[] { });
            }));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath, out string sourceExternalSubPath)
        {
            sourceFileSubPath = "CustomAssemblyTestWpf.py";
            sourceExternalSubPath = "CustomClassWpf.cs";
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

        private void CreateEditor(Border border, Border extBorder)
        {
            string sourceFileSubPath;
            string sourceExternalSubPath;

            GetSourceParametersForPython(out sourceFileSubPath, out sourceExternalSubPath);

            edit = CreateEditor(GetSourceFileFullPath(sourceFileSubPath), border, false);
            editExt = CreateEditor(GetSourceFileFullPath(sourceExternalSubPath), extBorder, true);
        }

        private IScriptEdit CreateEditor(string fileName, Border border, bool readOnly)
        {
            var edit = new ScriptCodeEdit();
            edit.FileName = fileName;

            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);
            edit.ReadOnly = readOnly;
            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;

            if (readOnly)
                edit.Lexer = csParser1;
            else
                edit.Lexer = pythonParser1;

            return edit;
        }

        private void RunScriptClick()
        {
            StartScript();
        }
    }
}