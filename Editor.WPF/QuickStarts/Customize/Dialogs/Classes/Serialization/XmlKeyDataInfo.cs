﻿#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.ComponentModel;
using Alternet.Common;

namespace Alternet.Editor.Wpf
{
    public class XmlKeyDataInfo
        : ISerializationInfo
    {
        #region Private Members

        private IKeyData owner;
        private string eventName = string.Empty;
        private string keys = string.Empty;
        private int state = -1;
        private int leaveState = -1;
        private string param = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <c>XmlKeyDataInfo</c> class with default settings.
        /// </summary>
        public XmlKeyDataInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>XmlKeyDataInfo</c> class with specified owner.
        /// </summary>
        /// <param name="owner">Specifies <c>IKeyData</c> object to store settings.</param>
        public XmlKeyDataInfo(IKeyData owner)
            : this()
        {
            this.owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stores name of the <c>IKeyData.EventName</c> property.
        /// </summary>
        [DefaultValue("")]
        public string EventName
        {
            get
            {
                return (owner != null) && !string.IsNullOrEmpty(owner.EventName) ? owner.EventName : eventName;
            }

            set
            {
                eventName = value;
                if (owner != null)
                    owner.EventName = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>IKeyData.Keys</c> property.
        /// </summary>
        [DefaultValue("")]
        public string KeyString
        {
            get
            {
                return (owner != null) ? KeyUtils.KeyDataToString(owner.Keys) : keys;
            }

            set
            {
                keys = value;
                if (owner != null)
                    owner.Keys = KeyUtils.KeyDataFromString(value);
            }
        }

        /// <summary>
        /// Stores name of the <c>IKeyData.State</c> property.
        /// </summary>
        [DefaultValue(-1)]
        public int State
        {
            get
            {
                return (owner != null) ? owner.State : state;
            }

            set
            {
                state = value;
                if (owner != null)
                    owner.State = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>IKeyData.LeaveState</c> property.
        /// </summary>
        [DefaultValue(-1)]
        public int LeaveState
        {
            get
            {
                return (owner != null) ? owner.LeaveState : leaveState;
            }

            set
            {
                leaveState = value;
                if (owner != null)
                    owner.LeaveState = value;
            }
        }

        /// <summary>
        /// Stores name of the <c>IKeyData.Param</c> property.
        /// </summary
        [DefaultValue("")]
        public string Param
        {
            get
            {
                return (owner != null) ? ParamToString(owner.Param) : param;
            }

            set
            {
                param = value;
                if (owner != null)
                    owner.Param = ParamFromString(value);
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
            this.owner = (IKeyData)owner;
            EventName = eventName;
            KeyString = keys;
            State = state;
            LeaveState = leaveState;
            Param = param;
        }

        /// <summary>
        /// Reads property values from external object. Reserved for internal use.
        /// </summary>
        public virtual void Load()
        {
            if (owner == null)
                return;
            eventName = EventName;
            keys = KeyString;
            state = State;
            leaveState = LeaveState;
            param = Param;
        }


        private string ParamToString(object param)
        {
            if (param is SelectionType)
                return Enum.GetName(typeof(SelectionType), param);

            if (param is int)
                return param.ToString();

            return string.Empty;
        }

        private object ParamFromString(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                int val = -1;
                if (int.TryParse(param, out val))
                {
                    return val;
                }
                else
                {
                    SelectionType selectionType = SelectionType.None;
                    if (Enum.TryParse(param, out selectionType))
                    {
                        return selectionType;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
