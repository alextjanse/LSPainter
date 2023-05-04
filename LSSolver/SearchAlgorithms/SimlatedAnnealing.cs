namespace LSPainter.LSSolver
{
    public class SimulatedAnnealingSolver : ISearchAlgorithm
    {
        double temperature = 1000;
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