using LSPainter.Solver;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolver : SimulatedAnnealingSolver<CanvasSolutionChecker<ShapePainterChange>, CanvasSolution<ShapePainterChange>, ShapePainterChange>
    {
        static Random random = new Random();

        public override CanvasSolution<ShapePainterChange> Solution { get; }
        public override CanvasSolutionChecker<ShapePainterChange> SolutionChecker { get; }

        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        public ShapePainterSolver(ImageHandler original)
        {
            Solution = new ShapePainterSolution(original.Width, original.Height);
            SolutionChecker = new CanvasSolutionChecker<ShapePainterChange>(original);

            shapeGeneratorSettings = new ShapeGeneratorSettings()
            {
                MinX = 0,
                MaxX = original.Width,
                MinY = 0,
                MaxY = original.Height,
                Area = 20,
            };

            colorGeneratorSettings = new ColorGeneratorSettings()
            {
                Alpha = 255 / 10,
            };
        }

        protected override long TryChange(ShapePainterChange change)
        {
            Shape shape = change.Shape;
            Color color = change.Color;

            long totalDiff = 0;

            BoundingBox bbox = shape.CreateBoundingBox();

            int minX = Math.Max(0, bbox.X);
            int maxX = Math.Min(Solution.Canvas.Width, bbox.X + bbox.Width);
            int minY = Math.Max(0, bbox.Y);
            int maxY = Math.Min(Solution.Canvas.Height, bbox.Y + bbox.Height);

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    if (shape.IsInside(x, y))
                    {
                        Color currentColor = Solution.Canvas.GetPixel(x, y);
                        Color blendedColor = Color.Blend(currentColor, color);

                        (int dRCurrent, int dGCurrent, int dBCurrent) = SolutionChecker.PixelDiff(x, y, currentColor);
                        (int dRNew, int dGNew, int dBNew) = SolutionChecker.PixelDiff(x, y, blendedColor);

                        int rDiff = dRNew - dRCurrent;
                        int gDiff = dGNew - dGCurrent;
                        int bDiff = dBNew - dBCurrent;
                        
                        int penaltyFactor = 10000;

                        if (rDiff > 0) rDiff *= penaltyFactor;
                        if (gDiff > 0) gDiff *= penaltyFactor;
                        if (bDiff > 0) bDiff *= penaltyFactor;

                        totalDiff += rDiff + gDiff + bDiff;
                    }
                }
            }

            return totalDiff;
        }

        protected override ShapePainterChange GenerateNeighbour()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            return new ShapePainterChange(shape, color);
        }

        protected override void ApplyChange(ShapePainterChange change)
        {
            Shape shape = change.Shape;
            Color color = change.Color;

            Solution.Canvas.DrawShape(shape, color);
        }
    }
}