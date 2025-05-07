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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Common.TypeScript.HostObjects;
using Alternet.Scripter;
using Alternet.Scripter.TypeScript;

namespace Threading.TypeScript
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const int ScriptRunCount = 3;
        private ArrayList bubbleArray = new ArrayList();
        private ArrayList selArray = new ArrayList();
        private ArrayList quickArray = new ArrayList();
        private int step = 3;
        private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        private bool threadsRun = false;
        private CancellationTokenSource tokenSource;

        private ScriptRun[] scriptRuns = new ScriptRun[ScriptRunCount];

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "Threading.TypeScript.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private async void StartSortingButton_Click(object sender, EventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            btStartSorting.Enabled = false;
            threadsRun = true;
            RandomizeArrays();
            t.Start();

            btCancel.Enabled = true;

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
            btCancel.Enabled = false;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < ScriptRunCount; i++)
            {
                var scriptRun = new ScriptRun();
                scriptRuns[i] = scriptRun;

                scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies()
                    .AddObject("bubbleArray", bubbleArray)
                    .AddObject("selArray", selArray)
                    .AddObject("quickArray", quickArray)
                    .AddObject("pnBubbleSort", pnBubbleSort)
                    .AddObject("pnSelectionSort", pnSelectionSort)
                    .AddObject("pnQuickSort", pnQuickSort);
            }

            cbLanguages.SelectedIndex = 0;

            RandomizeArrays();

            t.Interval = 100;
            t.Tick += new EventHandler(DoTick);
        }

        private void DoTick(object sender, EventArgs e)
        {
            if (!threadsRun)
            {
                btStartSorting.Enabled = true;
                t.Stop();
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

        private void PaintArray(Graphics graph, ArrayList array)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            int posY = 0;
            int len = 0;
            for (int i = 0; i < array.Count; i++)
            {
                posY += step;
                len = (int)array[i];
                graph.DrawLine(pen, 0, posY, len, posY);
            }
        }

        private void GetSourceParametersForTS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"TS\ThreadScript.ts";
            language = ScriptLanguage.TypeScript;
        }

        private void GetSourceParametersForJS(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = @"JS\ThreadScript.js";
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

            foreach (var scriptRun in scriptRuns)
            {
                scriptRun.ScriptSource.FromScriptFile(sourceFileFullPath);
                scriptRun.ScriptLanguage = language;
            }
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                btCancel.Enabled = false;
                btStartSorting.Enabled = true;
            }
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
    }
}