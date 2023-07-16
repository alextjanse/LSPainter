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
        public CanvasChecker<CanvasSolution, CanvasScore<CanvasSolution>> Checker { get; }

        public SolverFactory(CanvasChecker<CanvasSolution, CanvasScore<CanvasSolution>> checker)
        {
            Checker = checker;
        }

        public Solver<CanvasSolution, CanvasScore<CanvasSolution>, CanvasChecker<CanvasSolution, CanvasScore<CanvasSolution>>> CreateCanvasSolver(SolverType type, int width, int height)
        {
            switch (type)
            {
                case SolverType.ShapePainter:
                    ShapePainterSolution solution = new ShapePainterSolution(new Canvas(width, height));
                    ShapePainterOperationFactory factory = new ShapePainterOperationFactory(width, height, Checker);

                    return new Solver<CanvasSolution, CanvasScore<CanvasSolution>, CanvasChecker<CanvasSolution, CanvasScore<CanvasSolution>>>(solution, Checker, new SimulatedAnnealingAlgorithm(), factory);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}