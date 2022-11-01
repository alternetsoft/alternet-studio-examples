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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Common.DotNet;
using Alternet.Common.DotNet.DefaultAssemblies;
using Alternet.Common.Python;
using Alternet.Common.Wpf;
using Alternet.Editor.Common.Wpf;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;
using Alternet.Syntax.Parsers.Python;

namespace CallMethod
{
    public class ViewModel
    {
        private DisplayPanel displayPanel;
        private bool scriptRunning;
        private ScriptRun scriptRun = new ScriptRun();
        private DispatcherTimer updateTimer;
        private Stopwatch updateDeltaStopwatch = new Stopwatch();
        private MainWindow window;
        private IScriptEdit edit;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private IDefaultAssembliesProvider defaultAssembliesProvider = DefaultAssembliesProviderFactory.CreateDefaultAssembliesProvider();
        private PythonNETParser pythonParser1 = new PythonNETParser();
        private double currentAngle = 0;

        public ViewModel()
        {
            SetupPython();
            RunScript = new RelayCommand(RunScriptClick);
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            displayPanel = new DisplayPanel();
            displayPanel.Width = 150;
            displayPanel.Height = 150;
            displayPanel.Background = new SolidColorBrush(Colors.Black);
            displayPanel.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetRow(displayPanel, 1);
            if ((window != null) && (window.MainGrid != null))
                window.MainGrid.Children.Add(displayPanel);
            displayPanel.OnRendering += DisplayPanel_OnRendering;

            CreateEditor(window.EditBorder);

            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.Wpf;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Controls");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Media");

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += UpdateTimer_Tick;
            UpdateButtons();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunScript { get; set; }

        public void StartScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
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

                displayPanel.Background = new SolidColorBrush(Colors.Transparent);
                scriptRunning = true;
                UpdateButtons();
                displayPanel.InvalidateVisual();
                updateTimer.Start();
            }));
        }

        public void StopScript()
        {
            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                scriptRunning = false;
                UpdateButtons();
                updateTimer.Stop();
                displayPanel.Background = new SolidColorBrush(Colors.Black);
                displayPanel.InvalidateVisual();
            }));
        }

        public bool IsScriptRunning()
        {
            return scriptRunning;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"AlterNETStudio\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled())
                return;

            var progressDialog = new ProgressDialog()
            {
                ShowInTaskbar = true,
                Title = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
            };

            progressDialog.Loaded += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython();
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "CallMethodWpf.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.Python";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }

        private void LoadFile(IScriptEdit edit, string fileName)
        {
            if (new FileInfo(fileName).Exists)
                edit.LoadFile(fileName);

            edit.FileName = fileName;
        }

        private void CreateEditor(Border border)
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);
            edit = CreateEditor(sourceFileFullPath, border);
        }

        private IScriptEdit CreateEditor(string fileName, Border border)
        {
            var edit = new ScriptCodeEdit();
            edit.FileName = fileName;
            pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
            edit.Lexer = pythonParser1;
            if (edit is UIElement)
            {
                border.Child = (UIElement)edit;
            }

            LoadFile(edit, fileName);

            return edit;
        }

        private void UpdateButtons()
        {
            if (window != null)
                window.ScriptButton.Content = scriptRunning ? "Stop Script" : "Start Script";
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!scriptRunning)
                return;

            currentAngle += (int)updateDeltaStopwatch.ElapsedMilliseconds * 0.1;
            currentAngle = ConstrainAngle(currentAngle);

            updateDeltaStopwatch.Restart();
            displayPanel.InvalidateVisual();
        }

        private double ConstrainAngle(double x)
        {
            x %= 360;
            if (x < 0)
                x += 360;

            return x;
        }

        private void RunScriptClick()
        {
            if (scriptRunning)
                StopScript();
            else
                StartScript();
        }

        private void DisplayPanel_OnRendering(object sender, DrawingContext dc)
        {
            if (!scriptRunning)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(displayPanel);
            Pen pen = new Pen();

            try
            {
                scriptRun.RunFunction("OnRender", new object[] { dc, bounds, pen, currentAngle });
            }
            catch (Exception ex)
            {
                window.Dispatcher.BeginInvoke((Action)(() => MessageBox.Show(ex.Message, "Script Execution Error")));
                StopScript();
            }

            updateDeltaStopwatch.Restart();
        }
    }
}