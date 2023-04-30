namespace LSPainter.Solver
{
    public abstract class CanvasSolution<TChange> : Solution<TChange> where TChange : Change
    {
        public Painting Canvas { get; }

        public CanvasSolution(int width, int height)
        {
            Canvas = new Painting(width, height);
        }
    }
}