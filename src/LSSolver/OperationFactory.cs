namespace LSPainter.LSSolver
{
    /// <summary>
    /// Generate operations of a given solution.
    /// </summary>
    public abstract class OperationFactory<TSolution, TScore, TChecker> : IUpdatable where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        public abstract Operation<TSolution, TScore, TChecker> Generate(TSolution solution);
        public abstract void Update();
    }
}