namespace LSPainter
{
    public class ColorPalette : ICloneable
    {
        public IEnumerable<Color> Colors { get; private set; }

        public ColorPalette(IEnumerable<Color> colors)
        {
            Colors = colors;
        }

        public void AddColor(Color color)
        {
            Colors.Append(color);
        }

        public object Clone()
        {
            return new ColorPalette(Colors.ToArray());
        }
    }
}