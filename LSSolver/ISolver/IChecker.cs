namespace LSPainter.LSSolver
{
    public interface IChecker
    {
        long ScoreSolution(ISolution solution);
        long ScoreChange(ISolution solution, IChange change);
    }
}