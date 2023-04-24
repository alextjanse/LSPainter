using OpenTK.Graphics.OpenGL4;
using LSPainter.Maths;
using System.Collections;

namespace LSPainter
{
    public abstract class Texture : IEnumerable<Color>
    {
        public int Handle { get; private set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public (int, int) Size => (Width, Height);
        public abstract byte[] Data { get; }

        public void Load()
        {
            Handle = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, Handle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Unbind
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public virtual void Update()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, Data);
            
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public Color GetPixel(int x, int y)
        {
            int index = 4 * (y * Width + x);
            
            return new Color(
                Data[index + 0],    // R
                Data[index + 1],    // G
                Data[index + 2],    // B
                Data[index + 3]);   // A
        }

        protected void setPixel(int x, int y, Color color)
        {
            int index = 4 * (y * Width + x);

            Data[index + 0] = color.R;
            Data[index + 1] = color.G;
            Data[index + 2] = color.B;
            Data[index + 3] = color.A;
        }

        public IEnumerable<Color> EnumerateSection(BoundingBox bbox)
        {
            int x = bbox.X;
            int xMax = x + bbox.Width;
            int y = bbox.Y;
            int yMax = y + bbox.Height;

            for (; y < yMax; y++)
            {
                for (; x < xMax; x++)
                {
                    yield return GetPixel(x, y);
                }
            }
        }

        IEnumerator<Color> IEnumerable<Color>.GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return GetPixel(x, y);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}