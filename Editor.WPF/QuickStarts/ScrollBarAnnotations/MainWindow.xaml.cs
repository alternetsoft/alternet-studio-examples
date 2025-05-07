#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alternet.Editor.Wpf;
using Alternet.Syntax.Parsers.Roslyn;

namespace ScrollBarAnnotations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScrollBarAnnotationTypeAppearance customErrorAppearance;
        private ScrollBarAnnotationTypeAppearance defaultErrorAppearance;
        private TextSource textSource = new TextSource();
        private Style systemScrollBarStyle;

        public MainWindow()
        {
            InitializeComponent();

            textEditor.Source = textSource;
            textEditor.Lexer = new CsParser();

            InitializeCustomAnnotationsDemo();
            InitializeErrorAppearanceDemo();
            InitializeSystemScrollBarStyle();

            using (var testCodeStream = GetType().Assembly.GetManifestResourceStream("ScrollBarAnnotations.TestCode.cs"))
                textSource.LoadStream(testCodeStream);

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

            var annotations = textEditor.Scrolling.Annotations;
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
            textEditor.Scrolling.Annotations.Invalidate();
        }

        private void CustomErrorsAppearanceDemoCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            textEditor.Scrolling.Annotations.SetAnnotationTypeAppearance(
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

            var scrolling = textEditor.Scrolling;

            if (enabled)
                scrolling.Options |= ScrollingOptions.VerticalScrollBarAnnotations;
            else
                scrolling.Options &= ~ScrollingOptions.VerticalScrollBarAnnotations;
        }

        private IEnumerable<ScrollBarAnnotationPaintData> GetCustomAnnotations()
        {
            for (int line = 1; line <= textSource.Lines.Count; line++)
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
            textEditor.Scrolling.Annotations.CustomAnnotationsRequested += Annotations_CustomAnnotationsRequested;
        }

        private void InitializeErrorAppearanceDemo()
        {
            defaultErrorAppearance = textEditor.Scrolling.Annotations.GetAnnotationTypeAppearance(ScrollBarAnnotationType.SyntaxError);
            customErrorAppearance = new ScrollBarAnnotationTypeAppearance(
                ScrollBarAnnotationHorizontalAlignment.Center,
                ScrollBarAnnotationVerticalAlignment.Center,
                Colors.Cyan,
                10);
        }

        private void SaveTextChangesButton_Click(object sender, RoutedEventArgs e)
        {
            textSource.Modified = false;
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
                textEditor.Resources.Add(typeof(ScrollBar), systemScrollBarStyle);
            else
                textEditor.Resources.Remove(typeof(ScrollBar));
        }
    }
}
