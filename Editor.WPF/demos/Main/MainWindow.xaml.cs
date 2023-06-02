#region Copyright (c) 2016-2023 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Alternet.Common.Wpf;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool contentInitialized;
        private IDemoSettingsControl currentDemoSettingsControl;
        private string sampleDir = AppDomain.CurrentDomain.BaseDirectory + @"\";

        public MainWindow()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(sampleDir) + @"Resources");
            if (!dirInfo.Exists)
            {
                sampleDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\..\";
            }

            sampleDir = Path.Combine(sampleDir, @"Resources\Editor");
            ProgrammingLanguagesDemoInitializer.SampleDir = sampleDir;

            InitializeComponent();

            InitializeDemoItems();

            NavigatorListBox.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);

            Editor.VisualTheme = new CustomVisualTheme();
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (NavigatorListBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                var groups = (List<DemoItemsGroup>)DataContext;
                var groupItem = (ListBoxItem)NavigatorListBox.ItemContainerGenerator.ContainerFromItem(groups[0]);
                var childrenListBox = groupItem.FindChild<ListBox>("PART_ChildrenListBox");

                childrenListBox.SelectedIndex = 0;
            }
        }

        private void EnsureContentInitialized()
        {
            if (contentInitialized)
                return;

            ProgrammingLanguagesDemoInitializer.CSharpItemInit(Editor);

            contentInitialized = true;
        }

        private ListBoxItem[] GetAllSelectedNavigatorItems()
        {
            var items = new List<ListBoxItem>();

            foreach (var group in NavigatorListBox.Items)
            {
                var groupItem = (ListBoxItem)NavigatorListBox.ItemContainerGenerator.ContainerFromItem(group);
                var childrenListBox = groupItem.FindChild<ListBox>("PART_ChildrenListBox");

                items.AddRange(childrenListBox.SelectedItems.Cast<object>().Select(
                    x => (ListBoxItem)childrenListBox.ItemContainerGenerator.ContainerFromItem(x)));
            }

            return items.ToArray();
        }

        private void Navigator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var navigator = (ListBox)sender;

            var di = navigator.SelectedItem as DemoItem;
            if (di == null)
                return;

            var groups = (List<DemoItemsGroup>)DataContext;

            foreach (var group in groups)
            {
                foreach (var item in group.Items)
                {
                    if (item.OnDeinitializeEditor != null)
                        item.OnDeinitializeEditor(Editor);
                }
            }

            var s = navigator.ItemContainerGenerator.ContainerFromItem(di);

            var si = GetAllSelectedNavigatorItems();

            foreach (var i in si)
            {
                if (i != s)
                    i.IsSelected = false;
            }

            SettingControlContainer.Visibility = System.Windows.Visibility.Collapsed;

            var oc = Cursor;
            di.OnInitializeEditor(Editor);
        }

        private void InitializeDemoItems()
        {
            var groups = new List<DemoItemsGroup>();

            var editorFeaturesGroup = new DemoItemsGroup { Name = "Editor Features" };
            groups.Add(editorFeaturesGroup);
            {
                var fontItem = new DemoItem { Name = "Font", OnInitializeEditor = FontItemInit };
                editorFeaturesGroup.Items.Add(fontItem);

                var backgroundAndBorderItem = new DemoItem { Name = "Background and Border", OnInitializeEditor = BackgroundAndBorderItemInit };
                editorFeaturesGroup.Items.Add(backgroundAndBorderItem);

                var selectionItem = new DemoItem { Name = "Selection", OnInitializeEditor = SelectionItemInit };
                editorFeaturesGroup.Items.Add(selectionItem);

                var gutterItem = new DemoItem { Name = "Gutter", OnInitializeEditor = GutterItemInit };
                editorFeaturesGroup.Items.Add(gutterItem);

                var outliningItem = new DemoItem { Name = "Outlining", OnInitializeEditor = OutliningItemInit };
                editorFeaturesGroup.Items.Add(outliningItem);

                var wordWrapItem = new DemoItem { Name = "Word Wrap", OnInitializeEditor = WordWrapItemInit };
                editorFeaturesGroup.Items.Add(wordWrapItem);

                var bookmarksItem = new DemoItem { Name = "Bookmarks", OnInitializeEditor = BookmarksItemInit };
                editorFeaturesGroup.Items.Add(bookmarksItem);

                var dialogsItem = new DemoItem { Name = "Dialogs", OnInitializeEditor = DialogsItemInit };
                editorFeaturesGroup.Items.Add(dialogsItem);

                var lineStylesItem = new DemoItem { Name = "Line Styles", OnInitializeEditor = LineStylesItemInit };
                editorFeaturesGroup.Items.Add(lineStylesItem);

                var exportingAndPrintingItem = new DemoItem { Name = "Printing and Exporting", OnInitializeEditor = ExportingAndPrintingItemInit };
                editorFeaturesGroup.Items.Add(exportingAndPrintingItem);

                var appearanceCustomizationItem = new DemoItem { Name = "Appearance Customization", OnInitializeEditor = AppearanceCustomizationItemInit };
                editorFeaturesGroup.Items.Add(appearanceCustomizationItem);

                var themeItem = new DemoItem { Name = "Visual Themes", OnInitializeEditor = ThemeItemInit };
                editorFeaturesGroup.Items.Add(themeItem);

                var otherItem = new DemoItem { Name = "Miscellaneous", OnInitializeEditor = OtherItemInit };
                editorFeaturesGroup.Items.Add(otherItem);

                var hitTestItem = new DemoItem { Name = "Hit Test", OnInitializeEditor = HitTestItemInit, OnDeinitializeEditor = HitTestItemDeinit };
                editorFeaturesGroup.Items.Add(hitTestItem);

                var scrollBarAnnotationsItem = new DemoItem { Name = "Scroll Bar Annotations", OnInitializeEditor = ScrollBarAnnotationsInit, OnDeinitializeEditor = ScrollBarAnnotationsDeinit };
                editorFeaturesGroup.Items.Add(scrollBarAnnotationsItem);
            }

            var languagesWithDedicatedParsersGroup = new DemoItemsGroup { Name = "Syntax Parsing" };
            groups.Add(languagesWithDedicatedParsersGroup);

            ProgrammingLanguagesDemoInitializer.InitializeLanguagesWithParsersDemoGroup(languagesWithDedicatedParsersGroup);

            var programmingLanguagesSupportGroup = new DemoItemsGroup { Name = "Syntax Highlighting" };
            groups.Add(programmingLanguagesSupportGroup);

            var companyInfoGroup = new DemoItemsGroup { Name = "Company Info" };
            groups.Add(companyInfoGroup);

            var aboutItem = new DemoItem { Name = "About", OnInitializeEditor = AboutItemInit };
            companyInfoGroup.Items.Add(aboutItem);

            ProgrammingLanguagesDemoInitializer.InitializeProgrammingLanguagesDemoGroup(programmingLanguagesSupportGroup);

            DataContext = groups;
        }

        private void FontItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new FontSettingsUserControl());
        }

        private void SelectionItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new SelectionSettingsUserControl());
        }

        private void GutterItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new GutterSettingsUserControl());
        }

        private void OutliningItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new OutliningSettingsUserControl());
        }

        private void WordWrapItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new WordWrapSettingsUserControl());
        }

        private void BookmarksItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new BookmarksSettingsUserControl());
        }

        private void LineStylesItemInit(TextEditor editor)
        {
            ProgrammingLanguagesDemoInitializer.CSharpItemInit(Editor);
            SetCurrentDemoSettingsControl(new LineStylesSettingsUserControl());
        }

        private void DialogsItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new DialogsSettingsUserControl());
        }

        private void AppearanceCustomizationItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new AppearanceCustomizationSettingsUserControl());
        }

        private void OtherItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new OtherSettingsUserControl());
        }

        private void AboutItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new AboutBoxUserControl(), System.Windows.Visibility.Collapsed);
        }

        private void ExportingAndPrintingItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new ExportingAndPrintingSettingsUserControl());
        }

        private void ThemeItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new ThemeSettingsUserControl());
        }

        private void BackgroundAndBorderItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new BackgroundAndBorderSettingsUserControl());
        }

        private void HitTestItemInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new HitTestDemoUserControl());
        }

        private void HitTestItemDeinit(TextEditor editor)
        {
            SetCurrentDemoSettingsControl(null);
        }

        private void ScrollBarAnnotationsInit(TextEditor editor)
        {
            EnsureContentInitialized();
            SetCurrentDemoSettingsControl(new ScrollBarAnnotationsUserControl());
        }

        private void ScrollBarAnnotationsDeinit(TextEditor editor)
        {
            SetCurrentDemoSettingsControl(null);
        }

        private void SetCurrentDemoSettingsControl(IDemoSettingsControl control)
        {
            SetCurrentDemoSettingsControl(control, Visibility.Visible);
        }

        private void SetCurrentDemoSettingsControl(IDemoSettingsControl control, Visibility visible)
        {
            if (currentDemoSettingsControl != null)
                currentDemoSettingsControl.Editor = null;

            currentDemoSettingsControl = control;

            if (currentDemoSettingsControl != null)
            {
                SettingControlContainer.Child = control.Control;
                control.Editor = Editor;
                SettingControlContainer.Visibility = System.Windows.Visibility.Visible;
            }
            else
                SettingControlContainer.Visibility = System.Windows.Visibility.Collapsed;
            if (Editor != null)
            {
                Editor.Visibility = visible;
                SettingControlContainer.BorderThickness = (visible == System.Windows.Visibility.Visible) ? new Thickness(0, 0, 0, 1) : new Thickness(0, 0, 0, 0);
            }
        }

        private void GroupListBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double x = (double)e.Delta / 2;

            var sv = NavigatorListBox.FindChild<ScrollViewer>("PART_ScrollViewer");
            if (sv == null)
                return;

            double y = sv.VerticalOffset;

            sv.ScrollToVerticalOffset(y - x);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public class CustomVisualTheme : StandardVisualTheme
        {
            public CustomVisualTheme()
                : base("MyCustomTheme")
            {
            }

            protected override VisualThemeColors GetColors()
            {
                var colors = DarkVisualTheme.Instance.Colors.Clone();
                colors.Reswords = System.Drawing.Color.Red;
                colors.WindowBackground = System.Drawing.Color.FromArgb(40, 40, 40);
                return colors;
            }
        }
    }
}
