#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.Linq;

using Alternet.Common.DotNet.Projects;

namespace Alternet.Scripter.Roslyn
{
    public class NuGetRoslynScriptHost : RoslynScriptHost
    {
        public NuGetRoslynScriptHost(IScriptRun scriptRun)
            : base(scriptRun)
        {
        }

        public TargetFramework TargetFramework { get; set; }

        protected override IList<string> ResolveReferences(IList<string> references, IList<string> unresolvedReferences)
        {
            IList<string> sourceReferences = new List<string>();
            IList<string> packageReferences = new List<string>();

            foreach (var reference in references)
            {
                if (IsPackageReference(reference))
                    packageReferences.Add(reference);
                else
                    sourceReferences.Add(reference);
            }

            IList<string> refs = base.ResolveReferences(sourceReferences, unresolvedReferences);

            if (packageReferences.Count == 0)
                return refs;

            List<string> result = new List<string>();
            result.AddRange(refs);

            foreach (var packageRef in packageReferences)
            {
                IList<string> packageRefs = NuGetReferenceResolver.ResolveReferences(GetFrameworkVersion(), new string[] { packageRef }, unresolvedReferences);
                if (packageRefs != null && packageRefs.Count > 0)
                {
                    result = result.Concat(packageRefs).Distinct().ToList();
                }
            }

            return result;
        }

        protected virtual IList<string> GetPackageReferences()
        {
            IList<string> result = new List<string>();

            foreach (var reference in ScriptRun.ScriptSource.References)
            {
                if (IsPackageReference(reference))
                    result.Add(reference);
            }

            return result;
        }

        protected virtual string GetFrameworkVersion()
        {
            var framework = TargetFramework ?? TargetFramework.CurrentRuntimeFramework;
            return framework.Moniker;
        }

        private bool IsPackageReference(string reference)
        {
            return reference.StartsWith("nuget:");
        }
    }
}