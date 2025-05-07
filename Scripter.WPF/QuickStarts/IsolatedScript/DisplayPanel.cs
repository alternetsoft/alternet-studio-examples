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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace IsolatedScript
{
    public delegate void OnRendering(object sender, DrawingContext dc);

    public class DisplayPanel : Canvas
    {
        public DisplayPanel()
        {
        }

        public event OnRendering OnRendering;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (OnRendering != null)
                OnRendering(this, dc);
        }
    }
}
