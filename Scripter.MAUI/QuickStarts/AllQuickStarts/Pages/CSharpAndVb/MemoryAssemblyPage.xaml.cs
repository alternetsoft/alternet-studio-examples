#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System.Reflection;

using Alternet.Editor.Maui;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.Syntax.Parsers.Roslyn.CodeCompletion;

using Alternet.UI;
using Alternet.Maui;
using Microsoft.Maui.Layouts;
using Alternet.Editor.Common.AlternetUI;

namespace AllQuickStarts.Scripter.Pages;

public partial class MemoryAssemblyPage : DemoPage
{
    private ScriptRun scriptRun = new ScriptRun();
    private TextSource? cSharpSource = new();
    private TextSource? vbSource = new();
    private TextSource? cSharpSourceExt = new();
    private TextSource? vbSourceExt = new();

    private CsSolution csSolution = new CsSolution();
    private VbSolution vbSolution = new VbSolution();

    private CsParser? csParser1;
    private VbParser? vbParser1;
    private CsParser? csParserExt;
    private VbParser? vbParserExt;
    private readonly SimpleTabControlView editorsTabControl = new();
    SyntaxEditView syntaxEdit = new SyntaxEditView();
    SyntaxEditView syntaxEditExt = new SyntaxEditView();
    internal string CustomClassNoExt = "embres:AllQuickStarts.Scripter.Content.CustomClass";
    internal string CustomAssemblyTestNoExt = "embres:AllQuickStarts.Scripter.Content.CustomAssemblyTest";

    static MemoryAssemblyPage()
    {
    }

    public MemoryAssemblyPage()
    {
        csParser1 = new(csSolution);
        vbParser1 = new(vbSolution);
        csParserExt = new(csSolution);
        vbParserExt = new(vbSolution);

        InitializeComponent();

        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        SimpleToolBarView.StickyButtonStyle tabStyle;

        tabStyle = SimpleToolBarView.StickyButtonStyle.Border;
        editorsTabControl.Header.StickyStyle = tabStyle;

        MainGrid.Add(editorsTabControl);
        editorsTabControl.Header.IsBottomBorderVisible = true;
        editorsTabControl.Header.IsTopBorderVisible = true;
        InitEdit(syntaxEdit);
        InitEdit(syntaxEditExt);

        syntaxEdit.IsVisible = false;
        syntaxEditExt.IsVisible = false;

        syntaxEdit.SetBorderWidth(0, 0, 0, 0);
        syntaxEdit.Margin = 0;
        syntaxEditExt.SetBorderWidth(0, 0, 0, 0);
        syntaxEditExt.Margin = 0;
        syntaxEditExt.ReadOnly = true;

        cSharpSourceExt.Lexer = csParserExt;
        LoadFile(cSharpSourceExt, CustomClassNoExt + ".cs");
        cSharpSourceExt.HighlightReferences = true;
        cSharpSourceExt.FileName = "CustomClass.cs";

        vbSourceExt.Lexer = vbParserExt;
        LoadFile(vbSourceExt, CustomClassNoExt + ".vb");
        vbSourceExt.FileName = "CustomClass.vb";
        vbSourceExt.HighlightReferences = true;
        syntaxEditExt.Editor.Source = cSharpSourceExt;

        cSharpSource.Lexer = csParser1;
        LoadFile(cSharpSource, CustomAssemblyTestNoExt + ".cs");
        cSharpSource.HighlightReferences = true;
        cSharpSource.FileName = "CustomAssemblyTest.cs";

        vbSource.Lexer = vbParser1;
        LoadFile(vbSource, CustomAssemblyTestNoExt + ".vb");
        vbSource.HighlightReferences = true;
        syntaxEdit.Editor.Source = cSharpSource;
        vbSource.FileName = "CustomAssemblyTest.vb";

        editorsTabControl.Add(GetPageTitle(syntaxEdit), () => syntaxEdit);
        editorsTabControl.Add(GetPageTitle(syntaxEditExt), () => syntaxEditExt);

        this.scriptRun.ReferenceResolve += ScriptRun_ReferenceResolve;

        AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
        {
            if (ea.Name.Contains("ExternalAssembly"))
                return Assembly.Load(ReadFully(Assembly.GetExecutingAssembly().GetManifestResourceStream("AllQuickStarts.Scripter.ExternalAssembly.dll")));

            return null;
        };

        LanguagesPicker.SelectedIndex = 0;
        LanguagesPicker.SelectedIndexChanged += LanguagesPicker_SelectedIndexChanged;

        ScriptButton.Clicked += RunScriptButton_Click;

        var assemblies = DemoUtils.DefaultScriptAssemblies;

        scriptRun.ScriptSource.SearchPaths.Add(Microsoft.Maui.Storage.FileSystem.AppDataDirectory);


        csParser1?.Repository.RegisterAssemblies(assemblies);
        vbParser1?.Repository.RegisterAssemblies(assemblies);
        scriptRun.ScriptSource.VisualBasicMyType = VisualBasicMyType.Empty;

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);

        RaiseSystemColorsChanged();

        editorsTabControl.SelectedTabChanged += (s, e) =>
        {
        };
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit;

    public override SyntaxEditView? SyntaxEditExt => syntaxEditExt;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Memory Assembly";

    public static byte[] ReadFully(Stream? input)
    {
        if(input == null)
            return Array.Empty<byte>();

        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }

    public void RaiseSystemColorsChanged()
    {
        editorsTabControl.Header.BackgroundColor = SimpleTabControlView.AltHeaderBackColor;
    }

    public void StartScript()
    {
        if (LanguagesPicker.SelectedIndex == 0)
        {
            scriptRun.ScriptLanguage = ScriptLanguage.CSharp;
        }
        else
        {
            scriptRun.ScriptLanguage = ScriptLanguage.VisualBasic;
        }

        scriptRun.ScriptSource.FromScriptCode(syntaxEdit.Text);

        scriptRun.ScriptSource.SetReferences(DemoUtils.DefaultScriptAssemblies);

        string[] projectFiles =
            [
                    "ExternalAssembly.dll",
            ];

        scriptRun.ScriptSource.References.Add("ExternalAssembly");
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
    protected virtual string GetPageTitle(SyntaxEditView editor)
    {
        return Path.GetFileNameWithoutExtension(editor.Editor.Source.FileName)
            + (editor.Editor.Modified ? "*" : string.Empty);
    }

    private void LanguagesPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        syntaxEdit.Source = LanguagesPicker.SelectedIndex switch
        {
            0 => cSharpSource,
            1 => vbSource,
            _ => cSharpSource,
        } ?? new TextSource();
        syntaxEditExt.Source = LanguagesPicker.SelectedIndex switch
        {
            0 => cSharpSourceExt,
            1 => vbSourceExt,
            _ => cSharpSourceExt,
        } ?? new TextSource();

    }

    private void InitEdit(SyntaxEditView syntaxEdit)
    {
        syntaxEdit.Outlining.AllowOutlining = true;
        syntaxEdit.Gutter.Options |= GutterOptions.PaintLineNumbers
            | GutterOptions.PaintLineModificators
            | GutterOptions.PaintCodeActions
            | GutterOptions.PaintLinesBeyondEof;
        syntaxEdit.Selection.Options = syntaxEdit.Selection.Options | SelectionOptions.SelectBeyondEol;
        syntaxEdit.Gutter.Options &= ~GutterOptions.PaintCodeActionsOnGutter;
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

    private void RunScriptButton_Click(object? sender, EventArgs e)
    {
        StartScript();
    }

    protected override void DisposeResources()
    {
        base.DisposeResources();
        SafeDispose(ref cSharpSource);
        SafeDispose(ref vbSource);
        SafeDispose(ref csParser1);
        SafeDispose(ref vbParser1);
    }

    private void ScriptRun_ReferenceResolve(object? sender, Alternet.Scripter.ResolveReferenceEventArgs e)
    {
        if (e.Reference == "ExternalAssembly")
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AllQuickStarts.Scripter.ExternalAssembly.dll");
            byte[] bytes = ReadFully(stream);

            e.AssemblyImage = bytes;
            e.Handled = true;
        }
    }
}