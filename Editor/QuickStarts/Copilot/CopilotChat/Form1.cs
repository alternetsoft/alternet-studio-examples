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
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.AI.Common;
using Alternet.AI.Copilot;
using Alternet.AI.CopilotSDK;
using Alternet.AI.Winforms;
using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies.DotNetCore;
using Alternet.Common.License;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Syntax;
using Alternet.Syntax.CodeCompletion;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using GitHub.Copilot;

namespace CopilotChat
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Form1 : Form
    {
        private static readonly string TempFolder;

        private readonly AIChatPanel chatPanel = new();
        private readonly CsSolution csSolution = new();

        private CopilotSession? copilotSession;

        static Form1()
        {
            TempFolder = PathUtilities.GetTempPathUniquePerApp();

            Utilities.EnableLogToDebugAndConsole();

            PathUtilities.LogPathIf(false, TempFolder);
            FrameworkAssemblyListProvider.IgnoreSystemWideSDK = false;
        }

        public Form1()
        {
            try
            {
                InitializeComponent();

                if (IsDark)
                {
                    ControlUtilities.SetTabPageColor(aiProviderTab, BackColor);

                    chatPanel.BrowserAdapter.AddDarkTemplateStyles(BackColor, ForeColor);
                }

                chatPanel.AppUriClicked += (s, e) =>
                {
                    if (e.Value == new Uri("app://tokeninfo.html/"))
                    {
                        chatPanel.ChatHelper.OpenTokenInfoPageInBrowser();
                    }
                };

                var asm = this.GetType().Assembly;
                var prefix = "CopilotChat.Resources";
                Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

                ControlUtilities.BindFormInitializer(this, () =>
                {
                    openFileDialog1.Filter = "C # files (*.cs)|*.cs|VB files (*.vb)|*.vb";

                    aiProviderTab.Controls.Add(chatPanel);
                    chatPanel.Dock = DockStyle.Fill;

                    chatPanel.BeforeConnectButtonClick += (s, e) =>
                    {
                        ValueTask? valueTask = copilotSession?.DisposeAsync();
                        copilotSession = null;
                    };

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

                    var sampleFiles = SampleTextPathHelper.Instance.CopyScriptsToFolder(
                        new string[] { "Main.cs", "SampleClass.cs" },
                        TempFolder);

                    csSolution.RegisterCodeFiles(sampleFiles);

                    foreach (var file in sampleFiles)
                    {
                        AddEditor(file);
                    }
                });
            }
            finally
            {
            }
        }

        public static bool IsDark
        {
            get
            {
                return Alternet.Editor.SyntaxEdit.IsDarkModeEnabled();
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

        private IScriptEdit AddEditor(string filePath)
        {
            var result = ScriptEditAIHelper.AddEditor(
                filePath,
                editorTabControl,
                CreateEditor,
                () => chatPanel.AiProvider,
                null);

            var tabItem = ScriptEditAIHelper.GetEditorTabItem(result);

            if (tabItem is not null)
            {
                if (IsDark)
                {
                    ControlUtilities.SetTabPageColor(tabItem, BackColor);
                }
            }

            CsParser parser = new(csSolution);
            result.Lexer = parser;

            return result;

            static IScriptEdit CreateEditor()
            {
                var editor = new ScriptCodeEdit
                {
                    BorderStyle = EditBorderStyle.None,
                };
                return editor;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chatPanel.ShowWelcomeMessage();
        }
    }
}
