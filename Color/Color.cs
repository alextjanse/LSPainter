using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LSPainter
{
    public struct Color : IEquatable<Color>
    {
        public static implicit operator Rgba32(Color c) => new Rgba32(c.R, c.G, c.B, c.A);
        public static bool operator ==(Color a, Color b) => a.Equals(b);
        public static bool operator !=(Color a, Color b) => !(a == b);
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

        public static Color None = new(0, 0, 0, 0);
        public static Color Black = new (0, 0, 0);
        public static Color Red = new (255, 0, 0);
        public static Color Green = new (0, 255, 0);
        public static Color Blue = new (0, 0, 255);

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

        public static (int, int, int) Diff(Color a, Color b)
        {
            return (
                Math.Abs(a.R - b.R),
                Math.Abs(a.G - b.G),
                Math.Abs(a.B - b.B)
            );
        }

        public override string ToString()
        {
            return $"rgba({R}, {G}, {B}, {A})";
        }

        public bool Equals(Color other)
        {
            return (
                R == other.R
             && G == other.G
             && B == other.B
             && A == other.A
            );
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (R, G, B, A).GetHashCode();
        }
    }
}