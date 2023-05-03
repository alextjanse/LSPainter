namespace LSPainter.LSSolver
{
    public interface ISolver
    {
        ISearchAlgorithm SearchAlgorithm { get; }
        IChecker Checker { get; }
        ISolution Solution { get; }

        virtual void Iterate()
        {
            SearchAlgorithm.Iterate(Solution, Checker);
        }
    }
}