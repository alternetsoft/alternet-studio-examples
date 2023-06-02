#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using Alternet.Common.Python;
using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Python;

using Microsoft.Win32;

namespace PythonParser
{
    public class ViewModel
    {
        private IronPythonParser ironPythonParser = new IronPythonParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };

        public ViewModel()
        {
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;
            IList<string> paths = new List<string>();

            edit.Lexer = ironPythonParser;
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
                if (!Directory.Exists(dir))
                    dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\Script.py");
            if (fileInfo.Exists)
            {
                edit.Source.LoadFile(fileInfo.FullName);
                paths.Add(fileInfo.DirectoryName);
            }

            var imports = new List<string>();

            imports.Add("System");
            imports.Add("System.Drawing");
            imports.Add("System.Windows.Forms");
            var codeEnvironment = new CodeEnvironment(null, paths, imports, null, Alternet.Common.TechnologyEnvironment.WindowsForms);

            ironPythonParser.CodeEnvironment = codeEnvironment;
            LoadCommand = new RelayCommand(LoadClick);
            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";
            openFileDialog.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";
        }

        public ICommand LoadCommand { get; set; }

        private void LoadClick()
        {
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog().Value)
            {
                edit.Source.LoadFile(openFileDialog.FileName);
            }
        }
    }
}