namespace LSPainter
{
    public class Randomizer
    {
        static Random random = new Random();
        public static float[] RandomFactors(float target, int n)
        {
            float[] output = new float[n];

            float product = 1;

            for (int i = 0; i < n; i++)
            {
                float f = random.NextSingle();
                output[i] = f;
                product *= f;
            }

            float factor = (float)Math.Pow(target / product, 1f / n);

            for (int i = 0; i < n; i++)
            {
                output[i] *= factor;
            }

            float check = 1f;

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

        public static float[] SplitRandomly(float amount, int n)
        {
            float[] output = new float[n];

            float total = 0;

            for (int i = 0; i < n; i++)
            {
                float f = random.NextSingle();
                output[i] = f;
                total += f;
            }

            float factor = 1f / total;

            for (int i = 0; i < n; i++)
            {
                output[i] *= factor * amount;
            }

            return output;
        }

        public static float FloatInRange(float lb, float ub)
        {
            return lb + random.NextSingle() * (ub - lb);
        }
    }
}