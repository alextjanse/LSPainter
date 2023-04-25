namespace LSPainter.Solver
{
    public interface ISolver<TSolution, TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        public TSolution Solution { get; protected set; }
        public TChange GenerateNeighbour();
        protected bool EvaluateScoreDiff(long scoreDiff);
        public void Iterate()
        {
            TChange change = GenerateNeighbour();

            long scoreDiff = Solution.TryChange(change);

            if (EvaluateScoreDiff(scoreDiff))
            {
                Solution.ApplyChange(change);
            }
        }
    }
}