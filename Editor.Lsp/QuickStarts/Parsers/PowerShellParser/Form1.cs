#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Syntax;
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
            var asm = this.GetType().Assembly;
            var prefix = "PowerShellParsing.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
            InitEditor();
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

        private void InitEditor()
        {
            var gotoDefinitionMenuItem = new ToolStripMenuItem(StringConsts.GotoDefinition, null, GotoDefinitionMenuItem_Click);
            gotoDefinitionMenuItem.ShortcutKeys = Keys.F12;

            syntaxEdit1.DefaultMenu.Items.Add(gotoDefinitionMenuItem);
            syntaxEdit1.KeyDown += Edit_KeyDown;
            syntaxEdit1.Spelling.SpellColor = Color.Navy;
        }

        private void Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F12)
                GoToDefinition();
        }

        private void GotoDefinitionMenuItem_Click(object sender, EventArgs e)
        {
            GoToDefinition();
        }

        private async void GoToDefinition()
        {
            var parser = syntaxEdit1.Lexer as ISyntaxParser;
            var declaration = await parser.FindDeclarationAsync(syntaxEdit1.Position);
            if (declaration == null)
                return;

            if (syntaxEdit1.Source.FileName.Equals(declaration.FileName, StringComparison.OrdinalIgnoreCase))
            {
                syntaxEdit1.MoveTo(new Point(declaration.Column, declaration.Line));
                syntaxEdit1.MakeVisible(new Point(declaration.Column, declaration.Line), true);
            }
            else
            {
                MessageBox.Show(
                this,
                $"Definition: {declaration.FileName} ({declaration.Line}:{declaration.Column})",
                "Go To Definition",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
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
