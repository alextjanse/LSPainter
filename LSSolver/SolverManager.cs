using LSPainter.ShapePainter;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;
using System.Threading;

namespace LSPainter.LSSolver
{
    public class SolverManager
    {
        public List<ISolver<ICanvasSolution>> CanvasSolvers { get; }
        public IEnumerable<Canvas> EnumerateCanvases() => CanvasSolvers.Select(s => s.Solution.Canvas);
        private SolverFactory factory;

        public SolverManager(ImageHandler original, int n)
        {
            CanvasSolvers = new List<ISolver<ICanvasSolution>>();

            factory = new SolverFactory(original);

            for (int i = 0; i < n; i++)
            {
                CanvasSolvers.Add(factory.CreateCanvasSolver((i & 1) == 1 ? SolverType.ShapePainter : SolverType.FiniteShapePainter));
            }
        }

        public void Iterate()
        {
            foreach (ISolver<ICanvasSolution> solver in CanvasSolvers)
            {
                solver.Iterate();
            }
        }
    }
}