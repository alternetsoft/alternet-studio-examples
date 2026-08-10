#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using AllQuickStarts.Scripter;
using Alternet.Drawing;
using Alternet.Editor;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Editor.Maui;
using Alternet.Editor.TextSource;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Maui.Extensions;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;
//using Alternet.Maui;
using Alternet.UI;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace AllQuickStarts.Scripter.Pages;

public partial class IsolatedScriptPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private IDispatcherTimer updateTimer;
    private Stopwatch? updateDeltaStopwatch = new();
    private volatile bool scriptRunning;
    private TextSource? cSharpSource = new();
    private TextSource? vbSource = new();
    private CsParser? csParser1 = new(new CsSolution());
    private VbParser? vbParser1 = new(new VbSolution());
    internal readonly Alternet.UI.PaintActionsControl displayPanel;
    private Isolated<IsolatedScriptRun>? isolated;
    private ScriptLanguage language;

    internal string CallIsolatedMethodNoExt = "embres:AllQuickStarts.Scripter.Content.CallIsolatedMethod";

    static IsolatedScriptPage()
    {
    }

    public IsolatedScriptPage()
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

        SetDisplayPanelBackground();

        skiaContainer.Control = displayPanel;

        InitEdit();

        cSharpSource.Lexer = csParser1;
        LoadFile(cSharpSource, CallIsolatedMethodNoExt + ".cs");
        cSharpSource.HighlightReferences = true;

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, CallIsolatedMethodNoExt + ".vb");
        vbSource.HighlightReferences = true;
        syntaxEdit1.Editor.Source = cSharpSource;

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        ScriptButton.Clicked += RunScriptButton_Click;

        UpdateButtons();
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

    public override string DemoTitle => "Isolated Script";

    private static readonly string[] vbExtensions = [".vb"];
    private static readonly string[] csExtensions = [".cs"];

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
        isolated = new Isolated<IsolatedScriptRun>();
        try
        {
            MyObject myObject = new MyObject(laAngle);
            isolated.Value?.StartScript(syntaxEdit1.Text, myObject, language);
        }
        catch
        {
        }

        scriptRunning = true;
        UpdateButtons();
        displayPanel.Background = Alternet.Drawing.Brushes.Transparent;

        updateTimer?.Start();
    }

    public void StopScript()
    {
        isolated?.Dispose();
        isolated = null;

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

        language = LanguagesPicker.SelectedIndex switch
        {
            0 => ScriptLanguage.CSharp,
            1 => ScriptLanguage.VisualBasic,
            _ => ScriptLanguage.CSharp,
        };
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

    private void DisplayPanel_Paint(AbstractControl sender, Alternet.Drawing.Graphics graph, RectD rect)
    {
        if (!scriptRunning)
            return;

        if (isolated != null)
        {
            isolated.Value?.RunScript(graph, rect);
        }

        updateDeltaStopwatch?.Restart();
    }

    private void UpdateTimer_Tick()
    {
        if (!scriptRunning || updateDeltaStopwatch is null)
            return;

        if (isolated != null)
        {
            isolated.Value?.UpdateScript((int)updateDeltaStopwatch.ElapsedMilliseconds);
        }

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

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
public class MyObject : MarshalByRefObject
{
    private Microsoft.Maui.Controls.Label label;

    public MyObject(Microsoft.Maui.Controls.Label label)
    {
        this.label = label;
    }

    public void UpdateCurrentAngle(double currentAngle)
    {
        label.Text = Math.Floor(currentAngle).ToString();
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
public class IsolatedScriptRun : MarshalByRefObject
{
    private ScriptRun scriptRun = new ScriptRun();

    public void StartScript(string scriptText, MyObject myObject, ScriptLanguage language)
    {
        scriptRun.ScriptLanguage = language;
        scriptRun.ScriptHost.GenerateModulesOnDisk = false;
        scriptRun.ScriptSource.FromScriptCode(scriptText);

        scriptRun.ScriptSource.VisualBasicMyType = VisualBasicMyType.Empty;

        scriptRun.ScriptSource.SetReferences(DemoUtils.DefaultScriptAssemblies);
            
        scriptRun.AssemblyKind = ScriptAssemblyKind.DynamicLibrary;
        AddScriptItem(myObject);
        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                Debug.WriteLine(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                return;
            }
        }
    }

    public void RunScript(Graphics graph, RectD rect)
    {
        scriptRun.RunMethod("OnPaint", null, new object[] { graph, rect });
    }

    public void UpdateScript(int sec)
    {
        scriptRun.RunMethod("OnUpdate", null, new object[] { sec });
    }

    private void AddScriptItem(MyObject myObject)
    {
        ScriptGlobalItem item = new ScriptGlobalItem("MyObject", typeof(MyObject), myObject);
        scriptRun.GlobalItems.Clear();
        scriptRun.GlobalItems.Add(item);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
public partial class Isolated<T> : IDisposable
    where T : MarshalByRefObject
{
    private readonly AssemblyLoadContext assemblyLoadContext;

    private T? value;

    public Isolated()
    {
        assemblyLoadContext = new IsolatedAssemblyLoadContext(name: "Isolated:" + Guid.NewGuid(), isCollectible: true);
        Type type = typeof(T);
        var asm = assemblyLoadContext.LoadFromAssemblyName(type.Assembly.GetName());
        value = (T?)asm.CreateInstance(type.FullName!);
    }

    public T? Value
    {
        get
        {
            return value;
        }
    }

    public void Dispose()
    {
        assemblyLoadContext.Unload();
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "All Isolated classes in the same unit")]
public class IsolatedAssemblyLoadContext : AssemblyLoadContext
{
    public IsolatedAssemblyLoadContext(string name, bool isCollectible = false)
        : base(name, isCollectible: true)
    {
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        return Default.Assemblies
            .FirstOrDefault(x => x.FullName == assemblyName.FullName);
    }
}

