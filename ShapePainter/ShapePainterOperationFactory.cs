using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterOperationFactory : OperationFactory<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>
    {
        public ShapeGeneratorSettings ShapeGeneratorSettings { get; }
        public ColorGeneratorSettings ColorGeneratorSettings { get; }
        public double Alpha = 1;

        public ShapePainterOperationFactory(int canvasWidth, int canvasHeight, ShapePainterChecker checker) : base(checker)
        {
            ShapeGeneratorSettings = new ShapeGeneratorSettings(0, canvasWidth, 0, canvasHeight, 50);
            ColorGeneratorSettings = new ColorGeneratorSettings(255 / 10);
        }

        public override Operation<ShapePainterSolution, ShapePainterScore, ShapePainterChecker> Generate()
        {
            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);

            return new PaintShapeOperation(shape, color);
        }

        public override void Update()
        {
            ShapeGeneratorSettings.Area *= Alpha;

            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}