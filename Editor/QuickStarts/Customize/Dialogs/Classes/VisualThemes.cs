#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Collections.Generic;
using System.ComponentModel;

using Alternet.Common;
using Alternet.Editor;
using Customize.Serialization;

namespace Customize.Dialogs
{
    #region VisualThemes

    /// <summary>
    /// Represents a collection of color themes.
    /// </summary>
    public class VisualThemes : List<IVisualTheme>, IVisualThemes
    {
        #region Private Members

        private int activeThemeIndex = -1;

        #endregion

        #region IVisualThemes Members

        /// <summary>
        /// Gets or sets the index of the active color theme.
        /// </summary>
        public virtual int ActiveThemeIndex
        {
            get
            {
                return activeThemeIndex;
            }

            set
            {
                if (activeThemeIndex != value)
                {
                    activeThemeIndex = value;
                    OnActiveThemeIndexChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the active color theme.
        /// </summary>
        public virtual IVisualTheme ActiveTheme
        {
            get
            {
                return (activeThemeIndex >= 0) ? this[activeThemeIndex] : null;
            }

            set
            {
                if (this[activeThemeIndex] != value)
                {
                    this[activeThemeIndex] = value;
                    OnActiveThemeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets an xml representation of this <c>ColorTheme</c> object.
        /// </summary>
        /// <remarks>Normally, you do not need to use this property directly. It's used internally when serializing Editor's content to XML.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlVisualThemesInfo(this);
            }

            set
            {
                value.FixupReferences(this);
            }
        }

        public void Assign(IVisualThemes source)
        {
            Clear();
            foreach (var theme in source)
            {
                var newTheme = new VisualTheme();
                newTheme.Assign(theme);
                Add(newTheme);
            }

            activeThemeIndex = source.ActiveThemeIndex;
        }

        #endregion

        #region Protected Members

        protected virtual void OnActiveThemeIndexChanged()
        {
        }

        protected virtual void OnActiveThemeChanged()
        {
        }

        #endregion
    }
    #endregion VisualThemes
}
