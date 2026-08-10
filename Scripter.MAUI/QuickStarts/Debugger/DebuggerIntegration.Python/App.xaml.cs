using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

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
                Title = "AlterNET DebuggerIntegration.Python"
            };

            return result;
        }
    }
}
