#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using Alternet.Common;
using Alternet.Common.Projects.DotNet;
using Alternet.Editor.AlternetUI;
using Alternet.Editor.Maui;
using Alternet.Editor.TextSource.AlternetUI;
using Alternet.Scripter;
using Alternet.Syntax.Parsers.Roslyn;
using Alternet.UI;

using Microsoft.CodeAnalysis;
using Microsoft.Maui.Layouts;

namespace AllQuickStarts.Scripter.Pages;

public partial class PackageReferencePage : DemoPage
{
    private readonly ScriptRun scriptRun = new ();
    private readonly Alternet.Syntax.Parsers.Roslyn.CodeCompletion.CsSolution solution = new();
    private static readonly string ProjectFolder
    = "embres:AllQuickStarts.Scripter.Content.PackageReferenceTest";
    private DotNetProject project = new();
    private ProjectId? projectId;

    static PackageReferencePage()
    {
        ScriptHost.DefaultResolveRecursiveReferences = true;
    }

    public PackageReferencePage()
    {
        InitializeComponent();
        AbsoluteLayout.SetLayoutFlags(MainGrid, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(MainGrid, new Rect(0, 0, 1, 1));

        InitEdit();
        ScriptButton.Clicked += RunScriptButton_Click;
        string[] projectFiles =
         [
                "PackageReference.cs",
                "PackageReferenceTest.csproj",
         ];

        var destFolder = PathUtilities.GetTempPathUniquePerApp();

        var extractionResult = Alternet.UI.ResourceLoader.ExtractResourcesSafe(
            ProjectFolder,
            projectFiles,
            destFolder);

        OpenProject(destFolder, "PackageReferenceTest.csproj");

        if (HomePage.ShowLogButton)
            settingsInnerPanel.Children.Add(ShowLogsButton);
    }

    protected DotNetProject Project
    {
        get => project;
        private set => project = value;
    }

    public override SyntaxEditView? SyntaxEdit => syntaxEdit1;

    public override View? SettingsPanel => settingsPanel;

    public override string DemoTitle => "Package Reference";

    private void OpenProject(string? projectPathUrl, string projectName)
    {
        if (projectPathUrl is null || Project is null)
            return;

        var projectFileUrl = Path.Combine(projectPathUrl, projectName);

        if (Project is null)
            return;

        Project.Load(projectFileUrl);

        var projName = project.ProjectName;
        var projPath = project.ProjectFileName;

        projectId = solution.AddProject(projName, projPath);

        scriptRun.ScriptSource.FromScriptProject(projectFileUrl);
        if (Project.Files.Count > 0)
        {
            InitEditor(Project.Files[0]);
        }
    }

    public void StartScript()
    {
        if (!scriptRun.Compiled)
        {
            if (!scriptRun.Compile())
            {
                MessageBox.Show(string.Join("\r\n", scriptRun.ScriptHost.CompilerErrors.Select(x => x.ToString()).ToArray()));
                return;
            }
        }

        scriptRun.RunProcess();
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

    private void InitEditor(string fileName)
    {
        List<string> references = new ();
        foreach (var reference in Project.References)
        {
            if (reference.FullName is null)
                continue;
            references.Add(reference.FullName);
        }

        var parser = new CsParser(solution)
        {
            ProjectName = project.ProjectName,
            FileName = fileName,
        };

        parser.Repository.RegisterDefaultAssemblies(TechnologyEnvironment.System);
        parser.Repository.RegisterAssemblies(references.ToArray());
        syntaxEdit1.Lexer = parser;

        LoadFile(syntaxEdit1.Source, fileName);
    }

    public static void LoadFile(ITextSource? source, string url)
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
    }
}