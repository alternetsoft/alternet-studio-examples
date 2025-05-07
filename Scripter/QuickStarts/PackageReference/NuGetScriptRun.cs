#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using Alternet.Scripter.Roslyn;

namespace Alternet.Scripter
{
    public class NuGetScriptRun : ScriptRun
    {
        public NuGetScriptRun(System.ComponentModel.IContainer container)
            : base(container)
        {
        }

        public NuGetScriptRun()
            : base()
        {
        }

        public override IScriptHost CreateScriptHost()
        {
            return new NuGetRoslynScriptHost(this);
        }
    }
}
