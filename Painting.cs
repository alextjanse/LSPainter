using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace LSPainter
{
    public class Painting : Texture
    {
        Image<Rgba32> image;
        private byte[] data;
        public override byte[] Data => data;

        public Painting(int width, int height)
        {
            Width = width;
            Height = height;
            image = new Image<Rgba32>(Width, Height, Color.Red);

            data = new byte[4 * width * height];

            image.CopyPixelDataTo(data);
        }

        public void Update()
        {
            image.CopyPixelDataTo(data);
        }
    }
}