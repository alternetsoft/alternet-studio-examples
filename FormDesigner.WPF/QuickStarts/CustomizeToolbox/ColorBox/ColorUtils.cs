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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Alternet.ColorBox
{
    public static class ColorNames
    {
        #region Private Members

        private static Dictionary<Color, string> mcolorNames;

        #endregion

        static ColorNames()
        {
            mcolorNames = new Dictionary<Color, string>();

            FillColorNames();
        }

        #region Public Methods

        public static string GetColorName(Color colorToSeek)
        {
            if (mcolorNames.ContainsKey(colorToSeek))
                return mcolorNames[colorToSeek];
            else
                return colorToSeek.ToString();
        }

        public static void FillColorNames()
        {
            Type colorsType = typeof(System.Windows.Media.Colors);
            PropertyInfo[] colorsProperties = colorsType.GetProperties();

            foreach (PropertyInfo colorProperty in colorsProperties)
            {
                string colorName = colorProperty.Name;

                Color color = (Color)colorProperty.GetValue(null, null);

                // Path - Aqua is the same as Magenta - so we add 1 to red to avoid collision
                if (colorName == "Aqua")
                    color.R++;

                if (colorName == "Fuchsia")
                    color.G++;

                mcolorNames.Add(color, colorName);
            }
        }

        #endregion
    }

    public /*internal*/ static class ColorUtils
    {
        public static string[] GetColorNames()
        {
            Type colorsType = typeof(System.Windows.Media.Colors);
            PropertyInfo[] colorsProperties = colorsType.GetProperties();

            ColorConverter convertor = new ColorConverter();

            List<string> colorNames = new List<string>();
            foreach (PropertyInfo colorProperty in colorsProperties)
            {
                string colorName = colorProperty.Name;
                colorNames.Add(colorName);

                Color color = (Color)ColorConverter.ConvertFromString(colorName);
            }

            return colorNames.ToArray();
        }

        public static void FireSelectedColorChangedEvent(UIElement issuer, RoutedEvent routedEvent, Color oldColor, Color newColor)
        {
            RoutedPropertyChangedEventArgs<Color> newEventArgs =
                new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
            newEventArgs.RoutedEvent = routedEvent;
            issuer.RaiseEvent(newEventArgs);
        }

        public static void ConvertRgbToHsv(
            Color color, out double hue, out double saturation, out double value)
        {
            double red   = color.R / 255.0;
            double green = color.G / 255.0;
            double blue  = color.B / 255.0;
            double min = Math.Min(red, Math.Min(green, blue));
            double max = Math.Max(red, Math.Max(green, blue));

            value = max;
            double delta = max - min;

            if (value == 0)
                saturation = 0;
            else
                saturation = delta / max;

            if (saturation == 0)
                hue = 0;
            else
            {
                if (red == max)
                    hue = (green - blue) / delta;
                else if (green == max)
                    hue = 2 + ((blue - red) / delta);
                else // blue == max
                    hue = 4 + ((red - green) / delta);
            }

            hue *= 60;
            if (hue < 0)
                hue += 360;
        }

        // Converts an HSV color to an RGB color.
        // Algorithm taken from Wikipedia
        public static Color ConvertHsvToRgb(double hue, double saturation, double value)
        {
            double chroma = value * saturation;

            if (hue == 360)
                hue = 0;

            double hueTag = hue / 60;
            double x = chroma * (1 - Math.Abs((hueTag % 2) - 1));
            double m = value - chroma;
            switch ((int)hueTag)
            {
                case 0:
                    return BuildColor(chroma, x, 0, m);
                case 1:
                    return BuildColor(x, chroma, 0, m);
                case 2:
                    return BuildColor(0, chroma, x, m);
                case 3:
                    return BuildColor(0, x, chroma, m);
                case 4:
                    return BuildColor(x, 0, chroma, m);
                default:
                    return BuildColor(chroma, 0, x, m);
            }
        }

        public static Color[] GetSpectrumColors(int colorCount)
        {
            Color[] spectrumColors = new Color[colorCount];
            for (int i = 0; i < colorCount; ++i)
            {
                double hue = (i * 360.0) / colorCount;
                spectrumColors[i] = ConvertHsvToRgb(hue, /*saturation*/1.0, /*value*/1.0);
            }

            return spectrumColors;
        }

        public static bool TestColorConversion()
        {
            for (int i = 0; i <= 0xFFFFFF; ++i)
            {
                byte red = (byte)(i & 0xFF);
                byte green = (byte)((i & 0xFF00) >> 8);
                byte blue = (byte)((i & 0xFF0000) >> 16);
                Color originalColor = Color.FromRgb(red, green, blue);

                double hue, saturation, value;
                ConvertRgbToHsv(originalColor, out hue, out saturation, out value);

                Color resultColor = ConvertHsvToRgb(hue, saturation, value);
                if (originalColor != resultColor)
                    return false;
            }

            return true;
        }
        
        private static Color BuildColor(double red, double green, double blue, double m)
        {
            return Color.FromArgb(
                255,
                (byte)(((red + m) * 255) + 0.5),
                (byte)(((green + m) * 255) + 0.5),
                (byte)(((blue + m) * 255) + 0.5));
        }
    }

    [ValueConversion(typeof(Color), typeof(String))]
    public class ColorToStringConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color colorValue = (Color)value;
            return ColorNames.GetColorName(colorValue);
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
