#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using Alternet.Syntax.CodeCompletion;

namespace AllQuickStarts
{
    public class SqlWrapListMembers : ListMembers
    {
        public override IListMember CreateListMember()
        {
            return new SqlWrapListMember(this);
        }
    }
}
