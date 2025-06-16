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
