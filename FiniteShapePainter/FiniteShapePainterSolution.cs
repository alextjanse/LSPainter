using LSPainter.LSSolver.Painter;
using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter
{
    public class FiniteShapePainterSolution : CanvasSolution
    {
        public List<(Shape shape, Color color)> Shapes;
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

        public void InsertShape(Shape shape, Color color, int index)
        {
            Shapes.Insert(index, (shape, color));
        }

        public void RemoveAt(int index)
        {
            Shapes.RemoveAt(index);
        }

        public void DrawSection(Rectangle section)
        {
            IEnumerable<(Shape, Color)> intersectingShapes = Shapes.Where(item => item.shape.BoundingBox.Overlaps(section));

            foreach ((int x, int y) in section.PixelCoords())
            {
                Color pixelColor = default;
                Vector pixelCoord = new Vector(x + 0.5, y + 0.5);

                foreach ((Shape shape, Color color) in intersectingShapes)
                {
                    if (shape.IsInside(pixelCoord))
                    {
                        pixelColor = Color.Blend(pixelColor, color);
                    }    
                }

                Canvas.SetPixel(x, y, pixelColor);
            }
        }
    }
}