namespace LSPainter.LSSolver
{
    public interface ISolution : ICloneable
    {

    }

    public interface ICanvasSolution : ISolution
    {
        Canvas Canvas { get; }
    }

    /// <summary>
    /// Stores the solution. Gets altered by the operations.
    /// </summary>
    public abstract class Solution : ISolution
    {
        public abstract object Clone();
    }

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

    /// <summary>
    /// Generate operations of a given solution.
    /// </summary>
    public abstract class OperationFactory<TSolution, TScore, TChecker> : IUpdatable where TSolution : Solution where TScore : Score<TSolution> where TChecker : SolutionChecker<TSolution, TScore>
    {
        public abstract Operation<TSolution, TScore, TChecker> Generate();
        public abstract void Update();
    }

    /// <summary>
    /// Contains the score of a solution and its parameters that can be
    /// used by Constraints.
    /// </summary>
    public abstract class Score<TSolution> : ICloneable where TSolution : Solution
    {
        /// <summary>
        /// The "raw" solution score, e.g. total pixel score, with no penalties.
        /// </summary>
        public abstract double ToDouble();
        public abstract object Clone();
    }

    public abstract class SolutionChecker<TSolution, TScore> where TSolution : Solution where TScore : Score<TSolution>
    {
        public abstract TScore ScoreSolution(TSolution solution);
    }

    /// <summary>
    /// A constraint on a given solution, e.g. number of shapes used.
    /// Set a penalty for violating the constraint
    /// </summary>
    public abstract class Constraint<TSolution, TScore> : IUpdatable where TSolution : Solution where TScore : Score<TSolution>
    {
        public double Penalty { get; set; }
        public abstract double ApplyPenalty(TScore score);
        public abstract void Update();
    }
}