#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Scripter.Debugger;
using Alternet.Scripter.Debugger.Dap;
using Alternet.Scripter.Debugger.Dap.Cpp.Lldb;
using Alternet.Scripter.Debugger.Dap.Cpp.Lldb.Embedded;
using Alternet.Scripter.Integration;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Lsp.Clangd.Embedded;

namespace CppLlvmDapDebugger
{
    public partial class Form1 : Form
    {
        protected DotNetProject Project { get; private set; } = new DotNetProject();

        private static readonly string[] SourceSearchDirectories = new[] { "..", @"..\..\..\..\..\..\" };
        private static readonly string SourceSubPath = @"Resources\Debugger.Dap\Cpp.Lldb\ConsoleApplication1\ConsoleApplication1.cpp";
        private ScriptDebuggerEmbedded debugger = new ScriptDebuggerEmbedded();

        private DebugCodeEditContainer codeEditContainer;
        private CppScriptRun scriptRun = new CppScriptRun();

        public Form1()
        {
            DeployServer();
            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            scriptRun.ScriptSource.FromScriptFile(Path.GetFullPath(FindSourceFile()));
            codeEditContainer.TryActivateEditor(scriptRun.ScriptSource.ScriptFile);

            debugger.ScriptRun = scriptRun;

            debuggerControlToolbar1.Debugger = debugger;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;

            debugMenu1.Debugger = debugger;
            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;

            debuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(this, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = debuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            ScaleControls();
        }

        public void SaveAllModifiedFiles()
        {
            foreach (var edit in codeEditContainer.Editors)
            {
                if (edit.Modified)
                    edit.SaveFile(edit.FileName);
            }
        }

        private static string FindSourceFile() =>
           SourceSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, SourceSubPath))).FirstOrDefault(File.Exists);

        private void DeployServer()
        {
            if (CPlusPlusParserEmbedded.IsServerDeployed() && ScriptDebuggerEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "DAP C++ Debugger Demo",
                Message = "Please wait until the C++ DAP server files are extracted...",
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    CPlusPlusParserEmbedded.DeployServer(progressDialog.Progress);
                    debugger.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void OnDebuggerPreStartup(object sender, EventArgs e)
        {
            SaveAllModifiedFiles();
            SetScriptSource();
        }

        private bool SetScriptSource()
        {
            if (codeEditContainer.ActiveEditor != null)
            {
                string fileName = codeEditContainer.ActiveEditor.FileName;
                if (new FileInfo(fileName).Exists)
                {
                    scriptRun.ScriptSource.FromScriptFile(fileName);
                    return true;
                }
            }

            return false;
        }

        private void ScaleControls()
        {
            if (!DisplayScaling.NeedsScaling)
                return;

            splitContainer.SplitterDistance = DisplayScaling.AutoScale(splitContainer.SplitterDistance);
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.LoadFile(e.FileName);
            edit.Lexer = new CPlusPlusParserEmbedded();
            edit.GotoDefinition += Edit_GotoDefinition;
            edit.FindAllReferences += Edit_FindAllReferences;
            edit.AllowedActions &= ~AllowedActions.FindAllImplementations;

            e.DebugEdit = edit;
        }

        private void NavigateToRange(IFileRange range)
        {
            if (range != null)
            {
                if (!string.IsNullOrEmpty(range.FileName))
                {
                    var edit = codeEditContainer.TryActivateEditor(range.FileName);
                    if (edit != null)
                    {
                        edit.MakeVisible(new Point(range.StartPoint.X, range.StartPoint.Y), true);
                        edit.Focus();
                    }
                }
            }
        }

        private void Edit_FindAllReferences(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
                FindAllReferences(edit);
        }

        private async void FindAllReferences(IScriptEdit edit)
        {
            IRangeList references = new RangeList();
            var parser = edit is ISyntaxEdit ? ((ISyntaxEdit)edit).Lexer as CPlusPlusParserEmbedded : null;
            if (parser != null)
            {
                await parser.FindReferencesAsync(edit.Position, references, true);
                if (references.Count > 0)
                {
                    debuggerPanelsTabControl.FindResults.AddFindResults(references);
                    if (references.Count > 1)
                        ActivateFindResultsTab();
                    else
                        NavigateToRange(references[0] as IFileRange);
                }
            }
        }

        private void ActivateFindResultsTab()
        {
            debuggerPanelsTabControl.FocusPanel(DebuggerPanelKinds.FindResults);
        }

        private void Edit_GotoDefinition(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            GoToDefinition(edit);
        }

        private void GoToDefinition(IScriptEdit edit)
        {
            var parser = edit is ISyntaxEdit ? ((ISyntaxEdit)edit).Lexer as CPlusPlusParserEmbedded : null;
            if (parser != null)
            {
                var location = parser.FindDeclaration(edit.Position);
                if (location == null || string.IsNullOrEmpty(location.FileName))
                    return;

                edit = codeEditContainer.TryActivateEditor(location.FileName);
                edit.MakeVisible(new Point(location.Column, location.Line), true);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "C++ files (*.cpp)|*.cpp|Any files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = Path.GetDirectoryName(FindSourceFile()),
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = codeEditContainer.ActiveEditor;
            if (editor == null)
                return;

            if (!editor.Modified)
                return;

            editor.SaveFile(editor.FileName);
        }

        private void FileToolStripMenuItem_DropDownOpening(object sender, System.EventArgs e)
        {
            closeToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
            saveToolStripMenuItem.Enabled = codeEditContainer.ActiveEditor != null;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
            {
                codeEditContainer.CloseFile(edit.FileName);
                edit.FileName = string.Empty;
            }

            scriptRun.ScriptSource?.Reset();
        }
    }
}