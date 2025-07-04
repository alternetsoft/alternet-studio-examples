﻿#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.IO;
using System.Windows.Forms;

using Alternet.Common;
using Alternet.Scripter.IronPython;

namespace ExpressionEvaluation
{
    public partial class MainForm : Form
    {
        private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + len(tbExpression.Text)";

        public MainForm()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "ExpressionEvaluation.Resources";
            Icon = ControlUtilities.LoadIconFromAssembly(asm, $"{prefix}.Icon.ico");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", tbExpression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            tbExpression.Text = ExpressionPython;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            object obj = scriptRun.EvaluateExpression(tbExpression.Text);
            if (obj != null)
                MessageBox.Show(obj.ToString());
        }
    }
}
