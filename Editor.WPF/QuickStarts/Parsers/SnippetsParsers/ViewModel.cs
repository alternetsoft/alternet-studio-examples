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
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using Alternet.Editor.Wpf;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

namespace SnippetsParsers
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string ReadonlyCodeDescription = "Code in the editor is a full source code, but user can only change editable section.";
        private const string HiddenCodeDescription = "Code in the editor is a full source code, but user can only change visible section.";
        private const string PartialCodeDescription = "Code in this editor is a part of the full source code (in the top and bottom editors). User can change only that part and get full intellisense functionality.";
        private const string ClassLessCodeDescription = "Code in the editor is a full source code, and associated parser allows class-less scripts, i.e global code without class or method declaration.";

        private TextSource csSnippetSource = new TextSource();
        private TextSource csSource = new TextSource();
        private TextSource vbSnippetSource = new TextSource();
        private TextSource vbSource = new TextSource();

        private bool csharp = true;
        private bool isClass = false;
        private int readOnlyBefore = 0;
        private int readOnlyAfter = 0;
        private bool sourceUpdating = false;

        private ReadType currentType = ReadType.Partial;

        private VBMethodParser vbMethodParser = new VBMethodParser();
        private VBClassParser vbClassParser = new VBClassParser(new VbSolution());
        private VbParser vbParser = new VbParser(new VbSolution());

        private CSMethodParser csMethodParser = new CSMethodParser();
        private CSClassParser csClassParser = new CSClassParser(new CsSolution());
        private CsParser csClassLessParser = new CsParser(CsSolution.DefaultScriptSolution);
        private CsParser csParser = new CsParser(new CsSolution());

        private MainWindow window;

        public ViewModel()
        {
            LanguageCommand = new RelayCommand(LanguageClick);
            HideCommand = new RelayCommand(HideClick);
            MethodCommand = new RelayCommand(MethodClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            var readonlyColor = System.Windows.Media.Color.FromRgb(240, 240, 240);
            window.seTop.ReadonlyBackColor = readonlyColor;
            window.seBottom.ReadonlyBackColor = readonlyColor;
            window.syntaxEdit1.ReadonlyBackColor = readonlyColor;
            window.syntaxEdit1.DisplayLines.AllowHiddenLines = true;
            csSource.FileName = "csSource.cs";
            csSnippetSource.FileName = "csSnippetSource.cs";
            vbSource.FileName = "vbSource.vb";
            vbSnippetSource.FileName = "vbSnippetSource.vb";
            UpdateSource();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Flags]
        internal enum ReadType
        {
            Partial,
            Readonly,
            Hidden,
            ClassLess,
        }

        public ICommand LanguageCommand { get; set; }

        public ICommand HideCommand { get; set; }

        public ICommand MethodCommand { get; set; }

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LanguageClick()
        {
            CSharp = window.rbCSharp.IsChecked.Value;
        }

        private void HideClick()
        {
            if (window.rbPartial.IsChecked.Value)
                CurrentType = ReadType.Partial;
            else
                if (window.rbReadonly.IsChecked.Value)
                CurrentType = ReadType.Readonly;
            else
                if (window.rbHidden.IsChecked.Value)
                CurrentType = ReadType.Hidden;
            else
                CurrentType = ReadType.ClassLess;
        }

        private void MethodClick()
        {
            IsClass = window.rbClass.IsChecked.Value;
        }

        private void UpdateHidden()
        {
            if (window.Panels.RowDefinitions.Count >= 4)
            {
                window.Panels.RowDefinitions[0].Height = (currentType == ReadType.Partial) ? new GridLength(90) : new GridLength(0);
                window.Panels.RowDefinitions[3].Height = (currentType == ReadType.Partial) ? new GridLength(50) : new GridLength(0);
            }

            window.ReadonlyCodeLabel1.Visibility = (currentType == ReadType.Partial) ? Visibility.Visible : Visibility.Collapsed;
            window.ReadonlyCodeLabel2.Visibility = (currentType == ReadType.Partial) ? Visibility.Visible : Visibility.Collapsed;
            window.seTop.Visibility = currentType == ReadType.Partial ? Visibility.Visible : Visibility.Hidden;
            window.seBottom.Visibility = currentType == ReadType.Partial ? Visibility.Visible : Visibility.Hidden;

            if (csharp)
                window.syntaxEdit1.Source = currentType == ReadType.Readonly || currentType == ReadType.Hidden ? csSource : csSnippetSource;
            else
                window.syntaxEdit1.Source = currentType == ReadType.Readonly || currentType == ReadType.Hidden ? vbSource : vbSnippetSource;

            var parser = window.syntaxEdit1.Lexer as RoslynParser;
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
                    window.CodeDescription.Text = PartialCodeDescription;
                    break;
                case ReadType.Readonly:
                    window.CodeDescription.Text = ReadonlyCodeDescription;
                    break;
                case ReadType.ClassLess:
                    window.CodeDescription.Text = ClassLessCodeDescription;
                    break;
            }

            if (currentType == ReadType.Partial)
            {
                window.seTop.MakeVisible(new System.Drawing.Point(0, 0));
                window.seBottom.MakeVisible(new System.Drawing.Point(0, 0));
            }

            if (window.Editors.RowDefinitions.Count > 2)
            {
                window.Editors.RowDefinitions[0].Height = currentType == ReadType.Partial ? new GridLength(90) : new GridLength(0);
                window.Editors.RowDefinitions[2].Height = currentType == ReadType.Partial ? new GridLength(50) : new GridLength(0);
            }

            for (int i = 0; i < window.syntaxEdit1.Source.Lines.Count; i++)
            {
                window.syntaxEdit1.Source.SetLineReadonly(i, currentType == ReadType.Readonly && ((i <= readOnlyBefore) || (i >= readOnlyAfter)));
                window.syntaxEdit1.Source.SetLineHidden(i, currentType == ReadType.Hidden && ((i <= readOnlyBefore) || (i >= readOnlyAfter)));
            }
        }

        private void UpdateSource()
        {
            if (sourceUpdating)
                return;

            sourceUpdating = true;
            try
            {
                window.rbClassLess.IsEnabled = csharp;
                if (csharp)
                    UpdateCSSource();
                else
                    UpdateVBSource();
                UpdateHidden();

                window.syntaxEdit1.Position = System.Drawing.Point.Empty;
                window.syntaxEdit1.MakeVisible(window.syntaxEdit1.Position);
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
                    window.rbPartial.IsChecked = true;
                    HideClick();
                    break;
                default:
                    text = isClass ? vbClassParser.Snippet : vbMethodParser.Snippet;
                    break;
            }

            string[] lines = StringItem.Split(text);
            window.seTop.Lines.Clear();
            window.seBottom.Lines.Clear();
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
                        window.seTop.Lines.Add(s);
                    else
                        window.seBottom.Lines.Add(s);
                }

                if (currentType == ReadType.ClassLess)
                {
                }
                else
                    vbSnippetSource.Lexer = isClass ? vbClassParser : vbMethodParser;

                vbSource.Lexer = vbParser;
            }
            finally
            {
                vbSource.EndUpdate();
                vbSnippetSource.EndUpdate();
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
            window.seTop.Lines.Clear();
            window.seBottom.Lines.Clear();
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
                        window.seTop.Lines.Add(s);
                    else
                        window.seBottom.Lines.Add(s);
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
    }
}
