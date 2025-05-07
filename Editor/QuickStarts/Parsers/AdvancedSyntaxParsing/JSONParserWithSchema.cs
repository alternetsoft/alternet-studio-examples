using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Alternet.Common;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Advanced;
using NJsonSchema;
using NJsonSchema.Validation;

namespace AdvancedSyntaxParsing
{
    public class JSONParserWithSchema : JSONParser
    {
        private SyntaxError[] semanticErrors;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public JsonSchema Schema { get; set; }

        public override void ReparseText()
        {
            base.ReparseText();

            Validate();
        }

        public override int GetSyntaxErrors(IList<ISyntaxError> errors)
        {
            base.GetSyntaxErrors(errors);

            Validate();
            if (semanticErrors != null)
            {
                foreach (var err in semanticErrors)
                    errors.Add(err);
            }

            return errors.Count;
        }

        protected override void AssignParser(ISyntaxParser parser)
        {
            base.AssignParser(parser);
            ((JSONParserWithSchema)parser).Schema = Schema;
        }

        private void Validate()
        {
            semanticErrors = null;

            if (Schema == null || Strings.Count == 0)
                return;

            ICollection<ValidationError> errors;
            try
            {
                errors = Schema.Validate(Strings.Text);
            }
            catch
            {
                return;
            }

            semanticErrors = errors.Select(GetError).ToArray();
        }

        private IRange FindRange(string line, Point point)
        {
            int start = point.X;
            while ((start > 0 && char.IsLetterOrDigit(line[start - 1])) || line[start - 1] == '"')
                start--;

            int end = point.X;
            while ((end < line.Length && char.IsLetterOrDigit(line[end])) || line[start - 1] == '"')
                end++;

            return new Range(new Point(start, point.Y), new Point(end, point.Y));
        }

        private SyntaxError GetError(ValidationError input)
        {
            var lineIndex = input.HasLineInfo ? MathUtilities.Clamp(input.LineNumber - 1, 0, Strings.Count - 1) : 0;

            var line = Strings[lineIndex];
            var characterIndex = MathUtilities.Clamp(input.LinePosition - 1, 0, line.Length - 1);

            var error = new SyntaxError(new Point(characterIndex, lineIndex), string.Empty, input.ToString()) { ErrorType = SyntaxErrorType.Warning };

            error.Range = FindRange(line, error.Position);
            return error;
        }
    }
}