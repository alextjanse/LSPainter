namespace LSPainter.LSSolver
{
    public interface ISolution
    {
        IChange<ISolution> GenerateNeighbor();
        void Update();
    }

    public interface IChange<TSolution> where TSolution : ISolution
    {
        IScore<TSolution> Try(TSolution solution, ISolutionChecker<TSolution> comparer);
        void Apply(TSolution solution);
    }

    public interface ISolutionChecker<TSolution> where TSolution : ISolution
    {
        IScore<TSolution> CalculateScore(TSolution solution);
    }

    public interface IScore<TSolution> where TSolution : ISolution
    {
        long GetCleanScore();
        IScore<TSolution> Add(IScore<TSolution> score);
    }

    public interface IConstraints<TSolution> where TSolution : ISolution
    {
        long EvaluateScore(IScore<TSolution> score);
        void Update();
    }

    public abstract class Solver<TSolution> where TSolution : ISolution
    {
        public TSolution Solution { get; }
        public IScore<TSolution> Score { get; private set; }
        public ISolutionChecker<TSolution> SolutionChecker { get; }
        public IConstraints<TSolution> Constraints { get; }
        ISearchAlgorithm SearchAlgorithm { get; }

        public Solver(
            ISearchAlgorithm algorithm,
            TSolution initialSolution,
            IConstraints<TSolution> constraints,
            ISolutionChecker<TSolution> solutionChecker)
        {
            SearchAlgorithm = algorithm;
            Solution = initialSolution;
            Constraints = constraints;
            SolutionChecker = solutionChecker;

            Score = solutionChecker.CalculateScore(initialSolution);
        }

        public void Iterate()
        {
            IChange<TSolution> change = (IChange<TSolution>)Solution.GenerateNeighbor();

            IScore<TSolution> scoreDiff = change.Try(Solution, SolutionChecker);
            IScore<TSolution> newScore = Score.Add(scoreDiff);

            long curScoreEval = Constraints.EvaluateScore(Score);
            long newScoreEval = Constraints.EvaluateScore(newScore);

            if (SearchAlgorithm.EvaluateScoreDiff(newScoreEval - curScoreEval))
            {
                change.Apply(Solution);
                Score = newScore;
            }

            Solution.Update();
            SearchAlgorithm.Update();
            Constraints.Update();
        }
    }
}