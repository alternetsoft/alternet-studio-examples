#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using Alternet.Common;
using Alternet.Editor;

namespace Customize.Serialization
{
    public static class KeyDataSerialization
    {
        public static ISerializationInfo GetSerializationInfo(this IKeyData keyData)
        {
            return new XmlKeyDataInfo(keyData);
        }

        public static void SetSerializationInfo(this IKeyData keyData, ISerializationInfo info)
        {
            info.FixupReferences(keyData);
        }
    }
}
