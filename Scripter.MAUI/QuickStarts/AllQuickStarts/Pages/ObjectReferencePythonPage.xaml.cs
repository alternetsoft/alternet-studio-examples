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
using Alternet.Scripter.Python;
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
using Alternet.Syntax.Parsers.Python;
using Python.Runtime;

namespace AllQuickStarts.Scripter.Pages;

public partial class ObjectReferencePythonPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDisposable? scriptObject;
    private volatile bool scriptRunning;
    private IDispatcherTimer timer;

    private PythonNETParser? pythonParser1 = new PythonNETParser();

    internal string ObjectReferenceNoExt = "embres:AllQuickStarts.Scripter.Content.ObjectReference";

    static ObjectReferencePythonPage()
    {
    }

    public ObjectReferencePythonPage()
    {
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(500);

        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        LoadFile(syntaxEdit1.Source, ObjectReferenceNoExt + ".py");

        // This is like using in c#
        scriptRun.ScriptSource.Imports.Add("System");
        scriptRun.ScriptSource.Imports.Add("Microsoft");
        scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
        scriptRun.ScriptSource.Imports.Add("Alternet.UI");

        var assemblies = DemoUtils.DefaultMAUIScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        AddScriptItem();

        pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
        syntaxEdit1.Lexer = pythonParser1;

        if (scriptRun.ScriptHost is PythonScriptHost pythonHost)
        {
            pythonHost.AfterScopeCreated += (s, e) =>
            {
                var scope = e.ExecutionScope;

                if (scope is null)
                    return;
            };
        }

        ScriptButton.Clicked += RunScriptButton_Click;
        TestButton.Clicked += TestButton_Click;

        UpdateButtons();

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Object Reference Python";

    public void StartScript()
    {
        StopScript();
        scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text, "ObjectReference.py");

        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                var errors = scriptRun.ScriptHost.CompilerErrors;
                MessageBox.Show(string.Join("\r\n", errors.Select(x => x.ToString()).ToArray()));
                return;
            }
        }

        scriptObject = scriptRun.Run() as IDisposable;
        try
        {
            scriptRun.RunFunction("Main", new object[] { "Catch me if you can" });
            scriptRunning = true;
        }
        catch (PythonException ex)
        {
            MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            StopScript();
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
        timer.Stop();
        scriptRunning = false;
        TestButton.Text = "Test Button";
        UpdateButtons();
    }

    public bool IsScriptRunning()
    {
        return scriptRunning;
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

    public static void LoadFile(Alternet.Editor.TextSource.ITextSource? source, string url)
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
        ScriptGlobalItem item = new ScriptGlobalItem("RunButton", TestButton);
        ScriptGlobalItem item1 = new ScriptGlobalItem("timer", timer);
        scriptRun.GlobalItems.Clear();
        scriptRun.GlobalItems.Add(item);
        scriptRun.GlobalItems.Add(item1);
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref pythonParser1);
    }
}