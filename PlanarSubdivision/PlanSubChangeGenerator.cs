using LSPainter.Maths.DCEL;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public class PlanSubChangeGenerator
    {
        public static NewVertexChange NewVertexChangeFromTriangulation(Face face, Triangulation triangulation)
        {
            (Face tFace, Triangle triangle) = Randomizer.Pick(triangulation.TriangleFaces.Zip(triangulation.Triangles));

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

            return new NewVertexChange(face, v1, v2, newV, color1, color2);
        }

        public static FaceSplitChange GenerateFaceSplitChange(Face face)
        {
            if (face.Count() == 3)
            {
                throw new Exception("Face is a triangle, and can't be split");
            }

            Color color1 = ColorGenerator.Generate();
            Color color2 = ColorGenerator.Generate();

            Face clone = face.Clone();
            Triangulation triangulation = new Triangulation(clone);

            Face tFace = Randomizer.Pick<Face>(triangulation.TriangleFaces);

            // Get the original vertices, so we can check for adjacency
            Vertex[] vertices = tFace.Select(e => triangulation.VertexRefs[e.Origin ?? throw new NullReferenceException()]).ToArray();
            Vertex v1 = vertices[0],
                   v2 = vertices[1],
                   v3 = vertices[2];
            
            if (!v1.IsAdjacentVertex(v2)) return new FaceSplitChange(clone, v1, v2, color1, color2);
            else if (!v1.IsAdjacentVertex(v3)) return new FaceSplitChange(clone, v1, v3, color1, color2);
            else return new FaceSplitChange(clone, v2, v3, color1, color2);
        }
    }
}