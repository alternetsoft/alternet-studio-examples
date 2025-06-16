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

namespace AllQuickStarts.Scripter.Pages;

public partial class CallMethodPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDispatcherTimer updateTimer;
    private Stopwatch? updateDeltaStopwatch = new();
    private volatile bool scriptRunning;
    private Alternet.Editor.TextSource.TextSource? cSharpSource = new();
    private Alternet.Editor.TextSource.TextSource? vbSource = new();
    private CsParser? csParser1 = new(new CsSolution());
    private VbParser? vbParser1 = new(new VbSolution());
    internal readonly Alternet.UI.PaintActionsControl displayPanel;

    internal string CallMethodNoExt = "embres:AllQuickStarts.Scripter.Content.CallMethod";

    static CallMethodPage()
    {
    }

    public CallMethodPage()
    {
        updateTimer = Dispatcher.CreateTimer();
        updateTimer.Interval = TimeSpan.FromMilliseconds(500);
        updateTimer.Tick += (s, e) => UpdateTimer_Tick();

        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        displayPanel = new();
        displayPanel.Name = "displayPanel";

        displayPanel.SetPaintAction((control, canvas, rect) =>
        {
            DisplayPanel_Paint(control, canvas, rect);
        });

        skiaContainer.Interior.Required();
        skiaContainer.Interior.HideScrollBars();

        SetDisplayPanelBackground();

        skiaContainer.Control = displayPanel;

        InitEdit();

        cSharpSource.Lexer = csParser1;
        LoadFile(cSharpSource, CallMethodNoExt + ".cs");
        cSharpSource.HighlightReferences = true;

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, CallMethodNoExt + ".vb");
        vbSource.HighlightReferences = true;
        syntaxEdit1.Editor.Source = cSharpSource;

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        scriptRun.ScriptHost.GenerateModulesOnDisk = false;
        ScriptButton.Clicked += RunScriptButton_Click;

        UpdateButtons();
        scriptRun.ScriptSource.References.Clear();
        var assemblies = DemoUtils.DefaultScriptAssemblies;
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        csParser1?.Repository.RegisterAssemblies(assemblies);
        vbParser1?.Repository.RegisterAssemblies(assemblies);

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Call Method";

    private static readonly string[] vbExtensions = [".vb"];
    private static readonly string[] csExtensions = [".cs"];

    private bool IsVisualBasicSelected => syntaxEdit1.Source == vbSource;

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

    private void DisplayPanel_Paint(AbstractControl sender, Alternet.Drawing.Graphics graph, RectD rect)
    {
        if (!scriptRunning)
            return;

        scriptRun.RunMethod("OnPaint", null, new object[] { graph, rect });
        updateDeltaStopwatch?.Restart();
    }

    private void UpdateTimer_Tick()
    {
        if (!scriptRunning || updateDeltaStopwatch is null)
            return;

        scriptRun.RunMethod("OnUpdate", null, new object[] { (int)updateDeltaStopwatch.ElapsedMilliseconds });
        updateDeltaStopwatch.Restart();
        displayPanel.Refresh();
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