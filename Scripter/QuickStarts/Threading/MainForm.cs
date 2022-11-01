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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Scripter;

namespace Threading
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private ArrayList bubbleArray = new ArrayList();
        private ArrayList selArray = new ArrayList();
        private ArrayList quickArray = new ArrayList();
        private int step = 3;
        private System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        private bool threadsRun = false;
        private CancellationTokenSource tokenSource;

        public MainForm()
        {
            InitializeComponent();
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

            if (!scriptRun.Compiled)
            {
                if (!scriptRun.Compile())
                    MessageBox.Show(string.Join("\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
            }

            Task task1 = scriptRun.RunMethodAsync("BubbleSort", null, new object[] { token }, token);
            Task task2 = scriptRun.RunMethodAsync("SelectionSort", null, new object[] { token }, token);
            Task task3 = scriptRun.RunMethodAsync("QuickSort", null, new object[] { token }, token);

            try
            {
                await Task.WhenAll(task1, task2, task3);
            }
            catch (TargetInvocationException tie)
            {
                if (!(tie.InnerException is OperationCanceledException))
                    throw;
            }

            tokenSource = null;
            threadsRun = false;
            btCancel.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.WithDefaultReferences(ScriptTechnologyEnvironment.WindowsForms);
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

            IScriptGlobalItem itmBubbleArray = new ScriptGlobalItem("bubbleArray", typeof(ArrayList), bubbleArray);
            IScriptGlobalItem itmSelArray = new ScriptGlobalItem("selArray", typeof(ArrayList), selArray);
            IScriptGlobalItem itmQuickArray = new ScriptGlobalItem("quickArray", typeof(ArrayList), quickArray);

            IScriptGlobalItem itmBubblePanel = new ScriptGlobalItem("pnBubbleSort", typeof(Panel), pnBubbleSort);
            IScriptGlobalItem itmSelPanel = new ScriptGlobalItem("pnSelectionSort", typeof(Panel), pnSelectionSort);
            IScriptGlobalItem itmQuickPanel = new ScriptGlobalItem("pnQuickSort", typeof(Panel), pnQuickSort);

            scriptRun.GlobalItems.Add(itmBubbleArray);
            scriptRun.GlobalItems.Add(itmSelArray);
            scriptRun.GlobalItems.Add(itmQuickArray);

            scriptRun.GlobalItems.Add(itmBubblePanel);
            scriptRun.GlobalItems.Add(itmSelPanel);
            scriptRun.GlobalItems.Add(itmQuickPanel);

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
                btCancel.Enabled = false;
                btStartSorting.Enabled = true;
            }
        }

        private void GetSourceParametersForCSharp(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ThreadScript.cs";
            language = ScriptLanguage.CSharp;
        }

        private void GetSourceParametersForVisualBasic(out string sourceFileSubPath, out ScriptLanguage language)
        {
            sourceFileSubPath = "ThreadScript.vb";
            language = ScriptLanguage.VisualBasic;
        }

        private string GetSourceFileFullPath(string sourceFileSubPath)
        {
            const string ResourcesFolderName = @"Resources\Scripter";
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
