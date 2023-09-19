using LSPainter.LSSolver.Painter;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterScore : CanvasScore<FiniteShapePainterSolution>
    {
        public int NumberOfShapes { get; set; }

        public FiniteShapePainterScore(int nShapes, long squaredPixelDiff, long blankPixels) : base(squaredPixelDiff, blankPixels)
        {
            NumberOfShapes = nShapes;
        }

        public override object Clone()
        {
            return new FiniteShapePainterScore(NumberOfShapes, SquaredPixelDiff, BlankPixels);
        }
    }
}