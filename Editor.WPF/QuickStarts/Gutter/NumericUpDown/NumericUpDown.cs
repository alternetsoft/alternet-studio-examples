#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NumericUpDown
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Leads to XAML parser errors otherwise")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Implementing interface method in the separate region")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Implementing interface method in the separate region")]
    public class NumericUpDown : Control
    {
        private const decimal DefaultMinValue = 0;
        private const decimal DefaultValue = DefaultMinValue;
        private const decimal DefaultMaxValue = 100;
        private const decimal DefaultChange = 1;

        private const int DefaultDecimalPlaces = 0;

        #region Fields

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>),
            typeof(NumericUpDown));

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(
                "DecimalPlaces",
                typeof(int),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    DefaultDecimalPlaces,
                    new PropertyChangedCallback(OnDecimalPlacesChanged)),
                new ValidateValueCallback(ValidateDecimalPlaces));

        public static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register(
                "Change",
                typeof(decimal),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultChange, new PropertyChangedCallback(OnChangeChanged), new CoerceValueCallback(CoerceChange)),
                new ValidateValueCallback(ValidateChange));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(decimal),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    DefaultMaxValue,
                    new PropertyChangedCallback(OnMaximumChanged),
                    new CoerceValueCallback(CoerceMaximum)));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(decimal),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    DefaultMinValue,
                    new PropertyChangedCallback(OnMinimumChanged),
                    new CoerceValueCallback(CoerceMinimum)));

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(decimal),
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(
                    DefaultValue,
                    new PropertyChangedCallback(OnValueChanged),
                    new CoerceValueCallback(CoerceValue)));

        #endregion

        static NumericUpDown()
        {
            InitializeCommands();

            // Listen to MouseLeftButtonDown event to determine if NumericUpDown should move focus to itself
            EventManager.RegisterClassHandler(typeof(NumericUpDown), Mouse.MouseDownEvent, new MouseButtonEventHandler(NumericUpDown.OnMouseLeftButtonDown), true);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public NumericUpDown()
            : base()
        {
            UpdateValueString();
        }

        #region Properties

        #region Value
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            decimal oldValue = (decimal)args.OldValue;
            decimal newValue = (decimal)args.NewValue;

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                oldValue, newValue, ValueChangedEvent);

            control.OnValueChanged(e);

            control.UpdateValueString();
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            RaiseEvent(args);
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            decimal newValue = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue));
            newValue = decimal.Round(newValue, control.DecimalPlaces);

            return newValue;
        }
        #endregion

        #region Minimum
        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static void OnMinimumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(MaximumProperty);
            element.CoerceValue(ValueProperty);
        }

        private static object CoerceMinimum(DependencyObject element, object value)
        {
            decimal minimum = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;
            return decimal.Round(minimum, control.DecimalPlaces);
        }
        #endregion

        #region Maximum
        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(ValueProperty);
        }

        private static object CoerceMaximum(DependencyObject element, object value)
        {
            NumericUpDown control = (NumericUpDown)element;
            decimal newMaximum = (decimal)value;
            return decimal.Round(Math.Max(newMaximum, control.Minimum), control.DecimalPlaces);
        }
        #endregion

        #region Change
        public decimal Change
        {
            get { return (decimal)GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }

        private static bool ValidateChange(object value)
        {
            decimal change = (decimal)value;
            return change > 0;
        }

        private static void OnChangeChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
        }

        private static object CoerceChange(DependencyObject element, object value)
        {
            decimal newChange = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            decimal coercedNewChange = decimal.Round(newChange, control.DecimalPlaces);

            // If Change is .1 and DecimalPlaces is changed from 1 to 0, we want Change to go to 1, not 0.
            // Put another way, Change should always be rounded to DecimalPlaces, but never smaller than the
            // previous Change
            if (coercedNewChange < newChange)
            {
                coercedNewChange = SmallestForDecimalPlaces(control.DecimalPlaces);
            }

            return coercedNewChange;
        }

        private static decimal SmallestForDecimalPlaces(int decimalPlaces)
        {
            decimal d = 1;

            for (int i = 0; i < decimalPlaces; i++)
            {
                d /= 10;
            }

            return d;
        }

        #endregion

        #region DecimalPlaces
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)element;
            control.CoerceValue(ChangeProperty);
            control.CoerceValue(MinimumProperty);
            control.CoerceValue(MaximumProperty);
            control.CoerceValue(ValueProperty);
            control.UpdateValueString();
        }

        private static bool ValidateDecimalPlaces(object value)
        {
            int decimalPlaces = (int)value;
            return decimalPlaces >= 0;
        }

        #endregion

        #region ValueString
        public string ValueString
        {
            get
            {
                return (string)GetValue(ValueStringProperty);
            }
        }

        private static readonly DependencyPropertyKey ValueStringPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("ValueString", typeof(string), typeof(NumericUpDown), new PropertyMetadata());

        public static readonly DependencyProperty ValueStringProperty = ValueStringPropertyKey.DependencyProperty;

        private void UpdateValueString()
        {
            numberFormatInfo.NumberDecimalDigits = this.DecimalPlaces;
            string newValueString = this.Value.ToString("f", numberFormatInfo);
            this.SetValue(ValueStringPropertyKey, newValueString);
        }

        private NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        #endregion

        #region Commands

        public static RoutedCommand IncreaseCommand
        {
            get
            {
                return increaseCommand;
            }
        }

        public static RoutedCommand DecreaseCommand
        {
            get
            {
                return decreaseCommand;
            }
        }

        private static void InitializeCommands()
        {
            increaseCommand = new RoutedCommand("IncreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(increaseCommand, OnIncreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(increaseCommand, new KeyGesture(Key.Up)));

            decreaseCommand = new RoutedCommand("DecreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(decreaseCommand, OnDecreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(decreaseCommand, new KeyGesture(Key.Down)));
        }

        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnIncrease();
            }
        }

        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnDecrease();
            }
        }

        protected virtual void OnIncrease()
        {
            this.Value += Change;
        }

        protected virtual void OnDecrease()
        {
            this.Value -= Change;
        }

        private static RoutedCommand increaseCommand;
        private static RoutedCommand decreaseCommand;
        #endregion

        /// <summary>
        /// This is a class handler for MouseLeftButtonDown event.
        /// The purpose of this handle is to move input focus to NumericUpDown when user pressed
        /// mouse left button on any part of slider that is not focusable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if (!control.IsKeyboardFocusWithin)
            {
                e.Handled = control.Focus() || e.Handled;
            }
        }
    }
}
