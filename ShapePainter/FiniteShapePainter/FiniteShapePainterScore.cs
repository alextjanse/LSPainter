using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter.FiniteShapePainter
{
    public class FiniteShapePainterScore : CanvasScore<FiniteShapePainterSolution>
    {
        public int NumberOfShapes { get; }

        public FiniteShapePainterScore(int nShapes, long squaredPixelDiff) : base(squaredPixelDiff)
        {
            NumberOfShapes = nShapes;
        }

        public override object Clone()
        {
            return new FiniteShapePainterScore(NumberOfShapes, SquaredPixelDiff);
        }
    }
}