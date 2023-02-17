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

        static int counter = 0;

        static TextureUnit NextTextureUnit()
        {
            if (counter == 32)
            {
                throw new Exception("Too many textures!");
            }

            return (TextureUnit)((int)TextureUnit.Texture0 + counter++);
        }

        TextureUnit textureUnit;

        public Texture()
        {
            textureUnit = Texture.NextTextureUnit();
        }

        public void Load()
        {
            Handle = GL.GenTexture();

            GL.ActiveTexture(textureUnit);
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
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}