namespace LSPainter.LSSolver
{
    public interface ISearchAlgorithm
    {
        bool EvaluateScoreDiff(double scoreDiff);
        virtual void UpdateParameters()
        {
            
        }
    }
}