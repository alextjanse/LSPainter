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
        public CanvasChecker Checker { get; }

        public SolverFactory(CanvasChecker checker)
        {
            Checker = checker;
        }

        public Solver<CanvasSolution, CanvasScore, CanvasChecker> CreateSolver(SolverType type, CanvasChecker checker)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    
                default:
                    throw new NotImplementedException();
            }
        }
    }
}