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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using Alternet.AI.Common;
using Alternet.AI.Copilot;
using Alternet.AI.CopilotSDK;
using Alternet.AI.NodeJs;
using Alternet.AI.Wpf;

using Alternet.Common;
using Alternet.Common.License;
using Alternet.Common.Python;
using Alternet.Common.Wpf;
using Alternet.Common.Wpf.Extensions;
using Alternet.Editor.Common.Wpf;
using Alternet.Editor.Wpf;
using Alternet.Editor.Wpf.Extensions;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Python;

using GitHub.Copilot;

using Markdig;

using Microsoft.Extensions.Logging;

using StreamJsonRpc;

namespace PythonCopilotChat
{
    public partial class MainWindow : Window
    {
        private static readonly string TempFolder;

        private readonly AIChatPanel chatPanel = new ();

        private CopilotSession? copilotSession;

        static MainWindow()
        {
            Utilities.EnableLogToDebugAndConsole();

            TempFolder = PathUtilities.GetTempPathUniquePerApp();
            PathUtilities.LogPathIf(false, TempFolder, "TempPath");
        }

        public MainWindow()
        {
            InitializeComponent();

            aiProviderTab.Content = chatPanel;

            chatPanel.BeforeConnectButtonClick += (s, e) =>
            {
                ValueTask? valueTask = copilotSession?.DisposeAsync();
                copilotSession = null;
            };

            var sampleFiles = SampleTextPathHelper.Instance.CopyScriptsToFolder(
                new string[] { "Script.py", "ScriptMyModule.py" },
                TempFolder);

            foreach (var file in sampleFiles)
            {
                AddPythonEditor(file);
            }

            chatPanel.AddCustomPromptMenuItems();

            chatPanel.GetAiResponseFunc = GetAiResponse;
            chatPanel.GetCodeContextFunc = () =>
            {
                return ScriptEditAIHelper.CreateCodeContext(editorTabControl);
            };

            chatPanel.BeforeRunPrompt += (s, e) =>
            {
                ScriptEditAIHelper.SaveToFileIfChanged(GetAllEditors());
            };

            chatPanel.AfterRunPrompt += (s, e) =>
            {
                ScriptEditAIHelper.LoadFromFileIfChanged(GetAllEditors());
            };

            SetCodeEnvironment(GetAllEditors(), CreateCodeEnvironment());

            chatPanel.AddDefaultOptionItems();

            chatPanel.GetModels = CopilotSDKUtils.GetModels;

            var tray = new ToolBarTray();
            grid.Children.Add(tray);

            var toolBar = new ToolBar
            {
                Band = 0,
                BandIndex = 0,
            };
            tray.ToolBars.Add(toolBar);

            toolBar.AddSpeedButton(AIProviderImages.Default.NewButtonImageHighDpi, "New", () =>
            {
                var options = PathUtilities.FileDialogOptions.Create(PathUtilities.FileDialogKind.New, "py", "txt", "*");

                var filePath = Alternet.Common.Wpf.PlatformUtils.AskForNewFilePath(options);

                if (filePath != null)
                {
                    File.WriteAllText(filePath, AIProviderDemoUtils.GetTemplateForPath(filePath), Encoding.UTF8);
                    AddPythonEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            toolBar.AddSpeedButton(AIProviderImages.Default.OpenButtonImageHighDpi, "Open", () =>
            {
                var filePath = Alternet.Common.Wpf.PlatformUtils.AskForOpenFilePath("py", "txt", "*");

                if (filePath != null)
                {
                    AddPythonEditor(filePath);
                    editorTabControl.SelectLastTabByIndex();
                }
            });

            ContextMenu actionsMenu = new ();

            var testActions = false;

            if (testActions)
            {
                var actionsButton = toolBar.AddSpeedButton(AIProviderImages.Default.ActionsButtonImageHighDpi, "More actions", () =>
            {
                actionsMenu.IsOpen = true;
            });

                actionsMenu.PlacementTarget = actionsButton;
            }

            actionsMenu.Placement = PlacementMode.Bottom;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public virtual List<string> GetImports()
        {
            var imports = new List<string>
            {
                "System",
                "System.Drawing",
                "System.Windows.Forms",
            };

            return imports;
        }

        public virtual CodeEnvironment CreateCodeEnvironment()
        {
            var codeEnvironment = new CodeEnvironment(
                null,
                SampleTextPathHelper.Instance.GetPaths(),
                GetImports(),
                null,
                Alternet.Common.TechnologyEnvironment.Wpf);

            return codeEnvironment;
        }

        public virtual void SetCodeEnvironment(IEnumerable<IScriptEdit> allEditors, ICodeEnvironment codeEnvironment)
        {
            var parsers = allEditors.Select(e =>
            {
                return e.Lexer;
            }).OfType<PythonParser>().ToArray();

            foreach (var parser in parsers)
            {
                parser.CodeEnvironment = codeEnvironment;
            }
        }

        public IEnumerable<IScriptEdit> GetAllEditors()
        {
            return ScriptEditAIHelper.GetAllEditors(editorTabControl);
        }

        private Task<object?> GetAiResponse(AIRequestParams p)
        {
            return CopilotSDKUtils.GetAiResponse(p, copilotSession, ScriptEditAIHelper.GetAllEditorFileNames(GetAllEditors()));
        }

        private IScriptEdit? AddPythonEditor(string filePath)
        {
            var result = ScriptEditAIHelper.AddEditor(
                filePath,
                editorTabControl,
                CreateEditor,
                null,
                null);
            if (result == null)
                return null;

            if (PathUtilities.HasExtension(filePath, ".py"))
            {
                PythonNETParser pythonParser = new ();
                result.Lexer = pythonParser;
            }

            return result;

            static IScriptEdit CreateEditor()
            {
                var editor = new ScriptCodeEdit();

                return editor;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chatPanel.ShowWelcomeMessage();
        }
    }
}
