namespace LSPainter.LSSolver
{
    public abstract class SolutionChecker<TSolution, TScore> where TSolution : Solution where TScore : Score<TSolution>
    {
        public abstract TScore ScoreSolution(TSolution solution);
    }
}