using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver.Painter;
using LSPainter.LSSolver;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolution : CanvasSolution
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

        public override IChange<ISolution> GenerateNeighbor()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            ShapePainterChange change = new ShapePainterChange(shape, color);
            return (IChange<ISolution>)change;
        }

        public override void Update()
        {
            // TODO: update generator parameters
        }
    }
}