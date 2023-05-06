using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public abstract class PlanSubChange : CanvasChange
    {
        public abstract IEnumerable<Face> GetAlteredFaces();
    }

    public class FaceColorChange : PlanSubChange
    {
        public Face Face;
        public Color Color;

        public FaceColorChange(Face face, Color color)
        {
            Face = face;
            Color = color;
        }

        public override IEnumerable<Face> GetAlteredFaces()
        {
            yield return Face;
        }
    }

    public class FaceSplitChange : PlanSubChange
    {
        public Face Face;
        public Vertex V1, V2, NewV;
        public Color Color1, Color2;

        public FaceSplitChange(Face face, Vertex v1, Vertex v2, Vertex newV, Color color1, Color color2)
        {
            Face = face;
            V1 = v1;
            V2 = v2;
            NewV = newV;
            Color1 = color1;
            Color2 = color2;
        }

        public override IEnumerable<Face> GetAlteredFaces()
        {
            yield return Face;
        }
    }
}