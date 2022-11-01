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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Alternet.Common.TypeScript.HostObjects;

namespace ExpressionEvaluation.TypeScript
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string ExpressionTS = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.length";
        private const string ExpressionJS = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.length";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            scriptRun.ScriptHost.HostItemsConfiguration.AddSystemAssemblies()
                .AddObject("tbExpression", tbExpression);
            cbLanguages.SelectedIndex = 0;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            object obj = scriptRun.EvaluateExpression(tbExpression.Text);
            if (obj != null)
                MessageBox.Show(obj.ToString());
        }

        private void UpdateSource(int index)
        {
            switch (index)
            {
                case 0:
                    tbExpression.Text = ExpressionTS;
                    break;
                default:
                    tbExpression.Text = ExpressionJS;
                    break;
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
    }
}
