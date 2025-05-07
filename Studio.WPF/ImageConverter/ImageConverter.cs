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
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using Alternet.Common;
using Alternet.Editor.Wpf;

namespace AlternetStudio.Wpf.Demo
{
    public class ImageConverter : IValueConverter
    {
        private ImageSourceCollection images = new ImageSourceCollection();

        public ImageConverter()
        {
            images = LoadImages();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) =>
            images != null && images.Count >= (int)value && (int)value >= 0 ? images[(int)value] : null;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => null;

        protected virtual ImageSourceCollection LoadImages()
        {
            var images = new ImageSourceCollection();

            Func<string, AlphaImageList> load = suffix =>
                AlphaImageListHelper.LoadImageListFromStrip(
                    typeof(MainWindow).Assembly,
                    $"AlternetStudio.Wpf.Images.DotNetImagesAlpha{suffix}.png");

            var list = new DisplayScaledAlphaImages(() => load(string.Empty), () => load("HighDpi")).Images;

            if ((list != null) && (list.Images != null))
            {
                foreach (System.Drawing.Image image in list.Images)
                    AddImage(images, image);
            }

            return images;
        }

        private static void AddImage(ImageSourceCollection collection, System.Drawing.Image source)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            source.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            collection.Add(bi);
        }
    }
}
