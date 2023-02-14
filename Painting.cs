using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace LSPainter
{
    public class Painting
    {
        public Image<Rgba32> Image { get; }

        public int Width { get; }
        public int Height { get; }

        public Painting(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new Image<Rgba32>(Width, Height, Color.Black);
        }
    }
}