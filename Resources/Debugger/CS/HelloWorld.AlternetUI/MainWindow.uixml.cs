#region Copyright (c) 2016-2024 Alternet Software
/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2024 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2024 Alternet Software

using System;
using Alternet.UI;

namespace HelloWorld.AlternetUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //string url = AssemblyUtils.GetImageUrlInAssembly(assembly, relativePath);
            //var image = Image.FromUrlOrNull(url);
            //if (image is null || !image.IsOk)
            //    return;

            //PictureBox.Image = image;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}