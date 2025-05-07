#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Python.Embedded.Pyright;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

namespace LspMultipleFiles.Servers
{
    internal abstract class Server
    {
        public abstract string Description { get; }

        public abstract string[] SourceFilesSearchPatterns { get; }

        protected abstract string SourceFilesDirectory { get; }

        public abstract LspParser CreateParser();

        public virtual void InitializeWorkspace(LspWorkspace workspace)
        {
        }

        public void Deploy()
        {
            if (PythonParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "LSP Python Parser Demo",
                Message = "Please wait until the Python LSP server files are extracted...",
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    DeployCore(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        public string GetSourceRootDirectory()
        {
            var startupPath = Path.GetFullPath(Application.StartupPath);
            var demoPath = Path.Combine("Resources", SourceFilesDirectory);
            var directory = Path.Combine(startupPath, "..", demoPath);
            if (!Directory.Exists(directory))
                directory = Path.Combine(startupPath, @"..\..\..\..\..\..\..\", demoPath);
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);
            return directory;
        }

        protected abstract void DeployCore(IProgress<double> progress);
    }
}