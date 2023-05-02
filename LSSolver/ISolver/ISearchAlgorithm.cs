namespace LSPainter.LSSolver
{
    public interface ISearchAlgorithm<TChecker, TSolution, TChange>
        where TChecker : IChecker<TSolution, TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        bool EvaluateScoreDiff(long scoreDiff);
        virtual void UpdateParameters()
        {
            
        }

        virtual void Iterate(TSolution solution, TChecker checker)
        {
            TChange change = solution.GenerateNeighbor();

            long scoreDiff = checker.ScoreChange(solution, change);

            if (EvaluateScoreDiff(scoreDiff))
            {
                solution.ApplyChange(change);
            }

            UpdateParameters();
        }
    }
}