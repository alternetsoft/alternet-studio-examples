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
using Alternet.Maui;
using Alternet.Scripter;
using Alternet.UI;
using Microsoft.Maui.Layouts;


namespace AllQuickStarts.Scripter.Pages;

public partial class ExpressionEvaluationPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private const string ExpressionCSharp = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";
    private const string ExpressionVisualBasic = "(5+4)*2 - 9/3 + 10 + tbExpression.Text.Length";

    static ExpressionEvaluationPage()
    {
    }

    public ExpressionEvaluationPage()
    {
        InitializeComponent();

        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        scriptRun.ScriptHost.GenerateModulesOnDisk = false;
        scriptRun.ScriptSource.WithDefaultReferences();
        scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
        ScriptGlobalItem item = new ScriptGlobalItem("tbExpression", typeof(Microsoft.Maui.Controls.Entry), Expression);
        scriptRun.GlobalItems.Clear();
        scriptRun.GlobalItems.Add(item);
        scriptRun.ScriptSource.VisualBasicMyType = VisualBasicMyType.Empty;

        ScriptButton.Clicked += RunScriptButton_Click;

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }


    public override View? SettingsPanel => settingsPanel;

    public override SyntaxEditView? SyntaxEdit => null;

    public override string DemoTitle => "Expression Evaluation";

    public void StartScript()
    {
        object obj = scriptRun.EvaluateExpression(Expression.Text);
        if (obj != null)
            MessageBox.Show(obj.ToString());
    }

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        switch (LanguagesPicker.SelectedIndex)
        {
            case 0:
            default:
                Expression.Text = ExpressionCSharp;
                scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
                break;
            case 1:
                Expression.Text = ExpressionVisualBasic;
                scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
                break;
        }
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