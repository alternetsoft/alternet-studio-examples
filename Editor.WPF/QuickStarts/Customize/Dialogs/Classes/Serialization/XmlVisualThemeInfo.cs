#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Syntax.Lexer;
using Alternet.Syntax.Serialization;

namespace Alternet.Editor.Wpf
{
    /// <summary>
    /// Contains information about <c>IVisualTheme</c> object's settings.
    /// </summary>
    public class XmlVisualThemeInfo : ISerializationInfo
    {
        #region Private Members

        private IVisualTheme owner;
        private string name = string.Empty;
        private bool readOnly = false;
        private VisualThemeImagesColor imagesColor = VisualThemeImagesColor.Light;
        private MediaFont font = new MediaFont(new FontFamily(System.Drawing.FontFamily.GenericMonospace.Name), (double)new FontSizeConverter().ConvertFrom("10pt"), FontStretches.Normal, FontStyles.Normal, FontWeights.Normal);
        private string fontName = System.Drawing.FontFamily.GenericMonospace.Name;
        private double fontSize = (double)new FontSizeConverter().ConvertFrom("10pt");
        private XmlLexStyleInfo[] lexStyles = new XmlLexStyleInfo[] { };

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>XmlColorThemeInfo</c> class with default settings.
        /// </summary>
        public XmlVisualThemeInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>XmlColorThemeInfo</c> class with specified owner.
        /// </summary>
        /// <param name="owner">Specifies <c>IVisualTheme</c> object to store settings.</param>
        public XmlVisualThemeInfo(IVisualTheme owner)
            : this()
        {
            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores name of the <c>IVisualTheme.LexStyles</c> property.
        /// </summary>
        [XmlArray]
        [XmlArrayItem("Style")]
        public XmlLexStyleInfo[] LexStyles
        {
            get
            {
                if (owner != null)
                {
                    XmlLexStyleInfo[] result = new XmlLexStyleInfo[owner.LexStyles.Count];
                    for (int i = 0; i < owner.LexStyles.Count; i++)
                        result[i] = (XmlLexStyleInfo)owner.LexStyles[i].SerializationInfo;
                    return result;
                }
                else
                    return lexStyles;
            }

            set
            {
                lexStyles = value;
                if (owner != null && value != null)
                {
                    ILexStyles styles = new LexStyles(null);
                    foreach (XmlLexStyleInfo style in value)
                    {
                        ILexStyle lexStyle = new LexStyle();
                        styles.Add(lexStyle);
                        lexStyle.SerializationInfo = style;
                    }

                    owner.LexStyles = styles;
                }
            }
        }

        /// <summary>
        /// Stores name of the <c>IVisualTheme.Name</c> property.
        /// </summary>
        [DefaultValue("")]
        public string Name
        {
            get
            {
                return (owner != null) ? owner.Name : name;
            }

            set
            {
                name = value;
                if (owner != null)
                    owner.Name = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>IVisualTheme.ReadOnly</c> property.
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return (owner != null) ? owner.ReadOnly : readOnly;
            }

            set
            {
                readOnly = value;
                if (owner != null)
                    owner.ReadOnly = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>IVisualTheme.ImagesColor</c> property.
        /// </summary>
        [DefaultValue(VisualThemeImagesColor.Light)]
        public VisualThemeImagesColor ImagesColor
        {
            get
            {
                return (owner != null) ? owner.ImagesColor : imagesColor;
            }

            set
            {
                imagesColor = value;
                if (owner != null)
                    owner.ImagesColor = value;
            }
        }

        /// <summary>
        /// Stores <c>IVisualTheme.Font</c> property.
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
        /// Stores name of the <c>IVisualTheme.Font</c> property.
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
        /// Stores size of the <c>IVisualTheme.Font</c> property.
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

        #endregion

        #region ISerializationInfo Members

        /// <summary>
        /// Updates properties of external object with stored ones.
        /// </summary>
        /// <param name="owner">Specifies external object</param>
        public virtual void FixupReferences(object owner)
        {
            this.owner = (IVisualTheme)owner;
            Name = name;
            ReadOnly = readOnly;
            ImagesColor = imagesColor;
            LexStyles = lexStyles;
            Font = new MediaFont(new FontFamily(fontName), fontSize, FontStyles.Normal);
        }

        /// <summary>
        /// Reads property values from external object. Reserved for internal use.
        /// </summary>
        public virtual void Load()
        {
            if (owner == null)
                return;
            name = Name;
            readOnly = ReadOnly;
            imagesColor = ImagesColor;
            fontName = Font.Name;
            fontSize = Font.Size;
            lexStyles = LexStyles;
            foreach (XmlLexStyleInfo style in lexStyles)
            {
                style.Load();
            }
        }
        #endregion

        #region XmlColorThemeInfo Members

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
        /// Indicates whether the <c>LexStyles</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>LexStyles</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeLexStyles()
        {
            return (lexStyles.Length > 0) || ((owner != null) && (owner.LexStyles.Count > 0));
        }

        #endregion
    }
}
