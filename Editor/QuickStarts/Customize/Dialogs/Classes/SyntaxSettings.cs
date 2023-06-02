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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Dialogs;
using Alternet.Editor.TextSource;
using Alternet.Syntax.Lexer;
using Customize.Serialization;

namespace Customize.Dialogs
{
    #region SyntaxSettings

    /// <summary>
    /// This class is designed to hold settings for <c>SyntaxEdit</c> object, allowing to synchronize key-properties for all Edit controls in application.
    /// </summary>
    public class SyntaxSettings : PersistentSettings, ISyntaxSettings
    {
        #region Private Fields

        private IVisualThemes visualThemes = new VisualThemes();
        private IKeyData[] eventData = new IKeyData[] { };
        private NavigateOptions navigateOptions;
        private RichTextBoxScrollBars scrollBars;
        private SelectionOptions selectionOptions;
        private GutterOptions gutterOptions;
        private SeparatorOptions separatorOptions;
        private OutlineOptions outlineOptions;
        private StandardVisualTheme lightTheme = new LightVisualTheme();
        private StandardVisualTheme darkTheme = new DarkVisualTheme();
        private StandardVisualTheme customTheme = new CustomVisualTheme();
        private bool showMargin;
        private bool showGutter = true;
        private bool highlightHyperText = true;
        private bool allowOutlining;
        private bool useSpaces;
        private bool wordWrap;
        private bool whiteSpaceVisible = false;
        private PageType pageType;
        private int gutterWidth = EditConsts.DefaultGutterWidth;
        private int marginPos = EditConsts.DefaultMarginPosition;
        private int[] tabStops = { EditConsts.DefaultTabStop };
        private string[] eventNames = new string[] { };
        private KeyList keyList = null;
        private int ccolorStyles = -1;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <c>SyntaxSettings</c> class with default settings.
        /// </summary>
        public SyntaxSettings()
        {
            navigateOptions = EditConsts.DefaultNavigateOptions;
            scrollBars = RichTextBoxScrollBars.Both;
            selectionOptions = EditConsts.DefaultSelectionOptions;
            gutterOptions = EditConsts.DefaultGutterOptions;
            separatorOptions = SeparatorOptions.None;
            outlineOptions = EditConsts.DefaultOutlineOptions;
            gutterOptions |= EditConsts.DefaultGutterOptions;
            visualThemes.Add(lightTheme);
            visualThemes.Add(darkTheme);
            visualThemes.Add(customTheme);
            ccolorStyles = lightTheme.LexStyles.Count;
            visualThemes.ActiveThemeIndex = 0;
        }

        #region ISyntaxSettings Members

        /// <summary>
        /// Occurs when user requests help for a control.
        /// </summary>
        public virtual event HelpEventHandler HelpRequested
        {
            add
            {
                helpRequested += value;
            }

            remove
            {
                helpRequested -= value;
            }
        }

#pragma warning disable SA1300 // Element should begin with upper-case letter
        private event HelpEventHandler helpRequested;
#pragma warning restore SA1300 // Element should begin with upper-case letter

        /// <summary>
        /// Gets Default IVisualTheme instance.
        /// </summary>
        public virtual IVisualTheme DefaultVisualTheme
        {
            get
            {
                return visualThemes[0];
            }
        }

        /// <summary>
        /// Gets Dark IVisualTheme instance.
        /// </summary>
        public virtual IVisualTheme DarkVisualTheme
        {
            get
            {
                return visualThemes.Count > 1 ? visualThemes[1] : null;
            }
        }

        /// <summary>
        /// Gets or sets collection of lexical styles for the <c>Lexer</c> components.
        /// </summary>
        public virtual ILexStyles LexStyles
        {
            get
            {
                return (visualThemes.ActiveTheme != null) ? visualThemes.ActiveTheme.LexStyles : null;
            }

            set
            {
                if (visualThemes.ActiveTheme != null)
                    visualThemes.ActiveTheme.LexStyles = value;
            }
        }

        /// <summary>
        /// When implemented by a class, gets or sets collection of default lexical styles.
        /// </summary>
        public virtual ILexStyle[] DefaultLexStyles
        {
            get
            {
                ILexStyle[] result = new LexStyle[lightTheme.LexStyles.Count];
                lightTheme.LexStyles.CopyTo(result, 0);
                return result;
            }

            set
            {
                lightTheme.LexStyles.Clear();
                foreach (ILexStyle style in value)
                    lightTheme.LexStyles.Add(style);
                DefaultVisualTheme.LexStyles = lightTheme.LexStyles;
            }
        }

        /// <summary>
        /// Gets or sets the VisualThemes object.
        /// </summary>
        public virtual IVisualThemes VisualThemes
        {
            get
            {
                return visualThemes;
            }

            set
            {
                if (visualThemes != value)
                {
                    visualThemes = value;
                    OnVisualThemesChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets Font object for the <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual Font Font
        {
            get
            {
                return visualThemes.ActiveTheme != null ? visualThemes.ActiveTheme.Font : null;
            }

            set
            {
                if (visualThemes.ActiveTheme != null)
                    visualThemes.ActiveTheme.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets options for navigating within <c>SyntaxEdit</c> controls content.
        /// </summary>
        public virtual NavigateOptions NavigateOptions
        {
            get
            {
                return navigateOptions;
            }

            set
            {
                if (navigateOptions != value)
                {
                    navigateOptions = value;
                    OnNavigateOptionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of scroll bars to display in the <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return scrollBars;
            }

            set
            {
                if (scrollBars != value)
                {
                    scrollBars = value;
                    OnScrollBarsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets options determining appearance and behavior of the <c>Selection</c> object in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual SelectionOptions SelectionOptions
        {
            get
            {
                return selectionOptions;
            }

            set
            {
                if (selectionOptions != value)
                {
                    selectionOptions = value;
                    OnSelectionOptionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a gutter options that determines <c>Gutter</c> appearance and behavior for <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual GutterOptions GutterOptions
        {
            get
            {
                return gutterOptions;
            }

            set
            {
                if (gutterOptions != value)
                {
                    gutterOptions = value;
                    OnGutterOptionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets line separator options for SyntaxEdit controls.
        /// </summary>
        public virtual SeparatorOptions SeparatorOptions
        {
            get
            {
                return separatorOptions;
            }

            set
            {
                if (separatorOptions != value)
                {
                    separatorOptions = value;
                    OnSeparatorOptionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets outlining options for <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual OutlineOptions OutlineOptions
        {
            get
            {
                return outlineOptions;
            }

            set
            {
                if (outlineOptions != value)
                {
                    outlineOptions = value;
                    OnOutlineOptionsChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>Margin</c> is visible in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual bool ShowMargin
        {
            get
            {
                return showMargin;
            }

            set
            {
                if (showMargin != value)
                {
                    showMargin = value;
                    OnShowMarginChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>Gutter</c> is visible in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual bool ShowGutter
        {
            get
            {
                return showGutter;
            }

            set
            {
                if (showGutter != value)
                {
                    showGutter = value;
                    OnShowGutterChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the <c>SyntaxEdit</c> controls text should be highlighted.
        /// </summary>
        public virtual bool HighlightHyperText
        {
            get
            {
                return highlightHyperText;
            }

            set
            {
                if (highlightHyperText != value)
                {
                    highlightHyperText = value;
                    OnHighlightHyperTextChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether outlining is enabled for <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual bool AllowOutlining
        {
            get
            {
                return allowOutlining;
            }

            set
            {
                if (allowOutlining != value)
                {
                    allowOutlining = value;
                    OnAllowOutliningChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indent operations insert space characters rather than TAB characters in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual bool UseSpaces
        {
            get
            {
                return useSpaces;
            }

            set
            {
                if (useSpaces != value)
                {
                    useSpaces = value;
                    OnUseSpacesChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <c>SyntaxEdit</c> controls automatically wrap words to the beginning of the next line when necessary.
        /// </summary>
        public virtual bool WordWrap
        {
            get
            {
                return wordWrap;
            }

            set
            {
                if (wordWrap != value)
                {
                    wordWrap = value;
                    OnWordWrapChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether white space is visible.
        /// </summary>
        public virtual bool WhiteSpaceVisible
        {
            get
            {
                return whiteSpaceVisible;
            }

            set
            {
                if (whiteSpaceVisible != value)
                {
                    whiteSpaceVisible = value;
                    OnWhiteSpaceVisibleChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the <c>Gutter</c> for <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual int GutterWidth
        {
            get
            {
                return gutterWidth;
            }

            set
            {
                if (gutterWidth != value)
                {
                    gutterWidth = value;
                    OnGutterWidthChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets value indicating position, in characters, of the vertical line within the text portion of the <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual int MarginPos
        {
            get
            {
                return marginPos;
            }

            set
            {
                if (marginPos != value)
                {
                    marginPos = value;
                    OnMarginPosChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the character columns that the cursor will move to each time you press Tab in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual int[] TabStops
        {
            get
            {
                return tabStops;
            }

            set
            {
                if (tabStops != value)
                {
                    tabStops = new int[value.Length];
                    Array.Copy(value, tabStops, value.Length);
                    OnTabStopsChanged();
                }
            }
        }

        /// <summary>
        /// Represents names of all available event handlers.
        /// </summary>
        public virtual string[] EventNames
        {
            get
            {
                return eventNames;
            }

            set
            {
                if (eventNames != value)
                {
                    eventNames = value;
                    OnEventNamesChanged();
                }
            }
        }

        /// <summary>
        /// Represents array of event handlers associated with keys
        /// </summary>
        public virtual IKeyData[] EventData
        {
            get
            {
                return eventData;
            }

            set
            {
                if (eventData != value)
                {
                    eventData = value;
                    OnEventDataChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets value specifying the way of viewing <c>SyntaxEdit</c> control's content.
        /// </summary>
        public PageType PageType
        {
            get
            {
                return pageType;
            }

            set
            {
                pageType = value;
            }
        }

        /// <summary>
        /// Represents the SyntaxEdit object's KeyList property.
        /// </summary>
        public KeyList KeyList
        {
            get
            {
                return keyList;
            }

            set
            {
                keyList = value;
            }
        }

        // Xml serialization

        /// <summary>
        /// Gets or sets an xml representation of this <c>SyntaxSettings</c> object.
        /// </summary>
        /// <remarks>Normally, you do not need to use this property directly. It's used internally when serializing Editor's content to XML.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlSyntaxSettingsInfo(this);
            }

            set
            {
                value.FixupReferences(this);
            }
        }

        #endregion

        #region SyntaxSettings Members

        #region Public Methods

        /// <summary>
        /// Copies the content from another <c>IPersistentSettings</c> object.
        /// </summary>
        /// <param name="source">Specifies <c>IPersistentSettings</c> to assign.</param>
        public override void Assign(IPersistentSettings source)
        {
            if (source is SyntaxSettings)
            {
                SyntaxSettings src = (SyntaxSettings)source;
                navigateOptions = src.navigateOptions;
                scrollBars = src.scrollBars;
                selectionOptions = src.selectionOptions;
                gutterOptions = src.gutterOptions;
                separatorOptions = src.separatorOptions;
                outlineOptions = src.outlineOptions;
                showMargin = src.showMargin;
                showGutter = src.showGutter;
                highlightHyperText = src.highlightHyperText;
                allowOutlining = src.allowOutlining;
                useSpaces = src.useSpaces;
                wordWrap = src.wordWrap;
                whiteSpaceVisible = src.whiteSpaceVisible;
                pageType = src.pageType;
                gutterWidth = src.gutterWidth;
                marginPos = src.marginPos;
                TabStops = src.TabStops;
                DefaultLexStyles = src.DefaultLexStyles;

                visualThemes.Assign(src.VisualThemes);

                eventNames = new string[src.EventNames.Length];
                src.EventNames.CopyTo(eventNames, 0);

                eventData = new IKeyData[src.EventData.Length];
                src.EventData.CopyTo(eventData, 0);

                KeyList = src.KeyList;
            }
        }

        /// <summary>
        /// Changes values stored in the <c>SyntaxSettings</c> accordingly to values of <c>SyntaxEdit</c> control.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to copy properties from.</param>
        public virtual void LoadFromEdit(ISyntaxEdit edit)
        {
            GutterOptions = edit.Gutter.Options;
            SeparatorOptions = edit.LineSeparator.Options;
            GutterWidth = edit.Gutter.Width;
            HighlightHyperText = edit.HyperText.HighlightHyperText;
            MarginPos = edit.EditMargin.Position;
            NavigateOptions = edit.NavigateOptions;
            AllowOutlining = edit.Outlining.AllowOutlining;
            OutlineOptions = edit.Outlining.OutlineOptions;
            ScrollBars = edit.Scrolling.ScrollBars;
            SelectionOptions = edit.Selection.Options;
            ShowGutter = edit.Gutter.Visible;
            ShowMargin = edit.EditMargin.Visible;
            TabStops = edit.Source.Lines.TabStops;
            UseSpaces = edit.Source.Lines.UseSpaces;
            WordWrap = edit.WordWrap;
            WhiteSpaceVisible = edit.WhiteSpace.Visible;
            PageType = edit.Pages.PageType;
            EventNames = edit.KeyList.Handlers.EventNames;
            EventData = edit.KeyList.EventData;
            KeyList = (KeyList)edit.KeyList;

            Font = edit.Font;
            var theme = visualThemes.ActiveTheme;
            if (theme != null)
                theme.LoadFromEdit(edit);
        }

        /// <summary>
        /// Assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>SyntaxSettings</c> instance.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        public virtual void ApplyToEdit(ISyntaxEdit edit)
        {
            ApplyToEdit(edit, true);
        }

        /// <summary>
        /// Assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>SyntaxSettings</c> instance.
        /// </summary>
        /// <param name="withStyles">Specifies that color styles should be copied</param>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        public virtual void ApplyToEdit(ISyntaxEdit edit, bool withStyles)
        {
            edit.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                edit.Outlining.AllowOutlining = AllowOutlining;
                edit.Outlining.OutlineOptions = OutlineOptions;
                edit.Font = Font;
                edit.Gutter.Options = GutterOptions;
                edit.LineSeparator.Options = SeparatorOptions;
                edit.Gutter.Width = GutterWidth;
                edit.EditMargin.Position = MarginPos;
                edit.NavigateOptions = NavigateOptions;
                edit.Scrolling.ScrollBars = ScrollBars;
                edit.Selection.Options = SelectionOptions;
                edit.Gutter.Visible = ShowGutter;
                edit.EditMargin.Visible = ShowMargin;
                edit.Source.Lines.TabStops = TabStops;
                edit.Source.Lines.UseSpaces = UseSpaces;
                edit.WordWrap = WordWrap;
                edit.WhiteSpace.Visible = WhiteSpaceVisible;
                edit.Pages.PageType = PageType;

                edit.HyperText.HighlightHyperText = HighlightHyperText;
                IVisualTheme theme = visualThemes.ActiveTheme;
                if (withStyles && theme != null)
                    edit.ApplyTheme(theme);
            }
            finally
            {
                edit.Source.EndUpdate();
            }

            edit.Invalidate();
        }

        /// <summary>
        /// Raises the HelpRequested event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A HelpEventArgs that contains the event data.</param>
        public void OnHelpRequest(object sender, HelpEventArgs e)
        {
            if (helpRequested != null)
                helpRequested(this, e);
        }

        /// <summary
        /// Indicates whether description for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if description is enabled; otherwise false.</returns>
        public virtual bool IsDescriptionEnabled(int index)
        {
            return (index >= 0) && (visualThemes.ActiveTheme != null) && (visualThemes.ActiveTheme.LexStyles[index].Desc != string.Empty);
        }

        /// <summary>
        /// Indicates whether font style for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if font style is enabled; otherwise false.</returns>
        public virtual bool IsFontStyleEnabled(int index)
        {
            return (index >= 0) && (index < ccolorStyles);
        }

        /// <summary>
        /// Indicates whether background color for specified lexical style is enabled.
        /// </summary>
        /// <param name="index">Specifies index of lexical style to check-up.</param>
        /// <returns>True if background color is enabled; otherwise false.</returns>
        public virtual bool IsBackColorEnabled(int index)
        {
            return (index >= 0) && (visualThemes.ActiveTheme != null) && visualThemes.ActiveTheme.LexStyles[index].BackColorEnabled;
        }

        /// <summary>
        /// Initializes lexical styles according with current culture.
        /// </summary>
        public virtual void Localize()
        {
        }

        /// <summary>
        /// Returns Type object for a class that contain control's settings information. In this class method returns type of <c>XmlSyntaxSettingsInfo</c> class.
        /// </summary>
        public override Type GetXmlType()
        {
            return typeof(XmlSyntaxSettingsInfo);
        }

        #endregion

        #region Protected Methods

        protected virtual void OnVisualThemesChanged()
        {
        }

        protected virtual void OnNavigateOptionsChanged()
        {
        }

        protected virtual void OnScrollBarsChanged()
        {
        }

        protected virtual void OnSelectionOptionsChanged()
        {
        }

        protected virtual void OnGutterOptionsChanged()
        {
        }

        protected virtual void OnSeparatorOptionsChanged()
        {
        }

        protected virtual void OnOutlineOptionsChanged()
        {
        }

        protected virtual void OnShowMarginChanged()
        {
        }

        protected virtual void OnShowGutterChanged()
        {
        }

        protected virtual void OnHighlightHyperTextChanged()
        {
        }

        protected virtual void OnAllowOutliningChanged()
        {
        }

        protected virtual void OnUseSpacesChanged()
        {
        }

        protected virtual void OnWordWrapChanged()
        {
        }

        protected virtual void OnWhiteSpaceVisibleChanged()
        {
        }

        protected virtual void OnGutterWidthChanged()
        {
        }

        protected virtual void OnMarginPosChanged()
        {
        }

        protected virtual void OnTabStopsChanged()
        {
        }

        protected virtual void OnEventNamesChanged()
        {
        }

        protected virtual void OnEventDataChanged()
        {
        }
        #endregion

        #endregion
    }
    #endregion
}
