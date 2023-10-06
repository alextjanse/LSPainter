namespace LSPainter
{
    public class ColorGeneratorSettings : ICloneable
    {
        public double Alpha { get; set; }

        public ColorGeneratorSettings(double alpha)
        {
            Alpha = alpha;
        }

        public object Clone()
        {
            return new ColorGeneratorSettings(Alpha);
        }
    }

    public class ColorGenerator
    {
        static Random random = new Random();

        public static Color Generate(ColorGeneratorSettings settings)
        {
            byte[] bytes = new byte[4];
            random.NextBytes(bytes);

            return new Color(
                bytes[0],
                bytes[1],
                bytes[2],
                (byte)settings.Alpha
            );
        }
    }
}