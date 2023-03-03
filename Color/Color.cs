using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LSPainter
{
    public struct Color
    {
        public static implicit operator Rgba32(Color c) => new Rgba32(c.R, c.G, c.B, c.A);
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(Rgba32 color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public static Color Black = new Color(0, 0, 0);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);

        /// <summary>
        /// Blend the two colors using alpha compositing: https://en.wikipedia.org/wiki/Alpha_compositing
        /// </summary>
        /// <param name="backdrop">Background color</param>
        /// <param name="source">New color</param>
        /// <returns></returns>
        public static Color Blend(Color backdrop, Color source)
        {
            float factor = 1f / 255f;

            float backAlpha = backdrop.A * factor;
            float sourceAlpha = source.A * factor;

            Func<byte, byte, byte> colorBlend = (cB, cS) => (byte)(sourceAlpha * cS + backAlpha * cB * (1 - sourceAlpha));

            byte r = colorBlend(backdrop.R, source.R);
            byte g = colorBlend(backdrop.G, source.G);
            byte b = colorBlend(backdrop.B, source.B);
            byte a = (byte)((sourceAlpha + backAlpha * (1 - sourceAlpha)) * 255);

            return new Color(r, g, b, a);
        }

        public static int Diff(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }

        public override string ToString()
        {
            return $"rgba({R}, {G}, {B}, {A})";
        }
    }
}