namespace LSPainter
{
    public class Randomizer
    {
        static Random random = new Random();
        public static bool RandomBool(double p = 0.5) => random.NextDouble() < p;
        public static double RandomDouble(double lb = 0, double ub = 1) => lb + random.NextDouble() * (ub - lb);
        public static float RandomFloat(float lb = 0, float ub = 1) => lb + random.NextSingle() * (ub - lb);

        public static T[] PickMultiple<T>(T[] items, int n)
        {
            // For now, only pick multiple items uniformly

            int nItems = items.Length;

            if (nItems > n) throw new Exception("Too few items");

            int[] offSets = Randomizer.Split(nItems - n, n).Select(d => (int)d).ToArray();

            T[] output = new T[n];
            int index = 0;
            for (int i = 0; i < n; i++)
            {
                index += offSets[i];
                output[i] = items[index];
            }

            return output;
        }

        public static T Pick<T>(IEnumerable<T> items)
        {
            return Pick(items.Zip(Enumerable.Repeat(1f, items.Count())));
        }

        public static T Pick<T>(IEnumerable<(T, float)> items)
        {
            float sum = 0;

            foreach ((_, float p) in items)
            {
                sum += p;
            }

            float x = random.NextSingle() * sum;

            sum = 0;

            foreach ((T t, float p) in items)
            {
                sum += p;

                if (sum >= x)
                {
                    return t;
                }
            }

            return items.Last().Item1;
        }

        public static double[] Factorise(double target, int n)
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

        public static double[] Split(double amount, int n)
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

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
        {
            T[] array = items.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                int j = random.Next(array.Length);

                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return array;
        }
    }
}