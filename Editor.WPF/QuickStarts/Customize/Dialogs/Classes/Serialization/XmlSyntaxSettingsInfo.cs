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
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Wpf.Serialization;

namespace Alternet.Editor.Wpf
{
    #region XmlSyntaxSettingsInfo

    /// <summary>
    /// Contains information about <c>ISyntaxSettings</c> object's settings.
    /// </summary>
    public class XmlSyntaxSettingsInfo : ISerializationInfo
    {
        #region Private Members

        private ISyntaxSettings owner;
        private MediaFont font = new MediaFont(new FontFamily(System.Drawing.FontFamily.GenericMonospace.Name), (double)new FontSizeConverter().ConvertFrom("10pt"), FontStretches.Normal, FontStyles.Normal, FontWeights.Normal);
        private string fontName = System.Drawing.FontFamily.GenericMonospace.Name;
        private double fontSize = (double)new FontSizeConverter().ConvertFrom("10pt");
        private NavigateOptions navigateOptions = EditConsts.DefaultNavigateOptions;
        private RichTextBoxScrollBars scrollBars = RichTextBoxScrollBars.Both;
        private SelectionOptions selectionOptions = EditConsts.DefaultSelectionOptions;
        private OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;
        private bool showGutter = true;
        private bool horizontalScrollBarVisible = true;
        private bool verticalScrollBarVisible = true;
        private bool showMargin = true;
        private bool highlightHyperText = true;
        private bool allowOutlining;
        private bool useSpaces;
        private bool wordWrap;
        private double gutterWidth = EditConsts.DefaultGutterWidth;
        private int marginPos = EditConsts.DefaultMarginPosition;
        private int activeThemeIndex = 0;
        private int[] tabStops = { EditConsts.DefaultTabStop };
        private IKeyData[] eventData = new KeyData[] { };
        private XmlVisualThemesInfo colorThemes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>XmlSyntaxSettingsInfo</c> class with default settings.
        /// </summary>
        public XmlSyntaxSettingsInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>XmlSyntaxSettingsInfo</c> class with specified owner.
        /// </summary>
        /// <param name="owner">Specifies <c>ISyntaxSettings</c> object to store settings.</param>
        public XmlSyntaxSettingsInfo(ISyntaxSettings owner)
            : this()
        {
            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores <c>ISyntaxSettings.Font</c> property.
        /// </summary>
        [XmlIgnoreAttribute]
        public MediaFont Font
        {
            get
            {
                return (owner != null) ? owner.Font : font;
            }

            set
            {
                font = value;
                if (owner != null)
                    owner.Font = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>ISyntaxSettings.Font</c> property.
        /// </summary>
        public string FontName
        {
            get
            {
                return (owner != null) ? owner.Font.Name : fontName;
            }

            set
            {
                fontName = value;
            }
        }

        /// <summary>
        /// Stores size of the <c>ISyntaxSettings.Font</c> property.
        /// </summary>
        public double FontSize
        {
            get
            {
                return (owner != null) ? owner.Font.Size : fontSize;
            }

            set
            {
                fontSize = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.NavigateOptions</c> property.
        /// </summary>
        public NavigateOptions NavOptions
        {
            get
            {
                return (owner != null) ? owner.NavigateOptions : navigateOptions;
            }

            set
            {
                navigateOptions = value;
                if (owner != null)
                    owner.NavigateOptions = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.ScrollBars</c> property.
        /// </summary>
        [DefaultValue(RichTextBoxScrollBars.Both)]
        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return (owner != null) ? owner.ScrollBars : scrollBars;
            }

            set
            {
                scrollBars = value;
                if (owner != null)
                    owner.ScrollBars = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.SelectionOptions</c> property.
        /// </summary>
        public SelectionOptions SelOptions
        {
            get
            {
                return (owner != null) ? owner.SelectionOptions : selectionOptions;
            }

            set
            {
                selectionOptions = value;
                if (owner != null)
                    owner.SelectionOptions = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.OutlineOptions</c> property.
        /// </summary>
        public OutlineOptions OutlineOptions
        {
            get
            {
                return (owner != null) ? owner.OutlineOptions : outlineOptions;
            }

            set
            {
                outlineOptions = value;
                if (owner != null)
                    owner.OutlineOptions = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.ShowGutter</c> property.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowGutter
        {
            get
            {
                return (owner != null) ? owner.ShowGutter : showGutter;
            }

            set
            {
                showGutter = value;
                if (owner != null)
                    owner.ShowGutter = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.ShowMargin</c> property.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowMargin
        {
            get
            {
                return (owner != null) ? owner.ShowMargin : showMargin;
            }

            set
            {
                showMargin = value;
                if (owner != null)
                    owner.ShowMargin = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.HighlightUrls</c> property.
        /// </summary>
        [DefaultValue(false)]
        public bool HighlightHyperText
        {
            get
            {
                return (owner != null) ? owner.HighlightHyperText : highlightHyperText;
            }

            set
            {
                highlightHyperText = value;
                if (owner != null)
                    owner.HighlightHyperText = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.AllowOutlining</c> property.
        /// </summary>
        [DefaultValue(false)]
        public bool AllowOutlining
        {
            get
            {
                return (owner != null) ? owner.AllowOutlining : allowOutlining;
            }

            set
            {
                allowOutlining = value;
                if (owner != null)
                    owner.AllowOutlining = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.UseSpaces</c> property.
        /// </summary>
        [DefaultValue(false)]
        public bool UseSpaces
        {
            get
            {
                return (owner != null) ? owner.UseSpaces : useSpaces;
            }

            set
            {
                useSpaces = value;
                if (owner != null)
                    owner.UseSpaces = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.WordWrap</c> property.
        /// </summary>
        [DefaultValue(false)]
        public bool WordWrap
        {
            get
            {
                return (owner != null) ? owner.WordWrap : wordWrap;
            }

            set
            {
                wordWrap = value;
                if (owner != null)
                    owner.WordWrap = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.GutterWidth</c> property.
        /// </summary>
        public double GutterWidth
        {
            get
            {
                return (owner != null) ? owner.GutterWidth : gutterWidth;
            }

            set
            {
                gutterWidth = value;
                if (owner != null)
                    owner.GutterWidth = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.HorizontalScrollBarVisible</c> property.
        /// </summary>
        public bool HorizontalScrollBarVisible
        {
            get
            {
                return (owner != null) ? owner.HorizontalScrollBarVisible : horizontalScrollBarVisible;
            }

            set
            {
                horizontalScrollBarVisible = value;
                if (owner != null)
                    owner.HorizontalScrollBarVisible = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.VerticalScrollBarVisible</c> property.
        /// </summary>
        public bool VerticalScrollBarVisible
        {
            get
            {
                return (owner != null) ? owner.VerticalScrollBarVisible : verticalScrollBarVisible;
            }

            set
            {
                verticalScrollBarVisible = value;
                if (owner != null)
                    owner.VerticalScrollBarVisible = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.MarginPos</c> property.
        /// </summary>
        public int MarginPos
        {
            get
            {
                return (owner != null) ? owner.MarginPos : marginPos;
            }

            set
            {
                marginPos = value;
                if (owner != null)
                    owner.MarginPos = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.TabStops</c> property.
        /// </summary>
        public int[] TabStops
        {
            get
            {
                return (owner != null) ? owner.TabStops : tabStops;
            }

            set
            {
                tabStops = value;
                if (owner != null)
                {
                    owner.TabStops = new int[value.Length];
                    Array.Copy(value, owner.TabStops, value.Length);
                }
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.VisualThemes</c> property.
        /// </summary>
        public XmlVisualThemesInfo VisualThemes
        {
            get
            {
                return (owner != null) ? (XmlVisualThemesInfo)owner.VisualThemes.SerializationInfo : colorThemes;
            }

            set
            {
                colorThemes = value;
                if (owner != null && value != null)
                    owner.VisualThemes.SerializationInfo = value;
            }
        }

        /// <summary>
        /// Stores <c>ISyntaxSettings.GutterWidth</c> property.
        /// </summary>
        public int ActiveThemeIndex
        {
            get
            {
                return (owner != null) ? owner.VisualThemes.ActiveThemeIndex : activeThemeIndex;
            }

            set
            {
                activeThemeIndex = value;
                if (owner != null)
                    owner.VisualThemes.ActiveThemeIndex = value;
            }
        }

        /// <summary>
        /// Stores <c>EventHandlers</c> property.
        /// </summary>
        [XmlIgnoreAttribute]
        public IKeyData[] EventData
        {
            get
            {
                return (owner != null) ? owner.EventData : eventData;
            }

            set
            {
                eventData = value;
                if (owner != null && value.Length > 0)
                    owner.EventData = value;
            }
        }

        #endregion

        #region ISerializationInfo Members

        /// <summary>
        /// Updates properties of external object with stored ones.
        /// </summary>
        /// <param name="owner">Specifies external object</param>
        public virtual void FixupReferences(object owner)
        {
            this.owner = (ISyntaxSettings)owner;

            NavOptions = navigateOptions;
            ScrollBars = scrollBars;
            SelOptions = selectionOptions;
            HorizontalScrollBarVisible = horizontalScrollBarVisible;
            VerticalScrollBarVisible = verticalScrollBarVisible;
            OutlineOptions = outlineOptions;
            ShowGutter = showGutter;
            ShowMargin = showMargin;
            HighlightHyperText = highlightHyperText;
            AllowOutlining = allowOutlining;
            UseSpaces = useSpaces;
            WordWrap = wordWrap;
            GutterWidth = gutterWidth;
            MarginPos = marginPos;
            TabStops = tabStops;
            VisualThemes = colorThemes;
            Font = new MediaFont(new FontFamily(fontName), fontSize, FontStyles.Normal);
            ActiveThemeIndex = activeThemeIndex;
            EventData = eventData;
        }

        /// <summary>
        /// Reads property values from external object. Reserved for internal use.
        /// </summary>
        public virtual void Load()
        {
            if (owner == null)
                return;
            fontName = Font.Name;
            fontSize = Font.Size;
            navigateOptions = NavOptions;
            scrollBars = ScrollBars;
            selectionOptions = SelOptions;
            horizontalScrollBarVisible = HorizontalScrollBarVisible;
            verticalScrollBarVisible = VerticalScrollBarVisible;
            outlineOptions = OutlineOptions;
            showGutter = ShowGutter;
            showMargin = ShowMargin;
            highlightHyperText = HighlightHyperText;
            allowOutlining = AllowOutlining;
            useSpaces = UseSpaces;
            wordWrap = WordWrap;
            gutterWidth = GutterWidth;
            marginPos = MarginPos;
            tabStops = TabStops;
            colorThemes = VisualThemes;
            activeThemeIndex = ActiveThemeIndex;
            eventData = EventData;
            foreach (XmlMacroKeyDataInfo data in eventData)
            {
                data.Load();
            }
        }

        #endregion

        #region XmlSyntaxSettingsInfo Members

        /// <summary>
        /// Indicates whether the <c>FontName</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>FontName</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeFontName()
        {
            return FontName != System.Drawing.FontFamily.GenericMonospace.Name;
        }

        /// <summary>
        /// Indicates whether the <c>FontSize</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>FontSize</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeFontSize()
        {
            return FontSize != 10.0;
        }

        /// <summary>
        /// Indicates whether the <c>NavOptions</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>NavOptions</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeNavOptions()
        {
            return NavOptions != EditConsts.DefaultNavigateOptions;
        }

        /// <summary>
        /// Indicates whether the <c>Options</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>Options</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeSelOptions()
        {
            return SelOptions != EditConsts.DefaultSelectionOptions;
        }

        /// <summary>
        /// Indicates whether the <c>OutlineOptions</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>OutlineOptions</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeOutlineOptions()
        {
            return OutlineOptions != EditConsts.DefaultOutlineOptions;
        }

        /// <summary>
        /// Indicates whether the <c>GutterWidth</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>GutterWidth</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeGutterWidth()
        {
            return GutterWidth != EditConsts.DefaultGutterWidth;
        }

        /// <summary>
        /// Indicates whether the <c>MarginPos</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>MarginPos</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeMarginPos()
        {
            return MarginPos != EditConsts.DefaultMarginPosition;
        }

        /// <summary>
        /// Indicates whether the <c>TabStops</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>TabStops</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeTabStops()
        {
            return !((TabStops.Length == 1) && (TabStops[0] == EditConsts.DefaultTabStop));
        }

        #endregion
    }
    #endregion XmlSyntaxSettingsInfo
}
