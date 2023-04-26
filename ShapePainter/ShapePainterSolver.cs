using LSPainter.Solver;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolver : SimulatedAnnealingSolver<CanvasSolution<ShapePainterChange>, ShapePainterChange>
    {
        static Random random = new Random();
        public Painting Painting { get; }
        ImageHandler original;

        bool scoreUpdateFlag = true;

        private ulong score;
        public ulong Score
        {
            get
            {
                if (scoreUpdateFlag)
                {
                    score = GetScore();
                    scoreUpdateFlag = false;
                }
                return score;
            }
        }

        CanvasSolution<ShapePainterChange> ISolver<CanvasSolution<ShapePainterChange>, ShapePainterChange>.Solution { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        double temperature = 1000000;
        int coolingSteps = 100000;
        int iteration = 0;
        double alpha = 0.95;

        public ShapePainterSolver(ImageHandler original)
        {
            this.original = original;

            Painting = new Painting(original.Width, original.Height);

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

            score = GetScore();
        }

        ulong GetScore()
        {
            ulong totalScore = 0;

            for (int y = 0; y < Painting.Height; y++)
            {
                for (int x = 0; x < Painting.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color currentColor = Painting.GetPixel(x, y);

                    uint diff = Color.Diff(originalColor, currentColor);
                    totalScore += diff;
                }
            }

            return totalScore;
        }

        public void Iterate()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            long scoreDiff = TryShape(shape, color);

            if (scoreDiff < 0)
            {
                // If we improve our score, apply the shape
                Painting.DrawShape(shape, color);
                score = (ulong)((long)score + scoreDiff);
            }
            else
            {
                float x = random.NextSingle();
                double p = Math.Pow(Math.E, -scoreDiff / temperature);

                if (x < p)
                {
                    // Console.WriteLine($"Accepted by chance. diff = {scoreDiff}, p = {p}");
                    // Apply the change with probablity: https://en.wikipedia.org/wiki/Simulated_annealing#Acceptance_probabilities_2
                    Painting.DrawShape(shape, color);
                    score = (ulong)((long)score + scoreDiff);
                }
            }

            if (iteration++ > coolingSteps)
            {
                iteration = 0;
                temperature *= alpha;
            }
        }

        long TryShape(Shape shape, Color color)
        {
            long totalDiff = 0;

            BoundingBox bbox = shape.BoundingBox;

            int minX = Math.Max(0, bbox.X);
            int maxX = Math.Min(original.Width, bbox.X + bbox.Width);
            int minY = Math.Max(0, bbox.Y);
            int maxY = Math.Min(original.Height, bbox.Y + bbox.Height);

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    if (shape.IsInside(x, y))
                    {
                        Color originalColor = original.GetPixel(x, y);

                        Color currentColor = Painting.GetPixel(x, y);
                        Color blendedColor = Color.Blend(currentColor, color);

                        int rDiff = Math.Abs(originalColor.R - blendedColor.R) - Math.Abs(originalColor.R - currentColor.R);
                        int gDiff = Math.Abs(originalColor.G - blendedColor.G) - Math.Abs(originalColor.G - currentColor.G);
                        int bDiff = Math.Abs(originalColor.B - blendedColor.B) - Math.Abs(originalColor.B - currentColor.B);

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

        public ShapePainterChange GenerateNeighbour()
        {
            Shape shape = ShapeGenerator.Generate(shapeGeneratorSettings);
            Color color = ColorGenerator.Generate(colorGeneratorSettings);

            return new ShapePainterChange(shape, color);
        }
    }
}