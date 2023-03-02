using System;
using System.IO;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LSPainter
{
    public class ImageHandler : Texture // TODO: fix multiple frame problem to make this possible again
    {
        private string path;
        // TODO: make max file size 4MB (https://docs.sixlabors.com/articles/imagesharp/pixelformats.html)
        private Image<Rgba32> image;
        public string Title { get; private set; }
        private byte[] data;
        public override byte[] Data => data;

        public ImageHandler(string title, string path)
        {
            Title = title;

            this.path = path;

            image = Image.Load<Rgba32>(path);

            Width = image.Width;
            Height = image.Height;

            data = new byte[4 * Width * Height];

            image.CopyPixelDataTo(Data);
        }
    }
}