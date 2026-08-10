#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using Alternet.Common.TypeScript;
using Alternet.Editor.Maui;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter.TypeScript;
using Alternet.Syntax.Parsers.TypeScript;

using Alternet.UI;
using Alternet.Editor.Common.AlternetUI;
using Microsoft.Maui.Layouts;

using Host = Alternet.Common.TypeScript.HostObjects;

namespace AllQuickStarts.Scripter.Pages;

public partial class ObjectReferenceTypeScriptPage : DemoPage
{
    private readonly ScriptRun scriptRun = new();

    private Alternet.UI.Timer? timer = new();
    private dynamic? catcher;
    private volatile bool scriptRunning;
    private TextSource? source = new();
    private JavaScriptParser? jsParser1;
    private TypeScriptParser? tsParser1;

    internal string ObjectReferenceNoExt = "embres:AllQuickStarts.Scripter.Content.ObjectReference";

    static ObjectReferenceTypeScriptPage()
    {
    }

    public ObjectReferenceTypeScriptPage()
    {
        InitializeComponent();

        InitDefaultHostAssemblies();

        jsParser1 = new();
        tsParser1 = new();

        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();

        source.Lexer = tsParser1;
        source.HighlightReferences = true;

        syntaxEdit1.Editor.Source = source;

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        ScriptButton.Clicked += RunScriptButton_Click;
        TestButton.Clicked += TestButton_Click;

        UpdateButtons();

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);

        timer.Interval = 500;
        timer.TickAction = () =>
        {
            if (scriptRunning && catcher is not null)
            {
                try
                {
                    catcher.ChangeButtonLocation();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    StopScript();
                }
            }
        };

        TestButton.Clicked+=(s,e) =>
        {
            StopScript();
        };

        ScriptButton.IsEnabled = false;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Object Reference TypeScript";

    private bool IsJavaScript => LanguagesPicker.SelectedIndex == 1;

    public void StartScript()
    {
        StopScript();
        scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text);

        if(LanguagesPicker.SelectedIndex == 0)
        {
            scriptRun.ScriptLanguage = ScriptLanguage.TypeScript;
        }
        else
        {
            scriptRun.ScriptLanguage = ScriptLanguage.JavaScript;
        }

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
            catcher = scriptRun.RunFunction("RunMe");
            scriptRunning = true;
            timer!.StartRepeated();
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
        if (!scriptRunning)
            return;
        timer!.Stop();
        scriptRunning = false;

        if (catcher != null)
            catcher.Dispose();
        catcher = null;

        UpdateButtons();

        TestButton.Text = "Script Stopped";
    }

    public bool IsScriptRunning()
    {
        return scriptRunning;
    }

    /// <summary>
    /// Called once when the page appears for the first time.
    /// </summary>
    protected override void OnAppearingOnce()
    {
        LoadFile(source, ObjectReferenceNoExt + ".ts");
        ScriptButton.IsEnabled = true;
    }

    private void InitDefaultHostAssemblies()
    {
        bool generateDescriptions = true;

        var options = Host.HostItemOptions.GlobalMembers;

        if (generateDescriptions)
            options |= Host.HostItemOptions.GenerateDescriptions;

        var assemblies = DemoUtils.LoadDefaultScriptAssemblies();

        scriptRun.ScriptHost.HostItemsConfiguration.AddAssemblies("core", assemblies, options)
        .AddObject("autoRand", new System.Random())
        .AddObject("RunButton", TestButton);

        TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
    }

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (LanguagesPicker.SelectedIndex == 1)
        {
            LoadFile(source, ObjectReferenceNoExt + ".js");
            source!.Lexer = jsParser1;
        }
        else
        {
            LoadFile(source, ObjectReferenceNoExt + ".ts");
            source!.Lexer = tsParser1;
        }
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

    protected override void DisposeResources()
    {
        timer?.Stop();
        SafeDispose(ref timer);
        base.DisposeResources();
        SafeDispose(ref source);
        SafeDispose(ref tsParser1);
        SafeDispose(ref jsParser1);
    }
}