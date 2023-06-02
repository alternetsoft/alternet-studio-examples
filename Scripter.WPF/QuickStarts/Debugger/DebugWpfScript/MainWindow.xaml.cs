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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Scripter;
using Alternet.Scripter.Debugger;

namespace DebugWpfScript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IScriptRemoteControl
    {
        private DisplayPanel displayPanel;
        private ScriptRun scriptRun = new ScriptRun();
        private DispatcherTimer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private volatile bool scriptRunning;
        private Process debuggerProcess;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ScriptFinishedDelegate onScriptFinished;
        private int iterations;
        private string ipcPortName = null;
        private string ipcObjectUri = null;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            displayPanel = new DisplayPanel();
            displayPanel.Width = 150;
            displayPanel.Height = 150;
            displayPanel.Background = new SolidColorBrush(Colors.Black);
            displayPanel.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetRow(displayPanel, 1);
            MainGrid.Children.Add(displayPanel);
            displayPanel.OnRendering += DisplayPanel_OnRendering;
            ParseCommandLineArgs(args);
        }

        public void CompileScript(ScriptCompiledDelegate onScriptCompiled)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
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
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!CompileScriptIfNeeded())
                {
                    MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                    return;
                }

                iterations = 0;
                displayPanel.Background = new SolidColorBrush(Colors.Transparent);
                scriptRunning = true;
                UpdateButtons();
                displayPanel.InvalidateVisual();
                updateTimer.Start();
            }));
        }

        public void StopScript()
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer.Stop();
                displayPanel.Background = new SolidColorBrush(Colors.Black);
                displayPanel.InvalidateVisual();
                if (onScriptFinished != null)
                    InvokeOnScriptFinished(onScriptFinished);
                onScriptFinished = null;
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DebuggerCommunication.StartServer(this, ipcPortName, ipcObjectUri);
            }
            catch (Exception ee)
            {
                if (ee.Message != string.Empty)
                {
                }
            }

            DirectoryInfo dirInfo = new DirectoryInfo(System.IO.Path.GetFullPath(dir) + @"Resources\Scripter");
            if (!dirInfo.Exists)
            {
                dir = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\..\..\..\";
            }

            scriptRun.ScriptHost.GenerateModulesOnDisk = true;
            scriptRun.ScriptHost.ModulesDirectoryPath = System.IO.Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "Alternet Script Debugger Generated Modules");

            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.ScriptSource.References.Add("WindowsBase");
            scriptRun.ScriptSource.References.Add("PresentationCore");
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            scriptRun.ScriptMode = ScriptMode.Debug;

            textBox1.Text = Process.GetCurrentProcess().Id.ToString();

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();
            ClearTempDirectory();

            cbLanguages.SelectedIndex = 0;
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
                catch (SystemException)
                {
                    // Workaround for strange remoting behavior: trying one more time in case of error after client restart.
                    onScriptCompiled(result);
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
                catch (SystemException)
                {
                    // Workaround for strange remoting behavior: trying one more time in case of error after client restart.
                    onScriptFinished();
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
            runScriptButton.IsEnabled = debuggerProcess == null;
            runScriptButton.Content = scriptRunning ? "Stop Script" : "Start Script";
            startDebuggerButton.IsEnabled = !scriptRunning && debuggerProcess == null;
        }

        private void RunScriptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!scriptRunning)
                StartScript(null);
            else
                StopScript();
        }

        private void StartDebuggerButton_Click(object sender, RoutedEventArgs e)
        {
            const string ExeName = "AlternetStudio.Wpf.exe";
            var pathToDebugDemo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExeName);
            if (!new FileInfo(pathToDebugDemo).Exists)
            {
                var tfm = Path.GetFileName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).TrimEnd(
                    Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

                pathToDebugDemo = Path.Combine(dir, @"Studio.Wpf\bin\Debug\", tfm, ExeName);
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
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                debuggerProcess.Exited -= Process_Exited;
                debuggerProcess = null;
                onScriptFinished = null;
                UpdateButtons();
            }));
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
            displayPanel.InvalidateVisual();
        }

        private void DisplayPanel_OnRendering(object sender, DrawingContext dc)
        {
            if (!scriptRunning)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(displayPanel);
            scriptRun.RunMethod("OnRender", null, new object[] { dc, bounds });
            updateDeltaStopwatch.Restart();
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodWpf.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "CallMethodWpf.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
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

        private void LanguagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSource(cbLanguages.SelectedIndex);
        }
    }
}
