using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using LSPainter.Shapes;

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
            image = new Image<Rgba32>(Width, Height, Color.Black);

            data = new byte[4 * Width * Height];

            image.CopyPixelDataTo(data);
        }

        public void DrawShape(Shape shape, Color color)
        {
            BoundingBox bbox = shape.BoundingBox;

            int minX = Math.Max(0, bbox.X);
            int maxX = Math.Min(Width, bbox.X + bbox.Width);
            int minY = Math.Max(0, bbox.Y);
            int maxY = Math.Min(Height, bbox.Y + bbox.Height);

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    Vector p = new Vector(x + 0.5f, y + 0.5f);

                    if (shape.IsInside(p))
                    {
                        Color currentColor = GetPixel(x, y);
                        Color blendedColor = Color.Blend(currentColor, color);
                        setPixel(x, y, blendedColor);
                    }
                }
            }
        }

        void setPixel(int x, int y, Color color)
        {
            int index = 4 * (y * Width + x);

            data[index + 0] = color.R;
            data[index + 1] = color.G;
            data[index + 2] = color.B;
            data[index + 3] = color.A;
        }

        public override void Update()
        {
            // Upload Data
            base.Update();
        }
    }
}