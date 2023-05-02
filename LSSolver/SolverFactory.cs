using LSPainter.LSSolver.CanvasSolution;
using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public class SolverFactory
    {
        public enum SolverType
        {
            ShapePainter
        }

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