/* namespace LSPainter.LSSolver
{
    public abstract class SolutionChecker<TSolution, TChange>
        where TSolution : Solution<TChange>
        where TChange : Change
    {
        public abstract long ScoreSolution(TSolution solution);
        public abstract long GetScoreDiff(TSolution solution, TChange change);
    }
} */