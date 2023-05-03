using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver.Canvas;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : CanvasSolution<ShapePainterChange>
    {
        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        public ShapePainterSolution(int width, int height) : base(width, height)
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

        public override void ApplyChange(CanvasChange change)
        {
            
        }

        public override CanvasChange GenerateNeighbor()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            return new ShapePainterChange(shape, color);
        }

        public override long TryChange(CanvasChange change)
        {
            long scoreDiff = 
        }
    }
}