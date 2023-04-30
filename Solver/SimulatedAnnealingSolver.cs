namespace LSPainter.Solver
{
    public abstract class SimulatedAnnealingSolver<TSolutionChecker, TSolution, TChange> : 
        Solver<TSolutionChecker, TSolution, TChange>
        where TSolutionChecker : SolutionChecker<TSolution, TChange>
        where TSolution : Solution<TChange>
        where TChange : Change
    {
        double temperature = 1000000;
        int coolingSteps = 100000;
        int iteration = 0;
        double alpha = 0.95;

        protected override bool EvaluateScoreDiff(long scoreDiff)
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
                    /* 
                    Apply the change with a probablity
                    Source: https://en.wikipedia.org/wiki/Simulated_annealing#Acceptance_probabilities_2
                     */
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