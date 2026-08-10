using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace DebuggerIntegration
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var page = new AppShell();

            var result = new Window(page)
            {
                Title = "AlterNET DebuggerIntegration",
            };

#if WINDOWS
            result.Destroying += (_, __) =>
            {
                var winuiWindow = result.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
                if (winuiWindow != null)
                {
                    winuiWindow.Activated -= OnActivated;
                }
                result.HandlerChanged -= OnHandlerChanged;
            };

            void OnActivated(object? sender, Microsoft.UI.Xaml.WindowActivatedEventArgs e)
            {
                var winuiWindow = result.Handler?.PlatformView as Microsoft.UI.Xaml.Window;

                if (winuiWindow != null)
                {
                    winuiWindow.Activated -= OnActivated;

                    if (winuiWindow.Content is Microsoft.UI.Xaml.FrameworkElement root)
                    {
                        root.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
                    }
                }
            }

            void OnHandlerChanged(object? sender, EventArgs e)
            {
                if (result.Handler is null)
                    return;

                var winuiWindow = result.Handler.PlatformView as Microsoft.UI.Xaml.Window;
                if (winuiWindow != null)
                {
                    winuiWindow.Closed += (_, __) =>
                    {
                        winuiWindow.Activated -= OnActivated;
                    };

                    winuiWindow.Activated += OnActivated;
                }
            }

            result.HandlerChanged += OnHandlerChanged;
#endif

            return result;
        }
    }
}