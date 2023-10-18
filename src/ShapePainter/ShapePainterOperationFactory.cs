using LSPainter.Maths;
using LSPainter.LSSolver;

namespace LSPainter.ShapePainter
{
    public class ShapePainterOperationFactory : OperationFactory<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>
    {
        public ShapeGenerator ShapeGenerator { get; }
        public ColorGenerator ColorGenerator { get; }
        public double Alpha = 1;

        public ShapePainterOperationFactory(int canvasWidth, int canvasHeight)
        {
            RangeParameter xRange = new RangeParameter(0, canvasWidth);
            RangeParameter yRange = new RangeParameter(0, canvasHeight);
            RandomValueParameter area = new RandomValueParameter(500, 400, 0);

            var shapeOptions = new SelectionParameter<Shape.Type>(
                new (Shape.Type, double)[]
                {
                    (Shape.Type.Circle,     1.0),
                    (Shape.Type.Triangle,   1.0)
                }
            );

            ShapeGeneratorSettings shapeSettings = new ShapeGeneratorSettings(xRange, yRange, area, shapeOptions);
            ShapeGenerator = new ShapeGenerator(shapeSettings);

            ColorGeneratorSettings colorSettings = new ColorGeneratorSettings(
                new RangeParameter(0, 255)
            );

            ColorGenerator = new ColorGenerator(colorSettings);
        }

        public override Operation<ShapePainterSolution, ShapePainterScore, ShapePainterChecker> Generate(ShapePainterSolution solution)
        {
            Shape shape = ShapeGenerator.Generate();
            Color color = ColorGenerator.Generate();

            return new PaintShapeOperation(shape, color);
        }

        public override void Update()
        {
            
        }
    }
}