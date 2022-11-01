#region Copyright (c) 2016-2022 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2022 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2022 Alternet Software

using System;
using System.Windows.Controls;
using System.Windows.Media;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.Wpf
{
    [ExtensionFor(typeof(Button))]
    public sealed class ButtonInstanceFactory : CustomInstanceFactory
    {
        private Brush customBrush = new SolidColorBrush(Color.FromRgb(155, 120, 130));

        public override object CreateInstance(Type type, params object[] arguments)
        {
            object instance = base.CreateInstance(type, arguments);
            var button = instance as Button;
            if (button != null)
            {
                button.BorderBrush = customBrush;
                button.BorderThickness = new System.Windows.Thickness(2, 2, 2, 2);
            }

            return instance;
        }
    }
}
