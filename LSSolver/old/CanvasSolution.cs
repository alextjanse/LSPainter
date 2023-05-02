/* namespace LSPainter.LSSolver
{
    public interface ICanvasSolution : ISolution
    {
        public Painting Canvas { get; }
    }

    public abstract class CanvasSolution<TChange> : Solution<TChange>, ICanvasSolution
        where TChange : Change
    {
        public Painting Canvas { get; }

        public CanvasSolution(int width, int height)
        {
            Canvas = new Painting(width, height);
        }
    }
} */