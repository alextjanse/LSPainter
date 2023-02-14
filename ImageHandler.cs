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

            image.Mutate(x => x.Flip(FlipMode.Vertical));

            Data = new byte[4 * Width * Height];

            image.CopyPixelDataTo(Data);
        }

        public void Load()
        {
            Handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}