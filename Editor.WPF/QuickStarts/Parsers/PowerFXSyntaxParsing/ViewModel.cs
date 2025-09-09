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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Advanced;
using Alternet.Syntax.Parsers.PowerFx;
using Microsoft.Win32;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace PowerFxSyntaxParsing
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly JSONParser jsonParser = new JSONParser();
        private readonly PowerFxParser fxParser = new PowerFxParser();
        private string dir = AppDomain.CurrentDomain.BaseDirectory;
        private TextEditor syntaxEdit1;
        private TextEditor syntaxEdit2;
        private Window window;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };
        private TextSource source = new TextSource();
        private bool evaluateQuickInfo = true;

        public ViewModel()
        {
        }

        public ViewModel(Window window, TextEditor edit1, TextEditor edit2)
        {
            this.syntaxEdit1 = edit1;
            this.syntaxEdit2 = edit2;
            this.window = window;
            syntaxEdit1.Source = this.source;
            fxParser.EvaluateQuickInfo = this.evaluateQuickInfo;
            InitEditor();

            const string FxExpressionsFolder = @"Resources\Editor\Text";

            syntaxEdit2.ReadOnly = false;

            openFileDialog.Filter = "PowerFx files (*.fx)|*.fx|All files (*.*)|*.*";

            var path = Path.GetFullPath(dir) + FxExpressionsFolder;

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\" + FxExpressionsFolder;
            }

            FileInfo fileInfo = new FileInfo(path + @"\Record.fx");
            if (fileInfo.Exists)
            {
                source.FileName = fileInfo.FullName;
                source.LoadFile(fileInfo.FullName);
            }

            fileInfo = new FileInfo(path + @"\Record.fx.json");

            if (fileInfo.Exists && syntaxEdit2.LoadFile(fileInfo.FullName))
            {
                ApplyGlobalContext(syntaxEdit2.Text);
            }
            else
            {
                syntaxEdit2.Text = $"Error loading: {fileInfo.Name}";
            }

            openFileDialog.InitialDirectory = Path.GetFullPath(path);

            source.HighlightReferences = true;

            syntaxEdit2.TextChanged += SyntaxEdit2_TextChanged;

            LoadCommand = new RelayCommand(LoadClick);
            EvaluateCommand = new RelayCommand(EvaluateClick);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCommand { get; set; }

        public ICommand EvaluateCommand { get; set; }

        public bool EvaluateQuickInfo
        {
            get
            {
                return evaluateQuickInfo;
            }

            set
            {
                if (evaluateQuickInfo != value)
                {
                    evaluateQuickInfo = value;
                    OnPropertyChanged("EvaluateQuickInfo");
                    if (fxParser != null)
                        fxParser.EvaluateQuickInfo = evaluateQuickInfo;
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SyntaxEdit2_TextChanged(object sender, EventArgs e)
        {
            ApplyGlobalContext(syntaxEdit2.Text);
        }

        private bool ApplyGlobalContext(string s)
        {
            if (!PowerFxParser.IsValidJson(s))
                return false;

            if (string.IsNullOrEmpty(s))
            {
                fxParser.GlobalContextAsJson = null;
            }
            else
                fxParser.GlobalContextAsJson = s;
            return true;
        }

        private void LoadClick()
        {
            if (openFileDialog.ShowDialog().Value)
            {
                syntaxEdit1.Source.LoadFile(openFileDialog.FileName);
                syntaxEdit1.Source.FileName = openFileDialog.FileName;
            }
        }

        private void EvaluateClick()
        {
            var r = fxParser.Evaluator.Eval(syntaxEdit1.Text);
            var s = PowerFxEvaluator.EvalResultToString(r);
            MessageBox.Show(s, "Evaluation Result");
        }

        private void InitEditor()
        {
            source.Lexer = fxParser;
            syntaxEdit2.Source.FileName = "test.js";
            syntaxEdit2.Lexer = jsonParser;
            syntaxEdit1.Spelling.SpellColor = Color.Navy;
            syntaxEdit1.AllowOutlining = true;
        }
    }
}
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates