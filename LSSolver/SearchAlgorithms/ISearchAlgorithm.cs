namespace LSPainter.LSSolver
{
    public interface ISearchAlgorithm
    {
        bool EvaluateScoreDiff(long scoreDiff);
        void Update();
    }
}