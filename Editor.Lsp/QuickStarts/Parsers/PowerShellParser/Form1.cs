#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Syntax.Parsers.Lsp.PowerShell.Embedded;

namespace PowerShellParsing
{
    public partial class Form1 : Form
    {
        private string dir = Application.StartupPath + @"\..\";

        public Form1()
        {
            DeployServer();
            InitializeComponent();
        }

        private void DeployServer()
        {
            if (PowerShellParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "LSP PowerShell Parser Demo",
                Message = "Please wait until the PowerShell LSP server files are extracted...",
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    PowerShellParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            syntaxEdit1.Lexer = powerShellParser1;
            syntaxEdit1.Outlining.AllowOutlining = true;

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\Text");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\Text\powershell.txt");
            if (fileInfo.Exists)
            {
                syntaxEdit1.Source.FileName = fileInfo.FullName;
                syntaxEdit1.LoadFile(fileInfo.FullName);
            }

            syntaxEdit1.Source.HighlightReferences = true;
        }
    }
}
