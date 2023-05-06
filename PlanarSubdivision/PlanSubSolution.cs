using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class PlanSubSolution : CanvasSolution
    {
        Dictionary<uint, Color> faceColors;
        Dictionary<uint, Triangulation> triangulations;
        DCEL dcel;

        public PlanSubSolution(int width, int height, CanvasComparer comparer) : base(width, height, comparer)
        {
            faceColors = new Dictionary<uint, Color>();
            triangulations = new Dictionary<uint, Triangulation>();

            dcel = DCELGenerator.CreateCanvas(width, height);
            
            foreach (Face face in dcel.Faces)
            {
                faceColors.Add(face.ID, Color.Black);
                triangulations.Add(face.ID, new Triangulation(face));
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
            Triangulation triangulation = triangulations[id];

            foreach ((_, Triangle triangle) in triangulation.Triangles)
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

        PlanSubChange GenerateFaceSplitChange()
        {
            Face face = Randomizer.Pick(dcel.Faces);
            return PlanSubChangeGenerator.FaceSplitChangeFromTriangulation(face, triangulations[face.ID]);
        }

        protected override long TryChange(CanvasChange change) => TryPlanSubChange((PlanSubChange)change);

        protected override void ApplyChange(CanvasChange change) => ApplyPlanSubChange((PlanSubChange)change);

        long TryPlanSubChange(PlanSubChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            Color color = fcChange.Color;

            long scoreDiff = 0;

            foreach (Face face in fcChange.GetAlteredFaces())
            {
                Color currentColor = faceColors[face.ID];
                Triangulation triangulation = triangulations[face.ID];

                Color newColor = Color.Blend(currentColor, color);

                foreach ((_, Triangle triangle) in triangulation.Triangles)
                {
                    scoreDiff += TryDrawShape(triangle, newColor);
                }
            }

            return scoreDiff;
        }

        void ApplyPlanSubChange(PlanSubChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            Color color = fcChange.Color;

            foreach (Face face in fcChange.GetAlteredFaces())
            {
                Color currentColor = faceColors[face.ID];
                Triangulation triangulation = triangulations[face.ID];
                
                Color newColor = Color.Blend(currentColor, color);

                foreach ((_, Triangle triangle) in triangulation.Triangles)
                {
                    DrawShape(triangle, newColor);
                }

                faceColors[face.ID] = newColor;
            }
        }
    }
}