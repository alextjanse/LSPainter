using LSPainter.LSSolver.Painter;
using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterSolution : CanvasSolution
    {
        public List<(Shape, Color)> Shapes;
        public int NumberOfShapes => Shapes.Count;

        public FiniteShapePainterSolution(Canvas canvas) : base(canvas)
        {
            Shapes = new List<(Shape, Color)>();
        }

        public FiniteShapePainterSolution(Canvas canvas, IEnumerable<(Shape, Color)> shapes) : base(canvas)
        {
            Shapes = shapes.ToList();
        }

        public override object Clone()
        {
            return new FiniteShapePainterSolution((Canvas)Canvas.Clone(), Shapes);
        }

        public void InsertShape((Shape, Color) obj, int index)
        {
            Shapes.Insert(index, obj);
        }

        public void RemoveAt(int index)
        {
            Shapes.RemoveAt(index);
        }

        public void DrawSection(Rectangle section)
        {
            foreach ((int x, int y) in section.PixelCoords())
            {
                Canvas.SetPixel(x, y, default);
            }

            foreach ((Shape shape, Color color) in Shapes)
            {
                Rectangle intersection = Rectangle.Intersect(shape.BoundingBox, section);
                foreach ((int x, int y) in intersection.PixelCoords())
                {
                    if (shape.IsInside(new Vector(x + 0.5, y + 0.5)))
                    {
                        Canvas.PaintPixel(x, y, color);
                    }
                }
            }
        }
    }
}