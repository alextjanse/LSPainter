using LSPainter.Shapes;

namespace LSPainter
{
    public class SimulatedAnnealing
    {
        static Random random = new Random();
        public Painting Painting { get; }
        ImageHandler original;

        long score;
        bool scoreUpdateFlag = true;
        public long Score
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

        ShapeGeneratorSettings shapeGeneratorSettings;
        ColorGeneratorSettings colorGeneratorSettings;

        double temperature = 1000000;
        int coolingSteps = 100000;
        int iteration = 0;
        double alpha = 0.95;

        public SimulatedAnnealing(ImageHandler original)
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

        long GetScore()
        {
            long totalScore = 0;

            for (int y = 0; y < Painting.Width; y++)
            {
                for (int x = 0; x < Painting.Height; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color currentColor = Painting.GetPixel(x, y);

                    long diff = Color.Diff(originalColor, currentColor);
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
                score += scoreDiff;
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
                    score += scoreDiff;
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

                        int currentDiff = Color.Diff(originalColor, currentColor);
                        int newDiff = Color.Diff(originalColor, blendedColor);

                        // We want to minimize our score: try to get the color diff to be 0
                        int scoreDiff = newDiff - currentDiff;

                        totalDiff += scoreDiff;
                    }
                }
            }

            return totalDiff;
        }
    }
}