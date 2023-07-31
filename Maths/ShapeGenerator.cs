namespace LSPainter.Maths
{
    public class ShapeGeneratorSettings : ICloneable
    {
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }
        public double MaxArea { get; set; }

        public ShapeGeneratorSettings(double minX, double maxX, double minY, double maxY, double maxArea)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MaxArea = maxArea;
        }

        public object Clone()
        {
            return new ShapeGeneratorSettings(MinX, MaxX, MinY, MaxY, MaxArea);
        }
    }

    public class ShapeGenerator
    {
        static (Func<ShapeGeneratorSettings, Shape>, float)[] generators = new (Func<ShapeGeneratorSettings, Shape>, float)[]
        {
            (GenerateCircle, 0.5f),
            (GenerateTriangle, 0.5f),
        };

        static Random random = new Random();
        public static Shape Generate(ShapeGeneratorSettings settings)
        {
            Func<ShapeGeneratorSettings, Shape> generator = Randomizer.PickRandomly(generators);
            return generator(settings);
        }

        public static Vector GenerateVector(ShapeGeneratorSettings settings)
        {
            double x = Randomizer.RandomDouble(settings.MinX, settings.MaxX);
            double y = Randomizer.RandomDouble(settings.MinY, settings.MaxY);

            return new Vector(x, y);
        }

        public static Vector GenerateUnitVector(double angle)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        static Triangle GenerateTriangle(ShapeGeneratorSettings settings)
        {
            double area = GenerateArea(settings);

            Vector p1 = GenerateVector(settings);
            
            double gamma = Randomizer.RandomDouble(0.1f, 0.9f) * Math.PI;

            double remainder = 2 * area / Math.Sin(gamma);

            // Lengths a and b of the triangle
            double[] factors = Randomizer.RandomFactors(remainder, 2);

            double angle1 = 2 * Math.PI * random.NextDouble();
            double angle2 = angle1 - gamma;

            Vector p2 = p1 + factors[0] * GenerateUnitVector(angle1);
            Vector p3 = p1 + factors[1] * GenerateUnitVector(angle2);

            return new Triangle(p1, p2, p3);
        }

        static Circle GenerateCircle(ShapeGeneratorSettings settings)
        {
            Vector origin = GenerateVector(settings);
            double area = GenerateArea(settings);

            double radius = Math.Sqrt(area / Math.PI);

            return new Circle(origin, radius);
        }

        static double GenerateArea(ShapeGeneratorSettings settings)
        {
            return Randomizer.RandomDouble() * settings.MaxArea;
        }
    }
}