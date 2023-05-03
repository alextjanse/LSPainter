namespace LSPainter.LSSolver
{
    public interface ISolution
    {
        long Score { get; }
        IChange GenerateNeighbor();
        long TryChange(IChange change);
        void ApplyChange(IChange change);
        void Iterate();
    }
}