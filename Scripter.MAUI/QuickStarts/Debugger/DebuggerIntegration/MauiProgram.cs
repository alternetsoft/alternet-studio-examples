using Alternet.Editor;
using Alternet.Editor.Maui;
using Alternet.Maui;
using Alternet.UI;

using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace DebuggerIntegration;

public static class MauiProgram
{
    static MauiProgram()
    {
        BaseLogView.CreateLogView = () =>
        {
            return new SyntaxEditLogView();
        };
    }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit()
            .UseAlternetEditor()
            .UseAlternetUI();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}