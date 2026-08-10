#region Copyright (c) 2016-2026 Alternet Software
/*
    AlterNET Studio

    Copyright (c) 2016-2026 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2026 Alternet Software

using System;
using System.Drawing;
using System.Windows.Forms;

using Alternet.Common;

namespace AlternetStudio.Demo
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            laAdress.ForeColor = Consts.GetUrlNormalColor(MainForm.IsDark);
            laMailTo.ForeColor = Consts.GetUrlNormalColor(MainForm.IsDark);

            var asm = this.GetType().Assembly;
            var prefix = "AlternetStudio.Demo.Resources";
            var imagePath = $"{prefix}.pictureBox1.Image.png";

            if (MainForm.IsDark)
                imagePath = $"{prefix}.pictureBox1.Image.Dark.png";

            var image = ControlUtilities.LoadImageFromAssembly(asm, imagePath);

            pictureBox1.Image = DisplayImageScaling.CloneAndAutoScaleImage(image);
        }

        private void AddressLabel_Click(object sender, EventArgs e)
        {
            laAdress.ForeColor = Consts.GetUrlVisitedColor(MainForm.IsDark);
            try
            {
                ShellUtilities.OpenUrl(laAdress.Text);
            }
            catch
            {
            }
        }

        private void MailToLabel_Click(object sender, EventArgs e)
        {
            laMailTo.ForeColor = Consts.GetUrlVisitedColor(MainForm.IsDark);
            try
            {
                ShellUtilities.OpenMailTo("contact@alternetsoft.com");
            }
            catch
            {
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void CompanyInfo_Load(object sender, EventArgs e)
        {
        }
    }
}
