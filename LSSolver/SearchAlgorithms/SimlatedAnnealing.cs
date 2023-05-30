namespace LSPainter.LSSolver
{
    public class SimulatedAnnealingSolver : ISearchAlgorithm
    {
        double temperature = 1000;
        int coolingSteps = 10000;
        int iteration = 0;
        double alpha = 0.95;

        public void Update()
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

            if (Randomizer.RandomBool(Math.Pow(Math.E, -scoreDiff / temperature)))
            {
                return true;
            }

            return false;
        }
    }
}