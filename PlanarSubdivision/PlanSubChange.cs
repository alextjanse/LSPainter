using LSPainter.Maths.DCEL;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public abstract class PlanSubChange : CanvasChange
    {
        public abstract long Try(PlanSubSolution solution);
        public abstract void Apply(PlanSubSolution solution);
    }

    public class FaceColorChange : PlanSubChange
    {
        public Face Face { get; }
        public Color Color { get; }

        public FaceColorChange(Face face, Color color)
        {
            Face = face;
            Color = color;
        }

        public override long Try(PlanSubSolution solution)
        {
            Triangulation triangulation = solution.Triangulations[Face.ID];

            Color currentColor = solution.faceColors[Face.ID];
            Color newColor = Color.Blend(currentColor, Color);

            long scoreDiff = 0;

            foreach (Triangle triangle in triangulation.Triangles)
            {
                scoreDiff += solution.TryDrawShape(triangle, newColor);
            }

            return scoreDiff;
        }

        public override void Apply(PlanSubSolution solution)
        {
            Triangulation triangulation = solution.Triangulations[Face.ID];

            Color currentColor = solution.faceColors[Face.ID];
            Color newColor = Color.Blend(currentColor, Color);

            foreach (Triangle triangle in triangulation.Triangles)
            {
                solution.DrawShape(triangle, newColor);
            }
        }
    }

    public class FaceSplitChange : PlanSubChange
    {
        public Face Face { get; }
        public Face Split1 { get; }
        public Face Split2 { get; }
        public Vertex V1 { get; }
        public Vertex V2 { get; }
        public Color Color1 { get; }
        public Color Color2 { get; }

        private Triangulation triangulation1, triangulation2;

        public FaceSplitChange(Face face, Vertex v1, Vertex v2, Color color1, Color color2)
        {
            Face = face;
            V1 = v1;
            V2 = v2;
            Color1 = color1;
            Color2 = color2;

            (Split1, Split2) = face.SplitAt(v1, v2);

            triangulation1 = new Triangulation(Split1);
            triangulation2 = new Triangulation(Split2);
        }

        public override long Try(PlanSubSolution solution)
        {
            long scoreDiff = 0;

            foreach (Triangle triangle in triangulation1.Triangles)
            {
                scoreDiff += solution.TryDrawShape(triangle, Color1);
            }

            foreach (Triangle triangle in triangulation2.Triangles)
            {
                scoreDiff += solution.TryDrawShape(triangle, Color2);
            }

            return scoreDiff;
        }

        public override void Apply(PlanSubSolution solution)
        {
            foreach (Triangle triangle in triangulation1.Triangles)
            {
                solution.DrawShape(triangle, Color1);
            }

            foreach (Triangle triangle in triangulation2.Triangles)
            {
                solution.DrawShape(triangle, Color2);
            }
        }
    }

    public class NewVertexChange : PlanSubChange
    {
        public Face Face { get; }
        public Vertex V1 { get; }
        public Vertex V2 { get; }
        public Vertex NewV { get; }
        public Color Color1 { get; }
        public Color Color2 { get; }

        public NewVertexChange(Face face, Vertex v1, Vertex v2, Vertex newV, Color color1, Color color2)
        {
            Face = face;
            V1 = v1;
            V2 = v2;
            NewV = newV;
            Color1 = color1;
            Color2 = color2;
        }

        public override long Try(PlanSubSolution solution)
        {
            throw new NotImplementedException();
        }

        public override void Apply(PlanSubSolution solution)
        {
            throw new NotImplementedException();
        }
    }
}