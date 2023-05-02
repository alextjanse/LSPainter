/* namespace LSPainter.LSSolver
{
    public interface ISolver
    {
        void Iterate();
    }

    public abstract class Solver<TSolutionChecker, TSolution, TChange> : ISolver
        where TSolutionChecker : SolutionChecker<TSolution, TChange>
        where TSolution : Solution<TChange>
        where TChange : Change
    {
        public abstract TSolution Solution { get; }
        public abstract TSolutionChecker SolutionChecker { get; }
        protected abstract TChange GenerateNeighbour();
        protected abstract long TryChange(TChange change);
        protected abstract void ApplyChange(TChange change);
        protected abstract bool EvaluateScoreDiff(long scoreDiff);

        public void Iterate()
        {
            TChange change = GenerateNeighbour();

            long scoreDiff = TryChange(change);

            if (EvaluateScoreDiff(scoreDiff))
            {
                ApplyChange(change);
            }
        }
    }
} */