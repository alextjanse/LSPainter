using LSPainter.LSSolver.Painter;
using LSPainter.Maths.Shapes;
using LSPainter.Maths;

namespace LSPainter.ShapePainter
{
    public class ShapePainterSolver : CanvasSolver
    {
        static Random random = new Random();

        public ShapePainterSolver(ShapePainterSolution initialSolution) : base(initialSolution)
        {
        }
    }
}