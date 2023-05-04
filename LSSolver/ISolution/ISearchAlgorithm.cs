namespace LSPainter.LSSolver
{
    public interface ISearchAlgorithm
    {
        bool EvaluateScoreDiff(long scoreDiff);
        virtual void UpdateParameters()
        {
            
        }

        virtual void Iterate(ISolution solution)
        {
            IChange change = solution.GenerateNeighbor();

            long scoreDiff = solution.TryChange(change);

            if (EvaluateScoreDiff(scoreDiff))
            {
                solution.ApplyChange(change);
            }

            UpdateParameters();
        }
    }
}