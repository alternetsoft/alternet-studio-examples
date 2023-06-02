#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using Alternet.Syntax.CodeCompletion;

namespace SQLDOMParser
{
    public class SqlWrapListMembers : ListMembers
    {
        public override IListMember CreateListMember()
        {
            return new SqlWrapListMember(this);
        }
    }
}
