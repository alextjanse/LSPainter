namespace LSPainter.Maths
{
    public struct Rectangle
    {
        public float X, Y, Width, Height;
        public static implicit operator BoundingBox(Rectangle r) => new BoundingBox(
                                                                        (int)Math.Floor(r.X),
                                                                        (int)Math.Floor(r.Y),
                                                                        (int)Math.Ceiling(r.Width),
                                                                        (int)Math.Ceiling(r.Height)
                                                                    );

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}