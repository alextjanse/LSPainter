using LSPainter.LSSolver.Painter;
using LSPainter.ShapePainter;
using LSPainter.PlanarSubdivision;

namespace LSPainter.LSSolver
{
    public enum SolverType
    {
        ShapePainter,
        PlanarSubdivision
    }

    public class SolverFactory
    {
        public static CanvasSolver CreateCanvasSolver(SolverType type, int width, int height, CanvasComparer checker)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    return CreateShapePainterSolver(width, height, checker);
                case SolverType.PlanarSubdivision:
                    return CreatePlanarSubdivisionSolver(width, height, checker);
                default:
                    throw new Exception("Unknown solver type");
            }
        }

        static ShapePainterSolver CreateShapePainterSolver(int width, int height, CanvasComparer checker)
        {
            ShapePainterSolution solution = new ShapePainterSolution(width, height, checker);
            return new ShapePainterSolver(solution);
        }

        static PlanarSubdivisionSolver CreatePlanarSubdivisionSolver(int width, int height, CanvasComparer checker)
        {
            PlanarSubdivisionSolution solution = new PlanarSubdivisionSolution(width, height, checker);
            return new PlanarSubdivisionSolver(solution);
        }
    }
}