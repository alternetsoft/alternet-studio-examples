using System.Diagnostics;
using System.Reflection;

using AllQuickStarts.Scripter.Pages;

using Alternet.Common;
using Alternet.Editor;
using Alternet.Editor.Common.AlternetUI;
using Alternet.Maui;

using Microsoft.Maui.Controls.Shapes;

namespace AllQuickStarts.Scripter;

public partial class HomePage : ContentPage
{
    private static bool ExceptionsLogger = true;

    private readonly VerticalStackLayout stackLayout = new();
    private readonly ScrollView scrollView = new();

    private Page? currentPage;
    private bool hasItems;

    static HomePage()
    {
        if (Alternet.UI.CommandLineArgs.ParseAndHasArgument("-LogExceptions"))
            ExceptionsLogger = true;

        if (ExceptionsLogger)
        {
            Alternet.UI.DebugUtils.RegisterExceptionsLoggerIfDebug((e) =>
            {
#if ANDROID
            if(e is Java.Lang.IllegalArgumentException)
            {
                return;
            }
#endif
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

        Content = scrollView;

        scrollView.Content = stackLayout;

        if (Alternet.UI.App.IsWindowsOS || Alternet.UI.App.IsMacOS)
        {
        }

        AddPage<CallMethodPage>("Call method", DemoDescriptions.CallMethod);
        AddPage<CallMethodPythonPage>("Call method Python", DemoDescriptions.CallMethodPython);
        AddPage<ObjectReferencePage>("Object reference", DemoDescriptions.ObjectReference);
        AddPage<ObjectReferencePythonPage>("Object reference Python", DemoDescriptions.ObjectReferencePython);

        if (ShowLogButton)
        {
            AddPage<Alternet.Maui.LogContentPage>(
                "Show logs",
                "This page shows debug related logs.");
        }

        (Content as IView)?.InvalidateArrange();
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