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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.AI.Common;
using Alternet.AI.Copilot;
using Alternet.AI.CopilotSDK;
using Alternet.AI.Winforms;
using Alternet.Common;
using Alternet.Common.DotNet.DefaultAssemblies.DotNetCore;
using Alternet.Common.License;
using Alternet.Common.Python;
using Alternet.Editor;
using Alternet.Editor.Common;
using Alternet.Syntax.Parsers.Python;
using GitHub.Copilot;

namespace PythonCopilotChat
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Form1 : Form
    {
        private static readonly string TempFolder;

        private readonly AIChatPanel chatPanel = new ();

        private CopilotSession? copilotSession;

        static Form1()
        {
            Utilities.EnableLogToDebugAndConsole();

            ComponentLicenseProvider.ForceWarningWhenDebuggerIsAttached = false;

            TempFolder = PathUtilities.GetTempPathUniquePerApp();
            PathUtilities.LogPathIf(false, TempFolder, "TempPath");
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
                var prefix = "PythonCopilotChat.Resources";
                Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");

                ControlUtilities.BindFormInitializer(this, () =>
                {
                    openFileDialog1.Filter = "Python files (*.py)|*.py|All files (*.*)|*.*";

                    aiProviderTab.Controls.Add(chatPanel);
                    chatPanel.Dock = DockStyle.Fill;

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
                        return ScriptEditAIHelper.CreateContext(null, GetAllEditors());
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

                    foreach (var editor in GetAllEditors())
                    {
                    }

                    SetCodeEnvironment(GetAllEditors(), CreateCodeEnvironment());
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

        private IScriptEdit AddPythonEditor(string filePath)
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

            if (PathUtilities.HasExtension(filePath, ".py"))
            {
                PythonNETParser pythonParser = new ();
                result.Lexer = pythonParser;
            }

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
