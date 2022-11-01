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
using Alternet.Scripter.IronPython;

namespace Threading
{
    public class ViewModel
    {
        private ScriptRun scriptRun = new ScriptRun();
        private MainWindow window;
        private string dir = AppDomain.CurrentDomain.BaseDirectory + @"\";
        private ArrayList bubbleArray = new ArrayList();
        private ArrayList selArray = new ArrayList();
        private ArrayList quickArray = new ArrayList();
        private int step = 3;
        private DispatcherTimer t = new DispatcherTimer();
        private bool threadsRun = false;
        private CancellationTokenSource tokenSource;
        private DisplayPanel panelBubble;
        private DisplayPanel panelSelection;
        private DisplayPanel panelQuick;
        private bool panelCreated = false;

        public ViewModel()
        {
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
            IScriptGlobalItem itmQuickArray = new ScriptGlobalItem("quickArray", quickArray);

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

            t.Interval = TimeSpan.FromMilliseconds(100);
            t.Tick += new EventHandler(DoTick);

            RunSort = new RelayCommand(RunSortClick);
            CancelSort = new RelayCommand(CancelSortClick);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunSort { get; set; }

        public ICommand CancelSort { get; set; }

        public async void StartScript()
        {
            tokenSource = new CancellationTokenSource();
            threadsRun = true;
            RandomizeArrays();
            t.Start();
            if (window != null)
            {
                window.SortButton.IsEnabled = false;
                window.CancelButton.IsEnabled = true;
            }

            var token = tokenSource.Token;
            Task task1 = scriptRun.RunFunctionAsync("BubbleSort", new object[] { token }, token);
            Task task2 = scriptRun.RunFunctionAsync("SelectionSort", new object[] { token }, token);
            Task task3 = scriptRun.RunFunctionAsync("QuickSort", new object[] { token }, token);

            try
            {
                await Task.WhenAll(task1, task2, task3);
            }
            catch (OperationCanceledException)
            {
            }

            tokenSource = null;
            threadsRun = false;
            if (window != null)
            {
                window.CancelButton.IsEnabled = false;
            }
        }

        public void StopScript()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                if (window != null)
                {
                    window.CancelButton.IsEnabled = false;
                    window.SortButton.IsEnabled = true;
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

        private void DoTick(object sender, EventArgs e)
        {
            if ((!threadsRun) && (window != null))
            {
                window.SortButton.IsEnabled = true;
                t.Stop();
            }
        }

        private void RandomizeArrays()
        {
            if (!panelCreated)
                return;
            bubbleArray.Clear();
            selArray.Clear();
            quickArray.Clear();
            Random rnd = new Random();
            int arrCount = (int)(panelBubble.Height / step) - 1;
            int len = (int)panelBubble.Width;
            for (int i = 0; i < arrCount; i++)
                bubbleArray.Add(rnd.Next(len));
            selArray.AddRange(bubbleArray);
            quickArray.AddRange(bubbleArray);
        }

        private DisplayPanel CreatePanel(int colIndex)
        {
            DisplayPanel panel = new DisplayPanel();
            panel.Width = 150;
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
            sourceFileSubPath = "ThreadScriptWpf.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.IronPython";
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

        private void RunSortClick()
        {
            StartScript();
        }

        private void CancelSortClick()
        {
            StopScript();
        }
    }
}