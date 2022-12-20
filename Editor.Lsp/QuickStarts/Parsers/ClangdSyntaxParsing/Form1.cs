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
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Syntax.Parsers.Lsp;
using Alternet.Syntax.Parsers.Lsp.Clangd.Embedded;

namespace ClangdSyntaxParsing
{
    public partial class Form1 : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string LoadDesc = "Load code file";
        private string dir = Application.StartupPath + @"\..\";

        public Form1()
        {
            DeployServer();
            InitializeComponent();
            syntaxEdit1.Spelling.SpellColor = Color.Navy;
            syntaxEdit1.KeyDown += SyntaxEdit_KeyDown;
        }

        private void DeployServer()
        {
            if (CPlusPlusParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "LSP C++ Parser Demo",
                Message = "Please wait until the C++ LSP server files are extracted...",
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    CPlusPlusParserEmbedded.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void SyntaxEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F12)
                GoToDefinition();
        }

        private void GoToDefinition()
        {
            var declaration = cppParser.FindDeclaration(syntaxEdit1.Position);
            if (declaration == null)
                return;

            if (syntaxEdit1.Source.FileName.Equals(declaration.FileName, StringComparison.OrdinalIgnoreCase))
                syntaxEdit1.MakeVisible(new Point(declaration.Column, declaration.Line), true);
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
            openFileDialog1.Filter = "C/C++ files (*.c;*.cpp;*.h)|*.c;*.cpp;*.h|All files (*.*)|*.*";
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Editor\QuickStarts\Parsers\Clangd");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            FileInfo fileInfo = new FileInfo(dir + @"Resources\Editor\QuickStarts\Parsers\Clangd\src\dynamicvoronoi.cpp");
            if (fileInfo.Exists)
            {
                cppSource.FileName = fileInfo.FullName;
                cppSource.LoadFile(fileInfo.FullName);

                cppParser.Workspace.Project = new CProject();
                cppParser.Workspace.Project.SourceFiles.Add(fileInfo.FullName);
                cppParser.Workspace.Project.IncludeDirectories.Add(
                    Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fileInfo.FullName), "../include")));
            }

            openFileDialog1.InitialDirectory = Path.GetFullPath(dir) + @"Resources\Editor\QuickStarts\Parsers\Clangd\";
            cppSource.Lexer = cppParser;
            cppSource.HighlightReferences = true;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                syntaxEdit1.Source.LoadFile(openFileDialog1.FileName);
                syntaxEdit1.Source.FileName = openFileDialog1.FileName;
            }
        }

        private void LoadButton_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(btLoad);
            if (str != LoadDesc)
                toolTip1.SetToolTip(btLoad, LoadDesc);
        }
    }
}
