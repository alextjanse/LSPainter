using LSPainter.ShapePainter;

namespace LSPainter.LSSolver
{
    public class SolverManager
    {
        public ICanvasSolver[] Solvers { get; }
        public IEnumerable<Painting> Paintings => Solvers.Select(s => s.GetCanvas() );

        public SolverManager(ImageHandler original, int n)
        {
            Solvers = new ICanvasSolver[n];

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