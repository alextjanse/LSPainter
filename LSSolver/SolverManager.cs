using LSPainter.ShapePainter;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;

namespace LSPainter.LSSolver
{
    public class SolverManager
    {
        public CanvasSolver[] Solvers { get; }
        public IEnumerable<Canvas> EnumerateCanvases() => Solvers.Select(s => s.Solution.Canvas );

        public SolverManager(ImageHandler original, int n)
        {
            Solvers = new CanvasSolver[n];

            CanvasComparer checker = new CanvasComparer(original);

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
            foreach (CanvasSolver solver in Solvers)
            {
                solver.Iterate();
            }
        }
    }
}