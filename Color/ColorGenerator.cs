namespace LSPainter
{
    public struct ColorGeneratorSettings
    {
        public byte Alpha;
    }

    public class ColorGenerator
    {
        static Random random = new Random();

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