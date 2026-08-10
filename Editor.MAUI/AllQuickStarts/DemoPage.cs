using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AllQuickStarts.Pages;

using Alternet.Editor;
using Alternet.Editor.Maui;

using Alternet.Maui.Extensions;

using Microsoft.Maui.Controls.Handlers.Items;


namespace AllQuickStarts
{
    public abstract class DemoPage : Alternet.UI.DisposableContentPage
    {
        private readonly DemoTitleView titleView;
        private Button? showLogsButton;

        static DemoPage()
        {

            CollectionViewHandler.Mapper.AppendToMapping("NoBlueSelection", (handler, view) =>
            {
#if WINDOWS
                var nativeListView = handler.PlatformView;
                if (nativeListView == null) return;

                // Apply empty style to remove blue pill
                var style = new Microsoft.UI.Xaml.Style(typeof(Microsoft.UI.Xaml.Controls.ListViewItem));
                nativeListView.ItemContainerStyle = style;
#endif
            });
        }

        public DemoPage()
        {
            titleView = new(DemoTitle, this);
            titleView.BackButton.IsVisible = true;
            titleView.SettingsButton.IsVisible = true;
            NavigationPage.SetTitleView(this, titleView);

            if (Alternet.Common.Consts.IsWindows)
            {
                NavigationPage.SetHasBackButton(this, false);
            }

            this.Loaded += (s, e) =>
            {
                if (Alternet.UI.App.IsDesktopDevice)
                {
                    if (SettingsPanel is not null)
                    {
                        SettingsPanel.IsVisible = true;
                    }
                }
                else
                {
                    titleView.KeyboardButton.IsVisible = true;
                    if (SettingsPanel is not null)
                    {
                        SettingsPanel.IsVisible = false;
                    }
                }

                if (SyntaxEdit is not null)
                {
                    if (!Alternet.UI.App.IsDesktopDevice)
                    {
                        SyntaxEdit.Editor.HasBorder = false;
                        SyntaxEdit.Margin = new(0);
                    }
                    else
                    {
                        SyntaxEdit.Editor.HasBorder = true;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SyntaxEdit?.TrySetFocusWithTimeout();
        }

        protected virtual void OnShowLogsButtonClick(object? sender, EventArgs e)
        {
            Navigation.PushAsync(new Alternet.Maui.LogContentPage());
        }
    }
}


                        
