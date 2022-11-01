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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Syntax.CodeCompletion;

namespace SQLDOMParser
{
    public class SqlWrapParameterInfo : ParameterInfo
    {
        public override IListMember CreateListMember()
        {
            return new SqlWrapListMember(this);
        }
    }
}
