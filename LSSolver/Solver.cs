namespace LSPainter.LSSolver
{
    public interface ISolution
    {
        long Score { get; }
        IChange GenerateNeighbor();
        long TryChange(IChange change);
        void ApplyChange(IChange change);
    }

    public interface IChange
    {

    }

    public abstract class Solver<TSolution> where TSolution : ISolution
    {
        public TSolution Solution { get; }
        ISearchAlgorithm SearchAlgorithm { get; }

        public Solver(ISearchAlgorithm algorithm, TSolution initialSolution)
        {
            SearchAlgorithm = algorithm;
            Solution = initialSolution;
        }

        public void Iterate()
        {
            SearchAlgorithm.Iterate(Solution);
        }
    }
}