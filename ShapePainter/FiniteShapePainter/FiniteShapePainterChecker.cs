using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter.FiniteShapePainter
{
    public class FiniteShapePainterChecker : CanvasChecker<FiniteShapePainterSolution, FiniteShapePainterScore>
    {
        public FiniteShapePainterChecker(ImageHandler originalImage) : base(originalImage)
        {
        }

        public override FiniteShapePainterScore ScoreSolution(FiniteShapePainterSolution solution)
        {
            int n = solution.Shapes.Count;
            long pixelScore = GetTotalPixelScore(solution);

            return new FiniteShapePainterScore(n, pixelScore);
        }
    }
}