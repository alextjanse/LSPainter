using LSPainter.Maths;
using OpenTK.Graphics.ES11;

namespace LSPainter
{
    public class ColorGeneratorSettings : ICloneable
    {
        public IParameter<double> Alpha { get; set; }

        public ColorGeneratorSettings(IParameter<double> alpha)
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
        public ColorGeneratorSettings Settings { get; set; }

        public ColorGenerator(ColorGeneratorSettings settings)
        {
            Settings = settings;
        }

        public Color Generate()
        {
            byte[] bytes = new byte[4];
            Randomizer.Random.NextBytes(bytes);

            return new Color(
                bytes[0],
                bytes[1],
                bytes[2],
                (byte)Settings.Alpha.PickValue()
            );
        }
    }
}