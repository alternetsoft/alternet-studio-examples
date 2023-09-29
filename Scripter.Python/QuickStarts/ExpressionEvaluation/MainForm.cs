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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Common.Python;
using Alternet.Scripter.Python;
using Alternet.Scripter.Python.Embedded;

namespace ExpressionEvaluation
{
    public partial class MainForm : Form
    {
        private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + len(tbExpression.Text)";

        public MainForm()
        {
            SetupPython();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", tbExpression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            tbExpression.Text = ExpressionPython;
        }

        private void SetupPython()
        {
            var embeddedPythonInstaller = new EmbeddedPythonInstaller();
            embeddedPythonInstaller.InstallPath = Path.Combine(Path.GetTempPath(), @"Alternet.Studio.Demo\Scripter.Python\Demos");

            CodeEnvironment.PythonPath = embeddedPythonInstaller.EmbeddedPythonHome;

            if (embeddedPythonInstaller.IsPythonInstalled(true))
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
                    await embeddedPythonInstaller.SetupPython(true);
                }).ContinueWith(t => progressDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            };

            progressDialog.ShowDialog();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            object obj = scriptRun.EvaluateExpression(tbExpression.Text);
            if (obj != null)
                MessageBox.Show(obj.ToString());
        }
    }
}
