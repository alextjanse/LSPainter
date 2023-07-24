using LSPainter.LSSolver.Painter;
using LSPainter.Maths;

namespace LSPainter.ShapePainter.FiniteShapePainter
{
    public abstract class FiniteShapePainterOperation : CanvasOperation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>
    {
        protected FiniteShapePainterOperation(Rectangle bbox) : base(bbox)
        {
        }

        public abstract override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker);

        public abstract override void Apply(FiniteShapePainterSolution solution);
    }

    public class InsertNewShapeOperation : FiniteShapePainterOperation
    {
        public ShapeObject ShapeObject { get; }
        public int atIndex { get; }

        public InsertNewShapeOperation(ShapeObject shapeObject, int atIndex) : base(shapeObject.Shape.BoundingBox)
        {
            ShapeObject = shapeObject;
            this.atIndex = atIndex;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            throw new NotImplementedException();
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            throw new NotImplementedException();
        }
    }
}