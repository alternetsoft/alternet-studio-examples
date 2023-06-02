#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;

namespace Alternet.CodeEditor.Demo
{
    public class SchemeItem
    {
        public SchemeItem(string scheme, string sample)
        {
            Scheme = scheme;
            Sample = sample;
        }

        public string Scheme { get; private set; }

        public string Sample { get; private set; }
    }
}
