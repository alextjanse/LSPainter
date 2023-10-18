using LSPainter.LSSolver;

namespace LSPainter.Maths
{
    public class ShapeGeneratorSettings : ICloneable
    {
        public RangeParameter XRange { get; set; }
        public RangeParameter YRange { get; set; }
        public RandomValueParameter Area { get; set; }

        public SelectionParameter<Shape.Type> ShapeTypes { get; set; }

        public ShapeGeneratorSettings(RangeParameter xRange, RangeParameter yRange, RandomValueParameter area, SelectionParameter<Shape.Type> shapeTypes)
        {
            XRange = xRange;
            YRange = yRange;
            Area = area;
            ShapeTypes = shapeTypes;
        }

        public object Clone()
        {
            return new ShapeGeneratorSettings(XRange, YRange, Area, ShapeTypes);
        }
    }

    public class ShapeGenerator
    {
        public ShapeGeneratorSettings Settings { get; set; }

        public ShapeGenerator(ShapeGeneratorSettings settings)
        {
            Settings = settings;
        }

        public Shape Generate()
        {
            return Settings.ShapeTypes.PickValue() switch
            {
                Shape.Type.Triangle => GenerateTriangle(),
                Shape.Type.Circle => GenerateCircle(),
                _ => throw new Exception("Unknown shape type"),
            };
        }

        Vector PickPointOnCanvas()
        {
            double x = Settings.XRange.PickValue();
            double y = Settings.YRange.PickValue();
            
            return new Vector(x, y);
        }

        double PickArea()
        {
            return Settings.Area.PickValue();
        }

        Triangle GenerateTriangle()
        {
            double area = PickArea();

            Vector p1 = PickPointOnCanvas();
            
            double gamma = Randomizer.RandomDouble(0.1, 0.9) * Math.PI;

            double remainder = 2 * area / Math.Sin(gamma);

            // Lengths a and b of the triangle
            double[] factors = Randomizer.RandomFactors(remainder, 2);

            double angle1 = 2 * Math.PI * Randomizer.RandomDouble();
            double angle2 = angle1 - gamma;

            Vector p2 = p1 + factors[0] * Vector.UnitVector(angle1);
            Vector p3 = p1 + factors[1] * Vector.UnitVector(angle2);

            return new Triangle(p1, p2, p3);
        }

        Circle GenerateCircle()
        {
            Vector origin = PickPointOnCanvas();
            double area = PickArea();

            double radius = Math.Sqrt(area / Math.PI);

            return new Circle(origin, radius);
        }
    }
}