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
using System.Linq;

using Alternet.UI;

using Alternet.Scripter.Python;

using Alternet.Editor.Common.AlternetUI;

namespace ExpressionEvaluation.Python
{
    public partial class Form1 : Window
    {
        private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + len(tbExpression.Text)";
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

            ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", Expression);
            scriptRun.GlobalItems.Clear();
            scriptRun.GlobalItems.Add(item);
            Expression.Text = ExpressionPython;


            Form1_Load(this, EventArgs.Empty);
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

        private void RunScriptButton_Click(object? sender, EventArgs e)
        {
            StartScript();
        }
    }
}