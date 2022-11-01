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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Alternet.ColorBox
{
    internal class HueToColorConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = (double)value;

            return ColorUtils.ConvertHsvToRgb(doubleValue, 1, 1);
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class HsvControl : Control
    {
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(double), typeof(HsvControl), new UIPropertyMetadata((double)0, new PropertyChangedCallback(OnHueChanged)));

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(HsvControl), new UIPropertyMetadata((double)0, new PropertyChangedCallback(OnSaturationChanged)));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(HsvControl), new UIPropertyMetadata((double)0, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(HsvControl), new UIPropertyMetadata(Colors.Transparent));

        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Color>),
            typeof(HsvControl));

        #region Private Members

        private const string ThumbName = "PART_Thumb";

        private TranslateTransform mthumbTransform = new TranslateTransform();
        private Thumb mthumb;
        private bool mwithinUpdate = false;

        #endregion

        #region Public Methods

        static HsvControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HsvControl), new FrameworkPropertyMetadata(typeof(HsvControl)));

            // Register Event Handler for the Thumb 
            EventManager.RegisterClassHandler(typeof(HsvControl), Thumb.DragDeltaEvent, new DragDeltaEventHandler(HsvControl.OnThumbDragDelta));
        }

        #endregion

        #region Routed Events

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        #endregion

        #region Dependency Properties

        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        #endregion

        #region Overridden Members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mthumb = GetTemplateChild(ThumbName) as Thumb;
            if (mthumb != null)
            {
                UpdateThumbPosition();
                mthumb.RenderTransform = mthumbTransform;
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (mthumb != null)
            {
                Point position = e.GetPosition(this);

                UpdatePositionAndSaturationAndValue(position.X, position.Y);

                // Initiate mouse event on thumb so it will start drag
                mthumb.RaiseEvent(e);
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateThumbPosition();

            base.OnRenderSizeChanged(sizeInfo);
        }

        private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            HsvControl hsvControl = sender as HsvControl;
            hsvControl.OnThumbDragDelta(e);
        }

        private static void OnHueChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            HsvControl hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.mwithinUpdate)
                hsvControl.UpdateSelectedColor();
        }

        private static void OnSaturationChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            HsvControl hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.mwithinUpdate)
                hsvControl.UpdateThumbPosition();
        }

        private static void OnValueChanged(
            DependencyObject relatedObject, DependencyPropertyChangedEventArgs e)
        {
            HsvControl hsvControl = relatedObject as HsvControl;
            if (hsvControl != null && !hsvControl.mwithinUpdate)
                hsvControl.UpdateThumbPosition();
        }

        private void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            double offsetX = mthumbTransform.X + e.HorizontalChange;
            double offsetY = mthumbTransform.Y + e.VerticalChange;

            UpdatePositionAndSaturationAndValue(offsetX, offsetY);
        }

        #endregion

        #region Private Methods

        // Limit value to range (0 , max] 
        private double LimitValue(double value, double max)
        {
            if (value < 0)
                value = 0;
            if (value > max)
                value = max;
            return value;
        }

        private void UpdateSelectedColor()
        {
            Color oldColor = SelectedColor;
            Color newColor = ColorUtils.ConvertHsvToRgb(Hue, Saturation, Value);

            SelectedColor = newColor;
            ColorUtils.FireSelectedColorChangedEvent(this, SelectedColorChangedEvent, oldColor, newColor);
        }

        private void UpdatePositionAndSaturationAndValue(double positionX, double positionY)
        {
            positionX = LimitValue(positionX, ActualWidth);
            positionY = LimitValue(positionY, ActualHeight);

            mthumbTransform.X = positionX;
            mthumbTransform.Y = positionY;

            Saturation = positionX / ActualWidth;
            Value      = 1 - (positionY / ActualHeight);

            UpdateSelectedColor();
        }

        private void UpdateThumbPosition()
        {
            mthumbTransform.X = Saturation * ActualWidth;
            mthumbTransform.Y = (1 - Value) * ActualHeight;

            SelectedColor = ColorUtils.ConvertHsvToRgb(Hue, Saturation, Value);
        }

        #endregion
    }
}
