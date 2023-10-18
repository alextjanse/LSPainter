namespace LSPainter.LSSolver
{
    /// <summary>
    /// An instance of an operator, e.g. draw shape. Calculates
    /// the score in case the operation is applied, and handles
    /// the application as well.
    /// </summary>
    public abstract class Operation<TSolution, TScore, TChecker> where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        public abstract TScore Try(TSolution solution, TScore currentScore, TChecker checker);
        public abstract void Apply(TSolution solution);
    }
}