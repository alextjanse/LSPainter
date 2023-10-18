namespace LSPainter.LSSolver
{
    /// <summary>
    /// A constraint on a given solution, e.g. number of shapes used.
    /// Set a penalty for violating the constraint
    /// </summary>
    public abstract class Constraint<TSolution, TScore> : IUpdatable where TSolution : Solution where TScore : Score<TSolution>
    {
        public double Penalty { get; set; }
        public double Alpha { get; set; }
        
        public Constraint(double penalty, double alpha)
        {
            Penalty = penalty;
            Alpha = alpha;
        }

        public abstract double ApplyPenalty(TScore score);
        
        public void Update()
        {
            Penalty *= Alpha;
        }
    }
}