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
        // TODO: make max file size 4MB (https://docs.sixlabors.com/articles/imagesharp/pixelformats.html)
        public string Title { get; private set; }

        public ImageHandler(string title, Image<Rgba32> image) : base(image.Width, image.Height)
        {
            Title = title;
            image.CopyPixelDataTo(Data);
        }

        public static ImageHandler FromFile(string title, string path)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            return new ImageHandler(title, image);
        }
    }
}