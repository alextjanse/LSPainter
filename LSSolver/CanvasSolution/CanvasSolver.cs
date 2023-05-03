namespace LSPainter.LSSolver.Canvas
{
    public class CanvasSolver : ISolver<CanvasSolution>
    {
        public CanvasSolution Solution { get; }
        public IChecker Checker { get; }

        public ISearchAlgorithm SearchAlgorithm { get; }

        public CanvasSolver(CanvasSolution initialSolution, CanvasChecker checker)
        {
            Solution = initialSolution;
            Checker = checker;

            SearchAlgorithm = new SimulatedAnnealingSolver();
        }
    }
}