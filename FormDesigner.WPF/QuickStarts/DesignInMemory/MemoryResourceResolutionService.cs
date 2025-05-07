#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Alternet.FormDesigner.Wpf;

namespace FormDesigner.InMemory.Wpf
{
    public class MemoryResourceResolutionService : ResourceResolutionService
    {
        private static BitmapSource image;

        public MemoryResourceResolutionService(FormDesignerControl formDesigner)
            : base(formDesigner)
        {
        }

        public static BitmapSource GetOrCreateImage()
        {
            if (image == null)
                image = CreateBitmapSource(Colors.Blue);

            return image;
        }

        public override object TryResolveResource(Uri value)
        {
            if (value.ToString() == "1.png")
                return GetOrCreateImage();

            return null;
        }

        public override string[] GetPossibleImageSourceRelativePaths()
        {
            return new[] { "1.png" };
        }

        private static BitmapSource CreateBitmapSource(Color color)
        {
            int width = 128;
            int height = width;
            int stride = width / 8;
            var pixels = new byte[height * stride];
            var palette = new BitmapPalette(new[] { color });

            return BitmapSource.Create(
                width,
                height,
                96,
                96,
                PixelFormats.Indexed1,
                palette,
                pixels,
                stride);
        }
    }
}