using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;
using LSPainter.FiniteShapePainter.Operations;

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

        public override Operation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker> Generate(FiniteShapePainterSolution solution)
        {
            if (solution.Shapes.Count > 0 && Randomizer.RandomBool())
            {
                int index = Randomizer.RandomInt(solution.Shapes.Count);
                (Shape obj, _) = solution.Shapes[index];

                return new RemoveShapeOperation(index, obj.BoundingBox);
            }
            else
            {
                Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
                Color color = ColorGenerator.Generate(ColorGeneratorSettings);
                
                return new InsertShapeOperation(shape, color, 0);
            }

        }

        public override void Update()
        {
            ShapeGeneratorSettings.Area *= Alpha;
            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}