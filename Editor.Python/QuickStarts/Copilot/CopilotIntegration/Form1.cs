#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.AI.Common;
using Alternet.AI.Copilot;
using Alternet.AI.Winforms;

using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies.DotNetCore;
using Alternet.Common.Extensions;
using Alternet.Common.License;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Syntax;
using Alternet.Syntax.Parsers.Python;

namespace PythonCopilotIntegration
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Form1 : Form, INotifier, IAIProviderUserInterface
    {
        private readonly AIProviderMessagesView messageView = new ();
        private readonly GHCopilotLspWorkspace workspace = new ();
        private readonly StatusStrip statusStrip = new ();
        private readonly ToolStripStatusLabel positionStatus = new ();
        private readonly ToolStripStatusLabel serverStatusBusy = new ();
        private readonly ToolStripStatusLabel serverStatusKind = new ();
        private readonly ToolStripStatusLabel serverStatusMessage = new ();
        private readonly ToolStrip toolStrip;
        private readonly ToolStripButton buttonNew;
        private readonly ToolStripButton buttonOpen;
        private readonly ToolStripButton buttonConnect;
        private readonly ToolStripButton buttonDisconnect;

        static Form1()
        {
            Utilities.EnableLogToDebugAndConsole();

            ComponentLicenseProvider.ForceWarningWhenDebuggerIsAttached = false;

            FrameworkAssemblyListProvider.IgnoreSystemWideSDK = false;
        }

        public Form1()
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

            messageView.BorderStyle = BorderStyle.None;

            if (IsDark)
            {
                ControlUtilities.SetTabPageColor(messagesTab, BackColor);
            }

            serverStatusMessage.Spring = true;
            serverStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            statusStrip.Items.Add(positionStatus);
            statusStrip.Items.Add(serverStatusBusy);
            statusStrip.Items.Add(serverStatusKind);
            statusStrip.Items.Add(serverStatusMessage);
            Controls.Add(statusStrip);

            toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
            };

            buttonNew = toolStrip.AddTextButton(AIProviderImages.Default.NewButtonImages, "New", () =>
            {
                var options = PathUtilities.FileDialogOptions.Create(PathUtilities.FileDialogKind.New, "cs", "txt", "*");

                var filePath = Alternet.Common.PlatformUtils.AskForNewFilePath(options);

                if (filePath != null)
                {
                    File.WriteAllText(filePath, AIProviderDemoUtils.GetTemplateForPath(filePath), Encoding.UTF8);
                    AddPythonEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            buttonOpen = toolStrip.AddTextButton(AIProviderImages.Default.OpenButtonImages, "Open", () =>
            {
                var options = PathUtilities.FileDialogOptions.Create(PathUtilities.FileDialogKind.Open, "cs", "txt", "*");

                var filePath = Alternet.Common.PlatformUtils.AskForOpenFilePath(options);

                if (filePath != null)
                {
                    AddPythonEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            buttonConnect = toolStrip.AddTextButton(AIProviderImages.Default.SignInButtonImages, "Sign In", async () =>
            {
                await workspace.SignIn();
            });

            buttonDisconnect = toolStrip.AddTextButton(AIProviderImages.Default.SignOutButtonImages, "Sign Out", async () =>
            {
                await workspace.SignOut();
            });

            toolStrip.Items.Add(buttonNew);
            toolStrip.Items.Add(buttonOpen);
            toolStrip.Items.Add(buttonConnect);
            toolStrip.Items.Add(buttonDisconnect);

            Controls.Add(toolStrip);

            buttonConnect.Enabled = false;
            buttonDisconnect.Enabled = false;

            Shown += Form1_Shown;

            messageView.Dock = DockStyle.Fill;
            messagesTab.Controls.Add(messageView);
            var asm = this.GetType().Assembly;
            var prefix = "PythonCopilotIntegration.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

            ControlUtilities.BindFormInitializer(this, () =>
            {
                openFileDialog1.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";

                AddPythonEditorFromSamples("Script.py");
                AddPythonEditorFromSamples("ScriptMyModule.py");

                ControlUtilities.RunOnceOnIdle(() =>
                {
                    ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());

                    editorTabControl.SelectedIndexChanged += (s, e) =>
                    {
                        ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());
                    };
                });
            });
        }

        public static bool IsDark
        {
            get
            {
                return Alternet.Editor.SyntaxEdit.IsDarkModeEnabled();
            }
        }

        public void Reparse()
        {
            Debug.WriteLineIf(false, "Reparsing all editors...");
            ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());
            Debug.WriteLineIf(false, "Reparsing all editors completed.");
        }

        private async void Form1_Shown(object? sender, EventArgs e)
        {
            Shown -= Form1_Shown;

            UpdateStatusBar();

            await Task.Factory.FromAsync(
            this.BeginInvoke(new Action(() =>
            {
                Reparse();

                editorTabControl.SelectedIndexChanged += (s, e) =>
                {
                    Reparse();
                };
            })),
            this.EndInvoke);
            await workspace.ConnectToLSP();

            buttonConnect.Enabled = true;
            buttonDisconnect.Enabled = true;
        }

        public bool DebugWriteLine => true;

        public IAIProviderMessagesView MessagesView => messageView;

        public void InvokeAsync(Action callback)
        {
            BeginInvoke(callback);
        }

        public void CopyToClipboard(string? text)
        {
            Clipboard.SetText(text ?? string.Empty);
        }

        public void AskUserForText(
            string? prompt = null,
            string? title = null,
            string? initialText = null,
            Action<object>? configureDialog = null,
            StringInputDialogOptions? options = null,
            Action<string?>? onDialogClosed = null)
        {
            var result = Alternet.Common.StringInputDialog.AskUserForText(prompt, title, initialText, configureDialog, options);
            onDialogClosed?.Invoke(result);
        }

        public IEnumerable<IScriptEdit> GetAllEditors()
        {
            return ScriptEditAIHelper.GetAllEditors(editorTabControl);
        }

        public void Notification(object sender, EventArgs e)
        {
            Debug.WriteLineIf(false, "Solution notification: " + sender + ", " + e);
        }

        private void AddPythonEditorFromSamples(string fileName)
        {
            var filePath = SampleTextPathHelper.Instance.GetScriptFilePath(fileName);
            if (filePath == null)
                return;
            AddPythonEditor(filePath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private IScriptEdit? AddPythonEditor(string? filePath)
        {
            if (filePath is null) return null;

            var editor = ScriptEditAIHelper.AddEditor(filePath, editorTabControl, CreateEditor);

            if (editor is null)
                return null;

            var tabItem = ScriptEditAIHelper.GetEditorTabItem(editor);

            if (tabItem is not null)
            {
                if (IsDark)
                {
                    ControlUtilities.SetTabPageColor(tabItem, BackColor);
                }

                var contextMenu = new ContextMenuStrip();

                tabItem.ContextMenuStrip = contextMenu;

                var closeMenuItem = ControlUtilities.CreateMenuItem(StringConsts.CloseTab, async () =>
                {
                    var document = workspace.ToDocument(editor.FileName);
                    await workspace.RemoveDocument(document);
                    editorTabControl.TabPages.Remove(tabItem);
                });

                contextMenu.Items.Add(closeMenuItem);
            }

            if (PathUtilities.HasExtension(filePath, ".py"))
            {
                PythonNETParser pythonParser = new ();
                editor.Lexer = pythonParser;
            }

            IScriptEdit CreateEditor()
            {
                var editor = new ScriptCodeEdit
                {
                    BorderStyle = EditBorderStyle.None,
                };

                void OnStatusChanged(SyntaxEdit e)
                {
                    UpdateStatusBar(e);
                }

                SyntaxEditUtils.BindOnStatusChange(editor, OnStatusChanged);

                return editor;
            }

            async Task RunInsertAiSuggestion()
            {
                var document = workspace.ToDocument(editor.FileName);

                if (document == null || editor is null)
                    return;

                var result = await workspace.RunInsertAiSuggestion(document, editor.Position.Y, editor.Position.Y);
                if (result is null || result.Items.Count == 0)
                    return;

                editor.HideCodeCompletionHint();
                editor.ReadOnly = false;

                var changes = new List<ITextUndo>();
                foreach (var item in result.Items)
                {
                    var textUndo = new TextUndo(
                        new Point(item.RangeStartChar, item.RangeStartLine),
                        new Point(item.RangeEndChar, item.RangeEndLine),
                        item.InsertText);

                    changes.Add(textUndo);
                }

                editor.PreviewTextChanges(changes);
            }

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Alt+C"), async () =>
            {
                await RunInsertAiSuggestion();
            });

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Escape"), () =>
            {
                editor.RemovePreviewTextChanges();
            });

            editor.AddNormalKeyAction(editor.GetKeyIdentifier("Alt+Shift+Oem2"), async () =>
            {
                await RunInsertAiSuggestion();
            });

            GHCopilotLspDocument document = new(editor.FileName, () => editor.Text);

            workspace.AddDocument(document);

            return editor;
        }

        private void UpdateStatusBar()
        {
            UpdateStatusBar(GetCurrentTextEditor());
        }

        private void UpdateStatusBar(SyntaxEdit? editor)
        {
            var currentEditor = GetCurrentTextEditor();
            if (editor is null || currentEditor != editor)
                return;
            positionStatus.Text = SyntaxEditUtils.GetStatusText(editor);
        }

        private IScriptEdit? GetCurrentEditorAdapter()
        {
            return ScriptEditAIHelper.GetCurrentEditor(editorTabControl, GetAllEditors());
        }

        private SyntaxEdit? GetCurrentTextEditor()
        {
            var editorAdapter = GetCurrentEditorAdapter();
            return editorAdapter as SyntaxEdit;
        }
    }
}
