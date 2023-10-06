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
        public byte[] Data { get; protected set; }
        public Rectangle BoundingBox { get; }

        public Texture(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new byte[4 * Width * Height];

            BoundingBox = new Rectangle(0, Width, 0, Height);
        }

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

        public IEnumerable<Color> EnumerateSection(Rectangle bbox)
        {
            return bbox.PixelCoords().Select(((int x, int y) c) => GetPixel(c.x, c.y));
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