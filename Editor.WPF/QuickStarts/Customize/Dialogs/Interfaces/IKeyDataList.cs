#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System.Collections.Generic;
using Alternet.Common;

namespace Alternet.Editor.Wpf
{
    public interface IKeyDataList : IList<IKeyData>
    {
        /// <summary>
        /// When implemented by a class, gets or sets an xml representation of this <c>IKeyDataList</c> object.
        /// </summary>
        /// <remarks>Normally, you do not need to use this property directly. It's used internally when serializing IKeyData content to XML.</remarks>
        ISerializationInfo SerializationInfo
        {
            get;
            set;
        }
    }
}
