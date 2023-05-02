namespace LSPainter.LSSolver
{
    public abstract class SimulatedAnnealingSolver<TChecker, TSolution, TChange> : ISearchAlgorithm<TChecker, TSolution, TChange>
        where TChecker : IChecker<TSolution, TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        double temperature = 1000000;
        int coolingSteps = 10000;
        int iteration = 0;
        double alpha = 0.95;

        public void UpdateParameters()
        {
            if (iteration++ > coolingSteps)
            {
                iteration = 0;
                temperature *= alpha;
            }
        }

        public bool EvaluateScoreDiff(long scoreDiff)
        {
            if (scoreDiff < 0)
            {
                return true;
            }

            float x = Randomizer.RandomChance();
            double p = Math.Pow(Math.E, -scoreDiff / temperature);

            if (x < p)
            {
                return true;
            }

            return false;
        }
    }
}