using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterOperationFactory : OperationFactory<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>
    {
        public ShapeGeneratorSettings ShapeGeneratorSettings { get; }
        public ColorGeneratorSettings ColorGeneratorSettings { get; }
        public double Alpha = 1;

        public FiniteShapePainterOperationFactory(int canvasWidth, int canvasHeight)
        {
            ShapeGeneratorSettings = new ShapeGeneratorSettings(0, canvasWidth, 0, canvasHeight, 0.2 * canvasWidth * canvasHeight);
            ColorGeneratorSettings = new ColorGeneratorSettings(255 / 2);
        }

        public override Operation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker> Generate()
        {
            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);

            // TODO: make Generate take a solution as argument
            return new InsertNewShapeOperation(shape, color, 0);
        }

        public override void Update()
        {
            ShapeGeneratorSettings.Area *= Alpha;
            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}