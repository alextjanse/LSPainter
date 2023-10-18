namespace LSPainter.LSSolver
{
    public interface IParameter<T>
    {
        T PickValue();
    }

    public class SingleParameter<T> : IParameter<T>
    {
        public T Value { get; set; }

        public SingleParameter(T value)
        {
            Value = value;
        }

        public T PickValue()
        {
            return Value;
        }
    }

    public class RangeParameter : IParameter<double>
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public RangeParameter(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double PickValue()
        {
            return MinValue + Randomizer.RandomDouble() * (MaxValue - MinValue);
        }
    }
    
    public class SelectionParameter<T> : IParameter<T>
    {
        private (T item, double chance)[] options;

        public SelectionParameter(T[] options, double[]? chances = null)
        {
            if (chances is null)
            {
                chances = new double[options.Length];
                Array.Fill(chances, 1f / options.Length);
            }

            this.options = options.Zip(chances).ToArray();
        }

        public SelectionParameter(IEnumerable<(T, double)> options)
        {
            this.options = options.ToArray();
        }

        public T PickValue()
        {
            double p = Randomizer.RandomDouble() * options.Sum(option => option.chance);

            double sum = 0;

            int i = 0;

            while (sum < p && i < options.Length)
            {
                sum += options[i++].chance;
            }

            return options[i - 1].item;
        }
    }

    public class RandomValueParameter : IParameter<double>
    {
        public double Mean { get; set; }
        public double SD { get; set; }
        public double Lowerbound { get; set; }
        public double Upperbound { get; set; }

        public RandomValueParameter(double mean, double sd, double lowerbound = double.NegativeInfinity, double upperbound = double.PositiveInfinity)
        {
            Mean = mean;
            SD = sd;
            Lowerbound = lowerbound;
            Upperbound = upperbound;
        }

        public double PickValue()
        {
            double x = Randomizer.RandomDoubleND(Mean, SD);

            return Math.Max(Lowerbound, Math.Min(Upperbound, x));
        }
    }
}