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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.DotNet;
using Alternet.Common.Python;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace Threading
{
    public partial class MainForm : Form
    {
        private List<int> bubbleArray = new List<int>();
        private List<int> selArray = new List<int>();
        private List<int> quickSortArray = new List<int>();
        private int step = 3;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private bool threadsRun = false;
        private CancellationTokenSource cancellationTokenSource;

        public MainForm()
        {
            SetupPython();

            InitializeComponent();
        }

        private async void StartSorting(int index)
        {
            cancellationTokenSource = new CancellationTokenSource();
            ButtonBubbleSort.Enabled = false;
            ButtonSelectionSort.Enabled = false;
            ButtonQuickSort.Enabled = false;
            threadsRun = true;
            RandomizeArrays(index);
            timer.Start();

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
                    CancelBubble.Enabled = true;
                    break;
                case 1:
                    functionName = "SelectionSort";
                    CancelSelection.Enabled = true;
                    break;
                case 2:
                    functionName = "QuickSort";
                    CancelQuick.Enabled = true;
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
            CancelBubble.Enabled = false;
            CancelSelection.Enabled = false;
            CancelQuick.Enabled = false;
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
                Text = "Call Method Python Demo",
                Message = "Deploying Python and packages...",
                ProgressBarStyle = ProgressBarStyle.Marquee,
            };

            progressDialog.Load += async (_, __) =>
            {
                await Task.Run(async () =>
                {
                    await embeddedPythonInstaller.SetupPython();
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void ButtonBubbleSort_Click(object sender, EventArgs e)
        {
            StartSorting(0);
        }

        private void ButtonSelectionSort_Click(object sender, EventArgs e)
        {
            StartSorting(1);
        }

        private void StartQuickSortingButton_Click(object sender, EventArgs e)
        {
            StartSorting(2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.WindowsForms;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Forms");

            var itmBubbleArray = new ScriptGlobalItem("bubbleArray", bubbleArray);
            var itmSelArray = new ScriptGlobalItem("selArray", selArray);
            var itmQuickArray = new ScriptGlobalItem("quickArray", quickSortArray);

            var itmBubblePanel = new ScriptGlobalItem("pnBubbleSort", pnBubbleSort);
            var itmSelPanel = new ScriptGlobalItem("pnSelectionSort", pnSelectionSort);
            var itmQuickPanel = new ScriptGlobalItem("pnQuickSort", pnQuickSort);

            scriptRun.GlobalItems.Add(itmBubbleArray);
            scriptRun.GlobalItems.Add(itmSelArray);
            scriptRun.GlobalItems.Add(itmQuickArray);

            scriptRun.GlobalItems.Add(itmBubblePanel);
            scriptRun.GlobalItems.Add(itmSelPanel);
            scriptRun.GlobalItems.Add(itmQuickPanel);

            scriptRun.ScriptSource.FromScriptFile(GetSourceFileFullPath());

            RandomizeArrays();

            timer.Interval = 100;
            timer.Tick += new EventHandler(DoTick);
        }

        private void DoTick(object sender, EventArgs e)
        {
            if (!threadsRun)
            {
                ButtonBubbleSort.Enabled = true;
                ButtonSelectionSort.Enabled = true;
                ButtonQuickSort.Enabled = true;
                timer.Stop();
            }
        }

        private void RandomizeArrays(int index)
        {
            var rnd = new Random();
            int arrCount = (pnBubbleSort.Height / step) - 1;
            int len = pnBubbleSort.Width;
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

            Invalidate(true);
        }

        private void RandomizeArrays()
        {
            bubbleArray.Clear();
            selArray.Clear();
            quickSortArray.Clear();
            var rnd = new Random();
            int arrCount = (pnBubbleSort.Height / step) - 1;
            int len = pnBubbleSort.Width;
            for (int i = 0; i < arrCount; i++)
                bubbleArray.Add(rnd.Next(len));
            selArray.AddRange(bubbleArray);
            quickSortArray.AddRange(bubbleArray);

            Invalidate(true);
        }

        private void PaintArray(Graphics graph, List<int> array)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            int posY = 0;
            for (int i = 0; i < array.Count; i++)
            {
                posY += step;
                var len = array[i];
                graph.DrawLine(pen, 0, posY, len, posY);
            }
        }

        private void BubbleSortPanel_Paint(object sender, PaintEventArgs e)
        {
            PaintArray(e.Graphics, bubbleArray);
        }

        private void SelectionSortPanel_Paint(object sender, PaintEventArgs e)
        {
            PaintArray(e.Graphics, selArray);
        }

        private void QuickSortPanel_Paint(object sender, PaintEventArgs e)
        {
            PaintArray(e.Graphics, quickSortArray);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                CancelBubble.Enabled = false;
                CancelSelection.Enabled = false;
                CancelQuick.Enabled = false;
                ButtonBubbleSort.Enabled = true;
                ButtonSelectionSort.Enabled = true;
                ButtonQuickSort.Enabled = true;
            }
        }

        private string GetSourceFileFullPath()
        {
            const string ResourcesFolderName = @"Resources\Scripter.Python\Sorting.py";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName));
                if (!File.Exists(path))
                    throw new Exception("File not found: " + path);
            }

            return path;
        }
    }
}