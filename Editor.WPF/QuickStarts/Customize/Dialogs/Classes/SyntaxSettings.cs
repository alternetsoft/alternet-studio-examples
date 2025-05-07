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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Syntax.Lexer;

namespace Alternet.Editor.Wpf
{
    #region SyntaxSettings

    /// <summary>
    /// This class is designed to hold settings for <c>SyntaxEdit</c> object, allowing to synchronize key-properties for all Edit controls in application.
    /// </summary>
    public class SyntaxSettings : PersistentSettings, ISyntaxSettings
    {
        #region Private Fields

        private IVisualThemes visualThemes = new VisualThemes();
        private IKeyDataList eventDataList = new KeyDataList();
        private NavigateOptions navigateOptions;
        private RichTextBoxScrollBars scrollBars;
        private SelectionOptions selectionOptions;
        private bool horizontalScrollBarVisible = true;
        private bool verticalScrollBarVisible = true;
        private SeparatorOptions separatorOptions;
        private OutlineOptions outlineOptions;
        private StandardVisualTheme lightTheme = new LightVisualTheme();
        private StandardVisualTheme darkTheme = new DarkVisualTheme();
        private StandardVisualTheme customTheme = new CustomVisualTheme();
        private bool showMargin;
        private bool showGutter = true;
        private bool showLineNumbers = true;
        private bool highlightHyperText = true;
        private bool allowOutlining;
        private bool useSpaces;
        private bool wordWrap;
        private bool whiteSpaceVisible = false;
        private double gutterWidth = EditConsts.DefaultGutterWidth;
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
            separatorOptions = SeparatorOptions.None;
            outlineOptions = EditConsts.DefaultOutlineOptions;
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
        public virtual MediaFont Font
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
        /// Gets or sets a value indicating whether the <c>Gutter</c> is visible in <c>SyntaxEdit</c> controls.
        /// </summary>
        public virtual bool ShowLineNumbers
        {
            get
            {
                return showLineNumbers;
            }

            set
            {
                if (showLineNumbers != value)
                {
                    showLineNumbers = value;
                    OnShowLineNumbersChanged();
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
        public virtual double GutterWidth
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
        /// Gets or sets a value indicating whether horizontal scroll bar is visible.
        /// </summary>
        public virtual bool HorizontalScrollBarVisible
        {
            get
            {
                return horizontalScrollBarVisible;
            }

            set
            {
                if (horizontalScrollBarVisible != value)
                {
                    horizontalScrollBarVisible = value;
                    OnHorizontalScrollBarVisibleChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical scroll bar is visible.
        /// </summary>
        public virtual bool VerticalScrollBarVisible
        {
            get
            {
                return verticalScrollBarVisible;
            }

            set
            {
                if (verticalScrollBarVisible != value)
                {
                    verticalScrollBarVisible = value;
                    OnVerticalScrollBarVisibleChanged();
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
        public virtual IKeyDataList EventDataList
        {
            get
            {
                return eventDataList;
            }

            set
            {
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
                separatorOptions = src.separatorOptions;
                horizontalScrollBarVisible = src.horizontalScrollBarVisible;
                verticalScrollBarVisible = src.verticalScrollBarVisible;
                outlineOptions = src.outlineOptions;
                showMargin = src.showMargin;
                showGutter = src.showGutter;
                showLineNumbers = src.ShowLineNumbers;
                highlightHyperText = src.highlightHyperText;
                allowOutlining = src.allowOutlining;
                useSpaces = src.useSpaces;
                wordWrap = src.wordWrap;
                whiteSpaceVisible = src.whiteSpaceVisible;
                gutterWidth = src.gutterWidth;
                marginPos = src.marginPos;
                TabStops = src.TabStops;
                DefaultLexStyles = src.DefaultLexStyles;

                visualThemes.Assign(src.VisualThemes);

                eventNames = new string[src.EventNames.Length];
                src.EventNames.CopyTo(eventNames, 0);

                eventDataList = src.EventDataList;
                KeyList = src.KeyList;
            }
        }

        /// <summary>
        /// Changes values stored in the <c>SyntaxSettings</c> accordingly to values of <c>SyntaxEdit</c> control.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to copy properties from.</param>
        public virtual void LoadFromEdit(TextEditor edit)
        {
            SeparatorOptions = edit.LineSeparator.Options;
            GutterWidth = edit.GutterWidth;
            HighlightHyperText = edit.HyperText.HighlightHyperText;
            MarginPos = edit.EditMargin.Position;
            NavigateOptions = edit.NavigateOptions;
            AllowOutlining = edit.Outlining.AllowOutlining;
            OutlineOptions = edit.Outlining.OutlineOptions;
            HorizontalScrollBarVisible = (edit.HorizontalScrollBarVisibility & System.Windows.Controls.ScrollBarVisibility.Visible) != 0;
            VerticalScrollBarVisible = (edit.VerticalScrollBarVisibility & System.Windows.Controls.ScrollBarVisibility.Visible) != 0;
            SelectionOptions = edit.Selection.Options;
            ShowGutter = edit.GutterVisible;
            ShowLineNumbers = edit.LineNumbersVisible;
            ShowMargin = edit.EditMargin.Visible;
            TabStops = edit.Source.Lines.TabStops;
            UseSpaces = edit.Source.Lines.UseSpaces;
            WordWrap = edit.WordWrap;
            WhiteSpaceVisible = edit.WhitespaceVisible;
            EventNames = edit.KeyList.Handlers.EventNames;
            LoadEventData(edit.KeyList.EventData);
            KeyList = (KeyList)edit.KeyList;
            Font = new MediaFont(edit.FontFamily, edit.FontSize, edit.FontStretch, edit.FontStyle, edit.FontWeight);

            var theme = visualThemes.ActiveTheme;
            if (theme != null)
                theme.LoadFromEdit(edit);
        }

        /// <summary>
        /// Assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>SyntaxSettings</c> instance.
        /// </summary>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        public virtual void ApplyToEdit(TextEditor edit)
        {
            ApplyToEdit(edit, true);
        }

        /// <summary>
        /// Assigns key properties of given <c>SyntaxEdit</c> according to values stored in the <c>SyntaxSettings</c> instance.
        /// </summary>
        /// <param name="withStyles">Specifies that color styles should be copied</param>
        /// <param name="edit">Specifies <c>SyntaxEdit</c> to assign settings.</param>
        public virtual void ApplyToEdit(TextEditor edit, bool withStyles)
        {
            edit.Source.BeginUpdate(UpdateReason.Other);
            try
            {
                edit.Outlining.AllowOutlining = AllowOutlining;
                edit.Outlining.OutlineOptions = OutlineOptions;
                edit.LineSeparator.Options = SeparatorOptions;
                edit.GutterWidth = GutterWidth;
                edit.EditMargin.Position = MarginPos;
                edit.NavigateOptions = NavigateOptions;
                edit.HorizontalScrollBarVisibility = HorizontalScrollBarVisible ? System.Windows.Controls.ScrollBarVisibility.Visible : System.Windows.Controls.ScrollBarVisibility.Hidden;
                edit.VerticalScrollBarVisibility = VerticalScrollBarVisible ? System.Windows.Controls.ScrollBarVisibility.Visible : System.Windows.Controls.ScrollBarVisibility.Hidden;
                edit.Selection.Options = SelectionOptions;
                edit.GutterVisible = ShowGutter;
                edit.EditMargin.Visible = ShowMargin;
                edit.Source.Lines.TabStops = TabStops;
                edit.Source.Lines.UseSpaces = UseSpaces;
                edit.WordWrap = WordWrap;
                edit.WhitespaceVisible = WhiteSpaceVisible;
                edit.LineNumbersVisible = ShowLineNumbers;
                edit.FontFamily = Font.Family;
                edit.FontSize = Font.Size;
                edit.FontStretch = Font.Stretch;
                edit.FontStyle = Font.Style;
                edit.FontWeight = Font.Weight;
                edit.HyperText.HighlightHyperText = HighlightHyperText;
                ApplyEventData(edit.KeyList, EventDataList);

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

        protected virtual void OnShowLineNumbersChanged()
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

        protected virtual void OnHorizontalScrollBarVisibleChanged()
        {
        }

        protected virtual void OnVerticalScrollBarVisibleChanged()
        {
        }

        protected virtual void OnEventNamesChanged()
        {
        }

        protected virtual void ApplyEventData(IKeyList keyList, IKeyDataList keyDataList)
        {
            IKeyData FindKeyData(IList<IKeyData> list, IKeyData keyData)
            {
                foreach (var data in list)
                {
                    if (data.EventName == keyData.EventName && data.State == keyData.State && data.Keys == keyData.Keys)
                        return data;
                    if (string.IsNullOrEmpty(data.EventName) && string.IsNullOrEmpty(keyData.EventName) && data.State == keyData.State && data.Keys == keyData.Keys)
                        return data;
                }

                return null;
            }

            IKeyData FindBestCandidate(IList<IKeyData> list, IKeyData keyData)
            {
                foreach (var data in list)
                {
                    if (data.EventName == keyData.EventName && data.State == keyData.State && data.Keys != keyData.Keys)
                        return data;
                }

                return null;
            }

            IDictionary<IKeyData, IKeyData> eventsToUpdate = new Dictionary<IKeyData, IKeyData>();

            IList<IKeyData> oldList = new List<IKeyData>();
            IList<IKeyData> newList = new List<IKeyData>();

            foreach (var keyData in keyList.EventData)
                oldList.Add(keyData);

            foreach (var keyData in keyDataList)
                newList.Add(keyData);

            for (int i = oldList.Count - 1; i >= 0; i--)
            {
                var newData = FindKeyData(keyDataList, oldList[i]);
                if (newData != null)
                {
                    oldList.RemoveAt(i);
                    newList.Remove(newData);
                }
            }

            foreach (var keyData in oldList)
            {
                eventsToUpdate.Add(keyData, FindBestCandidate(newList, keyData));
            }

            foreach (var key in eventsToUpdate.Keys)
            {
                var keyData = eventsToUpdate[key];
                if (keyData != null)
                {
                    keyList.Remove(key.Keys, key.State);
                    if (key.ActionEx != null)
                        keyList.Add(keyData.Keys, key.ActionEx, key.Param, key.State, key.LeaveState);
                    else
                        keyList.Add(keyData.Keys, key.Action, key.State, key.LeaveState);
                }
            }
        }

        protected virtual void LoadEventData(IKeyData[] keyDates)
        {
            eventDataList.Clear();
            foreach (var data in keyDates)
                eventDataList.Add(data);
        }

        #endregion

        #endregion
    }
    #endregion
}
