#region Copyright (c) 2016-2017 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2017 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2017 Alternet Software

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Alternet.ColorBox
{
    [ValueConversion(typeof(double), typeof(String))]
    internal class DoubleToIntegerStringConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = (double)value;
            int intValue = (int)doubleValue;

            return intValue.ToString();
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            double doubleValue = 0;
            if (!double.TryParse(stringValue, out doubleValue))
                doubleValue = 0;

            return doubleValue;
        }
    }
    [System.Drawing.ToolboxBitmap(typeof(ColorPicker), "Alternet.ColorBox.Images.ColorPicker_12x_16x_24.bmp")]

    internal class ColorPicker : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new UIPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));

        // Using a DependencyProperty as the backing store for FixedSliderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FixedSliderColorProperty =
            DependencyProperty.Register("FixedSliderColor", typeof(bool), typeof(SpectrumSlider), new UIPropertyMetadata(false, new PropertyChangedCallback(OnFixedSliderColorPropertyChanged)));

        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Color>),
            typeof(ColorPicker));

        #endregion

        #region Private Members

        private const string RedColorSliderName = "PART_RedColorSlider";
        private const string GreenColorSliderName = "PART_GreenColorSlider";
        private const string BlueColorSliderName = "PART_BlueColorSlider";
        private const string AlphaColorSliderName = "PART_AlphaColorSlider";

        private const string SpectrumSliderName = "PART_SpectrumSlider1";
        private const string HsvControlName = "PART_HsvControl";

        private ColorSlider mredColorSlider;
        private ColorSlider mgreenColorSlider;
        private ColorSlider mblueColorSlider;
        private ColorSlider malphaColorSlider;

        private SpectrumSlider mspectrumSlider;

        private HsvControl mhsvControl;

        private bool mwithinChange;
        private bool mtemplateApplied;

        #endregion

        #region Public Methods

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));

            // Register Event Handler for slider
            EventManager.RegisterClassHandler(typeof(ColorPicker), Slider.ValueChangedEvent, new RoutedPropertyChangedEventHandler<double>(ColorPicker.OnSliderValueChanged));

            // Register Event Handler for Hsv Control
            EventManager.RegisterClassHandler(typeof(ColorPicker), HsvControl.SelectedColorChangedEvent, new RoutedPropertyChangedEventHandler<Color>(ColorPicker.OnHsvControlSelectedColorChanged));
        }

        #endregion

        #region Routed Events

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        #endregion

        public bool TemplateApplied
        {
            get
            {
                return mtemplateApplied;
            }
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public bool FixedSliderColor
        {
            get { return (bool)GetValue(FixedSliderColorProperty); }
            set { SetValue(FixedSliderColorProperty, value); }
        }

        #region Event Handling

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mredColorSlider = GetTemplateChild(RedColorSliderName) as ColorSlider;
            mgreenColorSlider = GetTemplateChild(GreenColorSliderName) as ColorSlider;
            mblueColorSlider = GetTemplateChild(BlueColorSliderName) as ColorSlider;
            malphaColorSlider = GetTemplateChild(AlphaColorSliderName) as ColorSlider;
            mspectrumSlider = GetTemplateChild(SpectrumSliderName) as SpectrumSlider;
            mhsvControl = GetTemplateChild(HsvControlName) as HsvControl;

            UpdateControlColors(SelectedColor);
            mtemplateApplied = true;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == UIElement.IsVisibleProperty && (bool)e.NewValue == true)
                Focus();
            base.OnPropertyChanged(e);
        }

        private static void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ColorPicker colorPicker = sender as ColorPicker;
            colorPicker.OnSliderValueChanged(e);
        }

        private static void OnHsvControlSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            ColorPicker colorPicker = sender as ColorPicker;
            colorPicker.OnHsvControlSelectedColorChanged(e);
        }

        private static void OnSelectedColorPropertyChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = relatedObject as ColorPicker;
            colorPicker.OnSelectedColorPropertyChanged(e);
        }

        private static void OnFixedSliderColorPropertyChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = relatedObject as ColorPicker;
            colorPicker.UpdateColorSlidersBackground();
        }

        private void OnSliderValueChanged(RoutedPropertyChangedEventArgs<double> e)
        {
            // Avoid endless loop
            if (mwithinChange)
                return;

            mwithinChange = true;
            if (e.OriginalSource == mredColorSlider || 
                e.OriginalSource == mgreenColorSlider ||
                e.OriginalSource == mblueColorSlider ||
                e.OriginalSource == malphaColorSlider)
            {
                Color newColor = GetRgbColor();
                UpdateHsvControlColor(newColor);
                UpdateSelectedColor(newColor);
            }
            else if (e.OriginalSource == mspectrumSlider)
            {
                double hue = mspectrumSlider.Hue;
                UpdateHsvControlHue(hue);
                Color newColor = GetHsvColor();
                UpdateRgbColors(newColor);
                UpdateSelectedColor(newColor);
            }

            mwithinChange = false;
        }

        private void OnHsvControlSelectedColorChanged(RoutedPropertyChangedEventArgs<Color> e)
        {
            // Avoid endless loop
            if (mwithinChange)
                return;

            mwithinChange = true;

            Color newColor = GetHsvColor();
            UpdateRgbColors(newColor);
            UpdateSelectedColor(newColor);

            mwithinChange = false;
        }

        private void OnSelectedColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!mtemplateApplied)
                return;

            // Avoid endless loop
            if (mwithinChange)
                return;

            mwithinChange = true;

            Color newColor = (Color)e.NewValue;
            UpdateControlColors(newColor);

            mwithinChange = false;
        }

        #endregion

        #region Private Methods

        private void SetColorSliderBackground(ColorSlider colorSlider, Color leftColor, Color rightColor)
        {
            colorSlider.LeftColor  = leftColor;
            colorSlider.RightColor = rightColor;
        }

        private void UpdateColorSlidersBackground()
        {
            if (FixedSliderColor)
            {
                SetColorSliderBackground(mredColorSlider, Colors.Red, Colors.Red);
                SetColorSliderBackground(mgreenColorSlider, Colors.Green, Colors.Green);
                SetColorSliderBackground(mblueColorSlider, Colors.Blue, Colors.Blue);
                SetColorSliderBackground(malphaColorSlider, Colors.Transparent, Colors.White);
            }
            else
            {
                byte red   = SelectedColor.R;
                byte green = SelectedColor.G;
                byte blue  = SelectedColor.B;
                SetColorSliderBackground(
                    mredColorSlider,
                    Color.FromRgb(0, green, blue), 
                    Color.FromRgb(255, green, blue));
                SetColorSliderBackground(
                    mgreenColorSlider,
                    Color.FromRgb(red, 0, blue), 
                    Color.FromRgb(red, 255, blue));
                SetColorSliderBackground(
                    mblueColorSlider,
                    Color.FromRgb(red, green, 0), 
                    Color.FromRgb(red, green, 255));
                SetColorSliderBackground(
                    malphaColorSlider,
                    Color.FromArgb(0, red, green, blue), 
                    Color.FromArgb(255, red, green, blue));
            }
        }

        private Color GetRgbColor()
        {
            return Color.FromArgb(
                (byte)malphaColorSlider.Value,
                (byte)mredColorSlider.Value,
                (byte)mgreenColorSlider.Value,
                (byte)mblueColorSlider.Value);
        }

        private void UpdateRgbColors(Color newColor)
        {
            malphaColorSlider.Value = newColor.A;
            mredColorSlider.Value   = newColor.R;
            mgreenColorSlider.Value = newColor.G;
            mblueColorSlider.Value  = newColor.B;
        }

        private Color GetHsvColor()
        {
            Color hsvColor = mhsvControl.SelectedColor;
            hsvColor.A = (byte)malphaColorSlider.Value;
            return hsvColor;
        }

        private void UpdateSpectrumColor(Color newColor)
        {
        }

        private void UpdateHsvControlHue(double hue)
        {
            mhsvControl.Hue = hue;
        }

        private void UpdateHsvControlColor(Color newColor)
        {
            double hue, saturation, value;

            ColorUtils.ConvertRgbToHsv(newColor, out hue, out saturation, out value);

            // if saturation == 0 or value == 1 hue don't count so we save the old hue
            if (saturation != 0 && value != 0)
                mhsvControl.Hue        = hue;

            mhsvControl.Saturation = saturation;
            mhsvControl.Value      = value;

            mspectrumSlider.Hue = mhsvControl.Hue;
        }

        private void UpdateSelectedColor(Color newColor)
        {
            Color oldColor = SelectedColor;
            SelectedColor = newColor;

            if (!FixedSliderColor)
                UpdateColorSlidersBackground();

            ColorUtils.FireSelectedColorChangedEvent(this, SelectedColorChangedEvent, oldColor, newColor);
        }

        private void UpdateControlColors(Color newColor)
        {
            UpdateRgbColors(newColor);
            UpdateSpectrumColor(newColor);
            UpdateHsvControlColor(newColor);
            UpdateColorSlidersBackground();
        }

        #endregion
    }
}
