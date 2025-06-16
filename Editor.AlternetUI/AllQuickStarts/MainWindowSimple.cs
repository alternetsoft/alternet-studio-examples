using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace AllDemos
{
    public class MainWindowSimple : Window
    {
        public static bool AddBuildNumber = false;

        private readonly InternalSamplesPage samplesPage = new();

        public MainWindowSimple()
        {
            Title = $"AlterNET UI Quick Start Projects";

            if (AddBuildNumber)
            {
                Title = $"{Title} {SystemSettings.Handler.GetUIVersion()}";
            }

            MinimumSize = (600, 600);
            StartLocation = WindowStartLocation.CenterScreen;

            samplesPage.Parent = this;

            SetSizeToContent(WindowSizeToContentMode.GrowWidthAndHeight);
        }
    }
}
