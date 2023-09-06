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
using System.Linq;
using System.Xml.Serialization;

using Alternet.Common;
using Alternet.Editor;
using Customize.Dialogs;

namespace Customize.Serialization
{
    /// <summary>
    /// Contains information about list of <c>IKeyData</c> object's settings.
    /// </summary>
    public class XmlKeyDataListInfo : ISerializationInfo
    {
        #region Private Members
        private IKeyDataList owner;
        private XmlKeyDataInfo[] keyDataList = new XmlKeyDataInfo[] { };
        #endregion

        #region ISerializationInfo Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>XmlKeyDataListInfo</c> class with default settings.
        /// </summary>
        public XmlKeyDataListInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>XmlKeyDataListInfo</c> class with specified owner.
        /// </summary>
        /// <param name="owner">Specifies <c>IKeyDataList</c> object to store settings.</param>
        public XmlKeyDataListInfo(IKeyDataList owner)
            : this()
        {
            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores <c>IKeyDataList.KeyData</c> property.
        /// </summary>
        [XmlArray]
        [XmlArrayItem("KeyData")]
        public XmlKeyDataInfo[] KeyDataList
        {
            get
            {
                if (owner != null)
                {
                    XmlKeyDataInfo[] result = new XmlKeyDataInfo[owner.Count];
                    for (int i = 0; i < owner.Count; i++)
                        result[i] = (XmlKeyDataInfo)owner[i].GetSerializationInfo();
                    return result;
                }
                else
                    return keyDataList;
            }

            set
            {
                keyDataList = value;
                if (owner != null && value != null)
                {
                    owner.Clear();
                    for (int i = 0; i < value.Length; i++)
                    {
                        XmlKeyDataInfo xmlKeyData = value[i];
                        IKeyData keyData = new KeyData();
                        keyData.SetSerializationInfo(xmlKeyData);
                        owner.Add(keyData);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Updates properties of external object with stored ones.
        /// </summary>
        /// <param name="owner">Specifies external object</param>
        public virtual void FixupReferences(object owner)
        {
            this.owner = (IKeyDataList)owner;
            KeyDataList = keyDataList;
        }

        /// <summary>
        /// Reads property values from external object. Reserwed for internal use.
        /// </summary>
        public virtual void Load()
        {
            if (owner == null)
                return;
            keyDataList = KeyDataList;
            foreach (XmlKeyDataInfo keyData in keyDataList)
            {
                keyData.Load();
            }
        }
        #endregion

        #region XmlColorThemesInfo Members

        /// <summary>
        /// Indicates whether the <c>KeyData</c> property should be persisted.
        /// </summary>
        /// <returns>True if <c>KeyData</c> differs from its default value; otherwise false.</returns>
        public bool ShouldSerializeKeyData()
        {
            return (keyDataList.Length > 0) || ((owner != null) && (owner.Count > 0));
        }

        #endregion
    }
}
