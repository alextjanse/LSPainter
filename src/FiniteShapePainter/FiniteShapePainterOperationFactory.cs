using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;
using LSPainter.FiniteShapePainter.Operations;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterOperationFactory : CanvasOperationFactory<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>
    {
        public ShapeGenerator ShapeGenerator { get; }
        public ColorGenerator ColorGenerator { get; }
        public OptionsParameter<Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>> NeighbourGenerators { get; }
        
        public double Alpha = 1;

        public FiniteShapePainterOperationFactory(int canvasWidth, int canvasHeight) : base(canvasWidth, canvasHeight)
        {
            RangeParameter xRange = new RangeParameter(0, canvasWidth);
            RangeParameter yRange = new RangeParameter(0, canvasHeight);
            AverageValueParameter area = new AverageValueParameter(500, 400);

            var shapeOptions = new OptionsParameter<Shape.Type>(
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

            NeighbourGenerators = new OptionsParameter<Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>>(
                new (Func<FiniteShapePainterSolution, FiniteShapePainterOperation?>, double)[]
                {
                    (GenerateInsertOperation,       1.0),
                    (GenerateRemoveOperation,       1.0),
                    (GenerateReplaceOperation,      1.0),
                    (GenerateRecolorOperation,      1.0),
                    (GenerateTranslateOperation,    1.0),
                    (GenerateReorderOperation,      1.0),
                    (GenerateResizeOperation,       1.0)
                }
            );
        }

        public override Operation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker> Generate(FiniteShapePainterSolution solution)
        {
            FiniteShapePainterOperation? operation = null;

            while (operation == null)
            {
                var generator = NeighbourGenerators.PickValue();
                
                operation = generator(solution);
            }

            return operation;
        }

        FiniteShapePainterOperation? GenerateInsertOperation(FiniteShapePainterSolution solution)
        {
            Shape shape = ShapeGenerator.Generate();
            Color color = ColorGenerator.Generate();
            
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

            Shape shape = ShapeGenerator.Generate();
            Color color = ColorGenerator.Generate();
            
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
            Color blendColor = ColorGenerator.Generate();

            return new RecolorOperation(index, blendColor, TrimToCanvas(shape.BoundingBox));
        }

        FiniteShapePainterOperation? GenerateTranslateOperation(FiniteShapePainterSolution solution)
        {
            if (solution.NumberOfShapes == 0) return null;

            int index = Randomizer.RandomInt(solution.NumberOfShapes);

            (Shape s, _) = solution.Shapes[index];

            double angle = Randomizer.RandomAngle();
            double magnitude = Randomizer.RandomDouble(0, 100);

            Vector translation = Vector.UnitVector(angle) * magnitude;

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

            (Shape shape1, _) = solution.Shapes[index1];
            (Shape shape2, _) = solution.Shapes[index2];

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
        }
    }
}