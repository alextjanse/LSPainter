/* namespace LSPainter.LSSolver
{
    public interface ISolution
    {
        public long Score { get; }
    }

    public abstract class Solution<TChange> : ISolution
        where TChange : Change
    {
        public long Score { get; protected set; }
    }
} */