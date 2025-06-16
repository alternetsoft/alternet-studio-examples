#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using Alternet.Common.DotNet;
using Alternet.Editor;
using Alternet.Editor.TextSource;
using Alternet.Scripter.Python;
using Alternet.Syntax.Parsers.Python;

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
using Python.Runtime;
using Alternet.Common;

namespace AllQuickStarts.Scripter.Pages;

public partial class CallMethodPythonPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDispatcherTimer updateTimer;
    private Stopwatch? updateDeltaStopwatch = new();
    private volatile bool scriptRunning;
    private PythonNETParser pythonParser1 = new PythonNETParser();
    private double currentAngle = 0;
    internal readonly Alternet.UI.PaintActionsControl displayPanel;

    internal string ProjectFolder = "embres:AllQuickStarts.Scripter.Content";

    internal string scriptFileName;

    static CallMethodPythonPage()
    {
    }

    public CallMethodPythonPage()
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

        scriptFileName = "CallMethod.py";

        LoadFile(syntaxEdit1.Source, ProjectFolder + "." + scriptFileName);

        ScriptButton.Clicked += RunScriptButton_Click;

        UpdateButtons();

        scriptRun.ScriptSource.ReferencedFrameworks = Framework.System;

        // This is like using in c#
        scriptRun.ScriptSource.Imports.Add("System");
        scriptRun.ScriptSource.Imports.Add("System.Diagnostics");
        scriptRun.ScriptSource.Imports.Add("Alternet.UI");

        var assemblies = DemoUtils.DefaultMAUIScriptAssemblies;
        scriptRun.ScriptSource.References.Clear();
        foreach (string asm in assemblies)
        {
            scriptRun.ScriptSource.References.Add(asm);
        }

        pythonParser1.CodeEnvironment = scriptRun.CodeEnvironment;
        syntaxEdit1.Lexer = pythonParser1;

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);

        if(scriptRun.ScriptHost is PythonScriptHost pythonHost)
        {
            pythonHost.AfterScopeCreated += (s, e) =>
            {
                var scope = e.ExecutionScope;

                if (scope is null)
                    return;
            };
        }
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Call Python Method";

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

        scriptRun.ScriptSource.FromScriptCode(syntaxEdit1.Text, scriptFileName);

        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                var errors = scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray();
                MessageBox.Show(string.Join("\r\n", errors));
                return;
            }
        }

        try
        {
            scriptRun.Run();
            scriptRunning = true;
            UpdateButtons();
            displayPanel.Refresh();
            updateTimer?.Start();
        }
        catch (PythonException e)
        {
            ReportError(null, e.Message, e.StackTrace);
        }
        catch (Exception e)
        {
            ReportError(null, e.Message, null);
        }

    }

    public void StopScript(bool refreshPanel = true)
    {
        scriptRunning = false;
        UpdateButtons();
        updateTimer?.Stop();
        SetDisplayPanelBackground();
        if(refreshPanel)
            displayPanel.Refresh();
    }

    private void InitEdit()
    {
        syntaxEdit1.Outlining.AllowOutlining = true;
        syntaxEdit1.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit1.Selection.Options
            = syntaxEdit1.Selection.Options | SelectionOptions.SelectBeyondEol;
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

    private void DisplayPanel_Paint(
        AbstractControl sender,
        Alternet.Drawing.Graphics graph,
        RectD rect)
    {
        if (!scriptRunning)
            return;

        try
        {
            scriptRun.RunFunction("OnPaint", new object[] { graph, rect, currentAngle });
            updateDeltaStopwatch?.Restart();
        }
        catch (PythonException e)
        {
            ReportError(graph, e.Message, e.StackTrace);
        }
        catch (Exception e)
        {
            ReportError(graph, e.Message, null);
        }
    }

    void ReportError(Alternet.Drawing.Graphics? graph, string message, string? details)
    {
        if (details is not null)
            message += "\n    " + details + "\n";

        Debug.WriteLine(message);

        if (graph is null)
        {
            MessageBox.Show(message);
        }
        else
        {
            var backColor = DefaultColors.GetWindowBackColor(SystemSettings.AppearanceIsDark);
            var foreColor = DefaultColors.GetWindowForeColor(SystemSettings.AppearanceIsDark);
            graph?.FillRectangle(backColor.AsBrush, (0, 0, 500, 500));
            graph?.DrawText("Error: " + message, Control.DefaultFont, foreColor.AsBrush, (5, 5));
        }

        Alternet.UI.App.AddBackgroundInvokeAction(() => { StopScript(false); });
    }

    private static double CoerceAngle(double x)
    {
        x %= 360;
        if (x < 0)
            x += 360;

        return x;
    }

    private void UpdateTimer_Tick()
    {
        if (!scriptRunning || updateDeltaStopwatch is null)
            return;

        currentAngle += (int)updateDeltaStopwatch.ElapsedMilliseconds * 0.1;
        currentAngle = CoerceAngle(currentAngle);

        updateDeltaStopwatch.Restart();
        displayPanel.Refresh();
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
    }
}