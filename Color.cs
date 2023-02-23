using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LSPainter
{
    public class Color
    {
        public Rgba32 Rgba32 { get; }
        public int ABGR { get; }
        public static implicit operator Rgba32(Color color) => color.Rgba32;
        public static implicit operator int(Color color) => color.ABGR;

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
            Rgba32 = new Rgba32(R, G, B);
            ABGR = (255 << 24) | (B << 16) | (G << 8) | (R << 0);
        }

        public Color(Rgba32 color)
        {
            Rgba32 = color;
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
            ABGR = (color.A << 24) | (color.B << 16) | (color.G << 8) | (color.R << 0);
        }

        public static Color Black = new Color(0, 0, 0);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
    }
}