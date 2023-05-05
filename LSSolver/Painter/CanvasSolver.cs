namespace LSPainter.LSSolver.Painter
{
    public class CanvasSolver : Solver<ICanvasSolution>
    {
        public CanvasSolver(CanvasSolution initialSolution) : base(new SimulatedAnnealingSolver(), initialSolution)
        {
        }
    }
}