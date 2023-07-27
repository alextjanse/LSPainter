namespace LSPainter.LSSolver
{
    public interface ISolver<out TSolution> : IIterable
        where TSolution : ISolution
    {
        TSolution Solution { get; }
    }

    /// <summary>
    /// A solver solves the solution it is holding. The solution space
    /// is defined by TSolution, with the solution Checker TChecker
    /// giving score value TScore, and an OperationFactory making changes
    /// to the solution.
    /// </summary>
    /// <typeparam name="TSolution">The solution data structure</typeparam>
    /// <typeparam name="TScore">The solution score and parameters</typeparam>
    /// <typeparam name="TChecker">The solution scorer</typeparam>
    public class Solver<TSolution, TScore, TChecker> : ISolver<TSolution> where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {   
        public TSolution Solution { get; private set; }
        public TScore Score { get; private set; }
        public TChecker Checker { get; }
        public IEnumerable<Constraint<TSolution, TScore>> Constraints { get; }
        public ISearchAlgorithm Algorithm;
        public OperationFactory<TSolution, TScore, TChecker> OperationFactory { get; }

        private int tick = 0;

        public Solver
        (
            TSolution startSolution,
            TChecker checker,
            ISearchAlgorithm algorithm,
            OperationFactory<TSolution, TScore, TChecker> factory,
            IEnumerable<Constraint<TSolution, TScore>>? constraints = null
        )
        {
            Solution = startSolution;
            Checker = checker;
            Algorithm = algorithm;
            OperationFactory = factory;
            Constraints = constraints ?? new List<Constraint<TSolution, TScore>>();

            Score = Checker.ScoreSolution(Solution);
        }

        public void Iterate()
        {
            Operation<TSolution, TScore, TChecker> operation = OperationFactory.Generate(Solution);
            
            // The score after the operation would be applied
            TScore newScore = operation.Try(Solution, Score, Checker);

            double currentValue = GetScoreValue(Score);
            double newValue = GetScoreValue(newScore);
            double scoreDiff = newValue - currentValue;

            if (Algorithm.EvaluateScoreDiff(scoreDiff))
            {
                operation.Apply(Solution);
                Score = newScore;
            }

            // Set on an interval of 1000 iterations
            if (tick++ > 1000)
            {
                UpdateParameters();
                tick = 0;
            }
        }

        void UpdateParameters()
        {
            Algorithm.UpdateParameters();
            OperationFactory.Update();
            foreach (Constraint<TSolution, TScore> constraint in Constraints)
            {
                constraint.Update();
            }
        }

        double GetScoreValue(TScore score)
        {
            double value = score.ToDouble();

            foreach (Constraint<TSolution, TScore> constraint in Constraints)
            {
                value += constraint.ApplyPenalty(score);
            }

            return value;
        }
    }
}