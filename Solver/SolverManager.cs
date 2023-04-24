namespace LSPainter.Solver
{
    public class SolverManager
    {
        public SimulatedAnnealing[] Instances { get; }

        public SolverManager(ImageHandler original, int n)
        {
            Instances = new SimulatedAnnealing[n];

            for (int i = 0; i < n; i++)
            {
                Instances[i] = new SimulatedAnnealing(original);
            }
        }

        public void Iterate()
        {
            for (int i = 0; i < Instances.Length; i++)
            {
                Instances[i].Iterate();
            }
        }
    }
}