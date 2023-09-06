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
using System.Xml.Serialization;

using Alternet.Common;

namespace Alternet.Editor.Wpf
{
    /// <summary>
    /// Contains information about <c>IVisualTheme</c> object's settings.
    /// </summary>
    public class XmlVisualThemesInfo : ISerializationInfo
    {
        #region Private Members
        private IVisualThemes owner;
        private XmlVisualThemeInfo[] colorThemes = new XmlVisualThemeInfo[] { };
        private int activeThemeIndex = -1;
        #endregion

        #region ISerializationInfo Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>XmlColorThemesInfo</c> class with default settings.
        /// </summary>
        public XmlVisualThemesInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>XmlColorThemesInfo</c> class with specified owner.
        /// </summary>
        /// <param name="owner">Specifies <c>IVisualThemes</c> object to store settings.</param>
        public XmlVisualThemesInfo(IVisualThemes owner)
            : this()
        {
            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores <c>IVisualThemes.ColorThemes</c> property.
        /// </summary>
        [XmlArray]
        [XmlArrayItem("ColorTheme")]
        public XmlVisualThemeInfo[] ColorThemes
        {
            get
            {
                if (owner != null)
                {
                    XmlVisualThemeInfo[] result = new XmlVisualThemeInfo[owner.Count];
                    for (int i = 0; i < owner.Count; i++)
                        result[i] = (XmlVisualThemeInfo)owner[i].GetSerializationInfo();
                    return result;
                }
                else
                    return colorThemes;
            }

            set
            {
                colorThemes = value;
                if (owner != null && value != null)
                {
                    owner.Clear();
                    foreach (XmlVisualThemeInfo theme in value)
                    {
                        IVisualTheme colorTheme = new VisualTheme();
                        colorTheme.SetSerializationInfo(theme);
                        owner.Add(colorTheme);
                    }
                }
            }
        }

        /// <summary>
        /// Stores <c>IVisualThemes.ActiveThemeIndex</c> property.
        /// </summary>
        [DefaultValue(-1)]
        public int ActiveThemeIndex
        {
            get
            {
                return owner != null ? owner.ActiveThemeIndex : activeThemeIndex;
            }

            set
            {
                activeThemeIndex = value;
                if (owner != null)
                    owner.ActiveThemeIndex = value;
            }
        }

        #endregion

        /// <summary>
        /// Updates properties of external object with stored ones.
        /// </summary>
        /// <param name="owner">Specifies external object</param>
        public virtual void FixupReferences(object owner)
        {
            this.owner = (IVisualThemes)owner;
            ColorThemes = colorThemes;
            ActiveThemeIndex = activeThemeIndex;
        }

        /// <summary>
        /// Reads property values from external object. Reserwed for internal use.
        /// </summary>
        public virtual void Load()
        {
            if (owner == null)
                return;
            colorThemes = ColorThemes;
            activeThemeIndex = ActiveThemeIndex;
            foreach (XmlVisualThemeInfo theme in colorThemes)
            {
                theme.Load();
            }
        }
        #endregion

        #region XmlColorThemesInfo Members

        /// <summary>
        /// Indicates whether the <c>ColorTheme</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>ColorTheme</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeColorTheme()
        {
            return (colorThemes.Length > 0) || ((owner != null) && (owner.Count > 0));
        }

        #endregion
    }
}
