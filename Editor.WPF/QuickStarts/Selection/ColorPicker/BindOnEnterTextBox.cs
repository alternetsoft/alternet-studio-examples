#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ColorPicker
{
    public class BindOnEnterTextBox : TextBox
    {
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
            {
                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(this, TextProperty);
                if (bindingExpression != null)
                    bindingExpression.UpdateSource();
            }
        }
    }
}
