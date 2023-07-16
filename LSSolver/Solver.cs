namespace LSPainter.LSSolver
{
    public interface IUpdatable
    {
        void Update();
    }

    public interface IIterable
    {
        void Iterate();
    }

    public abstract class Solution : ICloneable
    {
        public abstract object Clone();
    }

    public abstract class Operation<TSolution, TScore, TChecker> where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        public abstract TScore Try(TSolution solution, TScore currentScore, TChecker checker);
        public abstract void Apply(TSolution solution);
    }

    public abstract class OperationFactory<TSolution, TScore, TChecker> : IUpdatable where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        protected TChecker Checker { get; }

        public OperationFactory(TChecker checker)
        {
            Checker = checker;
        }

        public abstract Operation<TSolution, TScore, TChecker> Generate();
        public abstract void Update();
    }

    public abstract class Score<TSolution> : ICloneable where TSolution : Solution
    {
        public abstract double ToDouble();
        public abstract object Clone();
    }

    public abstract class Constraint<TSolution, TScore> : IUpdatable where TSolution : Solution where TScore : Score<TSolution>
    {
        public double Penalty { get; set; }
        public abstract double ApplyPenalty(TScore score);
        public abstract void Update();
    }

    public abstract class SolutionChecker<TSolution, TScore> where TSolution : Solution where TScore : Score<TSolution>
    {
        public abstract TScore ScoreSolution(TSolution solution);
    }

    public class Solver<TSolution, TScore, TChecker> : IIterable where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        public TSolution Solution { get; private set; }
        public TScore Score { get; private set; }
        public TChecker Checker { get; }
        public List<Constraint<TSolution, TScore>> Constraints { get; }
        public ISearchAlgorithm Algorithm;
        public OperationFactory<TSolution, TScore, TChecker> OperationFactory { get; }

        public Solver
        (
            TSolution startSolution,
            TChecker checker,
            ISearchAlgorithm algorithm,
            OperationFactory<TSolution, TScore, TChecker> factory,
            List<Constraint<TSolution, TScore>>? constraints = null
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
            Operation<TSolution, TScore, TChecker> operation = OperationFactory.Generate();
            
            TScore newScore = operation.Try(Solution, Score, Checker);

            double currentValue = GetScoreValue(Score);
            double newValue = GetScoreValue(newScore);

            double scoreDiff = newValue - currentValue;

            if (Algorithm.EvaluateScoreDiff(scoreDiff))
            {
                operation.Apply(Solution);
                Score = newScore;
            }

            UpdateParameters();
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