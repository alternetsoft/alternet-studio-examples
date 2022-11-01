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
using System.IO;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Clangd;
using Alternet.Syntax.Parsers.Lsp.Clangd.Embedded;

namespace LspMultipleFiles.Servers
{
    internal sealed class CppServer : Server
    {
        public override string Description => "C++ (clangd)";

        public override string[] SourceFilesSearchPatterns => new[] { "*.h", "*.cpp" };

        protected override string SourceFilesDirectory => @"Editor\QuickStarts\Parsers\Clangd";

        public override LspParser CreateParser() => new CPlusPlusParserEmbedded();

        public override void InitializeWorkspace(LspWorkspace workspace)
        {
            var project = new CProject();

            foreach (var file in Directory.GetFiles(GetSourceRootDirectory(), "*.cpp", SearchOption.AllDirectories))
                project.SourceFiles.Add(file);

            project.IncludeDirectories.Add(
                Path.GetFullPath(Path.Combine(GetSourceRootDirectory(), "include")));

            ((CPlusPlusWorkspace)workspace).Project = project;
        }

        protected override void DeployCore(IProgress<double> progress) => CPlusPlusParserEmbedded.DeployServer(progress);
    }
}