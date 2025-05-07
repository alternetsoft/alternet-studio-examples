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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPicker
{
    public class ColorComboBox : Control
    {
        #region Dependency Properties

        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
                "SelectedColorChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color>),
                typeof(ColorComboBox));

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ColorComboBox), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsDropDownOpenChanged)));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorComboBox), new UIPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));

        #endregion

        #region Private Members

        private UIElement mpopup;
        private ColorPicker mcolorPicker;
        private bool mwithinChange;
        private ToggleButton mtoggleButton;

        #endregion

        #region Public Methods

        static ColorComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorComboBox), new FrameworkPropertyMetadata(typeof(ColorComboBox)));

            EventManager.RegisterClassHandler(typeof(ColorComboBox), ColorPicker.SelectedColorChangedEvent, new RoutedPropertyChangedEventHandler<Color>(OnColorPickerSelectedColorChanged));
        }

        #endregion

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        #region Handling Events

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mpopup = GetTemplateChild("PART_Popup") as UIElement;
            mcolorPicker = GetTemplateChild("PART_ColorPicker") as ColorPicker;
            mtoggleButton = GetTemplateChild("PART_ToggleButton") as ToggleButton;

            if (mcolorPicker != null)
            {
                mcolorPicker.SelectedColor = SelectedColor;

                mcolorPicker.SelectedColorChanged += new RoutedPropertyChangedEventHandler<Color>(ColorPicker_SelectedColorChanged);
            }
        }

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorComboBox colorComboBox = d as ColorComboBox;
            bool newValue = (bool)e.NewValue;

            // Mask HistTest visibility of toggle button otherwise when pressing it
            // and popup is open the popup is closed (since StaysOpen is false)
            // and reopens immediately
            if (colorComboBox.mtoggleButton != null)
            {
                colorComboBox.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(
                      delegate()
                      {
                          colorComboBox.mtoggleButton.IsHitTestVisible = !newValue;
                      }));
            }
        }

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorComboBox colorComboBox = d as ColorComboBox;

            if (colorComboBox.mwithinChange)
                return;

            colorComboBox.mwithinChange = true;
            if (colorComboBox.mcolorPicker != null)
                colorComboBox.mcolorPicker.SelectedColor = colorComboBox.SelectedColor;
            colorComboBox.mwithinChange = false;
        }

        private static void OnColorPickerSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            ColorComboBox colorComboBox = sender as ColorComboBox;

            if (colorComboBox.mwithinChange)
                return;

            colorComboBox.mwithinChange = true;
            if (colorComboBox.mcolorPicker != null)
                colorComboBox.SelectedColor = colorComboBox.mcolorPicker.SelectedColor;
            colorComboBox.mwithinChange = false;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (!mcolorPicker.TemplateApplied)
                return;

            RaiseEvent(new RoutedPropertyChangedEventArgs<Color>(e.OldValue, e.NewValue, SelectedColorChangedEvent));
        }

        #endregion
    }
}
