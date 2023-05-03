using LSPainter.LSSolver.Canvas;
using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter
    }

    public class SolverFactory
    {
        public static CanvasSolver CreateSolver(SolverType type, int width, int height, CanvasChecker checker)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    ShapePainterSolution solution = new ShapePainterSolution(width, height);
                    return new ShapePainterSolver(solution, checker);
                default:
                    throw new Exception("Unknown solver type");
            }
        }
    }
}