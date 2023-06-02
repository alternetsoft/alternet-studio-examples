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
using System.Windows.Forms;

using Alternet.Scripter;

namespace ExpressionEvaluation
{
    public partial class MainForm : Form
    {
        private const string LanguageDescription = "Choose programming language";
        private const string ExpressionCSharp = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
        private const string ExpressionVisualBasic = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scriptRun.ScriptSource.WithDefaultReferences();
            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", typeof(System.Windows.Forms.TextBox), tbExpression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);

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
                    tbExpression.Text = ExpressionCSharp;
                    scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
                    break;
                default:
                    tbExpression.Text = ExpressionVisualBasic;
                    scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
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
