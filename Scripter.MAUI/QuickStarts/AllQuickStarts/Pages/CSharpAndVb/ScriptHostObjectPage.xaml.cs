#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using System.Linq;

using Alternet.Editor;
using Alternet.Editor.Maui;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Alternet.UI;
using Alternet.Maui;
using Alternet.Maui.Extensions;
using Microsoft.Maui.Layouts;
using SkiaSharp;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Alternet.Editor.Common.AlternetUI;
using System.Drawing;
using AllQuickStarts.Scripter;
using Alternet.Drawing;
using Microsoft.Maui;

namespace AllQuickStarts.Scripter.Pages;

public partial class ScriptHostObjectPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private TextSource? cSharpSource = new();
    private CsParser? csParser1 = new(new CsSolution(Microsoft.CodeAnalysis.SourceCodeKind.Script, typeof(Globals)));
    internal string ScriptHostObjectNoExt = "embres:AllQuickStarts.Scripter.Content.ScriptHostObject";

    static ScriptHostObjectPage()
    {
    }

    public ScriptHostObjectPage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        cSharpSource.Lexer = csParser1;
        LoadFile(cSharpSource, ScriptHostObjectNoExt + ".csx");
        cSharpSource.HighlightReferences = true;

        syntaxEdit1.Editor.Source = cSharpSource;

        scriptRun.ScriptSource.References.Clear();

        var assemblies = DemoUtils.DefaultMAUIScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        csParser1.Repository.RegisterAssemblies(assemblies);

        scriptRun.ScriptHost.GenerateModulesOnDisk = false;

        ScriptButton.Clicked += RunScriptButton_Click;

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Script Host Object";


    public void StartScript()
    {
        scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);
        scriptRun.ScriptSource.References.Add(typeof(Globals).Assembly.Location); // ???
        scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;

        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                return;
            }
        }

        scriptRun.Run();
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit1.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
        InitScripter();
    }

    public static void LoadFile(TextSource? source, string url)
    {
        if (source is null)
            return;

        source.Text = string.Empty;
        source.BookMarks.Clear();
        source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !source.LoadStream(stream))
        {
            source.Text = $"Error loading text: {url}";
            return;
        }
    }

    private void InitScripter()
    {
        scriptRun.ScriptLanguage = ScriptLanguage.CSharpScript;
        var global = new Globals();
        global.LabelExpression = ExpressionLabel;
        scriptRun.ScriptHost.HostGlobalObject = global;
        var assemblies = DemoUtils.DefaultScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }
    }

    private void RunScriptButton_Click(object? sender, EventArgs e)
    {
        StartScript();
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref cSharpSource);
        SafeDispose(ref csParser1);
    }
}