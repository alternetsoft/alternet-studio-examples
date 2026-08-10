#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Alternet.AI.Common;
using Alternet.AI.Copilot;
using Alternet.AI.NodeJs;
using Alternet.AI.Wpf;

using Alternet.Common;
using Alternet.Common.License;
using Alternet.Common.Wpf;
using Alternet.Common.Wpf.Extensions;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Editor.Wpf.Extensions;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Markdig;

using Microsoft.Extensions.Logging;

using StreamJsonRpc;

namespace CopilotIntegration
{
    public partial class MainWindow : Window, INotifier, IAIProviderUserInterface
    {
        private readonly CsSolution csSolution = new ();

        private readonly Button buttonConnect;
        private readonly Button buttonDisconnect;
        private readonly AIProviderMessagesView messageView = new ();
        private readonly GHCopilotLspWorkspace workspace = new ();

        static MainWindow()
        {
            Utilities.EnableLogToDebugAndConsole();
        }

        public MainWindow()
        {
            workspace.UserInterface = this;
            workspace.WorkspaceFolder = SampleTextPathHelper.Instance.GetScriptDirectory() ?? string.Empty;

            if (Debugger.IsAttached)
                workspace.ClientHandlers.SetLogging(true);

            workspace.ClientHandlers.WindowLogMessage += async (s, e) =>
            {
                messageView.Add(e.MessageType, e.Message);
            };

            workspace.ClientHandlers.WindowShowMessage += (s, e) =>
            {
                messageView.Add(e.MessageType, $"[Alert] {e.Message}");
            };

            workspace.ClientHandlers.StatusNotification += async (s, e) =>
            {
                InvokeAsync(() =>
                {
                });
            };

            workspace.ClientHandlers.DidChangeStatus += async (s, e) =>
            {
                InvokeAsync(() =>
                {
                    serverStatusBusy.Text = workspace.ClientHandlers.StatusIsBusyAsString;
                    serverStatusKind.Text = workspace.ClientHandlers.StatusKind;
                    serverStatusMessage.Text = workspace.ClientHandlers.StatusMessage?.TrimEnd('.');

                    this.MessagesView.Add(AIProviderLogMessageType.Log, $"GitHub Copilot status: {workspace.ClientHandlers.StatusKind}");
                });
            };

            InitializeComponent();

            messagesTab.Content = messageView;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            csSolution.AddNotifier(this);

            csSolution.RegisterCodeFiles(GetSampleFiles());
            AddEditorFromSamples("Main.cs");
            AddEditorFromSamples("SampleClass.cs");

            var tray = new ToolBarTray();
            grid.Children.Add(tray);

            var toolBar = new ToolBar
            {
                Band = 0,
                BandIndex = 0,
            };
            tray.ToolBars.Add(toolBar);

            toolBar.AddButton(AIProviderImages.Default.NewButtonImages, "New", () =>
            {
                var options = PathUtilities.FileDialogOptions.Create(PathUtilities.FileDialogKind.New, "cs", "txt", "*");

                var filePath = Alternet.Common.Wpf.PlatformUtils.AskForNewFilePath(options);

                if (filePath != null)
                {
                    File.WriteAllText(filePath, AIProviderDemoUtils.GetTemplateForPath(filePath), Encoding.UTF8);
                    AddEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            toolBar.AddButton(AIProviderImages.Default.OpenButtonImages, "Open", () =>
            {
                var filePath = Alternet.Common.Wpf.PlatformUtils.AskForOpenFilePath("cs", "txt", "*");

                if (filePath != null)
                {
                    AddEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            buttonConnect = toolBar.AddButton(AIProviderImages.Default.SignInButtonImages, "Sign In", async () =>
            {
                await workspace.SignIn();
            });

            buttonDisconnect = toolBar.AddButton(AIProviderImages.Default.SignOutButtonImages, "Sign Out", async () =>
            {
                await workspace.SignOut();
            });

            buttonConnect.IsEnabled = false;
            buttonDisconnect.IsEnabled = false;

            ContentRendered += OnContentRendered;
        }

        public IAIProviderMessagesView MessagesView => messageView;

        public bool DebugWriteLine => true;

        public void InvokeAsync(Action callback)
        {
            Application.Current.Dispatcher.InvokeAsync(callback);
        }

        public string[] GetSampleFiles()
        {
            var result = SampleTextPathHelper.Instance.GetScriptsFilePaths("Main.cs", "SampleClass.cs");
            return result;
        }

        public void CopyToClipboard(string? text)
        {
            WpfUtilities.CopyToClipboard(text);
        }

        public void AskUserForText(
            string? prompt = null,
            string? title = null,
            string? initialText = null,
            Action<object>? configureDialog = null,
            StringInputDialogOptions? options = null,
            Action<string?>? onDialogClosed = null)
        {
            var result = Alternet.Common.Wpf.StringInputDialog.AskUserForText(prompt, title, initialText, configureDialog, options);
            onDialogClosed?.Invoke(result);
        }

        public void Reparse()
        {
            Debug.WriteLineIf(false, "Reparsing all editors...");
            ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());
            Debug.WriteLineIf(false, "Reparsing all editors completed.");
        }

        public async void OnContentRendered(object? sender, EventArgs e)
        {
            ContentRendered -= OnContentRendered;

            UpdateStatusBar();

            await Dispatcher.BeginInvoke(
            new Action(() =>
            {
                editorTabControl.SelectionChanged += (s, e) =>
                {
                    Reparse();
                };
            }), DispatcherPriority.ApplicationIdle);

            await workspace.ConnectToLSP();

            buttonConnect.IsEnabled = true;
            buttonDisconnect.IsEnabled = true;
        }

        void INotifier.Notification(object sender, EventArgs e)
        {
            Debug.WriteLineIf(false, "Solution notification: " + sender + ", " + e);
        }

        public IEnumerable<IScriptEdit> GetAllEditors()
        {
            return ScriptEditAIHelper.GetAllEditors(editorTabControl);
        }

        private IScriptEdit? GetCurrentEditorAdapter()
        {
            return ScriptEditAIHelper.GetCurrentEditor(editorTabControl, GetAllEditors());
        }

        private ScriptCodeEdit? GetCurrentTextEditor()
        {
            var editorAdapter = GetCurrentEditorAdapter();
            return editorAdapter as ScriptCodeEdit;
        }

        private void UpdateStatusBar()
        {
            UpdateStatusBar(GetCurrentTextEditor());
        }

        private void UpdateStatusBar(TextEditor? editor)
        {
            var currentEditor = GetCurrentTextEditor();
            if (editor is null || currentEditor != editor)
                return;

            positionStatus.Text = TextEditorUtility.GetStatusText(editor);
        }

        private void AddEditorFromSamples(string fileName)
        {
            var filePath = SampleTextPathHelper.Instance.GetScriptFilePath(fileName);
            AddEditor(filePath);
        }

        private TabItem? GetEditorTabItem(IScriptEdit editor)
        {
            var textEditor = editor as ScriptCodeEdit;

            if (textEditor is not null)
            {
                var tabItem = textEditor.Parent as TabItem;

                if (tabItem is not null)
                {
                    return tabItem;
                }
            }

            return null;
        }

        private IScriptEdit? AddEditor(string? filePath)
        {
            if (filePath is null) return null;

            var editor = ScriptEditAIHelper.AddEditor(filePath, editorTabControl, CreateEditor);
            if (editor is null)
                return null;

            var tabItem = GetEditorTabItem(editor);

            if (tabItem is not null)
            {
                var contextMenu = new ContextMenu();

                tabItem.ContextMenu = contextMenu;

                var closeMenuItem = WpfUtilities.CreateMenuItem(StringConsts.CloseTab, async (s, e) =>
                {
                    var document = workspace.ToDocument(editor.FileName);
                    await workspace.RemoveDocument(document);
                    WpfUtilities.CloseTab(editorTabControl, tabItem);
                });

                contextMenu.Items.Add(closeMenuItem);
            }

            CsParser CreateParser()
            {
                CsParser parser = new (csSolution);
                return parser;
            }

            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

            if (fileExtension == ".cs")
            {
                editor.Lexer = CreateParser();
            }

            IScriptEdit CreateEditor()
            {
                var editor = new ScriptCodeEdit
                {
                    BorderThickness = new Thickness(0),
                };

                var useDarkTheme = false;

                if (useDarkTheme)
                {
                    editor.VisualThemeType = Alternet.Editor.Wpf.VisualThemeType.Dark;
                }

                void OnStatusChanged(TextEditor e)
                {
                    UpdateStatusBar(e);
                }

                TextEditorUtility.BindOnStatusChange(editor, OnStatusChanged);

                return editor;
            }

            async Task RunInsertAiSuggestion(GHCopilotSuggestionKind kind)
            {
                var document = workspace.ToDocument(editor.FileName);

                if (document == null || editor is null)
                    return;

                var result = await workspace.RunInsertAiSuggestion(document, editor.Position.Y, editor.Position.Y, kind);
                if (result is null || result.Items.Count == 0)
                    return;

                editor.HideCodeCompletionHint();
                editor.ReadOnly = false;

                var changes = new List<ITextUndo>();
                foreach (var item in result.Items)
                {
                    var textUndo = new TextUndo(
                        new System.Drawing.Point(item.RangeStartChar, item.RangeStartLine),
                        new System.Drawing.Point(item.RangeEndChar, item.RangeEndLine),
                        item.InsertText);

                    changes.Add(textUndo);
                }

                editor.PreviewTextChanges(changes);
            }

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Alt+Shift+C"), async () =>
            {
                await RunInsertAiSuggestion(GHCopilotSuggestionKind.InlineEdit);
            });

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Alt+C"), async () =>
            {
                await RunInsertAiSuggestion(GHCopilotSuggestionKind.InlineCompletion);
            });

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Escape"), () =>
            {
                editor.RemovePreviewTextChanges();
            });

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Alt+Shift+Oem2"), async () =>
            {
                await RunInsertAiSuggestion(GHCopilotSuggestionKind.InlineEdit);
            });

            GHCopilotLspDocument document = new(editor.FileName, () => editor.Text);

            workspace.AddDocument(document);

            return editor;
        }
    }
}
