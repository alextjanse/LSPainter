using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.Maths.DCEL;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public abstract class PlanSubChange : CanvasChange
    {
        public abstract PlanSubScore Try(PlanSubSolution solution, CanvasSolutionChecker checker);
        public abstract void Apply(PlanSubSolution solution);
    }

    public class FaceColorChange : PlanSubChange
    {
        public Face Face { get; }
        public Color Color { get; }

        public override BoundingBox BoundingBox => throw new NotImplementedException();

        public FaceColorChange(Face face, Color color)
        {
            Face = face;
            Color = color;
        }

        public override PlanSubScore Try(PlanSubSolution solution, CanvasSolutionChecker checker)
        {
            Triangulation triangulation = solution.Triangulations[Face.ID];

            Color currentColor = solution.faceColors[Face.ID];
            Color newColor = Color.Blend(currentColor, Color);

            long pixelDiff = 0;

            foreach (Triangle triangle in triangulation.Triangles)
            {
                pixelDiff += TryDrawShape(solution, checker, triangle, newColor);
            }

            return new PlanSubScore(0, pixelDiff);
        }

        public override void Apply(PlanSubSolution solution)
        {
            Triangulation triangulation = solution.Triangulations[Face.ID];

            Color currentColor = solution.faceColors[Face.ID];
            Color newColor = Color.Blend(currentColor, Color);

            foreach (Triangle triangle in triangulation.Triangles)
            {
                DrawShape(solution, triangle, newColor);
            }
        }

        public override IScore<CanvasSolution> Try(CanvasSolution solution, ISolutionChecker<CanvasSolution> solutionChecker)
        {
            throw new NotImplementedException();
        }

        public override void Apply(CanvasSolution solution)
        {
            throw new NotImplementedException();
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

        public override BoundingBox BoundingBox => throw new NotImplementedException();

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

        public override PlanSubScore Try(PlanSubSolution solution, ISolutionChecker<CanvasSolution> checker)
        {
            long pixelDiff = 0;

            foreach (Triangle triangle in triangulation1.Triangles)
            {
                pixelDiff += TryDrawShape(solution, (CanvasSolutionChecker)checker, triangle, Color1);
            }

            foreach (Triangle triangle in triangulation2.Triangles)
            {
                pixelDiff += TryDrawShape(solution, (CanvasSolutionChecker)checker, triangle, Color2);
            }

            return new PlanSubScore(0, pixelDiff);
        }

        public override void Apply(PlanSubSolution solution)
        {
            foreach (Triangle triangle in triangulation1.Triangles)
            {
                DrawShape(solution, triangle, Color1);
            }

            foreach (Triangle triangle in triangulation2.Triangles)
            {
                DrawShape(solution, triangle, Color2);
            }
        }

        public override IScore<CanvasSolution> Try(CanvasSolution solution, ISolutionChecker<CanvasSolution> solutionChecker)
        {
            throw new NotImplementedException();
        }

        public override void Apply(CanvasSolution solution)
        {
            throw new NotImplementedException();
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