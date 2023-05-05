namespace LSPainter.LSSolver
{
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