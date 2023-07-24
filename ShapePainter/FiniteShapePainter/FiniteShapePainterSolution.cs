using LSPainter.LSSolver.Painter;
using LSPainter.Maths.Shapes;

namespace LSPainter.ShapePainter.FiniteShapePainter
{
    public struct ShapeObject
    {
        public Shape Shape { get; set; }
        public Color Color { get; set; }
    }

    public class FiniteShapePainterSolution : CanvasSolution
    {
        public List<ShapeObject> Shapes;

        public FiniteShapePainterSolution(Canvas canvas) : base(canvas)
        {
            Shapes = new List<ShapeObject>();
        }

        public FiniteShapePainterSolution(Canvas canvas, IEnumerable<ShapeObject> shapes) : base(canvas)
        {
            Shapes = shapes.ToList();
        }

        public override object Clone()
        {
            return new FiniteShapePainterSolution((Canvas)Canvas.Clone(), Shapes);
        }
    }
}