using OpenTK.Graphics.OpenGL4;

namespace LSPainter
{
    public abstract class Texture
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
    }
}