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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace SnippetsParsers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All snippetparsers in the same unit")]
#pragma warning disable SA1649 // File name should match first type name
    public class VBSnippet
#pragma warning restore SA1649 // File name should match first type name
    {
        public const string MethodSnippet = @"Imports System
Public Class MyClass1
    Public Function MyFunc() As String
{0}
    End Function
End Class";

        public const string ClassSnippet = @"Imports System
Public Class MyClass1
{0}
End Class";

        public const string ClassLessSnippet = @"Imports System

' classes can be declared in the script
Class MyClass1
	Public Sub Test(ByVal text as String)
		Console.WriteLine(text)
	End Sub
End Class

' methods without class can be declared in the script

Public Function MyFunc(ByVal text As String) As String
	Console.WriteLine(text)
	Console.WriteLine(""Simple Properties"")
	{0}
End Function

' global code can be used in the script
Dim _myClass As MyClass1 = new MyClass1()
_myClass.Test(""hello"")
MyFunc(""hello"")";

        public const string MethodLessSnippet = @"Imports System

' methods without class can be declared in the script
Public Function MyFunc(ByVal text As String) As String
	Console.WriteLine(text)
	Console.WriteLine(""Simple Properties"")
	{0}
End Function

' global code can be used in the script
MyFunc(""hello"")";

        public const string MethodBodySnippet =
    @"Console.WriteLine(""Simple Properties"")

' Create a new Form object:
Dim form1 As New System.Windows.Forms.Form()

' Print out the name and the text associated with the form:
Console.WriteLine(""Form details - {0}"", form1)

' Set some values on the form object:
form1.Name = ""Form1""
form1.Text = ""New Form""
Console.WriteLine(""Form details - {0}"", form1)

' Change the Text property:
form1.Text = ""New Text""
Console.WriteLine(""Form details - {0}"", form1)
Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text)
' wrong method name
Dim i As Integer
' Unused variable
Return String.Empty";

        public const string ClassLessBodySnippet =
            @"' Create a new Form object:
Dim form1 As New System.Windows.Forms.Form()

' Print out the name and the text associated with the form:
Console.WriteLine(""Form details - {0}"", form1)

' Set some values on the form object:
form1.Name = ""Form1""
form1.Text = ""New Form""
Console.WriteLine(""Form details - {0}"", form1)

' Change the Text property:
form1.Text = ""New Text""
Console.WriteLine(""Form details - {0}"", form1)
Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text)
' wrong method name
Dim i As Integer
' Unused variable
Return String.Empty";

        public const string ClassBodySnippet =
            @"Public Function MyFunc() As String
	Console.WriteLine(""Simple Properties"")

	' Create a new Form object:
	Dim form1 As New System.Windows.Forms.Form()

    ' Print out the name and the text associated with the form:
	Console.WriteLine(""Form details - {0}"", form1)

	' Set some values on the form object:
	form1.Name = ""Form1""
	form1.Text = ""New Form""
	Console.WriteLine(""Form details - {0}"", form1)

	' Change the Text property:
	form1.Text = ""New Text""
	Console.WriteLine(""Form details - {0}"", form1)
	Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text)
	' wrong method name
	Dim i As Integer
	' Unused variable
	Return String.Empty
End Function";

        public const int StartMethodOffset = 3;
        public const int EndMethodOffset = 2;
        public const int StartClassOffset = 2;
        public const int EndClassOffset = 1;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All snippetparsers in the same unit")]
    public class ExtraVbSpaceRemover : VisualBasicSyntaxRewriter
    {
        private TextSpan span;
        private string textToRemove;
        private bool useSpaces = false;
        private int spacesInTab = 4;
        private int removed = 0;

        public ExtraVbSpaceRemover(TextSpan span, int count, bool useSpaces, int spacesInTab)
                 : base(true)
        {
            this.span = span;
            this.useSpaces = useSpaces;
            this.spacesInTab = spacesInTab;
            this.textToRemove = useSpaces ? new string(' ', spacesInTab * count) : new string('\t', count);
        }

        public int Removed => removed;

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (ContainsTrivia(trivia) && trivia.Kind() == SyntaxKind.WhitespaceTrivia && !ShouldSkipFormatting(trivia))
            {
                string text;
                if (NeedFormat(trivia, out text))
                    return SyntaxFactory.Whitespace(text);
            }

            return base.VisitTrivia(trivia);
        }

        private SyntaxTrivia? FindLeadingTrivia(Microsoft.CodeAnalysis.SyntaxNode node, int line)
        {
            while (node != null)
            {
                foreach (var trivia in node.GetLeadingTrivia())
                {
                    var location = trivia.GetLocation().GetLineSpan().StartLinePosition;

                    if (location.Line != line)
                        return null;

                    if (location.Character == 0)
                        return trivia;
                }

                node = node.Parent;
            }

            return null;
        }

        private bool NeedFormat(SyntaxTrivia trivia, out string text)
        {
            text = trivia.ToFullString();
            if (text.StartsWith(textToRemove))
            {
                text = text.Remove(0, textToRemove.Length);
                removed += textToRemove.Length;
                return true;
            }

            return false;
        }

        private bool ShouldSkipFormatting(SyntaxTrivia trivia)
        {
            var token = trivia.Token;
            var node = token.Parent;
            while (node != null)
            {
                switch (node.Kind())
                {
                    case SyntaxKind.FunctionBlock:
                    case SyntaxKind.SubBlock:
                    case SyntaxKind.ClassBlock:
                        return false;

                    case SyntaxKind.SimpleArgument:
                        var line = trivia.GetLocation().GetLineSpan().StartLinePosition.Line;
                        var argumentLine = node.GetLocation().GetLineSpan().StartLinePosition.Line;

                        if (line != argumentLine)
                            return false;

                        var argumentsLine = node.Parent.GetLocation().GetLineSpan().StartLinePosition.Line;
                        return line > argumentsLine;
                }

                node = node.Parent;
            }

            return false;
        }

        private bool ContainsTrivia(SyntaxTrivia trivia)
        {
            return (span.Start <= trivia.Span.Start && span.End >= trivia.Span.Start) || (span.Start <= trivia.Span.End && span.End >= trivia.Span.End);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBSnippetRepository : VbRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        private ISyntaxParser owner;

        public VBSnippetRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
        : base(owner, roslynSolution, caseSensitive)
        {
            this.owner = owner;
        }

        protected virtual int StartOffset
        {
            get
            {
                var parser = owner as VBMethodParser;
                return parser != null ? parser.StartOffset : 0;
            }
        }

        protected virtual int EndOffset
        {
            get
            {
                var parser = owner as VBMethodParser;
                return parser != null ? parser.EndOffset : 0;
            }
        }

        protected override bool UseIndentService
        {
            get
            {
                return false;
            }
        }

        public override int GetPosFromPosition(SourceText text, Point position)
        {
            return base.GetPosFromPosition(text, new Point(position.X, position.Y + StartOffset));
        }

        public override Point GetPositionFromPos(SourceText text, int pos)
        {
            Point point = base.GetPositionFromPos(text, pos);
            return new Point(point.X, point.Y - StartOffset);
        }

        protected virtual TextSpan TranslateSpan(Document document, Microsoft.CodeAnalysis.SyntaxNode newRoot, TextSpan span)
        {
            var newDoc = document.WithSyntaxRoot(newRoot);
            var changes = newDoc.GetTextChangesAsync(document).Result;

            int start = span.Start;
            int end = span.End;
            bool hasChange = false;
            foreach (TextChange change in changes)
            {
                if (start >= change.Span.Start)
                    start = change.Span.Start;

                end = end - change.Span.Length + change.NewText.Length;
                hasChange = true;
            }

            if (hasChange)
                end++;

            return new TextSpan(start, Math.Max(end - start, 0));
        }

        protected override bool IsValidSmartFormatNode(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            var linePos = node.SyntaxTree.GetLineSpan(node.Span);
            return linePos.StartLinePosition.Line >= StartOffset;
        }

        protected override bool IsValidChange(FileLinePositionSpan linePos, Point start, Point end)
        {
            int line = linePos.StartLinePosition.Line - StartOffset;
            return line >= start.Y && line <= end.Y;
        }

        protected override bool IsBlockNode(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            return base.IsBlockNode(node) && !(node.Kind() == SyntaxKind.ClassBlock) && !(node.Kind() == SyntaxKind.NamespaceBlock);
        }

        protected override Point GetPointFromLinePos(LinePosition linePos)
        {
            return new Point(linePos.Character, linePos.Line - StartOffset);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBMethodRepository : VBSnippetRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        public VBMethodRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
            : base(owner, roslynSolution, caseSensitive)
        {
        }

        protected override bool IsBlockNode(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            return base.IsBlockNode(node) && !(node.Kind() == SyntaxKind.FunctionBlock) && !(node.Kind() == SyntaxKind.SubBlock) && !(node.Kind() == SyntaxKind.ClassBlock) && !(node.Kind() == SyntaxKind.NamespaceBlock);
        }

        protected override Document CreateFormattedDocument(Document document, Microsoft.CodeAnalysis.SyntaxNode newRoot, TextSpan span, bool useSpaces, int spacesInTab)
        {
            span = TranslateSpan(document, newRoot, span);

            var rewriter = new ExtraVbSpaceRemover(span, 2, useSpaces, spacesInTab);
            newRoot = rewriter.Visit(newRoot);

            span = new TextSpan(span.Start, Math.Max(span.Length - rewriter.Removed, 0));
            return base.CreateFormattedDocument(document, newRoot, span, useSpaces, spacesInTab);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBClassRepository : VBSnippetRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        public VBClassRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
            : base(owner, roslynSolution, caseSensitive)
        {
        }

        protected override Document CreateFormattedDocument(Document document, Microsoft.CodeAnalysis.SyntaxNode newRoot, TextSpan span, bool useSpaces, int spacesInTab)
        {
            span = TranslateSpan(document, newRoot, span);

            var rewriter = new ExtraVbSpaceRemover(span, 1, useSpaces, spacesInTab);
            newRoot = rewriter.Visit(newRoot);

            span = new TextSpan(span.Start, Math.Max(span.Length - rewriter.Removed, 0));
            return base.CreateFormattedDocument(document, newRoot, span, useSpaces, spacesInTab);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBMethodParser : VbParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        public VBMethodParser()
            : this(VbSolution.DefaultSolution)
        {
        }

        public VBMethodParser(IRoslynSolution roslynSolution)
            : base(roslynSolution)
        {
            StartOffset = VBSnippet.StartMethodOffset;
            EndOffset = VBSnippet.EndMethodOffset;
            Snippet = VBSnippet.MethodSnippet;
        }

        public virtual int StartOffset { get; set; }

        public virtual int EndOffset { get; set; }

        public virtual string Snippet { get; set; }

        public override bool SupportsTextChanges()
        {
            return false;
        }

        public override void StartParsing(string fileName, int first, int last)
        {
            if (last == int.MaxValue)
                last = first + StartOffset + ParserConsts.LookAheadLinesAsync;
            else
                last += StartOffset;
            base.StartParsing(fileName, first, last);
        }

        public override async Task ParseSyntaxAsync(Document document, int first, int last, CancellationToken cancellationToken)
        {
            if (last == int.MaxValue)
                last = first + StartOffset + ParserConsts.LookAheadLinesAsync;
            else
                last += StartOffset;
            await base.ParseSyntaxAsync(document, first, last, cancellationToken);
        }

        public override int GetStructureGuideLines(IList<IRange> guideLines)
        {
            var list = new List<IRange>();
            base.GetStructureGuideLines(list);

            guideLines.Clear();
            foreach (var range in list)
            {
                if (range.StartPoint.Y >= StartOffset)
                {
                    IRange newRange = range.Clone() as IRange;
                    newRange.StartPoint = new Point(range.StartPoint.X, range.StartPoint.Y - StartOffset);
                    newRange.EndPoint = new Point(range.EndPoint.X, range.EndPoint.Y - StartOffset);
                    guideLines.Add(newRange);
                }
            }

            return guideLines.Count;
        }

        public override Alternet.Syntax.CodeCompletion.ICodeCompletionRepository CreateRepository()
        {
            return new VBMethodRepository(this, RoslynSolution, true);
        }

        public override int GetSyntaxErrors(IList<ISyntaxError> errors)
        {
            var list = new List<ISyntaxError>();
            base.GetSyntaxErrors(list);

            errors.Clear();
            foreach (var error in list)
            {
                if (error.Position.Y >= StartOffset)
                {
                    var newError = error.Clone() as ISyntaxError;
                    newError.Position = new Point(error.Position.X, error.Position.Y - StartOffset);
                    newError.Range.EndPoint = new Point(error.Range.EndPoint.X, error.Range.EndPoint.Y - StartOffset);
                    errors.Add(newError);
                }
            }

            return errors.Count;
        }

        public override int Outline(IList<IRange> ranges)
        {
            var list = new List<IRange>();
            base.Outline(list);

            ranges.Clear();
            foreach (var range in list)
            {
                if (range.StartPoint.Y >= StartOffset)
                {
                    IRange newRange = range.Clone() as IRange;
                    newRange.StartPoint = new Point(range.StartPoint.X, range.StartPoint.Y - StartOffset);
                    newRange.EndPoint = new Point(range.EndPoint.X, range.EndPoint.Y - StartOffset);
                    ranges.Add(newRange);
                }
            }

            return ranges.Count;
        }

        protected void SetSourceTextInternal(string text)
        {
            base.SetSourceText(text);
        }

        protected override void SetSourceText(string text)
        {
            SetSourceTextInternal(text != null ? string.Format(Snippet, text) : null);
        }

        protected override Tokenizer CreateTokenizer()
        {
            return new SnippetVbTokenizer(this);
        }

        public class SnippetVbTokenizer : VbTokenizer
        {
            private VBMethodParser parser;

            public SnippetVbTokenizer(VBMethodParser parser)
            : base(parser)
            {
                this.parser = parser;
            }

            public override void AddTokens(TokenList tokens, bool replace)
            {
                AdjustTokens(tokens);
                base.AddTokens(tokens, replace);
            }

            public void AdjustTokens(TokenList tokens)
            {
                int count = tokens.Count;

                int startOffset = parser != null ? parser.StartOffset : 0;
                int endOffset = parser != null ? parser.EndOffset : 0;
                if (count < startOffset)
                {
                    tokens.Clear();
                    return;
                }

                if (parser.Lines != null)
                    count = Math.Max(parser.Lines.Length + startOffset + endOffset, tokens.Count);

                RemoveRange(tokens, count - endOffset, endOffset);
                RemoveRange(tokens, 0, startOffset);

                foreach (var token in tokens)
                {
                    if (token != null)
                    {
                        foreach (var lineToken in token)
                        {
                            lineToken.Line -= startOffset;
                        }
                    }
                }
            }

            private void RemoveRange(TokenList tokens, int pos, int count)
            {
                if (pos >= tokens.Count)
                    return;
                count = Math.Min(count, tokens.Count - pos);
                tokens.RemoveRange(pos, count);
            }
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBClassLessParser : VBClassParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        public VBClassLessParser()
            : this(VbSolution.DefaultScriptSolution)
        {
        }

        public VBClassLessParser(IRoslynSolution roslynSolution)
            : base(roslynSolution)
        {
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class VBClassParser : VBMethodParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        public VBClassParser()
            : this(VbSolution.DefaultSolution)
        {
        }

        public VBClassParser(IRoslynSolution roslynSolution)
            : base(roslynSolution)
        {
            StartOffset = VBSnippet.StartClassOffset;
            EndOffset = VBSnippet.EndClassOffset;
            Snippet = VBSnippet.ClassSnippet;
        }

        public override Alternet.Syntax.CodeCompletion.ICodeCompletionRepository CreateRepository()
        {
            return new VBClassRepository(this, RoslynSolution, true);
        }

        protected override void SetSourceText(string text)
        {
            SetSourceTextInternal(text != null ? string.Format(VBSnippet.ClassSnippet, text) : null);
        }
    }
}
