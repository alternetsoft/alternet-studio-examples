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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Common.Wpf;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace Sorting
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ArrayList bubbleArray = new ArrayList();
        private ArrayList selArray = new ArrayList();
        private ArrayList quickSortArray = new ArrayList();
        private int step = 3;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool threadsRun = false;
        private CancellationTokenSource cancellationTokenSource;
        private DisplayPanel panelBubble;
        private DisplayPanel panelSelection;
        private DisplayPanel panelQuick;
        private bool panelCreated = false;

        public ViewModel()
        {
            SetupPython();
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.Wpf;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Windows");
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            panelBubble = CreatePanel(0);
            panelSelection = CreatePanel(1);
            panelQuick = CreatePanel(2);
            panelCreated = true;
            RandomizeArrays();

            IScriptGlobalItem itmBubbleArray = new ScriptGlobalItem("bubbleArray", bubbleArray);
            IScriptGlobalItem itmSelArray = new ScriptGlobalItem("selArray", selArray);
            IScriptGlobalItem itmQuickArray = new ScriptGlobalItem("quickArray", quickSortArray);

            IScriptGlobalItem itmBubblePanel = new ScriptGlobalItem("pnBubbleSort", panelBubble);
            IScriptGlobalItem itmSelPanel = new ScriptGlobalItem("pnSelectionSort", panelSelection);
            IScriptGlobalItem itmQuickPanel = new ScriptGlobalItem("pnQuickSort", panelQuick);

            scriptRun.GlobalItems.Add(itmBubbleArray);
            scriptRun.GlobalItems.Add(itmSelArray);
            scriptRun.GlobalItems.Add(itmQuickArray);

            scriptRun.GlobalItems.Add(itmBubblePanel);
            scriptRun.GlobalItems.Add(itmSelPanel);
            scriptRun.GlobalItems.Add(itmQuickPanel);

            UpdateSource();

            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(DoTick);

            RunBubbleSort = new RelayCommand(RunBubbleSortClick);
            RunSelectionSort = new RelayCommand(RunSelectionSortClick);
            RunQuickSort = new RelayCommand(RunQuickSortClick);
            CancelSort = new RelayCommand(CancelSortClick);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunBubbleSort { get; set; }

        public ICommand RunSelectionSort { get; set; }

        public ICommand RunQuickSort { get; set; }

        public ICommand CancelSort { get; set; }

        public async void StartScript(int index)
        {
            cancellationTokenSource = new CancellationTokenSource();
            threadsRun = true;
            RandomizeArrays(index);
            timer.Start();
            if (window != null)
            {
                window.ButtonBubbleSort.IsEnabled = false;
                window.ButtonSelectionSort.IsEnabled = false;
                window.ButtonQuickSort.IsEnabled = false;
            }
            else
                return;

            var token = cancellationTokenSource.Token;

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                    MessageBox.Show(string.Join("\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
            }

            string functionName = "BubbleSort";

            switch (index)
            {
                case 0:
                    functionName = "BubbleSort";
                    window.CancelBubbleButton.IsEnabled = true;
                    break;
                case 1:
                    functionName = "SelectionSort";
                    window.CancelSelectionButton.IsEnabled = true;
                    break;
                case 2:
                    functionName = "QuickSort";
                    window.CancelQuickButton.IsEnabled = true;
                    break;
            }

            try
            {
                await scriptRun.RunFunctionAsync(functionName, new object[] { token }, token);
            }
            catch (OperationCanceledException tie)
            {
                if (tie.InnerException != null && !(tie.InnerException is OperationCanceledException))
                    throw;
            }

            cancellationTokenSource = null;
            threadsRun = false;
            if (window != null)
            {
                window.CancelBubbleButton.IsEnabled = false;
                window.CancelSelectionButton.IsEnabled = false;
                window.CancelQuickButton.IsEnabled = false;
            }
        }

        public void StopScript()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                if (window != null)
                {
                    window.CancelBubbleButton.IsEnabled = false;
                    window.CancelSelectionButton.IsEnabled = false;
                    window.CancelQuickButton.IsEnabled = false;

                    window.ButtonBubbleSort.IsEnabled = true;
                    window.ButtonSelectionSort.IsEnabled = true;
                    window.ButtonQuickSort.IsEnabled = true;
                }
            }
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

        private void DoTick(object sender, EventArgs e)
        {
            if ((!threadsRun) && (window != null))
            {
                window.ButtonBubbleSort.IsEnabled = true;
                window.ButtonSelectionSort.IsEnabled = true;
                window.ButtonQuickSort.IsEnabled = true;
                timer.Stop();
            }
        }

        private void RandomizeArrays()
        {
            if (!panelCreated)
                return;
            bubbleArray.Clear();
            selArray.Clear();
            quickSortArray.Clear();
            Random rnd = new Random();
            int arrCount = (int)(panelBubble.Height / step) - 1;
            int len = (int)panelBubble.Width;
            for (int i = 0; i < arrCount; i++)
                bubbleArray.Add(rnd.Next(len));
            selArray.AddRange(bubbleArray);
            quickSortArray.AddRange(bubbleArray);
        }

        private void RandomizeArrays(int index)
        {
            if (!panelCreated)
                return;
            Random rnd = new Random();
            int arrCount = (int)(panelBubble.Height / step) - 1;
            int len = (int)panelBubble.Width;

            switch (index)
            {
                case 0:
                    bubbleArray.Clear();
                    for (int i = 0; i < arrCount; i++)
                        bubbleArray.Add(rnd.Next(len));
                    break;

                case 1:
                    selArray.Clear();
                    for (int i = 0; i < arrCount; i++)
                        selArray.Add(rnd.Next(len));
                    break;

                case 2:
                    quickSortArray.Clear();
                    for (int i = 0; i < arrCount; i++)
                        quickSortArray.Add(rnd.Next(len));
                    break;
            }
        }

        private DisplayPanel CreatePanel(int colIndex)
        {
            DisplayPanel panel = new DisplayPanel();
            panel.Width = 160;
            panel.Height = 250;
            panel.Background = new SolidColorBrush(Colors.WhiteSmoke);
            panel.HorizontalAlignment = HorizontalAlignment.Left;
            panel.Margin = new Thickness(10, 5, 0, 0);
            Grid.SetRow(panel, 1);
            Grid.SetColumn(panel, colIndex);
            if ((window != null) && (window.PanelGrid != null))
                window.PanelGrid.Children.Add(panel);
            panel.OnRendering += DisplayPanel_OnRendering;

            return panel;
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "SortingWpf.py";
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

        private void UpdateSource()
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);

            scriptRun.ScriptSource.FromScriptFile(sourceFileFullPath);
        }

        private void DisplayPanel_OnRendering(object sender, DrawingContext dc)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(sender as DisplayPanel);
            if (sender.Equals(panelBubble))
                scriptRun.RunFunction("OnRenderBubble", new object[] { dc, bounds });
            if (sender.Equals(panelSelection))
                scriptRun.RunFunction("OnRenderSelection", new object[] { dc, bounds });
            if (sender.Equals(panelQuick))
                scriptRun.RunFunction("OnRenderQuick", new object[] { dc, bounds });
        }

        private void RunBubbleSortClick()
        {
            StartScript(0);
        }

        private void RunSelectionSortClick()
        {
            StartScript(1);
        }

        private void RunQuickSortClick()
        {
            StartScript(2);
        }

        private void CancelSortClick()
        {
            StopScript();
        }
    }
}