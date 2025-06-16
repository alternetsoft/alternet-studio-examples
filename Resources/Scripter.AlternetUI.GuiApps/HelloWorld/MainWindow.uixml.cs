using Alternet.UI;

namespace HelloWorld
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
	    mainLabel.Text = $"Hello from Alternet.UI {SystemSettings.Handler.GetUIVersion()}";		
        }
    }
}



