namespace LSPainter.Shapes
{
    public struct ShapeGeneratorSettings
    {
        public float MinX, MaxX, MinY, MaxY, Area;
    }

    public class ShapeGenerator
    {
        static Random random = new Random();
        public static Shape Generate(ShapeGeneratorSettings settings)
        {
            return GenerateTriangle(settings);
        }

        static Point GeneratePoint(ShapeGeneratorSettings settings)
        {
            float x = random.NextSingle() * (settings.MaxX - settings.MinX) + settings.MinX;
            float y = random.NextSingle() * (settings.MaxY - settings.MinY) + settings.MinY;

            return new Point(x, y);
        }

        static Vector GenerateUnitVector(float angle)
        {
            return new Vector((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        static Triangle GenerateTriangle(ShapeGeneratorSettings settings)
        {
            Point p1 = GeneratePoint(settings);

            float gamma = Randomizer.FloatInRange(0.1f, 0.9f) * (float)Math.PI;

            float remainder = 2 * settings.Area / (float)Math.Sin(gamma);

            // Lengths a and b of the triangle
            float[] factors = Randomizer.RandomFactors(remainder, 2);

            float angle1 = 2 * (float)Math.PI * random.NextSingle();
            float angle2 = angle1 - gamma;

            Point p2 = p1 + factors[0] * GenerateUnitVector(angle1);
            Point p3 = p1 + factors[1] * GenerateUnitVector(angle2);

            return new Triangle(p1, p2, p3);
        }
    }
}