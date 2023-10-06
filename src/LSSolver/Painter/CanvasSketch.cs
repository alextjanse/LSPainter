using System.Security.Cryptography.X509Certificates;
using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public class CanvasSketch
    {
        public Rectangle BoundingBox { get; }
        public (int x, int y) OriginOffsets { get; }
        public int Width { get; }
        public int Height { get; }
        public Color[,] Colors;

        public CanvasSketch(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;

            OriginOffsets = BoundingBox.OriginOffsets;
            Width = BoundingBox.SectionWidth;
            Height = BoundingBox.SectionHeight;

            Colors = new Color[Width, Height];
        }

        public void SetColor(int x, int y, Color color)
        {
            Colors[x - OriginOffsets.x, y - OriginOffsets.y] = color;
        }

        public void DrawShape(Shape shape, Color color)
        {
            Rectangle intersection = Rectangle.Intersect(shape.BoundingBox, BoundingBox);

            foreach ((int xCanvas, int yCanvas) in intersection.PixelCoords())
            {
                if (shape.IsInside(Vector.PixelPoint(xCanvas, yCanvas)))
                {
                    int xPortion = xCanvas - OriginOffsets.x;
                    int yPortion = yCanvas - OriginOffsets.y;

                    Colors[xPortion, yPortion] = Color.Blend(Colors[xPortion, yPortion], color);
                }
            }
        }

        public void ApplyToCanvas(Canvas canvas)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    canvas.SetPixel(OriginOffsets.x + x, OriginOffsets.y + y, Colors[x, y]);
                }
            }
        }
    }
}