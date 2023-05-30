using LSPainter.LSSolver.Painter;
using LSPainter.LSSolver;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter.ShapePainter
{
    public class ShapePainterScore : CanvasScore
    {
        public ShapePainterScore(long pixelDiff) : base(pixelDiff)
        {
        }
    }

    public class ShapePainterChange

    public class ShapePainterSolver : CanvasSolver
    {
        public ShapePainterSolver(
            ShapePainterSolution initialSolution,
            CanvasSolutionChecker checker
        ) : base(
            initialSolution,
            new PlainCanvasConstraints(),
            checker)
        {
        }
    }
}