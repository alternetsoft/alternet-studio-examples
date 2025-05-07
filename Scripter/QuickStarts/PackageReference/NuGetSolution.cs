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

using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Common.DotNet.Projects;
using Alternet.Scripter.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
using Microsoft.CodeAnalysis;

namespace Alternet.Scripter
{
    public class NuGetSolution : CsSolution
    {
        public NuGetSolution(SourceCodeKind sourceKind = SourceCodeKind.Regular, Type globalType = null)
            : base(sourceKind, globalType)
        {
        }

        public TargetFramework TargetFramework { get; set; }

        public override string ResolveAssemblyName(string name, IDefaultAssembliesProvider defaultAssembliesProvider = null)
        {
            return base.ResolveAssemblyName(name, defaultAssembliesProvider);
        }

        public override bool RegisterAssemblies(string[] references, ProjectId projectId = null, TargetFramework targetFramework = null)
        {
            EnsureWorkspaceCreated();

            IList<string> sourceReferences = new List<string>();
            IList<string> packageReferences = new List<string>();

            foreach (var reference in references)
            {
                if (IsPackageReference(reference))
                    packageReferences.Add(reference);
                else
                    sourceReferences.Add(reference);
            }

            bool result = true;
            if (sourceReferences != null)
                result = base.RegisterAssemblies(sourceReferences.ToArray(), projectId, targetFramework);

            if (packageReferences.Count != 0)
            {
                IList<string> packageRefs = NuGetReferenceResolver.ResolveReferences(GetFrameworkVersion(targetFramework), packageReferences, null);
                if (packageRefs.Count != 0)
                {
                    if (!base.RegisterAssemblies(packageRefs.ToArray(), projectId, targetFramework))
                        result = false;
                }
            }

            return result;
        }

        public override bool RegisterAssembly(string name, ProjectId projectId = null)
        {
            EnsureWorkspaceCreated();

            if (!IsPackageReference(name))
                return base.RegisterAssembly(name, projectId);

            IList<string> packageRefs = NuGetReferenceResolver.ResolveReferences(GetFrameworkVersion(null), new string[] { name }, null);
            if (packageRefs.Count != 0)
                return base.RegisterAssemblies(packageRefs.ToArray(), projectId);

            return true;
        }

        private bool IsPackageReference(string reference)
        {
            return reference.StartsWith("nuget:");
        }

        private string GetFrameworkVersion(TargetFramework targetFramework)
        {
            if (targetFramework == null)
                targetFramework = TargetFramework ?? TargetFramework.CurrentRuntimeFramework;

            return targetFramework.Moniker;
        }
    }
}