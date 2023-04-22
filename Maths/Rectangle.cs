namespace LSPainter.Maths
{
    public struct Rectangle
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }
        
        public static implicit operator BoundingBox(Rectangle r) => new BoundingBox(
                                                                        (int)Math.Floor(r.X),
                                                                        (int)Math.Floor(r.Y),
                                                                        (int)Math.Ceiling(r.Width),
                                                                        (int)Math.Ceiling(r.Height)
                                                                    );

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}