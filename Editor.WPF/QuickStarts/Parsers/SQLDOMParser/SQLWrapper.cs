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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Parsers.Advanced;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SQLDOMParser
{
    public class SQLWrapper : SyntaxParser
    {
        private TSql100Parser sqlParser = new TSql100Parser(true);
        private IList<TSqlBatch> batches = new List<TSqlBatch>();
        private IList<TSqlParserToken> scriptTokenStream = new List<TSqlParserToken>();
        private IList<ParseError> scriptErrors = new List<ParseError>();
        private ImageList internalImages;
        private SQLTokenizer tokenizer = new SQLTokenizer();

        public SQLWrapper()
            : base()
        {
            internalImages = new ImageList();
            internalImages.ImageSize = new System.Drawing.Size(16, 16);

            try
            {
                internalImages = LoadImageListFromStrip("DotNetImages.png");
            }
            catch
            {
            }

            internalImages.TransparentColor = SyntaxConsts.DefaultTransparentColor;
        }

        public override void ResetOptions()
        {
            Options = SyntaxOptions.Outline | SyntaxOptions.SyntaxErrors | SyntaxOptions.SmartIndent |
            SyntaxOptions.CodeCompletion | SyntaxOptions.FormatSpaces | SyntaxOptions.FormatCase | SyntaxOptions.AutoComplete;
        }

        public override void ReparseText()
        {
            ParseSyntax();

            Notify(new SyntaxParserEventArgs(true, true, 0, int.MaxValue));

            OnTextParsed();
        }

        public virtual void ParseSyntax()
        {
            tokenizer.BeginUpdate();
            try
            {
                ParseSQL();

                if (Strings == null)
                    return;

                TokenList tokens = tokenizer.GetTokens(scriptTokenStream, Strings.Count);

                tokenizer.Tokens = tokens;
            }
            finally
            {
                tokenizer.EndUpdate();
            }
        }

        public override int ParseText(int state, int line, string str, ref StringItemInfo[] colorData)
        {
            if (Strings != null && !tokenizer.Initialized && tokenizer.UpdateCount == 0)
            {
                ReparseSyntax();
            }

            tokenizer.ParseText(line, ref colorData);

            return state;
        }

        public override int Outline(IList<IRange> ranges)
        {
           foreach (TSqlBatch batch in batches)
                Outline(ranges, batch.Statements, 0);
           return ranges.Count;
        }

        public override int GetSyntaxErrors(IList<ISyntaxError> errors)
        {
            if (scriptErrors != null)
            {
                foreach (var error in scriptErrors)
                {
                    var syntaxError = new SyntaxError(new Point(error.Column - 1, error.Line - 1), string.Empty, error.Message);
                    TSqlParserToken errorToken = FindErrorToken(error);
                    syntaxError.Range.EndPoint = (errorToken != null) && (!string.IsNullOrEmpty(errorToken.Text)) ? new Point(errorToken.Column + errorToken.Text.Length - 1, errorToken.Line - 1) : new Point(error.Column, error.Line - 1);
                    errors.Add(syntaxError);
                }
            }

            return errors.Count;
        }

        /// <summary>
        /// Indicates whether the <c>Options</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>Options</c> differs from its default value; otherwise false.</returns>
        public override bool ShouldSerializeOptions()
        {
            return Options != (SyntaxOptions.Outline | SyntaxOptions.SyntaxErrors);
        }

        public int GetSelectedIndex(IListMembers members, string name)
        {
            int idx;
            FindMember(name, members, out idx);
            return idx;
        }

        public override void ResetCodeCompletionChars()
        {
            CodeCompletionChars = (SyntaxParserConsts.ExtendedNetCodeCompletionChars + ' ').ToCharArray();
        }

        public override ICodeCompletionRepository CreateRepository()
        {
            return new SqlWrapRepository(CaseSensitive, SyntaxTree);
        }

        public override CodeCompletionType GetCompletionType(char ch)
        {
            switch (ch)
            {
                case '.':
                    return CodeCompletionType.ListMembers;
                default:
                    if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                        return CodeCompletionType.ListMembers;
                    break;
            }

            return base.GetCompletionType(ch);
        }

        public override int SmartFormatLine(int index, string text, StringItemInfo[] textData, ITextUndoList operations, out bool actualPosition)
        {
            if ((Options & SyntaxOptions.FormatCase) != 0)
                SmartCapitalize(text, textData, operations);
            return base.SmartFormatLine(index, text, textData, operations, out actualPosition);
        }

        protected virtual void SmartCapitalize(string s, StringItemInfo[] colorData, ITextUndoList operations)
        {
            SqlWrapRepository repository = CompletionRepository as SqlWrapRepository;
            int i = 0;
            int len = s.Length;
            while (i < len)
            {
                if (colorData[i].Data == 2 + 1)
                {
                    int j = i;
                    while ((j < len - 1) && (colorData[j + 1].Data == 2 + 1))
                        j++;
                    string word = s.Substring(i, j - i + 1);
                    object obj = repository.Reswords[word.ToUpper()];
                    if (obj != null)
                    {
                        string rword = obj.ToString().ToUpper();
                        if (rword != word)
                            operations.Add(new TextUndo(i, j - i + 1, rword));
                    }

                    i = j;
                }

                i++;
            }
        }

        protected void ReparseSyntax()
        {
            tokenizer.ReparseSyntax(scriptTokenStream, Strings.Count);
        }

        protected bool IsCollapsibleStatement(TSqlStatement statement)
        {
            if (statement is BeginEndBlockStatement)
                return true;

            if (statement is SelectStatement)
                return true;

            if (statement is UpdateStatement)
                return true;

            return false;
        }

        protected virtual void Outline(IList<IRange> ranges, IList<TSqlStatement> statements, int level)
        {
            foreach (TSqlStatement stat in statements)
            {
                if (IsCollapsibleStatement(stat))
                {
                    TSqlParserToken startToken = (stat.FirstTokenIndex >= 0 && stat.FirstTokenIndex < stat.ScriptTokenStream.Count) ? stat.ScriptTokenStream[stat.FirstTokenIndex] : null;
                    TSqlParserToken endToken = (stat.LastTokenIndex >= 0 && stat.LastTokenIndex < stat.ScriptTokenStream.Count) ? stat.ScriptTokenStream[stat.LastTokenIndex] : null;
                    if ((startToken != null) && (endToken != null) && (endToken.Line > startToken.Line))
                    {
                        ranges.Add(
                            new OutlineRange(
                                new Point(startToken.Column - 1, startToken.Line - 1),
                                !string.IsNullOrEmpty(endToken.Text) ? new Point(endToken.Column - 1 + endToken.Text.Length, endToken.Line - 1) : new Point(endToken.Column - 1, endToken.Line - 1),
                                level,
                                !string.IsNullOrEmpty(startToken.Text) ? startToken.Text : "..."));
                    }

                    level++;
                    if (stat is BeginEndBlockStatement)
                    {
                        BeginEndBlockStatement blockStat = (BeginEndBlockStatement)stat;
                        foreach (TSqlStatement child in blockStat.StatementList.Statements)
                            Outline(ranges, blockStat.StatementList.Statements, level);
                    }
                }
            }
        }

        protected override void InitStyles()
        {
            InitDefaultStyles();
        }

        protected override IListMembers CreateListMembers()
        {
            return new SqlWrapListMembers();
        }

        protected override IParameterInfo CreateParameterInfo()
        {
            return new SqlWrapParameterInfo();
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (internalImages != null)
                {
                    internalImages.Dispose();
                    internalImages = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void ParseSQL()
        {
            StringReader reader = new StringReader(Strings.Text);
            var script = sqlParser.Parse(reader, out scriptErrors) as TSqlScript;
            batches.Clear();
            if (script.Batches != null)
            {
                foreach (TSqlBatch batch in script.Batches)
                    batches.Add(batch);
            }

            scriptTokenStream.Clear();
            if (script.ScriptTokenStream != null)
            {
                foreach (TSqlParserToken token in script.ScriptTokenStream)
                    scriptTokenStream.Add(token);
            }
        }

        private TSqlParserToken FindErrorToken(ParseError error)
        {
            foreach (TSqlParserToken token in scriptTokenStream)
            {
                if ((token.Column == error.Column) && (token.Line == error.Line))
                    return token;
            }

            return null;
        }

        private TSqlParserToken GetPrevToken(Point position)
        {
            foreach (TSqlParserToken token in scriptTokenStream)
            {
                if (token.Line == position.Y + 1)
                {
                    if (token.Column + token.Text.Length == position.X)
                        return token;
                }
            }

            return null;
        }

        private TSqlParserToken GetToken(Point position)
        {
            foreach (TSqlParserToken token in scriptTokenStream)
            {
                if (token.Line == position.Y + 1)
                {
                    if (token.Column == position.X)
                        return token;
                }
            }

            return null;
        }

        private string GetQualifiedIdentifier(int x, int y)
        {
            string s = string.Empty;
            IList<TSqlParserToken> list = scriptTokenStream.Where(tok => tok.Line == y + 1).Where(tok => tok.Column <= x + 1).ToList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (IsKeywordOrIdentifier(list[i]) || (string.Compare(list[i].Text, ".") == 0))
                    s = list[i].Text + s;
                else
                    if (!string.IsNullOrEmpty(s))
                    break;
            }

            return s;
        }

        private string GetPrevWord(int x, int y)
        {
            TSqlParserToken result = scriptTokenStream.Where(tok => tok.Line == y + 1).Where(tok => tok.Column <= x + 1).Where(tok => IsKeywordOrIdentifier(tok)).LastOrDefault();
            return result != null ? result.Text : string.Empty;
        }

        private string GetCurWord(int x, int y)
        {
            TSqlParserToken result = scriptTokenStream.Where(tok => tok.Line == y + 1).Where(tok => tok.Column <= x + 1).Where(tok => tok.Column + tok.Text.Length > x).Where(tok => IsKeywordOrIdentifier(tok)).LastOrDefault();
            return result != null ? result.Text : string.Empty;
        }

        private string GetNextWord(int x, int y)
        {
            TSqlParserToken result = scriptTokenStream.Where(tok => tok.Line == y + 1).Where(tok => tok.Column >= x + 1).Where(tok => IsKeywordOrIdentifier(tok)).FirstOrDefault();
            return result != null ? result.Text : string.Empty;
        }

        private string GetDelimiter(int x, int y)
        {
            TSqlParserToken result = scriptTokenStream.Where(tok => tok.Line == y + 1).Where(tok => tok.Column <= x + 1).Where(tok => IsDelimiter(tok)).LastOrDefault();
            return result != null ? result.Text : string.Empty;
        }

        private bool IsKeywordOrIdentifier(TSqlParserToken token)
        {
            switch (token.TokenType)
            {
                case TSqlTokenType.Identifier:
                case TSqlTokenType.QuotedIdentifier:
                    return true;

                default:
                    return token.IsKeyword();
            }
        }

        private bool IsDelimiter(TSqlParserToken token)
        {
            switch (token.TokenType)
            {
                case TSqlTokenType.Dot:
                case TSqlTokenType.Colon:
                case TSqlTokenType.Comma:
                case TSqlTokenType.EqualsSign:
                    return true;

                default:
                    return false;
            }
        }

        private void FillMembers(CodeCompletionArgs e, Point position, string ident, string word, bool partial)
        {
            SqlWrapRepository repository = CompletionRepository as SqlWrapRepository;
            string[] idents = ident.Split(new char[] { '.' });
            string tableName = word;
            string fieldName = string.Empty;
            if (string.IsNullOrEmpty(repository.SchemaName))
            {
                tableName = idents.Count() > 0 ? idents[0] : tableName;
                fieldName = idents.Count() > 1 ? idents[1] : string.Empty;
            }
            else
            {
                tableName = idents.Count() > 1 ? idents[1] : tableName;
                fieldName = idents.Count() > 2 ? idents[2] : string.Empty;
            }

            bool fillFields = (!string.IsNullOrEmpty(fieldName) && partial && (string.Compare(idents[idents.Count() - 1], fieldName) == 0)) || (string.IsNullOrEmpty(fieldName) && !partial);
            bool fillTables = !fillFields && partial && (string.Compare(idents[idents.Count() - 1], tableName) == 0);

            if ((!string.IsNullOrEmpty(repository.SchemaName) && string.Compare(word, repository.SchemaName, !CaseSensitive) == 0) || fillTables)
            {
                e.Provider = new SqlWrapListMembers();
                repository.FillTableMembers(e.Provider as IListMembers);
            }
            else
            {
                if (fillFields)
                {
                    if (repository.FindTable(tableName) && fillFields)
                    {
                        e.Provider = new SqlWrapListMembers();
                        repository.FillTableFields(e.Provider as IListMembers, tableName, fieldName);
                    }
                }
            }
        }

        private ImageList LoadImageListFromStrip(string resourceName)
        {
            return ImageListHelper.LoadImageListFromStrip(typeof(NETRepository), "Alternet.Syntax.Parsers.Advanced.Images." + resourceName);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All support classes in the same unit")]
    public class SQLTokenizer : IUpdate
    {
        private TokenList tokens = new TokenList(0);
        private bool initialized = false;
        private int updateCount;

        public virtual TokenList Tokens
        {
            get
            {
                return tokens;
            }

            set
            {
                tokens = value;
                initialized = true;
            }
        }

        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        public int UpdateCount
        {
            get
            {
                return updateCount;
            }
        }

        public void ReparseSyntax(IList<TSqlParserToken> stream, int count)
        {
            if (updateCount > 0)
                return;

            BeginUpdate();
            try
            {
                TokenList tokenList = new TokenList(count);
                AddSyntaxTokens(tokenList, stream);
                Tokens = tokenList;
            }
            finally
            {
                EndUpdate();
            }
        }

        public void ParseText(int line, ref StringItemInfo[] colorData)
        {
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i].Data = 0;
                colorData[i].TextStyle = TextStyle.None;
            }

            if (tokens == null)
                return;
            IList<LineToken> lineTokens = tokens.GetTokens(line, false);

            if (lineTokens != null)
            {
                int len = colorData.Length;

                foreach (LineToken token in lineTokens)
                {
                    for (int i = token.Start; i < Math.Min(len, token.Start + token.Length); i++)
                        colorData[i].Data = (byte)(token.Style + 1);
                }
            }
        }

        public virtual TokenList GetTokens(IList<TSqlParserToken> stream, int count)
        {
            TokenList tokenList = new TokenList(count);
            AddSyntaxTokens(tokenList, stream);
            return tokenList;
        }

        public void PositionChanged(int x, int y, int deltaX, int deltaY, char ch)
        {
            if (tokens != null)
            {
                lock (tokens)
                {
                    tokens.PositionChanged(x, y, deltaX, deltaY, ch);
                }
            }
        }

        public int BeginUpdate()
        {
            updateCount++;
            return updateCount;
        }

        public int EndUpdate()
        {
            updateCount--;
            if (updateCount == 0)
                Update();
            return updateCount;
        }

        public int DisableUpdate()
        {
            updateCount++;
            return updateCount;
        }

        public int EnableUpdate()
        {
            updateCount--;
            return updateCount;
        }

        public void Update()
        {
        }

        protected virtual bool IsReswordToken(int token)
        {
            return token >= (int)TSqlTokenType.Add && token <= (int)TSqlTokenType.WriteText;
        }

        protected virtual int GetSyntaxToken(int kind)
        {
            if (IsReswordToken(kind))
                return (int)LexToken.Resword;
            else
                if ((kind >= (int)TSqlTokenType.Bang) && (kind <= (int)TSqlTokenType.BitwiseXorEquals))
                return (int)LexToken.Symbol;
            else
            {
                switch ((TSqlTokenType)kind)
                {
                    case TSqlTokenType.Numeric:
                        return (int)LexToken.Number;
                    case TSqlTokenType.AsciiStringLiteral:
                    case TSqlTokenType.UnicodeStringLiteral:
                    case TSqlTokenType.AsciiStringOrQuotedIdentifier:
                        return (int)LexToken.String;
                    case TSqlTokenType.Identifier:
                        return (int)LexToken.Identifier;
                    case TSqlTokenType.MultilineComment:
                    case TSqlTokenType.SingleLineComment:
                        return (int)LexToken.Comment;

                    default:
                        return (int)LexToken.Whitespace;
                }
            }
        }

        protected virtual void AddSyntaxTokens(TokenList tokens, IList<TSqlParserToken> stream)
        {
            foreach (TSqlParserToken token in stream)
            {
                AddSyntaxToken(tokens, token);
            }
        }

        protected void AddSyntaxToken(TokenList tokens, TSqlParserToken tok, int style)
        {
            IList<LineToken> lineTokens = tokens.GetTokens(tok.Line - 1, true);
            int start = tok.Column - 1;
            int end = start;
            if (!string.IsNullOrEmpty(tok.Text))
                end += tok.Text.Length;
            if (end > start)
                lineTokens.Add(new LineToken(tok.Line - 1, start, end - start, style));
        }

        protected void AddSyntaxToken(TokenList tokens, TSqlParserToken tok)
        {
            if (string.IsNullOrEmpty(tok.Text))
                return;
            int style = GetSyntaxToken((int)tok.TokenType);
            if (style >= 0)
                AddSyntaxToken(tokens, tok, style);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class LineToken
#pragma warning restore SA1402 // File may only contain a single type
    {
        public LineToken(int line, int start, int len, int style)
        {
            this.Line = line;
            this.Start = start;
            this.Length = len;
            this.Style = style;
        }

        public int Line { get; set; }

        public int Start { get; set; }

        public int Style { get; set; }

        public int Length { get; set; }

        public LineToken Clone()
        {
            return new LineToken(Line, Start, Length, Style);
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class TokenList : List<IList<LineToken>>
#pragma warning restore SA1402 // File may only contain a single type
    {
        public TokenList(int capacity)
        {
            this.Capacity = capacity;
        }

        public void PositionChangedY(int x, int y, int deltaY)
        {
            if (deltaY != 0 && y >= 0 && y < Count)
            {
                if (deltaY > 0)
                {
                    for (int i = 0; i < deltaY; i++)
                        Add(null);

                    for (int i = Count - 1; i >= y + deltaY; i--)
                        this[i] = this[i - deltaY];

                    for (int i = y + 1; i < y + deltaY; i++)
                        this[i] = null;

                    this[y] = CopyNextTokens(y, x);
                }
                else
                {
                    for (int i = y; i < Count + deltaY - 1; i++)
                    {
                        if (i == y)
                            this[i] = CopyPrevTokens(this[i], i - deltaY, x);
                        else
                            this[i] = this[i - deltaY];
                    }

                    for (int i = 0; i <= -deltaY; i++)
                    {
                        if (Count == 0)
                            break;
                        RemoveAt(Count - 1);
                    }
                }
            }
        }

        public void PositionChangedX(int x, int y, int deltaX, char ch)
        {
            if (deltaX != 0)
            {
                if (y >= 0 && y < Count)
                {
                    IList<LineToken> lineTokens = this[y];
                    if (lineTokens != null)
                    {
                        foreach (LineToken token in lineTokens)
                        {
                            if (token.Start >= x)
                            {
                                token.Start += deltaX;
                                token.Start = Math.Max(token.Start, 0);
                            }
                            else
                                if (deltaX < 0 && token.Start >= x + deltaX)
                            {
                                token.Length += deltaX;
                                token.Length = Math.Max(token.Length, 0);
                            }
                            else
                                    if (((token.Start + token.Length >= x) && (deltaX > 0)) || ((token.Start + token.Length > x) && (deltaX < 0)))
                            {
                                if (!((token.Start + token.Length == x) && (deltaX > 0) && IsStopChar(ch)))
                                {
                                    token.Length += (token.Length == int.MaxValue) ? 0 : deltaX;
                                    token.Length = Math.Max(token.Length, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PositionChanged(int x, int y, int deltaX, int deltaY, char ch)
        {
            if (y < 0 || x < 0)
                return;
            if (deltaY > 0)
            {
                PositionChangedY(x, y, deltaY);
                PositionChangedX(x, y + deltaY, deltaX, ch);
            }
            else
                if (deltaY < 0)
            {
                PositionChangedX(0, y - deltaY, deltaX, ch);
                PositionChangedY(x, y, deltaY);
            }
            else
                PositionChangedX(x, y, deltaX, ch);
        }

        public IList<LineToken> GetTokens(int index, bool add)
        {
            if (index < 0)
                return null;
            while (index >= Count)
                Add(null);
            IList<LineToken> result = this[index];
            if (result == null && add)
            {
                result = new List<LineToken>();
                this[index] = result;
            }

            return result;
        }

        private IList<LineToken> CopyNextTokens(int index, int start)
        {
            if (this[index] == null)
                return null;
            IList<LineToken> result = new List<LineToken>();

            foreach (LineToken token in this[index])
            {
                if (token.Start <= start)
                    result.Add(token.Clone());
            }

            return result;
        }

        private IList<LineToken> CopyPrevTokens(IList<LineToken> tokens, int index, int start)
        {
            IList<LineToken> result = tokens != null ? tokens : (this[index] != null ? new List<LineToken>() : null);
            if (result != null && this[index] != null)
            {
                foreach (LineToken token in this[index])
                {
                    if (token.Start >= start)
                        result.Add(token.Clone());
                }
            }

            return result;
        }

        private bool IsStopChar(char ch)
        {
            return (ch > 0) && (char.IsPunctuation(ch) || char.IsSeparator(ch));
        }
    }
}
