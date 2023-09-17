using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;
using LSPainter.FiniteShapePainter.Operations;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterOperationFactory : CanvasOperationFactory<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>
    {
        public ShapeGeneratorSettings ShapeGeneratorSettings { get; }
        public ColorGeneratorSettings ColorGeneratorSettings { get; }
        public ColorPalette ColorPalette { get; }
        
        public double Alpha = 1;

        public FiniteShapePainterOperationFactory(int canvasWidth, int canvasHeight, ColorPalette colorPalette) : base(canvasWidth, canvasHeight)
        {
            ColorPalette = colorPalette;

            ShapeGeneratorSettings = new ShapeGeneratorSettings(0, canvasWidth, 0, canvasHeight, 400);
            ColorGeneratorSettings = new ColorGeneratorSettings(50);
        }

        public override Operation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker> Generate(FiniteShapePainterSolution solution)
        {
            (Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>, float)[] generators = new (Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>, float)[]
            {
                (GenerateInsertOperation, 4f),
                (GenerateRemoveOperation, 1f),
                (GenerateReplaceOperation, 1f),
                (GenerateRecolorOperation, 10f),
                (GenerateTranslateOperation, 4f),
                (GenerateReorderOperation, 2f),
                (GenerateResizeOperation, 4f),
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
            // Color color = Randomizer.PickRandomly<Color>(ColorPalette.Colors);
            int index = Randomizer.RandomInt(solution.NumberOfShapes);

            return new InsertOperation(shape, color, index, TrimToCanvas(shape.BoundingBox));
        }

        FiniteShapePainterOperation? GenerateRemoveOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            int index = Randomizer.RandomInt(solution.NumberOfShapes);
            (Shape shape, _) = solution.Shapes[index];

            return new RemoveOperation(index, TrimToCanvas(shape.BoundingBox));
        }

        FiniteShapePainterOperation? GenerateReplaceOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            Shape shape = ShapeGenerator.Generate(ShapeGeneratorSettings);
            Color color = ColorGenerator.Generate(ColorGeneratorSettings);
            // Color color = Randomizer.PickRandomly<Color>(ColorPalette.Colors);
            int index = Randomizer.RandomInt(solution.NumberOfShapes);

            (Shape currentShape, Color currentColor) = solution.Shapes[index];

            Rectangle boundingBox = TrimToCanvas(Rectangle.Union(currentShape.BoundingBox, shape.BoundingBox));

            return new ReplaceOperation(shape, color, index, boundingBox);
        }

        FiniteShapePainterOperation? GenerateRecolorOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            int index = Randomizer.RandomInt(solution.NumberOfShapes);
            (Shape shape, _) = solution.Shapes[index];
            Color blendColor = ColorGenerator.Generate(ColorGeneratorSettings);

            return new RecolorOperation(index, blendColor, TrimToCanvas(shape.BoundingBox));
        }

        FiniteShapePainterOperation? GenerateTranslateOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            int index = Randomizer.RandomInt(solution.NumberOfShapes);

            (Shape s, _) = solution.Shapes[index];

            double angle = Randomizer.RandomAngle();
            double magnitude = Randomizer.RandomDouble(0, 100);

            Vector translation = ShapeGenerator.GenerateUnitVector(angle) * magnitude;

            Rectangle newBoundingBox = (Rectangle)s.BoundingBox.Clone();
            newBoundingBox.Translate(translation);
            Rectangle boundingBox = TrimToCanvas(Rectangle.Union(s.BoundingBox, newBoundingBox));

            return new TranslateOperation(index, translation, boundingBox);
        }

        FiniteShapePainterOperation? GenerateReorderOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes < 2) return null;

            int index1 = Randomizer.RandomInt(solution.NumberOfShapes - 1);
            int index2 = Randomizer.RandomInt(index1 + 1, solution.NumberOfShapes);

            (Shape shape1, Color color1) = solution.Shapes[index1];
            (Shape shape2, Color color2) = solution.Shapes[index2];

            Rectangle boundingBox = TrimToCanvas(Rectangle.Union(shape1.BoundingBox, shape2.BoundingBox));

            return new ReorderOperation(index1, index2, boundingBox);
        }

        FiniteShapePainterOperation? GenerateResizeOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            int index = Randomizer.RandomInt(solution.NumberOfShapes);
            (Shape shape, _) = solution.Shapes[index];

            double scale = Randomizer.RandomDouble(0, 2);

            Rectangle resized = (Rectangle)shape.BoundingBox.Clone();
            resized.Resize(scale);

            Rectangle boundingBox = TrimToCanvas(Rectangle.Union(resized, shape.BoundingBox));

            return new ResizeOperation(index, scale, boundingBox);
        }

        public override void Update()
        {
            ShapeGeneratorSettings.MaxArea *= Alpha;
            ColorGeneratorSettings.Alpha *= Alpha;
        }
    }
}