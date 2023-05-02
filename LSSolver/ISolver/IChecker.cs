namespace LSPainter.LSSolver
{
    public interface IChecker<in TSolution, in TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        long ScoreSolution(TSolution solution);
        long ScoreChange(TSolution solution, TChange change);
    }
}