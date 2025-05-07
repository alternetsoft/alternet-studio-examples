using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllQuickStarts
{
    public static class DemoDescriptions
    {
        public const string RoslynSyntaxParsing =
            "This demo shows how to use the dedicated parsers for C# and VB.NET languages based on .NET Compiler Platform ('Roslyn'). These parsers support automatic code completion, code outlining, smart formatting, and syntax error underlining.";
        public const string AdvancedSyntaxParsing =
            "This demo shows how to link text edit controls to the advanced parsers for subset of programming languages such as C#, VB.NET, JavaScript, XML and others. These parsers support automatic code completion, code outlining, smart formatting and syntax error underlining.";
        public const string PowerFXSyntaxParsing =
            "This demo shows how to parser PowerFx language based on Microsoft PowerFx project. This parser supports automatic code completion, code outlining and syntax error underlining.";
        public const string XamlSyntaxParsing =
            "This demo shows how to link text edit controls to the XAML parser for the XAML language. These parsers support automatic code completion, code outlining, smart formatting and syntax error underlining.";
        public const string SqlDomSyntaxParsing =
            "This demo shows how to use Microsoft TSql100Parser parser to support automatic code completion, code outlining, smart formatting, and syntax error underlining.";
        public const string TextMateSyntaxParsing =
            "This demo shows how to use predefined syntax highlighting TextMate schemes for various languages.";

        public const string Miscellaneous
            = "Code Editor can display watermarks or background image. Can display white-space symbols such as spalies, tabs, end-of-line and the end-of-file markers. Supports highlighting of the matching braces. Spell-as-you-type spellchecker integration with thirt-party spelling engines is supported.";

        public const string VisualTheme
            = "Visual theme control appearance of all editor elements, using a predefined set of fonts and colors.";

        public const string SyntaxHighlighting
            = "This demo shows how to use predefined syntax highlighting schemes for various languages";

        public const string CodeOutlining
            = "Code outlining is a text navigation feature that can make navigation of large structured texts more comfortable and effective.";

        public const string Gutter
            = "The gutter area can be used to display additional information related to the text content.";

        public const string HyperText
            = "Code Editor can highlight hyperlinks displayed in the text and navigate through them.";

        public const string LineStyle
            = "Single line or continuous text range can be associated with the line style represented by visual indicator in gutter area and different background color.";

        public const string Margin
            = "Margin indicates a special column visually, while User Margin allows displaying custom information associated with the lines.";
    }
}
