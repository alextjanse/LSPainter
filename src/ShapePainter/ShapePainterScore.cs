using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterScore : CanvasScore<ShapePainterSolution>
    {
        public ShapePainterScore(long squaredPixelDiff, long blankPixels) : base(squaredPixelDiff, blankPixels)
        {
        }

        public override object Clone()
        {
            return new ShapePainterScore(SquaredPixelDiff, BlankPixels);
        }
    }
}