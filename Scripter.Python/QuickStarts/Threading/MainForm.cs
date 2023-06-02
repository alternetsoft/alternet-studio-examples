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
using System.Collections;
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
        private List<int> quickArray = new List<int>();
        private int step = 3;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private bool threadsRun = false;
        private CancellationTokenSource tokenSource;

        public MainForm()
        {
            SetupPython();

            InitializeComponent();
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\Scripter.Python\Demos");

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

        private async void StartSortingButton_Click(object sender, EventArgs e)
        {
            btStartSorting.Enabled = false;
            RandomizeArrays();

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                    MessageBox.Show(string.Join("\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
            }

            threadsRun = true;
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            timer.Start();

            btCancel.Enabled = true;

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
            btCancel.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.ReferencedFrameworks = Framework.System | Framework.WindowsForms;
            scriptRun.ScriptSource.Imports.Add("System");
            scriptRun.ScriptSource.Imports.Add("System.Drawing");
            scriptRun.ScriptSource.Imports.Add("System.Windows.Forms");

            IScriptGlobalItem itmBubbleArray = new ScriptGlobalItem("bubbleArray", bubbleArray);
            IScriptGlobalItem itmSelArray = new ScriptGlobalItem("selArray", selArray);
            IScriptGlobalItem itmQuickArray = new ScriptGlobalItem("quickArray", quickArray);

            IScriptGlobalItem itmBubblePanel = new ScriptGlobalItem("pnBubbleSort", pnBubbleSort);
            IScriptGlobalItem itmSelPanel = new ScriptGlobalItem("pnSelectionSort", pnSelectionSort);
            IScriptGlobalItem itmQuickPanel = new ScriptGlobalItem("pnQuickSort", pnQuickSort);

            scriptRun.GlobalItems.Add(itmBubbleArray);
            scriptRun.GlobalItems.Add(itmSelArray);
            scriptRun.GlobalItems.Add(itmQuickArray);

            scriptRun.GlobalItems.Add(itmBubblePanel);
            scriptRun.GlobalItems.Add(itmSelPanel);
            scriptRun.GlobalItems.Add(itmQuickPanel);

            RandomizeArrays();

            UpdateSource();

            timer.Interval = 100;
            timer.Tick += new EventHandler(DoTick);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource = null;
                btCancel.Enabled = false;
                btStartSorting.Enabled = true;
            }
        }

        private void DoTick(object sender, EventArgs e)
        {
            if (!threadsRun)
            {
                btStartSorting.Enabled = true;
                timer.Stop();
            }
        }

        private void RandomizeArrays()
        {
            bubbleArray.Clear();
            selArray.Clear();
            quickArray.Clear();
            Random rnd = new Random();
            int arrCount = (pnBubbleSort.Height / step) - 1;
            int len = pnBubbleSort.Width;
            for (int i = 0; i < arrCount; i++)
                bubbleArray.Add(rnd.Next(len));
            selArray.AddRange(bubbleArray);
            quickArray.AddRange(bubbleArray);
            Invalidate(true);
        }

        private void PaintArray(Graphics graph, List<int> array)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            int posY = 0;
            int len = 0;
            for (int i = 0; i < array.Count; i++)
            {
                posY += step;
                len = array[i];
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
            PaintArray(e.Graphics, quickArray);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource = null;
                btCancel.Enabled = false;
                btStartSorting.Enabled = true;
            }
        }

        private void GetSourceParametersForPython(out string sourceFileSubPath)
        {
            sourceFileSubPath = "ThreadScript.py";
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter.Python";
            var path = Path.Combine(Application.StartupPath, ResourcesFolderName, sourceFileSubPath);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.StartupPath + @"\..\..\..\..\..\..\", ResourcesFolderName, sourceFileSubPath));
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
    }
}
