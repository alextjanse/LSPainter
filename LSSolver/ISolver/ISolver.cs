namespace LSPainter.LSSolver
{
    public interface ISolver<TSearcher, TChecker, TSolution, TChange>
        where TSearcher : ISearchAlgorithm<TChecker, TSolution, TChange>
        where TChecker : IChecker<TSolution, TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        TSearcher Searcher { get; }
        TChecker Checker { get; }
        TSolution Solution { get; }

        void Iterate()
        {
            Searcher.Iterate(Solution, Checker);
        }
    }
}