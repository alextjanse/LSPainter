using LSPainter.ShapePainter;

namespace LSPainter.Solver
{
    public class SolverManager
    {
        public ShapePainterSolver[] Solvers { get; }
        public IEnumerable<Painting> Paintings => Solvers.Select(s => s.Solution.Canvas );

        public SolverManager(ImageHandler original, int n)
        {
            Solvers = new ShapePainterSolver[n];

            for (int i = 0; i < n; i++)
            {
                Solvers[i] = new ShapePainterSolver(original);
            }
        }

        public void Iterate()
        {
            for (int i = 0; i < Solvers.Length; i++)
            {
                Solvers[i].Iterate();
            }
        }
    }
}