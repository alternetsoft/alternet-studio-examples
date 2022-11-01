#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System.Collections.Generic;

using Alternet.Common;
using Alternet.Editor;

namespace Alternet.Editor.Wpf
{
    #region IVisualThemes

    /// <summary>
    /// Properties and methods that represent a collection of color themes.
    /// </summary>
    public interface IVisualThemes : IList<IVisualTheme>
    {
        /// <summary>
        /// When implemented by a class, gets or sets the index of the
        /// active ColorTheme.
        /// </summary>
        int ActiveThemeIndex
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets the active ColorTheme.
        /// </summary>
        IVisualTheme ActiveTheme
        {
            get;
            set;
        }

        /// <summary>
        /// When implemented by a class, gets or sets an xml representation of this <c>ColorTheme</c> object.
        /// </summary>
        /// <remarks>Normally, you do not need to use this property directly. It's used internally when serializing Editor's content to XML.</remarks>
        ISerializationInfo SerializationInfo
        {
            get;
            set;
        }

        void Assign(IVisualThemes source);
    }
    #endregion IVisualThemes
}
