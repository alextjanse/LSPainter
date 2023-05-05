using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    public abstract class PlanarSubdivisionChange : CanvasChange
    {
        public abstract IEnumerable<Face> GetAlteredFaces();
    }

    public class FaceColorChange : PlanarSubdivisionChange
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
}