
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace AllQuickStarts.Scripter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var page = new NavigationPage(new HomePage());
            var result = new Window(page);

#if WINDOWS
#endif
            result.Title = "AlterNET Software MAUI Demo";
            return result;
        }
    }
}
