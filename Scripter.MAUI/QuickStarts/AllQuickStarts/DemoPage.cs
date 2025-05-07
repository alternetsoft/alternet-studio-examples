using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AllQuickStarts.Scripter.Pages;

using Alternet.Editor;

namespace AllQuickStarts.Scripter
{
    public abstract class DemoPage : Alternet.UI.DisposableContentPage
    {
        private readonly DemoTitleView titleView;
        private Button? showLogsButton;

        public DemoPage()
        {
            titleView = new(DemoTitle, this);
            titleView.SettingsButton.IsVisible = true;
            NavigationPage.SetTitleView(this, titleView);

            this.Loaded += (s, e) =>
            {
                if (Alternet.UI.App.IsDesktopDevice)
                {
                    if (SettingsPanel is not null)
                        SettingsPanel.IsVisible = true;
                }
                else
                {
                    titleView.KeyboardButton.IsVisible = true;
                    if (SettingsPanel is not null)
                        SettingsPanel.IsVisible = false;
                }

                if (SyntaxEdit is not null)
                {
                    if (!Alternet.UI.App.IsDesktopDevice)
                    {
                        SyntaxEdit.Interior.HasBorder = false;
                        SyntaxEdit.Margin = new(0);
                    }
                    else
                    {
                        SyntaxEdit.Interior.HasBorder = true;
                        SyntaxEdit.Margin = new(10);
                    }

                    SyntaxEdit.IsVisible = true;
                }

                this.ForceLayout();

                if (SyntaxEdit is not null)
                {
                    SyntaxEdit.Source.MoveTo(0, 0);
                    SyntaxEdit.Scrolling.DoHorizontalScroll(Alternet.UI.ScrollEventType.First, 0);
                }
            };

            titleView.SettingsButtonClick += (s, e) =>
            {
                if (SettingsPanel is not null)
                {
                    SettingsPanel.IsVisible = !SettingsPanel.IsVisible;
                }
            };

            titleView.KeyboardButton.Clicked += (s, e) =>
            {
                SyntaxEdit?.ToggleKeyboard();
            };

            BindingContext = this;
        }

        public abstract View? SettingsPanel { get; }

        public DemoTitleView TitleView => titleView;

        public abstract SyntaxEditView? SyntaxEdit { get; }

        public abstract string DemoTitle { get; }

        public Button ShowLogsButton
        {
            get
            {
                if (showLogsButton is null)
                {
                    showLogsButton = new()
                    {
                        Text = "Show Logs",
                        IsVisible = HomePage.ShowLogButton,
                        Margin = 10,
                        HorizontalOptions = LayoutOptions.Start,
                    };

                    showLogsButton.Clicked += (s, e) =>
                    {
                        OnShowLogsButtonClick(s, e);
                    };
                }

                return showLogsButton;
            }
        }

        protected virtual void OnShowLogsButtonClick(object? sender, EventArgs e)
        {
            Navigation.PushAsync(new Alternet.Maui.LogContentPage());
        }
    }
}


                        
