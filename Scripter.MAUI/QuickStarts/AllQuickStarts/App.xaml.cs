
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
            result.Title = "AlterNET Software MAUI Demo";
            return result;
        }
    }
}
