using LSPainter.Maths;
using LSPainter.Maths.Shapes;

namespace LSPainter.Maths.Shapes
{
    public class ShapeGeneratorSettings : ICloneable
    {
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }
        public double Area { get; set; }

        public ShapeGeneratorSettings(double minX, double maxX, double minY, double maxY, double area)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            Area = area;
        }

        public object Clone()
        {
            return new ShapeGeneratorSettings(MinX, MaxX, MinY, MaxY, Area);
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

        static Point GeneratePoint(ShapeGeneratorSettings settings)
        {
            double x = random.NextDouble() * (settings.MaxX - settings.MinX) + settings.MinX;
            double y = random.NextDouble() * (settings.MaxY - settings.MinY) + settings.MinY;

            return new Point(x, y);
        }

        static Vector GenerateUnitVector(double angle)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        static Triangle GenerateTriangle(ShapeGeneratorSettings settings)
        {
            Point p1 = GeneratePoint(settings);

            double gamma = Randomizer.RandomDouble(0.1f, 0.9f) * Math.PI;

            double remainder = 2 * settings.Area / Math.Sin(gamma);

            // Lengths a and b of the triangle
            double[] factors = Randomizer.RandomFactors(remainder, 2);

            double angle1 = 2 * Math.PI * random.NextDouble();
            double angle2 = angle1 - gamma;

            Point p2 = p1 + factors[0] * GenerateUnitVector(angle1);
            Point p3 = p1 + factors[1] * GenerateUnitVector(angle2);

            return new Triangle(p1, p2, p3);
        }

        static Circle GenerateCircle(ShapeGeneratorSettings settings)
        {
            Point origin = GeneratePoint(settings);

            double radius = Math.Sqrt(settings.Area / Math.PI);

            return new Circle(origin, radius);
        }
    }
}