using LSPainter.Maths.DCEL;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public class PlanSubChangeGenerator
    {
        public static FaceSplitChange FaceSplitChangeFromTriangulation(Face face, Triangulation triangulation)
        {
            (Face tFace, Triangle triangle) = Randomizer.Pick(triangulation.Triangles);

            /* 
            All the points in a triangle (p1, p2, p3) can be written as:

            p(x, y) = p1 + (p2 - p1) * a + (p3 - p1) * b where a, b in [0..1] and a + b <= 1
             */
            
            double[] factors = Randomizer.Split(1, 2);

            double d = Randomizer.RandomDouble();

            Vector u = triangle.P2 - triangle.P1;
            Vector v = triangle.P3 - triangle.P1;

            Vector p = triangle.P1 + (factors[0] * d * u) + (factors[1] * d * v);

            Vertex[] vertices = Randomizer.PickMultiple<Vertex>(
                tFace.Vertices.Select(v => triangulation.VertexRefs[v]).ToArray(),
                2
            );

            Vertex v1 = vertices[0];
            Vertex v2 = vertices[1];
            Vertex newV = new Vertex(p.X, p.Y);

            Color color1 = ColorGenerator.Generate();
            Color color2 = ColorGenerator.Generate();

            return new FaceSplitChange(face, v1, v2, newV, color1, color2);
        }
    }
}