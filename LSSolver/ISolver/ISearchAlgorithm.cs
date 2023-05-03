namespace LSPainter.LSSolver
{
    public interface ISearchAlgorithm
    {
        bool EvaluateScoreDiff(long scoreDiff);
        virtual void UpdateParameters()
        {
            
        }

        virtual void Iterate(ISolution solution, IChecker checker)
        {
            IChange change = solution.GenerateNeighbor();

            long scoreDiff = checker.ScoreChange(solution, change);

            if (EvaluateScoreDiff(scoreDiff))
            {
                solution.ApplyChange(change);
            }

            UpdateParameters();
        }
    }
}