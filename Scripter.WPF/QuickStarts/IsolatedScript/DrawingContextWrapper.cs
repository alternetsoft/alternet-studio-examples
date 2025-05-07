#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows;
using System.Windows.Media;

namespace IsolatedScript
{
    public class DrawingContextWrapper : MarshalByRefObject
    {
        private readonly DrawingContext drawingContext;

        public DrawingContextWrapper(DrawingContext drawingContext)
        {
            this.drawingContext = drawingContext;
        }

        public void FillRectangle(string color, Rect rect)
        {
            drawingContext.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)), null, rect);
        }

        public void DrawEllipse(string color, Pen pen, Point center, double radiusX, double radiusY)
        {
            drawingContext.DrawEllipse(new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)), pen, center, radiusX, radiusY);
        }
    }
}