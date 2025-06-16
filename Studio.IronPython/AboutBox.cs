#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Drawing;
using System.Windows.Forms;

using Alternet.Common;

namespace AlternetStudio.Demo
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            var asm = this.GetType().Assembly;
            var prefix = "AlternetStudio.Demo.IronPython.Resources";
            pictureBox1.Image = ControlUtilities.LoadImageFromAssembly(asm, $"{prefix}.pictureBox1.Image.png");
        }

        private void AdressLabel_Click(object sender, EventArgs e)
        {
            laAdress.ForeColor = Color.Purple;
            try
            {
                System.Diagnostics.Process.Start(laAdress.Text);
            }
            catch
            {
            }
        }

        private void MailToLabel_Click(object sender, EventArgs e)
        {
            laMailTo.ForeColor = Color.Purple;
            System.Diagnostics.Process.Start("mailto:contact@alternetsoft.com");
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void CompanyInfo_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = DisplayImageScaling.CloneAndAutoScaleImage(pictureBox1.Image);
            if (pictureBox1.Image is Bitmap)
                ((Bitmap)pictureBox1.Image).MakeTransparent(Color.White);
        }
    }
}
