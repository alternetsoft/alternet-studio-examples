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
using System.Drawing;
using System.Windows.Forms;
using Alternet.Common;
using Alternet.Editor;

namespace ScrollBarAnnotations
{
    public partial class MainForm : Form
    {
        private ScrollBarAnnotationTypeAppearance customErrorAppearance;
        private ScrollBarAnnotationTypeAppearance defaultErrorAppearance;

        public MainForm()
        {
            InitializeComponent();

            InitializeCustomAnnotationsDemo();
            InitializeErrorAppearanceDemo();

            InitializeScrollBarVisualStyleComboBox();

            using (var testCodeStream = GetType().Assembly.GetManifestResourceStream("ScrollBarAnnotations.TestCode.cs"))
                textSource.LoadStream(testCodeStream);

            EnableOrDisableAllAnnotations();
        }

        private void Annotations_CustomAnnotationsRequested(object sender, ScrollBarCustomAnnotationsEventArgs e)
        {
            if (customAnnotationsCheckBox.Checked)
                e.Annotations = GetCustomAnnotations();
        }

        private void AnnotationTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var annotations = syntaxEdit.Scrolling.Annotations;
            var enabledKinds = annotations.EnabledAnnotationKinds;

            Action<CheckBox, ScrollBarAnnotationKinds> apply = (checkBox, kind) =>
            {
                if (checkBox.Checked)
                    enabledKinds |= kind;
                else
                    enabledKinds &= ~kind;
            };

            apply(bookmarksTypeCheckBox, ScrollBarAnnotationKinds.Bookmark);
            apply(changedLinesTypeCheckBox, ScrollBarAnnotationKinds.Change);
            apply(cursorPositionTypeCheckBox, ScrollBarAnnotationKinds.CursorPosition);
            apply(customTypeCheckBox, ScrollBarAnnotationKinds.Custom);
            apply(searchResultsTypeCheckBox, ScrollBarAnnotationKinds.SearchResult);
            apply(syntaxErrorsTypeCheckBox, ScrollBarAnnotationKinds.Error);

            annotations.EnabledAnnotationKinds = enabledKinds;
        }

        private void ChangeErrorsAppearanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit.Scrolling.Annotations.SetAnnotationTypeAppearance(
                ScrollBarAnnotationType.SyntaxError,
                changeErrorsAppearanceCheckBox.Checked ? customErrorAppearance : defaultErrorAppearance);
        }

        private void CustomAnnotationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            syntaxEdit.Refresh();
        }

        private void EnableOrDisableAllAnnotations()
        {
            var enabled = scrollBarAnnotationsEnabledCheckBox.Checked;

            bookmarksTypeCheckBox.Enabled =
                changedLinesTypeCheckBox.Enabled =
                cursorPositionTypeCheckBox.Enabled =
                customTypeCheckBox.Enabled =
                searchResultsTypeCheckBox.Enabled =
                syntaxErrorsTypeCheckBox.Enabled =
                enabled;

            var scrolling = syntaxEdit.Scrolling;

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
                        Color.Magenta,
                        5);
                }
            }
        }

        private void InitializeCustomAnnotationsDemo()
        {
            syntaxEdit.Scrolling.Annotations.CustomAnnotationsRequested += Annotations_CustomAnnotationsRequested;
        }

        private void InitializeErrorAppearanceDemo()
        {
            defaultErrorAppearance = syntaxEdit.Scrolling.Annotations.GetAnnotationTypeAppearance(ScrollBarAnnotationType.SyntaxError);
            customErrorAppearance = new ScrollBarAnnotationTypeAppearance(
                ScrollBarAnnotationHorizontalAlignment.Center,
                ScrollBarAnnotationVerticalAlignment.Center,
                Color.Cyan,
                10);
        }

        private void InitializeScrollBarVisualStyleComboBox()
        {
            scrollBarsVisualStyleComboBox.DataSource = Enum.GetValues(typeof(ScrollBarVisualStyle));
            scrollBarsVisualStyleComboBox.SelectedItem = ScrollBarVisualStyle.VisualStudio;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            syntaxEdit.Modified = false;
        }

        private void ScrolarAnnotationsEnabledCheckBoxLisoxTextBox_CheckedChanged(object sender, EventArgs e)
        {
            EnableOrDisableAllAnnotations();
        }

        private void ScrolarsVisualStyleComboBoxLisoxTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            syntaxEdit.Scrolling.ScrollBarsVisualStyle = (ScrollBarVisualStyle)scrollBarsVisualStyleComboBox.SelectedItem;
        }
    }
}