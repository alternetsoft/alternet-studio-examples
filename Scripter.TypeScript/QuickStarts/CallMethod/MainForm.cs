#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Alternet.Common.TypeScript;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Editor.Common;
using Alternet.Editor.TypeScript;
using Alternet.Scripter.TypeScript;

using Microsoft.ClearScript;

namespace CallMethod
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private Timer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private volatile bool scriptRunning;

        private IScriptEdit edit;

        public MainForm()
        {
            InitializeComponent();

            InitDefaultHostAssemblies();

            CreateEditor();

            updateTimer = new Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;

            cbLanguages.SelectedIndex = 0;
            UpdateButtons();
        }

        public void StartScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRun.ScriptSource.FromScriptCode(edit.Text);
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                        return;
                    }
                }

                scriptRunning = true;
                UpdateButtons();

                displayPanel.Refresh();
                updateTimer.Start();
            }));
        }

        public void StopScript()
        {
            BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer.Stop();
                displayPanel.Refresh();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private void InitDefaultHostAssemblies()
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(options: HostItemOptions.GlobalMembers | HostItemOptions.GenerateDescriptions);
            TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor()
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            GetSourceParametersForTS(out sourceFileSubPath, out language);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, pnEdit);
        }

        private IScriptEdit CreateEditor(string fileName, Control parent)
        {
            IScriptEdit edit;
            edit = new ScriptCodeEdit();
            edit.Parent = parent;
            edit.Dock = DockStyle.Fill;
            edit.InitSyntax();

            LoadFile(edit, fileName);

            return edit;
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\CallMethod.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\CallMethod.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void UpdateSource(int index)
        {
            string sourceFileSubPath;
            ScriptLanguage language;
            switch (index)
            {
                case 0:
                    GetSourceParametersForTS(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            LoadFile(edit, sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
        }

        private void UpdateButtons()
        {
            runScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void CatchAndDisplayExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException tex)
            {
                StopScript();
                if (tex.InnerException is ScriptEngineException)
                    MessageBox.Show(((ScriptEngineException)tex.InnerException).ErrorDetails);
                else
                    MessageBox.Show(tex.ToString());
            }
            catch (Exception e)
            {
                StopScript();
                MessageBox.Show(e.ToString());
            }
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            CatchAndDisplayExceptions(() => scriptRun.RunFunction("OnPaint", new object[] { e.Graphics, displayPanel.ClientRectangle }));
            updateDeltaStopwatch.Restart();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!scriptRunning)
                return;

            CatchAndDisplayExceptions(() => scriptRun.RunFunction("OnUpdate", new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds }));
            updateDeltaStopwatch.Restart();
            displayPanel.Refresh();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }

        private void LanguagesComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            string str = toolTip1.GetToolTip(cbLanguages);
            if (str != LanguageDescription)
                toolTip1.SetToolTip(cbLanguages, LanguageDescription);
        }
    }
}
