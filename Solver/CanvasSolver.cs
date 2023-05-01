namespace LSPainter.LSSolver
{
    public interface ICanvasSolver : ISolver
    {
        Painting GetCanvas();
    }

    public abstract class CanvasSolver<TChange> : Solver<CanvasSolutionChecker<TChange>, CanvasSolution<TChange>, TChange>, ICanvasSolver
    where TChange : Change
    {
        Painting ICanvasSolver.GetCanvas()
        {
            return Solution.Canvas;
        }
    }
}