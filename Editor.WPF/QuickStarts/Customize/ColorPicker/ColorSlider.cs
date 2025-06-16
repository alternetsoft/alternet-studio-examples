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

namespace ColorPicker
{
    public class ColorSlider : Slider
    {
        #region Dependency Properties

        public static readonly DependencyProperty LeftColorProperty =
            DependencyProperty.Register("LeftColor", typeof(Color), typeof(ColorSlider), new UIPropertyMetadata(Colors.Black));

        public static readonly DependencyProperty RightColorProperty =
            DependencyProperty.Register("RightColor", typeof(Color), typeof(ColorSlider), new UIPropertyMetadata(Colors.White));

        // TESTING
        //
        // public System.String Text
        // {
        //    get { return (System.String)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        // }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        // public static readonly DependencyProperty TextProperty =
        // DependencyProperty.Register("Text", typeof(System.String), typeof(ColorSlider), new UIPropertyMetadata(""));
        #endregion

        #region Public Methods

        static ColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
        }

        #endregion

        public Color LeftColor
        {
            get { return (Color)GetValue(LeftColorProperty); }
            set { SetValue(LeftColorProperty, value); }
        }

        public Color RightColor
        {
            get { return (Color)GetValue(RightColorProperty); }
            set { SetValue(RightColorProperty, value); }
        }
    }
}
