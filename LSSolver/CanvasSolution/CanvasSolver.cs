namespace LSPainter.LSSolver.CanvasSolution
{
    public class CanvasSolver : ISolver<CanvasSimAl, CanvasChecker, CanvasSolution, CanvasChange>
    {
        public CanvasSolution Solution { get; }
        public CanvasChecker Checker { get; }

        public CanvasSimAl Searcher { get; }

        public CanvasSolver(CanvasSolution initialSolution, CanvasChecker checker)
        {
            Solution = initialSolution;
            Checker = checker;

            Searcher = new CanvasSimAl();
        }
    }
}