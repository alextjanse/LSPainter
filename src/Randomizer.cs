using System.Reflection.Metadata;
using LSPainter.Maths;

namespace LSPainter
{
    public class Randomizer
    {
        public static Random Random = new Random();

        public static T PickRandomly<T>((T, float)[] items)
        {
            float sum = 0;

            foreach ((_, float p) in items)
            {
                sum += p;
            }

            float x = Random.NextSingle() * sum;

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

        public static T PickRandomly<T>(IEnumerable<T> items)
        {
            int length = items.Count();

            int index = RandomInt(length);

            return items.ElementAt(index);
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
        {
            List<T> list = items.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Next(list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }

        public static double[] RandomFactors(double target, int n)
        {
            double[] output = new double[n];

            double product = 1;

            for (int i = 0; i < n; i++)
            {
                double f = Random.NextDouble();
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
                float f = Random.NextSingle();
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

        public static double RandomDouble() => Random.NextDouble();

        public static double RandomDouble(double lb, double ub) => lb + Random.NextDouble() * (ub - lb);

        public static double RandomDoubleND(double mean = 0, double sd = 1)
        {
            double u1 = RandomDouble();
            double u2 = RandomDouble();

            // Box-Muller transform - https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
            double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            return mean + z0 * sd;
        }

        public static double RandomAngle(double maxAngle = 2 * Math.PI) => Random.NextDouble() * maxAngle;

        public static bool RandomBool(double p = 0.5)
        {
            return Random.NextDouble() < p;
        }

        public static int RandomInt() => Random.Next();
        public static int RandomInt(int maxValue) => Random.Next(maxValue);
        public static int RandomInt(int minValue, int maxValue) => Random.Next(minValue, maxValue);
    }
}