namespace LSPainter.Solver
{
    public interface ISolution<TChange>
        where TChange : IChange
    {
        public long Score { get; protected set; }
        public long TryChange(TChange change);
        public void ApplyChange(TChange change);
        public void Draw(Frame frame);
    }
}