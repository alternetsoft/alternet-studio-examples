#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Drawing;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

using Alternet.UI;

using Python.Runtime;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Threading.Python
{
    public partial class Form1 : Window
    {
        private ScriptRun scriptRun = new();
        private List<int> bubbleArray = [];
        private List<int> selArray = [];
        private Random rand = new Random();
        private List<int> quickArray = [];
        private int step = 3;
        private System.Threading.CancellationTokenSource? tokenSource;
        private Alternet.UI.Timer? timer = new(50);

        public Form1()
        {
            InitializeComponent();

            SizeI size = (250, 400);

            BabblePanel.SuggestedSize = size;
            SelectionPanel.SuggestedSize = size;
            QuickPanel.SuggestedSize = size;

            BabblePanel.HasBorder = true;
            SelectionPanel.HasBorder = true;
            QuickPanel.HasBorder = true;

            BabblePanel.UseControlColors(true);
            SelectionPanel.UseControlColors(true);
            QuickPanel.UseControlColors(true);

            RandomizeArrays();
            UpdateSource();

            IScriptGlobalItem itmBubbleArray
                = new ScriptGlobalItem("bubbleArray", bubbleArray);
            IScriptGlobalItem itmSelArray
                = new ScriptGlobalItem("selArray", selArray);
            IScriptGlobalItem itmQuickArray
                = new ScriptGlobalItem("quickArray", quickArray);

            IScriptGlobalItem itmContainer
                = new ScriptGlobalItem("ProgressManager", this);

            scriptRun.GlobalItems.Add(itmBubbleArray);
            scriptRun.GlobalItems.Add(itmSelArray);
            scriptRun.GlobalItems.Add(itmQuickArray);
            scriptRun.GlobalItems.Add(itmContainer);

            SortButton.Click += StartSortButton_Click;
            BabblePanel.Paint += (s, e) =>
            {
                BubbleSortPanelPaint(s, e);
            };

            SelectionPanel.Paint += (s, e) =>
            {
                SelectionSortPanelPaint(s, e);
            };
            QuickPanel.Paint += (s, e) =>
            {
                QuickSortPanelPaint(s, e);
            };

            this.Closing += Form1_Closing;

            lbDescription.WordWrap = true;

            SetSizeToContent();

            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in assemblies)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            timer.TickAction = () =>
            {
                BabblePanel.Refresh();
                SelectionPanel.Refresh();
                QuickPanel.Refresh();
            };

            timer.StartRepeated();

            CancelButton.Click += (s, e) =>
            {
                Stop();
            };
        }

        private void Stop()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource = null;
                CancelButton.Enabled = false;
                SortButton.Enabled = true;
            }
        }

        private void Form1_Closing(object? sender, WindowClosingEventArgs e)
        {
            Stop();
        }

        protected override void DisposeManaged()
        {
            timer?.Stop();
            SafeDispose(ref timer);
            base.DisposeManaged();
        }

        public Task HandleErrors(Func<Task?> createTask)
        {
            var result = createTask();
            
            if (result is null)
            {
                if (scriptRun.ScriptHost.CompileFailed)
                {
                    var errors = scriptRun.ScriptHost.CompilerErrors
                                            .Select(x => x.ToString()).ToArray();
                    MessageBox.Show(string.Join("\r\n", errors));
                }
                else
                {
                    MessageBox.Show("Result of RunMethodAsync is null");
                }
            }

            return result ?? Task.CompletedTask;
        }

        public Task RunMethod(string name)
        {
            var token = tokenSource?.Token ?? default;

            return HandleErrors(() =>
            {
                var result = scriptRun.RunFunctionAsync(name, new object?[] { token }, token);
                return result;
            });
        }

        public void Refresh(string? id = null)
        {
            Thread.Sleep(10);
        }

        public async Task StartScript()
        {
            tokenSource = new CancellationTokenSource();
            RandomizeArrays();
            SortButton.IsEnabled = false;
            CancelButton.IsEnabled = true;

            var task1 = RunMethod("BubbleSort");
            var task2 = RunMethod("SelectionSort");
            var task3 = RunMethod("QuickSort");

            try
            {
                await Task.WhenAll(task1, task2, task3).ContinueWith((t) =>
                {
                    Invoke(() =>
                    {
                        tokenSource = null;
                        CancelButton.IsEnabled = false;
                        SortButton.IsEnabled = true;
                    });
                });
            }
            catch (TargetInvocationException tie)
            {
                if (!(tie.InnerException is OperationCanceledException))
                    throw;
            }
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "ThreadScript.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Scripter.AlternetUI";
            var path = DemoUtils.GetResourceFileFullPath(ResourcesFolderName, sourceFileSubPath);
            return path;
        }

        private void UpdateSource()
        {
            string sourceFileSubPath;
            GetSourceParametersForPython(out sourceFileSubPath);
            var sourceFileFullPath = GetSourceFileFullPath(sourceFileSubPath);

            scriptRun.ScriptSource.FromScriptFile(sourceFileFullPath);
        }


        private async void StartSortButton_Click(object? sender, EventArgs e)
        {
            await StartScript();
        }

        private void RandomizeArrays()
        {
            int arrCount = (int)(BabblePanel.Height / step) - 1;

            bubbleArray.Clear();
            selArray.Clear();
            quickArray.Clear();

            Random rnd = new Random();

            int len = (int)BabblePanel.Width;

            for (int i = 0; i < arrCount; i++)
            {
                var v = rnd.Next(len);
                bubbleArray.Add(v);
                selArray.Add(v);
                quickArray.Add(v);
            }

            BabblePanel.Refresh();
            SelectionPanel.Refresh();
            QuickPanel.Refresh();
        }

       private void PaintArray(AbstractControl sender, Alternet.UI.PaintEventArgs e, List<int> array)
        {
            sender.DrawDefaultBackground(e);

            Pen pen = LightDarkColors.Red.AsPen;
            int posY = 0;
            int len = 0;

            for (int i = 0; i < array.Count; i++)
            {
                posY += step;
                len = array[i];
                e.Graphics.DrawLine(pen, 0, posY, len, posY);
            }
        }

        private void BubbleSortPanelPaint(object? sender, Alternet.UI.PaintEventArgs e)
        {
            PaintArray((AbstractControl)sender!, e, bubbleArray);
        }

        private void SelectionSortPanelPaint(object? sender, Alternet.UI.PaintEventArgs e)
        {
            PaintArray((AbstractControl)sender!, e, selArray);
        }
        private void QuickSortPanelPaint(object? sender, Alternet.UI.PaintEventArgs e)
        {
            PaintArray((AbstractControl)sender!, e, quickArray);
        }
    }
}