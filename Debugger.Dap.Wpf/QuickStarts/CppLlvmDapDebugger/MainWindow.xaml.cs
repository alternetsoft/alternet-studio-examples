using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Alternet.Common;
using Alternet.Common.Wpf;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Scripter.Debugger.Dap.Cpp.Lldb;
using Alternet.Scripter.Debugger.Dap.Cpp.Lldb.Embedded;
using Alternet.Scripter.Integration.Wpf;
using Alternet.Syntax.Parsers.Lsp.Clangd.Embedded;

using Microsoft.Win32;

namespace CppLlvmDapDebugger.Wpf
{
    public partial class MainWindow : Window
    {
        private static readonly string[] SourceSearchDirectories = new[] { "..", @"..\..\..\..\..\..\" };
        private static readonly string SourceSubPath = @"Resources\Debugger.Dap\Cpp.Lldb\ConsoleApplication1\ConsoleApplication1.cpp";
        private ScriptDebuggerEmbedded debugger = new ScriptDebuggerEmbedded();

        private DebugCodeEditContainer codeEditContainer;
        private CppScriptRun scriptRun = new CppScriptRun();

        public MainWindow()
        {
            DeployServer();
            InitializeComponent();

            codeEditContainer = new DebugCodeEditContainer(EditorsTabControl);
            codeEditContainer.EditorRequested += EditorContainer_EditorRequested;

            scriptRun.ScriptSource.FromScriptFile(Path.GetFullPath(FindSourceFile()));
            codeEditContainer.TryActivateEditor(scriptRun.ScriptSource.ScriptFile);

            debugger.ScriptRun = scriptRun;

            DebuggerControlToolbar.Debugger = debugger;
            DebuggerControlToolbar.DebuggerPreStartup += OnDebuggerPreStartup;

            DebugMenu.Debugger = debugger;
            DebugMenu.DebuggerPreStartup += OnDebuggerPreStartup;

            DebuggerPanelsTabControl.VisiblePanels &= ~DebuggerPanelKinds.Threads;
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
           SourceSearchDirectories.Select(x => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x, SourceSubPath))).FirstOrDefault(File.Exists);

        private void DeployServer()
        {
            if (CPlusPlusParserEmbedded.IsServerDeployed() && ScriptDebuggerEmbedded.IsServerDeployed())
                return;

            var progressDialog = new Alternet.Common.Wpf.ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "DAP C++ Debugger Demo",
                Message = "Please wait until the C++ DAP server files are extracted...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(() =>
                {
                    CPlusPlusParserEmbedded.DeployServer(progressDialog.Progress);
                    debugger.DeployServer(progressDialog.Progress);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

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
            var cppParser = syntaxEdit.Lexer as CPlusPlusParserEmbedded;
            if (cppParser == null)
                return;

            await cppParser.FindReferencesAsync(syntaxEdit.Position, references, true);
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

        private async void GoToDefinition()
        {
            var syntaxEdit = codeEditContainer.ActiveEditor as TextEditor;
            if (syntaxEdit == null)
                return;

            var parser = syntaxEdit.Lexer as CPlusPlusParserEmbedded;
            if (parser == null)
                return;

            var declaration = await parser.FindDeclarationAsync(syntaxEdit.Position);
            if (declaration == null)
                return;

            var edit = codeEditContainer.TryActivateEditor(declaration.FileName);
            edit.MakeVisible(new System.Drawing.Point(declaration.Column, declaration.Line), true);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "C++ files (*.cpp)|*.cpp|Any files (*.*)|*.*";
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