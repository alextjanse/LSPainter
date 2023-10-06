using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterOperationFactory : OperationFactory<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>
    {
        public ShapeGeneratorSettings ShapeGeneratorSettings { get; }
        public ColorGeneratorSettings ColorGeneratorSettings { get; }
        public double Alpha = 1;

        public ShapePainterOperationFactory(int canvasWidth, int canvasHeight)
        {
            ShapeGeneratorSettings = new ShapeGeneratorSettings(0, canvasWidth, 0, canvasHeight, 200);
            ColorGeneratorSettings = new ColorGeneratorSettings(50);
        }

        public override Operation<ShapePainterSolution, ShapePainterScore, ShapePainterChecker> Generate(ShapePainterSolution solution)
        {
            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);

            return new PaintShapeOperation(shape, color);
        }

        public override void Update()
        {
            ShapeGeneratorSettings.MaxArea *= Alpha;

            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}