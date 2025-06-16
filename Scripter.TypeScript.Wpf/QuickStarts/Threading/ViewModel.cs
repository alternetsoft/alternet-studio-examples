#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

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

using Alternet.Common;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Scripter;
using Alternet.Scripter.TypeScript;

namespace Threading.TypeScript
{
    public class ViewModel
    {
        private const int ScriptRunCount = 3;

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
        private string lang = string.Empty;
        private ObservableCollection<string> languages = new ObservableCollection<string>();
        private ScriptRun[] scriptRuns = new ScriptRun[ScriptRunCount];

        public ViewModel()
        {
        }

        public ViewModel(MainWindow window)
            : this()
        {
            this.window = window;
            panelBubble = CreatePanel(0);
            panelSelection = CreatePanel(1);
            panelQuick = CreatePanel(2);
            panelCreated = true;

            for (int i = 0; i < ScriptRunCount; i++)
            {
                var scriptRun = new ScriptRun();
                scriptRuns[i] = scriptRun;

                scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies(TechnologyEnvironment.Wpf)
                    .AddObject("bubbleArray", bubbleArray)
                    .AddObject("selArray", selArray)
                    .AddObject("quickArray", quickArray)
                    .AddObject("pnBubbleSort", panelBubble)
                    .AddObject("pnSelectionSort", panelSelection)
                    .AddObject("pnQuickSort", panelQuick);
            }

            RandomizeArrays();

            t.Interval = TimeSpan.FromMilliseconds(100);
            t.Tick += new EventHandler(DoTick);

            RunSort = new RelayCommand(RunSortClick);
            CancelSort = new RelayCommand(CancelSortClick);
            window.Closing += Window_Closing;

            languages.Add("TypeScript");
            languages.Add("JavaScript");
            Language = "TypeScript";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string Language
        {
            get
            {
                return lang;
            }

            set
            {
                if (lang != value)
                {
                    lang = value;
                    OnPropertyChanged("Language");
                    UpdateSource();
                }
            }
        }

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

            foreach (var scriptRun in scriptRuns)
            {
                if (!scriptRun.Compiled)
                {
                    if (!scriptRun.Compile())
                    {
                        MessageBox.Show(scriptRun.ScriptHost.CompilerErrors.First(x => x.Kind == ScriptCompilationDiagnosticKind.Error).ToString());
                        return;
                    }
                }
            }

            Task task1 = scriptRuns[0].RunFunctionAsync("BubbleSort", new object[] { token }, token);
            Task task2 = scriptRuns[1].RunFunctionAsync("SelectionSort", new object[] { token }, token);
            Task task3 = scriptRuns[2].RunFunctionAsync("QuickSort", new object[] { token }, token);

            try
            {
                await Task.WhenAll(task1, task2, task3);
            }
            catch (Exception s)
            {
                if (!FindInnerException(s, inner => inner is OperationCanceledException))
                    throw;
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

        private bool FindInnerException(Exception e, Func<Exception, bool> predicate)
        {
            var innerException = e.InnerException;
            if (innerException == null)
                return false;
            if (predicate(innerException))
                return true;
            return FindInnerException(innerException, predicate);
        }

        private void DoTick(object sender, EventArgs e)
        {
            if ((!threadsRun) && (window != null))
            {
                window.SortButton.IsEnabled = true;
                t.Stop();
            }
            else
            {
                panelBubble.InvalidateVisual();
                panelQuick.InvalidateVisual();
                panelSelection.InvalidateVisual();
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

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\ThreadScriptWPF.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\ThreadScriptWPF.js";
            language = ScriptLanguage.JavaScript;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.TypeScript";
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
            ScriptLanguage language;
            switch (lang)
            {
                case "TypeScript":
                    GetSourceParametersForTS(out sourceFileSubPath, out language);
                    break;
                default:
                    GetSourceParametersForJS(out sourceFileSubPath, out language);
                    break;
            }

            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);

            foreach (var scriptRun in scriptRuns)
            {
                scriptRun.ScriptSource.FromScriptFile(sourceFileFullPath);
                scriptRun.ScriptLanguage = language;
            }
        }

        private void DisplayPanel_OnRendering(object sender, DrawingContext dc)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(sender as DisplayPanel);

            if (sender.Equals(panelBubble))
                OnRenderBubble(dc, bounds);
            if (sender.Equals(panelSelection))
                OnRenderSelection(dc, bounds);
            if (sender.Equals(panelQuick))
                OnRenderQuick(dc, bounds);
        }

        private void OnRenderBubble(DrawingContext dc, Rect bound)
        {
            int myStep = 3;

            for (int i = bubbleArray.Count - 1; i >= 0; i--)
            {
                PaintLine(dc, System.Windows.Media.Brushes.Red, myStep * (i + 1), (int)bubbleArray[i]);
            }
        }

        private void OnRenderSelection(DrawingContext dc, Rect bound)
        {
            int myStep = 3;

            for (int i = selArray.Count - 1; i >= 0; i--)
            {
                PaintLine(dc, System.Windows.Media.Brushes.Red, myStep * (i + 1), (int)selArray[i]);
            }
        }

        private void OnRenderQuick(DrawingContext dc, Rect bound)
        {
            int myStep = 3;

            for (int i = quickArray.Count - 1; i >= 0; i--)
            {
                PaintLine(dc, System.Windows.Media.Brushes.Red, myStep * (i + 1), (int)quickArray[i]);
            }
        }

        private void PaintLine(DrawingContext dc, Brush brush, int y, int len)
        {
            dc.DrawLine(new System.Windows.Media.Pen(brush, 1), new System.Windows.Point(0, y), new System.Windows.Point(len, y));
        }

        private void RunSortClick()
        {
            StartScript();
        }

        private void CancelSortClick()
        {
            StopScript();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StopScript();
        }
    }
}