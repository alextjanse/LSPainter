namespace LSPainter
{
    public struct ColorGeneratorSettings
    {
        public byte Alpha;
    }

    public class ColorGenerator
    {
        static Random random = new Random();
        static ColorGeneratorSettings defaultSettings = new ColorGeneratorSettings()
        {
            Alpha = 255 / 10,
        };

        public static Color Generate()
        {
            return Generate(defaultSettings);
        }

        public static Color Generate(ColorGeneratorSettings settings)
        {
            byte[] bytes = new byte[3];
            random.NextBytes(bytes);

            return new Color(
                bytes[0],
                bytes[1],
                bytes[2],
                settings.Alpha
            );
        }
    }
}