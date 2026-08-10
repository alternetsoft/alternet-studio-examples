#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System.Diagnostics;

using Alternet.Common.TypeScript;
using Alternet.Drawing;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.Maui;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter.TypeScript;
using Alternet.Syntax.Parsers.TypeScript;

using Alternet.UI;
using Microsoft.Maui.Layouts;

using Host = Alternet.Common.TypeScript.HostObjects;

namespace AllQuickStarts.Scripter.Pages;

public partial class CallMethodTypeScriptPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDispatcherTimer updateTimer;
    private Stopwatch? updateDeltaStopwatch = new();
    private volatile bool scriptRunning;
    private TextSource? source = new();
    private JavaScriptParser? jsParser1;
    private TypeScriptParser? tsParser1;
    internal readonly Alternet.UI.PaintActionsControl displayPanel;

    internal string CallMethodNoExt = "embres:AllQuickStarts.Scripter.Content.CallMethod";

    static CallMethodTypeScriptPage()
    {
    }

    public CallMethodTypeScriptPage()
    {
        InitializeComponent();

        InitDefaultHostAssemblies();

        jsParser1 = new();
        tsParser1 = new();

        updateTimer = Dispatcher.CreateTimer();
        updateTimer.Interval = TimeSpan.FromMilliseconds(500);
        updateTimer.Tick += (s, e) => UpdateTimer_Tick();

        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        displayPanel = new();
        displayPanel.Name = "displayPanel";

        displayPanel.SetPaintAction((control, canvas, rect) =>
        {
            DisplayPanel_Paint(control, canvas, rect);
        });

        SetDisplayPanelBackground();

        skiaContainer.Control = displayPanel;

        InitEdit();

        source.Lexer = tsParser1;
        LoadFile(source, CallMethodNoExt + ".ts");
        source.HighlightReferences = true;
        syntaxEdit1.Editor.Source = source;

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        ScriptButton.Clicked += RunScriptButton_Click;

        UpdateButtons();

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Call TypeScript Method";

    private static readonly string[] tsExtensions = [".ts"];
    private static readonly string[] jsExtensions = [".js"];

    private bool IsTypeScriptSelected => LanguagesPicker.SelectedIndex == 0;

    public void SetDisplayPanelBackground()
    {
        if (Alternet.UI.ControlView.IsDark)
        {
            displayPanel.BackgroundColor
                = Alternet.Drawing.DefaultColors.WindowBackColor.Dark;
        }
        else
        {
            displayPanel.BackgroundColor = Alternet.Drawing.Color.Gray;
        }
    }

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

        scriptRunning = true;
        UpdateButtons();
        displayPanel.Background = Alternet.Drawing.Brushes.Transparent;

        updateTimer?.Start();
    }

    public void StopScript()
    {
        scriptRunning = false;
        UpdateButtons();
        updateTimer?.Stop();
        SetDisplayPanelBackground();
        displayPanel.Refresh();
    }

    public bool IsScriptRunning()
    {
        return scriptRunning;
    }

    private bool IsJavaScript => LanguagesPicker.SelectedIndex == 1;

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if(LanguagesPicker.SelectedIndex == 1)
        {
            LoadFile(source, CallMethodNoExt + ".js");
            source!.Lexer = jsParser1;
        }
        else
        {
            LoadFile(source, CallMethodNoExt + ".ts");
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
        if (source is null || source.FileName == url)
            return;

        source.Text = string.Empty;
        source.FileName = url;
        source.BookMarks.Clear();
        source.LineStyles.Clear();

        var stream = Alternet.UI.ResourceLoader.StreamFromUrlOrDefault(url);

        if (stream is null || !source.LoadStream(stream))
        {
            source.Text = $"Error loading text: {url}";
            return;
        }
    }

    private void InitDefaultHostAssemblies()
    {
        bool generateDescriptions = true;

        var options = Host.HostItemOptions.GlobalMembers;

        if (generateDescriptions)
            options |= Host.HostItemOptions.GenerateDescriptions;

        var assemblies = DemoUtils.LoadDefaultScriptAssemblies();

        scriptRun.ScriptHost.HostItemsConfiguration.AddAssemblies("core", assemblies, options);

        TypeScriptProject.DefaultProject.HostItemsConfiguration = scriptRun.ScriptHost.HostItemsConfiguration;
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

    private void DisplayPanel_Paint(AbstractControl sender, Alternet.Drawing.Graphics graph, RectD rect)
    {
        if (!scriptRunning)
            return;

        scriptRun.RunFunction(IsJavaScript ? "OnPaintJs" : "OnPaint", new object[] { graph, rect });
        updateDeltaStopwatch?.Restart();
    }

    private void UpdateTimer_Tick()
    {
        if (!scriptRunning || updateDeltaStopwatch is null)
            return;

        scriptRun.RunFunction(
            IsJavaScript ? "OnUpdateJs" : "OnUpdate",
            new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });

        updateDeltaStopwatch.Restart();
        displayPanel.Refresh();
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref source);
        SafeDispose(ref tsParser1);
        SafeDispose(ref jsParser1);
    }
}