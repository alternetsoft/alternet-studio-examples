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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    /// <summary>
    /// Interaction logic for ScrollBarAnnotationsUserControl.xaml
    /// </summary>
    public partial class ScrollBarAnnotationsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private ScrollBarAnnotationTypeAppearance customErrorAppearance;
        private ScrollBarAnnotationTypeAppearance defaultErrorAppearance;
        private Style systemScrollBarStyle;

        public ScrollBarAnnotationsUserControl()
        {
            InitializeComponent();
        }

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        public TextEditor Editor
        {
            get
            {
                return editor;
            }

            set
            {
                if (editor == value)
                    return;

                editor = value;

                if (editor != null)
                    UpdateDataFromEditor();
            }
        }

        private void UpdateDataFromEditor()
        {
            InitializeCustomAnnotationsDemo();
            InitializeErrorAppearanceDemo();
            InitializeSystemScrollBarStyle();

            EnableOrDisableAllAnnotations();
        }

        private void InitializeSystemScrollBarStyle()
        {
            systemScrollBarStyle = new Style(typeof(ScrollBar), (Style)FindResource(typeof(ScrollBar)));
        }

        private void Annotations_CustomAnnotationsRequested(object sender, ScrollBarCustomAnnotationsEventArgs e)
        {
            if (CustomAnnotationsCheckBox.IsChecked.Value)
                e.Annotations = GetCustomAnnotations();
        }

        private void AnnotationTypeCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;

            var annotations = Editor.Scrolling.Annotations;
            var enabledKinds = annotations.EnabledAnnotationKinds;

            Action<CheckBox, ScrollBarAnnotationKinds> apply = (checkBox, kind) =>
            {
                if (checkBox.IsChecked.Value)
                    enabledKinds |= kind;
                else
                    enabledKinds &= ~kind;
            };

            apply(BookmarksTypeCheckBox, ScrollBarAnnotationKinds.Bookmark);
            apply(ChangedLinesTypeCheckBox, ScrollBarAnnotationKinds.Change);
            apply(CursorPositionTypeCheckBox, ScrollBarAnnotationKinds.CursorPosition);
            apply(CustomTypeCheckBox, ScrollBarAnnotationKinds.Custom);
            apply(SearchResultsTypeCheckBox, ScrollBarAnnotationKinds.SearchResult);
            apply(SyntaxErrorsTypeCheckBox, ScrollBarAnnotationKinds.Error);

            annotations.EnabledAnnotationKinds = enabledKinds;
        }

        private void CustomAnnotationsCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.Scrolling.Annotations.Invalidate();
        }

        private void CustomErrorsAppearanceDemoCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Editor.Scrolling.Annotations.SetAnnotationTypeAppearance(
                ScrollBarAnnotationType.SyntaxError,
                CustomErrorsAppearanceDemoCheckBox.IsChecked.Value ? customErrorAppearance : defaultErrorAppearance);
        }

        private void EnableOrDisableAllAnnotations()
        {
            var enabled = ScrollBarAnnotationsEnabledCheckBox.IsChecked.Value;

            BookmarksTypeCheckBox.IsEnabled =
                ChangedLinesTypeCheckBox.IsEnabled =
                CursorPositionTypeCheckBox.IsEnabled =
                CustomTypeCheckBox.IsEnabled =
                SearchResultsTypeCheckBox.IsEnabled =
                SyntaxErrorsTypeCheckBox.IsEnabled =
                enabled;

            var scrolling = Editor.Scrolling;

            if (enabled)
                scrolling.Options |= ScrollingOptions.VerticalScrollBarAnnotations;
            else
                scrolling.Options &= ~ScrollingOptions.VerticalScrollBarAnnotations;
        }

        private IEnumerable<ScrollBarAnnotationPaintData> GetCustomAnnotations()
        {
            for (int line = 1; line <= Editor.Source.Lines.Count; line++)
            {
                if (line % 10 == 0)
                {
                    yield return new ScrollBarAnnotationPaintData(
                        ScrollBarAnnotationType.Custom,
                        line,
                        ScrollBarAnnotationHorizontalAlignment.Center,
                        ScrollBarAnnotationVerticalAlignment.Center,
                        Colors.Magenta,
                        5);
                }
            }
        }

        private void InitializeCustomAnnotationsDemo()
        {
            Editor.Scrolling.Annotations.CustomAnnotationsRequested += Annotations_CustomAnnotationsRequested;
        }

        private void InitializeErrorAppearanceDemo()
        {
            defaultErrorAppearance = Editor.Scrolling.Annotations.GetAnnotationTypeAppearance(ScrollBarAnnotationType.SyntaxError);
            customErrorAppearance = new ScrollBarAnnotationTypeAppearance(
                ScrollBarAnnotationHorizontalAlignment.Center,
                ScrollBarAnnotationVerticalAlignment.Center,
                Colors.Cyan,
                10);
        }

        private void SaveTextChangesButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Source.Modified = false;
        }

        private void ScrollBarAnnotationsEnabledCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;

            EnableOrDisableAllAnnotations();
        }

        private void UseSystemScrollBarStyleCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (UseSystemScrollBarStyleCheckBox.IsChecked.Value)
                Editor.Resources.Add(typeof(ScrollBar), systemScrollBarStyle);
            else
                Editor.Resources.Remove(typeof(ScrollBar));
        }
    }
}
