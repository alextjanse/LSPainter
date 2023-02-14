using System;
using System.IO;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LSPainter
{
    public class ImageHandler
    {
        private string path;
        // TODO: make max file size 4MB (https://docs.sixlabors.com/articles/imagesharp/pixelformats.html)
        private Image<Rgba32> image;

        public int Handle { get; private set; }

        public byte[] Data { get; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public (int, int) Size
        {
            get
            {
                return (Width, Height);
            }
        }
        public string Title { get; private set; }

        public ImageHandler(string title, string path)
        {
            Title = title;

            this.path = path;

            image = Image.Load<Rgba32>(path);

            Width = image.Width;
            Height = image.Height;

            Data = new byte[4 * Width * Height];

            image.CopyPixelDataTo(Data);
        }

        public void Use()
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}