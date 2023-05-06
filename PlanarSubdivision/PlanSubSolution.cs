using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class PlanSubSolution : CanvasSolution
    {
        public Dictionary<uint, Color> faceColors;
        public Dictionary<uint, Triangulation> Triangulations;
        DCEL dcel;

        public PlanSubSolution(int width, int height, CanvasComparer comparer) : base(width, height, comparer)
        {
            faceColors = new Dictionary<uint, Color>();
            Triangulations = new Dictionary<uint, Triangulation>();

            dcel = DCELGenerator.CreateCanvas(width, height);
            
            foreach (Face face in dcel.Faces)
            {
                faceColors.Add(face.ID, Color.Black);
                Triangulations.Add(face.ID, new Triangulation(face));
            }
        }

        public void Draw()
        {
            foreach (Face face in dcel.Faces)
            {
                DrawFace(face);
            }
        }

        void DrawFace(Face face)
        {
            uint id = face.ID;

            Color color = faceColors[id];
            Triangulation triangulation = Triangulations[id];

            foreach (Triangle triangle in triangulation.Triangles)
            {
                DrawShape(triangle, color);
            }
        }

        protected override CanvasChange GenerateCanvasChange()
        {
            Func<PlanSubChange>[] generators = new Func<PlanSubChange>[]
            {
                GenerateFaceColorChange,
            };

            return Randomizer.Pick(generators)();
        }

        PlanSubChange GenerateFaceColorChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);
            Color color = ColorGenerator.Generate();

            return new FaceColorChange(face, color);
        }

        PlanSubChange GenerateNewVertexChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);
            return PlanSubChangeGenerator.NewVertexChangeFromTriangulation(face, Triangulations[face.ID]);
        }

        protected override long TryChange(CanvasChange change) => TryPlanSubChange((PlanSubChange)change);

        protected override void ApplyChange(CanvasChange change) => ApplyPlanSubChange((PlanSubChange)change);

        long TryPlanSubChange(PlanSubChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            return fcChange.Try(this);
        }

        void ApplyPlanSubChange(PlanSubChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            fcChange.Apply(this);
        }
    }
}