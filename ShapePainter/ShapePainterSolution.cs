using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : CanvasSolution
    {
        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        public ShapePainterSolution(int width, int height, CanvasComparer comparer) : base(width, height, comparer)
        {
            shapeGeneratorSettings = new ShapeGeneratorSettings()
            {
                MinX = 0,
                MaxX = width,
                MinY = 0,
                MaxY = height,
                Area = 20,
            };

            colorGeneratorSettings = new ColorGeneratorSettings()
            {
                Alpha = 255 / 10,
            };
        }

        protected override void ApplyChange(CanvasChange change) => ApplyChange((ShapePainterChange)change);

        protected void ApplyChange(ShapePainterChange change)
        {
            Shape shape = change.Shape;
            Color color = change.Color;

            foreach ((int x, int y) in shape.EnumeratePixels(Canvas.Width, Canvas.Height))
            {
                Color currentColor = Canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);
                Canvas.SetPixel(x, y, newColor);
            }
        }

        protected override CanvasChange GenerateCanvasChange()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            return new ShapePainterChange(shape, color);
        }

        protected override long TryChange(CanvasChange change) => TryChange((ShapePainterChange)change);

        protected long TryChange(ShapePainterChange change)
        {
            long scoreDiff = 0;

            Shape shape = change.Shape;
            Color color = change.Color;

            foreach ((int x, int y) in shape.EnumeratePixels(Canvas.Width, Canvas.Height))
            {
                Color currentColor = Canvas.GetPixel(x, y);
                Color newColor = Color.Blend(currentColor, color);

                long pixelScoreDiff = comparer.PixelScoreDiff(x, y, currentColor, newColor);
                scoreDiff += pixelScoreDiff;
            }

            return scoreDiff;
        }
    }
}