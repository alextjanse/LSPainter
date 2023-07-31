using LSPainter.Maths;
using LSPainter.LSSolver;
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
            (Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>, float)[] generators = new (Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>, float)[]
            {
                (GenerateInsertOperation, 1f),
                (GenerateRemoveOperation, 1f),
                (GenerateReplaceOperation, 1f),
                (GenerateRecolorOperation, 1f),
                (GenerateTranslateOperation, 1f),
            };

            FiniteShapePainterOperation? operation = null;

            while (operation == null)
            {
                var generator = Randomizer.PickRandomly(generators);
                
                operation = generator(solution);
            }

            return operation;
        }

        FiniteShapePainterOperation? GenerateInsertOperation(FiniteShapePainterSolution solution)
        {
            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);
            int index = Randomizer.RandomInt(solution.Shapes.Count);

            return new InsertOperation(shape, color, index);
        }

        FiniteShapePainterOperation? GenerateRemoveOperation(FiniteShapePainterSolution solution)
        {
            if (solution.Shapes.Count == 0) return null;

            int index = Randomizer.RandomInt(solution.Shapes.Count);
            (Shape shape, _) = solution.Shapes[index];

            return new RemoveOperation(index, shape.BoundingBox);
        }

        FiniteShapePainterOperation? GenerateReplaceOperation(FiniteShapePainterSolution solution)
        {
            if (solution.Shapes.Count == 0) return null;

            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);
            int index = Randomizer.RandomInt(solution.Shapes.Count);

            return new ReplaceOperation(shape, color, solution.Shapes[index].shape, index);
        }

        FiniteShapePainterOperation? GenerateRecolorOperation(FiniteShapePainterSolution solution)
        {
            if (solution.Shapes.Count == 0) return null;

            int index = Randomizer.RandomInt(solution.Shapes.Count);
            (Shape shape, _) = solution.Shapes[index];
            Color blendColor = ColorGenerator.Generate(ColorGeneratorSettings);

            return new RecolorOperation(index, blendColor, shape.BoundingBox);
        }

        FiniteShapePainterOperation? GenerateTranslateOperation(FiniteShapePainterSolution solution)
        {
            if (solution.Shapes.Count == 0) return null;

            int index = Randomizer.RandomInt(solution.Shapes.Count);

            (Shape s, _) = solution.Shapes[index];

            double angle = Randomizer.RandomAngle();
            double magnitude = Randomizer.RandomDouble(0, 100);

            Vector translation = ShapeGenerator.GenerateUnitVector(angle) * magnitude;

            Rectangle newBoundingBox = (Rectangle)s.BoundingBox.Clone();
            newBoundingBox.Translate(translation);
            Rectangle union = Rectangle.Union(s.BoundingBox, newBoundingBox);

            return new TranslateOperation(index, translation, union);
        }

        public override void Update()
        {
            ShapeGeneratorSettings.MaxArea *= Alpha;
            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}