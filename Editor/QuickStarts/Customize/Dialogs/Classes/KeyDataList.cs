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
using System.ComponentModel;
using Alternet.Common;
using Alternet.Editor;
using Customize.Serialization;

namespace Customize.Dialogs
{
    public class KeyDataList : List<IKeyData>, IKeyDataList
    {
        /// <summary>
        /// Gets or sets an xml representation of this <c>KeyDataList</c> object.
        /// </summary>
        /// <remarks>Normally, you do not need to use this property directly. It's used internally when serializing <c>IKeyData</c> content to XML.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ISerializationInfo SerializationInfo
        {
            get
            {
                return new XmlKeyDataListInfo(this);
            }

            set
            {
                value.FixupReferences(this);
            }
        }
    }
}
