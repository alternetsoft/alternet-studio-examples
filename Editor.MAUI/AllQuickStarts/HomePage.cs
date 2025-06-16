using System.Diagnostics;
using System.Reflection;
using System.Xml;

using AllQuickStarts.Pages;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Maui;

using Microsoft.Maui.Controls.Shapes;

namespace AllQuickStarts;

public partial class HomePage : ContentPage
{
    private readonly VerticalStackLayout stackLayout = new();
    private readonly ScrollView scrollView = new();

    private Page? currentPage;
    private bool hasItems;

    static HomePage()
    {
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
        DemoTitleView titleView = new("AlterNET Quick Start Projects", this);
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

        Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
        {
#if ANDROID
            if(e is Java.Lang.IllegalArgumentException)
            {
                return;
            }
#endif

            if(e is XmlException)
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

            if(e is TargetInvocationException)
            {
                if (e.InnerException is ReflectionTypeLoadException)
                {
                    return;
                }
            }

            Nop();
        });

        Content = scrollView;

        scrollView.Content = stackLayout;

        if (Alternet.UI.App.IsWindowsOS || Alternet.UI.App.IsMacOS)
        {
            AddPage<RoslynSyntaxParsingPage>(
                "Roslyn Syntax",
                DemoDescriptions.RoslynSyntaxParsing);
        }

        AddPage<AdvancedSyntaxParsingPage>("Advanced Syntax", DemoDescriptions.AdvancedSyntaxParsing);
        AddPage<SyntaxHighlightingPage>("Syntax Highlighting", DemoDescriptions.SyntaxHighlighting);
        AddPage<CodeOutliningPage>("Outlining", DemoDescriptions.CodeOutlining);
        AddPage<GutterPage>("Gutter", DemoDescriptions.Gutter);
        AddPage<HypertextPage>("Hypertext", DemoDescriptions.HyperText);
        AddPage<LineStylesPage>("Line Styles", DemoDescriptions.LineStyle);
        AddPage<MarginPage>("Margin", DemoDescriptions.Margin);
        AddPage<MiscellaneousPage>("Miscellaneous", DemoDescriptions.Miscellaneous);
        AddPage<VisualThemePage>("Visual Theme", DemoDescriptions.VisualTheme);
        AddPage<PowerFXSyntaxParsingPage>("PowerFX Syntax", DemoDescriptions.PowerFXSyntaxParsing);
        AddPage<XamlSyntaxParsingPage>("Xaml Syntax", DemoDescriptions.XamlSyntaxParsing);
        AddPage<SqlDomSyntaxParsingPage>("SQL DOM Syntax", DemoDescriptions.SqlDomSyntaxParsing);

        if (true)
        {
            AddPage<TextMateParsingPage>("TextMate", DemoDescriptions.TextMateSyntaxParsing);
        }

        if (ShowLogButton)
        {
            AddPage<Alternet.Maui.LogContentPage>(
                "Show logs",
                "This page shows debug related logs.");
        }

        (Content as IView).InvalidateArrange();
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

    public void AddPage<T>(string text, string? desc = null)
        where T : Page
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

        if (hasItems)
        {
            var line = new BoxView()
            {
                HeightRequest = 1,
                Color = title.TextColor,
                Margin = 5,
            };

            itemContainer.Add(line);
        }

        itemContainer.Add(title);

        var button = new Button
        {
            Text = "Open",
            Margin = new Thickness(10, 10, 10, 5),
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
        hasItems = true;
    }
}