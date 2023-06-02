#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;
using Alternet.Scripter;
using Alternet.Scripter.Debugger;

namespace DebugMyScript
{
    public partial class MainForm : Form, IScriptRemoteControl
    {
        private const string LanguageDescription = "Choose programming language";
        private Timer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private volatile bool scriptRunning;
        private Process debuggerProcess;
        private string dir = Application.StartupPath + @"\";
        private ScriptFinishedDelegate onScriptFinished;
        private int iterations;
        private string ipcPortName = null;
        private string ipcObjectUri = null;

        public MainForm(string[] args)
        {
            InitializeComponent();

            ParseCommandLineArgs(args);
            DebuggerCommunication.StartServer(this, ipcPortName, ipcObjectUri);

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(dir) + @"Resources\Scripter");
            if (!dirInfo.Exists)
            {
                dir = Application.StartupPath + @"\..\..\..\..\..\..\..\";
            }

            scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            scriptRun.ScriptHost.ModulesDirectoryPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "Alternet Script Debugger Generated Modules");

            scriptRun.ScriptSource.WithDefaultReferences();

            scriptRun.ScriptSource.References.Add(typeof(MainForm).Assembly.Location);

            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

            textBox1.Text = Process.GetCurrentProcess().Id.ToString();
            updateTimer = new Timer();
            updateTimer.Interval = 50;
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();
            ClearTempDirectory();
            cbLanguages.SelectedIndex = 0;
        }

        public void CompileScript(ScriptCompiledDelegate onScriptCompiled)
        {
            BeginInvoke((Action)(() =>
            {
                var success = CompileScriptIfNeeded();

                InvokeOnScriptCompiled(
                    onScriptCompiled,
                    new ScriptCompilationResult
                    {
                        IsSuccessful = success,
                        TargetAssemblyName = scriptRun.ScriptHost.ExecutableModulePath,
                        Errors = scriptRun.ScriptHost.CompilerErrors,
                    });
            }));
        }

        public void StartScript(ScriptFinishedDelegate onScriptFinished)
        {
            this.onScriptFinished = onScriptFinished;
            BeginInvoke((Action)(() =>
            {
                if (!CompileScriptIfNeeded())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }

                scriptRunning = true;
                UpdateButtons();
                displayPanel.Refresh();
                iterations = 0;
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

                if (onScriptFinished != null)
                    InvokeOnScriptFinished(onScriptFinished);
                onScriptFinished = null;
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private bool CompileScriptIfNeeded()
        {
            if (!scriptRun.Compiled)
            {
                scriptRun.ScriptHost.AssemblyFileName = Guid.NewGuid().ToString("N") + ".dll";
                return scriptRun.Compile();
            }

            return true;
        }

        private void InvokeOnScriptCompiled(ScriptCompiledDelegate onScriptCompiled, ScriptCompilationResult result)
        {
            if (onScriptCompiled != null)
            {
                try
                {
                    onScriptCompiled(result);
                }
#if NETFRAMEWORK
                catch (SystemException)
                {
                    // Workaround for strange remoting behavior: trying one more time in case of error after client restart.
                    onScriptCompiled(result);
                }
#endif
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void InvokeOnScriptFinished(ScriptFinishedDelegate onScriptFinished)
        {
            if (onScriptFinished != null)
            {
                try
                {
                    onScriptFinished();
                }
#if NETFRAMEWORK
                catch (SystemException)
                {
                    // Workaround for strange remoting behavior: trying one more time in case of error after client restart.
                    onScriptFinished();
                }
#endif
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private string ExtractCommandLineArg(string arg)
        {
            int idx = arg.IndexOf("=");
            return idx >= 0 ? arg.Substring(idx + 1) : arg;
        }

        private void ParseCommandLineArgs(string[] args)
        {
            foreach (string arg in args)
            {
                string larg = arg.ToLower();
                if (larg.StartsWith("-ipcportname="))
                    ipcPortName = ExtractCommandLineArg(arg);
                else
                if (larg.StartsWith("-ipcobjecturi="))
                    ipcObjectUri = ExtractCommandLineArg(arg);
            }
        }

        private void ClearTempDirectory()
        {
            try
            {
                var path = scriptRun.ScriptHost.ModulesDirectoryPath;
                if (Directory.Exists(path))
                {
                    var directory = new DirectoryInfo(path);
                    foreach (System.IO.FileInfo file in directory.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void UpdateButtons()
        {
            runScriptButton.Enabled = debuggerProcess == null;
            runScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
            startDebuggerButton.Enabled = !scriptRunning && debuggerProcess == null;
        }

        private void RunScriptButton_Click(object sender, EventArgs e)
        {
            if (!scriptRunning)
                StartScript(null);
            else
                StopScript();
        }

        private void StartDebuggerButton_Click(object sender, EventArgs e)
        {
            const string ExeName = "AlternetStudio.exe";
            var pathToDebugDemo = Path.Combine(Application.StartupPath, ExeName);
            if (!new FileInfo(pathToDebugDemo).Exists)
            {
                var tfm = Path.GetFileName(Application.StartupPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                pathToDebugDemo = Path.Combine(dir, @"Studio\bin\Debug\", tfm, ExeName);
            }

            if (!new FileInfo(pathToDebugDemo).Exists)
            {
                MessageBox.Show("Can not find Script Debugger executable");
                return;
            }

            Debug.Assert(debuggerProcess == null, "Debugger process already started");

            debuggerProcess = Process.Start(
                new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = pathToDebugDemo,
                    Arguments = string.Format(
                        "\"-mainScriptFile={0}\" \"-controlledProcessId={1}\" \"-myCodeModules={2}\" \"-ipcPortName={3}\" \"-ipcObjectUri={4}\" \"-references={5}\"",
                        scriptRun.ScriptSource.ScriptFile,
                        Process.GetCurrentProcess().Id,
                        scriptRun.ScriptHost.ExecutableModulePath,
                        ipcPortName ?? string.Empty,
                        ipcObjectUri ?? string.Empty,
                        string.Join(",", scriptRun.ScriptSource.References)),
                });

            debuggerProcess.EnableRaisingEvents = true;
            debuggerProcess.Exited += Process_Exited;

            UpdateButtons();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                debuggerProcess.Exited -= Process_Exited;
                debuggerProcess = null;
                onScriptFinished = null;
                UpdateButtons();
            }));
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!scriptRunning)
                return;

            scriptRun.RunMethod("OnPaint", null, new object[] { e.Graphics, displayPanel.ClientRectangle });
            updateDeltaStopwatch.Restart();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!scriptRunning)
                return;
            iterations++;
            if (iterations == 50)
            {
                StopScript();
                return;
            }

            scriptRun.RunMethod("OnUpdate", null, new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });
            updateDeltaStopwatch.Restart();
            displayPanel.Refresh();
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethod.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
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
                    GetSourceParametersForCSharp(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForVisualBasic(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            scriptRun.ScriptSource.FromScriptFile(sourceFileFullPath);

            scriptRun.ScriptLanguage = language;
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
