#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Syntax;
using Alternet.Syntax.Parsers;

namespace Alternet.CodeEditor.Demo
{
    public class DemoItem
    {
        public DemoItem(ISyntaxParser parser, string sample)
        {
            Parser = parser;
            Sample = sample;
        }

        public ISyntaxParser Parser { get; private set; }

        public string Sample { get; private set; }
    }
}
