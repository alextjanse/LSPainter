namespace LSPainter.Solver
{
    public abstract class Solution<TChange>
        where TChange : Change
    {
        public long Score { get; protected set; }
    }
}