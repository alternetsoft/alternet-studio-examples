#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Common;
using Alternet.Common.TypeScript.Types;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Syntax;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.TypeScript;
using Alternet.UI;
using System;
using System.ComponentModel;
using System.IO;

namespace TypeScriptParsing
{
    public enum TypeScriptLexToken
    {
        TypeName = 12,
        Warning,
        XmlParams,
        MethodName,
    }

    public partial class Form1 : Window
    {
        private readonly TextSource typeScriptSource = new();
        private readonly TextSource javaScriptSource = new();
        private readonly OpenFileDialog openFileDialog1 = new();

        private TypeScriptParserWithSemantic typeScriptParser = new TypeScriptParserWithSemantic();
        private JavaScriptParserWithSemantic javaScriptParser = new JavaScriptParserWithSemantic();


        public Form1()
        {
            InitializeComponent();

            DemoUtils.InitCommonEditorProps(syntaxEdit1);

            typeScriptParser.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                | SyntaxOptions.SyntaxErrors;

            javaScriptParser.Options |= SyntaxOptions.CodeCompletion | SyntaxOptions.QuickInfoTips
                | SyntaxOptions.SyntaxErrors;

            if (CommandLineArgs.ParseAndGetIsDark())
                syntaxEdit1.VisualThemeType = VisualThemeType.Dark;

            cbLanguages.AddRange(new[]
            {
                "TypeScript",
                "JavaScript"
            });

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;
            customHighlightingCheckBox.CheckedChanged += CustomHighlightingCheckBox_CheckedChanged;
            btLoad.Click += LoadButton_Click;

            syntaxEdit1.Source = typeScriptSource;
            syntaxEdit1.Outlining.AllowOutlining = true;

            typeScriptSource.OptimizedForMemory = false;
            javaScriptSource.OptimizedForMemory = false;

            Form1_Load(this, EventArgs.Empty);

            cbLanguages.Value = "TypeScript";

            lbDescription.WordWrap = true;
            ActiveControl = syntaxEdit1;
        }

        private void CustomHighlightingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            App.AddIdleTask(() =>
            {
                typeScriptParser.SemanticHighlighting = customHighlightingCheckBox.Checked;
                javaScriptParser.SemanticHighlighting = customHighlightingCheckBox.Checked;

                var parser = syntaxEdit1.Lexer as TypeScriptParser;
                if (parser != null)
                    parser.ReparseText();
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TypeScript files (*.ts)|*.ts|Js # files (*.js)|*.js";
            DirectoryInfo dirInfo = new(DemoUtils.GetResourceFolderFullPath(@"Editor/Text/"));

            FileInfo fileInfo = new(Path.Combine(dirInfo.FullName, "TypeScript.ts"));
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);

            fileInfo = new FileInfo(Path.Combine(dirInfo.FullName, "JavaScript.js"));
            if (fileInfo.Exists)
                javaScriptSource.LoadFile(fileInfo.FullName);

            openFileDialog1.InitialDirectory = dirInfo.FullName;

            typeScriptSource.Lexer = typeScriptParser;
            javaScriptSource.Lexer = javaScriptParser;
            typeScriptSource.HighlightReferences = true;
            javaScriptSource.HighlightReferences = true;
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            syntaxEdit1.Source = cbLanguages.Value switch
            {
                "TypeScript" => typeScriptSource,
                "JavaScript" => javaScriptSource,
                _ => typeScriptSource,
            };
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = cbLanguages.Value?.ToString() == "TypeScript" ? 0 : 1;

            openFileDialog1.ShowAsync(() =>
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
        public class TypeScriptParserWithSemantic : TypeScriptParser
        {
            private bool semanticHighlighting;

            [DefaultValue(false)]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
        public class JavaScriptParserWithSemantic : JavaScriptParser
        {
            private bool semanticHighlighting;

            [DefaultValue(false)]
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
                        OnSemanticHighlightingChanged();
                    }
                }
            }

            protected virtual void OnSemanticHighlightingChanged()
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
        public class TypeScriptTokenizerWithSemantic : TypeScriptTokenizer
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
                    }
                }

                return base.GetSyntaxToken(span);
            }
        }
    }
}
