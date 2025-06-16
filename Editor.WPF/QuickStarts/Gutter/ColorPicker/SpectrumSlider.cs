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
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPicker
{
    public class SpectrumSlider : Slider
    {
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(SpectrumSlider), new UIPropertyMetadata(0D, new PropertyChangedCallback(OnHuePropertyChanged)));

        #region Private Members

        private bool mwithinChanging = false;

        #endregion

        #region Public Methods

        static SpectrumSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpectrumSlider), new FrameworkPropertyMetadata(typeof(SpectrumSlider)));
        }

        public SpectrumSlider()
        {
            SetBackground();
        }

        #endregion

        #region Dependency Properties

        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        #endregion

        #region Protected Methods

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            if (!mwithinChanging && !BindingOperations.IsDataBound(this, HueProperty))
            {
                mwithinChanging = true;
                Hue = 360 - newValue;
                mwithinChanging = false;
            }
        }

        #endregion

        #region Private Methods

        private static void OnHuePropertyChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            SpectrumSlider spectrumSlider = relatedObject as SpectrumSlider;
            if (spectrumSlider != null && !spectrumSlider.mwithinChanging)
            {
                spectrumSlider.mwithinChanging = true;

                double hue = (double)e.NewValue;
                spectrumSlider.Value = 360 - hue;

                spectrumSlider.mwithinChanging = false;
            }
        }

        private void SetBackground()
        {
            LinearGradientBrush backgroundBrush = new LinearGradientBrush();
            backgroundBrush.StartPoint = new Point(0.5, 0);
            backgroundBrush.EndPoint = new Point(0.5, 1);

            const int SpectrumColorCount = 30;

            Color[] spectrumColors = ColorUtils.GetSpectrumColors(SpectrumColorCount);
            for (int i = 0; i < SpectrumColorCount; ++i)
            {
                double offset = i * 1.0 / SpectrumColorCount;
                GradientStop gradientStop = new GradientStop(spectrumColors[i], offset);
                backgroundBrush.GradientStops.Add(gradientStop);
            }

            Background = backgroundBrush;
        }

        #endregion
    }
}
