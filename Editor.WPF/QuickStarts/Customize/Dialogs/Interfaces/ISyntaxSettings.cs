#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.Drawing;
using System.Windows.Forms;

using Alternet.Syntax.Lexer;

namespace Alternet.Editor.Wpf
{
    #region ISyntaxSettings

    /// <summary>
    /// Represents methods to save/restore key properties for Edit control.
    /// </summary>
    public interface ISyntaxSettings : IPersistentSettings
    {
        /// <summary>
        /// When implemented by a class, occurs when user requests help for a control.
        /// </summary>
        event HelpEventHandler HelpRequested;

        /// <summary>
        /// When implemented by a class, gets or sets collection of lexical styles for the <c>Lexer</c> components.
        /// </summary>
        ILexStyles LexStyles
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets collection of default lexical styles.
        /// </summary>
        ILexStyle[] DefaultLexStyles
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a VisualThemes object.
        /// </summary>
        IVisualThemes VisualThemes
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets Font object for the <c>SyntaxEdit</c> controls.
        /// </summary>
        MediaFont Font
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets options for navigating within <c>SyntaxEdit</c> controls content.
        /// </summary>
        NavigateOptions NavigateOptions
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets the type of scroll bars to display in the <c>SyntaxEdit</c> controls.
        /// </summary>
        RichTextBoxScrollBars ScrollBars
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets options determining appearance and behavior of the <c>Selection</c> object in <c>SyntaxEdit</c> controls.
        /// </summary>
        SelectionOptions SelectionOptions
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets line separator options for SyntaxEdit controls.
        /// </summary>
        SeparatorOptions SeparatorOptions
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets outlining options for <c>SyntaxEdit</c> controls.
        /// </summary>
        OutlineOptions OutlineOptions
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether the <c>Margin</c> is visible in <c>SyntaxEdit</c> controls.
        /// </summary>
        bool ShowMargin
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether the <c>Gutter</c> is visible in <c>TextEditor</c> controls.
        /// </summary>
        bool ShowGutter
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a boolean value indicating whether TextEditor should display line numbers.
        /// </summary>
        bool ShowLineNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether urls in the <c>SyntaxEdit</c> controls text should be highlighted.
        /// </summary>
        bool HighlightHyperText
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether outlining is enabled for <c>SyntaxEdit</c> controls.
        /// </summary>
        bool AllowOutlining
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether indent operations insert space characters rather than TAB characters in <c>SyntaxEdit</c> controls.
        /// </summary>
        bool UseSpaces
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether <c>SyntaxEdit</c> controls automatically wrap words to the beginning of the next line when necessary.
        /// </summary>
        bool WordWrap
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets a sets a value indicating whether white space is visible.
        /// </summary>
        bool WhiteSpaceVisible
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets the width of the <c>Gutter</c> for <c>SyntaxEdit</c> controls.
        /// </summary>
        double GutterWidth
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets a sets a value indicating whether the horizontal scroll bar is visible.
        /// </summary>
        bool HorizontalScrollBarVisible { get; set; }

        /// <summary>
        /// When implemented by a class, gets a sets a value indicating whether the vertical scroll bar is visible.
        /// </summary>
        bool VerticalScrollBarVisible { get; set; }

        /// <summary>
        /// When implemented by a class, gets or sets value indicating position, in characters, of the vertical line within the text portion of the <c>SyntaxEdit</c> controls.
        /// </summary>
        int MarginPos
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets the character columns that the cursor will move to each time you press Tab in <c>SyntaxEdit</c> controls.
        /// </summary>
        int[] TabStops
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, represents array of event handlers associated with keys
        /// </summary>
        IKeyData[] EventData
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, changes values stored in the <c>ISyntaxSettings</c> accordingly to property values of <c>SyntaxEdit</c> control.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to copy properties from.</param>
        void LoadFromEdit(TextEditor edit);

        /// <summary>
        /// When implemented by a class, assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>ISyntaxSettings</c> instance.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        void ApplyToEdit(TextEditor edit);

        /// <summary>
        /// When implemented by a class, assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>ISyntaxSettings</c> instance.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        /// <param name="withStyles">Specifies that color styles should be copied</param>
        void ApplyToEdit(TextEditor edit, bool withStyles);

        /// <summary>
        /// When implemented by a class, indicates whether description for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if description is enabled; otherwise false.</returns>
        bool IsDescriptionEnabled(int index);

        /// <summary>
        /// When implemented by a class, indicates whether font style for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if font style is enabled; otherwise false.</returns>
        bool IsFontStyleEnabled(int index);

        /// <summary>
        /// When implemented by a class, indicates whether background color for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if background color is enabled; otherwise false.</returns>
        bool IsBackColorEnabled(int index);

        /// <summary>
        /// When implemented by a class, raises the HelpRequested event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A HelpEventArgs that contains the event data.</param>
        void OnHelpRequest(object sender, HelpEventArgs e);

        /// <summary>
        /// When implemented by a class, initializes lexical styles according with current culture.
        /// </summary>
        void Localize();
    }

    #endregion
}
