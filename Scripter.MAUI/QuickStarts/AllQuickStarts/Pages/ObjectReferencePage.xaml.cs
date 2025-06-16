#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Alternet.UI;
using Alternet.Maui;
using Alternet.Maui.Extensions;
using Microsoft.Maui.Layouts;
using SkiaSharp;
using Microsoft.Maui.Graphics;
using System.Diagnostics;
using Alternet.Editor.Common.AlternetUI;
using System.Drawing;
using AllQuickStarts.Scripter;
using Alternet.Drawing;
using Microsoft.Maui;

namespace AllQuickStarts.Scripter.Pages;

public partial class ObjectReferencePage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDisposable? scriptObject;
    private volatile bool scriptRunning;
    private Alternet.Editor.TextSource.TextSource? cSharpSource = new();
    private Alternet.Editor.TextSource.TextSource? vbSource = new();
    private CsParser? csParser1 = new(new CsSolution());
    private VbParser? vbParser1 = new(new VbSolution());

    internal string ObjectReferenceNoExt = "embres:AllQuickStarts.Scripter.Content.ObjectReference";

    static ObjectReferencePage()
    {
    }

    public ObjectReferencePage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        cSharpSource.Lexer = csParser1;
        LoadFile(cSharpSource, ObjectReferenceNoExt + ".cs");
        cSharpSource.HighlightReferences = true;

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, ObjectReferenceNoExt + ".vb");
        vbSource.HighlightReferences = true;
        syntaxEdit1.Editor.Source = cSharpSource;

        scriptRun.ScriptSource.References.Clear();

        var assemblies = DemoUtils.DefaultMAUIScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        csParser1.Repository.RegisterAssemblies(assemblies);
        vbParser1.Repository.RegisterAssemblies(assemblies);

        AddScriptItem();

        scriptRun.ScriptHost.GenerateModulesOnDisk = false;
        
        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        ScriptButton.Clicked += RunScriptButton_Click;
        TestButton.Clicked += TestButton_Click;

        UpdateButtons();

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Object Reference";

    private static readonly string[] vbExtensions = [".vb"];
    private static readonly string[] csExtensions = [".cs"];

    private bool IsVisualBasicSelected => syntaxEdit1.Source == vbSource;


    public void StartScript()
    {
        StopScript();
        scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);

        if(LanguagesPicker.SelectedIndex == 0)
        {
            scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
        }
        else
        {
            scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
        }

        scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                return;
            }
        }

        try
        {
            scriptObject = scriptRun.Run() as IDisposable;
            scriptRunning = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            StopScript();
        }

        UpdateButtons();
    }

    public void StopScript()
    {
        SafeDispose(ref scriptObject);
        scriptRunning = false;
        TestButton.Text = "Test Button";
        UpdateButtons();
    }

    public bool IsScriptRunning()
    {
        return scriptRunning;
    }

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit1.Source = LanguagesPicker.SelectedIndex switch
        {
            0 => cSharpSource,
            1 => vbSource,
            _ => cSharpSource,
        } ?? new TextSource();
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
    }

    public static void LoadFile(Alternet.Editor.TextSource.TextSource? source, string url)
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

    private void UpdateButtons()
    {
        ScriptButton.Text = scriptRunning ? "Stop Script" : "Start Script";
    }

    private void RunScriptButton_Click(object? sender, EventArgs e)
    {
        if (scriptRunning)
            StopScript();
        else
            StartScript();
    }

    private void TestButton_Click(object? sender, EventArgs e)
    {
        StopScript();
    }

    private void AddScriptItem()
    {
        ScriptGlobalItem item = new("RunButton", typeof(Microsoft.Maui.Controls.Button), TestButton);
        scriptRun.GlobalItems.Clear();
        scriptRun.GlobalItems.Add(item);

        scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
        var s2 = scriptRun.ScriptHost.GlobalCode;
        scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
        var s = scriptRun.ScriptHost.GlobalCode;

        csParser1?.Repository.AddDocument("global.cs", s);
        vbParser1?.Repository.AddDocument("global.vb", s2);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref cSharpSource);
        SafeDispose(ref vbSource);
        SafeDispose(ref csParser1);
        SafeDispose(ref vbParser1);
    }
}