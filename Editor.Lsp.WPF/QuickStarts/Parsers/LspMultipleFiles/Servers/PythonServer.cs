﻿#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Python.Embedded.Pyright;

namespace LspMultipleFiles.Servers
{
    internal sealed class PythonServer : Server
    {
        public override string Description => "Python (Pyright)";

        public override string[] SourceFilesSearchPatterns => new[] { "*.py" };

        protected override string SourceFilesDirectory => @"Editor\QuickStarts\Parsers\Python\MultipleFiles";

        public override LspParser CreateParser() => new PythonParserEmbedded();

        protected override void DeployCore(IProgress<double> progress) => PythonParserEmbedded.DeployServer(progress);
    }
}