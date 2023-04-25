namespace LSPainter.Solver
{
    public abstract class CanvasSolution<TChange> : ISolution<TChange> where TChange : IChange
    {
        Painting painting;

        public abstract long Score { get; set; }

        public CanvasSolution(int width, int height)
        {
            painting = new Painting(width, height);
        }

        public void ApplyChange(TChange change)
        {
            change.ApplyToCanvas(painting);
        }

        public void Draw(Frame frame)
        {
            throw new NotImplementedException();
        }

        public abstract long TryChange(TChange change);
    }
}