namespace LSPainter.LSSolver
{
    public interface ISolution<TChange> where TChange : IChange
    {
        long Score { get; }
        TChange GenerateNeighbor();
        long TryChange(TChange change);
        void ApplyChange(TChange change);
        void Iterate();
    }
}