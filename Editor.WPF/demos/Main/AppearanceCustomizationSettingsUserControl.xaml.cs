#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alternet.Editor.Wpf.MainDemo_Wpf
{
    public partial class AppearanceCustomizationSettingsUserControl : UserControl, IDemoSettingsControl
    {
        private TextEditor editor;
        private Brush defaultSelectionBrush;
        private Brush defaultBackgroundBrush;
        private Brush defaultGutterBrush;
        private Brush defaultLineNumbersBrush;
        private Brush defaultLineNumbersBackBrush;
        private Brush defaultOutliningFillExpandedBrush;
        private Brush defaultOutliningFillCollapsedBrush;
        private Brush defaultOutliningLineBrush;
        private Brush defaultOutliningGlyphBrush;
        private Pen defaultOutlineSectionBoundsPen;

        public AppearanceCustomizationSettingsUserControl()
        {
            InitializeComponent();
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

        public UserControl Control
        {
            get
            {
                return this;
            }
        }

        private void CustomizeAppearanceCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (CustomizeAppearanceCheckBox.IsChecked.Value)
            {
                defaultBackgroundBrush = editor.Background;
                defaultSelectionBrush = editor.SelectionBrush;
                defaultGutterBrush = editor.GutterBrush;
                defaultLineNumbersBrush = editor.LineNumbersBrush;
                defaultLineNumbersBackBrush = editor.LineNumbersBackBrush;
                defaultOutliningFillExpandedBrush = editor.OutliningFillExpandedBrush;
                defaultOutliningFillCollapsedBrush = editor.OutliningFillCollapsedBrush;
                defaultOutliningLineBrush = editor.OutliningLineBrush;
                defaultOutliningGlyphBrush = editor.OutliningGlyphBrush;
                defaultOutlineSectionBoundsPen = editor.OutlineSectionBoundsPen;

                editor.ModifyTextDisplay += Editor_ModifyTextDisplay;
                editor.Background = new SolidColorBrush(Colors.Black);
                editor.SelectionBrush = new SolidColorBrush(Colors.DarkOrange);
                editor.GutterBrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                editor.LineNumbersBrush = new SolidColorBrush(Colors.Orange);
                editor.LineNumbersBackBrush = new SolidColorBrush(Colors.Black);
                editor.OutliningFillExpandedBrush = new SolidColorBrush(Colors.DarkGreen);
                editor.OutliningFillCollapsedBrush = new SolidColorBrush(Colors.LightGreen);
                editor.OutliningLineBrush = new SolidColorBrush(Colors.LightGreen);
                editor.OutliningGlyphBrush = new SolidColorBrush(Colors.White);
                editor.OutlineSectionBoundsPen = new Pen(new SolidColorBrush(Colors.LightGreen), 1);
            }
            else
            {
                editor.ModifyTextDisplay -= Editor_ModifyTextDisplay;
                editor.Background = defaultBackgroundBrush;
                editor.SelectionBrush = defaultSelectionBrush;
                editor.GutterBrush = defaultGutterBrush;
                editor.LineNumbersBrush = defaultLineNumbersBrush;
                editor.LineNumbersBackBrush = defaultLineNumbersBackBrush;
                editor.OutliningFillExpandedBrush = defaultOutliningFillExpandedBrush;
                editor.OutliningFillCollapsedBrush = defaultOutliningFillCollapsedBrush;
                editor.OutliningLineBrush = defaultOutliningLineBrush;
                editor.OutliningGlyphBrush = defaultOutliningGlyphBrush;
                editor.OutlineSectionBoundsPen = defaultOutlineSectionBoundsPen;
            }

            editor.Invalidate();
        }

        private Color Invert(Color originalColor)
        {
            Color invertedColor = new Color();
            invertedColor.ScR = 1.0F - originalColor.ScR;
            invertedColor.ScG = 1.0F - originalColor.ScG;
            invertedColor.ScB = 1.0F - originalColor.ScB;
            invertedColor.ScA = originalColor.ScA;
            return invertedColor;
        }

        private void Editor_ModifyTextDisplay(object sender, ModifyTextDisplayEventArgs e)
        {
            if (e.TextFragmentData.Foreground != null)
                e.TextFragmentData.Foreground = new SolidColorBrush(Invert(((SolidColorBrush)e.TextFragmentData.Foreground).Color));

            if (e.TextFragmentData.Background != null)
                e.TextFragmentData.Background = new SolidColorBrush(Invert(((SolidColorBrush)e.TextFragmentData.Background).Color));
        }

        private void UpdateDataFromEditor()
        {
        }
    }
}
