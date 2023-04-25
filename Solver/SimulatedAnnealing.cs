namespace LSPainter.Solver
{
    public abstract class SimulatedAnnealingSolver<TSolution, TChange> : ISolver<TSolution, TChange>
        where TSolution : ISolution<TChange>
        where TChange : IChange
    {
        double temperature = 1000000;
        int coolingSteps = 100000;
        int iteration = 0;
        double alpha = 0.95;

        protected bool EvaluateScoreDiff(long scoreDiff)
        {
            if (scoreDiff < 0)
            {
                UpdateParameters();
                return true;
            }
            else
            {
                float x = Randomizer.RandomChance();
                double p = Math.Pow(Math.E, -scoreDiff / temperature);

                if (x < p)
                {
                    // Apply the change with probablity: https://en.wikipedia.org/wiki/Simulated_annealing#Acceptance_probabilities_2
                    UpdateParameters();
                    return true;
                }
            }

            UpdateParameters();
            return false;
        }

        private void UpdateParameters()
        {
            if (iteration++ > coolingSteps)
            {
                iteration = 0;
                temperature *= alpha;
            }
        }
    }
}