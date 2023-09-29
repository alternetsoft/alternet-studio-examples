#pragma warning disable VSTHRD101 // Avoid unsupported async delegates

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Common.Python.Deployment;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Scripter.Debugger.Dap.Python;
using Alternet.Scripter.Integration;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Python;

namespace EmbeddedPythonDapDebugger
{
    public partial class Form1 : Form
    {
        private static readonly string[] SourceSearchDirectories = new[] { "..", @"..\..\..\..\..\..\" };
        private static readonly string SourceFileSubPath = @"Resources\Debugger.Dap\Python\MultiThreadedTest.py";
        private Alternet.Scripter.Debugger.Dap.Python.Embedded.ScriptDebugger debugger;
        private ScriptRun scriptRun = new ScriptRun();

        private DebugCodeEditContainer codeEditContainer;

        public Form1()
        {
            SetupPython();

            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(editorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            scriptRun.ScriptSource.FromScriptFile(Path.GetFullPath(FindSourceFile()));
            codeEditContainer.TryActivateEditor(scriptRun.ScriptSource.ScriptFile);

            debugger = new Alternet.Scripter.Debugger.Dap.Python.Embedded.ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            debuggerControlToolbar1.Debugger = debugger;
            debuggerControlToolbar1.DebuggerPreStartup += OnDebuggerPreStartup;

            debugMenu1.Debugger = debugger;
            debugMenu1.DebuggerPreStartup += OnDebuggerPreStartup;
            debugMenu1.AllowedDebuggerCommands &= ~Alternet.Scripter.Debugger.AllowedDebuggerCommands.StartWithoutDebug;

            debuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
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

        protected async override void OnClosing(CancelEventArgs e)
        {
            if (debugger.IsStarted)
                await debugger.StopDebuggingAsync();

            base.OnClosing(e);
        }

        private static string FindSourceFile()
        {
            return SourceSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(Application.StartupPath, x, SourceFileSubPath))).
                FirstOrDefault(File.Exists);
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

        private void SetupPython()
        {
            var pythonPath = CodeEnvironment.GetSystemPythonPath();
            var pythonPrerequisitesService = new PythonPrerequisitesService(pythonPath);

            var result = pythonPrerequisitesService.CheckPythonInstallation();
            if (result.IsFailure)
            {
                MessageBox.Show(result.ErrorMessage);
                Environment.Exit(1);
            }

            var requiredModules = new[] { "numpy" };
            var pythonInstaller = new PythonInstaller { ExistingPythonHome = pythonPath };

            if (pythonPrerequisitesService.CheckRequiredModules().IsSuccess &&
                requiredModules.All(x => pythonInstaller.IsModuleInstalled(x)))
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "Debugger Integration Python DAP Demo",
                Message = "Downloading required Python packages...",
                ProgressBarStyle = ProgressBarStyle.Marquee,
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await pythonPrerequisitesService.InstallRequiredModules();
                    foreach (var module in requiredModules)
                        await pythonInstaller.PipInstallModule(module);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void EditorContainer_EditorRequested(object sender, DebugEditRequestedEventArgs e)
        {
            var edit = new DebugCodeEdit();
            edit.LoadFile(e.FileName);
            edit.Lexer = new PythonNETParser();
            edit.GotoDefinition += Edit_GotoDefinition;
            edit.FindAllReferences += Edit_FindAllReferences;
            edit.AllowedActions &= ~AllowedActions.FindAllImplementations;

            edit.KeyDown += (_, ea) =>
            {
                if (ea.KeyData == Keys.F12)
                    GoToDefinition();
            };

            e.DebugEdit = edit;
        }

        private void Edit_GotoDefinition(object sender, EventArgs e)
        {
            GoToDefinition();
        }

        private async void GoToDefinition()
        {
            var syntaxEdit = codeEditContainer.ActiveEditor as SyntaxEdit;
            if (syntaxEdit == null)
                return;

            var parser = syntaxEdit.Lexer as PythonNETParser;
            if (parser == null)
                return;

            var declaration = await parser.FindDeclarationAsync(syntaxEdit.Position);
            if (declaration == null)
                return;

            var edit = codeEditContainer.TryActivateEditor(declaration.FileName);
            edit.MakeVisible(new Point(declaration.Column, declaration.Line), true);
        }

        private void Edit_FindAllReferences(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
                FindAllReferences(edit);
        }

        private async void FindAllReferences(IScriptEdit edit)
        {
            var syntaxEdit = edit as SyntaxEdit;
            if (syntaxEdit == null)
                return;

            IRangeList references = new RangeList();
            var pythonParser = syntaxEdit.Lexer as PythonNETParser;
            if (pythonParser == null)
                return;

            await pythonParser.FindReferencesAsync(syntaxEdit.Position, references, true);
            if (references.Count > 0)
            {
                debuggerPanelsTabControl.FindResults.AddFindResults(references);
                if (references.Count > 1)
                    ActivateFindResultsTab();
                else
                    NavigateToRange(references[0] as IFileRange);
            }
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

        private void ActivateFindResultsTab()
        {
            debuggerPanelsTabControl.FocusPanel(DebuggerPanelKinds.FindResults);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Python files (*.py)|*.py|Any files (*.*)|*.*",
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