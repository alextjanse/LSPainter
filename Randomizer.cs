namespace LSPainter
{
    public class Randomizer
    {
        static Random random = new Random();

        public static T PickRandomly<T>((T, float)[] items)
        {
            float sum = 0;

            foreach ((_, float p) in items)
            {
                sum += p;
            }

            float x = random.NextSingle() * sum;

            int i = -1;

            sum = 0;

            do
            {
                (_, float p) = items[++i];

                sum += p;
            }
            while (sum < x);

            return items[i].Item1;
        }

        public static double[] RandomFactors(double target, int n)
        {
            double[] output = new double[n];

            double product = 1;

            for (int i = 0; i < n; i++)
            {
                double f = random.NextDouble();
                output[i] = f;
                product *= f;
            }

            double factor = Math.Pow(target / product, 1f / n);

            for (int i = 0; i < n; i++)
            {
                output[i] *= factor;
            }

            double check = 1f;

            for (int i = 0; i < n; i++)
            {
                check *= output[i];
            }

            if (!(check - 1 <= target && target <= check + 1))
            {
                Console.WriteLine("Random factors is malfunctioning! Target: {0}, Actual: {1}", target, check);
            }

            return output;
        }

        public static double[] SplitRandomly(double amount, int n)
        {
            double[] output = new double[n];

            double total = 0;

            for (int i = 0; i < n; i++)
            {
                float f = random.NextSingle();
                output[i] = f;
                total += f;
            }

            double factor = 1 / total;

            for (int i = 0; i < n; i++)
            {
                output[i] *= factor * amount;
            }

            return output;
        }

        public static double Range(double lb, double ub)
        {
            return lb + random.NextDouble() * (ub - lb);
        }

        public static bool RandomBool(double p = 0.5)
        {
            return random.NextDouble() < p;
        }
    }
}