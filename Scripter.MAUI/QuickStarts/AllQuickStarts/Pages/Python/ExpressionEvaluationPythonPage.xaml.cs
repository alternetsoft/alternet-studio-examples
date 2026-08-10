#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Editor.Maui;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Maui;
using Alternet.Scripter.Python;
using Alternet.UI;
using Microsoft.Maui.Layouts;


namespace AllQuickStarts.Scripter.Pages;

public partial class ExpressionEvaluationPythonPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private const string ExpressionPython = "(5+4)*2 - 9/3 + 10 + len(tbExpression.Text)";

    static ExpressionEvaluationPythonPage()
    {
    }

    public ExpressionEvaluationPythonPage()
    {
        InitializeComponent();

        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        scriptRun.ScriptSource.References.Clear();

        scriptRun.ScriptSource.Imports.Add("System");
        scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
        scriptRun.ScriptSource.Imports.Add("Alternet.UI");

        var assemblies = DemoUtils.DefaultMAUIScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", Expression);
        scriptRun.GlobalItems.Clear();
        scriptRun.GlobalItems.Add(item);
        Expression.Text = ExpressionPython;


        ScriptButton.Clicked += RunScriptButton_Click;

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }


    public override View? SettingsPanel => settingsPanel;

    public override SyntaxEditView? SyntaxEdit => null;

    public override string DemoTitle => "Expression Evaluation for Python";

    public void StartScript()
    {
        object obj = scriptRun.EvaluateExpression(Expression.Text);
        if (obj != null)
            MessageBox.Show(obj.ToString());
    }

    private void RunScriptButton_Click(object? sender, EventArgs e)
    {
        StartScript();
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
    }
}