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
            image = new Image<Rgba32>(Width, Height, Color.Blue);

            data = new byte[4 * width * height];

            image.CopyPixelDataTo(data);

            Point p1 = new Point(10, 10);
            Point p2 = new Point(Width / 2f, Height - 10);
            Point p3 = new Point(Width - 10, 10);

            Triangle triangle = new Triangle(p1, p2, p3);

            Color color = Color.Red;

            DrawShape(triangle, color);
        }

        public void DrawShape(Shape shape, Color color)
        {
            BoundingBox bbox = shape.BoundingBox;

            int x = bbox.X;
            int y = bbox.Y;
            int width = bbox.Width;
            int height = bbox.Height;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Vector p = new Vector(x + i + 0.5f, y + j + 0.5f);

                    if (shape.IsInside(p))
                    {
                        setPixel(x + i, y + j, color);
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