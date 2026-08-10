using Foundation;

using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace DebuggerIntegration;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
