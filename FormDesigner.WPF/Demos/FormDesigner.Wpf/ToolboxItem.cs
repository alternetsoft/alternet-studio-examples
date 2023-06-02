#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;

namespace FormDesigner.Wpf
{
    public class ToolBoxItem
    {
        private object instance;

        public object Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance(Type);
                }

                return instance;
            }
        }

        public string Name
        {
            get
            {
                return Type.Name;
            }
        }

        public Type Type { get; set; }
    }
}