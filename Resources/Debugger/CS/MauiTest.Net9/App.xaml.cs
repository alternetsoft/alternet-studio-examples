namespace MauiApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var page = new NavigationPage(new AppShell());
            var result = new Window(page);
            return result;
        }
    }
}
