using System;
using System.Diagnostics;
using System.Reflection;

using AllQuickStarts.Scripter.Pages;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Maui;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Maui;

using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.CSharp.RuntimeBinder;

namespace AllQuickStarts.Scripter;

public partial class HomePage : ContentPage
{
    internal static bool ExceptionsLogger { get; set; } = true;

    private static bool noClearScript;

    private readonly VerticalStackLayout stackLayoutCSharp = new();
    private readonly VerticalStackLayout stackLayoutPython = new();
    private readonly VerticalStackLayout stackLayoutTypeScript = new();

    private readonly ScrollView scrollViewCSharp = new();
    private readonly ScrollView scrollViewPython = new();
    private readonly ScrollView scrollViewTypeScript = new();

    private readonly SimpleTabControlView tabControlView = new();

    private Page? currentPage;

    static HomePage()
    {
        Alternet.Editor.AlternetUI.EditorOnMobileHelper.BindToInstanceIfDebug = false;
        Alternet.Editor.AlternetUI.EditorOnMobileHelper.BindToInstanceCreated();

        if (Alternet.UI.App.IsWindowsOS)
        {
        }
        else
        {
            noClearScript = true;
        }

        Alternet.Scripter.ScriptHost.DefaultResolveRecursiveReferences = true;

        Alternet.UI.DebugUtils.ExceptionsLoggerDebugWriteLine = true;

        if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
        {
            ExceptionsLogger = true;
        }

        if (ExceptionsLogger)
        {
            Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
            {
#if ANDROID
                if (e is Java.Lang.IllegalArgumentException)
                {
                    return;
                }
#endif

                if (e is OperationCanceledException)
                {
                    return;
                }

                if (e is XmlException)
                {
                    return;
                }

                if (e is RuntimeBinderException)
                {
                    return;
                }

                if (e is FileNotFoundException)
                {
                    return;
                }

                if (e is ReflectionTypeLoadException)
                {
                    return;
                }

                Nop();
            });

            ExceptionsLogger = false;
        }

        LogContentPage.BindApplicationLog();
        Alternet.UI.App.Log("Application started...");
        Alternet.UI.KnownAssemblies.PreloadReferenced();
        LexerDemoUtils.PreloadAssemblyMetaData();

        EditorTests.Init();

        if (ShowLogButton)
        {
        }
    }

    public HomePage()
    {
        DemoTitleView titleView = new("AlterNET Scripter Samples", this);
        titleView.Label.VerticalTextAlignment = TextAlignment.Center;
        NavigationPage.SetTitleView(this, titleView);

        Alternet.UI.PlessMouse.ShowTestMouseInControl = false;

        if (Alternet.UI.App.IsAndroidOS)
        {
            ErrorHandler.OnError += (e) =>
            {
                Alternet.UI.App.LogError(e.Message);
                e.Handled = true;
            };
        }

        Alternet.UI.DebugUtils.ExceptionsLoggerAppLog = true;

        tabControlView.Header.IsBottomBorderVisible = true;
        tabControlView.Header.IsTopBorderVisible = true;
        tabControlView.Header.BackgroundColor = Alternet.Maui.SimpleTabControlView.AltHeaderBackColor;

        tabControlView.Add("C# and VB", () => scrollViewCSharp);
        tabControlView.Add("Python", () => scrollViewPython);

        if (!noClearScript)
            tabControlView.Add("TypeScript", () => scrollViewTypeScript);

        Content = tabControlView;

        scrollViewCSharp.Content = stackLayoutCSharp;
        scrollViewPython.Content = stackLayoutPython;
        scrollViewTypeScript.Content = stackLayoutTypeScript;

        if (Alternet.UI.App.IsWindowsOS || Alternet.UI.App.IsMacOS)
        {
        }

        AddPage<CallMethodPage>("C# and VB: Call method", PageKind.CSharp, DemoDescriptions.CallMethod);
        AddPage<CustomAssemblyPage>("C# and VB: Custom Assembly", PageKind.CSharp, DemoDescriptions.CustomAssembly);
        AddPage<ExpressionEvaluationPage>(
            "C# and VB: Expression Eval",
            PageKind.CSharp,
            DemoDescriptions.ExpressionEvaluation);

        AddPage<IsolatedScriptPage>("C# and VB: Isolated Script", PageKind.CSharp, DemoDescriptions.IsolatedScript);

        AddPage<MemoryAssemblyPage>("C# and VB: Memory Assembly", PageKind.CSharp, DemoDescriptions.MemoryAssembly);

        AddPage<ObjectReferencePage>("C# and VB: Object reference", PageKind.CSharp, DemoDescriptions.ObjectReference);

        AddPage<PackageReferencePage>("C# and VB: Package Reference", PageKind.CSharp, DemoDescriptions.PackageReference);

        AddPage<ScriptHostObjectPage>("C# and VB: Script Host Object", PageKind.CSharp, DemoDescriptions.ScriptHostObject);

        /* Removed as it does not work. I have no idea on how to repaint control from another thread in MAUI.
        AddPage<ThreadingPage>("C# and VB: Thread Script", PageKind.CSharp, DemoDescriptions.ThreadScript);
        */

        AddPage<CallMethodPythonPage>("Python: Call method", PageKind.Python, DemoDescriptions.CallMethodPython);
        
        AddPage<ObjectReferencePythonPage>(
            "Python: Object reference",
            PageKind.Python,
            DemoDescriptions.ObjectReferencePython);
        
        AddPage<ExpressionEvaluationPythonPage>(
            "Python: Expression evaluation",
            PageKind.Python,
            DemoDescriptions.ExpressionEvaluationPython);

        if (!noClearScript)
        {
            AddPage<CallMethodTypeScriptPage>("TypeScript: Call method", PageKind.TypeScript, DemoDescriptions.CallMethodTypeScript);
            AddPage<ObjectReferenceTypeScriptPage>(
                "TypeScript: Object reference",
                PageKind.TypeScript,
                DemoDescriptions.ObjectReferenceTypeScript);
        }

        if (ShowLogButton)
        {
            AddPage<Alternet.Maui.LogContentPage>(
                "Show logs",
                PageKind.General,
                "This page shows debug related logs.");
        }

        (Content as IView)?.InvalidateArrange();
    }

    public enum PageKind
    {
        CSharp,
        Python,
        TypeScript,
        General,
    }

    internal VerticalStackLayout[] GetParentByPageKind(PageKind kind)
    {
        return kind switch
        {
            PageKind.CSharp => new[] { stackLayoutCSharp },
            PageKind.Python => new[] { stackLayoutPython },
            PageKind.TypeScript => new[] { stackLayoutTypeScript },
            PageKind.General => new[] { stackLayoutCSharp, stackLayoutPython, stackLayoutTypeScript },
            _ => Array.Empty<VerticalStackLayout>(),
        };
    }

    internal static IntPtr ImportResolver(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath)
    {
        return Alternet.UI.AssemblyUtils.NativeLibraryLoad(libraryName, assembly, searchPath);
    }

    public static bool ShowLogButton
    {
        get
        {
            return Alternet.UI.DebugUtils.IsDebugDefined || Alternet.UI.App.IsAndroidOS || true;
        }
    }

    public static void Nop()
    {
    }

    public void AddPage<T>(string text, PageKind kind, string? desc = null)
        where T : Page
    {
        VerticalStackLayout[] parentLayouts = GetParentByPageKind(kind);

        foreach (var layout in parentLayouts)
        {
            Internal(layout);
        }

        void Internal(VerticalStackLayout stackLayout)
        {
            var title = new Label
            {
                Text = text,
                Margin = 10,
                FontAttributes = FontAttributes.Bold,
                LineBreakMode = LineBreakMode.WordWrap,
            };

            title.FontSize *= 1.5;

            ContentView container = new()
            {
                Margin = new Thickness(10, 10, 20, 10),
                Padding = 10,
                MaximumWidthRequest = 600,
            };

            var itemContainer = new VerticalStackLayout();

            container.Content = itemContainer;

            bool hasItems = stackLayout.Children.Count > 0;

            if (hasItems)
            {
                var line = new BoxView()
                {
                    HeightRequest = 1,
                    Color = Colors.Gray,
                    Margin = 5,
                };

                itemContainer.Add(line);
            }

            itemContainer.Add(title);

            var button = new Button
            {
                Text = "Open",
                Margin = new Thickness(10, 10, 10, 0),
                HorizontalOptions = LayoutOptions.Start,
            };

            var description = new Label
            {
                Text = desc,
                Margin = 10,
            };

            itemContainer.Add(description);
            itemContainer.Add(button);

            button.Clicked += (s, e) =>
            {
                (currentPage as IDisposable)?.Dispose();
                currentPage = Activator.CreateInstance<T>();
                Navigation.PushAsync(currentPage);
            };

            stackLayout.Add(container);
        }
    }
}