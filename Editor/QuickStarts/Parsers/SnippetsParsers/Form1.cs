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
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor.TextSource;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.CodeAnalysis;

namespace SnippetsParsers
{
    public partial class Form1 : Form
    {
        private const string ReadonlyCodeDescription = "Code in the editor is a full source code, but user can only change editable section.";
        private const string HiddenCodeDescription = "Code in the editor is a full source code, but user can only change visible section.";
        private const string PartialCodeDescription = "Code in this editor is a part of the full source code (in the top and bottom editors). User can change only that part and get full intellisense functionality.";
        private const string ClassLessCodeDescription = "Code in the editor is a full source code, and associated parser allows class-less scripts, i.e global code without class or method declaration.";

        private bool csharp = true;
        private bool isClass = false;
        private ReadType currentType = ReadType.Partial;

        private VBMethodParser vbMethodParser = new VBMethodParser();
        private VBClassParser vbClassParser = new VBClassParser(new VbSolution());
        private VbParser vbParser = new VbParser(new VbSolution());

        private CSMethodParser csMethodParser = new CSMethodParser();
        private CSClassParser csClassParser = new CSClassParser(new CsSolution());
        private CsParser csClassLessParser = new CsParser(CsSolution.DefaultScriptSolution);
        private CsParser csParser = new CsParser(new CsSolution());
        private int readOnlyBefore = 0;
        private int readOnlyAfter = 0;
        private bool sourceUpdating = false;

        public Form1()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "SnippetsParsers.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        [Flags]
        internal enum ReadType
        {
            Partial,
            Readonly,
            Hidden,
            ClassLess,
        }

        [DefaultValue(true)]
        public bool CSharp
        {
            get
            {
                return csharp;
            }

            set
            {
                if (csharp != value)
                {
                    csharp = value;
                    UpdateSource();
                }
            }
        }

        [DefaultValue(false)]
        public bool IsClass
        {
            get
            {
                return isClass;
            }

            set
            {
                if (isClass != value)
                {
                    isClass = value;
                    UpdateSource();
                }
            }
        }

        [DefaultValue(ReadType.Partial)]
        internal ReadType CurrentType
        {
            get
            {
                return currentType;
            }

            set
            {
                if (currentType != value)
                {
                    currentType = value;
                    UpdateSource();
                }
            }
        }

        private void UpdatePartial()
        {
            panel1.Visible = currentType == ReadType.Partial;
            panel3.Visible = currentType == ReadType.Partial;
            seTop.Visible = currentType == ReadType.Partial;
            seBottom.Visible = currentType == ReadType.Partial;
            if (csharp)
                syntaxEdit1.Source = currentType == ReadType.Readonly || currentType == ReadType.Hidden ? csSource : csSnippetSource;
            else
                syntaxEdit1.Source = currentType == ReadType.Readonly || currentType == ReadType.Hidden ? vbSource : vbSnippetSource;

            var parser = syntaxEdit1.Lexer as RoslynParser;
            if (parser != null)
            {
                if (currentType == ReadType.Hidden)
                    parser.Options &= ~(SyntaxOptions.SmartIndent | SyntaxOptions.FormatSpaces | SyntaxOptions.FormatCase);
                else
                    parser.Options |= SyntaxOptions.SmartIndent | SyntaxOptions.FormatSpaces | SyntaxOptions.FormatCase;

                parser.ReparseText();
            }

            switch (currentType)
            {
                case ReadType.Partial:
                    label3.Text = PartialCodeDescription;
                    break;
                case ReadType.Readonly:
                    label3.Text = ReadonlyCodeDescription;
                    break;
                case ReadType.Hidden:
                    label3.Text = HiddenCodeDescription;
                    break;
                case ReadType.ClassLess:
                    label3.Text = ClassLessCodeDescription;
                    break;
            }

            for (int i = 0; i < syntaxEdit1.Source.Lines.Count; i++)
            {
                syntaxEdit1.Source.SetLineReadonly(i, currentType == ReadType.Readonly && ((i <= readOnlyBefore) || (i >= readOnlyAfter)));
                syntaxEdit1.Source.SetLineHidden(i, currentType == ReadType.Hidden && ((i <= readOnlyBefore) || (i >= readOnlyAfter)));
            }
        }

        private void UpdateSource()
        {
            if (sourceUpdating)
                return;

            sourceUpdating = true;
            try
            {
                rbClassLess.Enabled = csharp;
                if (csharp)
                    UpdateCSSource();
                else
                    UpdateVBSource();
                UpdatePartial();
                syntaxEdit1.Position = System.Drawing.Point.Empty;
                syntaxEdit1.MakeVisible(syntaxEdit1.Position);
            }
            finally
            {
                sourceUpdating = false;
            }
        }

        private void InsertMethodBody(ITextSource source, string text)
        {
            string[] lines = StringItem.Split(text);
            string indent = isClass ? "\t" : "\t\t";
            for (int i = 0; i < lines.Length; i++)
            {
                string s = string.Empty;
                if ((currentType == ReadType.ClassLess) && (lines[i] != string.Empty))
                    s = isClass ? "\t" + lines[i] : lines[i];
                else
                    s = (currentType != ReadType.Readonly) || (lines[i] == string.Empty) ? lines[i] : indent + lines[i];
                source.Lines.Add(s);
            }
        }

        private void UpdateVBSource()
        {
            string text = string.Empty;
            switch (currentType)
            {
                case ReadType.ClassLess:
                    text = isClass ? VBSnippet.ClassLessSnippet : VBSnippet.MethodLessSnippet;
                    rbPartial.Checked = true;
                    break;
                default:
                    text = isClass ? vbClassParser.Snippet : vbMethodParser.Snippet;
                    break;
            }

            string[] lines = StringItem.Split(text);

            seTop.Lines.Clear();
            seBottom.Lines.Clear();
            vbSource.BeginUpdate();
            vbSnippetSource.BeginUpdate();
            try
            {
                vbSnippetSource.Lines.Clear();
                vbSource.Lines.Clear();
                bool before = true;
                for (int i = 0; i < lines.Length; i++)
                {
                    string s = lines[i];
                    if (s.Contains("{0}"))
                    {
                        InsertMethodBody(vbSnippetSource, GetVBCode());
                        InsertMethodBody(vbSource, GetVBCode());
                        if (currentType != ReadType.ClassLess)
                        {
                            readOnlyBefore = i - 1;
                            readOnlyAfter = vbSource.Lines.Count;
                        }

                        before = false;
                        continue;
                    }

                    vbSource.Lines.Add(s);
                    if (currentType == ReadType.ClassLess)
                        vbSnippetSource.Lines.Add(s);
                    else
                      if (before)
                        seTop.Lines.Add(s);
                    else
                        seBottom.Lines.Add(s);
                }

                if (currentType != ReadType.ClassLess)
                    vbSnippetSource.Lexer = isClass ? vbClassParser : vbMethodParser;
                vbSource.Lexer = vbParser;
            }
            finally
            {
                vbSource.EndUpdate();
                vbSnippetSource.EndUpdate();
            }
        }

        private void UpdateCSSource()
        {
            string text = string.Empty;
            switch (currentType)
            {
                case ReadType.ClassLess:
                    text = isClass ? CSSnippet.ClassLessSnippet.Replace("{{", "{").Replace("}}", "}") : CSSnippet.MethodLessSnippet.Replace("{{", "{").Replace("}}", "}");
                    break;
                default:
                    text = isClass ? csClassParser.Snippet.Replace("{{", "{").Replace("}}", "}") : csMethodParser.Snippet.Replace("{{", "{").Replace("}}", "}");
                    break;
            }

            string[] lines = StringItem.Split(text);
            seTop.Lines.Clear();
            seBottom.Lines.Clear();
            csSource.BeginUpdate();
            csSnippetSource.BeginUpdate();
            try
            {
                csSnippetSource.Lines.Clear();
                csSource.Lines.Clear();
                bool before = true;
                for (int i = 0; i < lines.Length; i++)
                {
                    string s = lines[i];
                    if (s.Contains("{0}"))
                    {
                        InsertMethodBody(csSnippetSource, GetCSCode());
                        InsertMethodBody(csSource, GetCSCode());
                        if (currentType != ReadType.ClassLess)
                        {
                            readOnlyBefore = i - 1;
                            readOnlyAfter = csSource.Lines.Count;
                        }

                        before = false;
                        continue;
                    }

                    csSource.Lines.Add(s);
                    if (currentType == ReadType.ClassLess)
                        csSnippetSource.Lines.Add(s);
                    else
                        if (before)
                        seTop.Lines.Add(s);
                    else
                        seBottom.Lines.Add(s);
                }

                if (currentType == ReadType.ClassLess)
                {
                    csSnippetSource.Lexer = csClassLessParser;
                }
                else
                    csSnippetSource.Lexer = isClass ? csClassParser : csMethodParser;

                csSource.Lexer = csParser;
            }
            finally
            {
                csSource.EndUpdate();
                csSnippetSource.EndUpdate();
            }
        }

        private string GetCSCode()
        {
            switch (currentType)
            {
                case ReadType.ClassLess:
                    return CSSnippet.ClassLessBodySnippet;
                default:
                    return isClass ? CSSnippet.ClassBodySnippet.Replace("{{", "{").Replace("}}", "}") : CSSnippet.MethodBodySnippet;
            }
        }

        private string GetVBCode()
        {
            switch (currentType)
            {
                case ReadType.ClassLess:
                    return VBSnippet.ClassLessBodySnippet;
                default:
                    return isClass ? VBSnippet.ClassBodySnippet : VBSnippet.MethodBodySnippet;
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            syntaxEdit1.DisplayLines.AllowHiddenLines = true;
            csSource.FileName = "csSource.cs";
            csSnippetSource.FileName = "csSnippetSource.cs";
            vbSource.FileName = "vbSource.vb";
            vbSnippetSource.FileName = "vbSnippetSource.vb";
            UpdateSource();
        }

        private void PartialRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbPartial.Checked)
                CurrentType = ReadType.Partial;
            else
                if (rbReadonly.Checked)
                CurrentType = ReadType.Readonly;
            else
                if (rbHidden.Checked)
                CurrentType = ReadType.Hidden;
            else
                CurrentType = ReadType.ClassLess;
        }

        private void CSharpRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CSharp = rbCSharp.Checked;
        }

        private void MethodRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            IsClass = rbClass.Checked;
        }
    }
}