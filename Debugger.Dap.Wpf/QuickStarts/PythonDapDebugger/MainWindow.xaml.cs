using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Common.Python.Deployment;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Scripter.Debugger.Dap;
using Alternet.Scripter.Debugger.Dap.Python;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Syntax.Parsers.Lsp.Python.Embedded;

using Microsoft.Win32;

namespace PythonDapDebugger.Wpf
{
    public partial class MainWindow : Window
    {
        private static readonly string[] SourceSearchDirectories = new[] { "..", @"..\..\..\..\..\..\" };
        private static readonly string SourceFileSubPath = @"Resources\Debugger.Dap\Python\test1.py";
        private Alternet.Scripter.Debugger.Dap.Python.ScriptDebugger debugger;

        private DebugCodeEditContainer codeEditContainer;
        private ScriptRun scriptRun = new ScriptRun();

        public MainWindow()
        {
            SetupPython();
            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            scriptRun.ScriptSource.FromScriptFile(Path.GetFullPath(FindSourceFile()));
            codeEditContainer.TryActivateEditor(scriptRun.ScriptSource.ScriptFile);

            debugger = new Alternet.Scripter.Debugger.Dap.Python.ScriptDebugger
            {
                ScriptRun = scriptRun,
            };

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebuggerPanelsTabControl.Debugger = debugger;

            var controller = new DebuggerUIController(Dispatcher, codeEditContainer);
            controller.Debugger = debugger;
            controller.DebuggerPanels = DebuggerPanelsTabControl;
            codeEditContainer.Debugger = debugger;

            DebugMenu.InstallKeyboardShortcuts(CommandBindings);
            FileMenu.SubmenuOpened += FileMenu_SubmenuOpened;
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
           SourceSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, SourceFileSubPath))).FirstOrDefault(File.Exists);

        private void OnDebuggerPreStartup(object sender, System.EventArgs e)
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

        private void SetupPython()
        {
            var pythonPath = CodeEnvironment.GetSystemPythonPath();
            var pythonPrerequisitesService = new PythonPrerequisitesService(CodeEnvironment.GetSystemPythonPath());

            var result = pythonPrerequisitesService.CheckPythonInstallation();
            if (result.IsFailure)
            {
                MessageBox.Show(result.ErrorMessage);
                Environment.Exit(1);
            }

            var requiredModules = new[] { "numpy" };
            var pythonInstaller = new PythonInstaller { ExistingPythonHome = pythonPath };

            if (pythonPrerequisitesService.CheckRequiredModules().IsSuccess &&
                requiredModules.All(x => pythonInstaller.IsModuleInstalled(x)) &&
                PythonParserEmbedded.IsServerDeployed())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Text = "Debugger Integration Python DAP Demo",
                Message = "Downloading required Python packages...",
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    PythonParserEmbedded.DeployServer(progressDialog.Progress);
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
            edit.Lexer = new Alternet.Syntax.Parsers.Lsp.Python.Embedded.PythonParserEmbedded();
            edit.GotoDefinition += Edit_GotoDefinition;
            edit.FindAllReferences += Edit_FindAllReferences;
            edit.AllowedActions &= ~AllowedActions.FindAllImplementations;
            edit.KeyDown += (_, ea) =>
            {
                if (ea.Key == Key.F12)
                    GoToDefinition();
            };

            e.DebugEdit = edit;
        }

        private void Edit_FindAllReferences(object sender, EventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
                FindAllReferences(edit);
        }

        private async void FindAllReferences(IScriptEdit edit)
        {
            var syntaxEdit = edit as TextEditor;
            if (syntaxEdit == null)
                return;

            IRangeList references = new RangeList();
            var pythonParser = syntaxEdit.Lexer as Alternet.Syntax.Parsers.Lsp.Python.PythonParser;
            if (pythonParser == null)
                return;

            await pythonParser.FindReferencesAsync(syntaxEdit.Position, references, true);
            if (references.Count > 0)
            {
                DebuggerPanelsTabControl.FindResults.AddFindResults(references);
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
                        edit.MakeVisible(new System.Drawing.Point(range.StartPoint.X, range.StartPoint.Y), true);
                        edit.Focus();
                    }
                }
            }
        }

        private void ActivateFindResultsTab()
        {
            DebuggerPanelsTabControl.FocusPanel(DebuggerPanelKinds.FindResults);
        }

        private void Edit_GotoDefinition(object sender, EventArgs e)
        {
            GoToDefinition();
        }

        private void GoToDefinition()
        {
            var syntaxEdit = codeEditContainer.ActiveEditor as TextEditor;
            if (syntaxEdit == null)
                return;

            var pythonParser = syntaxEdit.Lexer as Alternet.Syntax.Parsers.Lsp.Python.PythonParser;
            if (pythonParser == null)
                return;

            var declaration = pythonParser.FindDeclaration(syntaxEdit.Position);
            if (declaration == null)
                return;

            var edit = codeEditContainer.TryActivateEditor(declaration.FileName);
            edit.Position = new System.Drawing.Point(declaration.Column, declaration.Line);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "Python files (*.py)|*.py|Any files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = Path.GetDirectoryName(FindSourceFile());
            if (dialog.ShowDialog().Value)
            {
                codeEditContainer.TryActivateEditor(dialog.FileName);
            }
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var edit = codeEditContainer.ActiveEditor;
            if (edit != null)
            {
                codeEditContainer.CloseFile(edit.FileName);
                edit.FileName = string.Empty;
            }

            scriptRun.ScriptSource?.Reset();
        }

        private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            CloseMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
            SaveMenuItem.IsEnabled = codeEditContainer.ActiveEditor != null;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (codeEditContainer.ActiveEditor != null)
                codeEditContainer.ActiveEditor.SaveFile(codeEditContainer.ActiveEditor.FileName);
        }
    }
}