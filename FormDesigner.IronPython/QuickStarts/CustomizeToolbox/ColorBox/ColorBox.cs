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
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;


namespace Alternet.ColorBox
{
    /// <summary>
    /// Represents a combo-box like control used to select color from drop-down list of colors.
    /// </summary>
	[ToolboxBitmap(typeof(ColorBox), "Images.ColorBox.png")]
    public class ColorBox : ComboBox, IDisposable
    {
        #region Fields
        
        private const int ColorWidth = 28;

        private System.ComponentModel.Container components = null;

        private Brush fontBrush;

        #endregion

        #region ColorBox Members

        /// <summary>
        /// Initializes a new instance of the <c>ColorBox</c> class with specified container.
        /// </summary>
        /// <param name="container">Specifies IContainer that contains this new instance.</param>
        public ColorBox(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InternalCreate();
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <c>ColorBox</c> class with default settings.
        /// </summary>
        public ColorBox()
        {
            InternalCreate();
            InitializeComponent();
        }

        /// <summary>
        /// Gets an object representing the collection of the colors contained in this <c>ColorBox</c>.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        /// <summary>
        /// Represents text associated with this control.
        /// </summary>
        [Description("Represents text associated with this control.")]
        public new string Text
        {
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        /// Gets or sets currently selected color in the <c>ColorBox</c>.
        /// </summary>
        [Category("Appearence")]
        [Description("Gets or sets currently selected color in the \"ColorBox\".")]
        public Color SelectedColor
        {
            get
            {
                return (SelectedItem != null) ? GetColor(SelectedIndex) : Color.Empty;
            }

            set
            {
                if (value == Color.Empty)
                    SelectedIndex = 0;
                else
                {
                    if (value.IsNamedColor)
                    {
                        for (int i = 0; i < Items.Count; i++)
                            if (string.Compare(value.Name, Items[i].ToString(), true) == 0)
                            {
                                SelectedIndex = i;
                                break;
                            }
                    }
                    else
                    {
                        Items[Items.Count - 1] = value;
                        SelectedIndex = Items.Count - 1;
                    }
                }
            }
        }

        #endregion

        #region Protected Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (fontBrush != null)
                {
                    fontBrush.Dispose();
                    fontBrush = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            fontBrush = new SolidBrush(ForeColor);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.Index < 0) || (e.Index >= Items.Count))
                return;

            e.DrawBackground();
            bool disabled = (e.State & DrawItemState.Disabled) != 0;
            bool focused = ((e.State & DrawItemState.Focus) != 0) && ((e.State & DrawItemState.NoFocusRect) == 0);
            bool selected = (e.State & DrawItemState.Selected) != 0;

            if (focused)
                e.DrawFocusRectangle();

            Brush brush = new SolidBrush(GetColor(e.Index));

            try
            {
                string s = (e.Index == 0) ? "None" : ColorName(GetColor(e.Index));
                Rectangle colorR = new Rectangle(e.Bounds.Left, e.Bounds.Top, Math.Min(e.Bounds.Width, ColorWidth), e.Bounds.Height);
                colorR.Inflate(-2, -2);
                Rectangle textR = new Rectangle(colorR.Right + 1, e.Bounds.Top, e.Bounds.Width - colorR.Right - 1, e.Bounds.Height);

                if (e.State == DrawItemState.HotLight)
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                e.Graphics.FillRectangle(brush, colorR);
                e.Graphics.DrawRectangle(Pens.Black, colorR);
                Brush br = fontBrush;

                if (disabled)
                    br = Brushes.Gray;
                else
                    if (selected)
                        br = SystemBrushes.HighlightText;
                e.Graphics.DrawString(s, Font, br, textR);
            }
            finally
            {
                brush.Dispose();
            }
        }

        #endregion

        #region Private Membbers

        private void InternalCreate()
        {
            fontBrush = new SolidBrush(ForeColor);
            DropDownStyle = ComboBoxStyle.DropDown;
            DrawMode = DrawMode.OwnerDrawFixed;
            UpdateColors();
            SelectedIndex = 0;
        }

        private void UpdateColors()
        {
            Items.Add("None");
            Type type = typeof(Color);
            PropertyInfo[] info = type.GetProperties();

            for (int i = 0; i < info.Length; i++)
                if (info[i].PropertyType == typeof(Color))
                    Items.Add(info[i].Name);
            type = typeof(SystemColors);
            info = type.GetProperties();

            for (int i = 0; i < info.Length; i++)
                if (info[i].PropertyType == typeof(Color))
                    Items.Add(info[i].Name);
            Items.Add(Color.FromArgb(0, 0, 0));
        }

        private Color GetColor(int index)
        {
            if ((index > 0) && (index < Items.Count - 1))
                return Color.FromName(Items[index].ToString());
            else if (index == Items.Count - 1)
                return Items[index] is Color ? (Color)Items[index] : Color.Empty;
            else
                return Color.Empty;
        }

        private string ColorName(Color color)
        {
            return color.IsNamedColor ? color.Name : "Custom";
        }

        #endregion

        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}