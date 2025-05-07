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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.TypeScript.Types;
using Alternet.Editor;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.TypeScript;

namespace TypeScriptParserDemo
{
    public enum TypeScriptLexToken
    {
        TypeName = 12,
        Warning,
        XmlParams,
        MethodName,
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Info classes in the same unit")]
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string LoadDesc = "Load code file";
        private string dir = Application.StartupPath + @"\";
        private TypeScriptParserWithSemantic typeScriptParser = new TypeScriptParserWithSemantic();
        private JavaScriptParserWithSemantic javaScriptParser = new JavaScriptParserWithSemantic();

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "TypeScriptParserDemo.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            syntaxEdit1.Spelling.SpellColor = Color.Navy;
            syntaxEdit1.HighlightReferences = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TypeScript files (*.ts)|*.ts|Js # files (*.js)|*.js";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\text\typescript.ts");
            if (fileInfo.Exists)
                syntaxEdit1.LoadFile(fileInfo.FullName);

            syntaxEdit1.Source.FileName = fileInfo.Name;
            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\text\";

            typeScriptParser.RegisterDefaultAssemblies(TechnologyEnvironment.WindowsForms);
            javaScriptParser.RegisterDefaultAssemblies(TechnologyEnvironment.WindowsForms);

            syntaxEdit1.HighlightReferences = true;
            cbLanguages.SelectedIndex = 0;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = Math.Max(1, cbLanguages.SelectedIndex + 1);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syntaxEdit1.Source.FileName = openFileDialog1.FileName;
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
            }
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            switch (index)
            {
                case 0:
                    GetSourceParametersForTS(out sourceFileSubPath);
                    syntaxEdit1.Lexer = typeScriptParser;
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath);
                    syntaxEdit1.Lexer = javaScriptParser;
                    break;
            }

            LoadFile(syntaxEdit1, GetSourceFileFullPath(sourceFileSubPath));
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath)
        {
            sourceFileSubPath = @"TypeScript.ts";
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath)
        {
            sourceFileSubPath = @"JavaScript.js";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Editor\Text\";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void LoadFile(SyntaxEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.Source.FileName = fileName;
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }

        private void CustomHighlightingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            typeScriptParser.SemanticHighlighting = customHighlightingCheckBox.Checked;
            javaScriptParser.SemanticHighlighting = customHighlightingCheckBox.Checked;

            var parser = syntaxEdit1.Lexer as TypeScriptParser;
            if (parser != null)
                parser.ReparseText();
        }
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
