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
using Alternet.AI.CopilotSDK;
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

using GitHub.Copilot;

using Markdig;

using Microsoft.Extensions.Logging;

namespace CopilotChat
{
    public partial class MainWindow : Window
    {
        private static readonly string TempFolder;

        private readonly AIChatPanel chatPanel = new ();
        private readonly CsSolution csSolution = new ();

        private CopilotSession? copilotSession;

        static MainWindow()
        {
            TempFolder = PathUtilities.GetTempPathUniquePerApp();

            Utilities.EnableLogToDebugAndConsole();

            ComponentLicenseProvider.ForceWarningWhenDebuggerIsAttached = false;

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
                new string[] { "Main.cs", "SampleClass.cs" },
                TempFolder);

            csSolution.RegisterCodeFiles(sampleFiles);

            foreach (var file in sampleFiles)
            {
                AddEditor(file);
            }

            chatPanel.AddCustomPromptMenuItems();

            chatPanel.GetAiResponseFunc = GetAiResponse;
            chatPanel.GetCodeContextFunc = () =>
            {
                return ScriptEditAIHelper.CreateCodeContext(editorTabControl);
            };

            chatPanel.GetModels = CopilotSDKUtils.GetModels;

            chatPanel.BeforeRunPrompt += (s, e) =>
            {
                ScriptEditAIHelper.SaveToFileIfChanged(GetAllEditors());
            };

            chatPanel.AfterRunPrompt += (s, e) =>
            {
                ScriptEditAIHelper.LoadFromFileIfChanged(GetAllEditors());
            };

            ContentRendered += OnContentRendered;
        }

        public async void OnContentRendered(object? sender, EventArgs e)
        {
            ContentRendered -= OnContentRendered;

            await Dispatcher.BeginInvoke(
            new Action(() =>
            {
                ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());

                editorTabControl.SelectionChanged += (s, e) =>
                {
                    ScriptEditAIHelper.ReparseAllEditors(GetAllEditors());
                };
            }), DispatcherPriority.ApplicationIdle);
        }

        public IEnumerable<IScriptEdit> GetAllEditors()
        {
            return ScriptEditAIHelper.GetAllEditors(editorTabControl);
        }

        private Task<object?> GetAiResponse(AIRequestParams p)
        {
            return CopilotSDKUtils.GetAiResponse(p, copilotSession, ScriptEditAIHelper.GetAllEditorFileNames(GetAllEditors()));
        }

        private IScriptEdit? AddEditor(string filePath)
        {
            var result = ScriptEditAIHelper.AddEditor(
                filePath,
                editorTabControl,
                CreateEditor,
                null,
                null);
            if (result == null)
                return null;
            CsParser parser = new (csSolution);
            result.Lexer = parser;
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
