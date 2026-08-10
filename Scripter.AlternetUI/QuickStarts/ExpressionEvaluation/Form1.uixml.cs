#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;

using Alternet.UI;

using Alternet.Common;
using Alternet.Scripter;
using System.Linq;
using Alternet.Editor.Common.AlternetUI;

namespace ExpressionEvaluation
{
    public partial class Form1 : Window
    {
        private const string ExpressionCSharp = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
        private const string ExpressionVisualBasic = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
        private ScriptRun scriptRun = new();

        public Form1()
        {
            InitializeComponent();

            scriptRun.ScriptSource.References.Clear();
            var assemblies = DemoUtils.DefaultScriptAssemblies;
            foreach (string asm in assemblies)
            {
                scriptRun.ScriptSource.References.Add(asm);
            }

            scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", typeof(Alternet.UI.TextBox), Expression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            
            cbLanguages.Add("C#");
            cbLanguages.Add("Visual Basic");

            cbLanguages.ValueChanged += LanguagesComboBox_SelectedIndexChanged;

            Form1_Load(this, EventArgs.Empty);
            scriptRun.ScriptHost.GenerateModulesOnDisk = false;

            cbLanguages.Value = "C#";
            ScriptButton.Click += RunScriptButton_Click;

            lbDescription.WordWrap = true;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
        }

        public void StartScript()
        {
            Invoke((Action)(() =>
            {
                object obj = scriptRun.EvaluateExpression(Expression.Text);
                if (obj != null)
                    MessageBox.Show(obj.ToString());
                else
                {
                    if (scriptRun.ScriptHost.CompileFailed)
                    {
                        var errors = scriptRun.ScriptHost.CompilerErrors
                                                .Select(x => x.ToString()).ToArray();
                        MessageBox.Show(string.Join("\r\n", errors));
                    }
                    else
                    {
                        MessageBox.Show("Result of evaluation is null");
                    }
                    
                }
            }));
        }
        private void UpdateSource(int index)
        {
            switch (index)
            {
                case 0:
                    Expression.Text = ExpressionCSharp;
                    scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
                    break;
                default:
                    Expression.Text = ExpressionVisualBasic;
                    scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
                    break;
            }
        }

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            StartScript();
        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = cbLanguages.IndexOfValue;
            if (index is null)
                return;
            UpdateSource(index.Value);
        }
    }
}