namespace LSPainter.LSSolver
{
    public interface ISolution
    {
        IScore Score { get; }
    }

    public interface IScore
    {
        double GetValue();
    }
    public interface IOperation<TSolution>
        where TSolution : ISolution
    {
        void Apply(TSolution solution);
        double Try(TSolution solution);
    }

    public interface IOperationFactory<TSolution> where TSolution : ISolution
    {
        IOperation<TSolution, IScore, ISolutionComparer<TSolution>> Generate(TSolution solution);
    }

    public interface IConstraint<TScore> where TScore : IScore
    {
        double GetPenalty(IScore score);
    }

    public interface ISolutionComparer<TSolution> where TSolution : ISolution
    {
        
    }

    public abstract class Solver<TSolution> where TSolution : ISolution
    {
        public TSolution Solution { get; }
        public IScore Score { get; }
        public List<IConstraint<IScore>> Constraints { get; }
        ISearchAlgorithm SearchAlgorithm { get; }
        ISolutionComparer<TSolution> Comparer { get; }
        IOperationFactory<TSolution> OperationFactory { get; }

        public Solver(TSolution initialSolution, ISolutionComparer<TSolution> comparer, IOperationFactory<TSolution> factory, ISearchAlgorithm algorithm, List<IConstraint<IScore>>? constraints = null)
        {
            Solution = initialSolution;
            Constraints = constraints ?? new List<IConstraint<IScore>>();
            Comparer = comparer;
            OperationFactory = factory;
            SearchAlgorithm = algorithm;

            
        }

        public void Iterate()
        {
            // TODO: make operation, calculate its score diff
            IOperation<TSolution> operation = OperationFactory.Generate(Solution);

            

            SearchAlgorithm.UpdateParameters();
        }
    }
}