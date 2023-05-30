using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public class PlanSubSolution : CanvasSolution
    {
        public Dictionary<uint, Color> faceColors;
        public Dictionary<uint, Triangulation> Triangulations;
        DCEL dcel;

        Func<PlanSubChange?>[] generators;

        PlanSubSolutionConstraints constraints = new PlanSubSolutionConstraints()
        {
            MaxVertices = 100,
        };

        PlanSubScore penalties = new PlanSubScore(1000, 1);

        public PlanSubSolution(int width, int height, CanvasSolutionChecker comparer) : base(width, height, comparer)
        {
            faceColors = new Dictionary<uint, Color>();
            Triangulations = new Dictionary<uint, Triangulation>();

            dcel = DCELGenerator.CreateCanvas(width, height);
            
            foreach (Face face in dcel.Faces)
            {
                faceColors.Add(face.ID, Color.Black);
                Triangulations.Add(face.ID, new Triangulation(face));
            }

            generators = new Func<PlanSubChange?>[]
            {
                GenerateFaceColorChange,
                GenerateFaceSplitChange
            };
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
            Stack<Func<PlanSubChange?>> shuffledGenerators = new Stack<Func<PlanSubChange?>>(Randomizer.Shuffle(generators));

            while (shuffledGenerators.Count > 0)
            {
                PlanSubChange? change = shuffledGenerators.Pop()();

                if (change != null) return change;
            }

            Console.WriteLine("None of the generators could make a change. Retrying.");

            return GenerateCanvasChange();
        }

        PlanSubChange? GenerateFaceColorChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);
            Color color = ColorGenerator.Generate();

            return new FaceColorChange(face, color);
        }

        PlanSubChange? GenerateFaceSplitChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);

            if (face.Count() <= 3)
            {
                return null;
            }

            return PlanSubChangeGenerator.GenerateFaceSplitChange(face);
        }

        PlanSubChange? GenerateNewVertexChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);
            
            return PlanSubChangeGenerator.NewVertexChangeFromTriangulation(face, Triangulations[face.ID]);
        }

        protected override long TryChange(CanvasChange change) => TryPlanSubChange((PlanSubChange)change);

        protected override void ApplyChange(CanvasChange change) => ApplyPlanSubChange((PlanSubChange)change);

        long TryPlanSubChange(PlanSubChange change)
        {
            return change.Try(this);
        }

        void ApplyPlanSubChange(PlanSubChange change)
        {
            change.Apply(this);
        }
    }
}