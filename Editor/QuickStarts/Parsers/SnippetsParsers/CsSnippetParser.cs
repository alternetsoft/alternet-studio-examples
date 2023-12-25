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
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace SnippetsParsers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All snippetparsers in the same unit")]
#pragma warning disable SA1649 // File name should match first type name
    public class CSSnippet
#pragma warning restore SA1649 // File name should match first type name
    {
        public const string MethodSnippet = @"using System;
public class MyClass1
{{
    public string MyFunc()
    {{
{0}
    }}
}}";

        public const string ClassSnippet = @"using System;
public class MyClass1
{{
{0}
}}";

        public const string ClassLessSnippet = @"using System;

// classes can be declared in the script
class MyClass
{{
	public void Test(string text)
	{{
		Console.WriteLine(text);
	}}
}}

// methods without class can be declared in the script
public string MyFunc(string text)
{{
	Console.WriteLine(text);
	Console.WriteLine(""Simple Properties"");
	{0}
}}

// global code can be used in the script
var myClass = new MyClass();
myClass.Test(""hello"");
MyFunc(""hello"");";

        public const string MethodLessSnippet = @"using System;

Console.WriteLine(""Simple Properties"");
{0}";

        public const string MethodBodySnippet =
            @"Console.WriteLine(""Simple Properties"");

// Create a new Form object:
System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();

// Print out the name and the text associated with the form:
Console.WriteLine(""Form details - {0}"", form1);

// Set some values on the form object:
form1.Name = ""Form1"";
form1.Text = ""New Form"";
Console.WriteLine(""Form details - {0}"", form1);

// Change the Text property:
form1.Text = ""New Text"";
Console.WriteLine(""Form details - {0}"", form1);
Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text);
// wrong method name
int i;
// Unused variable
return string.Empty;";

        public const string ClassLessBodySnippet =
            @"// Create a new Form object:
System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();

// Print out the name and the text associated with the form:
Console.WriteLine(""Form details - {0}"", form1);

// Set some values on the form object:
form1.Name = ""Form1"";
form1.Text = ""New Form"";
Console.WriteLine(""Form details - {0}"", form1);

// Change the Text property:
form1.Text = ""New Text"";
Console.WriteLine(""Form details - {0}"", form1);
Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text);
// wrong method name
int i;
// Unused variable
return string.Empty;";

        public const string ClassBodySnippet =
            @"public string MyFunc()
{{
	Console.WriteLine(""Simple Properties"");

	// Create a new Form object:
	System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();

	// Print out the name and the text associated with the form:
	Console.WriteLine(""Form details - {0}"", form1);

	// Set some values on the form object:
	form1.Name = ""Form1"";
	form1.Text = ""New Form"";
	Console.WriteLine(""Form details - {0}"", form1);

	// Change the Text property:
	form1.Text = ""New Text"";
	Console.WriteLine(""Form details - {0}"", form1);
	Console.DoSomething(""Name: {0}, Text: {1}"", form1.Name, form1.Text);
	// wrong method name
	int i;
	// Unused variable
	return string.Empty;
}}";

        public const int StartMethodOffset = 5;
        public const int EndMethodOffset = 2;
        public const int StartClassOffset = 3;
        public const int EndClassOffset = 1;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All snippetparsers in the same unit")]
    public class ExtraCsSpaceRemover : CSharpSyntaxRewriter
    {
        private TextSpan span;
        private string textToRemove;
        private int removed = 0;
        private IList<TextChange> changes = new List<TextChange>();

        public ExtraCsSpaceRemover(TextSpan span, int count, bool useSpaces, int spacesInTab)
            : base(true)
        {
            this.span = span;
            this.textToRemove = useSpaces ? new string(' ', spacesInTab * count) : new string('\t', count);
        }

        public int Removed => removed;

        public IList<TextChange> Changes => changes;

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (ContainsTrivia(trivia) && trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                string text;
                if (NeedFormat(trivia, out text))
                    return SyntaxFactory.Whitespace(text);
            }

            return base.VisitTrivia(trivia);
        }

        private bool NeedFormat(SyntaxTrivia trivia, out string text)
        {
            text = trivia.ToFullString();
            if (text.StartsWith(textToRemove))
            {
                int len = textToRemove.Length;
                text = text.Remove(0, len);
                removed += len;
                changes.Add(new TextChange(new TextSpan(trivia.FullSpan.Start, len), string.Empty));
                return true;
            }

            return false;
        }

        private bool ContainsTrivia(SyntaxTrivia trivia)
        {
            return (span.Start <= trivia.Span.Start && span.End >= trivia.Span.Start) || (span.Start <= trivia.Span.End && span.End >= trivia.Span.End);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class CSSnippetRepository : CsRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        private ISyntaxParser owner;

        public CSSnippetRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
            : base(owner, roslynSolution, caseSensitive)
        {
            this.owner = owner;
        }

        protected virtual int StartOffset
        {
            get
            {
                var parser = owner as CSMethodParser;
                return parser != null ? parser.StartOffset : 0;
            }
        }

        protected virtual int EndOffset
        {
            get
            {
                var parser = owner as CSMethodParser;
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

        protected override bool ShowItemsFromUnimportedNamespaces
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

        protected override bool IsBlockNode(Microsoft.CodeAnalysis.SyntaxNode node, int pos)
        {
            return base.IsBlockNode(node, pos) && !node.IsKind(SyntaxKind.MethodDeclaration) && !node.IsKind(SyntaxKind.ClassDeclaration) && !node.IsKind(SyntaxKind.NamespaceDeclaration);
        }

        protected override bool IsValidSmartFormatNode(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            var linePos = node.SyntaxTree.GetLineSpan(node.Span);
            return linePos.StartLinePosition.Line >= StartOffset;
        }

        protected override bool IsValidChange(Microsoft.CodeAnalysis.SyntaxTree tree, TextChange textChange)
        {
            var linePos = tree.GetLineSpan(textChange.Span);
            return linePos.StartLinePosition.Line >= StartOffset;
        }

        protected override bool IsValidChange(FileLinePositionSpan linePos, Point start, Point end)
        {
            int line = linePos.StartLinePosition.Line - StartOffset;
            return line >= start.Y && line <= end.Y;
        }

        protected virtual TextSpan TranslateSpan(Document document, Microsoft.CodeAnalysis.SyntaxNode newRoot, TextSpan span)
        {
            var newDoc = document.WithSyntaxRoot(newRoot);
            var changes = newDoc.GetTextChangesAsync(document).Result;
            return TranslateSpan(span, changes);
        }

        protected virtual TextSpan TranslateSpan(TextSpan span, IEnumerable<TextChange> changes)
        {
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

        protected virtual ExtraCsSpaceRemover CreateRewriter(TextSpan span, bool useSpaces, int spacesInTab)
        {
            return new ExtraCsSpaceRemover(span, 1, useSpaces, spacesInTab);
        }

        protected override Document CreateFormattedDocument(Document document, Microsoft.CodeAnalysis.SyntaxNode newRoot, TextSpan span, bool useSpaces, int spacesInTab)
        {
            span = TranslateSpan(document, newRoot, span);
            var rewriter = CreateRewriter(span, useSpaces, spacesInTab);
            newRoot = rewriter.Visit(newRoot);
            span = new TextSpan(span.Start, Math.Max(span.Length - rewriter.Removed, 0));
            return base.CreateFormattedDocument(document, newRoot, span, useSpaces, spacesInTab);
        }

        protected override IEnumerable<TextChange> FormatChanges(Document document, Microsoft.CodeAnalysis.SyntaxTree tree, ref SourceText text, TextSpan span, bool useSpaces, int spacesInTab)
        {
            SourceText newText = text;
            var changes = base.FormatChanges(document, tree, ref newText, span, useSpaces, spacesInTab);
            span = TranslateSpan(span, changes);

            tree = tree.WithChangedText(newText);
            var root = tree.GetRoot();

            var rewriter = CreateRewriter(span, useSpaces, spacesInTab);
            rewriter.Visit(root);

            if (rewriter.Changes.Count > 0)
            {
                var mergedText = newText.WithChanges(rewriter.Changes);
                changes = MergeChanges(mergedText, changes, rewriter.Changes);
                text = mergedText;
            }
            else
                text = newText;

            return changes;
        }

        protected override Point GetPointFromLinePos(LinePosition linePos)
        {
            return new Point(linePos.Character, linePos.Line - StartOffset);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class CSMethodRepository : CSSnippetRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CSMethodRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
        : base(owner, roslynSolution, caseSensitive)
        {
        }

        protected override bool IsBlockNode(Microsoft.CodeAnalysis.SyntaxNode node, int pos)
        {
            if (node.IsKind(SyntaxKind.Block))
            {
                if (node.Parent != null && (node.Parent.IsKind(SyntaxKind.MethodDeclaration) || node.IsKind(SyntaxKind.PropertyDeclaration) || node.Parent.IsKind(SyntaxKind.GetAccessorDeclaration) || node.Parent.IsKind(SyntaxKind.SetAccessorDeclaration)))
                    return false;
            }

            return base.IsBlockNode(node, pos) && !node.IsKind(SyntaxKind.PropertyDeclaration);
        }

        protected override ExtraCsSpaceRemover CreateRewriter(TextSpan span, bool useSpaces, int spacesInTab)
        {
            return new ExtraCsSpaceRemover(span, 2, useSpaces, spacesInTab);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class CSClassRepository : CSSnippetRepository
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CSClassRepository(ISyntaxParser owner, IRoslynSolution roslynSolution, bool caseSensitive)
            : base(owner, roslynSolution, caseSensitive)
        {
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class CSMethodParser : CsParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CSMethodParser()
            : this(CsSolution.DefaultSolution)
        {
        }

        public CSMethodParser(IRoslynSolution roslynSolution)
            : base(roslynSolution)
        {
            StartOffset = CSSnippet.StartMethodOffset;
            EndOffset = CSSnippet.EndMethodOffset;
            Snippet = CSSnippet.MethodSnippet;
            Options = Options & ~(SyntaxOptions.CodeFixes | SyntaxOptions.CodeRefactors);
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
            return new CSMethodRepository(this, RoslynSolution, true);
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
            return new SnippetCsTokenizer(this);
        }

        public class SnippetCsTokenizer : CsTokenizer
        {
            private CSMethodParser parser;

            public SnippetCsTokenizer(CSMethodParser parser)
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
    public class CSClassParser : CSMethodParser
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CSClassParser()
            : this(CsSolution.DefaultSolution)
        {
        }

        public CSClassParser(IRoslynSolution roslynSolution)
            : base(roslynSolution)
        {
            StartOffset = CSSnippet.StartClassOffset;
            EndOffset = CSSnippet.EndClassOffset;
            Snippet = CSSnippet.ClassSnippet;
        }

        public override Alternet.Syntax.CodeCompletion.ICodeCompletionRepository CreateRepository()
        {
            return new CSClassRepository(this, RoslynSolution, true);
        }

        protected override void SetSourceText(string text)
        {
            SetSourceTextInternal(text != null ? string.Format(Snippet, text) : null);
        }
    }
}
