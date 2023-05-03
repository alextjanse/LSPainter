using LSPainter.ShapePainter;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Canvas;

namespace LSPainter.LSSolver
{
    public class SolverManager
    {
        public ISolver<CanvasSolution>[] Solvers { get; }
        public IEnumerable<Painting> Paintings => Solvers.Select(s => s.Solution.Canvas );

        public SolverManager(ImageHandler original, int n)
        {
            Solvers = new ShapePainterSolver[n];

            CanvasChecker checker = new CanvasChecker(original);

            for (int i = 0; i < n; i++)
            {
                Solvers[i] = SolverFactory.CreateSolver(
                    SolverType.ShapePainter,
                    original.Width,
                    original.Height,
                    checker
                );
            }
        }

        public void Iterate()
        {
            foreach (ISolver<CanvasSolution> solver in Solvers)
            {
                solver.Iterate();
            }
        }
    }
}