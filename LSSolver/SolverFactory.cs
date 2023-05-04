using LSPainter.LSSolver.Painter;
using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter
    }

    public class SolverFactory
    {
        public static CanvasSolver CreateSolver(SolverType type, int width, int height, CanvasComparer checker)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    ShapePainterSolution solution = new ShapePainterSolution(width, height, checker);
                    return new ShapePainterSolver(solution);
                default:
                    throw new Exception("Unknown solver type");
            }
        }
    }
}