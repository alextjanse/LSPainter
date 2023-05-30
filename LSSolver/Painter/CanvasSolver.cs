namespace LSPainter.LSSolver.Painter
{
    public class CanvasSolver : Solver<CanvasSolution>
    {
        public CanvasSolver(
            CanvasSolution initialSolution,
            IConstraints<CanvasSolution> constraints,
            CanvasSolutionChecker solutionChecker
            ) :
            base(new SimulatedAnnealingSolver(), initialSolution, constraints, solutionChecker)
        {

        }
    }
}