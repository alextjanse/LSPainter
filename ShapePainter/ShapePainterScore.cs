using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterScore : CanvasScore<ShapePainterSolution>
    {
        public ShapePainterScore(long squaredPixelDiff) : base(squaredPixelDiff)
        {
        }

        public override object Clone()
        {
            return new ShapePainterScore(SquaredPixelDiff);
        }
    }
}