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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

using Alternet.Common;
using Alternet.Common.TypeScript.Types;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.TypeScript;
using Microsoft.Win32;

namespace TypeScriptParserDemo
{
    public enum TypeScriptLexToken
    {
        TypeName = 12,
        Warning,
        XmlParams,
        MethodName,
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private TextEditor edit;
        private OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = false };
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private TypeScriptParserWithSemantic typeScriptParser = new TypeScriptParserWithSemantic();
        private JavaScriptParserWithSemantic javaScriptParser = new JavaScriptParserWithSemantic();
        private bool customHighlight;

        public ViewModel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\";
            }

            LoadCommand = new RelayCommand(LoadClick);
            openFileDialog.Filter = "TypeScript files (*.ts)|*.ts|Js # files (*.js)|*.js";
            openFileDialog.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\Text\";
        }

        public ViewModel(TextEditor edit)
            : this()
        {
            this.edit = edit;

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\typescript.ts");
            if (edit != null && fileInfo.Exists)
            {
                edit.Source.FileName = fileInfo.Name;
                edit.Source.LoadFile(fileInfo.FullName);
                edit.Spelling.SpellColor = System.Drawing.Color.Navy;
                edit.HighlightReferences = true;
            }

            typeScriptParser.RegisterDefaultAssemblies(TechnologyEnvironment.WindowsForms);
            javaScriptParser.RegisterDefaultAssemblies(TechnologyEnvironment.WindowsForms);

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";
            CustomHighlight = false;
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

        public bool CustomHighlight
        {
            get
            {
                return customHighlight;
            }

            set
            {
                if (customHighlight != value)
                {
                    customHighlight = value;
                    OnPropertyChanged("CustomHighlight");
                    UpdateCustomHighlight();
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateCustomHighlight()
        {
            typeScriptParser.SemanticHighlighting = CustomHighlight;
            javaScriptParser.SemanticHighlighting = CustomHighlight;
            var parser = edit.Lexer as TypeScriptParser;
            if (parser != null)
                parser.ReparseText();
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath)
        {
            sourceFileSubPath = @"TypeScript.ts";
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath)
        {
            sourceFileSubPath = @"JavaScript.js";
        }

        private void UpdateSource()
        {
            string sourceFileSubPath;
            switch (lang)
            {
                case "TypeScript":
                    GetSourceParametersForTS(out sourceFileSubPath);
                    edit.Lexer = typeScriptParser;
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath);
                    edit.Lexer = javaScriptParser;
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Editor\Text\";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void LoadFile(TextEditor edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.Source.LoadFile(fileName);
            edit.Source.FileName = fileName;
        }

        private void LoadClick()
        {
            switch (Language)
            {
                case "TypeScript":
                    openFileDialog.FilterIndex = 1;
                    break;
                case "JavaScript":
                    openFileDialog.FilterIndex = 2;
                    break;
                default:
                    openFileDialog.FilterIndex = 1;
                    break;
            }

            if (edit != null && openFileDialog.ShowDialog().Value)
            {
                LoadFile(edit, openFileDialog.FileName);
            }
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class TypeScriptParserWithSemantic : TypeScriptParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        private bool semanticHighlighting;

        public bool SemanticHighlighting
        {
            get
            {
                return semanticHighlighting;
            }

            set
            {
                if (semanticHighlighting != value)
                {
                    semanticHighlighting = value;
                    OnSemanticHighlightingPropertyChanged();
                }
            }
        }

        protected virtual void OnSemanticHighlightingPropertyChanged()
        {
            var tokenizer = Tokenizer as TypeScriptTokenizerWithSemantic;
            if (tokenizer != null)
                tokenizer.SemanticHighlighting = semanticHighlighting;
        }

        protected override TypeScriptTokenizer CreateTokenizer()
        {
            return new TypeScriptTokenizerWithSemantic();
        }

        protected override void InitDefaultStyles()
        {
            base.InitDefaultStyles();
            AddStyle(TypeScriptConsts.MethodInternalName, TypeScriptConsts.DefaultMethodForeColor);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class JavaScriptParserWithSemantic : JavaScriptParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        private bool semanticHighlighting;

        public bool SemanticHighlighting
        {
            get
            {
                return semanticHighlighting;
            }

            set
            {
                if (semanticHighlighting != value)
                {
                    semanticHighlighting = value;
                    OnSemanticHighlightingPropertyChanged();
                }
            }
        }

        protected virtual void OnSemanticHighlightingPropertyChanged()
        {
            var tokenizer = Tokenizer as TypeScriptTokenizerWithSemantic;
            if (tokenizer != null)
                tokenizer.SemanticHighlighting = semanticHighlighting;
        }

        protected override TypeScriptTokenizer CreateTokenizer()
        {
            return new TypeScriptTokenizerWithSemantic();
        }

        protected override void InitDefaultStyles()
        {
            base.InitDefaultStyles();
            AddStyle(TypeScriptConsts.MethodInternalName, TypeScriptConsts.DefaultMethodForeColor);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class TypeScriptTokenizerWithSemantic : TypeScriptTokenizer
#pragma warning restore SA1402 // File may only contain a single type
    {
        public bool SemanticHighlighting { get; set; }

        protected override int GetSyntaxToken(ClassifiedSpan span)
        {
            if (SemanticHighlighting)
            {
                switch (span.ClassificationType.Value)
                {
                    case "identifier":
                        var symbol = Repository.Parser.GetQuickInfoAtPosition(Repository.FileName, span.Span.Start);
                        if (symbol != null)
                        {
                            switch (symbol.Kind.Value)
                            {
                                case "function":
                                case "method":
                                    return (int)TypeScriptLexToken.MethodName;
                                case "class":
                                    return (int)TypeScriptLexToken.TypeName;
                            }
                        }

                        return (int)LexToken.Identifier;
                    default:
                        return base.GetSyntaxToken(span);
                }
            }
            else
                return base.GetSyntaxToken(span);
        }
    }
}